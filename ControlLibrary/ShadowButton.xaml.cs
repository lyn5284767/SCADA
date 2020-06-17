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

namespace ControlLibrary
{
    /// <summary>
    /// ShadowButton.xaml 的交互逻辑
    /// </summary>
    public partial class ShadowButton : UserControl
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string TBText { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int BDWidth { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int BDHeight { get; set; }


        public static readonly DependencyProperty ShadowButtonShowTxtProperty = DependencyProperty.Register("ShadowButtonShowTxt", typeof(string), typeof(ShadowButton), new PropertyMetadata("0"));
        /// <summary>
        /// 控件显示
        /// </summary>
        public string ShowText
        {
            get { return (string)GetValue(ShadowButtonShowTxtProperty); }
            set { SetValue(ShadowButtonShowTxtProperty, value); }
        }
        /// <summary>
        /// 字体大小
        /// </summary>
        public int TBFontSize { get; set; }
        public ShadowButton()
        {
            InitializeComponent();
            this.Loaded += ShadowButton_Loaded;
        }

        private void ShadowButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.TBText != string.Empty) this.tbTxt.Text = this.TBText;
            if (this.TBFontSize > 0) this.tbTxt.FontSize = this.TBFontSize;
            if (this.BDWidth > 0) this.bd.Width = this.BDWidth;
            if (this.BDHeight > 0) this.bd.Height = this.BDHeight;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            this.sh.Opacity = 0.5;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            this.sh.Opacity = 0.1;
        }
    }
}
