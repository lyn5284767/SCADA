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

namespace Main.SIR
{
    /// <summary>
    /// SIRSelfRecord.xaml 的交互逻辑
    /// </summary>
    public partial class SIRSelfRecord : UserControl
    {
        private static SIRSelfRecord _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRSelfRecord Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRSelfRecord();
                        }
                    }
                }
                return _instance;
            }
        }
        Dictionary<int, string> dicNumToValue = new Dictionary<int, string>();
        BackgroundWorker bgMeet;
        public SIRSelfRecord()
        {
            InitializeComponent();
            this.VariableBinding();
            Init();
            this.Loaded += SIRSelfRecord_Loaded;
        }

        private void VariableBinding()
        {
            this.tbMainOneTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["M1RunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbMainTwoTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["M2RunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbConstantVoltage.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["CRunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbDissipateHeat.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["DRunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
        }

        private void SIRSelfRecord_Loaded(object sender, RoutedEventArgs e)
        {
            //QueryRecord();
            this.beginTime.SelectedDate = DateTime.Now.AddDays(-1);
            this.endTime.SelectedDate = DateTime.Now.AddDays(1);
        }

        /// <summary>
        /// 初始化查询类型
        /// </summary>
        public void Init()
        {
            dicNumToValue.Add(0, "全部");
            dicNumToValue.Add(1, "系统压力");
            dicNumToValue.Add(2, "油温");
            dicNumToValue.Add(3, "液位");

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

        string cond = "";

        private void Button_MouseDown(object sender, RoutedEventArgs e)
        {
            string tagID = this.cbTypeSelect.SelectedValue.ToString();
            string bTime = this.beginTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string eTime = this.endTime.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string condition = string.Empty;
            if (tagID != "0") condition = string.Format("Where Type = '{0}' and CreateTime>'{1}' and CreateTime<'{2}'", tagID, bTime, eTime);
            else condition = string.Format("Where CreateTime>'{0}' and CreateTime<'{1}'", bTime, eTime);
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
