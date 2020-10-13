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
    /// IngDR.xaml 的交互逻辑
    /// </summary>
    public partial class IngDR : UserControl
    {
        private static IngDR _instance = null;
        private static readonly object syncRoot = new object();

        public static IngDR Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngDR();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        public IngDR()
        {
            InitializeComponent();
            timer = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);
            aminationNew.SendFingerBeamNumberEvent += Instance_SendFingerBeamNumberEvent;
            aminationNew.SystemChange(SystemType.DrillFloor);
            VariableBinding();
            this.Loaded += IngDR_Loaded;

        }

        private void VariableBinding()
        {
            this.drcarMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.drRotateMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.drcarMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.drRotateMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
        }

        private void IngDR_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 1, 32, 1, 10, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);

            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);
            this.aminationNew.InitRowsColoms(SystemType.DrillFloor);
        }

        /// <summary>
        /// 50ms定时器
        /// </summary>
        /// <param name="obj"></param>
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    aminationNew.LoadFingerBeamDrillPipe(SystemType.SecondFloor);
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Warnning();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void Instance_SendFingerBeamNumberEvent(byte number)
        {
            if (GlobalData.Instance.da["operationModel"].Value.Byte == 5 || GlobalData.Instance.da["operationModel"].Value.Byte == 3)
            {
                byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 5, number });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            Thread.Sleep(50);
            if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5 || GlobalData.Instance.da["droperationModel"].Value.Byte == 3)
            {
                byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 5, number });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        private int iTimeCnt = 0;//用来为时钟计数的变量
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private bool bCommunicationCheck = false; // 是否有中断标志

        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
            #region 操作提示
            int oprTps = GlobalData.Instance.da["drTipsCode"].Value.Int32;
            if (oprTps == 1)
            {
                this.tbOprTips.Text = "小车电机故障";
            }
            else if (oprTps == 3)
            {
                this.tbOprTips.Text = "回转电机故障";
            }
            else if (oprTps == 6)
            {
                this.tbOprTips.Text = "机械手禁止向井口移动";
            }
            else if (oprTps == 7)
            {
                this.tbOprTips.Text = "禁止机械手缩回";
            }
            else if (oprTps == 11)
            {
                this.tbOprTips.Text = "抓手不允许打开";
            }
            else if (oprTps == 12)
            {
                this.tbOprTips.Text = "抓手不允许关闭";
            }
            else if (oprTps == 13)
            {
                this.tbOprTips.Text = "请将小车移至井口";
            }
            else if (oprTps == 15)
            {
                this.tbOprTips.Text = "小车不能往井口移动";
            }
            else if (oprTps == 20)
            {
                this.tbOprTips.Text = "回转回零异常，检查传感器";
            }
            else if (oprTps == 21)
            {
                this.tbOprTips.Text = "小车未回零或最大最小值限制";
            }
            else if (oprTps == 22 || oprTps == 23)
            {
                this.tbOprTips.Text = "手臂回零或最大最小值限制";
            }
            else if (oprTps == 31)
            {
                this.tbOprTips.Text = "抓手中右钻杆，请切换手动处理";
            }
            else if (oprTps == 32)
            {
                this.tbOprTips.Text = "请先回收手臂";
            }
            else if (oprTps == 33)
            {
                this.tbOprTips.Text = "小车回零中...";
            }
            else if (oprTps == 34)
            {
                this.tbOprTips.Text = "请将手臂收到最小";
            }
            else if (oprTps == 35)
            {
                this.tbOprTips.Text = "手臂回零中...";
            }
            else if (oprTps == 36)
            {
                this.tbOprTips.Text = "请先回收小车";
            }
            else if (oprTps == 37)
            {
                this.tbOprTips.Text = "回转回零中...";
            }
            else if (oprTps == 38)
            {
                this.tbOprTips.Text = "请将小车移动到中间靠井口";
            }
            else if (oprTps == 39)
            {
                this.tbOprTips.Text = "此轴已回零";
            }
            else if (oprTps == 40)
            {
                this.tbOprTips.Text = "系统未回零，请谨慎操作";
            }
            else if (oprTps == 41)
            {
                this.tbOprTips.Text = "请选择管柱类型";
            }
            else if (oprTps == 42)
            {
                this.tbOprTips.Text = "请选择指梁";
            }
            else if (oprTps == 43)
            {
                this.tbOprTips.Text = "请选择目的地";
            }
            else if (oprTps == 44)
            {
                this.tbOprTips.Text = "管柱已满，请切换指梁";
            }
            else if (oprTps == 45)
            {
                this.tbOprTips.Text = "请启动确认按键启动";
            }
            else if (oprTps == 46)
            {
                this.tbOprTips.Text = "确定大钩上提";
            }
            else if (oprTps == 47)
            {
                this.tbOprTips.Text = "抓手还未打开";
            }
            else if (oprTps == 48)
            {
                this.tbOprTips.Text = "抓手动作中...";
            }
            else if (oprTps == 49)
            {
                this.tbOprTips.Text = "请确认井口安全";
            }
            else if (oprTps == 50)
            {
                this.tbOprTips.Text = "确认上管柱已脱离下管柱";
            }
            else if (oprTps == 51)
            {
                this.tbOprTips.Text = "确认抓手中已有管柱";
            }
            else if (oprTps == 52)
            {
                this.tbOprTips.Text = "确认定位完成，大钩下落，钻杆入盒";
            }
            else if (oprTps == 53)
            {
                this.tbOprTips.Text = "请确认抓手已打开";
            }
            else if (oprTps == 54)
            {
                this.tbOprTips.Text = "自动排管动作完成";
            }
            else if (oprTps == 55)
            {
                this.tbOprTips.Text = "此指梁无钻杆";
            }
            else if (oprTps == 56)
            {
                this.tbOprTips.Text = "自动送杆抓杆失败，请确认抓手有钻杆";
            }
            else if (oprTps == 57)
            {
                this.tbOprTips.Text = "抓手未关闭";
            }
            else if (oprTps == 58)
            {
                this.tbOprTips.Text = "确认定位完成，大钩下落，上下管柱接触";
            }
            else if (oprTps == 59)
            {
                this.tbOprTips.Text = "需要人工确认吊卡关闭";
            }
            else if (oprTps == 60)
            {
                this.tbOprTips.Text = "自动送杆动作完成";
            }
            else if (oprTps == 61)
            {
                this.tbOprTips.Text = "确认启动回收模式";
            }
            else if (oprTps == 62)
            {
                this.tbOprTips.Text = "回收完成";
            }
            else if (oprTps == 65)
            {
                this.tbOprTips.Text = "请先回收手臂";
            }
            else if (oprTps == 71)
            {
                this.tbOprTips.Text = "请启动运输模式";
            }
            else if (oprTps == 72)
            {
                this.tbOprTips.Text = "已完成运输模式";
            }
            else if (oprTps == 81)
            {
                this.tbOprTips.Text = "两点距离太近";
            }
            else if (oprTps == 82)
            {
                this.tbOprTips.Text = "双轴运动";
            }
            else if (oprTps == 83)
            {
                this.tbOprTips.Text = "记录点数超限";
            }
            else if (oprTps == 84)
            {
                this.tbOprTips.Text = "记录点数为零";
            }
            else if (oprTps == 85)
            {
                this.tbOprTips.Text = "即将进入下一次示教";
            }
            else if (oprTps == 91)
            {
                this.tbOprTips.Text = "小车补偿超出范围";
            }
            else if (oprTps == 93)
            {
                this.tbOprTips.Text = "回转补偿超出范围";
            }
            else if (oprTps == 101)
            {
                this.tbOprTips.Text = "盖板未打开，无法向右平移";
            }
            else if (oprTps == 102)
            {
                this.tbOprTips.Text = "盖板未打开，无法向左平移";
            }
            else if (oprTps == 103)
            {
                this.tbOprTips.Text = "机械手在左侧，无法向左平移";
            }
            else if (oprTps == 104)
            {
                this.tbOprTips.Text = "机械手在右侧，无法向右平移";
            }
            else
            {
                this.tbOprTips.Text = "";
            }
            #endregion

            #region 告警提示
            int warnTwoNO = GlobalData.Instance.da["drErrorCode"].Value.Int32;
            if (warnTwoNO == 1) this.tbAlarmTips.Text = "小车电机故障";
            else if (warnTwoNO == 3) this.tbAlarmTips.Text = "回转电机故障";
            else if (warnTwoNO == 6) this.tbAlarmTips.Text = "机械手禁止向井口移动";
            else if (warnTwoNO == 7) this.tbAlarmTips.Text = "禁止机械手缩回";
            else if (warnTwoNO == 11) this.tbAlarmTips.Text = "抓手不允许打开";
            else if (warnTwoNO == 12) this.tbAlarmTips.Text = "抓手不允许关闭";
            else if (warnTwoNO == 13) this.tbAlarmTips.Text = "小车不允许在此位置";
            else this.tbAlarmTips.Text = "";
            #endregion

            #region 告警3
            if (iTimeCnt % 10 == 0)
            {
                tbAlarmTips.FontSize = 14;
                tbAlarmTips.Visibility = Visibility.Visible;
                if (!GlobalData.Instance.da["324b0"].Value.Boolean || !GlobalData.Instance.da["324b4"].Value.Boolean) this.tbAlarmTips.Text = "系统还未回零，请注意安全！";
                else if (GlobalData.Instance.da["337b0"].Value.Boolean) this.tbAlarmTips.Text = "抓手传感器异常";
                else if (GlobalData.Instance.da["337b1"].Value.Boolean) this.tbAlarmTips.Text = "抓手打开卡滞";
                else if (GlobalData.Instance.da["337b2"].Value.Boolean) this.tbAlarmTips.Text = "抓手关闭卡滞";
                else if (GlobalData.Instance.da["337b4"].Value.Boolean) this.tbAlarmTips.Text = "手臂传感器故障";
                else if (GlobalData.Instance.da["337b5"].Value.Boolean) this.tbAlarmTips.Text = "手臂伸出异常";
                else if (GlobalData.Instance.da["337b6"].Value.Boolean) this.tbAlarmTips.Text = "手臂缩回异常";
                else this.tbAlarmTips.Text = "";
            }
            else
            {
                tbAlarmTips.FontSize = 20;
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

            if (!GlobalData.Instance.ComunciationNormal) this.tbAlarmTips.Text = "网络连接失败！";
        }
    }
}
