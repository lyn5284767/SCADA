using COM.Common;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ControlLibrary.InputControl
{
    /// <summary>
    /// TextWithCombox.xaml 的交互逻辑
    /// </summary>
    public partial class TextWithCombox : UserControl
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string TbkText { get; set; }
        /// <summary>
        /// 第一标志，用于发送通信协议
        /// </summary>
        public int TagOne { get; set; }
        /// <summary>
        /// 第二标志，用于发送通信协议
        /// </summary>
        public int TagTwo { get; set; }
        public Dictionary<int, string> dicNumToValue { get; set; }
        public static readonly DependencyProperty ShowTxtWithCBProperty = DependencyProperty.Register("ShowTxtWithCB", typeof(string), typeof(TextWithCombox), new PropertyMetadata("无"));
        /// <summary>
        /// 控件显示
        /// </summary>
        public string ShowTxtWithCB
        {
            get { return (string)GetValue(ShowTxtWithCBProperty); }
            set { SetValue(ShowTxtWithCBProperty, value); }
        }
        public TextWithCombox()
        {
            InitializeComponent();
            this.Loaded += TextWithCombox_Loaded;
        }

        private void TextWithCombox_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbk.Text = TbkText;
            this.cbSet.ItemsSource = dicNumToValue;
            this.cbSet.SelectedValuePath = "Key";
            this.cbSet.DisplayMemberPath = "Value";
            this.cbSet.SelectedIndex = 0;
        }

        private void textBox_ParameterConfig_LostFocus(object sender, RoutedEventArgs e)
        {
            this.sh.Opacity = 0;
            string strText = this.cbSet.SelectedValue.ToString();
            if (strText.Length == 0) strText = "0";
                try
                {
                    if (GlobalData.Instance.systemType == SystemType.SIR)
                    {
                        short i16Text = Convert.ToInt16(strText);
                        byte[] tempByte = BitConverter.GetBytes(i16Text);
                        GlobalData.Instance.ConfigParameter[0] = (byte)TagTwo;
                        GlobalData.Instance.ConfigParameter[1] = (byte)TagOne;
                        GlobalData.Instance.ConfigParameter[2] = tempByte[0];
                        GlobalData.Instance.ConfigParameter[3] = tempByte[1];
                    }
                    else
                    {
                        short i16Text = Convert.ToInt16(strText);
                        byte[] tempByte = BitConverter.GetBytes(i16Text);
                        GlobalData.Instance.ConfigParameter[0] = (byte)TagOne;
                        GlobalData.Instance.ConfigParameter[1] = tempByte[0];
                        GlobalData.Instance.ConfigParameter[2] = tempByte[1];
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("参数数值范围异常:" + ex.Message);
                }
        }
    }
}
