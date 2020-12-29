using COM.Common;
using ControlLibrary;
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

namespace Main.SIR.SanyRail
{
    /// <summary>
    /// SIRRailWayMain.xaml 的交互逻辑
    /// </summary>
    public partial class SIRRailWayMain : UserControl
    {
        private static SIRRailWayMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRRailWayMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRRailWayMain();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer ReportTimer;
        System.Threading.Timer timerWarning;
        public SIRRailWayMain()
        {
            InitializeComponent();
            VariableBinding();
            InitAlarmKey();
            ReportTimer = new System.Threading.Timer(new TimerCallback(ReportTimer_Elapse), null, 500, 2000);
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }
        private void VariableBinding()
        {
            try
            {
                #region 旋转开关
                this.oprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_OprModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayOperationModelConverter() });
                this.oprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_OprModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayIsCheckConverter() });
                this.workModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_WorkModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayWorkModelConverter() });
                this.workModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_WorkModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayIsCheckedConverter() });
                this.controlModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_ControlModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayControlModelConverter() });
                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_ControlModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayIsCheckConverter() });
                // 高低档切换
                //MultiBinding highOrLowModelMultiBind = new MultiBinding();
                //highOrLowModelMultiBind.Converter = new SIRRailWayHighOrLowCoverter();
                //highOrLowModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["811b0"], Mode = BindingMode.OneWay });
                //highOrLowModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["811b1"], Mode = BindingMode.OneWay });
                //highOrLowModelMultiBind.NotifyOnSourceUpdated = true;
                //this.highOrLowModel.SetBinding(BasedSwitchButton.ContentDownProperty, highOrLowModelMultiBind);
                //MultiBinding highOrLowModelCheckMultiBind = new MultiBinding();
                //highOrLowModelCheckMultiBind.Converter = new SIRRailWayHighOrLowCheckCoverter();
                //highOrLowModelCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["811b0"], Mode = BindingMode.OneWay });
                //highOrLowModelCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["811b1"], Mode = BindingMode.OneWay });
                //highOrLowModelCheckMultiBind.NotifyOnSourceUpdated = true;
                //this.highOrLowModel.SetBinding(BasedSwitchButton.IsCheckedProperty, highOrLowModelCheckMultiBind);
                #endregion

                #region 类型选择
                this.tbDrillType.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_DrillType"], Mode = BindingMode.OneWay, Converter = new SIRRailWayDrillTypeConverter() });
                this.tbDrillRote.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Direction"], Mode = BindingMode.OneWay, Converter = new SIRRailWayDirectionConverter() });
                this.tbLocation.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Location"], Mode = BindingMode.OneWay, Converter = new SIRRailWayLocationConverter() });
                this.tbSpraySet.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Spray"], Mode = BindingMode.OneWay, Converter = new SIRRailWaySprayConverter() });
                MultiBinding highOrLowModelMultiBind = new MultiBinding();
                MultiBinding highOrLowModelCheckMultiBind = new MultiBinding();
                highOrLowModelCheckMultiBind.Converter = new SIRRailWayHighOrLowCoverter();
                highOrLowModelCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["811b0"], Mode = BindingMode.OneWay });
                highOrLowModelCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["811b1"], Mode = BindingMode.OneWay });
                highOrLowModelCheckMultiBind.NotifyOnSourceUpdated = true;
                this.tbSpeedSet.SetBinding(TextBlock.TextProperty, highOrLowModelCheckMultiBind);
                #endregion

                #region 主参数
                this.tbSysPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIR_RailWay_SystemPress"], Mode = BindingMode.OneWay });
                this.tbInBtnPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_InBtnPress"], Mode = BindingMode.OneWay });
                this.tbHighPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_High"], Mode = BindingMode.OneWay });
                this.tbLowPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_Low"], Mode = BindingMode.OneWay });
                this.tbTorquePress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIR_RailWay_InBtnTorque"], Mode = BindingMode.OneWay });
                //this.tbInWorkTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsPress"], Mode = BindingMode.OneWay });
                this.tbUpDownMove.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_TongsHeight"], Mode = BindingMode.OneWay });
                this.tbBackTongsPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsPress"], Mode = BindingMode.OneWay });
                this.tbBackTongsTorque.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIR_RailWay_BackTongsTorque"], Mode = BindingMode.OneWay });
                #endregion

                // 一键上扣
                MultiBinding sbInbuttonMultiBind = new MultiBinding();
                sbInbuttonMultiBind.Converter = new SIRRailWayAutoModeStepCoverter();
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_OprModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_WorkModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Step"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.ConverterParameter = "inButton";
                sbInbuttonMultiBind.NotifyOnSourceUpdated = true;
                this.sbInButton.SetBinding(StepBar.StepIndexProperty, sbInbuttonMultiBind);
                // 一键卸扣
                MultiBinding sbOutButtonMultiBind = new MultiBinding();
                sbOutButtonMultiBind.Converter = new SIRRailWayAutoModeStepCoverter();
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_OprModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_WorkModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Step"], Mode = BindingMode.OneWay });
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
        /// 图标定时器
        /// </summary>
        /// <param name="value"></param>
        private void ReportTimer_Elapse(object value)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    double drillTore = GlobalData.Instance.da["SIR_RailWay_SystemPress"].Value.Int16 / 1.0;
                    double cosingTorque = GlobalData.Instance.da["SIR_RailWay_InBtnTorque"].Value.Int16 / 1.0;
                    this.PressChart.AddPoints(drillTore);
                    this.TorqueChart.AddPoints(cosingTorque);
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private int iTimeCnt = 0;//用来为时钟计数的变量
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.MonitorAlarmStatus();
                    this.Warnning();
                    if (GlobalData.Instance.da["SIR_RailWay_OprModel"].Value.Byte == 1) // 自动模式
                    {
                        if (GlobalData.Instance.da["SIR_RailWay_WorkModel"].Value.Byte == 1)//上扣
                        {
                            this.spOneKeyInbutton.Visibility = Visibility.Visible;
                            this.spOneKeyOutButton.Visibility = Visibility.Collapsed;
                        }
                        else // 卸扣
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
        List<AlarmInfo> alarmList = new List<AlarmInfo>();
        private void InitAlarmKey()
        {
            alarmList.Add(new AlarmInfo() { TagName = "969b0", Description = "发生偏扣", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "969b1", Description = "发生打滑", NowType = 0 });
        }
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private bool bCommunicationCheck = false; // 是否有中断标志
        /// <summary>
        /// 监控告警状态
        /// </summary>
        private void MonitorAlarmStatus()
        {
            foreach (AlarmInfo info in alarmList)
            {
                if (GlobalData.Instance.da[info.TagName].Value.Boolean)//有报警
                {
                    if (info.NowType == 0) info.NowType = 1;// 前状态为未告警
                }
                else
                {
                    info.NowType = 0;
                }
            }
        }
        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
            if (GlobalData.Instance.da["977b0"].Value.Boolean) this.tbOprTips.Text = "请按启动键开始对缺";
            if (GlobalData.Instance.da["977b1"].Value.Boolean) this.tbOprTips.Text = "请按启动键开始上卸扣切换";
            if (GlobalData.Instance.da["977b2"].Value.Boolean) this.tbOprTips.Text = "请按启动键开始移动至目标位";
            if (GlobalData.Instance.da["977b3"].Value.Boolean) this.tbOprTips.Text = "请确认丝扣油位置，按启动键继续";
            if (GlobalData.Instance.da["977b4"].Value.Boolean) this.tbOprTips.Text = "请确认上卸扣位置，按启动键继续";
            if (GlobalData.Instance.da["977b5"].Value.Boolean) this.tbOprTips.Text = "正在换挡，请确认换挡开关空挡，按启动键继续";
            if (GlobalData.Instance.da["977b6"].Value.Boolean) this.tbOprTips.Text = "请确认安全门已开，按启动键退回待机位";
            else this.tbOprTips.Text = "暂无操作";
            if (iTimeCnt % 10 == 0)
            {
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
                        this.tbAlarmTips.FontSize = 24;
                        this.tbAlarmTips.Text = tmp.Description;
                        tmp.NowType = 2;
                    }

                }
                else
                {
                    this.tbAlarmTips.Text = "暂无告警";
                }
            }
            else
            {

                this.tbAlarmTips.FontSize = 20;
            }

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

            if (!GlobalData.Instance.ComunciationNormal) this.tbAlarmTips.Text = "网络连接失败！";
            else
            {
                if (this.tbAlarmTips.Text == "网络连接失败！") this.tbAlarmTips.Text = "";
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
                byteToSend = new byte[10] { 80, 16, 13, 3, 0, 0, 0, 0, 0, 0 };
            }
            else//当前自动状态
            {
                byteToSend = new byte[10] { 80, 16, 13, 4, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_workModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.workModel.IsChecked) //当前卸扣
            {
                byteToSend = new byte[10] { 80, 16, 13, 5, 0, 0, 0, 0, 0, 0 };
            }
            else//当前上扣
            {
                byteToSend = new byte[10] { 80, 16, 13, 6, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 控制模式
        /// </summary>
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.controlModel.IsChecked) //当前近控
            {
                byteToSend = new byte[10] { 80, 16, 13, 7, 0, 0, 0, 0, 0, 0 };
            }
            else//当前远控
            {
                byteToSend = new byte[10] { 80, 16, 13, 8, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 高低档切换
        /// </summary>
        private void btn_highOrLowModel(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.controlModel.IsChecked) //当前高档
            //{
            //    byteToSend = new byte[10] { 80, 16, 13, 7, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前低档
            //{
            //    byteToSend = new byte[10] { 80, 16, 13, 8, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钻杆类型选择
        /// </summary>
        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 16, 13, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左旋
        /// </summary>
        private void btn_TurnLeft(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 16, 13, 1, 0, 0, 0, 0, 0, 0 };

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右旋
        /// </summary>
        private void btn_TurnRight(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 16, 13, 2, 0, 0, 0, 0, 0, 0 };

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工位选择
        /// </summary>
        private void btn_SelectLocation(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 16, 13, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 喷涂启停
        /// </summary>
        private void btn_SelectSpray(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 16, 13, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 高低档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectSpeed(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 16, 13, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
