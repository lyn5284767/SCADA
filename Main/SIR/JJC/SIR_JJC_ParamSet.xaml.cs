using COM.Common;
using ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.SIR.JJC
{
    /// <summary>
    /// SIR_JJC_ParamSet.xaml 的交互逻辑
    /// </summary>
    public partial class SIR_JJC_ParamSet : UserControl
    {
        private static SIR_JJC_ParamSet _instance = null;
        private static readonly object syncRoot = new object();

        public static SIR_JJC_ParamSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIR_JJC_ParamSet();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIR_JJC_ParamSet()
        {
            InitializeComponent();
            this.tbCalibrationStatus.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b3"], Mode = BindingMode.OneWay, Converter = new SIRCalibrationStatusCoverter() });
            this.tbRealTimeRatate.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDRotate"], Mode = BindingMode.OneWay });
            this.tbCylinderTrip.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDCylinderTrip"], Mode = BindingMode.OneWay });
            this.tbCylinderZero.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDCylinderZero"], Mode = BindingMode.OneWay });
            this.tbZeroAngleOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDRotateZero"], Mode = BindingMode.OneWay });
            this.tbRotateWaitOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDRotateWait"], Mode = BindingMode.OneWay });
            this.tbWellLocationOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDWellLocation"], Mode = BindingMode.OneWay });
            this.tbMouseLocationOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDMouseLocatio"], Mode = BindingMode.OneWay });

            this.tbInOutStatus.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b4"], Mode = BindingMode.OneWay, Converter = new SIRCalibrationStatusCoverter() });
            this.tbInOutCylinderTrip.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDInOutCylinderTrip"], Mode = BindingMode.OneWay });
            this.tbFlowRateValve.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDFlowRateValve"], Mode = BindingMode.OneWay });
            this.tbPressureRateValve.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDPressureRateValve"], Mode = BindingMode.OneWay });
            this.tbInOutMaxSpeedOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDInOutMaxSpeed"], Mode = BindingMode.OneWay });
            this.tbInOutCrawlSpeedOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDInOutCrawlSpeed"], Mode = BindingMode.OneWay });
            this.tbWellRetractLengthOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDWellRetractLength"], Mode = BindingMode.OneWay });
            this.tbMouseRetractLengthOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDMouseRetractLength"], Mode = BindingMode.OneWay });
            this.tbInOutCylinderZeroOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDInOutCylinderZero"], Mode = BindingMode.OneWay });
            this.smRotate.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smInOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

        }

        /// <summary>
        /// 旋转设定请求
        /// </summary>
        private void spRotateSetMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 23, 1, 0, 0, 1, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
            this.effRotateSelect.BorderBrush = new SolidColorBrush(Colors.Blue);
            var bc = new BrushConverter();
            this.effInOutSelect.BorderBrush = (Brush)bc.ConvertFrom("#F6F9FF");
        }
        /// <summary>
        /// 伸缩标定请求
        /// </summary>
        private void InOutSetMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 23, 2, 0, 0, 1, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
            this.effInOutSelect.BorderBrush = new SolidColorBrush(Colors.Blue);
            var bc = new BrushConverter();
            this.effRotateSelect.BorderBrush = (Brush)bc.ConvertFrom("#F6F9FF");
        }

        /// <summary>
        /// 零位角度标定
        /// </summary>
        private void ZeroAngleSet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 23, 1, 0, 0, 2, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }

        /// <summary>
        /// 获取键盘
        /// </summary>
        private void tbFocus(object sender, RoutedEventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }
        /// <summary>
        /// 失去焦点绑定值
        /// </summary>
        private void tbLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            Regex regexParameterConfigurationConfirm = new Regex(@"^[0-9]+$");
            string strText = tb.Text;
            if (strText.Length == 0) strText = "0";
            if ((regexParameterConfigurationConfirm.Match(strText)).Success)
            {
                try
                {
                    if (strText.Length == 0) strText = "0";
                    short i16Text = (short)(double.Parse(strText) * 10);

                    //short i16Text = Convert.ToInt16(strText);
                    byte[] tempByte = BitConverter.GetBytes(i16Text);
                    GlobalData.Instance.ConfigParameter[0] = (byte)int.Parse(tb.Tag.ToString());
                    GlobalData.Instance.ConfigParameter[1] = tempByte[0];
                    GlobalData.Instance.ConfigParameter[2] = tempByte[1];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("参数数值范围异常:" + ex.Message);
                    tb.Text = "0";
                }

            }
            else
            {
                MessageBox.Show("参数为非数字");
                tb.Text = "0";
            }
        }

        private byte[] bConfigParameter = new byte[3];
        private void ParamSet(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SendListToByte(new List<byte>() { 80, 16, 23, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2], 2 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
