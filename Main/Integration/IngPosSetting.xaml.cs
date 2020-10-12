using COM.Common;
using ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// IngPosSetting.xaml 的交互逻辑
    /// </summary>
    public partial class IngPosSetting : UserControl
    {
        private static IngPosSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static IngPosSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngPosSetting();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        public IngPosSetting()
        {
            InitializeComponent();
            VariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 1000, 500);//改成50ms 的时钟
        }
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    //string msg = GetMsg(GlobalData.Instance.da["155InterlockPromptMessageCode"].Value.Byte);
                    //if (msg != string.Empty && tmp!= msg)
                    //{
                    //    MessageBox.Show(msg);
                    //}
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private string GetMsg(int val)
        {
            if (val == 1) return "零位标定不合理";
            else if (val == 2) return "高位标定不合理";
            else if (val == 3) return "高度设置不合理";
            else if (val == 11) return "低位刹车设置成功";
            else if (val == 12) return "低位报警区设置成功";
            else if (val == 22) return "低位报警区设置不合理";
            else if (val == 13) return "低位设置成功";
            else if (val == 23) return "低位设置不合理";
            else if (val == 14) return "中位设置成功";
            else if (val == 24) return "中位设置不合理";
            else if (val == 15) return "高位设置成功";
            else if (val == 25) return "高位设置不合理";
            else if (val == 16) return "高位报警区设置成功";
            else if (val == 26) return "高位报警区设置不合理";
            else if (val == 17) return "高位刹车区设置成功";
            else if (val == 27) return "高位刹车区设置不合理";
            else return string.Empty;
        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.tbZeroSet.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["575b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.tbHighSet.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["575b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.twtL3.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["LowBrakeArea"], Mode = BindingMode.OneWay });
                this.twtL4.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["LowAlarmArea"], Mode = BindingMode.OneWay });
                this.twtL5.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["LowArea"], Mode = BindingMode.OneWay });
                this.twtL6.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["MidArea"], Mode = BindingMode.OneWay });
                this.twtL7.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HighArea"], Mode = BindingMode.OneWay });
                this.twtL8.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HighAlarmArea"], Mode = BindingMode.OneWay });
                this.twtL9.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HighBrakeArea"], Mode = BindingMode.OneWay });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private byte[] bConfigParameter = new byte[4];
        private void Button_Setting(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = new byte[] { 16, 1, 25, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2], 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            ShowTips();
        }

        private void ShowTips()
        {
            Thread.Sleep(500);
            string msg = GetMsg(GlobalData.Instance.da["155InterlockPromptMessageCode"].Value.Byte);
            if (msg != string.Empty)
            {
                MessageBox.Show(msg);
            }
        }

        /// <summary>
        /// 零位标定--弃用2020.10.9
        /// </summary>
        private void cbZeroSet_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (!cb.IsChecked.Value) //取消零位标定
            {
                byte[] byteToSend = new byte[] { 16, 1, 24, 11, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                byte[] byteToSend = new byte[] { 16, 1, 24, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            ShowTips();
        }
        /// <summary>
        /// 高位标定--弃用2020.10.9
        /// </summary>
        private void cbHighSet_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (!cb.IsChecked.Value)
            {
                byte[] byteToSend = new byte[] { 16, 1, 24, 12, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                byte[] byteToSend = new byte[] { 16, 1, 24, 2, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 高位标定
        /// </summary>
        private void tbHighSet_Checked(object sender, RoutedEventArgs e)
        {
            Regex regexParameterConfigurationConfirm = new Regex(@"^[0-9]+$");
            if ((regexParameterConfigurationConfirm.Match(this.tbHighVal.Text)).Success)
            {
                short i16Text = Convert.ToInt16(this.tbHighVal.Text);
                byte[] tempByte = BitConverter.GetBytes(i16Text);
                byte[] byteToSend = new byte[] { 16, 1, 24, 2, tempByte[0], tempByte[1], 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                MessageBox.Show("输入有误");
            }
            ShowTips();
        }
        /// <summary>
        /// 取消高位标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbHighUnSet_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[] { 16, 1, 24, 12, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            ShowTips();
        }
        /// <summary>
        /// 零位标定
        /// </summary>
        private void tbZeroSet_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[] { 16, 1, 24, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            ShowTips();
        }
        /// <summary>
        /// 取消零位标定
        /// </summary>
        private void tbZeroUnSet_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[] { 16, 1, 24, 11, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            ShowTips();
        }
    }
}
