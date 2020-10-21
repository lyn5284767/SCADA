using COM.Common;
using ControlLibrary;
using DatabaseLib;
using HandyControl.Controls;
using HBGKTest;
using HBGKTest.YiTongCamera;
using LiveCharts;
using LiveCharts.Wpf;
using Main.SecondFloor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.SIR
{
    /// <summary>
    /// SIRSelfMain.xaml 的交互逻辑
    /// </summary>
    public partial class SIRSelfMain : UserControl
    {
        private static SIRSelfMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRSelfMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRSelfMain();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        System.Threading.Timer VariableReBinding;
        System.Threading.Timer ReportTimer;
        byte bworkModel = 0;
        bool workModelCheck = false;
        public SIRSelfMain()
        {
            InitializeComponent();
            InitControls();
            InitCameraInfo();
            VariableBinding();
            //InitReport();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟
            VariableReBinding = new System.Threading.Timer(new TimerCallback(VariableTimer), null, Timeout.Infinite, 500);
            VariableReBinding.Change(0, 500);
            ReportTimer = new System.Threading.Timer(new TimerCallback(ReportTimer_Elapse), null, 500, 2000);
            this.Loaded += SIRSelfMain_Loaded;
        }

        private void SIRSelfMain_Loaded(object sender, RoutedEventArgs e)
        {
            PlayCameraInThread();
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.oprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfOperationModelConverter() });
                this.oprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                this.workModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                this.workModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                this.PipeTypeModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeModelConverter() });
                this.PipeTypeModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeIsCheckConverter() });
                this.locationModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocationModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocationModelConverter() });
                this.locationModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocationModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });

                //this.tbOneKeyInButton.SetBinding(ToggleButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOneKeyInButton"], Mode = BindingMode.OneWay, Converter = new SIRSelfTwoToCheckConverter() });
                //this.tbOneKeyOutButton.SetBinding(ToggleButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOneKeyOutButton"], Mode = BindingMode.OneWay, Converter = new SIRSelfTwoToCheckConverter() });

                this.smMainGapOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMainGapTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBackGapOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInButtonPosOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInButtonPosTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOutButtonPosOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOutButtonPosTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSafeDoorReset.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["842b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSafeDoorClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["842b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.tbControlModel.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbLoaction.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbOperModel.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbWorkModel.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbSafeDoor.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbGap.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.tbTongsGap.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfClampStatus"], Mode = BindingMode.OneWay, Converter = new SIRSelfTongsGapConverter() });
                //// 上扣压力                                                                                                                                          
                //this.cpbInButtonPress.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.cpbInButtonPress.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //MultiBinding cpbInButtonPressMultiBind = new MultiBinding();
                //cpbInButtonPressMultiBind.Converter = new ColorCoverter();
                //cpbInButtonPressMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonPress"], Mode = BindingMode.OneWay });
                //cpbInButtonPressMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbInButtonPress, Mode = BindingMode.OneWay });
                //cpbInButtonPressMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbInButtonPress, Mode = BindingMode.OneWay });
                //cpbInButtonPressMultiBind.NotifyOnSourceUpdated = true;
                //this.cpbInButtonPress.SetBinding(CircleProgressBar.ForegroundProperty, cpbInButtonPressMultiBind);
                //// 卸扣压力
                //this.cpbOutButtonPress.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.cpbOutButtonPress.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //MultiBinding cpbOutButtonPressMultiBind = new MultiBinding();
                //cpbOutButtonPressMultiBind.Converter = new ColorCoverter();
                //cpbOutButtonPressMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonPress"], Mode = BindingMode.OneWay });
                //cpbOutButtonPressMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbOutButtonPress, Mode = BindingMode.OneWay });
                //cpbOutButtonPressMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbOutButtonPress, Mode = BindingMode.OneWay });
                //cpbOutButtonPressMultiBind.NotifyOnSourceUpdated = true;
                //this.cpbOutButtonPress.SetBinding(CircleProgressBar.ForegroundProperty, cpbOutButtonPressMultiBind);
                //// 上扣背钳加紧
                //this.cpbInButtonClampingForce.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.cpbInButtonClampingForce.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //MultiBinding cpbInButtonClampingForceMultiBind = new MultiBinding();
                //cpbInButtonClampingForceMultiBind.Converter = new ColorCoverter();
                //cpbInButtonClampingForceMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonClampingForce"], Mode = BindingMode.OneWay });
                //cpbInButtonClampingForceMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbInButtonClampingForce, Mode = BindingMode.OneWay });
                //cpbInButtonClampingForceMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbInButtonClampingForce, Mode = BindingMode.OneWay });
                //cpbInButtonClampingForceMultiBind.NotifyOnSourceUpdated = true;
                //this.cpbInButtonClampingForce.SetBinding(CircleProgressBar.ForegroundProperty, cpbInButtonClampingForceMultiBind);
                //// 卸扣背钳加紧
                //this.cpbOutButtonClampingForce.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.cpbOutButtonClampingForce.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //MultiBinding cpbOutButtonClampingForceMultiBind = new MultiBinding();
                //cpbOutButtonClampingForceMultiBind.Converter = new ColorCoverter();
                //cpbOutButtonClampingForceMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonClampingForce"], Mode = BindingMode.OneWay });
                //cpbOutButtonClampingForceMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbOutButtonClampingForce, Mode = BindingMode.OneWay });
                //cpbOutButtonClampingForceMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbOutButtonClampingForce, Mode = BindingMode.OneWay });
                //cpbOutButtonClampingForceMultiBind.NotifyOnSourceUpdated = true;
                //this.cpbOutButtonClampingForce.SetBinding(CircleProgressBar.ForegroundProperty, cpbOutButtonClampingForceMultiBind);

                this.tbInButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbInButtonClampingForce.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonClampingForce.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbRotate.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfRotateEncodePulse"], Mode = BindingMode.OneWay ,Converter= new SIRSelfRotateConverter ()});
                this.tbSysPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSysPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

                this.tbArmPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfArmPos"], Mode = BindingMode.OneWay,Converter= new TakeTenConverter() });
                this.tbClampHeight.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfClampHeight"], Mode = BindingMode.OneWay, Converter = new TakeTenConverter() });
                this.tbWorkCylinderPress.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkCylinderPress"], Mode = BindingMode.OneWay, Converter = new DivideHundredConverter() });
                this.tbBrakingCylinderPress.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrakingCylinderPress"], Mode = BindingMode.OneWay, Converter = new DivideHundredConverter() });
               //改为曲线显示
                //this.tbInButtonTorque.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfInButtonTorque"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.tbOutButtonTorque.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonTorque"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbInButtonCircle.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonCircle"], Mode = BindingMode.OneWay });
                this.tbOutButtonCircle.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonCircle"], Mode = BindingMode.OneWay });
                this.tbWorkTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfWorkTime"], Mode = BindingMode.OneWay });

                // 一键上扣
                MultiBinding sbInbuttonMultiBind = new MultiBinding();
                sbInbuttonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.ConverterParameter = "inButton";
                sbInbuttonMultiBind.NotifyOnSourceUpdated = true;
                this.sbInButton.SetBinding(StepBar.StepIndexProperty, sbInbuttonMultiBind);
                // 一键卸扣
                MultiBinding sbOutButtonMultiBind = new MultiBinding();
                sbOutButtonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.ConverterParameter = "outButton";
                sbOutButtonMultiBind.NotifyOnSourceUpdated = true;
                this.sbOutButton.SetBinding(StepBar.StepIndexProperty, sbOutButtonMultiBind);


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_oprModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.oprModel.IsChecked) //当前手动状态
            {
                byteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前自动状态
            {
                byteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_workModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.workModel.IsChecked) //当前上扣模式
            {
                byteToSend = new byte[10] { 23, 17, 2, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前卸扣模式
            {
                   byteToSend = new byte[10] { 23, 17, 2, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
            this.workModel.ContentDown = "切换中";
            workModelCheck = true;
        }
        /// <summary>
        /// 管柱选择
        /// </summary>
        private void btn_PipeTypeModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.PipeTypeModel.IsChecked) //当前钻杠
            {
                byteToSend = new byte[10] { 23, 17, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前套管
            {
                byteToSend = new byte[10] { 23, 17, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工位选择
        /// </summary>
        private void btn_locationModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.locationModel.IsChecked) //当前井口
            {
                byteToSend = new byte[10] { 23, 17, 4, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前鼠洞
            {
                byteToSend = new byte[10] { 23, 17, 4, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 一键上扣
        /// </summary>
        private void OneKeyInButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tbn = sender as ToggleButton;
            byte[] byteToSend;
            if (tbn.IsChecked.Value) // 当前一键上扣
            {
                byteToSend = new byte[10] { 23, 17, 5, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 23, 17, 5, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 一键卸扣
        /// </summary>
        private void tbOneKeyOutButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tbn = sender as ToggleButton;
            byte[] byteToSend;
            if (tbn.IsChecked.Value) // 当前一键卸扣
            {
                byteToSend = new byte[10] { 23, 17, 6, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 23, 17, 6, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        private void VariableTimer(object value)
        {
            if (bworkModel != GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte && workModelCheck)
            {
                this.workModel.Dispatcher.Invoke(new Action(() =>
                {        
                    this.workModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                }));
                workModelCheck = false;
            }
            bworkModel = GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte;
        }
        #region 扭矩曲线
        private int SaveCount = 20;
        /// <summary>
        /// 钻杆扭矩
        /// </summary>
        public SeriesCollection DrillTorqueSeries { get; set; }
        /// <summary>
        /// 钻杆扭矩时间
        /// </summary>
        public List<string> DrillTorqueLabels { get; set; }
        /// <summary>
        /// 钻杆扭矩
        /// </summary>
        public SeriesCollection CasingTorqueSeries { get; set; }
        /// <summary>
        /// 钻杆扭矩时间
        /// </summary>
        public List<string> CasingTorqueLabels { get; set; }
        private void InitReport()
        {
            LineSeries drillTorqueLine = new LineSeries();
            //drillTorqueLine.LineSmoothness = 0;
            //drillTorqueLine.PointGeometry = null;
            DrillTorqueLabels = new List<string> ();
            drillTorqueLine.Values = new ChartValues<double>();
            DrillTorqueSeries = new SeriesCollection { };
            DrillTorqueSeries.Add(drillTorqueLine);

            LineSeries casingTorqueLine = new LineSeries();
            //drillTorqueLine.LineSmoothness = 0;
            //drillTorqueLine.PointGeometry = null;
            CasingTorqueLabels = new List<string>();
            casingTorqueLine.Values = new ChartValues<double>();
            CasingTorqueSeries = new SeriesCollection { };
            CasingTorqueSeries.Add(casingTorqueLine);
            DataContext = this;
        }
        List<double> data = new List<double>();
        List<double> datad = new List<double>();
        /// <summary>
        /// 扭矩定时器
        /// </summary>
        /// <param name="value"></param>
        private void ReportTimer_Elapse(object value)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    double drillTore = GlobalData.Instance.da["SIRSelfInButtonTorque"].Value.Int32 / 10.0;
                    double cosingTorque = GlobalData.Instance.da["SIRSelfOutButtonTorque"].Value.Int32 / 10.0;
                    this.drillTorqueChart.AddPoints(drillTore);
                    this.cosingTorqueChart.AddPoints(cosingTorque);
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        #endregion
        private int iTimeCnt = 0;//用来为时钟计数的变量
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Warnning();
                    this.Communcation();
                    if (GlobalData.Instance.da["SIRSelfOperModel"].Value.Byte == 2) // 自动模式
                    {
                        if (GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 1)//一键上扣开启
                        {
                            this.spOneKeyInbutton.Visibility = Visibility.Visible;
                            this.spOneKeyOutButton.Visibility = Visibility.Collapsed;
                        }
                        else // 一键卸扣
                        {
                            this.spOneKeyInbutton.Visibility = Visibility.Collapsed;
                            this.spOneKeyOutButton.Visibility = Visibility.Visible;
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 融信数据
        /// </summary>
        private void Communcation()
        {
            #region 通信

            if (!GlobalData.Instance.ComunciationNormal) this.tbTips.Text = "网络连接失败！";
            #endregion
        }
        Dictionary<string, byte> WarnInfo = new Dictionary<string, byte>();
        private void Warnning()
        {
            try
            {

                byte alarmTips = GlobalData.Instance.da["SIRSelfAlarm"].Value.Byte;
                if (iTimeCnt % 10 != 0)
                {
                    switch (alarmTips)
                    {
                        case 120:
                            this.tbTips.Text = "工作缸气源气压过低告警";
                            break;
                        case 121:
                            this.tbTips.Text = "制动缸气源气压过低告警";
                            break;
                        case 1:
                            this.tbTips.Text = "背钳复位故障";
                            break;
                        case 102:
                            this.tbTips.Text = "工况选择错误";
                            break;
                        case 61:
                            this.tbTips.Text = "背钳夹紧故障";
                            break;
                        case 2:
                            this.tbTips.Text = "为达到上扣扭矩";
                            break;
                        case 3:
                            this.tbTips.Text = "未达到上扣圈数";
                            break;
                        case 4:
                            this.tbTips.Text = "未达到紧扣扭矩";
                            break;
                        case 5:
                            this.tbTips.Text = "未检测到背钳复位信号";
                            break;
                        case 6:
                            this.tbTips.Text = "未检测到安全门打开信号";
                            break;
                        case 12:
                            this.tbTips.Text = "钻杆未卸开";
                            break;
                        case 13:
                            this.tbTips.Text = "钻杆打滑或空杠卸扣";
                            break;
                        case 21:
                            this.tbTips.Text = "系统压力故障";
                            break;
                        case 70:
                            this.tbTips.Text = "当前手臂伸缩运动停止";
                            break;
                        case 71:
                            this.tbTips.Text = "当前钳体升降运动停止";
                            break;
                        case 75:
                            this.tbTips.Text = "当前钳体旋转运动停止";
                            break;
                    }
                }
                else
                {
                    //tbTips.Visibility = Visibility.Hidden;
                    tbTips.Text = "";
                }
                byte oprTips = GlobalData.Instance.da["SIRSelfOprInfo"].Value.Byte;
                switch (oprTips)
                {
                    case 60:
                        this.tbOprTips.Text = "工况选择结束后，请执行上扣对缺确认";
                        break;
                    case 61:
                        this.tbOprTips.Text = "工况选择结束后，请执行卸扣对缺确认";
                        break;
                    case 50:
                        this.tbOprTips.Text = "进入井口工位后切换自动模式";
                        break;
                    case 51:
                        this.tbOprTips.Text = "进入井口工位后切换自动模式";
                        break;
                    case 20:
                        this.tbOprTips.Text = "请检测钳头缺口状态";
                        break;
                    case 30:
                        this.tbOprTips.Text = "请确认安全门打开状态";
                        break;
                    case 29:
                        this.tbOprTips.Text = "请确认接箍高度";
                        break;
                    case 32:
                        this.tbOprTips.Text = "请确认井口安全";
                        break;
                    case 33:
                        this.tbOprTips.Text = "请将钳体缺口对正";
                        break;
                    case 34:
                        this.tbOprTips.Text = "请人工确认缺口状态";
                        break;
                    case 35:
                        this.tbOprTips.Text = "请人工确认安全门状态";
                        break;
                    case 101:
                        this.tbOprTips.Text = "请注意紧急停止";
                        break;
                    case 99:
                        this.tbOprTips.Text = "请退出自动模式检测故障";
                        break;
                    default:
                        this.tbOprTips.Text = "";
                        break;
                }
            
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.DEBUG);
            }
        }

        #region 摄像头操作
        System.Threading.Timer cameraSaveThreadTimer1; //摄像头录像线程
        System.Threading.Timer cameraSaveThreadTimer2;
        private string configPath = System.Environment.CurrentDirectory + @"\Config.ini";
        const int STRINGMAX = 255;

        Image cameraInitImage1 = new Image();
        Image cameraInitImage2 = new Image();
        /// <summary>
        /// 初始化摄像头信息
        /// </summary>
        private void InitCameraInfo()
        {
            //ChannelInfo info5 = GetConfigPara("CAMERA5");
            //ChannelInfo info6 = GetConfigPara("CAMERA6");
            //if (info5 != null)
            //{
            //    info5.ID = 5;
            //    GlobalData.Instance.chList.Add(info5);
            //}
            //if (info6 != null)
            //{
            //    info6.ID = 6;
            //    GlobalData.Instance.chList.Add(info6);
            //}
            //foreach (ChannelInfo info in GlobalData.Instance.chList)
            //{
            //    switch (info.CameraType)
            //    {
            //        case 0:
            //            {
            //                GlobalData.Instance.cameraList.Add(new UIControl_HBGK1(info));
            //                break;
            //            }
            //        case 1:
            //            {
            //                GlobalData.Instance.cameraList.Add(new YiTongCameraControl(info));
            //                break;
            //            }
            //    }
            //}
            InitCameraSaveTimeThread();
        }
        /// <summary>
        /// 初始化摄像头录像线程
        /// </summary>
        private void InitCameraSaveTimeThread()
        {
            cameraSaveThreadTimer1 = new System.Threading.Timer(new TimerCallback(CameraVideoSave1), null, Timeout.Infinite, 60000);
            cameraSaveThreadTimer2 = new System.Threading.Timer(new TimerCallback(CameraVideoSave2), null, Timeout.Infinite, 60000);
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns></returns>
        private ChannelInfo GetConfigPara(string cameraTag)
        {
            try
            {
                if (System.IO.File.Exists(configPath))
                {
                    StringBuilder sb = new StringBuilder(STRINGMAX);
                    string strChlID = "0";
                    string strNDeviceType = "0";
                    string strRemoteChannle = "0";
                    string strRemoteIP = "0.0.0.0";
                    string strRemotePort = "0";
                    string strRemoteUser = "0";
                    string strRemotePwd = "0";
                    string strNPlayPort = "0";
                    string strPtzPort = "0";
                    string strCameraType = "0";
                    ChannelInfo ch1 = new ChannelInfo();
                    WinAPI.GetPrivateProfileString(cameraTag, "CHLID", strChlID, sb, STRINGMAX, configPath);
                    strChlID = sb.ToString();
                    ch1.ChlID = strChlID;

                    WinAPI.GetPrivateProfileString(cameraTag, "NDEVICETYPE", strNDeviceType, sb, STRINGMAX, configPath);
                    strNDeviceType = sb.ToString();
                    int.TryParse(strNDeviceType, out ch1.nDeviceType);

                    WinAPI.GetPrivateProfileString(cameraTag, "REMOTECHANNLE", strRemoteChannle, sb, STRINGMAX, configPath);
                    strRemoteChannle = sb.ToString();
                    ch1.RemoteChannle = strRemoteChannle;

                    WinAPI.GetPrivateProfileString(cameraTag, "REMOTEIP", strRemoteIP, sb, STRINGMAX, configPath);
                    strRemoteIP = sb.ToString();
                    ch1.RemoteIP = strRemoteIP;

                    WinAPI.GetPrivateProfileString(cameraTag, "REMOTEPORT", strRemotePort, sb, STRINGMAX, configPath);
                    strRemotePort = sb.ToString();
                    int tmpRemotePort = 0;
                    int.TryParse(strRemotePort, out tmpRemotePort);
                    ch1.RemotePort = tmpRemotePort;

                    WinAPI.GetPrivateProfileString(cameraTag, "REMOTEUSER", strRemoteUser, sb, STRINGMAX, configPath);
                    strRemoteUser = sb.ToString();
                    ch1.RemoteUser = strRemoteUser;

                    WinAPI.GetPrivateProfileString(cameraTag, "REMOTEPWD", strRemotePwd, sb, STRINGMAX, configPath);
                    strRemotePwd = sb.ToString();
                    ch1.RemotePwd = strRemotePwd;

                    WinAPI.GetPrivateProfileString(cameraTag, "NPLAYPORT", strNPlayPort, sb, STRINGMAX, configPath);
                    strNPlayPort = sb.ToString();
                    int tmpNPlayPort = 0;
                    int.TryParse(strNPlayPort, out tmpNPlayPort);
                    ch1.nPlayPort = tmpNPlayPort;

                    WinAPI.GetPrivateProfileString(cameraTag, "PTZPORT", strPtzPort, sb, STRINGMAX, configPath);
                    strPtzPort = sb.ToString();
                    int tmpPtzPort = 0;
                    int.TryParse(strPtzPort, out tmpPtzPort);
                    ch1.PtzPort = tmpPtzPort;

                    WinAPI.GetPrivateProfileString(cameraTag, "CAMERATYPE", strCameraType, sb, STRINGMAX, configPath);
                    strCameraType = sb.ToString();
                    int cameraType = 0;
                    int.TryParse(strCameraType, out cameraType);
                    ch1.CameraType = cameraType;

                    return ch1;//正常返回。
                }
                else
                {
                    return null;//配置文件不存在
                }
            }
            catch (Exception e)
            {
                DataHelper.AddErrorLog(e);
                return null;//出现异常情况
            }
        }
        /// <summary>
        /// 摄像头1播放
        /// </summary>
        private void Button_CameraStart(object sender, RoutedEventArgs e)
        {
            gridCamera1.Children.Clear();
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 1).FirstOrDefault();
            CameraVideoStop1();
            ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 1).FirstOrDefault();
            cameraOne.InitCamera(info);
            CameraVideoStart1();
            cameraOne.SetSize(300, 400);
            if (cameraOne is UIControl_HBGK1)
            {
                gridCamera1.Children.Add(cameraOne as UIControl_HBGK1);
                //(cameraOne as UIControl_HBGK1).SetValue(Grid.RowProperty, 0);
                //(cameraOne as UIControl_HBGK1).SetValue(Grid.ColumnProperty, 0);
            }
            else if (cameraOne is YiTongCameraControl)
            {
                gridCamera1.Children.Add(cameraOne as YiTongCameraControl);
                //(cameraOne as YiTongCameraControl).SetValue(Grid.RowProperty, 0);
                //(cameraOne as YiTongCameraControl).SetValue(Grid.ColumnProperty, 0);
            }
            else
            {
                gridCamera1.Children.Add(cameraInitImage1);
            }
            viewboxCameral1.Height = 300;
            viewboxCameral1.Width = 403;
        }  

        private void CameraVideoStop1()
        {
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 5).FirstOrDefault();
            cameraOne.StopCamera();
            cameraSaveThreadTimer1.Change(Timeout.Infinite, 60000);
        }

        private void CameraVideoStart1()
        {
            cameraSaveThreadTimer1.Change(0, 60000);
        }
        private void CameraVideoStop2()
        {
            ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 6).FirstOrDefault();
            cameraTwo.StopCamera();
            cameraSaveThreadTimer2.Change(Timeout.Infinite, 60000);
        }

        private void CameraVideoStart2()
        {
            cameraSaveThreadTimer2.Change(0, 60000);
        }

        private void CameraVideoSave1(object value)
        {
            try
            {
                string str1 = System.Environment.CurrentDirectory;
                string filePath = str1 + "\\video" + "\\video5";
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
                ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 5).FirstOrDefault();
                cameraOne.StopFile();
                cameraOne.SaveFile(filePath, fileName);
                DeleteOldFileName(filePath);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
            }
        }

        private void CameraVideoSave2(object value)
        {
            try
            {
                string str1 = System.Environment.CurrentDirectory;
                string filePath = str1 + "\\video" + "\\video6";
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
                ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 6).FirstOrDefault();
                cameraOne.StopFile();
                cameraOne.SaveFile(filePath, fileName);
                DeleteOldFileName(filePath);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
            }
        }


        /// <summary>
        /// 删除最老的视频文件
        /// </summary>
        /// <param name="path"></param>
        private void DeleteOldFileName(string path)
        {
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            string[] disk = path.Split('\\');
            // 硬盘空间小于1G，开始清理录像
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == disk[0] + "\\" && drive.TotalFreeSpace / (1024 * 1024) < 1024 * 2)
                {
                    DirectoryInfo root = new DirectoryInfo(path);
                    if (root.GetFiles().Count() > 10)
                    {
                        List<FileInfo> fileList = root.GetFiles().OrderBy(s => s.CreationTime).Take(10).ToList();
                        foreach (FileInfo file in fileList)
                        {
                            file.Delete();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 全屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_CameraFullScreen(object sender, RoutedEventArgs e)
        {
            //if (FullScreenEvent != null)
            //{
            //    FullScreenEvent();
            //}
        }

        /// <summary>
        /// 跨线程调用播放视频
        /// </summary>
        public void PlayCamera()
        {
            this.gridCamera1.Dispatcher.Invoke(new PayOneDelegate(PlayOneAction), null);
            this.gridCamera2.Dispatcher.Invoke(new PayTwoDelegate(PlayTwoAction), null);
        }

        private delegate void PayOneDelegate();
        private delegate void PayTwoDelegate();
        private void PlayOneAction()
        {
            SIRCameraFullScreen.Instance.gridCamera1.Children.Clear();
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 5).FirstOrDefault();
            CameraVideoStop1();
            ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 5).FirstOrDefault();
            bool isPlay = cameraOne.InitCamera(info);
            CameraVideoStart1();
            cameraOne.SetSize(220, 370);
            if (isPlay)
            {
                gridCamera1.Children.Clear();
                if (cameraOne is UIControl_HBGK1)
                {
                    gridCamera1.Children.Add(cameraOne as UIControl_HBGK1);
                    //(cameraOne as UIControl_HBGK1).SetValue(Grid.RowProperty, 0);
                    //(cameraOne as UIControl_HBGK1).SetValue(Grid.ColumnProperty, 0);
                }
                else if (cameraOne is YiTongCameraControl)
                {
                    gridCamera1.Children.Add(cameraOne as YiTongCameraControl);
                    //(cameraOne as YiTongCameraControl).SetValue(Grid.RowProperty, 0);
                    //(cameraOne as YiTongCameraControl).SetValue(Grid.ColumnProperty, 0);
                }
                else
                {
                    gridCamera1.Children.Add(cameraInitImage1);
                }
            }
            cameraOne.FullScreenEvent -= CameraOne_FullScreenEvent;
            cameraOne.FullScreenEvent += CameraOne_FullScreenEvent;
            //viewboxCameral1.Height = 300;
            //viewboxCameral1.Width = 403;
        }

        private void PlayTwoAction()
        {
            ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 6).FirstOrDefault();
            CameraVideoStop2();
            ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 6).FirstOrDefault();
            bool isPlay = cameraTwo.InitCamera(info);
            CameraVideoStart2();
            cameraTwo.SetSize(220, 370);
            if (isPlay)
            {
                gridCamera2.Children.Clear();
                if (cameraTwo is UIControl_HBGK1)
                {
                    gridCamera2.Children.Add(cameraTwo as UIControl_HBGK1);
                    //(cameraOne as UIControl_HBGK1).SetValue(Grid.RowProperty, 0);
                    //(cameraOne as UIControl_HBGK1).SetValue(Grid.ColumnProperty, 0);
                }
                else if (cameraTwo is YiTongCameraControl)
                {
                    gridCamera2.Children.Add(cameraTwo as YiTongCameraControl);
                    //(cameraOne as YiTongCameraControl).SetValue(Grid.RowProperty, 0);
                    //(cameraOne as YiTongCameraControl).SetValue(Grid.ColumnProperty, 0);
                }
                else
                {
                    gridCamera2.Children.Add(cameraInitImage1);
                }
            }
            cameraTwo.FullScreenEvent -= CameraTwo_FullScreenEvent;
            cameraTwo.FullScreenEvent += CameraTwo_FullScreenEvent;
            //viewboxCameral1.Height = 300;
            //viewboxCameral1.Width = 403;
        }

        public delegate void FullScreenHandler(int camId);

        public event FullScreenHandler FullScreenEvent;
        private void CameraOne_FullScreenEvent()
        {
            if (FullScreenEvent != null)
            {
                FullScreenEvent(5);
            }
        }

        private void CameraTwo_FullScreenEvent()
        {
            if (FullScreenEvent != null)
            {
                FullScreenEvent(6);
            }
        }

        private void InitControls()
        {
            cameraInitImage1.Source = new BitmapImage(new Uri("../Images/camera.jpg", UriKind.Relative));

            gridCamera1.Children.Add(cameraInitImage1);

            cameraInitImage2.Source = new BitmapImage(new Uri("../Images/camera.jpg", UriKind.Relative));

            gridCamera2.Children.Add(cameraInitImage2);
        }

        public void PlayCameraInThread()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                PlayCamera();
            });
        }

        #endregion
    }
}
