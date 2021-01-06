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

namespace Main.WellRepair.HS_Self
{
    /// <summary>
    /// HS_Self_Main.xaml 的交互逻辑
    /// </summary>
    public partial class WR_HS_Self_Main : UserControl
    {
        private static WR_HS_Self_Main _instance = null;
        private static readonly object syncRoot = new object();

        public static WR_HS_Self_Main Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new WR_HS_Self_Main();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer ReportTimer;
        System.Threading.Timer timerWarning;
        public WR_HS_Self_Main()
        {
            InitializeComponent();
            VariableBinding();
            ReportTimer = new System.Threading.Timer(new TimerCallback(ReportTimer_Elapse), null, 500, 2000);
            timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.smHighOilAlarm.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["764b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smLowOilAlarm.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["764b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smHighWaterAlarm.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["764b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smLowWaterAlarm.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["764b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smHighVoltageAlarm.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["764b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smLowVoltageAlarm.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["764b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.tbWorkTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["WR_HS_WorkTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["763b0"], Mode = BindingMode.OneWay });
                this.PumpOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["759b1"], Mode = BindingMode.OneWay, Converter = new BoolNegationConverter() });
                this.FanOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["759b4"], Mode = BindingMode.OneWay, Converter = new BoolNegationConverter() });
                this.PumpTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["759b3"], Mode = BindingMode.OneWay, Converter = new BoolNegationConverter() });
                this.FanTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["759b6"], Mode = BindingMode.OneWay, Converter = new BoolNegationConverter() });

                // 1#主泵
                this.cpbPumpOne.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_PumpOnePress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbPumpOne.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_PumpOnePress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbPumpOneMultiBind = new MultiBinding();
                cpbPumpOneMultiBind.Converter = new ColorCoverter();
                cpbPumpOneMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_PumpOnePress"], Mode = BindingMode.OneWay });
                cpbPumpOneMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbPumpOne, Mode = BindingMode.OneWay });
                cpbPumpOneMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbPumpOne, Mode = BindingMode.OneWay });
                cpbPumpOneMultiBind.NotifyOnSourceUpdated = true;
                this.cpbPumpOne.SetBinding(CircleProgressBar.ForegroundProperty, cpbPumpOneMultiBind);
                // 2#主泵
                this.cpbPumpTwo.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_PumpTwoPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbPumpTwo.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_PumpTwoPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbPumpTwoMultiBind = new MultiBinding();
                cpbPumpTwoMultiBind.Converter = new ColorCoverter();
                cpbPumpTwoMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_PumpTwoPress"], Mode = BindingMode.OneWay });
                cpbPumpTwoMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbPumpTwo, Mode = BindingMode.OneWay });
                cpbPumpTwoMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbPumpTwo, Mode = BindingMode.OneWay });
                cpbPumpTwoMultiBind.NotifyOnSourceUpdated = true;
                this.cpbPumpTwo.SetBinding(CircleProgressBar.ForegroundProperty, cpbPumpTwoMultiBind);
                // 油温
                this.cpbOil.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_OilTmp"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbOil.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_OilTmp"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbOilMultiBind = new MultiBinding();
                cpbOilMultiBind.Converter = new ColorCoverter();
                cpbOilMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_OilTmp"], Mode = BindingMode.OneWay });
                cpbOilMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbOil, Mode = BindingMode.OneWay });
                cpbOilMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbOil, Mode = BindingMode.OneWay });
                cpbOilMultiBind.NotifyOnSourceUpdated = true;
                this.cpbOil.SetBinding(CircleProgressBar.ForegroundProperty, cpbOilMultiBind);
                // 电压
                this.cpbVol.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_Vol"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbVol.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_Vol"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbVolMultiBind = new MultiBinding();
                cpbVolMultiBind.Converter = new ColorCoverter();
                cpbVolMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_HS_Vol"], Mode = BindingMode.OneWay });
                cpbVolMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbVol, Mode = BindingMode.OneWay });
                cpbVolMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbVol, Mode = BindingMode.OneWay });
                cpbVolMultiBind.NotifyOnSourceUpdated = true;
                this.cpbVol.SetBinding(CircleProgressBar.ForegroundProperty, cpbVolMultiBind);

                this.btnIronOil.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["766b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnOilOne.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["766b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnOilTwo.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["766b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnOilAlarmOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["763b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnOilAlarmClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["763b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnWaterAlarmOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["763b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnWaterAlarmClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["763b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 控制模式
        /// </summary>
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.controlModel.IsChecked) // 当前本地切换司钻
            {
                byteToSend = new byte[10] { 0, 19, 18, 2, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 0, 19, 18, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 1#电机启动
        /// </summary>
        private void btn_PumpOne(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.PumpOne.IsChecked) // 当前停止切换启动
            {
                byteToSend = new byte[10] { 0, 19, 12, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 0, 19, 12, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 1#电风扇启停
        /// </summary>
        private void btn_FanOne(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.FanOne.IsChecked) // 当前停止切换启动
            {
                byteToSend = new byte[10] { 0, 19, 13, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 0, 19, 13, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 2#电机启动
        /// </summary>
        private void btn_PumpTwo(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.PumpTwo.IsChecked) // 当前停止切换启动
            {
                byteToSend = new byte[10] { 0, 19, 12, 3, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 0, 19, 12, 4, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 2#电风扇启停
        /// </summary>
        private void btn_FanTwo(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.PumpTwo.IsChecked) // 当前停止切换启动
            {
                byteToSend = new byte[10] { 0, 19, 13, 3, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 0, 19, 13, 4, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 图表定时器
        /// </summary>
        /// <param name="value"></param>
        private void ReportTimer_Elapse(object value)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    double drillTore = GlobalData.Instance.da["WR_HS_Water"].Value.Int16 / 10.0;
                    this.wr_HS_Self_Water.AddPoints(drillTore);
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 铁钻工油源开启
        /// </summary>
        private void btnIronOil_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工油源关闭
        /// </summary>
        private void btnIronOilClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 油源1开启
        /// </summary>
        private void btnOilOne_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 1, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 油源1关闭
        /// </summary>
        private void btnOilOneClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 1, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 油源2开启
        /// </summary>
        private void btnOilTwo_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 2, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btnOilTwoClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 2, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 油温报警开启
        /// </summary>
        private void btnOilAlarmOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 9, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 油温报警关闭
        /// </summary>
        private void btnOilAlarmClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 9, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 液位报警开启
        /// </summary>
        private void btnWaterAlarmOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 10, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 液位报警关闭
        /// </summary>
        private void btnWaterAlarmClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 10, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        private int iTimeCnt = 0;
        /// <summary>
        /// 报警时钟
        /// </summary>
        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
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
        private Dictionary<string, int> tipList = new Dictionary<string, int>(); //告警列表
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private bool bCommunicationCheck = false; // 是否有中断标志
        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
            #region 告警
            int key = 0;
            if (GlobalData.Instance.da["764b0"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站高油温报警", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站高油温报警", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站高油温报警", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站高油温报警");
                }
            }

            if (GlobalData.Instance.da["764b1"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站低油温报警", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站低油温报警", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站低油温报警", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站低油温报警");
                }
            }

            if (GlobalData.Instance.da["764b3"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站低液位报警", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站低液位报警", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站低液位报警", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站低液位报警");
                }
            }

            #endregion
            if (GlobalData.Instance.da["763b6"].Value.Boolean)
            {
                this.tbTips.Text = "液压站急停";
            }
                //操作台控制器心跳
                if (GlobalData.Instance.da["504b7"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 60)
                {
                    this.tipList.TryGetValue("液压站与操作台信号中断", out key);
                    if (key == 0)
                    {
                        this.tipList.Add("液压站与操作台信号中断", 1);
                    }
                }
                else
                {
                    this.tipList.TryGetValue("液压站与操作台信号中断", out key);
                    if (key != 0)
                    {
                        this.tipList.Remove("液压站与操作台信号中断");
                    }
                }
                if (!bCommunicationCheck && controlHeartTimes > 60)
                {
                    bCommunicationCheck = true;
                }
            }
            else
            {
                this.controlHeartTimes = 0;
            }
            this.tmpStatus = GlobalData.Instance.da["504b7"].Value.Boolean;

            if (!GlobalData.Instance.ComunciationNormal)
            {
                this.tipList.TryGetValue("网络连接失败", out key);
                if (key == 0)
                {
                    this.tipList.Add("网络连接失败", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("网络连接失败", out key);
                if (key != 0)
                {
                    this.tipList.Remove("网络连接失败");
                }
            }

            if (iTimeCnt % 10 == 0)
            {
                if (this.tipList.Count > 0)
                {
                    this.tbTips.FontSize = 20;
                    this.tbTips.Visibility = Visibility.Visible;

                    if (!this.tipList.ContainsValue(1))
                    {
                        this.tipList.Keys.ToList().ForEach(k => this.tipList[k] = 1);
                    }

                    foreach (var tkey in this.tipList.Keys.ToList())
                    {
                        if (this.tipList[tkey] == 1)
                        {
                            this.tbTips.Text = tkey;
                            this.tipList[tkey] = 2;
                            break;
                        }
                    }
                }
                else
                {
                    this.tbTips.Visibility = Visibility.Hidden;
                    this.tbTips.Text = "";
                }
            }
            else
            {
                this.tbTips.FontSize = 28;
            }

        }
    }
}
