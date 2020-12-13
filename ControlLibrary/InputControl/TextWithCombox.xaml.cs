using COM.Common;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        public string Head { get; set; }
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
            string strText = string.Empty;
            if (this.cbSet.SelectedValue != null) strText = this.cbSet.SelectedValue.ToString();
            if (strText.Length == 0) strText = "0";
            short i16Text = Convert.ToInt16(strText);
            byte[] tempByte = BitConverter.GetBytes(i16Text);
            string[] strs = this.Head.Split(',');
            for (int i = 0; i < strs.Length; i++) // 协议头
            {
                    GlobalData.Instance.SetParam[i] = byte.Parse(strs[i]);
            }
            // 协议内容
            GlobalData.Instance.SetParam[strs.Length] = tempByte[0];
            GlobalData.Instance.SetParam[strs.Length +1] = tempByte[1];
            // 补零
            for (int i = strs.Length + 2; i < 10; i++)
            {
                GlobalData.Instance.SetParam[i] = 0;
            }
            BrushConverter bc = new BrushConverter();

            this.stackPanel.Background = (Brush)bc.ConvertFrom("#FFFFFF");
        }


        private void cbSet_GotFocus(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();

            this.stackPanel.Background = (Brush)bc.ConvertFrom("#4E80C8");
        }
    }
}
