using COM.Common;
using ControlLibrary;
using DatabaseLib;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
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

namespace Main.Integration
{
    /// <summary>
    /// IngMain.xaml 的交互逻辑
    /// </summary>
    public partial class IngMain : UserControl
    {
        private static IngMain _instance = null;
        private static readonly object syncRoot = new object();

        public static IngMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngMain();
                        }
                    }
                }
                return _instance;
            }
        }
        SystemType systemType = SystemType.SecondFloor;
        System.Threading.Timer timerWarning;
        System.Timers.Timer pageChange;
        int count = 0; // 进入页面发送协议次数
        public IngMain()
        {
            InitializeComponent();
            GlobalData.Instance.Rows = GlobalData.Instance.da["DrillNums"].Value.Byte;
            GlobalData.Instance.DrillNum = GlobalData.Instance.da["103E23B5"].Value.Byte;
            //SFSelectMouseDown(null, null);
            ////amination.SendFingerBeamNumberEvent += Amination_SendFingerBeamNumberEvent;
            //aminationNew.SendFingerBeamNumberEvent += Amination_SendFingerBeamNumberEvent;
            //VariableBinding();
            this.Loaded += IngMain_Loaded;
        }

        private void IngMain_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);

            count = 0;
            GlobalData.Instance.DRNowPage = "IngMain";
            pageChange = new System.Timers.Timer(500);
            pageChange.Elapsed += PageChange_Elapsed;
            pageChange.Enabled = true;

            string configPath = System.Environment.CurrentDirectory + "\\KeyBoard.exe";
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processList)
            {
                if (process.ProcessName.Contains("KeyBoard"))
                {
                    process.Kill();
                }
            }
            InitAllModel();
        }

        /// <summary>
        /// 切换页面发送指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageChange_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            count++;
            if (GlobalData.Instance.da["drPageNum"].Value.Byte == 30 || count > 5 || GlobalData.Instance.DRNowPage != "IngMain")
            {
                pageChange.Stop();
            }
            else
            {
                byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
                GlobalData.Instance.da.SendBytes(data);
            }
        }

        private void VariableBinding()
        {
            //try
            //{
            //    this.rotateAngle.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });
            //    this.bigHookRealTimeValue.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["156To159BigHookEncoderRealTimeValue"], Mode = BindingMode.OneWay });
            //    this.bigHookCalibrationValue.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["160To163BigHookEncoderCalibrationValue"], Mode = BindingMode.OneWay });
            //    this.leftFinger.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23LeftFingerMotorSampleValue"], Mode = BindingMode.OneWay });
            //    this.rightFinger.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23RightFingerMotorSampleValue"], Mode = BindingMode.OneWay });
            //    this.gripMotor.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23GripMotorSampleValue"], Mode = BindingMode.OneWay });

            //    this.carMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            //    this.armMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            //    this.rotateMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["rotateMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            //    this.carMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
            //    this.armMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
            //    this.rotateMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["rotateMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });

            //    this.operateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
            //    this.operateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
            //    this.workMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
            //    this.workMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelIsCheckConverter() });

            //    RopeModelConverter RopeModeleMultiConverter = new RopeModelConverter();
            //    MultiBinding RopeModelMultiBind = new MultiBinding();
            //    RopeModelMultiBind.Converter = RopeModeleMultiConverter;
            //    RopeModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b0"], Mode = BindingMode.OneWay });
            //    RopeModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b1"], Mode = BindingMode.OneWay });
            //    RopeModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b2"], Mode = BindingMode.OneWay });
            //    RopeModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b3"], Mode = BindingMode.OneWay });
            //    RopeModelMultiBind.NotifyOnSourceUpdated = true;
            //    this.RopeModel.SetBinding(BasedSwitchButton.ContentDownProperty, RopeModelMultiBind);

            //    RopeModelIsCheckConverter RopeModelIsCheckMultiConverter = new RopeModelIsCheckConverter();
            //    MultiBinding RopeModelIsCheckMultiBind = new MultiBinding();
            //    RopeModelIsCheckMultiBind.Converter = RopeModelIsCheckMultiConverter;
            //    RopeModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b0"], Mode = BindingMode.OneWay });
            //    RopeModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b1"], Mode = BindingMode.OneWay });
            //    RopeModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b2"], Mode = BindingMode.OneWay });
            //    RopeModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b3"], Mode = BindingMode.OneWay });
            //    RopeModelIsCheckMultiBind.NotifyOnSourceUpdated = true;
            //    this.RopeModel.SetBinding(BasedSwitchButton.IsCheckedProperty, RopeModelIsCheckMultiBind);
            //    IngDrillPipeTypeConverter ingDrillPipeTypeConverter = new IngDrillPipeTypeConverter();
            //    MultiBinding ingDrillPipeTypeMultiBind = new MultiBinding();
            //    ingDrillPipeTypeMultiBind.Converter = ingDrillPipeTypeConverter;
            //    ingDrillPipeTypeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drillPipeType"], Mode = BindingMode.OneWay });
            //    ingDrillPipeTypeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drdrillPipeType"], Mode = BindingMode.OneWay });
            //    ingDrillPipeTypeMultiBind.NotifyOnSourceUpdated = true;
            //    this.tubeType.SetBinding(TextBlock.TextProperty, ingDrillPipeTypeMultiBind);

            //    #region 钻台面变量
            //    this.drcarMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            //    this.drRotateMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            //    this.drcarMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            //    this.drRotateMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

            //    this.drgripMotor.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["gridSample"], Mode = BindingMode.OneWay });

            //    this.droperateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
            //    this.droperateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
            //    this.drworkMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new DRWorkModelConverter() });
            //    this.drworkMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new DRWorkModelIsCheckConverter() });

            //    //MultiBinding carMoveModelMultiBind = new MultiBinding();
            //    //carMoveModelMultiBind.Converter = new CarMoveModelCoverter();
            //    //carMoveModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["326b4"], Mode = BindingMode.OneWay });
            //    //carMoveModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["326b5"], Mode = BindingMode.OneWay });
            //    //carMoveModelMultiBind.NotifyOnSourceUpdated = true;
            //    //this.drCarMoveModel.SetBinding(BasedSwitchButton.ContentDownProperty, carMoveModelMultiBind);
            //    //MultiBinding carMoveModelIsCheckMultiBind = new MultiBinding();
            //    //carMoveModelIsCheckMultiBind.Converter = new CarMoveModelIsCheckCoverter();
            //    //carMoveModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["326b4"], Mode = BindingMode.OneWay });
            //    //carMoveModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["326b5"], Mode = BindingMode.OneWay });
            //    //carMoveModelIsCheckMultiBind.NotifyOnSourceUpdated = true;
            //    //this.drCarMoveModel.SetBinding(BasedSwitchButton.IsCheckedProperty, carMoveModelIsCheckMultiBind);
            //    #endregion
            //    // 6.30新增
            //    this.oobLink.SetBinding(OnOffButton.OnOffButtonCheckProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
            //    this.tbLink.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay, Converter = new LinkOpenOrCloseConverter() });

            //    this.drDestination.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDes"], Mode = BindingMode.OneWay, Converter = new DesTypeConverter() });

            //    MultiBinding LinkErrorMultiBind = new MultiBinding();
            //    LinkErrorMultiBind.Converter = new LinkErrorCoverter();
            //    LinkErrorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drLinkError"], Mode = BindingMode.OneWay });
            //    LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
            //    LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b1"], Mode = BindingMode.OneWay });
            //    LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b2"], Mode = BindingMode.OneWay });
            //    LinkErrorMultiBind.NotifyOnSourceUpdated = true;
            //    this.LinkError.SetBinding(TextBlock.TextProperty, LinkErrorMultiBind);
            //    // 自动步骤-自动控件
            //    MultiBinding IngStepMultiBind = new MultiBinding();
            //    IngStepMultiBind.Converter = new IngStepCoverter();
            //    IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //    IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
            //    IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
            //    IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //    IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
            //    IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
            //    IngStepMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b1"], Mode = BindingMode.OneWay });
            //    IngStepMultiBind.NotifyOnSourceUpdated = true;
            //    this.sbDrillUp.SetBinding(StepBar.StepIndexProperty, IngStepMultiBind);
            //    this.sbDrillDown.SetBinding(StepBar.StepIndexProperty, IngStepMultiBind);
            //    //this.IngStep.SetBinding(StepControl.SelectStepProperty, IngStepMultiBind);


            //    //MultiBinding AutoStepCurrentTxtMultiBind = new MultiBinding();
            //    //AutoStepCurrentTxtMultiBind.Converter = new IngAutoModeNowStepCoverter();
            //    //AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //    //AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
            //    //AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
            //    //AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //    //AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
            //    //AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
            //    //AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b1"], Mode = BindingMode.OneWay });
            //    //AutoStepCurrentTxtMultiBind.NotifyOnSourceUpdated = true;
            //    //this.AutoStepCurrentTxt.SetBinding(TextBlock.TextProperty, AutoStepCurrentTxtMultiBind);

            //    // 一键上扣
            //    MultiBinding sbInbuttonMultiBind = new MultiBinding();
            //    sbInbuttonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
            //    sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
            //    sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
            //    sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
            //    sbInbuttonMultiBind.ConverterParameter = "inButton";
            //    sbInbuttonMultiBind.NotifyOnSourceUpdated = true;
            //    this.sbInButton.SetBinding(StepBar.StepIndexProperty, sbInbuttonMultiBind);
            //    // 一键卸扣
            //    MultiBinding sbOutButtonMultiBind = new MultiBinding();
            //    sbOutButtonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
            //    sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
            //    sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
            //    sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
            //    sbOutButtonMultiBind.ConverterParameter = "outButton";
            //    sbOutButtonMultiBind.NotifyOnSourceUpdated = true;
            //    this.sbOutButton.SetBinding(StepBar.StepIndexProperty, sbOutButtonMultiBind);

            //    this.warningOne.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["155InterlockPromptMessageCode"], Mode = BindingMode.OneWay, Converter = new IngLockTipsCoverter() });
            //    timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟
            //}
            //catch (Exception ex)
            //{
            //    Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            //}

        }
        /// <summary>
        /// 集成模式下，二层台/钻台面全选
        /// </summary>
        /// <param name="number"></param>
        private void Amination_SendFingerBeamNumberEvent(byte number)
        {
            //if (systemType == SystemType.SecondFloor)
            //{
                if (GlobalData.Instance.da["operationModel"].Value.Byte == 5 || GlobalData.Instance.da["operationModel"].Value.Byte == 3)
                {
                    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 5, number });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            //}
            //else if (systemType == SystemType.DrillFloor)
            //{
            Thread.Sleep(50);
                if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5 || GlobalData.Instance.da["droperationModel"].Value.Byte == 3)
                {
                    byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 5, number });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            //}
        }
        bool b459b0 = false; bool b459b1 = false;
        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private bool bCommunicationCheck = false; // 是否有中断标志
        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
           
        }
        /// <summary>
        /// 新建模式
        /// </summary>

        private void tbModel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ModelSetWindow modelSetWindow = new ModelSetWindow();
            modelSetWindow.ShowDialog();
            InitAllModel();
        }
        /// <summary>
        /// 初始化所有模式
        /// </summary>
        public void InitAllModel()
        {
            List<Grid> gdList = new List<Grid>();
            gdList.Add(this.gdModelTwo);
            gdList.Add(this.gdModelThree);
            gdList.Add(this.gdModelFour);
            gdList.Add(this.gdModelFive);
            gdList.Add(this.gdModelSix);
            string sql = "Select * from GlobalModel Order by ID Desc";
            List<GlobalModel> list = DataHelper.Instance.ExecuteList<GlobalModel>(sql);
            for (int i = 0; i < 5; i++)
            {
                if (i<list.Count)
                {
                    ModelDetailData data = new ModelDetailData(list[i]);
                    if(gdList[i].Children[0] is TextBlock)
                        gdList[i].Children[0].Visibility = Visibility.Collapsed;
                    gdList[i].Children.Add(data);
                }
                else
                {
                    if (gdList[i].Children[0] is TextBlock)
                        gdList[i].Children[0].Visibility = Visibility.Visible;
                    if (gdList[i].Children.Count == 2)
                    {
                        gdList[i].Children.RemoveAt(1);
                    }
                }
            }
        }
    }
}
