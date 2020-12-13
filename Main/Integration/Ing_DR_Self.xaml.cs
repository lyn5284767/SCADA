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
    /// Ing_DR_Self.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_DR_Self : UserControl
    {
        private static Ing_DR_Self _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_DR_Self Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_DR_Self();
                        }
                    }
                }
                return _instance;
            }
        }

        System.Threading.Timer timerWarning;
        public Ing_DR_Self()
        {
            InitializeComponent();
            DRVariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟

            this.Loaded += Ing_DR_Self_Loaded;
        }

        private void Ing_DR_Self_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);
        }

        #region 钻台面
        private void DRVariableBinding()
        {
            try
            {
                this.droperateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.droperateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
                this.drworkMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
                this.drworkMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new WorkModelIsCheckConverter() });
                this.drTelecontrolModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["325b1"], Mode = BindingMode.OneWay, Converter = new TelecontrolModelConverter() });
                this.drTelecontrolModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["325b1"], Mode = BindingMode.OneWay, Converter = new TelecontrolModelIsCheckConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 一键回零
        /// </summary>
        private void btn_DRAllMotorRetZero(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);
            Thread.Sleep(50);

            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 一键使能
        /// </summary>
        private void btn_DRMotorEnable(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);
            Thread.Sleep(50);
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_DROpState(object sender, EventArgs e)
        {
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);
            Thread.Sleep(50);
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
        /// 工作模式
        /// </summary>
        private void btn_DRWorkModel(object sender, EventArgs e)
        {
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);
            Thread.Sleep(50);
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
        #endregion
        /// <summary>
        /// 司钻/遥控切换，二层台，钻台面，铁钻工只允许一个设备为遥控模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_drTelecontrolModel(object sender, EventArgs e)
        {
            byte[] drbyteToSend;
            byte[] sirbyteToSend;
            byte[] sfbyteToSend;
            if (this.drTelecontrolModel.IsChecked)
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
            else 
            {
                drbyteToSend = new byte[10] { 1, 32, 2, 20, 0, 0, 0, 0, 0, 0 };// 钻台面-遥控切司钻
                GlobalData.Instance.da.SendBytes(drbyteToSend);

            }

          
        }

        private int iTimeCnt = 0;//用来为时钟计数的变量

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Warnning();
                    if (GlobalData.Instance.da["droperationModel"].Value.Byte == 1)
                    {
                        this.tbDRAlarm.Text = "当前系统为紧急停止状态";
                    }
                    else
                    {
                        if (this.tbDRAlarm.Text == "当前系统为紧急停止状态")
                            this.tbDRAlarm.Text = "";
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

        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
            #region 告警1
            int warnOne = GlobalData.Instance.da["drTipsCode"].Value.Int32;
            if (warnOne == 1)
            {
                this.tbDROpr.Text = "小车电机故障";
            }
            else if (warnOne == 3)
            {
                this.tbDROpr.Text = "回转电机故障";
            }
            else if (warnOne == 6)
            {
                this.tbDROpr.Text = "机械手禁止向井口移动";
            }
            else if (warnOne == 7)
            {
                this.tbDROpr.Text = "禁止机械手缩回";
            }
            else if (warnOne == 11)
            {
                this.tbDROpr.Text = "抓手不允许打开";
            }
            else if (warnOne == 12)
            {
                this.tbDROpr.Text = "抓手不允许关闭";
            }
            else if (warnOne == 13)
            {
                this.tbDROpr.Text = "请将小车移至井口";
            }
            else if (warnOne == 15)
            {
                this.tbDROpr.Text = "小车不能往井口移动";
            }
            else if (warnOne == 20)
            {
                this.tbDROpr.Text = "回转回零异常，检查传感器";
            }
            else if (warnOne == 21)
            {
                this.tbDROpr.Text = "小车未回零或最大最小值限制";
            }
            else if (warnOne == 22 || warnOne == 23)
            {
                this.tbDROpr.Text = "手臂回零或最大最小值限制";
            }
            else if (warnOne == 31)
            {
                this.tbDROpr.Text = "抓手中有钻杆，请切换手动处理";
            }
            else if (warnOne == 32)
            {
                this.tbDROpr.Text = "请先回收手臂";
            }
            else if (warnOne == 33)
            {
                this.tbDROpr.Text = "小车回零中...";
            }
            else if (warnOne == 34)
            {
                this.tbDROpr.Text = "请将手臂收到最小";
            }
            else if (warnOne == 35)
            {
                this.tbDROpr.Text = "手臂回零中...";
            }
            else if (warnOne == 36)
            {
                this.tbDROpr.Text = "请先回收小车";
            }
            else if (warnOne == 37)
            {
                this.tbDROpr.Text = "回转回零中...";
            }
            else if (warnOne == 38)
            {
                this.tbDROpr.Text = "请将小车移动到中间靠井口";
            }
            else if (warnOne == 39)
            {
                this.tbDROpr.Text = "此轴已回零";
            }
            else if (warnOne == 40)
            {
                this.tbDROpr.Text = "系统未回零，请谨慎操作";
            }
            else if (warnOne == 41)
            {
                this.tbDROpr.Text = "请选择管柱类型";
            }
            else if (warnOne == 42)
            {
                this.tbDROpr.Text = "请选择指梁";
            }
            else if (warnOne == 43)
            {
                this.tbDROpr.Text = "请选择目的地";
            }
            else if (warnOne == 44)
            {
                this.tbDROpr.Text = "管柱已满，请切换指梁";
            }
            else if (warnOne == 45)
            {
                this.tbDROpr.Text = "请启动确认按键启动";
            }
            else if (warnOne == 46)
            {
                this.tbDROpr.Text = "确定大钩上提";
            }
            else if (warnOne == 47)
            {
                this.tbDROpr.Text = "抓手还未打开";
            }
            else if (warnOne == 48)
            {
                this.tbDROpr.Text = "抓手动作中...";
            }
            else if (warnOne == 49)
            {
                this.tbDROpr.Text = "请确认井口安全";
            }
            else if (warnOne == 50)
            {
                this.tbDROpr.Text = "确认上管柱已脱离下管柱";
            }
            else if (warnOne == 51)
            {
                this.tbDROpr.Text = "确认抓手中已有管柱";
            }
            else if (warnOne == 52)
            {
                this.tbDROpr.Text = "确认定位完成，大钩下落，钻杆入盒";
            }
            else if (warnOne == 53)
            {
                this.tbDROpr.Text = "请确认抓手已打开";
            }
            else if (warnOne == 54)
            {
                this.tbDROpr.Text = "自动排管动作完成";
            }
            else if (warnOne == 55)
            {
                this.tbDROpr.Text = "此指梁无钻杆";
            }
            else if (warnOne == 56)
            {
                this.tbDROpr.Text = "自动送杆抓杆失败，请确认抓手有钻杆";
            }
            else if (warnOne == 57)
            {
                this.tbDROpr.Text = "抓手未关闭";
            }
            else if (warnOne == 58)
            {
                this.tbDROpr.Text = "确认定位完成，大钩下落，上下管柱接触";
            }
            else if (warnOne == 59)
            {
                this.tbDROpr.Text = "需要人工确认吊卡关闭";
            }
            else if (warnOne == 60)
            {
                this.tbDROpr.Text = "自动送杆动作完成";
            }
            else if (warnOne == 61)
            {
                this.tbDROpr.Text = "确认启动回收模式";
            }
            else if (warnOne == 62)
            {
                this.tbDROpr.Text = "回收完成";
            }
            else if (warnOne == 65)
            {
                this.tbDROpr.Text = "请先回收手臂";
            }
            else if (warnOne == 71)
            {
                this.tbDROpr.Text = "请启动运输模式";
            }
            else if (warnOne == 72)
            {
                this.tbDROpr.Text = "已完成运输模式";
            }
            else if (warnOne == 81)
            {
                this.tbDROpr.Text = "两点距离太近";
            }
            else if (warnOne == 82)
            {
                this.tbDROpr.Text = "双轴运动";
            }
            else if (warnOne == 83)
            {
                this.tbDROpr.Text = "记录点数超限";
            }
            else if (warnOne == 84)
            {
                this.tbDROpr.Text = "记录点数为零";
            }
            else if (warnOne == 85)
            {
                this.tbDROpr.Text = "即将进入下一次示教";
            }
            else if (warnOne == 91)
            {
                this.tbDROpr.Text = "小车补偿超出范围";
            }
            else if (warnOne == 93)
            {
                this.tbDROpr.Text = "回转补偿超出范围";
            }
            else if (warnOne == 101)
            {
                this.tbDROpr.Text = "盖板未打开，无法向右平移";
            }
            else if (warnOne == 102)
            {
                this.tbDROpr.Text = "盖板未打开，无法向左平移";
            }
            else if (warnOne == 103)
            {
                this.tbDROpr.Text = "机械手在左侧，无法向左平移";
            }
            else if (warnOne == 104)
            {
                this.tbDROpr.Text = "机械手在右侧，无法向右平移";
            }
            else
            {
                this.tbDROpr.Text = "";
            }
            #endregion

            #region 告警2
            int warnTwoNO = GlobalData.Instance.da["drErrorCode"].Value.Int32;
            if (warnTwoNO == 1) this.tbDRAlarm.Text = "小车电机故障";
            else if (warnTwoNO == 3) this.tbDRAlarm.Text = "回转电机故障";
            else if (warnTwoNO == 6) this.tbDRAlarm.Text = "机械手禁止向井口移动";
            else if (warnTwoNO == 7) this.tbDRAlarm.Text = "禁止机械手缩回";
            else if (warnTwoNO == 11) this.tbDRAlarm.Text = "抓手不允许打开";
            else if (warnTwoNO == 12) this.tbDRAlarm.Text = "抓手不允许关闭";
            else if (warnTwoNO == 13) this.tbDRAlarm.Text = "小车不允许在此位置";
            else this.tbDRAlarm.Text = "";
            #endregion

            #region 告警3
                if (!GlobalData.Instance.da["324b0"].Value.Boolean || !GlobalData.Instance.da["324b4"].Value.Boolean) this.tbDRAlarm.Text = "系统还未回零，请注意安全！";
                else if (GlobalData.Instance.da["337b0"].Value.Boolean) this.tbDRAlarm.Text = "抓手传感器异常";
                else if (GlobalData.Instance.da["337b1"].Value.Boolean) this.tbDRAlarm.Text = "抓手打开卡滞";
                else if (GlobalData.Instance.da["337b2"].Value.Boolean) this.tbDRAlarm.Text = "抓手关闭卡滞";
                else if (GlobalData.Instance.da["337b4"].Value.Boolean) this.tbDRAlarm.Text = "手臂传感器故障";
                else if (GlobalData.Instance.da["337b5"].Value.Boolean) this.tbDRAlarm.Text = "手臂伸出异常";
                else if (GlobalData.Instance.da["337b6"].Value.Boolean) this.tbDRAlarm.Text = "手臂缩回异常";
            #endregion

            //操作台控制器心跳
            if (GlobalData.Instance.da["508b5"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 600)
                {
                    //Communication = 2;
                    //this.tbDROpr.Text = "操作台信号中断";
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

            if (!GlobalData.Instance.ComunciationNormal) this.tbDROpr.Text = "网络连接失败！";
            else
            {
                if (this.tbDROpr.Text == "网络连接失败！") this.tbDROpr.Text = "";
            }
        }
    }
}
