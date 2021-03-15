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
    /// Ing_SF_Self.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_SF_Self : UserControl
    {
        private static Ing_SF_Self _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_SF_Self Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_SF_Self();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        public Ing_SF_Self()
        {
            InitializeComponent();
            SFVariableBinding();
            InitAlarmKey();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 500);//改成50ms 的时钟
        }


        #region 铁架工操作
        /// <summary>
        /// 铁架工变量
        /// </summary>
        private void SFVariableBinding()
        {
            try
            {
                this.carMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
                this.armMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
                this.rotateMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["rotateMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
                this.carMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.armMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rotateMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["rotateMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 一键回零
        /// </summary>
        private void btn_SFAllMotorRetZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 点击使能
        /// </summary>
        private void btn_SFMotorEnable(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SFOpState(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.sfoperateMode.IsChecked)
            //{
            //    byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 5 });
            //}
            //else
            //{
            //    byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 4 });
            //}

            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_SFWorkModel(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (sfworkMode.IsChecked)
            //{
            //    byteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
            //}
            //else
            //{
            //    byteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btn_controlModel(object sender, EventArgs e)
        {
            //byte[] drbyteToSend;
            //byte[] sirbyteToSend;
            //byte[] sfbyteToSend;
            //if (this.controlModel.IsChecked)
            //{
            //    drbyteToSend = new byte[10] { 1, 32, 2, 20, 0, 0, 0, 0, 0, 0 }; // 扶杆臂-遥控切司钻
            //    sirbyteToSend = new byte[10] { 23, 17, 10, 1, 0, 0, 0, 0, 0, 0 }; // 铁钻工-遥控切司钻
            //    sfbyteToSend = new byte[10] { 16, 1, 27, 1, 2, 0, 0, 0, 0, 0 };// 铁架工-司钻切遥控
            //    GlobalData.Instance.da.SendBytes(drbyteToSend);
            //    Thread.Sleep(50);
            //    GlobalData.Instance.da.SendBytes(sirbyteToSend);
            //    Thread.Sleep(50);
            //    GlobalData.Instance.da.SendBytes(sfbyteToSend);
            //}
            //else
            //{
            //    drbyteToSend = new byte[10] { 16, 1, 27, 1, 1, 0, 0, 0, 0, 0 };// 铁架工-遥控切司钻
            //    GlobalData.Instance.da.SendBytes(drbyteToSend);

            //}
        }
        #endregion

        #region 铁架工告警
        private int iTimeCnt = 0;//用来为时钟计数的变量
        Dictionary<string, byte> alarmKey = new Dictionary<string, byte>();
        List<AlarmInfo> alarmList = new List<AlarmInfo>();

        private void InitAlarmKey()
        {
            //alarmKey.Add("抓手传感器错误", 0);
            //alarmKey.Add("左手指传感器错误", 0);
            //alarmKey.Add("右手指传感器错误", 0);
            alarmList.Add(new AlarmInfo() { TagName = "103N23B6b0", Description = "抓手传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "103N23B6b1", Description = "左手指传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "103N23B6b2", Description = "右手指传感器错误", NowType = 0 });

            //alarmKey.Add("抓手打开卡滞", 0);
            //alarmKey.Add("左手指打开卡滞", 0);
            //alarmKey.Add("右手指打开卡滞", 0);
            //alarmKey.Add("抓手关闭卡滞", 0);
            //alarmKey.Add("左手指关闭卡滞", 0);
            //alarmKey.Add("右手指关闭卡滞", 0);
            alarmList.Add(new AlarmInfo() { TagName = "103N23B7b0", Description = "抓手打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "103N23B7b1", Description = "左手指打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "103N23B7b2", Description = "右手指打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "103N23B7b3", Description = "抓手关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "103N23B7b4", Description = "左手指关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "103N23B7b5", Description = "右手指关闭卡滞", NowType = 0 });

            //alarmKey.Add("左1#钻铤锁打开卡滞", 0);
            //alarmKey.Add("左1#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("左2#钻铤锁打开卡滞", 0);
            //alarmKey.Add("左2#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("左3#钻铤锁打开卡滞", 0);
            //alarmKey.Add("左3#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("左4#钻铤锁打开卡滞", 0);
            //alarmKey.Add("左4#钻铤锁关闭卡滞", 0);
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B4b0", Description = "左1#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B4b1", Description = "左1#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B4b2", Description = "左2#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B4b3", Description = "左2#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B4b4", Description = "左3#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B4b5", Description = "左3#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B4b6", Description = "左4#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B4b7", Description = "左4#钻铤锁关闭卡滞", NowType = 0 });

            //alarmKey.Add("左5#钻铤锁打开卡滞", 0);
            //alarmKey.Add("左5#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("左6#钻铤锁打开卡滞", 0);
            //alarmKey.Add("左6#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("左7#钻铤锁打开卡滞", 0);
            //alarmKey.Add("左7#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("左8#钻铤锁打开卡滞", 0);
            //alarmKey.Add("左8#钻铤锁关闭卡滞", 0);
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B5b0", Description = "左5#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B5b1", Description = "左5#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B5b2", Description = "左6#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B5b3", Description = "左6#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B5b4", Description = "左7#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B5b5", Description = "左7#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B5b6", Description = "左8#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B5b7", Description = "左8#钻铤锁关闭卡滞", NowType = 0 });

            //alarmKey.Add("右1#钻铤锁打开卡滞", 0);
            //alarmKey.Add("右1#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("右2#钻铤锁打开卡滞", 0);
            //alarmKey.Add("右2#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("右3#钻铤锁打开卡滞", 0);
            //alarmKey.Add("右3#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("右4#钻铤锁打开卡滞", 0);
            //alarmKey.Add("右4#钻铤锁关闭卡滞", 0);
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B6b0", Description = "右1#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B6b1", Description = "右1#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B6b2", Description = "右2#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B6b3", Description = "右2#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B6b4", Description = "右3#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B6b5", Description = "右3#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B6b6", Description = "右4#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B6b7", Description = "右4#钻铤锁关闭卡滞", NowType = 0 });

            //alarmKey.Add("右5#钻铤锁打开卡滞", 0);
            //alarmKey.Add("右5#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("右6#钻铤锁打开卡滞", 0);
            //alarmKey.Add("右6#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("右7#钻铤锁打开卡滞", 0);
            //alarmKey.Add("右7#钻铤锁关闭卡滞", 0);
            //alarmKey.Add("右8#钻铤锁打开卡滞", 0);
            //alarmKey.Add("右8#钻铤锁关闭卡滞", 0);
            alarmList.Add(new AlarmInfo() { TagName = "123b0", Description = "右5#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "123b1", Description = "右5#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "123b2", Description = "右6#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "123b3", Description = "右6#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "123b4", Description = "右7#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "123b5", Description = "右7#钻铤锁关闭卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "123b6", Description = "右8#钻铤锁打开卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "123b7", Description = "右8#钻铤锁关闭卡滞", NowType = 0 });

            //alarmKey.Add("左挡绳伸出卡滞", 0);
            //alarmKey.Add("左挡绳缩回卡滞", 0);
            //alarmKey.Add("右挡绳伸出卡滞", 0);
            //alarmKey.Add("右挡绳缩回卡滞", 0);
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B7b0", Description = "左挡绳伸出卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B7b1", Description = "左挡绳缩回卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B7b2", Description = "右挡绳伸出卡滞", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "105N2N22B7b3", Description = "右挡绳缩回卡滞", NowType = 0 });

            //alarmKey.Add("左1#钻铤锁传感器错误", 0);
            //alarmKey.Add("左2#钻铤锁传感器错误", 0);
            //alarmKey.Add("左3#钻铤锁传感器错误", 0);
            //alarmKey.Add("左4#钻铤锁传感器错误", 0);
            //alarmKey.Add("左5#钻铤锁传感器错误", 0);
            //alarmKey.Add("左6#钻铤锁传感器错误", 0);
            //alarmKey.Add("左7#钻铤锁传感器错误", 0);
            //alarmKey.Add("左8#钻铤锁传感器错误", 0);
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B4b0", Description = "左1#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B4b1", Description = "左2#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B4b2", Description = "左3#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B4b3", Description = "左4#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B4b4", Description = "左5#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B4b5", Description = "左6#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B4b6", Description = "左7#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B4b7", Description = "左8#钻铤锁传感器错误", NowType = 0 });
            
            //alarmKey.Add("右1#钻铤锁传感器错误", 0);
            //alarmKey.Add("右2#钻铤锁传感器错误", 0);
            //alarmKey.Add("右3#钻铤锁传感器错误", 0);
            //alarmKey.Add("右4#钻铤锁传感器错误", 0);
            //alarmKey.Add("右5#钻铤锁传感器错误", 0);
            //alarmKey.Add("右6#钻铤锁传感器错误", 0);
            //alarmKey.Add("右7#钻铤锁传感器错误", 0);
            //alarmKey.Add("右8#钻铤锁传感器错误", 0);
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B5b0", Description = "右1#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B5b1", Description = "右2#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B5b2", Description = "右3#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B5b3", Description = "右4#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "135b4", Description = "右5#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "135b5", Description = "右6#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "135b6", Description = "右7#钻铤锁传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "135b7", Description = "右8#钻铤锁传感器错误", NowType = 0 });

            //alarmKey.Add("左挡绳传感器错误", 0);
            //alarmKey.Add("右挡绳传感器错误", 0);
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B5b4", Description = "左挡绳传感器错误", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "107N2N22B5b5", Description = "右挡绳传感器错误", NowType = 0 });

            alarmList.Add(new AlarmInfo() { TagName = "carMotorRetZeroStatus", Description = "机械手未回零，位置未知，请注意防碰！", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "101B7b1", Description = "机械手已进入防碰区，请注意防碰！", NowType = 0 });
            //alarmList.Add(new AlarmInfo() { TagName = "101B7b1", Description = "机械手已进入防碰区，请注意防碰！", NowType = 0 });

        }
        BrushConverter bc = new BrushConverter();
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.tbSFAlarm.Foreground = (Brush)bc.ConvertFrom("#E0496D");
                    this.tbSFOpr.Foreground = (Brush)bc.ConvertFrom("#E0496D");

                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Operation();
                    this.Warnning();
                    MonitorAlarmStatus();

                    if (GlobalData.Instance.da["operationModel"].Value.Byte == 1)
                    {
                        this.tbSFOpr.Text = "当前系统为紧急停止状态";
                    }
                    else
                    {
                        if (this.tbSFOpr.Text == "当前系统为紧急停止状态")
                        {
                            this.tbSFOpr.Text = "暂无告警";
                            this.tbSFOpr.Foreground = (Brush)bc.ConvertFrom("#000000");
                        }
                    }
                    this.Communcation();
                    if (this.tbSFAlarm.Text == "暂无告警") this.tbSFAlarm.Foreground = (Brush)bc.ConvertFrom("#000000");
                    else this.tbSFAlarm.Foreground = (Brush)bc.ConvertFrom("#E0496D");
                    if (this.tbSFOpr.Text == "暂无操作提示") this.tbSFOpr.Foreground = (Brush)bc.ConvertFrom("#000000");
                    else this.tbSFOpr.Foreground = (Brush)bc.ConvertFrom("#E0496D");
                }));

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 监控告警状态
        /// </summary>
        private void MonitorAlarmStatus()
        {
            foreach(AlarmInfo info in alarmList)
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

        private void Warnning()
        {
            //if (iTimeCnt % 10 == 0)
            //{
                // 告警列表!=0则有告警 
                if (alarmList.Where(w=>w.NowType!=0).Count() > 0)
                {
                  
                    // 有告警且全部显示完成
                    if (this.alarmList.Where(w=>w.NowType==1).Count()==0)   
                    {
                        this.alarmList.ForEach(w => w.NowType = 1);
                    }
                    AlarmInfo tmp = this.alarmList.Where(w => w.NowType == 1).FirstOrDefault();
                    if (tmp != null)
                    {
                        this.tbSFAlarm.FontSize = 18;
                        this.tbSFAlarm.Text = tmp.Description;
                        tmp.NowType = 2;
                    }

                }
                else
                {
                    this.tbSFAlarm.Text = "暂无告警";
                    this.tbSFAlarm.Foreground = (Brush)bc.ConvertFrom("#000000");
                }

         
            //}
            //else
            //{
            //    this.tbSFAlarm.FontSize = 14;
            //}
        }
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private bool bCommunicationCheck = false; // 是否有中断标志
        private bool bCheckTwo = false;
        /// <summary>
        /// 通信数据
        /// </summary>
        private void Communcation()
        {
            #region 通信
            // UDP通信，操作台/铁架工心跳都正常，通信正常
            if (GlobalData.Instance.da.NetStatus && !GlobalData.Instance.da["508b6"].Value.Boolean && !GlobalData.Instance.da["508b5"].Value.Boolean)
            {
                //Communication = 1;
            }

            //if (!GlobalData.Instance.da.NetStatus) Communication = 0; // UDP建立成功/失败标志

            //操作台控制器心跳
            if (GlobalData.Instance.da["508b5"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 600)
                {
                    //Communication = 2;
                    this.tbSFOpr.Text = "操作台信号中断";
                }
                if (!bCheckTwo && controlHeartTimes > 600)
                {
                    GlobalData.Instance.reportData.OperationFloorCommunication += 1;//the report
                    bCheckTwo = true;
                }
            }
            else
            {
                this.controlHeartTimes = 0;
                bCheckTwo = false;
            }
            this.tmpStatus = GlobalData.Instance.da["508b6"].Value.Boolean;

            //铁架工控制器心跳
            if (GlobalData.Instance.da["508b6"].Value.Boolean)
            {
                //Communication = 3;
                this.tbSFOpr.Text = "铁架工信号中断";
                if (!bCommunicationCheck)
                {
                    GlobalData.Instance.reportData.OperationFloorCommunication += 1;//the report
                    bCommunicationCheck = true;
                }
            }
            else
            {
                if (this.tbSFOpr.Text == "铁架工信号中断")
                {
                    this.tbSFOpr.Text = "暂无操作指示";
                    this.tbSFOpr.Foreground = (Brush)bc.ConvertFrom("#000000");
                }
                bCommunicationCheck = false;
            }

            if (!GlobalData.Instance.ComunciationNormal) this.tbSFOpr.Text = "网络连接失败！";
            else
            {
                if (this.tbSFOpr.Text == "网络连接失败！")
                {
                    this.tbSFOpr.Text = "暂无操作指示";
                    this.tbSFOpr.Foreground = (Brush)bc.ConvertFrom("#000000");
                }
            }
            #endregion
        }
        private void Operation()
        {
            switch (GlobalData.Instance.da["promptInfo"].Value.Byte)
            {
                case 0:
                    tbSFOpr.Text = "暂无操作提示";
                    this.tbSFOpr.Foreground = (Brush)bc.ConvertFrom("#000000");
                    break;
                case 1:
                    tbSFOpr.Text = "小车电机报警";
                    break;
                case 2:
                    tbSFOpr.Text = "手臂电机报警";
                    break;
                case 3:
                    tbSFOpr.Text = "回转电机报警";
                    break;
                case 6:
                    tbSFOpr.Text = "非安全条件，机械手禁止向井口移动！";
                    break;
                case 7:
                    tbSFOpr.Text = "吊卡未打开，机械手禁止缩回！";
                    break;
                case 8:
                    tbSFOpr.Text = "请将挡绳缩回后再回零！";
                    break;
                case 9:
                    tbSFOpr.Text = "抓手中有钻杆，手指已经打开，请注意安全！";
                    break;
                case 10:
                    tbSFOpr.Text = "此位置禁止打开手指！";
                    break;
                case 11:
                    tbSFOpr.Text = "此位置禁止打开抓手！";
                    break;
                case 12:
                    tbSFOpr.Text = "此位置抓手不允许关闭！";
                    break;
                case 13:
                    tbSFOpr.Text = "小车电机已回零完成！";
                    break;
                case 14:
                    tbSFOpr.Text = "手臂电机已回零完成!";
                    break;
                case 15:
                    tbSFOpr.Text = "回转电机（机械手）已完成回零！";
                    break;
                case 31:
                    tbSFOpr.Text = "抓手中有钻杆，请将钻杆取出后再回零！";
                    break;
                case 32:
                    tbSFOpr.Text = "请先将手臂电机回零！";
                    break;
                case 33:
                    tbSFOpr.Text = "请注意安全，小车电机正在回零……";
                    break;
                case 34:
                    tbSFOpr.Text = "请先将手臂缩回！";
                    break;
                case 35:
                    tbSFOpr.Text = "请注意安全，手臂电机正在回零……";
                    break;
                case 36:
                    tbSFOpr.Text = "请先将小车电机正在回零！";
                    break;
                case 37:
                    tbSFOpr.Text = "请注意安全，回转电机正在回零……";
                    break;
                case 38:
                    tbSFOpr.Text = "请将小车移动到中间靠井口位置！";
                    break;
                case 39:
                    tbSFOpr.Text = "所选择的电机已经回零完成！";
                    break;
                case 40:
                    tbSFOpr.Text = "机械手还未回零，请注意安全！";
                    break;
                case 41:
                    tbSFOpr.Text = "请先选择管柱类型！";
                    break;
                case 42:
                    tbSFOpr.Text = "请选择目标指梁号！";
                    break;
                case 43:
                    tbSFOpr.Text = "未感应到回转回零传感器,请检查!";
                    break;
                case 44:
                    tbSFOpr.Text = "所选指梁钻杆已满，请切换！";
                    break;
                case 45:
                    tbSFOpr.Text = "请按确认键启动！";
                    break;
                case 46:
                    tbSFOpr.Text = "请确认指梁锁已打开到位！";
                    break;
                case 47:
                    tbSFOpr.Text = "手指正在打开……";
                    break;
                case 48:
                    tbSFOpr.Text = "抓手正在打开……";
                    break;
                case 49:
                    tbSFOpr.Text = "请确认吊卡是否在铁架工上方！";
                    break;
                case 50:
                    tbSFOpr.Text = "请确认吊卡已经打开！";
                    break;
                case 51:
                    tbSFOpr.Text = "吊卡未打开！";
                    break;
                case 52:
                    tbSFOpr.Text = "请操作手柄！";
                    break;
                case 53:
                    tbSFOpr.Text = "手指和抓手未打开！";
                    break;
                case 54:
                    tbSFOpr.Text = "自动排管完成！";
                    break;
                case 55:
                    tbSFOpr.Text = "所选指梁钻杆已空，请切换！";
                    break;
                case 56:
                    tbSFOpr.Text = "指梁内抓杆失败！";
                    break;
                case 57:
                    tbSFOpr.Text = "抓手正在关闭……";
                    break;
                case 58:
                    tbSFOpr.Text = "手指正在关闭……";
                    break;
                case 59:
                    tbSFOpr.Text = "人工确认吊卡关闭！";
                    break;
                case 60:
                    tbSFOpr.Text = "自动下钻完成！";
                    break;
                case 61:
                    tbSFOpr.Text = "请按确认键启动回收模式！";
                    break;
                case 62:
                    tbSFOpr.Text = "机械手已回收完成！";
                    break;
                case 65:
                    tbSFOpr.Text = "请先收回手臂再操作！";
                    break;
                case 66:
                    tbSFOpr.Text = "此位置不能进行手臂伸出和回转操作！";
                    break;
                case 67:
                    tbSFOpr.Text = "此位置不能旋转！";
                    break;
                case 68:
                    tbSFOpr.Text = "此位置不能伸出手臂！";
                    break;
                case 71:
                    tbSFOpr.Text = "请按确认键启动运输模式！";
                    break;
                case 72:
                    tbSFOpr.Text = "机械手已完成运输模式！";
                    break;
                case 73:
                    tbSFOpr.Text = "请谨慎确认钻杆已送入吊卡！";
                    break;
                case 74:
                    tbSFOpr.Text = "请谨慎确认手臂已伸到位！";
                    break;
                case 75:
                    tbSFOpr.Text = "请确认手指开合状态！";
                    break;
                case 81:
                    tbSFOpr.Text = "记录的两点太近！";
                    break;
                case 82:
                    tbSFOpr.Text = "示教过程存在双轴运动！";
                    break;
                case 83:
                    tbSFOpr.Text = "记录点数超出限制！";
                    break;
                case 84:
                    tbSFOpr.Text = "记录点数为零！";
                    break;
                case 85:
                    tbSFOpr.Text = "即将进入下一次示教循环！";
                    break;
                case 96:
                    tbSFOpr.Text = "小车电机动作卡滞！";
                    break;
                case 97:
                    tbSFOpr.Text = "手臂电机动作卡滞！";
                    break;
                case 98:
                    tbSFOpr.Text = "回转电机动作卡滞！";
                    break;
                default:
                    tbSFOpr.Text = "暂无操作提示";
                    break;

            }
        }
        #endregion
    }
}
