using COM.Common;
using DatabaseLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.SecondFloor
{
    /// <summary>
    /// SFPositionSetting.xaml 的交互逻辑
    /// </summary>
    public partial class SFPositionSetting : UserControl
    {
        private static SFPositionSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static SFPositionSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFPositionSetting();
                        }
                    }
                }
                return _instance;
            }
        }

        public SFPositionSetting()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFPositionSetting_Loaded;
        }

        private void SFPositionSetting_Loaded(object sender, RoutedEventArgs e)
        {
            //VariableBinding();
        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            this.txtOpModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
            this.txtWorkModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
            this.txtRotateAngle_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });

            this.returnSelectParaName.SetBinding(TextBox.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["Con_Set1"], Mode = BindingMode.OneWay, Converter = new ReturnSelectParaConverter() });
            this.returnSelectParaValue.SetBinding(TextBox.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["108N23PositionCalibrationValue"], Mode = BindingMode.OneWay });
        }

        const byte bHeadFirst = 0x50;
        const byte bHeadTwo = 0x01;
        /// <summary>
        /// 参数读取
        /// </summary>
        private void btn_ParaRead_LocationCalibration(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = bHeadFirst;
            byteToSend[1] = bHeadTwo;
            byteToSend[2] = 12;
            byteToSend[3] = (byte)selectParaName.TabIndex;
            byteToSend[6] = 1;

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        private void btn_ParaWrite_LocationCalibration(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = bHeadFirst;
            byteToSend[1] = bHeadTwo;
            byteToSend[2] = 12;
            byteToSend[3] = (byte)selectParaName.TabIndex;
            byteToSend[6] = 2;

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 选择设置类型
        /// </summary>
        private void btn_SelectPara_LocationCalibration(object sender, RoutedEventArgs e)
        {
            MenuItem menuObject = (MenuItem)sender;
            if (menuObject != null)
            {
                selectParaName.Text = menuObject.Header.ToString();
                selectParaName.TabIndex = menuObject.TabIndex;
            }
        }

        BackgroundWorker bgMeet;
        List<Calibration> calList = new List<Calibration>();
        List<Calibration> calErrorList = new List<Calibration>();
        /// <summary>
        /// 一键保存标定参数
        /// </summary>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.lvCali.Items.Clear();
            this.calErrorList.Clear();
            this.calList.Clear();
            this.tbCorrect.Text = "0";
            this.tbError.Text = "0";
            string sql = "Select * from Calibration Order by ID";
            calList = DataHelper.Instance.ExecuteList<Calibration>(sql);
            this.tbTips.Text = "开始读取";
            bgMeet = new BackgroundWorker();
            //能否报告进度更新
            bgMeet.WorkerReportsProgress = true;
            bgMeet.WorkerSupportsCancellation = true;
            //要执行的后台任务
            bgMeet.DoWork += new DoWorkEventHandler(bgMeet_DoWork);
            //进度报告方法
            bgMeet.ProgressChanged += new ProgressChangedEventHandler(bgMeet_ProgressChanged);
            //后台任务执行完成时调用的方法
            bgMeet.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgMeet_RunWorkerCompleted);
            bgMeet.RunWorkerAsync(); //任务启动
        }

        //执行任务
        void bgMeet_DoWork(object sender, DoWorkEventArgs e)
        {
            int time = 0;
            ////开始播放等待动画
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.tbprocess.Text = "0/" + calList.Count;
                time = int.Parse(this.tbTime.Text);
            }));
            for (int i = 0; i < calList.Count; i++)
            {
                if (bgMeet.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    byte[] byteToSend = new byte[10];
                    byteToSend[0] = bHeadFirst;
                    byteToSend[1] = bHeadTwo;
                    byteToSend[2] = 12;
                    byteToSend[3] = (byte)calList[i].TagID;
                    byteToSend[6] = 1;

                    GlobalData.Instance.da.SendBytes(byteToSend);
                    System.Threading.Thread.Sleep(time);
                    bgMeet.ReportProgress(i+1, calList[i]);
                }
            }
        }
        //报告任务进度
        void bgMeet_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //item.Value = "1";
            //Thread.Sleep(1000);
            //this.lvCali.Items.Add(item);
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (e.ProgressPercentage > 0)
                {
                    this.tbprocess.Text = e.ProgressPercentage + "/" + calList.Count;
                    if (e.UserState is Calibration)
                    {
                        Calibration cali = e.UserState as Calibration;
                        cali.Value = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32.ToString();
                        cali.Test = "待检测";
                        this.lvCali.Items.Add(cali);
                        //string sql = string.Format("Update Calibration Set Value='{0}' Where ID='{1}'", cali.Value, cali.ID);
                        //DataHelper.Instance.ExecuteNonQuery(sql);
                    }
                }
            }));
        }
        //任务执行完成后更新状态
        void bgMeet_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.tbTips.Text = "读取完成，开始检测";
            }));

            bgMeet = new BackgroundWorker();
            //能否报告进度更新
            bgMeet.WorkerReportsProgress = true;
            bgMeet.WorkerSupportsCancellation = true;
            //要执行的后台任务
            bgMeet.DoWork += new DoWorkEventHandler(bgMeet_DoTestWork);
            //进度报告方法
            bgMeet.ProgressChanged += new ProgressChangedEventHandler(bgMeet_ProgressTestChanged);
            //后台任务执行完成时调用的方法
            bgMeet.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgMeet_RunTestWorkerCompleted);
            bgMeet.RunWorkerAsync(); //任务启动
        }


        //执行任务
        void bgMeet_DoTestWork(object sender, DoWorkEventArgs e)
        {
            int time = 0;
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.lvCali.Items.Clear();
                time = int.Parse(this.tbTime.Text);
            }));
            for (int i = -1; i < calList.Count; i++)
            {
                if (bgMeet.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    int nowID = 0;
                    if (i == -1) nowID = 0;
                    else nowID = i;
                    byte[] byteToSend = new byte[10];
                    byteToSend[0] = bHeadFirst;
                    byteToSend[1] = bHeadTwo;
                    byteToSend[2] = 12;
                    byteToSend[3] = (byte)calList[nowID].TagID;
                    byteToSend[6] = 1;

                    GlobalData.Instance.da.SendBytes(byteToSend);
                    System.Threading.Thread.Sleep(time);
                    bgMeet.ReportProgress(i + 1, calList[nowID]);
                }
            }
        }
        //报告任务进度
        void bgMeet_ProgressTestChanged(object sender, ProgressChangedEventArgs e)
        {
            //item.Value = "1";
            //Thread.Sleep(1000);
            //this.lvCali.Items.Add(item);
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (e.ProgressPercentage > 0)
                {
                    if (e.UserState is Calibration)
                    {
                        Calibration cali = e.UserState as Calibration;
                        if (GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32.ToString() == cali.Value)
                        {
                            cali.Test = "检测完成";
                            string sql = string.Format("Update Calibration Set Value='{0}' Where ID='{1}'", cali.Value, cali.ID);
                            DataHelper.Instance.ExecuteNonQuery(sql);
                            this.tbCorrect.Text = (int.Parse(this.tbCorrect.Text) + 1).ToString();
                        }
                        else
                        {
                            cali.Test = "检测失败";
                            this.calErrorList.Add(cali);
                            this.tbError.Text = (int.Parse(this.tbError.Text) + 1).ToString();
                        }
                        this.lvCali.Items.Add(cali);
                    }
                }
            }));
        }
        //任务执行完成后更新状态
        void bgMeet_RunTestWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (this.calErrorList.Count > 0)
                {
                    this.tbTips.Text = "数据检测出现不一致，请重新读取";
                }
                else
                {
                    this.tbTips.Text = "完成";
                }
            }));
        }

        /// <summary>
        ///  查找子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var list = this.calList;
        }

        private void tb_ParameterConfig_Focus(object sender, MouseButtonEventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }
    }

    public class Calibration
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 父参数名
        /// </summary>
        public string Parent { get; set; }
        /// <summary>
        /// 标志ID
        /// </summary>
        public int TagID { get; set; }

        public string Test { get; set; }
    }
}
