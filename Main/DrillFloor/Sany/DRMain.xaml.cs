using COM.Common;
using ControlLibrary;
using DatabaseLib;
using HandyControl.Controls;
using HBGKTest;
using HBGKTest.YiTongCamera;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.DrillFloor
{
    /// <summary>
    /// DRMain.xaml 的交互逻辑
    /// </summary>
    public partial class DRMain : UserControl
    {
        private static DRMain _instance = null;
        private static readonly object syncRoot = new object();

        public static DRMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DRMain();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        /// <summary>
        /// 开一个2秒后的定时器用于延时播放视频
        /// </summary>
        System.Timers.Timer cameraTimer;
        public delegate void FullScreenHandler(int camId);

        public event FullScreenHandler DRFullScreenEvent;

        public DRMain()
        {
            InitializeComponent();
            //this.amination.SendFingerBeamNumberEvent += Amination_SendFingerBeamNumberEvent; ;
            //this.amination.CIMSChange(SystemType.DrillFloor);
            //this.amination.SystemChange(SystemType.DrillFloor);
            this.aminationNew.SendFingerBeamNumberEvent += Amination_SendFingerBeamNumberEvent;
            this.aminationNew.SystemChange(SystemType.DrillFloor);

            VariableBinding();
            this.Loaded += DRMain_Loaded;
        }

        System.Timers.Timer pageChange;
        int count = 0; // 进入页面发送协议次数
        private void DRMain_Loaded(object sender, RoutedEventArgs e)
        {
            // 扶杆臂主界面
            //byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 30 };
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);
            Thread.Sleep(50);
            // 不为自动或手动则切换为手动
            if (!(GlobalData.Instance.da["droperationModel"].Value.Byte == 4 || GlobalData.Instance.da["droperationModel"].Value.Byte == 5))
            {//切换到手动命令
                data = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(data);
            }
            count = 0;
            GlobalData.Instance.DRNowPage = "DRMain";
            pageChange = new System.Timers.Timer(500);
            pageChange.Elapsed += PageChange_Elapsed;
            pageChange.Enabled = true;

            //this.amination.InitRowsColoms(SystemType.DrillFloor);
            this.aminationNew.InitRowsColoms(SystemType.DrillFloor);
            PlayCameraInThread();
            // 其他厂家集成我们设备发送
            if (GlobalData.Instance.da["IngSetting"]!=null && GlobalData.Instance.da["IngSetting"].Value.Byte != 0)
            {
                data = new byte[10] { 16, 1, 1, 10, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(data);
            }
        }
        /// <summary>
        /// 切换页面发送指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageChange_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            count++;
            if (GlobalData.Instance.da["drPageNum"].Value.Byte == 30 || count>5 || GlobalData.Instance.DRNowPage != "DRMain")
            {
                pageChange.Stop();             
            }
            else
            {
                //byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 30 };
                byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
                GlobalData.Instance.da.SendBytes(data);
            }
        }

        private void Amination_SendFingerBeamNumberEvent(byte number)
        {
            if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5 || GlobalData.Instance.da["droperationModel"].Value.Byte == 3)
            {
                byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 5, number });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        private void VariableBinding()
        {
            try
            {
                //this.carPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drCarPos"], Mode = BindingMode.OneWay, Converter = new CarPosCoverter() });//小车位置
                this.carPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drCarPos"], Mode = BindingMode.OneWay });//小车位置
                                                                                                                                                                   //this.armPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drArmPos"], Mode = BindingMode.OneWay, Converter = new ArmPosCoverter() });//手臂实际位置
                this.armPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drArmPos"], Mode = BindingMode.OneWay });//手臂实际位置
                this.rotatePos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drRotePos"], Mode = BindingMode.OneWay, Converter = new DRCallAngleConverter() });//回转角度
                if (GlobalData.Instance.da["DR_DeviceType"] != null &&
                    (GlobalData.Instance.da["DR_DeviceType"].Value.Int32 == 5100 || GlobalData.Instance.da["DR_DeviceType"].Value.Int32 == 7100
                     || GlobalData.Instance.da["DR_DeviceType"].Value.Int32 == 8100 || GlobalData.Instance.da["DR_DeviceType"].Value.Int32 == 9100))
                {
                    //MultiBinding dr_GripMultiBind = new MultiBinding();
                    //dr_GripMultiBind.Converter = new DR_GripCoverter();
                    //dr_GripMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["320b0"], Mode = BindingMode.OneWay });
                    //dr_GripMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["320b1"], Mode = BindingMode.OneWay });
                    //dr_GripMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["320b2"], Mode = BindingMode.OneWay });
                    //dr_GripMultiBind.NotifyOnSourceUpdated = true;
                    //this.gripMotor.SetBinding(TextBlock.TextProperty, dr_GripMultiBind);
                    this.gripMotor.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drgripStatus"], Mode = BindingMode.OneWay, Converter = new GripNewConverter() });//抓手状态


                }
                else
                {
                    this.gripMotor.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drgripStatus"], Mode = BindingMode.OneWay, Converter = new GripConverter() });//抓手状态
                }
                this.drcarMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drRotateMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drcarMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drRotateMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                this.droperateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.droperateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
                this.drworkMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new DRWorkModelConverter() });
                this.drworkMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new DRWorkModelIsCheckConverter() });

                //MultiBinding carMoveModelMultiBind = new MultiBinding();
                //carMoveModelMultiBind.Converter = new CarMoveModelCoverter();
                //carMoveModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["326b4"], Mode = BindingMode.OneWay });
                //carMoveModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["326b5"], Mode = BindingMode.OneWay });
                //carMoveModelMultiBind.NotifyOnSourceUpdated = true;
                //this.drCarMoveModel.SetBinding(BasedSwitchButton.ContentDownProperty, carMoveModelMultiBind);
                //MultiBinding carMoveModelIsCheckMultiBind = new MultiBinding();
                //carMoveModelIsCheckMultiBind.Converter = new CarMoveModelIsCheckCoverter();
                //carMoveModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["326b4"], Mode = BindingMode.OneWay });
                //carMoveModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["326b5"], Mode = BindingMode.OneWay });
                //carMoveModelIsCheckMultiBind.NotifyOnSourceUpdated = true;
                //this.drCarMoveModel.SetBinding(BasedSwitchButton.IsCheckedProperty, carMoveModelIsCheckMultiBind);

                this.SelectType.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["325b0"], Mode = BindingMode.OneWay, Converter = new SelectTypeConverter() });
                this.SelectType.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["325b0"], Mode = BindingMode.OneWay, Converter = new SelectTypeIsCheckConverter() });
                this.drTelecontrolModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["325b1"], Mode = BindingMode.OneWay, Converter = new TelecontrolModelConverter() });
                this.drTelecontrolModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["325b1"], Mode = BindingMode.OneWay, Converter = new TelecontrolModelIsCheckConverter() });
                //下钻
                MultiBinding sbDrillUpMultiBind = new MultiBinding();
                sbDrillUpMultiBind.Converter = new DRStepCoverter();
                sbDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                sbDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                sbDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
                sbDrillUpMultiBind.NotifyOnSourceUpdated = true;
                this.sbDrillUp.SetBinding(StepBar.StepIndexProperty, sbDrillUpMultiBind);
                // 起钻
                MultiBinding sbDrillDownMultiBind = new MultiBinding();
                sbDrillDownMultiBind.Converter = new DRStepCoverter();
                sbDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                sbDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                sbDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
                sbDrillDownMultiBind.NotifyOnSourceUpdated = true;
                this.sbDrillDown.SetBinding(StepBar.StepIndexProperty, sbDrillDownMultiBind);
                //MultiBinding drStepMultiBind = new MultiBinding();
                //drStepMultiBind.Converter = new DRStepCoverter();
                //drStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                //drStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                //drStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
                //drStepMultiBind.NotifyOnSourceUpdated = true;
                //this.drStep.SetBinding(StepControl.SelectStepProperty, drStepMultiBind);

                //MultiBinding stepTxtMultiBind = new MultiBinding();
                //stepTxtMultiBind.Converter = new DRStepTxtCoverter();
                //stepTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                //stepTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                //stepTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
                //stepTxtMultiBind.NotifyOnSourceUpdated = true;
                //this.stepTxt.SetBinding(TextBlock.TextProperty, stepTxtMultiBind);

                this.drDestination.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDes"], Mode = BindingMode.OneWay, Converter = new DesTypeConverter() });// 目的地选择
                //this.tubeType.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drdrillPipeType"], Mode = BindingMode.OneWay, Converter = new DrillPipeTypeConverter() });// 管柱选择
                MultiBinding drillTypeSelectMultiBind = new MultiBinding();
                drillTypeSelectMultiBind.Converter = new DrillTypeSelectCoverter();
                drillTypeSelectMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drdrillPipeType"], Mode = BindingMode.OneWay });
                drillTypeSelectMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["103b7"], Mode = BindingMode.OneWay });
                drillTypeSelectMultiBind.NotifyOnSourceUpdated = true;
                this.tubeType.SetBinding(TextBlock.TextProperty, drillTypeSelectMultiBind);

                LeftHandMultiConverter leftHandMultiConverter = new LeftHandMultiConverter();
                MultiBinding leftHand = new MultiBinding();
                leftHand.Converter = leftHandMultiConverter;
                leftHand.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["503b5"], Mode = BindingMode.OneWay });
                leftHand.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["535b0"], Mode = BindingMode.OneWay });
                leftHand.NotifyOnSourceUpdated = true;
                this.tbLeftHand.SetBinding(TextBlock.TextProperty, leftHand);

                this.tbMemoryPos.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_DR_MemoryPos"], Mode = BindingMode.OneWay, Converter = new DR_MemoryPos() });// 井口记忆

                InitCameraInfo();

                //cameraTimer = new System.Timers.Timer(2 * 1000);
                //cameraTimer.Elapsed += CameraTimer_Elapsed;
                //cameraTimer.Enabled = true;
                //PlayCameraInThread();
                timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        public void PlayCameraInThread()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2 * 1000);
                PlayCamera();
            });

        }

        private int iTimeCnt = 0;//用来为时钟计数的变量

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    //amination.LoadFingerBeamDrillPipe(SystemType.DrillFloor);
                    aminationNew.LoadFingerBeamDrillPipe(SystemType.DrillFloor);
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.HandTips();
                    this.Warnning();
                    if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5)
                    {
                        if (GlobalData.Instance.da["drworkModel"].Value.Byte == 2)
                        {
                            if (GlobalData.Instance.da["drDes"] != null && GlobalData.Instance.da["drDes"].Value.Byte == 4)
                            {
                                this.sbDrillUp.Visibility = Visibility.Collapsed;
                                this.sbDrillDown.Visibility = Visibility.Collapsed;
                                this.sbDrillDownWithNumFour.Visibility = Visibility.Collapsed;
                                this.sbDrillUpWithNumFour.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                this.sbDrillUp.Visibility = Visibility.Visible;
                                this.sbDrillDown.Visibility = Visibility.Collapsed;
                                this.sbDrillDownWithNumFour.Visibility = Visibility.Collapsed;
                                this.sbDrillUpWithNumFour.Visibility = Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            if (GlobalData.Instance.da["drDes"] != null && GlobalData.Instance.da["drDes"].Value.Byte == 4)
                            {
                                this.sbDrillUp.Visibility = Visibility.Collapsed;
                                this.sbDrillDown.Visibility = Visibility.Collapsed;
                                this.sbDrillUpWithNumFour.Visibility = Visibility.Collapsed;
                                this.sbDrillDownWithNumFour.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                this.sbDrillUp.Visibility = Visibility.Collapsed;
                                this.sbDrillDown.Visibility = Visibility.Visible;
                                this.sbDrillUpWithNumFour.Visibility = Visibility.Collapsed;
                                this.sbDrillDownWithNumFour.Visibility = Visibility.Collapsed;
                            }
                        }
                    }

                    if (GlobalData.Instance.da["droperationModel"].Value.Byte == 1)
                    {
                        this.warnTwo.Text = "当前系统为紧急停止状态";
                    }
                    else
                    {
                        if (this.warnTwo.Text == "当前系统为紧急停止状态")
                            this.warnTwo.Text = "";
                    }
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 操作手性指示
        /// </summary>
        private void HandTips()
        {
            // 电机使能提示
            if (!GlobalData.Instance.da["324b1"].Value.Boolean || !GlobalData.Instance.da["324b5"].Value.Boolean)
            {
                if (this.tbEnableHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbEnableHand.Visibility = Visibility.Collapsed;
                else if (this.tbEnableHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbEnableHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbEnableHand.Visibility = Visibility.Collapsed;
            }
            // 电机回零提示
            if (!GlobalData.Instance.da["324b0"].Value.Boolean || !GlobalData.Instance.da["324b4"].Value.Boolean)
            {
                if (this.tbTurnZeroHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbTurnZeroHand.Visibility = Visibility.Collapsed;
                else if (this.tbTurnZeroHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbTurnZeroHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbTurnZeroHand.Visibility = Visibility.Collapsed;
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
            #region 告警1
            int warnOne = GlobalData.Instance.da["drTipsCode"].Value.Byte;
            if (warnOne == 1)
            {
                this.warnOne.Text = "小车电机故障";
            }
            else if (warnOne == 3)
            {
                this.warnOne.Text = "回转电机故障";
            }
            else if (warnOne == 6)
            {
                this.warnOne.Text = "机械手禁止向井口移动";
            }
            else if (warnOne == 7)
            {
                this.warnOne.Text = "禁止机械手缩回";
            }
            else if (warnOne == 11)
            {
                this.warnOne.Text = "抓手不允许打开";
            }
            else if (warnOne == 12)
            {
                this.warnOne.Text = "抓手不允许关闭";
            }
            else if (warnOne == 13)
            {
                this.warnOne.Text = "请将小车移至井口";
            }
            else if (warnOne == 15)
            {
                this.warnOne.Text = "小车不能往井口移动";
            }
            else if (warnOne == 20)
            {
                this.warnOne.Text = "回转回零异常，检查传感器";
            }
            else if (warnOne == 21)
            {
                this.warnOne.Text = "小车未回零或最大最小值限制";
            }
            else if (warnOne == 22 || warnOne == 23)
            {
                this.warnOne.Text = "手臂回零或最大最小值限制";
            }
            else if (warnOne == 31)
            {
                this.warnOne.Text = "抓手中有钻杆，请切换手动处理";
            }
            else if (warnOne == 32)
            {
                this.warnOne.Text = "请先回收手臂";
            }
            else if (warnOne == 33)
            {
                this.warnOne.Text = "小车回零中...";
            }
            else if (warnOne == 34)
            {
                this.warnOne.Text = "请将手臂收到最小";
            }
            else if (warnOne == 35)
            {
                this.warnOne.Text = "手臂回零中...";
            }
            else if (warnOne == 36)
            {
                this.warnOne.Text = "请先回收小车";
            }
            else if (warnOne == 37)
            {
                this.warnOne.Text = "回转回零中...";
            }
            else if (warnOne == 38)
            {
                this.warnOne.Text = "请将小车移动到中间靠井口";
            }
            else if (warnOne == 39)
            {
                this.warnOne.Text = "此轴已回零";
            }
            else if (warnOne == 40)
            {
                this.warnOne.Text = "系统未回零，请谨慎操作";
            }
            else if (warnOne == 41)
            {
                this.warnOne.Text = "请选择管柱类型";
            }
            else if (warnOne == 42)
            {
                this.warnOne.Text = "请选择指梁";
            }
            else if (warnOne == 43)
            {
                this.warnOne.Text = "请选择目的地";
            }
            else if (warnOne == 44)
            {
                this.warnOne.Text = "管柱已满，请切换指梁";
            }
            else if (warnOne == 45)
            {
                this.warnOne.Text = "请启动确认按键启动";
            }
            else if (warnOne == 46)
            {
                this.warnOne.Text = "确定大钩上提";
            }
            else if (warnOne == 47)
            {
                this.warnOne.Text = "抓手还未打开";
            }
            else if (warnOne == 48)
            {
                this.warnOne.Text = "抓手动作中...";
            }
            else if (warnOne == 49)
            {
                this.warnOne.Text = "请确认井口安全";
            }
            else if (warnOne == 50)
            {
                this.warnOne.Text = "确认上管柱已脱离下管柱";
            }
            else if (warnOne == 51)
            {
                this.warnOne.Text = "确认抓手中已有管柱";
            }
            else if (warnOne == 52)
            {
                this.warnOne.Text = "确认定位完成，大钩下落，钻杆入盒";
            }
            else if (warnOne == 53)
            {
                this.warnOne.Text = "请确认抓手已打开";
            }
            else if (warnOne == 54)
            {
                this.warnOne.Text = "自动排管动作完成";
            }
            else if (warnOne == 55)
            {
                this.warnOne.Text = "此指梁无钻杆";
            }
            else if (warnOne == 56)
            {
                this.warnOne.Text = "自动下钻抓杆失败，请确认抓手有钻杆";
            }
            else if (warnOne == 57)
            {
                this.warnOne.Text = "抓手未关闭";
            }
            else if (warnOne == 58)
            {
                this.warnOne.Text = "确认定位完成，大钩下落，上下管柱接触";
            }
            else if (warnOne == 59)
            {
                this.warnOne.Text = "需要人工确认吊卡关闭";
            }
            else if (warnOne == 60)
            {
                this.warnOne.Text = "自动下钻动作完成";
            }
            else if (warnOne == 61)
            {
                this.warnOne.Text = "确认启动回收模式";
            }
            else if (warnOne == 62)
            {
                this.warnOne.Text = "回收完成";
            }
            else if (warnOne == 65)
            {
                this.warnOne.Text = "请先回收手臂";
            }
            else if (warnOne == 71)
            {
                this.warnOne.Text = "请启动运输模式";
            }
            else if (warnOne == 72)
            {
                this.warnOne.Text = "已完成运输模式";
            }
            else if (warnOne == 81)
            {
                this.warnOne.Text = "两点距离太近";
            }
            else if (warnOne == 82)
            {
                this.warnOne.Text = "双轴运动";
            }
            else if (warnOne == 83)
            {
                this.warnOne.Text = "记录点数超限";
            }
            else if (warnOne == 84)
            {
                this.warnOne.Text = "记录点数为零";
            }
            else if (warnOne == 85)
            {
                this.warnOne.Text = "即将进入下一次示教";
            }
            else if (warnOne == 91)
            {
                this.warnOne.Text = "小车补偿超出范围";
            }
            else if (warnOne == 93)
            {
                this.warnOne.Text = "回转补偿超出范围";
            }
            else if (warnOne == 94)
            {
                this.warnOne.Text = "行车正在右移...";
            }
            else if (warnOne == 95)
            {
                this.warnOne.Text = "行车正在左移...";
            }
            else if (warnOne == 101)
            {
                this.warnOne.Text = "盖板未打开，无法向右平移";
            }
            else if (warnOne == 102)
            {
                this.warnOne.Text = "盖板未打开，无法向左平移";
            }
            else if (warnOne == 103)
            {
                this.warnOne.Text = "机械手在左侧，无法向左平移";
            }
            else if (warnOne == 104)
            {
                this.warnOne.Text = "机械手在右侧，无法向右平移";
            }
            else if (warnOne == 106)
            {
                this.warnOne.Text = "行车未左移到位";
            }
            else if (warnOne == 107)
            {
                this.warnOne.Text = "行车未右移到位";
            }
            else
            {
                this.warnOne.Text = "";
            }
            #endregion

            #region 告警2
            int warnTwoNO = GlobalData.Instance.da["drErrorCode"].Value.Int32;
            if (warnTwoNO == 1) this.warnTwo.Text = "小车电机故障";
            else if (warnTwoNO == 3) this.warnTwo.Text = "回转电机故障";
            else if(warnTwoNO == 6) this.warnTwo.Text = "机械手禁止向井口移动";
            else if(warnTwoNO == 7) this.warnTwo.Text = "禁止机械手缩回";
            else if (warnTwoNO == 11) this.warnTwo.Text = "抓手不允许打开";
            else if (warnTwoNO == 12) this.warnTwo.Text = "抓手不允许关闭";
            else if (warnTwoNO == 13) this.warnTwo.Text = "小车不允许在此位置";
            else this.warnTwo.Text = "";
            #endregion

            #region 告警3
            if (iTimeCnt % 10 == 0)
            {
                warnThree.FontSize = 14;
                warnThree.Visibility = Visibility.Visible;
                if (!GlobalData.Instance.da["324b0"].Value.Boolean || !GlobalData.Instance.da["324b4"].Value.Boolean) this.warnThree.Text = "系统还未回零，请注意安全！";
                else if(GlobalData.Instance.da["337b0"].Value.Boolean) this.warnThree.Text = "抓手传感器异常";
                else if (GlobalData.Instance.da["337b1"].Value.Boolean) this.warnThree.Text = "抓手打开卡滞";
                else if (GlobalData.Instance.da["337b2"].Value.Boolean) this.warnThree.Text = "抓手关闭卡滞";
                else if (GlobalData.Instance.da["337b4"].Value.Boolean) this.warnThree.Text = "手臂传感器故障";
                else if (GlobalData.Instance.da["337b5"].Value.Boolean) this.warnThree.Text = "手臂伸出异常";
                else if (GlobalData.Instance.da["337b6"].Value.Boolean) this.warnThree.Text = "手臂缩回异常";
                else this.warnThree.Text = "";
            }
            else
            {
                warnThree.FontSize = 20;
            }
            #endregion

            //操作台控制器心跳
            if (GlobalData.Instance.da["508b5"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 600)
                {
                    //Communication = 2;
                    //this.warnOne.Text = "操作台信号中断";
                }
                if (!bCommunicationCheck && controlHeartTimes > 600)
                {
                    bCommunicationCheck = true;
                }
            }
            else
            {
                this.controlHeartTimes = 0;
            }
            this.tmpStatus = GlobalData.Instance.da["508b6"].Value.Boolean;

            if (!GlobalData.Instance.ComunciationNormal) this.warnOne.Text = "网络连接失败！";
            else
            {
                if (this.warnOne.Text == "网络连接失败！") this.warnOne.Text = "";
            }
        }

        /// <summary>
        /// 扶杆臂-一键回零
        /// </summary>
        private void btn_drAllMotorRetZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 扶杆臂-电机使能
        /// </summary>
        private void btn_drMotorEnable(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 扶杆臂-操作模式
        /// </summary>
        private void btn_drOpState(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.droperateMode.IsChecked) //当前手动切换自动
            {
                byteToSend = new byte[10] { 1, 32, 3, 31, 0, 0, 0, 0, 0, 0 };
            }
            else// 当前自动切换手动
            {
                byteToSend = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 扶杆臂-工作模式
        /// </summary>
        private void btn_drWorkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.drworkMode.IsChecked)// 下钻
            {
                byteToSend = new byte[10] { 1, 32, 4, 41, 0, 0, 0, 0, 0, 0 };
            }
            else// 起钻
            {
                byteToSend = new byte[10] { 1, 32, 4, 40, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 扶杆臂-行车模式
        /// </summary>
        private void btn_drCarMoveModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.drCarMoveModel.IsChecked)
            {
                this.drCarMoveModel.IsChecked = false;
                this.drCarMoveModel.ContentDown = "行车右移";
                byteToSend = new byte[10] { 80, 33, 9, 11, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                this.drCarMoveModel.IsChecked = true;
                this.drCarMoveModel.ContentDown = "行车左移";
                byteToSend = new byte[10] { 80, 33, 9, 1, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 平台选择 -停用
        /// </summary>
        private void btn_SelectType(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.SelectType.IsChecked)
            {
                byteToSend = new byte[10] { 1, 32, 1, 11, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 1, 32, 1, 10, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 控制模式
        /// </summary>
        private void btn_drTelecontrolModel(object sender, EventArgs e)
        {
            byte[] drbyteToSend;
            byte[] sirbyteToSend;
            byte[] sfbyteToSend;
            if (this.drTelecontrolModel.IsChecked) // 司钻切遥控
            {
               
                drbyteToSend = new byte[10] { 1, 32, 2, 21, 0, 0, 0, 0, 0, 0 }; // 扶杆臂-司钻切遥控
                sirbyteToSend = new byte[10] { 23, 17, 10, 1, 0, 0, 0, 0, 0, 0 }; // 铁钻工-遥控切司钻
                sfbyteToSend = new byte[10] { 16, 1, 27, 1, 1, 0, 0, 0, 0, 0 };// 铁架工-遥控切司钻
                GlobalData.Instance.da.SendBytes(drbyteToSend);
                Thread.Sleep(50);
                GlobalData.Instance.da.SendBytes(sirbyteToSend);
                Thread.Sleep(50);
                GlobalData.Instance.da.SendBytes(sfbyteToSend);
            }
            else // 遥控切司钻
            {
                drbyteToSend = new byte[10] { 1, 32, 2, 20, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(drbyteToSend);
            }

         
        }

        /// <summary>
        /// 扶杆臂-发送目的地设置
        /// </summary>
        private void btn_SelectDes(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 33, 11, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 设置钻杆
        /// </summary>
        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte>() {3, (byte)tag });
            GlobalData.Instance.da.SendBytes(byteToSend);
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
            try
            {
                //ChannelInfo info3 = GetConfigPara("CAMERA3");
                //ChannelInfo info4 = GetConfigPara("CAMERA4");
                //if (info3 != null)
                //{
                //    info3.ID = 3;
                //    GlobalData.Instance.chList.Add(info3);
                //}
                //if (info4 != null)
                //{
                //    info4.ID = 4;
                //    GlobalData.Instance.chList.Add(info4);
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
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
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
        /// 摄像头3播放
        /// </summary>
        private void Button_CameraStart(object sender, RoutedEventArgs e)
        {
            try
            {
                gridCamera1.Children.Clear();
                ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 3).FirstOrDefault();
                CameraVideoStop1();
                ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 3).FirstOrDefault();
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
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private void Button_CameraStart2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gridCamera2.Children.Count > 0)
                {
                    ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 4).FirstOrDefault();
                    CameraVideoStop2();
                    gridCamera2.Children.Clear();
                    ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 4).FirstOrDefault();
                    cameraTwo.InitCamera(info);
                    CameraVideoStart2();
                    //cameraTwo.SetSize(288, 352);
                    cameraTwo.SetSize(300, 400);
                    if (cameraTwo is UIControl_HBGK1)
                    {
                        gridCamera2.Children.Add(cameraTwo as UIControl_HBGK1);
                        //(cameraTwo as UIControl_HBGK1).SetValue(Grid.RowProperty, 0);
                        //(cameraTwo as UIControl_HBGK1).SetValue(Grid.ColumnProperty, 0);
                    }
                    else if (cameraTwo is YiTongCameraControl)
                    {
                        gridCamera2.Children.Add(cameraTwo as YiTongCameraControl);
                        //(cameraTwo as YiTongCameraControl).SetValue(Grid.RowProperty, 0);
                        //(cameraTwo as YiTongCameraControl).SetValue(Grid.ColumnProperty, 0);
                    }
                    else
                    {
                        gridCamera2.Children.Add(cameraInitImage1);
                    }
                    viewboxCameral2.Height = 300;
                    viewboxCameral2.Width = 403;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void CameraVideoStop1()
        {
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 3).FirstOrDefault();
            cameraOne.StopCamera();
            cameraSaveThreadTimer1.Change(Timeout.Infinite, 60000);
        }

        private void CameraVideoStop2()
        {
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 4).FirstOrDefault();
            cameraOne.StopCamera();
            cameraSaveThreadTimer2.Change(Timeout.Infinite, 60000);
        }

        private void CameraVideoStart1()
        {
            cameraSaveThreadTimer1.Change(0, 60000);
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
                string filePath = str1 + "\\video" + "\\video3";
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
                ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 3).FirstOrDefault();
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
                string filePath = str1 + "\\video" + "\\video4";
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
                ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 4).FirstOrDefault();
                if (cameraTwo == null) return;
                if (cameraTwo.Info.CameraType == 0)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
                }
                else if (cameraTwo.Info.CameraType == 1)
                {
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".h264";
                }
                else return;
                cameraTwo.StopFile();
                cameraTwo.SaveFile(filePath, fileName);
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
        /// 切换视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_CameraExchange(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (ICameraFactory cam in GlobalData.Instance.cameraList)
                {
                    if (cam.Info.ID == 3) cam.Info.ID = 4;
                    else if (cam.Info.ID == 4) cam.Info.ID = 3;
                }

                gridCamera1.Children.Clear();
                gridCamera2.Children.Clear();

                ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 3).FirstOrDefault();
                //cameraOne.SetSize(300, 400);
                if (cameraOne is UIControl_HBGK1)
                {
                    gridCamera1.Children.Add(cameraOne as UIControl_HBGK1);
                }
                else if (cameraOne is YiTongCameraControl)
                {
                    gridCamera1.Children.Add(cameraOne as YiTongCameraControl);
                }
                else
                {
                    gridCamera1.Children.Add(cameraInitImage1);
                }
                ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 4).FirstOrDefault();
                if (cameraTwo is UIControl_HBGK1)
                {
                    gridCamera2.Children.Add(cameraTwo as UIControl_HBGK1);
                }
                else if (cameraTwo is YiTongCameraControl)
                {
                    gridCamera2.Children.Add(cameraTwo as YiTongCameraControl);
                }
                else
                {
                    gridCamera2.Children.Add(cameraInitImage1);
                }
                //viewboxCameral1.Height = 300;
                //viewboxCameral1.Width = 403;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 全屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_CameraFullScreen(object sender, RoutedEventArgs e)
        {
            //if (DRFullScreenEvent != null)
            //{
            //    DRFullScreenEvent();
            //}
        }

        /// <summary>
        /// 播放视频定时器
        /// </summary>
        private void CameraTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            cameraTimer.Stop();
            PlayCamera();
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

        private void PlayOneAction()
        {
            try
            {
                DRCameraFullScreen.Instance.gridCamera1.Children.Clear();
                ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 3).FirstOrDefault();
                if (cameraOne == null) return;
                CameraVideoStop1();
                ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 3).FirstOrDefault();

                cameraOne.SetSize(220, 380);
                bool isPlay = cameraOne.InitCamera(info);
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
                cameraOne.ChangeVideoEvent -= CameraTwo_ChangeVideoEvent;
                cameraOne.FullScreenEvent -= CameraOne_FullScreenEvent;
                cameraOne.ChangeVideoEvent += CameraTwo_ChangeVideoEvent;
                cameraOne.FullScreenEvent += CameraOne_FullScreenEvent;
                CameraVideoStart1();
                //viewboxCameral1.Height = 300;
                //viewboxCameral1.Width = 403;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void CameraOne_FullScreenEvent()
        {
            if (DRFullScreenEvent != null)
            {
                DRFullScreenEvent(3);
            }
        }

        private delegate void PayTwoDelegate();

        private void PlayTwoAction()
        {
            try
            {
                ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 4).FirstOrDefault();
                if (cameraTwo == null) return;
                CameraVideoStop2();
                ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 4).FirstOrDefault();
                //cameraTwo.SetSize(130, 200);
                cameraTwo.SetSize(220, 380);
                bool isPlay = cameraTwo.InitCamera(info);
                if (isPlay)
                {
                    gridCamera2.Children.Clear();
                    if (cameraTwo is UIControl_HBGK1)
                    {
                        gridCamera2.Children.Add(cameraTwo as UIControl_HBGK1);
                        //(cameraTwo as UIControl_HBGK1).SetValue(Grid.RowProperty, 0);
                        //(cameraTwo as UIControl_HBGK1).SetValue(Grid.ColumnProperty, 0);
                    }
                    else if (cameraTwo is YiTongCameraControl)
                    {
                        gridCamera2.Children.Add(cameraTwo as YiTongCameraControl);
                        //(cameraTwo as YiTongCameraControl).SetValue(Grid.RowProperty, 0);
                        //(cameraTwo as YiTongCameraControl).SetValue(Grid.ColumnProperty, 0);
                    }
                    else
                    {
                        gridCamera2.Children.Add(cameraInitImage1);
                    }
                }
                cameraTwo.ChangeVideoEvent -= CameraTwo_ChangeVideoEvent;
                cameraTwo.FullScreenEvent -= CameraTwo_FullScreenEvent;
                cameraTwo.ChangeVideoEvent += CameraTwo_ChangeVideoEvent;
                cameraTwo.FullScreenEvent += CameraTwo_FullScreenEvent;
                CameraVideoStart2();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void CameraTwo_FullScreenEvent()
        {
            if (DRFullScreenEvent != null)
            {
                DRFullScreenEvent(4);
            }
        }

        private void CameraTwo_ChangeVideoEvent()
        {
            try
            {
                foreach (ICameraFactory cam in GlobalData.Instance.cameraList)
                {
                    if (cam.Info.ID == 3) cam.Info.ID = 4;
                    else if (cam.Info.ID == 4) cam.Info.ID = 3;
                }

                gridCamera1.Children.Clear();
                gridCamera2.Children.Clear();

                ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 3).FirstOrDefault();
                //cameraOne.SetSize(300, 400);
                if (cameraOne is UIControl_HBGK1)
                {
                    gridCamera1.Children.Add(cameraOne as UIControl_HBGK1);
                }
                else if (cameraOne is YiTongCameraControl)
                {
                    gridCamera1.Children.Add(cameraOne as YiTongCameraControl);
                }
                else
                {
                    gridCamera1.Children.Add(cameraInitImage1);
                }
                ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 4).FirstOrDefault();
                if (cameraTwo is UIControl_HBGK1)
                {
                    gridCamera2.Children.Add(cameraTwo as UIControl_HBGK1);
                }
                else if (cameraTwo is YiTongCameraControl)
                {
                    gridCamera2.Children.Add(cameraTwo as YiTongCameraControl);
                }
                else
                {
                    gridCamera2.Children.Add(cameraInitImage1);
                }
                //viewboxCameral1.Height = 300;
                //viewboxCameral1.Width = 403;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        #endregion

        private void btn_CarToZero(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 13, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btn_RotateToZero(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 13, 3 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 清除钻杆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearDrill(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("确认抓手已无钻杆，并清除此状态?", "提示信息", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 10, 1 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        private void btnDeviceBack(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 33, 1, 6, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井口记忆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWellMemory(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 33, 16, 2, 1, 1, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 记忆清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemoryClear(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 33, 16, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 记忆恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWellMemoryRecovery(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 33, 16, 2, 0, 1, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
