using COM.Common;
using ControlLibrary;
using Log;
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
    /// IngCat.xaml 的交互逻辑
    /// </summary>
    public partial class IngCat : UserControl
    {
        private static IngCat _instance = null;
        private static readonly object syncRoot = new object();

        public static IngCat Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngCat();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        public IngCat()
        {
            InitializeComponent();
            VariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }

        private void VariableBinding()
        {
            try
            {
                this.smRight.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["504b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBigCarEncode.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInDangerArea.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSmallCarEncode.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.smSpare.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["504b2"], Mode = BindingMode.OneWay });

                this.btnStart.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                this.smLeftOrRight.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["505b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInOrOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["505b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smAutoUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["509b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smAutoDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["509b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smAutoIn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["509b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smAutoOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["509b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOnkeyUseDrill.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOnKeyUnuserDrill.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBentUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBentDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smTiltUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smTiltDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOrganKickOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOrganRetract.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            }
            catch (Exception ex)
            {
                Log4Net.AddLog(ex.StackTrace, InfoLevel.ERROR);
            }
        }

        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Warning();
                    if (!GlobalData.Instance.ComunciationNormal) this.tbAlarmTips.Text = "网络连接失败！";
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private void Warning()
        {
            if (GlobalData.Instance.da["504b2"].Value.Boolean)
            {
                if (!GlobalData.Instance.da["334b4"].Value.Boolean && !GlobalData.Instance.da["505b7"].Value.Boolean)
                {
                    this.tbAlarmTips.Text = "禁止猫道前进，请先将钻台面回收至安全位置！";
                }
            }
            else
            {
                this.tbAlarmTips.Text = "请先将旋钮选择猫道！";
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 1, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 辅助上升
        /// </summary>
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 4, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 辅助下降
        /// </summary>
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 4, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 多根踢出
        /// </summary>
        private void btnKickOut_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 4, 5, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
