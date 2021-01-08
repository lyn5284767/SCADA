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
        int curPage = 1;
        int maxPage = 2;
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
            ReportTimer = new System.Threading.Timer(new TimerCallback(ReportTimer_Elapse), null, 500, 20);
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
                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocalOrRemote"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocalOrRemoreCheckConverter() });
                this.controlModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocalOrRemote"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocalOrRemoreConverter() });

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
                this.tbLocalOrRemote.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocalOrRemote"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocalOrRemoreConverter() });

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
                this.tbWorkTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfWorkTime"], Mode = BindingMode.OneWay ,Converter=new DivideTenConverter()});

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

                this.smSixLUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSixLDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSixRUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSixRDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSixMidUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSixMidDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSixBack.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["517b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSixEnable.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

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
                this.bdCosing.Visibility = Visibility.Visible;
                this.bdDrill.Visibility = Visibility.Collapsed;
            }
            else//当前套管
            {
                byteToSend = new byte[10] { 23, 17, 3, 1, 0, 0, 0, 0, 0, 0 };
                this.bdCosing.Visibility = Visibility.Collapsed;
                this.bdDrill.Visibility = Visibility.Visible;
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
        int tmpCount = 0;
        List<string> sqlList = new List<string>();
        double maxTorque = 0.0;
        double maxCosing = 0.0;
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
                    double drillval = GlobalData.Instance.da["SIRSelfInButtonSpeedSet"].Value.Byte / 10.0;
                    drillval = 6.9 * drillval;
                    drillval = Math.Round(drillval, 1);
                    double drillTore = GlobalData.Instance.da["SIRSelfInButtonTorque"].Value.Int32 / 10.0;
                    double cosingTorque = GlobalData.Instance.da["SIRSelfOutButtonTorque"].Value.Int32 / 10.0;
                    if (GlobalData.Instance.da["SIRSelfPlierInButtonElectric"].Value.Int32 > 0)
                    {
                        tmpCount = 0;
                        // 上扣模式 有扭矩值才存
                        if(drillTore>0 && GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 1)
                        {
                            
                            if (maxTorque < drillTore)
                            {
                                maxTorque = drillTore;
                            }
                            
                        }
                        this.drillTorqueChart.AddPoints(drillTore, maxTorque);
                        // 上扣模式 有套管扭矩才记录
                        if (cosingTorque > 0 && GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 1)
                        {
                            if (maxCosing < cosingTorque)
                            {
                                maxCosing = cosingTorque;
                            }
                        }
                        this.cosingTorqueChart.AddPoints(cosingTorque, maxCosing);
                    }
                    else // 无动作10秒后清除数据
                    {
                        tmpCount++;
                        if (tmpCount > 600)
                        {
                            tmpCount = 0;
                            this.cosingTorqueChart.ClearPoint();
                            this.drillTorqueChart.ClearPoint();
                            if (maxTorque > 0) // 记录钻杆扭矩 在-1和+5 之间记录
                            {
 
                                if ( maxTorque < drillval - 1) // 小于标准值-1 ,设为标准值
                                {
                                    maxTorque = drillval + 1.2;
                                }
                                if (maxTorque > drillval + 5)// 大于标准值+5 ,设为标准值+3
                                {
                                    maxTorque = drillval + 3;
                                }
                                string sql = string.Format("Insert Into DateBaseReport (Name,Value,CreateTime,Type,StandardValue) Values('{0}','{1}','{2}','{3}','{4}')", "自研铁钻工-钻杆扭矩",
                            maxTorque, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)SaveType.SIR_Self_DrillTorque, drillval);
                                sqlList.Add(sql);
                                //一个周期结束也记录一次
                                DataHelper.Instance.ExecuteNonQuery(sqlList.ToArray());
                                sqlList.Clear();
                                maxTorque = 0.0;
                            }
                            if (maxCosing > 0) //记录钻杆扭矩
                            {
                                string sql = string.Format("Insert Into DateBaseReport (Name,Value,CreateTime,Type,StandardValue) Values('{0}','{1}','{2}','{3}','{4}')", "自研铁钻工-钻杆扭矩",
                            maxCosing, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)SaveType.SIR_Self_CosingTorque, 0);
                                sqlList.Add(sql);
                                //一个周期结束也记录一次
                                DataHelper.Instance.ExecuteNonQuery(sqlList.ToArray());
                                sqlList.Clear();
                                maxCosing = 0.0;
                            }
                        }
                    }
                   
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
            else
            {
                if (this.tbTips.Text == "网络连接失败！") this.tbTips.Text = "";
            }

            //上扣指示灯亮，但是未卸扣工作模式 
            if (GlobalData.Instance.da["841b3"].Value.Boolean && GlobalData.Instance.da["841b5"].Value.Boolean
            && GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 2)
            {
                this.tbTips.Text = "卸扣模式不一致，请切换手/自动";
            }
            else
            {
                if (this.tbTips.Text == "卸扣模式不一致，请切换手/自动") this.tbTips.Text = "";
            }

            if (GlobalData.Instance.da["841b4"].Value.Boolean && GlobalData.Instance.da["841b6"].Value.Boolean
        && GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 1)
            {
                this.tbTips.Text = "上扣模式不一致，请切换手/自动";
            }
            else
            {
                if (this.tbTips.Text == "上扣模式不一致，请切换手/自动") this.tbTips.Text = "";
            }
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
                        case 15:
                            this.tbTips.Text = "背钳复位故障";
                            break;
                        case 16:
                            this.tbTips.Text = "工况选择故障";
                            break;
                        case 17:
                            this.tbTips.Text = "背钳夹紧压力过低";
                            break;
                        case 18:
                            this.tbTips.Text = "关门传感器故障";
                            break;
                        case 19:
                            this.tbTips.Text = "未到上扣扭矩";
                            break;
                        case 20:
                            this.tbTips.Text = "未到上扣圈数";
                            break;
                        case 21:
                            this.tbTips.Text = "未到紧扣扭矩";
                            break;
                        case 22:
                            this.tbTips.Text = "背钳传感器故障";
                            break;
                        case 23:
                            this.tbTips.Text = "开门传感器故障";
                            break;
                        case 24:
                            this.tbTips.Text = "钻杆未卸开";
                            break;
                        case 25:
                            this.tbTips.Text = "卸扣打滑";
                            break;
                        case 26:
                            this.tbTips.Text = "液压系统压力过低";
                            break;
                        case 27:
                            this.tbTips.Text = "手臂伸缩卡滞";
                            break;
                        case 28:
                            this.tbTips.Text = "钳体升降卡滞";
                            break;
                        case 29:
                            this.tbTips.Text = "钳体回转限位已发生";
                            break;
                        case 30:
                            this.tbTips.Text = "工作缸气压过低警告";
                            break;
                        case 31:
                            this.tbTips.Text = "制动缸气压过低警告";
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
                    case 20:
                        this.tbOprTips.Text = "上扣工况选择后请执行自动对缺确认";
                        break;
                    case 21:
                        this.tbOprTips.Text = "卸扣工况选择后请执行自动对缺确认";
                        break;
                    case 22:
                        this.tbOprTips.Text = "请确认缺口复位状态";
                        break;
                    case 23:
                        this.tbOprTips.Text = "在井口工位请确认接箍高度";
                        break;
                    case 24:
                        this.tbOprTips.Text = "当前工况请使用手动上扣";
                        break;
                    case 25:
                        this.tbOprTips.Text = "传感器异常请重新复位缺口";
                        break;
                    case 26:
                        this.tbOprTips.Text = "请人工确认缺口复位状态";
                        break;
                    case 27:
                        this.tbOprTips.Text = "当前工况请使用手动卸扣";
                        break;
                    case 28:
                        this.tbOprTips.Text = "注意:当前系统状态为紧急停止!";
                        break;
                    case 29:
                        this.tbOprTips.Text = "模式切换了!";
                        break;
                    case 30:
                        this.tbOprTips.Text = "请退出检查故障!";
                        break;
                    case 31:
                        this.tbOprTips.Text = "铁钻工不在上扣工位";
                        break;
                    case 32:
                        this.tbOprTips.Text = "铁钻工不在卸扣工位";
                        break;
                    case 33:
                        this.tbOprTips.Text = "安全互锁已启动";
                        break;
                    case 34:
                        this.tbOprTips.Text = "安全互锁已解除";
                        break;
                    case 35:
                        this.tbOprTips.Text = "司钻模式下回转调节启用";
                        break;
                    case 36:
                        this.tbOprTips.Text = "遥控模式下回转调节启用";
                        break;
                    case 37:
                        this.tbOprTips.Text = "已到钻杆顶销位";
                        break;
                    case 38:
                        this.tbOprTips.Text = "手臂禁止伸出";
                        break;
                    case 39:
                        this.tbOprTips.Text = "禁止上卸扣";
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
                if (cameraOne == null) return;
                if (cameraOne.Info.CameraType == 0)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
                }
                else if (cameraOne.Info.CameraType == 1)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".h264";
                }
                else return;
                cameraOne.StopFile();
                cameraOne.SaveFile(filePath, fileName);
                DeleteOldFileName(filePath);
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
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
                if (cameraOne == null) return;
                if (cameraOne.Info.CameraType == 0)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
                }
                else if (cameraOne.Info.CameraType == 1)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".h264";
                }
                else return;
                cameraOne.StopFile();
                cameraOne.SaveFile(filePath, fileName);
                DeleteOldFileName(filePath);
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
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
            if (cameraOne == null) return;
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
            if (cameraTwo == null) return;
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
        /// <summary>
        /// 控制模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] drbyteToSend;
            byte[] sirbyteToSend;
            byte[] sfbyteToSend;
            if (this.controlModel.IsChecked)
            {
                drbyteToSend = new byte[10] { 1, 32, 2, 20, 0, 0, 0, 0, 0, 0 }; // 钻台面-遥控切司钻
                sirbyteToSend = new byte[10] { 23, 17, 10, 2, 0, 0, 0, 0, 0, 0 }; // 铁钻工-司钻切遥控
                sfbyteToSend = new byte[10] { 16, 1, 27, 1, 1, 0, 0, 0, 0, 0 };// 二层台-遥控切司钻
                GlobalData.Instance.da.SendBytes(drbyteToSend);
                Thread.Sleep(50);
                GlobalData.Instance.da.SendBytes(sirbyteToSend);
                Thread.Sleep(50);
                GlobalData.Instance.da.SendBytes(sfbyteToSend);
            }
            else
            {
                drbyteToSend = new byte[10] { 23, 17, 10, 1, 0, 0, 0, 0, 0, 0 };// 钻台面-遥控切司钻
                GlobalData.Instance.da.SendBytes(drbyteToSend);

            }
        }
        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Left_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.curPage -= 1;
            if (this.curPage < 1) curPage = 1;
            this.ShowPage(this.curPage);
        }
        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Right_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.curPage += 1;
            if (this.curPage > maxPage) this.curPage = maxPage;
            this.ShowPage(this.curPage);
        }

        private void ShowPage(int page)
        {
            if (page == 1)
            {
                this.firstPage.Visibility = Visibility.Visible;
                this.secondPage.Visibility = Visibility.Collapsed;
            }
            else if (page == 2)
            {
                this.firstPage.Visibility = Visibility.Collapsed;
                this.secondPage.Visibility = Visibility.Visible;
            }
        }
    }
}
