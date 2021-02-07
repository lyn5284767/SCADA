using COM.Common;
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
    /// Ing_SIR_RailWay.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_SIR_RailWay : UserControl
    {
        private static Ing_SIR_RailWay _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_SIR_RailWay Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_SIR_RailWay();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        public Ing_SIR_RailWay()
        {
            InitializeComponent();
            VariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }

        private void VariableBinding()
        {
            try
            {

                #region 类型选择
                this.tbDrillRote.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Direction"], Mode = BindingMode.OneWay, Converter = new SIRRailWayDirectionConverter() });
                this.tbLocation.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Location"], Mode = BindingMode.OneWay, Converter = new SIRRailWayLocationConverter() });
                this.tbSpraySet.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Spray"], Mode = BindingMode.OneWay, Converter = new SIRRailWaySprayConverter() });
               #endregion

                #region 主参数
                this.tbSysPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIR_RailWay_SystemPress"], Mode = BindingMode.OneWay });
                #endregion

               }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        BrushConverter bc = new BrushConverter();
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
                    if (this.tbAlarmTips.Text == "暂无告警") this.tbAlarmTips.Foreground = (Brush)bc.ConvertFrom("#000000");
                    else this.tbAlarmTips.Foreground = (Brush)bc.ConvertFrom("#E0496D");
                    if (this.tbOprTips.Text == "暂无操作提示") this.tbOprTips.Foreground = (Brush)bc.ConvertFrom("#000000");
                    else this.tbOprTips.Foreground = (Brush)bc.ConvertFrom("#E0496D");
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
            alarmList.Add(new AlarmInfo() { TagName = "969b2", Description = "上扣故障", NowType = 0 });
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
            else if (GlobalData.Instance.da["977b1"].Value.Boolean) this.tbOprTips.Text = "请按启动键开始上卸扣切换";
            else if (GlobalData.Instance.da["977b2"].Value.Boolean) this.tbOprTips.Text = "请按启动键开始移动至目标位";
            else if (GlobalData.Instance.da["977b3"].Value.Boolean) this.tbOprTips.Text = "请确认丝扣油位置，按启动键继续";
            else if (GlobalData.Instance.da["977b4"].Value.Boolean) this.tbOprTips.Text = "请确认上卸扣位置，按启动键继续";
            else if (GlobalData.Instance.da["977b5"].Value.Boolean) this.tbOprTips.Text = "正在换挡，请确认换挡开关空挡，按启动键继续";
            else if (GlobalData.Instance.da["977b6"].Value.Boolean) this.tbOprTips.Text = "请确认安全门已开，按启动键退回待机位";
            else this.tbOprTips.Text = "暂无操作提示";
            if (iTimeCnt % 10 == 0)
            {
                // 告警列表!=0则有告警 
                if (alarmList.Where(w => w.NowType != 0).Count() > 0)
                {

                    // 有告警且全部显示完成
                    if (this.alarmList.Where(w => w.NowType == 1).Count() == 0)
                    {
                        this.alarmList.Where(w => w.NowType == 2).ToList().ForEach(w => w.NowType = 1);
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
                if (this.tbAlarmTips.Text == "网络连接失败！") this.tbAlarmTips.Text = "暂无告警";
            }
        }
    }
}
