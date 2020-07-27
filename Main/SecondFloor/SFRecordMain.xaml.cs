using DatabaseLib;
using DevExpress.Xpf.Editors;
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

namespace Main.SecondFloor
{
    /// <summary>
    /// SFRecordMain.xaml 的交互逻辑
    /// </summary>
    public partial class SFRecordMain : UserControl
    {

        private static SFRecordMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SFRecordMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFRecordMain();
                        }
                    }
                }
                return _instance;
            }
        }
        BackgroundWorker bgMeet;
        Dictionary<int, string> dicNumToValue = new Dictionary<int, string>();

        public SFRecordMain()
        {
            InitializeComponent();
            //PageChange();
            Init();
            this.Loaded += SFRecordMain_Loaded;
        }

        public void Init()
        {
            dicNumToValue.Add(0, "全部");
            dicNumToValue.Add(465, "小车电机报警状态");
            dicNumToValue.Add(505, "手臂电机报警状态");
            dicNumToValue.Add(545, "回转电机报警状态");
            dicNumToValue.Add(463, "小车电机报警代码");
            dicNumToValue.Add(503, "手臂电机报警代码");
            dicNumToValue.Add(543, "回转电机报警代码");
            dicNumToValue.Add(37, "抓手打开卡滞");
            dicNumToValue.Add(40, "抓手关闭卡滞");
            dicNumToValue.Add(38, "左手指打开卡滞");
            dicNumToValue.Add(41, "左手指关闭卡滞");
            dicNumToValue.Add(39, "右手指打开卡滞");
            dicNumToValue.Add(42, "右手指关闭卡滞");
            dicNumToValue.Add(34, "抓手传感器断线");
            dicNumToValue.Add(35, "左手指传感器断线");
            dicNumToValue.Add(36, "右手指传感器断线");
            dicNumToValue.Add(18, "操作模式");
            dicNumToValue.Add(19, "工作模式");
            dicNumToValue.Add(593, "机械手对准指梁号");
            dicNumToValue.Add(603, "自动模式步骤");
            dicNumToValue.Add(592, "机械手当前位置区域");
            dicNumToValue.Add(20, "抓手状态");
            dicNumToValue.Add(21, "管柱类型");
            dicNumToValue.Add(25, "小车回零");
            dicNumToValue.Add(27, "手臂回零");
            dicNumToValue.Add(29, "旋转回零");
            dicNumToValue.Add(15, "机械手在井口");
            dicNumToValue.Add(13, "操作员选择的指梁号");
            dicNumToValue.Add(727, "左手柄使能");
            dicNumToValue.Add(722, "左手柄左右");
            dicNumToValue.Add(724, "左手柄前后");
            dicNumToValue.Add(739, "吊卡状态");
            dicNumToValue.Add(736, "互锁提示");
            dicNumToValue.Add(698, "大钩标定状态");
            dicNumToValue.Add(701, "关门信号屏蔽");
            dicNumToValue.Add(700, "二层台与顶驱互锁");
            dicNumToValue.Add(699, "二层台与大钩互锁");
            dicNumToValue.Add(75, "左挡绳伸出");
            dicNumToValue.Add(76, "左挡绳缩回");
            dicNumToValue.Add(77, "右挡绳伸出");
            dicNumToValue.Add(78, "右挡绳缩回");
            dicNumToValue.Add(600, "机械手挡绳互锁");
            dicNumToValue.Add(810, "回转回零限制");
            dicNumToValue.Add(1071, "吊卡打开限制");
            dicNumToValue.Add(1072, "左手指打开");
            dicNumToValue.Add(1073, "右手指打开");
            dicNumToValue.Add(1074, "左手指关闭");
            dicNumToValue.Add(1075, "右手指关闭");
            dicNumToValue.Add(1135, "自动选择指梁");

            this.cbTypeSelect.ItemsSource = dicNumToValue;
            this.cbTypeSelect.SelectedValuePath = "Key";
            this.cbTypeSelect.DisplayMemberPath = "Value";
            this.cbTypeSelect.SelectedIndex = 0;
        }

        private void SFRecordMain_Loaded(object sender, RoutedEventArgs e)
        {
            //PageChange();
            QueryRecord();
            this.beginTime.Value = DateTime.Now.AddDays(-1);
            this.endTime.Value = DateTime.Now.AddDays(1);
        }


        private void QueryRecord()
        {
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
                    string content = string.Empty;
                    dicNumToValue.TryGetValue(date.ID, out content);
                    showDate.Content = content;
                    showDate.Value = GetValue(date.ID, date.Value);
                    showDate.Type = GetRecordType(date.ID);
                    showDate.TimeStamp = date.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss");
                    showList.Add(showDate);
                }
            }
            if (cbOprOrAlarm.SelectedIndex == 1)
            {
                showList = showList.Where(w => w.Type == "报警信息").ToList();
            }
            else if (cbOprOrAlarm.SelectedIndex == 2)
            {
                showList = showList.Where(w => w.Type == "状态信息").ToList();
            }
            this.lvRecord.ItemsSource = showList;
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

        private string GetValue(int content,int value)
        {
            if (content == 465 || content == 505 || content == 545)
            {
                #region 电机告警状态
                switch (value)
                {
                    case 5:
                        return "正常";
                    case 2:
                        return "异常";
                    case 3:
                        return "使能";
                    case 4:
                        return "打开";
                    case 1:
                        return "断电";
                    case 6:
                        return "停止";
                    case 7:
                        return "错误7";
                    case 8:
                        return "错误8";
                    case 127:
                        return "错误";
                    default:
                        return value.ToString();
                }
                #endregion
            }
            else if (content == 463 || content == 503 || content == 543)
            {
                return value.ToString();
            }
            else if (content >= 34 && content <= 42)
            {
                if (value == 1) return "报警";
                else return "正常";
            }
            else if (content == 18)
            {
                #region 操作模式
                switch (value)
                {
                    case 1:
                        return "急停";
                    case 2:
                        return "调试模式";
                    case 3:
                        return "回零";
                    case 4:
                        return "手动";
                    case 5:
                        return "自动";
                    case 6:
                        return "回收";
                    case 7:
                        return "运输";
                    case 8:
                        return "实验";
                    case 9:
                        return "补偿模式";
                    default:
                        return value.ToString();
                }
                #endregion;
            }
            else if (content == 19)
            {
                #region 工作模式
                switch (value)
                {
                    case 1:
                        return "送杆";
                    case 2:
                        return "排杆";
                    default:
                        return value.ToString();
                }
                #endregion
            }
            else if (content == 20)
            {
                #region 抓手18中状态
                switch (value)
                {
                    case 1:
                        return "抓手打开手指打开无钻杆";
                    case 2:
                        return "抓手打开手指中间无钻杆";
                    case 3:
                        return "抓手打开手指关闭无钻杆";
                    case 4:
                        return "抓手打开手指打开有钻杆";
                    case 5:
                        return "抓手打开手指中间有钻杆";
                    case 6:
                        return "抓手打开手指关闭有钻杆";
                    case 7:
                        return "抓手中间手指打开无钻杆";
                    case 8:
                        return "抓手中间手指中间无钻杆";
                    case 9:
                        return "抓手中间手指关闭无钻杆";
                    case 10:
                        return "抓手中间手指打开有钻杆";
                    case 11:
                        return "抓手中间手指中间有钻杆";
                    case 12:
                        return "抓手中间手指关闭有钻杆";
                    case 13:
                        return "抓手关闭手指打开无钻杆";
                    case 14:
                        return "抓手关闭手指中间无钻杆";
                    case 15:
                        return "抓手关闭手指关闭无钻杆";
                    case 16:
                        return "抓手关闭手指打开有钻杆";
                    case 17:
                        return "抓手关闭手指中间有钻杆";
                    case 18:
                        return "抓手关闭手指关闭有钻杆";
                    default:
                        return "未知";
                }
                #endregion
            }
            else if (content == 21)
            {
                #region 管柱类型
                if (value == 35) return "3.5寸钻杆";
                else if (value == 40) return "4寸钻杆";
                else if (value == 45) return "4.5寸钻杆";
                else if (value == 50) return "5寸钻杆";
                else if (value == 55) return "5.5寸钻杆";
                else if (value == 57) return "5寸7/8钻杆";
                else if (value == 60) return "6寸钻铤";
                else if (value == 65) return "6.5寸钻铤";
                else if (value == 68) return "6寸5/8钻杆";
                else if (value == 70) return "7寸钻铤";
                else if (value == 75) return "7.5寸钻铤";
                else if (value == 80) return "8寸钻铤";
                else if (value == 90) return "9寸钻铤";
                else if (value == 100) return "10寸钻铤";
                else if (value == 110) return "11寸钻铤";
                #endregion
            }
            else if (content == 25 || content == 27 || content == 29)
            {
                if (value == 1) return "回零";
                else return "未回零";
            }
            else if (content == 15)
            {
                if (value == 1) return "在井口";
                else return "不在井口";
            }
            else if (content == 727)
            {
                if (value == 1) return "使能";
                else return "未使能";
            }
            else if (content == 739)
            {
                #region 吊卡状态
                switch (value)
                {
                    case 1:
                        return "打开无杆";
                    case 2:
                        return "关闭无杆";
                    case 3:
                        return "打开有杆";
                    case 4:
                        return "关闭有杆";
                    default:
                        return "未知";
                }
                #endregion
            }
            else if (content == 736)
            {
                #region 互锁提示
                if (value == 30) return "吊卡与大钩互锁中";
                else if (value == 31) return "互锁解除中，请注意吊卡是否关闭";
                else if (value == 32) return "互锁解除中，请谨慎操作大钩";
                else if (value == 33) return "危险区域，大钩安全高度未标定";
                else if (value == 34) return "危险区域，机械手与大钩互锁中";
                else if (value == 35) return "危险区域，大钩下降中";
                else if (value == 36) return "危险区域，大钩上升中";
                else if (value == 37) return "危险区域，大钩在机械手下方";
                else if (value == 38) return "危险区域，大钩在机械手上方";
                else if (value == 43) return "大钩安全高度未标定";
                else if (value == 45) return "大钩下降中";
                else if (value == 46) return "大钩上升中";
                else if (value == 47) return "大钩在机械手下方";
                else if (value == 34) return "大钩在机械手上方";
                #endregion
            }
            else if (content == 698)
            {
                if (value == 1) return "已标定";
                else return "未标定";
            }
            else if (content == 701)
            {
                if (value == 1) return "屏蔽";
                else return "未屏蔽";
            }
            else if (content == 700 || content == 699 || content == 600)
            {
                if (value == 1) return "互锁";
                else return "解除";
            }
            else if (content == 810)
            {
                if (value == 1) return "解除";
                else return "未解除";
            }
            else if (content == 1071)
            {
                if (value == 1) return "忽略";
                else return "未忽略";
            }
            return value.ToString();
        }

        private string GetRecordType(int content)
        {
            if (content == 465 || content == 505 || content == 545 || content == 463 || content == 503 || content == 543
                || (content >= 34 && content <= 42)) return "报警信息";
            else return "状态信息";
        }
        string cond = "Where LogType='1'";
        private void Button_MouseDown(object sender, RoutedEventArgs e)
        {
            string tagID = this.cbTypeSelect.SelectedValue.ToString();
            string bTime = this.beginTime.Value.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string eTime = this.endTime.Value.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string condition = string.Empty;
            if (tagID != "0") condition = string.Format("Where LogType='1'and Id = '{0}' and TimeStamp>'{1}' and TimeStamp<'{2}'", tagID, bTime, eTime);
            else condition = string.Format("Where LogType='1'and TimeStamp>'{1}' and TimeStamp<'{2}'", tagID, bTime, eTime);
            cond = condition;
            QueryRecord();
        }
        //int page = 1;
        ///// <summary>
        ///// 页面切换
        ///// </summary>
        //private void PageChange()
        //{
        //    try
        //    {
        //        this.RecordPage.Children.Clear();
        //        if (page == 1)
        //        {
        //            this.left.Visibility = Visibility.Hidden;
        //            this.right.Visibility = Visibility.Visible;
        //            this.RecordPage.Children.Add(SFRecordOne.Instance);
        //        }
        //        else if (page == 2)
        //        {
        //            this.left.Visibility = Visibility.Visible;
        //            this.right.Visibility = Visibility.Visible;
        //            this.RecordPage.Children.Add(SFRecordTwo.Instance);
        //        }
        //        else if (page == 3)
        //        {
        //            this.left.Visibility = Visibility.Visible;
        //            this.right.Visibility = Visibility.Hidden;
        //            this.RecordPage.Children.Add(SFRecordThree.Instance);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
        //    }
        //}
        ///// <summary>
        ///// 上一页
        ///// </summary>
        //private void Left_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.page -= 1;
        //    PageChange();
        //}
        ///// <summary>
        ///// 下一页
        ///// </summary>
        //private void Right_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.page += 1;
        //    PageChange();
        //}
    }

    public class RecordDate
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int OID { get; set; }
        /// <summary>
        ///  tagID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }

    public class ShowDate
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int OID { get; set; }
        /// <summary>
        ///  tagID
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string TimeStamp { get; set; }
    }
}
