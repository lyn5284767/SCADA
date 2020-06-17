using COM.Common;
using ControlLibrary;
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
        public IngMain()
        {
            InitializeComponent();
            this.amination.CIMSChange(SystemType.SecondFloor);
            amination.SendFingerBeamNumberEvent += Amination_SendFingerBeamNumberEvent;
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
                this.rotateAngle.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });
                this.bigHookRealTimeValue.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["156To159BigHookEncoderRealTimeValue"], Mode = BindingMode.OneWay });
                this.bigHookCalibrationValue.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["160To163BigHookEncoderCalibrationValue"], Mode = BindingMode.OneWay });
                this.leftFinger.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23LeftFingerMotorSampleValue"], Mode = BindingMode.OneWay });
                this.rightFinger.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23RightFingerMotorSampleValue"], Mode = BindingMode.OneWay });
                this.gripMotor.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23GripMotorSampleValue"], Mode = BindingMode.OneWay });

                this.carMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.armMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rotateMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["rotateMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.carMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
                this.armMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
                this.rotateMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["rotateMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });

                this.operateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.operateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
                this.workMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
                this.workMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelIsCheckConverter() });

                RopeModelConverter RopeModeleMultiConverter = new RopeModelConverter();
                MultiBinding RopeModelMultiBind = new MultiBinding();
                RopeModelMultiBind.Converter = RopeModeleMultiConverter;
                RopeModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b0"], Mode = BindingMode.OneWay });
                RopeModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b1"], Mode = BindingMode.OneWay });
                RopeModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b2"], Mode = BindingMode.OneWay });
                RopeModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b3"], Mode = BindingMode.OneWay });
                RopeModelMultiBind.NotifyOnSourceUpdated = true;
                this.RopeModel.SetBinding(BasedSwitchButton.ContentDownProperty, RopeModelMultiBind);

                RopeModelIsCheckConverter RopeModelIsCheckMultiConverter = new RopeModelIsCheckConverter();
                MultiBinding RopeModelIsCheckMultiBind = new MultiBinding();
                RopeModelIsCheckMultiBind.Converter = RopeModelIsCheckMultiConverter;
                RopeModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b0"], Mode = BindingMode.OneWay });
                RopeModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b1"], Mode = BindingMode.OneWay });
                RopeModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b2"], Mode = BindingMode.OneWay });
                RopeModelIsCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b3"], Mode = BindingMode.OneWay });
                RopeModelIsCheckMultiBind.NotifyOnSourceUpdated = true;
                this.RopeModel.SetBinding(BasedSwitchButton.IsCheckedProperty, RopeModelIsCheckMultiBind);
                this.tubeType.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drillPipeType"], Mode = BindingMode.OneWay, Converter = new DrillPipeTypeConverter() });

                #region 钻台面变量
                this.drcarMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drRotateMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drcarMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drRotateMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                this.drgripMotor.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["gridSample"], Mode = BindingMode.OneWay });

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
                #endregion

                this.tbLink.SetBinding(ShadowButton.ShadowButtonShowTxtProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay, Converter = new LinkOpenOrCloseConverter() });
                this.drDestination.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDes"], Mode = BindingMode.OneWay, Converter = new DesTypeConverter() });

                MultiBinding LinkErrorMultiBind = new MultiBinding();
                LinkErrorMultiBind.Converter = new LinkErrorCoverter();
                LinkErrorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drLinkError"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b1"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b2"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.NotifyOnSourceUpdated = true;
                this.LinkError.SetBinding(TextBlock.TextProperty, LinkErrorMultiBind);
                // 自动步骤-自动控件
                MultiBinding IngStepMultiBind = new MultiBinding();
                IngStepMultiBind.Converter = new IngStepCoverter();
                IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
                IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
                IngStepMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
                IngStepMultiBind.NotifyOnSourceUpdated = true;
                this.IngStep.SetBinding(StepControl.SelectStepProperty, IngStepMultiBind);

                MultiBinding AutoStepCurrentTxtMultiBind = new MultiBinding();
                AutoStepCurrentTxtMultiBind.Converter = new IngAutoModeNowStepCoverter();
                AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
                AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
                AutoStepCurrentTxtMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
                AutoStepCurrentTxtMultiBind.NotifyOnSourceUpdated = true;
                this.AutoStepCurrentTxt.SetBinding(TextBlock.TextProperty, AutoStepCurrentTxtMultiBind);

                this.warningOne.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["155InterlockPromptMessageCode"], Mode = BindingMode.OneWay, Converter = new IngLockTipsCoverter() });
                timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }

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
                if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5 || GlobalData.Instance.da["droperationModel"].Value.Byte == 3)
                {
                    byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 5, number });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            //}
        }
        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    amination.LoadFingerBeamDrillPipe(systemType);
                    this.Warnning();
                    this.AutoChange();
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
            #region 告警1
            int warnOne = GlobalData.Instance.da["drTipsCode"].Value.Int32;
            if (warnOne == 1)
            {
                this.warningTwo.Text = "小车电机故障";
            }
            else if (warnOne == 3)
            {
                this.warningTwo.Text = "回转电机故障";
            }
            else if (warnOne == 6)
            {
                this.warningTwo.Text = "机械手禁止向井口移动";
            }
            else if (warnOne == 7)
            {
                this.warningTwo.Text = "禁止机械手缩回";
            }
            else if (warnOne == 11)
            {
                this.warningTwo.Text = "抓手不允许打开";
            }
            else if (warnOne == 12)
            {
                this.warningTwo.Text = "抓手不允许关闭";
            }
            else if (warnOne == 13)
            {
                this.warningTwo.Text = "请将小车移至井口";
            }
            else if (warnOne == 15)
            {
                this.warningTwo.Text = "小车不能往井口移动";
            }
            else if (warnOne == 20)
            {
                this.warningTwo.Text = "回转回零异常，检查传感器";
            }
            else if (warnOne == 21)
            {
                this.warningTwo.Text = "小车未回零或最大最小值限制";
            }
            else if (warnOne == 22 || warnOne == 23)
            {
                this.warningTwo.Text = "手臂回零或最大最小值限制";
            }
            else if (warnOne == 31)
            {
                this.warningTwo.Text = "抓手中右钻杆，请切换手动处理";
            }
            else if (warnOne == 32)
            {
                this.warningTwo.Text = "请先回收手臂";
            }
            else if (warnOne == 33)
            {
                this.warningTwo.Text = "小车回零中...";
            }
            else if (warnOne == 34)
            {
                this.warningTwo.Text = "请将手臂收到最小";
            }
            else if (warnOne == 35)
            {
                this.warningTwo.Text = "手臂回零中...";
            }
            else if (warnOne == 36)
            {
                this.warningTwo.Text = "请先回收小车";
            }
            else if (warnOne == 37)
            {
                this.warningTwo.Text = "回转回零中...";
            }
            else if (warnOne == 38)
            {
                this.warningTwo.Text = "请将小车移动到中间靠井口";
            }
            else if (warnOne == 39)
            {
                this.warningTwo.Text = "此轴已回零";
            }
            else if (warnOne == 40)
            {
                this.warningTwo.Text = "系统未回零，请谨慎操作";
            }
            else if (warnOne == 41)
            {
                this.warningTwo.Text = "请选择管柱类型";
            }
            else if (warnOne == 42)
            {
                this.warningTwo.Text = "请选择指梁";
            }
            else if (warnOne == 43)
            {
                this.warningTwo.Text = "请选择目的地";
            }
            else if (warnOne == 44)
            {
                this.warningTwo.Text = "管柱已满，请切换指梁";
            }
            else if (warnOne == 45)
            {
                this.warningTwo.Text = "请启动确认按键启动";
            }
            else if (warnOne == 46)
            {
                this.warningTwo.Text = "确定大钩上提";
            }
            else if (warnOne == 47)
            {
                this.warningTwo.Text = "抓手还未打开";
            }
            else if (warnOne == 48)
            {
                this.warningTwo.Text = "抓手动作中...";
            }
            else if (warnOne == 49)
            {
                this.warningTwo.Text = "请确认井口安全";
            }
            else if (warnOne == 50)
            {
                this.warningTwo.Text = "确认上管柱已脱离下管柱";
            }
            else if (warnOne == 51)
            {
                this.warningTwo.Text = "确认抓手中已有管柱";
            }
            else if (warnOne == 52)
            {
                this.warningTwo.Text = "确认定位完成，大钩下落，钻杆入盒";
            }
            else if (warnOne == 53)
            {
                this.warningTwo.Text = "请确认抓手已打开";
            }
            else if (warnOne == 54)
            {
                this.warningTwo.Text = "自动排管动作完成";
            }
            else if (warnOne == 55)
            {
                this.warningTwo.Text = "此指梁无钻杆";
            }
            else if (warnOne == 56)
            {
                this.warningTwo.Text = "自动送杆抓干失败，请确认抓手有钻杆";
            }
            else if (warnOne == 57)
            {
                this.warningTwo.Text = "抓手未关闭";
            }
            else if (warnOne == 58)
            {
                this.warningTwo.Text = "确认定位完成，大钩下落，上下管柱接触";
            }
            else if (warnOne == 59)
            {
                this.warningTwo.Text = "需要人工确认吊卡关闭";
            }
            else if (warnOne == 60)
            {
                this.warningTwo.Text = "自动送杆动作完成";
            }
            else if (warnOne == 61)
            {
                this.warningTwo.Text = "确认启动回收模式";
            }
            else if (warnOne == 62)
            {
                this.warningTwo.Text = "回收完成";
            }
            else if (warnOne == 65)
            {
                this.warningTwo.Text = "请先回收手臂";
            }
            else if (warnOne == 71)
            {
                this.warningTwo.Text = "请启动运输模式";
            }
            else if (warnOne == 72)
            {
                this.warningTwo.Text = "已完成运输模式";
            }
            else if (warnOne == 81)
            {
                this.warningTwo.Text = "两点距离太近";
            }
            else if (warnOne == 82)
            {
                this.warningTwo.Text = "双轴运动";
            }
            else if (warnOne == 83)
            {
                this.warningTwo.Text = "记录点数超限";
            }
            else if (warnOne == 84)
            {
                this.warningTwo.Text = "记录点数为零";
            }
            else if (warnOne == 85)
            {
                this.warningTwo.Text = "即将进入下一次示教";
            }
            else if (warnOne == 91)
            {
                this.warningTwo.Text = "小车补偿超出范围";
            }
            else if (warnOne == 93)
            {
                this.warningTwo.Text = "回转补偿超出范围";
            }
            else if (warnOne == 101)
            {
                this.warningTwo.Text = "盖板未打开，无法向右平移";
            }
            else if (warnOne == 102)
            {
                this.warningTwo.Text = "盖板未打开，无法向左平移";
            }
            else if (warnOne == 103)
            {
                this.warningTwo.Text = "机械手在左侧，无法向左平移";
            }
            else if (warnOne == 104)
            {
                this.warningTwo.Text = "机械手在右侧，无法向右平移";
            }
            else
            {
                this.warningTwo.Text = "";
            }
            #endregion

            //操作台控制器心跳
            if (GlobalData.Instance.da["508b5"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 600)
                {
                    //Communication = 2;
                    //this.warningTwo.Text = "操作台信号中断";
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

            if (!GlobalData.Instance.ComunciationNormal) this.warningTwo.Text = "网络连接失败！";
        }
        /// <summary>
        /// 集成系统自动步骤切换页面
        /// </summary>
        private void AutoChange()
        {
            try
            {
                if (GlobalData.Instance.da["460b0"].Value.Boolean) // 联动开启
                {
                    if (GlobalData.Instance.da["operationModel"].Value.Byte == 5 && GlobalData.Instance.da["droperationModel"].Value.Byte == 5) // 自动模式
                    {
                        if (GlobalData.Instance.da["workModel"].Value.Byte == 2 && GlobalData.Instance.da["drworkModel"].Value.Byte == 2) // 排管
                        {
                            if (this.IngStep.SelectStep <= 6 && systemType == SystemType.SecondFloor) // 自动操作钻台面但界面处于二层台界面，切换钻台面界面
                            {
                                DRSelectMouseDown(null, null);
                            }
                            else if (this.IngStep.SelectStep > 6 && systemType == SystemType.DrillFloor)
                            {
                                sfSelectMouseDown(null, null);
                            }
                        }
                        else if (GlobalData.Instance.da["workModel"].Value.Byte == 2 && GlobalData.Instance.da["drworkModel"].Value.Byte == 1)// 送杆
                        {
                            if (this.IngStep.SelectStep <= 7 && systemType == SystemType.DrillFloor)
                            {
                                sfSelectMouseDown(null, null);
                            }
                            else if (this.IngStep.SelectStep > 7 && systemType == SystemType.SecondFloor)
                            {
                                DRSelectMouseDown(null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 二层台-一键回零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllMotorRetZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 二层台-电机使能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MotorEnable(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 二层台-操作模式
        /// </summary>
        private void btn_OpState(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.operateMode.IsChecked)
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 4 });
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 5 });
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 二层台-工作模式
        /// </summary>
        private void btn_WorkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (workMode.IsChecked)
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 二层台-档绳操作
        /// </summary>
        private void btn_RopeModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (workMode.IsChecked)
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 8, 3 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 8, 6 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        #region 管柱类型选择
        ///// <summary>
        ///// 3.5寸钻杆
        ///// </summary>
        //private void btn_SelectDrillPipe312(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 35 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);

        //}

        ///// <summary>
        ///// 4寸钻杆
        ///// </summary>
        //private void btn_SelectDrillPipe4(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 40 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}

        ///// <summary>
        ///// 4.5寸钻杆
        ///// </summary>
        //private void btn_SelectDrillPipe412(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 45 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}

        ///// <summary>
        ///// 5寸钻杆
        ///// </summary>
        //private void btn_SelectDrillPipe5(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 50 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}

        ///// <summary>
        ///// 5.5寸钻杆
        ///// </summary>
        //private void btn_SelectDrillPipe55(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 55 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        ///// <summary>
        ///// 5 7/8寸钻杆
        ///// </summary>
        //private void btn_SelectDrillPipe578(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 57 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        ///// <summary>
        ///// 6 5/8寸钻杆
        ///// </summary>
        //private void btn_SelectDrillPipe658(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 68 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        ///// <summary>
        ///// 6寸钻铤
        ///// </summary>
        //private void btn_SelectDrillPipe6(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 60 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        ///// <summary>
        ///// 6.5寸钻铤
        ///// </summary>
        //private void btn_SelectDrillPipe65(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 65 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        ///// <summary>
        ///// 7寸钻铤
        ///// </summary>
        //private void btn_SelectDrillPipe7(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 70 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        ///// <summary>
        ///// 7.5寸钻铤
        ///// </summary>
        //private void btn_SelectDrillPipe75(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 75 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        ///// <summary>
        ///// 8寸钻铤
        ///// </summary>
        //private void btn_SelectDrillPipe8(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 80 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        ///// <summary>
        ///// 9寸钻铤
        ///// </summary>
        //private void btn_SelectDrillPipe9(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 90 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        ///// <summary>
        ///// 10寸钻铤
        ///// </summary>
        //private void btn_SelectDrillPipe10(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 100 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        ///// <summary>
        ///// 11寸钻铤
        ///// </summary>
        //private void btn_SelectDrillPipe11(object sender, RoutedEventArgs e)
        //{
        //    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, 110 });
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}


        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, (byte)tag });
            GlobalData.Instance.da.SendBytes(byteToSend);

            byteToSend = GlobalData.Instance.SendToDR(new List<byte>() { 3, (byte)tag });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        /// <summary>
        /// 钻台面-操作模式
        /// </summary>
        private void btn_drOpState(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.droperateMode.IsChecked)
            {
                byteToSend = new byte[10] { 1, 32, 3, 31, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钻台面-工作模式
        /// </summary>
        private void btn_drWorkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.drworkMode.IsChecked)
            {
                byteToSend = new byte[10] { 1, 32, 4, 41, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 1, 32, 4, 40, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钻台面-行车模式
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
        /// 钻台面-一键回零
        /// </summary>
        private void btn_drAllMotorRetZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钻台面-电机使能
        /// </summary>
        private void btn_drMotorEnable(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 6, 2});
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 选择二层台
        /// </summary>
        private void sfSelectMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 1, 32, 1, 11, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            systemType = SystemType.SecondFloor;
            this.amination.CIMSChange(systemType);
            this.amination.InitRowsColoms(systemType);
            this.effSFSelect.Opacity = 0.5;
            this.effDRSelect.Opacity = 0;
        }
        /// <summary>
        /// 选择钻台面
        /// </summary>
        private void DRSelectMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 1, 32, 1, 10, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            systemType = SystemType.DrillFloor;
            this.amination.CIMSChange(systemType);
            this.amination.InitRowsColoms(systemType);
            this.effSFSelect.Opacity = 0;
            this.effDRSelect.Opacity = 0.5;
        }
        /// <summary>
        /// 联动开启关闭
        /// </summary>
        private void LinkOpenOrCloseMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            if (tbLink.TBText == "联动开启")
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 0, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        /// <summary>
        /// 钻台面-发送目的地设置
        /// </summary>
        private void btn_SelectDes(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 33, 11, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
