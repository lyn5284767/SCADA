using COM.Common;
using DatabaseLib;
using Main.SecondFloor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Main.Integration
{
    /// <summary>
    /// IngRecord.xaml 的交互逻辑
    /// </summary>
    public partial class IngRecord : UserControl
    {
        private static IngRecord _instance = null;
        private static readonly object syncRoot = new object();

        public static IngRecord Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngRecord();
                        }
                    }
                }
                return _instance;
            }
        }
        Dictionary<int, string> dicNumToValue = new Dictionary<int, string>();
        BackgroundWorker bgMeet;
        public IngRecord()
        {
            InitializeComponent();
            Init();
            this.Loaded += IngRecord_Loaded;
        }
        private void IngRecord_Loaded(object sender, RoutedEventArgs e)
        {
            QueryRecord();
            this.beginTime.SelectedDate = DateTime.Now.AddDays(-1);
            this.endTime.SelectedDate = DateTime.Now.AddDays(1);
        }
        /// <summary>
        /// 初始化查询类型
        /// </summary>
        public void Init()
        {
            dicNumToValue.Add(0, "全部");
            dicNumToValue.Add(1, "卡瓦互锁");
            dicNumToValue.Add(2, "吊卡互锁");
            dicNumToValue.Add(3, "大钩互锁");
            dicNumToValue.Add(4, "顶驱互锁");
            dicNumToValue.Add(5, "铁架工互锁");
            dicNumToValue.Add(6, "扶杆臂互锁");
            dicNumToValue.Add(7, "铁钻工互锁");
            dicNumToValue.Add(8, "防喷盒互锁");
            dicNumToValue.Add(9, "猫道互锁");

            this.cbTypeSelect.ItemsSource = dicNumToValue;
            this.cbTypeSelect.SelectedValuePath = "Key";
            this.cbTypeSelect.DisplayMemberPath = "Value";
            this.cbTypeSelect.SelectedIndex = 0;
        }

        private void QueryRecord()
        {
            this.lvRecord.ItemsSource = null;
            bgMeet = new BackgroundWorker();
            //能否报告进度更新
            bgMeet.WorkerReportsProgress = true;
            //要执行的后台任务
            bgMeet.DoWork += new DoWorkEventHandler(bgMeet_DoWork);
            //进度报告方法
            bgMeet.ProgressChanged += new ProgressChangedEventHandler(bgMeet_ProgressChanged);
            //后台任务执行完成时调用的方法
            bgMeet.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgMeet_RunWorkerCompleted);
            bgMeet.RunWorkerAsync(); //任务启动
        }


        public delegate void ShowRecord();//定义委托

        private void BindingRecord()
        {
            string sql = string.Format("SELECT  * FROM Log {0} Order by OID DESC LIMIT 0,1000", cond);
            List<RecordDate> recordList = DataHelper.Instance.ExecuteList<RecordDate>(sql);
            List<ShowDate> showList = new List<ShowDate>();
            if (recordList != null)
            {
                foreach (RecordDate date in recordList)
                {
                    ShowDate showDate = new ShowDate();
                    showDate.OID = date.OID;
                    showDate.Content = GetContent(date.ID);
                    showDate.Value = date.Value == 1 ? "解锁" : "上锁";
                    showDate.Type = GetType(date.ID);
                    showDate.TimeStamp = date.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss");
                    showList.Add(showDate);
                }
            }
            this.lvRecord.ItemsSource = showList;
        }
        /// <summary>
        /// 根据ID获取内容
        /// </summary>
        /// <param name="id">主键ID</param>
        private string GetContent(int id)
        {
            if (id == 1471) return "吊卡关门允许卡瓦打开";
            else if (id == 1473) return "铁钻工非上卸扣允许卡瓦打开";
            else if (id == 1475) return "大钩静止允许卡瓦关闭";
            else if (id == 1477) return "抓手关闭允许吊卡打开";
            else if (id == 1479) return "卡瓦关门允许吊卡打开";
            else if (id == 1481) return "铁架工在井口禁止顶驱偏摆";
            else if (id == 1483) return "吊卡关门进行中锁大钩";
            else if (id == 1485) return "顶驱偏摆锁大钩";
            else if (id == 1487) return "铁架工在井口锁大钩";
            else if (id == 1489) return "扶杆臂在井口锁大钩";
            else if (id == 1491) return "铁钻工在井口锁大钩";
            else if (id == 1493) return "防喷盒在井口锁大钩";
            else if (id == 1495) return "吊卡关门允许抓手打开";
            else if (id == 1497) return "吊卡开门允许手臂回收";
            else if (id == 1499) return "大钩高位允许机械手井口移动";
            else if (id == 1501) return "吊卡关门允许抓手打开";
            else if (id == 1503) return "大钩低位允许机械手井口移动";
            else if (id == 1505) return "铁钻工井口禁止扶杆臂井口移动";
            else if (id == 1507) return "防喷盒井口禁止扶杆臂井口移动";
            else if (id == 1509) return "猫道防碰区禁止行车移动";
            else if (id == 1511) return "大钩低位禁止铁钻工井口移动";
            else if (id == 1513) return "扶杆臂井口禁止铁钻工井口移动";
            else if (id == 1515) return "防喷盒井口禁止铁钻工井口移动";
            else if (id == 1517) return "卡瓦关门允许铁钻工上卸扣";
            else if (id == 1519) return "大钩低位禁止防喷盒井口移动";
            else if (id == 1521) return "扶杆臂井口禁止防喷盒井口移动";
            else if (id == 1523) return "铁钻工井口禁止防喷盒井口移动";
            else if (id == 1525) return "扶杆臂中位禁止推管柱上钻台";
            else if (id == 1529) return "顶驱卡扣允许卡瓦打开";
            else if (id == 1531) return "顶驱卡扣允许吊卡打开";
            else if (id == 1533) return "卡瓦关门允许顶驱解扣";
            else if (id == 1535) return "吊卡关门允许顶驱解扣";
            else return string.Empty;
        }

        private string GetType(int id)
        {
            if (id == 1471 || id == 1473 || id == 1475 || id == 1529) return "卡瓦互锁";
            else if (id == 1477 || id == 1479 || id == 1531) return "吊卡互锁";
            else if (id == 1483 || id == 1485 || id == 1487 || id == 1489 || id == 1491 || id == 1493) return "大钩互锁";
            else if (id == 1481 || id == 1533 || id == 1535) return "顶驱互锁";
            else if (id == 1495 || id == 1497 || id == 1499) return "铁架工互锁";
            else if (id == 1501 || id == 1503 || id == 1505 || id == 1507 || id == 1509) return "扶杆臂互锁";
            else if (id == 1511 || id == 1513 || id == 1515 || id == 1517) return "铁钻工互锁";
            else if (id == 1519 || id == 1521 || id == 1523) return "防喷盒互锁";
            else if (id == 1525) return "猫道互锁";
            else return string.Empty;
        }

        //执行任务
        void bgMeet_DoWork(object sender, DoWorkEventArgs e)
        {
            //开始播放等待动画
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.projectLoad.Visibility = System.Windows.Visibility.Visible;
            }));
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ShowRecord(BindingRecord));
        }
        //报告任务进度
        void bgMeet_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //this.Dispatcher.Invoke(new Action(() =>
            //{
            //    this.lab_pro.Content = e.ProgressPercentage + "%";
            //}));
        }
        //任务执行完成后更新状态
        void bgMeet_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.projectLoad.Visibility = System.Windows.Visibility.Collapsed;
        }

        string cond = "Where LogType='11'";

        /// <summary>
        /// 结束时间改变
        /// </summary>
        private void endTime_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.endTime.SelectedDate <= this.beginTime.SelectedDate)
            {
                MessageBox.Show("结束日期必须大于开始日期");
            }
        }
        /// <summary>
        /// 记录查询
        /// </summary>
        private void Button_MouseDown(object sender, RoutedEventArgs e)
        {
            string tagID = LockTypeStr(this.cbTypeSelect.SelectedValue.ToString());
            string bTime = this.beginTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string eTime = this.endTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string condition = string.Empty;
            if (tagID != "0") condition = string.Format("Where Id in ({0}) and TimeStamp>'{1}' and TimeStamp<'{2}' and LogType='11'", tagID, bTime, eTime);
            else condition = string.Format("Where TimeStamp>'{0}' and TimeStamp<'{1}' and LogType='11'", bTime, eTime);
            cond = condition;
            QueryRecord();
        }
        /// <summary>
        /// 根据互锁类型数据库查询具体信息
        /// </summary>
        /// <param name="val">comgbox类型</param>
        /// <returns></returns>
        private string LockTypeStr(string val)
        {
            switch (val)
            {
                case "0": // 全部
                    return "0";
                case "1":// 卡瓦
                    return "1471,1473,1475,1529";
                case "2":// 吊卡
                    return "1477,1479,1531";
                case "3":// 大钩
                    return "1483,1485,1487,1489,1491,1493";
                case "4":// 顶驱
                    return "1481,1533,1535";
                case "5":// 铁架工
                    return "1495,1497,1499";
                case "6":// 扶杆臂
                    return "1501,1503,1505,1507,1509";
                case "7":// 铁钻工
                    return "1511,1513,1515,1517";
                case "8":// 防喷盒
                    return "1519,1521,1523";
                case "9":// 猫道
                    return "1525";
                default:
                    return "0";
            }
        }
    }
}
