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
        public WR_HS_Self_Main()
        {
            InitializeComponent();
            VariableBinding();
            ReportTimer = new System.Threading.Timer(new TimerCallback(ReportTimer_Elapse), null, 500, 2000);
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
                    double drillTore = GlobalData.Instance.da["SIR_RailWay_SystemPress"].Value.Int16 / 1.0;
                    this.wr_HS_Self_Water.AddPoints(drillTore);
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
