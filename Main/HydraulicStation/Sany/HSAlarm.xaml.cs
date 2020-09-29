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

namespace Main.HydraulicStation
{
    /// <summary>
    /// HSAlarm.xaml 的交互逻辑
    /// </summary>
    public partial class HSAlarm : UserControl
    {
        private static HSAlarm _instance = null;
        private static readonly object syncRoot = new object();

        public static HSAlarm Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new HSAlarm();
                        }
                    }
                }
                return _instance;
            }
        }
        Dictionary<int, string> dicNumToValue = new Dictionary<int, string>();
        BackgroundWorker bgMeet;
        public HSAlarm()
        {
            InitializeComponent();
            Init();
            this.Loaded += HSAlarm_Loaded;
        }

        private void HSAlarm_Loaded(object sender, RoutedEventArgs e)
        {
            QueryRecord();
            this.beginTime.Value = DateTime.Now.AddDays(-1);
            this.endTime.Value = DateTime.Now.AddDays(1);
        }

        public void Init()
        {
            dicNumToValue.Add(0, "全部");
            dicNumToValue.Add(1208, "高温报警");
            dicNumToValue.Add(1209, "高温预警");
            dicNumToValue.Add(1210, "低温报警");
            dicNumToValue.Add(1211, "低液位预警");
            dicNumToValue.Add(1212, "低液位报警");
            dicNumToValue.Add(1229, "液位异常降低报警");
            dicNumToValue.Add(1222, "加热效果异常");
            dicNumToValue.Add(1215, "主泵1已连续运行500小时，请切换主泵2使用");
            dicNumToValue.Add(1216, "主泵2已连续运行500小时，请切换主泵1使用");
            dicNumToValue.Add(1217, "主电机1已连续运行600小时，请加注黄油");
            dicNumToValue.Add(1218, "主电机2已连续运行600小时，请加注黄油");
            dicNumToValue.Add(1219, "距上次更换滤芯已经大于2000小时，请更换滤芯");
            dicNumToValue.Add(1220, "距上次更换液压油已经大于2000小时，请更换液压油");
            dicNumToValue.Add(1221, "油位下降异常，请检查是否漏油");

            this.cbTypeSelect.ItemsSource = dicNumToValue;
            this.cbTypeSelect.SelectedValuePath = "Key";
            this.cbTypeSelect.DisplayMemberPath = "Value";
            this.cbTypeSelect.SelectedIndex = 0;
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
                    showDate.TimeStamp = date.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss");
                    showList.Add(showDate);
                }
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

        private string GetValue(int content, int value)
        {
            if (value == 1) return "报警";
            else return "消除";
        }

        string cond = "Where LogType='2'";

        private void Button_MouseDown(object sender, RoutedEventArgs e)
        {
            string tagID = this.cbTypeSelect.SelectedValue.ToString();
            string bTime = this.beginTime.Value.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string eTime = this.endTime.Value.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string condition = string.Empty;
            if (tagID != "0") condition = string.Format("Where LogType='2'and Id = '{0}' and TimeStamp>'{1}' and TimeStamp<'{2}'", tagID, bTime, eTime);
            else condition = string.Format("Where LogType='2'and TimeStamp>'{1}' and TimeStamp<'{2}'", tagID, bTime, eTime);
            cond = condition;
            QueryRecord();
        }
    }
}
