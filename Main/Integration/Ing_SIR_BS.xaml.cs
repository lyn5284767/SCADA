using COM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Ing_SIR_BS.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_SIR_BS : UserControl
    {
        private static Ing_SIR_BS _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_SIR_BS Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_SIR_BS();
                        }
                    }
                }
                return _instance;
            }
        }
        public Ing_SIR_BS()
        {
            InitializeComponent();
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
                this.tbInButtonTimeShowValue.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRInButtonTime"], Mode = BindingMode.OneWay });
                this.tbOutButtonTimeShowValue.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIROutButtonTime"], Mode = BindingMode.OneWay });
                this.tbInButtonTimesShow.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIROutButtonTimes"], Mode = BindingMode.OneWay });

                this.cbDRSafeLimit.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["505b6"], Mode = BindingMode.OneWay });
              

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 上扣旋扣时间
        /// </summary>
        private void BtnInButtonTime_Click(object sender, RoutedEventArgs e)
        {
            int val = 0;
            int.TryParse(this.tbInButtonTimeSetValue.Text, out val);
            if (val > 0)
            {
                int paramOne = val % 256;
                int paramTwo = val / 256;
                byte[] data = new byte[10] { 80, 16, 2, 11, (byte)paramOne, (byte)paramTwo, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(data);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("数据解析错误");
            }
        }
        /// <summary>
        /// 卸扣旋扣时间
        /// </summary>
        private void BtnOutButtonTime_Click(object sender, RoutedEventArgs e)
        {
            int val = 0;
            int.TryParse(this.tbOutButtonTimeSetValue.Text, out val);
            if (val > 0)
            {
                int paramOne = val % 256;
                int paramTwo = val / 256;
                byte[] data = new byte[10] { 80, 16, 2, 12, (byte)paramOne, (byte)paramTwo, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(data);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("数据解析错误");
            }
        }
        /// <summary>
        /// 冲扣次数
        /// </summary>
        private void BtnInButtonTimes_Click(object sender, RoutedEventArgs e)
        {
            int val = 0;
            int.TryParse(this.tbInButtonTimesSet.Text, out val);
            if (val > 0)
            {
                int paramOne = val % 256;
                int paramTwo = val / 256;
                byte[] data = new byte[10] { 80, 16, 2, 13, (byte)paramOne, (byte)paramTwo, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(data);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("数据解析错误");
            }
        }

        /// <summary>
        /// 钻台面安全限制解除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDRSafeLimit_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked.Value)
            {
                byte[] byteToSend = new byte[10] { 80, 16, 1, 24, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("确认解除钻台面对铁钻工的安全设置?", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    byte[] byteToSend = new byte[10] { 80, 16, 1, 24, 1, 0, 0, 0, 0, 0 };
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
        }
    }
}
