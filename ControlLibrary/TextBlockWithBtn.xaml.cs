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
    /// TextBlockWithBtn.xaml 的交互逻辑
    /// </summary>
    public partial class TextBlockWithBtn : UserControl
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string TbkText { get; set; }
        /// <summary>
        /// 控件标志
        /// </summary>
        public int ControlTag { get; set; }
        public static readonly DependencyProperty ShowTxtProperty = DependencyProperty.Register("ShowTxt", typeof(string), typeof(TextBlockWithBtn), new PropertyMetadata("0"));
        /// <summary>
        /// 控件显示
        /// </summary>
        public string ShowTxt
        {
            get { return (string)GetValue(ShowTxtProperty); }
            set { SetValue(ShowTxtProperty, value); }
        }

        public delegate void TBMouseDown(int crtTag, string txt);

        public event TBMouseDown textBoxSetMouseDownEvent;

        public TextBlockWithBtn()
        {
            InitializeComponent();
            this.Loaded += TextBlockWithBtn_Loaded;
        }

        private void TextBlockWithBtn_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbk.Text = TbkText;
        }

        private void TextBoxSet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBox = (sender as TextBlock);
            if (textBox.Text == "读取")
            {
                textBox.Text = "标定";
            }
            else
            {
                textBox.Text = "读取";
            }
            if (textBoxSetMouseDownEvent != null)
            {
                textBoxSetMouseDownEvent(this.ControlTag, textBox.Text);
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
