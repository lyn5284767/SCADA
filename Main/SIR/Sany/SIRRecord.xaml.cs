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

namespace Main.SIR.Sany
{
    /// <summary>
    /// SIRRecord.xaml 的交互逻辑
    /// </summary>
    public partial class SIRRecord : UserControl
    {
        private static SIRRecord _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRRecord Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRRecord();
                        }
                    }
                }
                return _instance;
            }
        }
        Dictionary<int, string> dicNumToValue = new Dictionary<int, string>();
        BackgroundWorker bgMeet;
        public SIRRecord()
        {
            InitializeComponent();
            Init();
            this.Loaded += SIRRecord_Loaded; ;
        }

        private void SIRRecord_Loaded(object sender, RoutedEventArgs e)
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
            dicNumToValue.Add(6, "卸扣扭矩");
            dicNumToValue.Add(7, "套管扭矩");

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
            string sql = string.Format("SELECT  * FROM DateBaseReport {0} Order by OID DESC LIMIT 0,1000", cond);
            List<DateBaseReport> recordList = DataHelper.Instance.ExecuteList<DateBaseReport>(sql);
            List<ShowDate> showList = new List<ShowDate>();
            if (recordList != null)
            {
                foreach (DateBaseReport date in recordList)
                {
                    ShowDate showDate = new ShowDate();
                    showDate.OID = date.ID;
                    showDate.Content = date.Name;
                    showDate.Value = date.Value;
                    showDate.TimeStamp = date.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    showDate.StandardValue = date.StandardValue;
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

        string cond = "Where Type in (6,7)";

        private void Button_MouseDown(object sender, RoutedEventArgs e)
        {
            string tagID = this.cbTypeSelect.SelectedValue.ToString();
            string bTime = this.beginTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string eTime = this.endTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string condition = string.Empty;
            if (tagID != "0") condition = string.Format("Where Type = '{0}' and CreateTime>'{1}' and CreateTime<'{2}'", tagID, bTime, eTime);
            else condition = string.Format("Where Type in (6,7) and CreateTime>'{0}' and CreateTime<'{1}'", bTime, eTime);
            cond = condition;
            QueryRecord();
        }

        private void endTime_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.endTime.SelectedDate <= this.beginTime.SelectedDate)
            {
                MessageBox.Show("结束日期必须大于开始日期");
            }
        }
    }
}
