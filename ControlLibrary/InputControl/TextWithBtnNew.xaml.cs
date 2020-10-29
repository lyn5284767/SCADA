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

namespace ControlLibrary.InputControl
{
    /// <summary>
    /// TextWithBtnNew.xaml 的交互逻辑
    /// </summary>
    public partial class TextWithBtnNew : UserControl
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string TbkText { get; set; }
        /// <summary>
        /// 协议头
        /// </summary>
        public string Head { get; set; }
        /// <summary>
        /// 协议
        /// </summary>
        public int TagOne { get; set; }
        public static readonly DependencyProperty ShowTxtWithBtnTxtProperty = DependencyProperty.Register("ShowTxtWithBtnTxt", typeof(string), typeof(TextWithBtnNew), new PropertyMetadata("0"));
        /// <summary>
        /// 控件显示
        /// </summary>
        public string ShowTxtWithBtnTxt
        {
            get { return (string)GetValue(ShowTxtWithBtnTxtProperty); }
            set { SetValue(ShowTxtWithBtnTxtProperty, value); }
        }

        public TextWithBtnNew()
        {
            InitializeComponent();
            this.Loaded += TextWithBtnNew_Loaded;
        }

        private void TextWithBtnNew_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbk.Text = TbkText;
        }


        private void TextBoxSet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string[] strs = this.Head.Split(',');
            for (int i = 0; i < strs.Length; i++) // 协议头
            {
                GlobalData.Instance.SetParam[i] = byte.Parse(strs[i]);
            }
            // 补零
            for (int i = strs.Length; i < 10; i++)
            {
                GlobalData.Instance.SetParam[i] = 0;
            }
            if (GlobalData.Instance.SetParam[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SetParam;
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            this.sh.Opacity = 0.5;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            this.sh.Opacity = 0;
        }
    }
}
