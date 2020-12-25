using COM.Common;
using ControlLibrary;
using DevExpress.Mvvm.Native;
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

namespace Main.WellRepair.DrillFloor
{
    /// <summary>
    /// WP_DRMain.xaml 的交互逻辑
    /// </summary>
    public partial class WR_DRMain : UserControl
    {
        private static WR_DRMain _instance = null;
        private static readonly object syncRoot = new object();

        public static WR_DRMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new WR_DRMain();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        public WR_DRMain()
        {
            InitializeComponent();

            VariableBinding();
            this.Loaded += WR_DRMain_Loaded;
            timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//50ms 的时钟
            this.wr_Amination.SendFingerBeamNumberEvent += Amination_SendFingerBeamNumberEvent;
        }

        private void VariableBinding()
        {
            try
            {
                //this.carPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drCarPos"], Mode = BindingMode.OneWay, Converter = new CarPosCoverter() });//小车位置
                this.carPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drCarPos"], Mode = BindingMode.OneWay });//小车位置                                                                                                                                                             //this.armPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drArmPos"], Mode = BindingMode.OneWay, Converter = new ArmPosCoverter() });//手臂实际位置
                this.armPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drArmPos"], Mode = BindingMode.OneWay });//手臂实际位置
                this.rotatePos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drRotePos"], Mode = BindingMode.OneWay, Converter = new DRCallAngleConverter() });//回转角度
                this.gripMotor.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drgripStatus"], Mode = BindingMode.OneWay, Converter = new GripConverter() });//抓手状态

                this.drcarMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drArmMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drRotateMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drcarMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drArmMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.drRotateMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //强制是否开启
                MultiBinding tbForceMultiBind = new MultiBinding();
                tbForceMultiBind.Converter = new WR_ForceCoverter();
                tbForceMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["335b0"], Mode = BindingMode.OneWay });
                tbForceMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["335b1"], Mode = BindingMode.OneWay });
                tbForceMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["335b2"], Mode = BindingMode.OneWay });
                tbForceMultiBind.NotifyOnSourceUpdated = true;
                this.tbForce.SetBinding(TextBlock.TextProperty, tbForceMultiBind);

                this.droperateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.droperateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
                this.drworkMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new DRWorkModelConverter() });
                this.drworkMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new DRWorkModelIsCheckConverter() });

                this.drDestination.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDes"], Mode = BindingMode.OneWay, Converter = new DesTypeConverter() });// 目的地选择
                this.tubeType.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drdrillPipeType"], Mode = BindingMode.OneWay, Converter = new DrillPipeTypeConverter() });// 管柱选择

                this.drTelecontrolModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["325b1"], Mode = BindingMode.OneWay, Converter = new TelecontrolModelConverter() });
                this.drTelecontrolModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["325b1"], Mode = BindingMode.OneWay, Converter = new TelecontrolModelIsCheckConverter() });
                //送杆
                MultiBinding sbDrillUpMultiBind = new MultiBinding();
                sbDrillUpMultiBind.Converter = new WR_DRStepCoverter();
                sbDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                sbDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                sbDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
                sbDrillUpMultiBind.NotifyOnSourceUpdated = true;
                this.sbDrillUp.SetBinding(StepBar.StepIndexProperty, sbDrillUpMultiBind);
                // 排杆
                MultiBinding sbDrillDownMultiBind = new MultiBinding();
                sbDrillDownMultiBind.Converter = new WR_DRStepCoverter();
                sbDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                sbDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                sbDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
                sbDrillDownMultiBind.NotifyOnSourceUpdated = true;
                this.sbDrillDown.SetBinding(StepBar.StepIndexProperty, sbDrillDownMultiBind);

                timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        System.Timers.Timer pageChange;
        int count = 0; // 进入页面发送协议次数
        private void WR_DRMain_Loaded(object sender, RoutedEventArgs e)
        {
            // 钻台面主界面
            //byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 30 };
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);
            count = 0;
            GlobalData.Instance.DRNowPage = "DRMain";
            pageChange = new System.Timers.Timer(500);
            pageChange.Elapsed += PageChange_Elapsed;
            pageChange.Enabled = true;

            this.wr_Amination.InitRowsColoms();
        }

        /// <summary>
        /// 切换页面发送指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageChange_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            count++;
            if (GlobalData.Instance.da["drPageNum"].Value.Byte == 30 || count > 5 || GlobalData.Instance.DRNowPage != "DRMain")
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

        private int iTimeCnt = 0;//用来为时钟计数的变量

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.wr_Amination.LoadFingerBeamDrillPipe();
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Warnning();
                    if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5)
                    {
                        if (GlobalData.Instance.da["drworkModel"].Value.Byte == 2)
                        {
                            this.sbDrillUp.Visibility = Visibility.Visible;
                            this.sbDrillDown.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            this.sbDrillUp.Visibility = Visibility.Collapsed;
                            this.sbDrillDown.Visibility = Visibility.Visible;
                        }
                    }

                    if (GlobalData.Instance.da["droperationModel"].Value.Byte == 1)
                    {
                        this.tbAlarm.Text = "当前系统为紧急停止状态";
                    }
                    else
                    {
                        if (this.tbAlarm.Text == "当前系统为紧急停止状态")
                            this.tbAlarm.Text = "";
                    }


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
        List<AlarmInfo> alarmList = new List<AlarmInfo>();
        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
            #region 操作提示
            int warnOne = GlobalData.Instance.da["drTipsCode"].Value.Int32;
            if (warnOne == 1)
            {
                this.tbOpr.Text = "小车电机故障";
            }
            else if (warnOne == 3)
            {
                this.tbOpr.Text = "回转电机故障";
            }
            else if (warnOne == 6)
            {
                this.tbOpr.Text = "机械手禁止向井口移动";
            }
            else if (warnOne == 7)
            {
                this.tbOpr.Text = "禁止机械手缩回";
            }
            else if (warnOne == 11)
            {
                this.tbOpr.Text = "抓手不允许打开";
            }
            else if (warnOne == 12)
            {
                this.tbOpr.Text = "抓手不允许关闭";
            }
            else if (warnOne == 13)
            {
                this.tbOpr.Text = "请将小车移至井口";
            }
            else if (warnOne == 15)
            {
                this.tbOpr.Text = "小车不能往井口移动";
            }
            else if (warnOne == 20)
            {
                this.tbOpr.Text = "回转回零异常，检查传感器";
            }
            else if (warnOne == 21)
            {
                this.tbOpr.Text = "小车未回零或最大最小值限制";
            }
            else if (warnOne == 22 || warnOne == 23)
            {
                this.tbOpr.Text = "手臂回零或最大最小值限制";
            }
            else if (warnOne == 31)
            {
                this.tbOpr.Text = "抓手中有钻杆，请切换手动处理";
            }
            else if (warnOne == 32)
            {
                this.tbOpr.Text = "请先回收手臂";
            }
            else if (warnOne == 33)
            {
                this.tbOpr.Text = "小车回零中...";
            }
            else if (warnOne == 34)
            {
                this.tbOpr.Text = "请将手臂收到最小";
            }
            else if (warnOne == 35)
            {
                this.tbOpr.Text = "手臂回零中...";
            }
            else if (warnOne == 36)
            {
                this.tbOpr.Text = "请先回收小车";
            }
            else if (warnOne == 37)
            {
                this.tbOpr.Text = "回转回零中...";
            }
            else if (warnOne == 38)
            {
                this.tbOpr.Text = "请将小车移动到中间靠井口";
            }
            else if (warnOne == 39)
            {
                this.tbOpr.Text = "此轴已回零";
            }
            else if (warnOne == 40)
            {
                this.tbOpr.Text = "系统未回零，请谨慎操作";
            }
            else if (warnOne == 41)
            {
                this.tbOpr.Text = "请选择管柱类型";
            }
            else if (warnOne == 42)
            {
                this.tbOpr.Text = "请选择指梁";
            }
            else if (warnOne == 43)
            {
                this.tbOpr.Text = "请选择目的地";
            }
            else if (warnOne == 44)
            {
                this.tbOpr.Text = "管柱已满，请切换指梁";
            }
            else if (warnOne == 45)
            {
                this.tbOpr.Text = "请启动确认按键启动";
            }
            else if (warnOne == 46)
            {
                this.tbOpr.Text = "确定大钩上提";
            }
            else if (warnOne == 47)
            {
                this.tbOpr.Text = "抓手还未打开";
            }
            else if (warnOne == 48)
            {
                this.tbOpr.Text = "抓手动作中...";
            }
            else if (warnOne == 49)
            {
                this.tbOpr.Text = "请确认井口安全";
            }
            else if (warnOne == 50)
            {
                this.tbOpr.Text = "确认上管柱已脱离下管柱";
            }
            else if (warnOne == 51)
            {
                this.tbOpr.Text = "确认抓手中已有管柱";
            }
            else if (warnOne == 52)
            {
                this.tbOpr.Text = "确认定位完成，大钩下落，钻杆入盒";
            }
            else if (warnOne == 53)
            {
                this.tbOpr.Text = "请确认抓手已打开";
            }
            else if (warnOne == 54)
            {
                this.tbOpr.Text = "自动排管动作完成";
            }
            else if (warnOne == 55)
            {
                this.tbOpr.Text = "此指梁无钻杆";
            }
            else if (warnOne == 56)
            {
                this.tbOpr.Text = "自动送杆抓杆失败，请确认抓手有钻杆";
            }
            else if (warnOne == 57)
            {
                this.tbOpr.Text = "抓手未关闭";
            }
            else if (warnOne == 58)
            {
                this.tbOpr.Text = "确认定位完成，大钩下落，上下管柱接触";
            }
            else if (warnOne == 59)
            {
                this.tbOpr.Text = "需要人工确认吊卡关闭";
            }
            else if (warnOne == 60)
            {
                this.tbOpr.Text = "自动送杆动作完成";
            }
            else if (warnOne == 61)
            {
                this.tbOpr.Text = "确认启动回收模式";
            }
            else if (warnOne == 62)
            {
                this.tbOpr.Text = "回收完成";
            }
            else if (warnOne == 65)
            {
                this.tbOpr.Text = "请先回收手臂";
            }
            else if (warnOne == 71)
            {
                this.tbOpr.Text = "请启动运输模式";
            }
            else if (warnOne == 72)
            {
                this.tbOpr.Text = "已完成运输模式";
            }
            else if (warnOne == 81)
            {
                this.tbOpr.Text = "两点距离太近";
            }
            else if (warnOne == 82)
            {
                this.tbOpr.Text = "双轴运动";
            }
            else if (warnOne == 83)
            {
                this.tbOpr.Text = "记录点数超限";
            }
            else if (warnOne == 84)
            {
                this.tbOpr.Text = "记录点数为零";
            }
            else if (warnOne == 85)
            {
                this.tbOpr.Text = "即将进入下一次示教";
            }
            else if (warnOne == 91)
            {
                this.tbOpr.Text = "小车补偿超出范围";
            }
            else if (warnOne == 93)
            {
                this.tbOpr.Text = "回转补偿超出范围";
            }
            else if (warnOne == 101)
            {
                this.tbOpr.Text = "盖板未打开，无法向右平移";
            }
            else if (warnOne == 102)
            {
                this.tbOpr.Text = "盖板未打开，无法向左平移";
            }
            else if (warnOne == 103)
            {
                this.tbOpr.Text = "机械手在左侧，无法向左平移";
            }
            else if (warnOne == 104)
            {
                this.tbOpr.Text = "机械手在右侧，无法向右平移";
            }
            else
            {
                this.tbOpr.Text = "";
            }
            #endregion


            #region 告警3
            if (iTimeCnt % 10 == 0)
            {
                this.tbAlarm.FontSize = 18;
                this.tbAlarm.Visibility = Visibility.Visible;
                // 告警列表!=0则有告警 
                if (alarmList.Where(w => w.NowType != 0).Count() > 0)
                {

                    // 有告警且全部显示完成
                    if (this.alarmList.Where(w => w.NowType == 1).Count() == 0)
                    {
                        this.alarmList.ForEach(w => w.NowType = 1);
                    }
                    AlarmInfo tmp = this.alarmList.Where(w => w.NowType == 1).FirstOrDefault();
                    if (tmp != null)
                    {
                        this.tbAlarm.FontSize = 18;
                        this.tbAlarm.Text = tmp.Description;
                        tmp.NowType = 2;
                    }
                }
            }
            else
            {
                this.tbAlarm.FontSize = 24;
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

            if (!GlobalData.Instance.ComunciationNormal) this.tbAlarm.Text = "网络连接失败！";
            else
            {
                if (this.tbAlarm.Text == "网络连接失败！") this.tbAlarm.Text = "";
            }
        }

        private void InitAlarmKey()
        {
            alarmList.Add(new AlarmInfo() { TagName = "drErrorCode1", Description = "小车电机故障", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "drErrorCode3", Description = "回转电机故障", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "drErrorCode6", Description = "机械手禁止向井口移动", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "drErrorCode7", Description = "禁止机械手缩回", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "drErrorCode11", Description = "抓手不允许打开", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "drErrorCode12", Description = "抓手不允许关闭", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "drErrorCode13", Description = "小车不允许在此位置", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "UnReZero", Description = "系统未回零", NowType = 0 });

            alarmList.Add(new AlarmInfo() { TagName = "337b0", Description = "抓手传感器异常", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "337b1", Description = "抓手打开卡滞", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "337b2", Description = "抓手关闭卡滞", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "337b4", Description = "手臂伸出异常", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "337b5", Description = "手臂缩回异常", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "337b6", Description = "抓手传感器异常", NowType = 0, NeedCheck = true });

            alarmList.Add(new AlarmInfo() { TagName = "338b0", Description = "滑车驱动器通讯故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "338b1", Description = "手臂驱动器通讯故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "338b2", Description = "回转驱动器通讯故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "338b3", Description = "滑车运动检测异常", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "338b4", Description = "手臂运动检测异常", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "338b5", Description = "回转运动检测异常", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "338b6", Description = "遥控器通讯故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "338b7", Description = "遥控器系统故障", NowType = 0, NeedCheck = true });
        }

        /// <summary>
        /// 监控告警状态
        /// </summary>
        private void MonitorAlarmStatus()
        {
            int warnTwoNO = GlobalData.Instance.da["drErrorCode"].Value.Int32;
            if (warnTwoNO == 1) alarmList.Where(w => w.TagName == "drErrorCode1").FirstOrDefault().NowType = 1;
            else if (warnTwoNO == 3) alarmList.Where(w => w.TagName == "drErrorCode3").FirstOrDefault().NowType = 1;
            else if (warnTwoNO == 6) alarmList.Where(w => w.TagName == "drErrorCode6").FirstOrDefault().NowType = 1;
            else if (warnTwoNO == 7) alarmList.Where(w => w.TagName == "drErrorCode7").FirstOrDefault().NowType = 1;
            else if (warnTwoNO == 11) alarmList.Where(w => w.TagName == "drErrorCode11").FirstOrDefault().NowType = 1;
            else if (warnTwoNO == 12) alarmList.Where(w => w.TagName == "drErrorCode12").FirstOrDefault().NowType = 1;
            else if (warnTwoNO == 13) alarmList.Where(w => w.TagName == "drErrorCode13").FirstOrDefault().NowType = 1;
            else alarmList.Where(w => w.TagName.Contains("drErrorCode")).ForEach(o => o.NowType = 0);
            if (!GlobalData.Instance.da["324b0"].Value.Boolean
                || !GlobalData.Instance.da["324b2"].Value.Boolean
                || !GlobalData.Instance.da["324b4"].Value.Boolean)
            {
                alarmList.Where(w => w.TagName == "UnReZero").FirstOrDefault().NowType = 1;
            }
            else alarmList.Where(w => w.TagName == "UnReZero").FirstOrDefault().NowType = 0;
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
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 钻台面-操作模式
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
        /// 钻台面-工作模式
        /// </summary>
        private void btn_drWorkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.drworkMode.IsChecked)// 送杆
            {
                byteToSend = new byte[10] { 1, 32, 4, 41, 0, 0, 0, 0, 0, 0 };
            }
            else// 排杆
            {
                byteToSend = new byte[10] { 1, 32, 4, 40, 0, 0, 0, 0, 0, 0 };
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

                drbyteToSend = new byte[10] { 1, 32, 2, 21, 0, 0, 0, 0, 0, 0 }; // 钻台面-司钻切遥控
                sirbyteToSend = new byte[10] { 23, 17, 10, 1, 0, 0, 0, 0, 0, 0 }; // 铁钻工-遥控切司钻
                sfbyteToSend = new byte[10] { 16, 1, 27, 1, 1, 0, 0, 0, 0, 0 };// 二层台-遥控切司钻
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
        /// 钻台面-发送目的地设置
        /// </summary>
        private void btn_SelectDes(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 33, 11, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 设置钻杠
        /// </summary>
        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte>() { 3, (byte)tag });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

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

        private void Amination_SendFingerBeamNumberEvent(byte number)
        {
            if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5 || GlobalData.Instance.da["droperationModel"].Value.Byte == 3)
            {
                byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 5, number });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
