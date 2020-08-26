using DatabaseLib;
using DataService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using System.Xml;
using Log;
using System.Data;
using System.Windows;

namespace DemoDriver
{
    public class DAService : IDataServer
    {
        //const int PORT = 6543;


        const char SPLITCHAR = '.';
        //const string SERVICELOGSOURCE = "DataProcess";
        //const string SERVICELOGNAME = "DataProcess";
        //const string PATH = @"C:\DataConfig\";


        static string PATH = System.Environment.CurrentDirectory;
        const string FILENAME = "Command.xml";//用来做命定的字节保存，这样修改字节内容时，不需要改变程序
        string curActiveGrpName;

        //可配置参数，从XML文件读取
        //int DELAY = 3000;
        /// <summary>
        ///单次归档能力 1万条记录
        /// </summary>
       // int MAXHDACAP = 10000;
        int MAXHDACAP = 10;//考虑到内部变量不是太多，10个就存储一次
        int ALARMLIMIT = 1000;
        int CYCLE = 60000;//1分钟
        int CYCLE2 = 600000;//10分钟
        int SENDTIMEOUT = 60000;//1分钟

        int HDALEN = 1024 * 1024;
        int MAXLOGSIZE = 1024;
        int HDADELAY = 3600 * 1000;
        int ALARMDELAY = 3600 * 1000;
        //int ARCHIVEINTERVAL = 100;//归档周期最快为 100ms
        int ARCHIVEINTERVAL = 500;//归档周期最快为 50ms

        private System.Timers.Timer timer1 = new System.Timers.Timer();
        private System.Timers.Timer timer3 = new System.Timers.Timer();
        private DateTime _hdastart = DateTime.Now;//要归档数据的 开始时间

        private bool _bNetStatus = false;
        /// <summary>
        /// 网络状态
        /// </summary>
        public bool NetStatus
        {
            get { return _bNetStatus; }
        }

        public ITag this[short id]
        {
            get
            {
                int index = GetItemProperties(id);//通过ID  得到在 _list<TagMetaData> 的索引号，从索引号得到 Tag name名字
                if (index >= 0)
                {
                    return this[_list[index].Name];
                }
                return null;
            }
        }

        //完成从_list<TagMetaData>  ===>   _mapping<string,ITag>里面通过 名字找出 Itag
        public ITag this[string name]
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(name)) return null;
                    ITag dataItem;
                    _mapping.TryGetValue(name.ToUpper(), out dataItem);
                    return dataItem;
                }
                catch (Exception ex)
                {
                    Log4Net.AddLog(ex.StackTrace, InfoLevel.ERROR);
                    return null;
                }
            }
        }

        /// <summary>
        /// 这个_list 便是 TagMetaData
        /// </summary>
        List<TagMetaData> _list;
        public IList<TagMetaData> MetaDataList
        {
            get
            {
                return _list;
            }
        }

        /// <summary>
        /// 是否有历史数据
        /// </summary>
        bool _hasHda = false;
        List<HistoryData> _hda;//归档数据暂存于此
        Dictionary<short, ArchiveTime> _archiveTimes = new Dictionary<short, ArchiveTime>();//所有归档的记录，建立一个词典

        /// <summary>
        ///通过名字存储 Itag的字典
        /// </summary>
        Dictionary<string, ITag> _mapping;

        IDriver _driver;
        IDriver _camDriver; // add by lyn,2020.8.21摄像头通信


        private object _myLock = new object();
        Dictionary<short, string> _archiveList = null;//是否需要lock
        public Dictionary<short, string> ArchiveList
        {
            get
            {
                lock (_myLock)
                {
                    if (_archiveList == null)
                    {
                        var list = MetaDataList.Where(x => x.Archive).Select(y => y.ID);//&& x.DataType != DataType.BOOL
                        if (list != null && list.Count() > 0)
                        {
                            string sql = "SELECT TAGID,DESCRIPTION FROM Protocol WHERE TAGID IN(" + string.Join(",", list) + ");";
                            using (var reader = DataHelper.Instance.ExecuteReader(sql))
                            {
                                if (reader != null)
                                {
                                    _archiveList = new Dictionary<short, string>();
                                    while (reader.Read())
                                    {
                                        _archiveList.Add(reader.GetInt16(0), reader.GetNullableString(1));
                                    }
                                }
                            }
                        }
                    }
                }
                return _archiveList;
            }
        }

        public ExpressionEval Eval { get { throw new NotImplementedException(); } }

        public IList<Scaling> ScalingList { get { throw new NotImplementedException(); } }

        public IEnumerable<IDriver> Drivers { get { throw new NotImplementedException(); } }

        public GloConfig GloConfig { get; set; }

        object _syncRoot;
        public object SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    /*
                     * 比较第一个参数和最后一个参数是否相等，相等则用第二个参数替换第一个，并返回第一个参数的最新值。
                     * 这是在多线程中，防止 if 进去之后，_synRoot值被其他线程置为非null，所以用 Interlocked.CompareExchange
                     */
                    Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        public DAService()
        {

            InitServerByXml();

            _driver = new SecondFloorPLCDriver(this, 0, "SecondFloor");//中间参数ID,应为驱动ID 号
            // add by lyn,2020.8.21摄像头通信
            //_camDriver = new CameraDriver(this, 0, "Camera");
            _hda = new List<HistoryData>();
            //test
            //InitServerByDatabase();
            InitServerBySqlite();
            InitConnection();//创建驱动实例，并尝试建立连接
            DeleteHdaFromLog();//删除3天之前的历史数据

            timer1.Elapsed += timer1_Elapsed;
            timer3.Elapsed += timer3_Elapsed;
            timer3.Interval = ARCHIVEINTERVAL;//100ms
            timer3.Enabled = true;
            timer3.Start();

            timer1.Interval = CYCLE;
            timer1.Enabled = true;
            timer1.Start();
            //if (_hasHda)//用来负责 归档数据的
            //{
            //    foreach (var item in _archiveTimes.Values)
            //    {
            //        //说白了，只有归档周期不为零，才能开启存储数据的 时钟周期函数
            //        if (item != null)
            //        {
            //            timer3.Interval = ARCHIVEINTERVAL;//100ms
            //            timer3.Enabled = true;
            //            timer3.Start();
            //            return;//对的，这个return  代表归档列表里面有个 Cycle不为0的值，便返回，那么归档的周期也是共用一个归档周期，不是每个变量都有一个周期哈
            //        }
            //    }
            //}

        }

        private void DeleteHdaFromLog()
        {
            DateTime now = DateTime.Now.AddDays(-30);
            string sql = string.Format("DELETE FROM Log WHERE [TIMESTAMP]<'{0}'", now.ToString("yyyy-MM-dd HH:mm:ss"));
            DataHelper.Instance.ExecuteNonQuery(sql);
        }
        // add by lyn,2020.8.21,摄像头通信
        public void CamSendBytes(byte[] bytes)
        {
            _camDriver.SendBytes(bytes);
        }
        public void SendBytes(byte[] bytes)
        {
            _driver.SendBytes(bytes);
        }

        public void SendDataToIPAndPort(byte[] buffer, string ip, int port)
        {
            _driver.SendDataToIPAndPort(buffer, ip, port);
        }

        public void Dispose()
        {
            lock (this)
            {
                try
                {
                    if (timer1 != null)
                        timer1.Dispose();
                    //if (timer2 != null)
                    //    timer2.Dispose();
                    if (timer3 != null)
                        timer3.Dispose();
                    if (_driver != null)
                    {

                        _driver.OnError -= this.reader_OnClose;
                        _driver.Dispose();


                        if (_hasHda)
                        {
                            Flush();
                            //hda.Clear();
                        }
                        _mapping.Clear();
                    }
                }
                catch (Exception e)
                {
                    AddErrorLog(e);
                }
            }
        }

        public void AddErrorLog(Exception e)
        {
            Log4Net.AddLog(e.GetExceptionMsg(), InfoLevel.ERROR);
        }

        /// <summary>
        /// 这里负责对各个驱动进行连接，一分钟检查一次是否断开，也需要加入对线程的异常情况检测。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (_driver.IsClosed)
            {
                _bNetStatus = _driver.Connect();
            }
        }

        /// <summary>
        /// 100ms周期性执行,用来把内存中的数据归档到数据库中，1次10000条记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Elapsed(object sender, ElapsedEventArgs e)
        {
            SaveTick++;
            var now = e.SignalTime;
            List<HistoryData> tempData = new List<HistoryData>();
            foreach (var archive in _archiveTimes)
            {
                var archiveTime = archive.Value;//归档列表里面 一个 id,一个归档的时间 ArchiveTime 两个属性；   public int Cycle; public DateTime LastTime;
                if (archiveTime != null && (now - archiveTime.LastTime).TotalMilliseconds > archiveTime.Cycle)//当前时间 减去上次更新时间 已经大于 更新周期时间 Cycle，而且只有归档周期大于0的数据才会归档 
                {
                    var tag = this[archive.Key];//取出当前 Itag,并且当前内存中的 Itag值 的时间已经更新了，于是加入归档列表，说明数据有更新
                    if (tag != null && tag.TimeStamp > archiveTime.LastTime)
                    {
                        tempData.Add(new HistoryData(tag.ID, tag.Quality, tag.Value, now));
                        archive.Value.LastTime = now;
                    }
                }
            }
            if (tempData.Count > 0)//归档列表里面有多余的数据，于是调用新的线程去更新
            {
                ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.OnUpdate), tempData);
            }
            //var result = from item in _archiveTimes where item.Value.Cycle > 0 && (now - item.Value.LastTime).Milliseconds > item.Value.Cycle select item.Key;
        }

        #region 初始化（标签数据服务器）
        /// <summary>
        /// 这个部分就是 读取的各个驱动进行 连接，和绑定相关处理事件
        /// </summary>
        void InitConnection()
        {
            _driver.OnError += new IOErrorEventHandler(reader_OnClose);
            if (_driver.IsClosed)
            {
                _bNetStatus = _driver.Connect();
            }

            foreach (IGroup grp in _driver.Groups)//驱动中每个组别 的数据变化，都绑定一个
            {
                grp.DataChange += new DataChangeEventHandler(grp_DataChange);//暂时不存储变化的变量
                                                                             // 可在此加入判断，如为ClientDriver发出，则变化数据毋须广播，只需归档。
                grp.IsActive = grp.IsActive;//该处是未来启用 组内部的时钟
            }
        }


        public void ActiveGroupByGrpName(string grpName)
        {

            foreach (IGroup grp in _driver.Groups)//驱动中每个组别 的数据变化，都绑定一个
            {
                if (grp.Name == curActiveGrpName)//关闭上一个激活的变量组
                {
                    grp.IsActive = false;
                }

                if (grp.Name == grpName)
                {
                    grp.IsActive = grp.IsActive;//该处是未来启用 组内部的时钟
                }
            }

            curActiveGrpName = grpName;
        }

        void InitServerByDatabase()
        {
            using (var dataReader = DataHelper.Instance.ExecuteProcedureReader("InitServer"))
            {
                if (dataReader == null) return;// Stopwatch sw = Stopwatch.StartNew();

                dataReader.Read();
                int count = dataReader.GetInt32(0);
                _list = new List<TagMetaData>(count);//把所有激活的 数据库记录 都导入 _list<TagMetaData> 里面
                _mapping = new Dictionary<string, ITag>(count);
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    var meta = new TagMetaData(dataReader.GetInt16(0), dataReader.GetInt16(1), dataReader.GetString(2), dataReader.GetString(3), dataReader.GetString(10), (DataType)dataReader.GetByte(4),
                     (ushort)dataReader.GetInt16(5), dataReader.GetBoolean(6), dataReader.GetFloat(7), dataReader.GetFloat(8), dataReader.GetInt32(9));

                    _list.Add(meta);
                    if (meta.Archive)
                    {
                        //Id 和Cycle 其实就是归档的时间，归档周期没有设定（0），则置为 null
                        _archiveTimes.Add(meta.ID, meta.Cycle == 0 ? null : new ArchiveTime(meta.Cycle, DateTime.MinValue));//归档的ID,归档的时间
                    }
                }
                _list.Sort();//只是id 排序就好了
                dataReader.NextResult();
                while (dataReader.Read())
                {
                    if (_driver != null)
                    {
                        IGroup grp = _driver.AddGroup(dataReader.GetString(1), dataReader.GetInt16(2), dataReader.GetInt32(3),
                               dataReader.GetFloat(4), dataReader.GetBoolean(5));
                        if (grp != null)
                            grp.AddItems(_list);//这里完成 从 _list<TagMetaData> 到 _list<ITag>的转换  ，这个地方是需要根据 GroupID 的不同，生成不同的组别，对应于不同的页面??
                    }
                }
            }
            //待归档的列表里面  有 超过一个的 变量,就开启归档
            if (_archiveTimes.Count > 0)
            {
                _hasHda = true;
                _hda.Capacity = MAXHDACAP;//单次归档的最大数据 1 万条；
            }

        }

        void InitServerBySqlite()
        {
            string sql = "select * from Protocol";
            List<Protocol> protocolList = DataHelper.Instance.ExecuteList<Protocol>(sql);
            if (protocolList.Count == 0)
            {
                MessageBox.Show("数据库读取失败!");
                return;
            }
            int count = protocolList.Count();
            _list = new List<TagMetaData>(count);//把所有激活的 数据库记录 都导入 _list<TagMetaData> 里面
            _mapping = new Dictionary<string, ITag>(count);
            foreach (Protocol protocol in protocolList)
            {
                var meta = new TagMetaData((short)protocol.TagID, (short)protocol.GroupID, protocol.TagName, protocol.Address, protocol.Description, (DataType)protocol.DataType,
                     (ushort)protocol.DataSize, protocol.Archive == 1 ? true : false, (float)protocol.Maximum, (float)protocol.Minimum, protocol.Cycle);

                _list.Add(meta);
                if (meta.Archive)
                {
                    //Id 和Cycle 其实就是归档的时间，归档周期没有设定（0），则置为 null
                    _archiveTimes.Add(meta.ID, meta.Cycle == 0 ? null : new ArchiveTime(meta.Cycle, DateTime.MinValue));//归档的ID,归档的时间
                }
                _list.Sort();//只是id 排序就好了
            }
            sql = "select * from ProtocolGroup";
            List<ProtocolGroup> groupList = DataHelper.Instance.ExecuteList<ProtocolGroup>(sql);
            foreach (ProtocolGroup pgroup in groupList)
            {
                if (_driver != null)
                {
                    IGroup grp = _driver.AddGroup(pgroup.GroupName, (short)pgroup.GroupID, pgroup.UpdateRate,
                           pgroup.DeadBand, pgroup.IsActive == 1 ? true : false);
                    if (grp != null)
                        grp.AddItems(_list);//这里完成 从 _list<TagMetaData> 到 _list<ITag>的转换  ，这个地方是需要根据 GroupID 的不同，生成不同的组别，对应于不同的页面??
                }
            }

            sql = "Select * from GloConfig";
            GloConfig = DataHelper.Instance.ExecuteList<GloConfig>(sql).FirstOrDefault();
            //using (var dataReader = DataHelper.Instance.ExecuteProcedureReader("InitServer"))
            //{
            //    if (dataReader == null) return;// Stopwatch sw = Stopwatch.StartNew();

            //    dataReader.Read();
            //    int count = dataReader.GetInt32(0);
            //    _list = new List<TagMetaData>(count);//把所有激活的 数据库记录 都导入 _list<TagMetaData> 里面
            //    _mapping = new Dictionary<string, ITag>(count);
            //    dataReader.NextResult();
            //    while (dataReader.Read())
            //    {
            //        var meta = new TagMetaData(dataReader.GetInt16(0), dataReader.GetInt16(1), dataReader.GetString(2), dataReader.GetString(3), dataReader.GetString(10), (DataType)dataReader.GetByte(4),
            //         (ushort)dataReader.GetInt16(5), dataReader.GetBoolean(6), dataReader.GetFloat(7), dataReader.GetFloat(8), dataReader.GetInt32(9));

            //        _list.Add(meta);
            //        if (meta.Archive)
            //        {
            //            //Id 和Cycle 其实就是归档的时间，归档周期没有设定（0），则置为 null
            //            _archiveTimes.Add(meta.ID, meta.Cycle == 0 ? null : new ArchiveTime(meta.Cycle, DateTime.MinValue));//归档的ID,归档的时间
            //        }
            //    }
            //    _list.Sort();//只是id 排序就好了
            //    dataReader.NextResult();
            //    while (dataReader.Read())
            //    {
            //        if (_driver != null)
            //        {
            //            IGroup grp = _driver.AddGroup(dataReader.GetString(1), dataReader.GetInt16(2), dataReader.GetInt32(3),
            //                   dataReader.GetFloat(4), dataReader.GetBoolean(5));
            //            if (grp != null)
            //                grp.AddItems(_list);//这里完成 从 _list<TagMetaData> 到 _list<ITag>的转换  ，这个地方是需要根据 GroupID 的不同，生成不同的组别，对应于不同的页面??
            //        }
            //    }
            //}
            //待归档的列表里面  有 超过一个的 变量,就开启归档
            if (_archiveTimes.Count > 0)
            {
                _hasHda = true;
                _hda.Capacity = MAXHDACAP;//单次归档的最大数据 1 万条；
            }

        }

        void InitServerByXml()
        {
            string path = PATH + '\\' + FILENAME;
            if (File.Exists(path))
            {
                try
                {
                    using (var reader = XmlTextReader.Create(path))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "Server":
                                        {
                                            if (reader.MoveToAttribute("MaxLogSize"))
                                                int.TryParse(reader.Value, out MAXLOGSIZE);
                                        }
                                        break;
                                    case "Data":
                                        {
                                            if (reader.MoveToAttribute("TestCycle"))
                                                int.TryParse(reader.Value, out CYCLE);
                                            if (reader.MoveToAttribute("SendTimeout"))
                                                int.TryParse(reader.Value, out SENDTIMEOUT);
                                        }
                                        break;
                                    case "Hda":
                                        {
                                            if (reader.MoveToAttribute("MaxHdaCap"))
                                            {
                                                int.TryParse(reader.Value, out MAXHDACAP);
                                            }
                                            if (reader.MoveToAttribute("HdaLen"))
                                                int.TryParse(reader.Value, out HDALEN);
                                            if (reader.MoveToAttribute("WriteCycle"))
                                                int.TryParse(reader.Value, out CYCLE2);
                                            if (reader.MoveToAttribute("Delay"))
                                                int.TryParse(reader.Value, out HDADELAY);
                                            if (reader.MoveToAttribute("Interval"))
                                                int.TryParse(reader.Value, out ARCHIVEINTERVAL);
                                        }
                                        break;
                                    case "Alarm":
                                        {
                                            if (reader.MoveToAttribute("AlarmLimit"))
                                                int.TryParse(reader.Value, out ALARMLIMIT);
                                            if (reader.MoveToAttribute("Delay"))
                                                int.TryParse(reader.Value, out ALARMDELAY);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    AddErrorLog(err);
                }
            }
        }


        /// <summary>
        /// 历史数据归档查询 包括文档中 和 数据库中，还有内存中的数据都要比对一下时间
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="ID"></param>
        /// <returns></returns>

        private IEnumerable<HistoryData> GetHData(DateTime start, DateTime end, short ID)
        {
            if (start > end) yield break;
            DateTime now = DateTime.Now;
            if (start > now) yield break;
            if (end > now) end = now;
            //如果现在的时间 大于要查询的开始时间，说明历史文件中可能有归档的数据
            //if (now.Date > start.Date)
            //{
            //    DateTime tempstart = DateTime.MinValue;
            //    DateTime tempend = end;
            //    HDAIOHelper.GetRangeFromDatabase(ID, ref tempend, ref tempstart);
            //    if (tempend > end) tempend = end;
            //    if (tempend > start)
            //    {
            //        int eyear = tempend.Year;
            //        int syear = start.Year;
            //        int emonth = tempend.Month;
            //        int smonth = start.Month;
            //        int year = syear;
            //        while (year <= eyear)
            //        {
            //            int month = (year == syear ? smonth : 1);
            //            while (month <= (year == eyear ? emonth : 12))
            //            {
            //                var result = HDAIOHelper.LoadFromFile((year == syear && month == smonth ? start : new DateTime(year, month, 1)),
            //                    (year == eyear && month == emonth ? tempend : new DateTime(year, month, 1).AddMonths(1).AddMilliseconds(-2)), ID);//考虑按月遍历
            //                if (result != null)
            //                {
            //                    foreach (var data in result)
            //                    {
            //                        yield return data;
            //                    }
            //                }
            //                month++;
            //            }
            //            year++;
            //        }
            //    }
            //}
            var tempdata = _hda.ToArray();
            DateTime ftime = (tempdata.Length > 0 ? tempdata[0].TimeStamp : DateTime.Now);//这里内存中最小的时间
            if (start < ftime)
            {
                var result = HDAIOHelper.LoadFromDatabase(start, ftime > end ? end : ftime, ID);//范围冲突
                if (result != null)
                {
                    foreach (var data in result)
                    {
                        yield return data;
                    }
                }
            }
            if (end > ftime)
            {
                var result = tempdata.Where(x => x.ID == ID && x.TimeStamp >= ftime && x.TimeStamp <= end);
                if (result != null)
                {
                    foreach (var data in result)
                    {
                        yield return data;
                    }
                }
            }
            yield break;
        }
        /// <summary>
        /// 从文件或者数据库中获取归档的数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private IEnumerable<HistoryData> GetHData(DateTime start, DateTime end)
        {
            if (start > end) yield break;
            DateTime now = DateTime.Now;
            if (start > now) yield break;
            if (end > now) end = now;
            //if (now.Date > start.Date)
            //{
            //    DateTime tempstart = DateTime.MinValue;
            //    DateTime tempend = end;
            //    HDAIOHelper.GetRangeFromDatabase(null, ref tempend, ref tempstart);
            //    if (tempend > start)
            //    {
            //        int eyear = tempend.Year;
            //        int syear = start.Year;
            //        int emonth = tempend.Month;
            //        int smonth = start.Month;
            //        int year = syear;
            //        while (year <= eyear)
            //        {
            //            int month = (year == syear ? smonth : 1);
            //            while (month <= (year == eyear ? emonth : 12))
            //            {
            //                var result = HDAIOHelper.LoadFromFile((year == syear && month == smonth ? start : new DateTime(year, month, 1)),
            //                    (year == eyear && month == emonth ? tempend : new DateTime(year, month, 1).AddMonths(1).AddMilliseconds(-2)));//考虑按月遍历
            //                if (result != null)
            //                {
            //                    foreach (var data in result)
            //                    {
            //                        yield return data;
            //                    }
            //                }
            //                month++;
            //            }
            //            year++;
            //        }
            //    }
            //}
            var tempdata = _hda.ToArray();
            DateTime ftime = (tempdata.Length > 0 ? tempdata[0].TimeStamp : DateTime.Now);
            if (start < ftime)
            {
                var result = HDAIOHelper.LoadFromDatabase(start, ftime > end ? end : ftime);//范围冲突
                if (result != null)
                {
                    foreach (var data in result)
                    {
                        yield return data;
                    }
                }
            }
            if (end > ftime)
            {
                var result = tempdata.Where(x => x.TimeStamp >= ftime && x.TimeStamp <= end);
                if (result != null)
                {
                    foreach (var data in result)
                    {
                        yield return data;
                    }
                }
            }
            yield break;
        }

        private object _hdaRoot = new object();
        /// <summary>
        /// 手动 归档缓存中的数据
        /// </summary>
        public void Flush()
        {
            lock (_hdaRoot)
            {
                if (_hda.Count == 0) return;
                if (DataHelper.Instance.BulkCopy(new HDASqlReader(_hda, this), "Log",
                      string.Format("DELETE FROM Log WHERE [TIMESTAMP]>'{0}'", _hda[0].TimeStamp.ToString("yyyy/MM/dd HH:mm:ss"))))
                {
                    _hda.Clear();
                    _hdastart = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 整个函数是在归档，把缓存中时间范围内的未归档数据写入到数据库中。
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public bool SaveRange(DateTime startTime, DateTime endTime)
        {
            var tempdata = _hda.ToArray();
            if (tempdata.Length == 0) return true;
            return DataHelper.Instance.BulkCopy(new HDASqlReader(GetData(tempdata, startTime, endTime), this), "Log",
                     string.Format("DELETE FROM Log WHERE [TIMESTAMP] BETWEEN '{0}' AND '{1}'", startTime.ToString("yyyy/MM/dd HH:mm:ss"), endTime.ToString("yyyy/MM/dd HH:mm:ss")));
        }

        int SaveTick = 0;
        /// <summary> 
        /// 把数据加入到 Log_HData 进行归档存储
        /// 历史数据归档失败后，会开一个子线程：用于在将来再次尝试进行归档，但是不能无限尝试啊，所以尝试5次后还不成功就丢弃了。
        /// </summary>
        /// <param name="stateInfo"></param>
        public void OnUpdate(object stateInfo)
        {
            lock (_hdaRoot)
            {
                var tempData = (List<HistoryData>)stateInfo;
                _hda.AddRange(tempData);//把新产生的数据加入到 _hda末尾；
                if (_hda.Count >= 1 && SaveTick > 5)//这个是每次有 10000条数据才写入数据库里面
                {
                    if (DataHelper.Instance.GetType().Name == "SQLiteFac")
                    {
                        List<string> sqlList = new List<string>();
                        foreach (HistoryData data in _hda)
                        {

                            //SysLog sysLog = new SysLog();
                            //sysLog.Id = data.ID;
                            //sysLog.Value = data.Value.Int32;
                            //sysLog.TimeStamp = data.TimeStamp;
                            if (data.ID == 465 || data.ID == 505 || data.ID == 545 || data.ID == 463 || data.ID == 503 || data.ID == 543 || data.ID == 37
                                || data.ID == 40 || data.ID == 38 || data.ID == 41 || data.ID == 39 || data.ID == 42 || data.ID == 34 || data.ID == 35
                                || data.ID == 36 || data.ID == 18 || data.ID == 19 || data.ID == 593 || data.ID == 603 || data.ID == 592 || data.ID == 603
                                || data.ID == 592 || data.ID == 20 || data.ID == 21 || data.ID == 25 || data.ID == 27 || data.ID == 29 || data.ID == 15
                                || data.ID == 13 || data.ID == 727 || data.ID == 722 || data.ID == 724 || data.ID == 739 || data.ID == 736 || data.ID == 698
                                || data.ID == 701 || data.ID == 700 || data.ID == 699 || data.ID == 75 || data.ID == 76 || data.ID == 77 || data.ID == 78
                                || data.ID == 600 || data.ID == 810 || data.ID == 1071 || data.ID == 1072 || data.ID == 1073 || data.ID == 1074 || data.ID == 1075
                                || data.ID == 1135)
                            {
                                string sql = string.Format("Insert Into Log (Id,Value,TimeStamp,LogType) Values('{0}','{1}','{2}','1')", data.ID, data.Value.Int32, data.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"));
                                sqlList.Add(sql);
                            }
                            else
                            {
                                string sql = string.Format("Insert Into Log (Id,Value,TimeStamp,LogType) Values('{0}','{1}','{2}','2')", data.ID, data.Value.Int32, data.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"));
                                sqlList.Add(sql);
                            }
                        }
                        DataHelper.Instance.ExecuteNonQuery(sqlList.ToArray());
                    }
                    else
                    {
                        //Reverse(data);
                        DateTime start = _hda[0].TimeStamp;
                        //_array.CopyTo(data, 0);
                        if (DataHelper.Instance.BulkCopy(new HDASqlReader(_hda, this), "Log",
                        string.Format("DELETE FROM Log WHERE [TIMESTAMP]>'{0}'", start.ToString("yyyy/MM/dd HH:mm:ss"))))//删除历史数据库中存在的那些记录，这些记录的时间大于列表中最小时间记录
                            _hdastart = DateTime.Now;
                        else ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.SaveCachedData), _hda.ToArray());
                        _hda.Clear();//最后面把列表清空。
                        SaveTick = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 如果直接归档数据不成功，就单独开辟一个线程处理。
        /// </summary>
        /// <param name="stateInfo"></param>
        public void SaveCachedData(object stateInfo)
        {
            var tempData = (HistoryData[])stateInfo;
            if (tempData.Length == 0) return;
            DateTime startTime = tempData[0].TimeStamp;
            DateTime endTime = tempData[tempData.Length - 1].TimeStamp;
            //Thread.Sleep(TimeSpan.FromMinutes(10)); 
            int count = 0;
            while (true)
            {
                if (count >= 5) return;//多次写入，防止写失败
                if (DataHelper.Instance.BulkCopy(new HDASqlReader(tempData, this), "Log",
                   string.Format("DELETE FROM Log WHERE [TIMESTAMP] BETWEEN '{0}' AND '{1}'",
                    startTime.ToString("yyyy/MM/dd HH:mm:ss"), endTime.ToString("yyyy/MM/dd HH:mm:ss"))))
                {
                    stateInfo = null;
                    _hdastart = DateTime.Now;
                }
                count++;
                Thread.Sleep(CYCLE2);//60万
            }
        }

        /// <summary>
        /// 从数组中 返回在时间范围内的数据
        /// </summary>
        /// <param name="hdaarray"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IEnumerable<HistoryData> GetData(HistoryData[] hdaarray, DateTime startTime, DateTime endTime)
        {
            //if (hdaarray.Length == 0) yield break;
            foreach (var data in hdaarray)
            {
                if (data.TimeStamp >= startTime)
                {
                    if (data.TimeStamp <= endTime)
                        yield return data;
                    else
                        yield break;
                }
            }
        }
        #endregion

        /// <summary>
        /// 调用所属group中的批量读取函数 BatchRead
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sync"></param>
        /// <param name="itemArray"></param>
        /// <returns></returns>
        public HistoryData[] BatchRead(DataSource source, bool sync, params ITag[] itemArray)
        {
            int count = itemArray.Length;
            HistoryData[] data = new HistoryData[count];
            Dictionary<IGroup, List<ITag>> dict = new Dictionary<IGroup, List<ITag>>();
            for (int i = 0; i < count; i++)
            {
                short id = itemArray[i].ID;
                ITag tag = this[id];
                if (tag != null)
                {
                    IGroup grp = tag.Parent;
                    if (!dict.ContainsKey(grp))
                        dict.Add(grp, new List<ITag> { tag });
                    else
                        dict[grp].Add(tag);
                }
            }
            int j = 0;
            foreach (var dev in dict)
            {
                var list = dev.Value;
                var array = dev.Key.BatchRead(source, sync, list.ToArray());
                if (array == null) continue;
                Array.Copy(array, 0, data, j, array.Length);
                j += array.Length;
            }
            return data;
        }

        public int BatchWrite(Dictionary<string, object> tags, bool sync)
        {
            int rs = -1;
            Dictionary<IGroup, SortedDictionary<ITag, object>> dict = new Dictionary<IGroup, SortedDictionary<ITag, object>>();
            foreach (var item in tags)
            {
                var tag = this[item.Key];
                if (tag != null)
                {
                    IGroup grp = tag.Parent;
                    SortedDictionary<ITag, object> values;
                    if (!dict.ContainsKey(grp))
                    {
                        values = new SortedDictionary<ITag, object>();
                        if (tag.Address.VarType != DataType.BOOL && tag.Address.VarType != DataType.STR)
                        {
                            values.Add(tag, tag.ValueToScale(Convert.ToSingle(item.Value)));
                        }
                        else
                            values.Add(tag, item.Value);
                        dict.Add(grp, values);
                    }
                    else
                    {
                        values = dict[grp];
                        if (tag.Address.VarType != DataType.BOOL && tag.Address.VarType != DataType.STR)
                        {
                            values.Add(tag, tag.ValueToScale(Convert.ToSingle(item.Value)));
                        }
                        else
                            values.Add(tag, item.Value);
                    }
                }
                else
                {
                    Log4Net.AddLog(string.Format("变量{0}不在变量表中，无法下载", item.Key), InfoLevel.ERROR);
                }
            }
            foreach (var dev in dict)
            {
                rs = dev.Key.BatchWrite(dev.Value, sync);
            }
            return rs;
        }

        /// <summary>
        /// ??为什么这里单单只是，对那些归档周期为0的数据进行更新，因为这些可以归为一组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grp_DataChange(object sender, DataChangeEventArgs e)
        {
            //var data = e.Values;//IList<HistoryData> Values
            //var now = DateTime.Now;
            //if (_hasHda)
            //{
            //    ArchiveTime archiveTime;
            //    List<HistoryData> tempData = new List<HistoryData>(20);//初始大小设定为为20，
            //    for (int i = 0; i < data.Count; i++)
            //    {
            //        //归档时间为 0 ，但是数据里面的时间戳 已经有更新，也要归档
            //        if (_archiveTimes.TryGetValue(data[i].ID, out archiveTime) && archiveTime == null && data[i].TimeStamp != DateTime.MinValue)
            //        {
            //            tempData.Add(data[i]);
            //        }
            //    }
            //    if (tempData.Count > 0)
            //    {
            //        //这是把有变化的数据归档
            //        ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.OnUpdate), tempData);
            //    }
            //}
        }

        /// <summary>
        /// 把异常写入系统日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void reader_OnClose(object sender, IOErrorEventArgs e)
        {
            Log4Net.AddLog(e.Reason, InfoLevel.ERROR);
        }

        /// <summary>
        /// 通过 Itag的名字，把所有的Itag值都存储起来
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddItemIndex(string key, ITag value)
        {
            key = key.ToUpper();
            if (_mapping.ContainsKey(key))
                return false;
            _mapping.Add(key, value);
            return true;
        }

        public bool RemoveItemIndex(string key)
        {
            return _mapping.Remove(key.ToUpper());
        }

        object _alarmsync = new object();

        string[] itemList = null;
        public IEnumerable<string> BrowseItems(BrowseType browseType, string tagName, DataType dataType)
        {
            if (_list.Count == 0) yield break;
            int len = _list.Count;
            if (itemList == null)
            {
                itemList = new string[len];
                for (int i = 0; i < len; i++)
                {
                    itemList[i] = _list[i].Name;
                }
                Array.Sort(itemList);
            }
            int ii = 0;
            bool hasTag = !string.IsNullOrEmpty(tagName);
            bool first = true;
            string str = hasTag ? tagName + SPLITCHAR : string.Empty;
            if (hasTag)
            {
                ii = Array.BinarySearch(itemList, tagName);
                if (ii < 0) first = false;
                //int strLen = str.Length;
                ii = Array.BinarySearch(itemList, str);
                if (ii < 0) ii = ~ii;
            }
            //while (++i < len && temp.Length >= strLen && temp.Substring(0, strLen) == str)
            do
            {
                if (first && hasTag)
                {
                    first = false;
                    yield return tagName;
                }
                string temp = itemList[ii];
                if (hasTag && !temp.StartsWith(str, StringComparison.Ordinal))
                    break;
                if (dataType == DataType.NONE || _mapping[temp].Address.VarType == dataType)
                {
                    bool b3 = true;
                    if (browseType != BrowseType.Flat)
                    {
                        string curr = temp + SPLITCHAR;
                        int index = Array.BinarySearch(itemList, ii, len - ii, curr);
                        if (index < 0) index = ~index;
                        b3 = itemList[index].StartsWith(curr, StringComparison.Ordinal);
                        if (browseType == BrowseType.Leaf)
                            b3 = !b3;
                    }
                    if (b3)
                        yield return temp;
                }
            } while (++ii < len);
        }


        public IGroup GetGroupByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            foreach (IDriver device in Drivers)
            {
                foreach (IGroup grp in device.Groups)
                {
                    if (grp.Name == name)
                        return grp;
                }
            }
            return null;
        }

        public void ActiveItem(bool active, params ITag[] items)
        {
            Dictionary<IGroup, List<short>> dict = new Dictionary<IGroup, List<short>>();
            for (int i = 0; i < items.Length; i++)
            {
                List<short> list = null;
                ITag item = items[i];
                dict.TryGetValue(item.Parent, out list);
                if (list != null)
                {
                    list.Add(item.ID);
                }
                else
                    dict.Add(item.Parent, new List<short> { item.ID });

            }
            foreach (var grp in dict)
            {
                grp.Key.SetActiveState(active, grp.Value.ToArray());
            }
        }

        /// <summary>
        /// 返回在整个列表的索引，通过ID找到列表索引
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetItemProperties(short id)
        {
            return _list.BinarySearch(new TagMetaData { ID = id });
        }

        public string Read(string id)
        {
            throw new NotImplementedException();
        }

        public bool ReadExpression(string expression)
        {
            throw new NotImplementedException();
        }

        public int Write(string id, string value)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> BatchRead(string[] tags)
        {
            throw new NotImplementedException();
        }

        public int BatchWrite(Dictionary<string, string> tags)
        {
            throw new NotImplementedException();
        }

        public Stream LoadMetaData()
        {
            throw new NotImplementedException();
        }

        public Stream LoadHdaBatch(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Stream LoadHdaSingle(DateTime start, DateTime end, short id)
        {
            throw new NotImplementedException();
        }

        public IDriver AddDriver(short id, string name, string assembly, string className)
        {
            throw new NotImplementedException();
        }

        public bool RemoveDriver(IDriver device)
        {
            throw new NotImplementedException();
        }

        public int GetScaleByID(short id)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 和具体IP地址绑定的 带归档数据
    /// </summary>
    class TempCachedData
    {
        IPAddress _addr;
        public IPAddress Address
        {
            get { return _addr; }
        }

        IList<HistoryData> _data;
        public IList<HistoryData> Data
        {
            get { return _data; }
        }

        public TempCachedData(IPAddress addr, IList<HistoryData> data)
        {
            _addr = addr;
            _data = data;
        }
    }

    /// <summary>
    /// 归档周期 、最后一次归档的时间
    /// </summary>
    internal sealed class ArchiveTime
    {
        public int Cycle;//归档周期
        public DateTime LastTime;//最后一次归档的时间
        public ArchiveTime(int cycle, DateTime last)
        {
            Cycle = cycle;
            LastTime = last;
        }
    }

    public class Protocol
    {
        /// <summary>
        /// 变量ID
        /// </summary>
        public int TagID { get; set; }
        /// <summary>
        /// 变量名
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public int DataType { get; set; }
        /// <summary>
        /// 数据大小
        /// </summary>
        public int DataSize { get; set; }
        /// <summary>
        /// 数据地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public int GroupID { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public int IsActive { get; set; }
        /// <summary>
        /// 是否归档
        /// </summary>
        public int Archive { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public int DefaultValue { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public double Maximum { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public double Minimum { get; set; }
        /// <summary>
        /// 更新周期（ms）
        /// </summary>
        public int Cycle { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        public DateTime RowVersion { get; set; }
    }

    public class ProtocolGroup
    {
        public int GroupID { get; set; }
        public int DriverID { get; set; }
        public string GroupName { get; set; }
        public int UpdateRate { get; set; }
        public int DeadBand { get; set; }
        public int IsActive { get; set; }
    }

    public class SysLog
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime TimeStamp { get; set; }
        public int OID{ get; set; }
    }

    /// <summary>
    /// 全局配置
    /// </summary>
    public class GloConfig
    {
        /// <summary>
        /// UDP发送IP
        /// </summary>
        public string UdpSendIP { set; get; }
        /// <summary>
        /// UDP端口
        /// </summary>
        public int UdpSendPort { set; get; }
        /// <summary>
        /// Udp是否开放
        /// </summary>
        public bool UdpOpen { set; get; }
        /// <summary>
        /// 铁钻工类型 0-无 1-自研 2-JJC 3-宝石 4-江汉
        /// </summary>
        public int SIRType { get; set; }
        /// <summary>
        /// 猫道类型 0-无猫道 1-自研 2-宝石猫道 3-宏达猫道
        /// </summary>
        public int CatType { get; set; }
        /// <summary>
        /// 液压站类型 0-无液压站 1-自研 2-宝石液压站 3- JJC液压站
        /// </summary>
        public int HydType { get; set; }
        /// <summary>
        /// 钻台面类型 0-自研 1-杰瑞
        /// </summary>
        public int DRType { get; set; }
    }
}