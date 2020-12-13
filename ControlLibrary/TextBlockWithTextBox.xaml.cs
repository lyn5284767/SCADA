using COM.Common;
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

namespace ControlLibrary
{
    /// <summary>
    /// TextBlockWithTexitBox.xaml 的交互逻辑
    /// </summary>
    public partial class TextBlockWithTextBox : UserControl
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string TbkText { get; set; }
        /// <summary>
        /// 控件标志
        /// </summary>
        public int ControlTag { get; set; }
        /// <summary>
        /// 第二标志，用于发送通信协议
        /// </summary>
        public int TagTwo { get; set; }
        /// <summary>
        /// 控件输入值
        /// </summary>
        public int ControlText { get; set; }

        public delegate void CtrGetFocus(int crtTag);

        public event CtrGetFocus CtrGetFocusEvent;

        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register("ShowText", typeof(object), typeof(TextBlockWithTextBox), new PropertyMetadata(0));
        /// <summary>
        /// 控件显示
        /// </summary>
        public object ShowText
        {
            get { return GetValue(ShowTextProperty); }
            set { SetValue(ShowTextProperty, value); }
        }
        public TextBlockWithTextBox()
        {
            InitializeComponent();
            this.Loaded += TextBlockWithTextBox_Loaded;
        }

        private void TextBlockWithTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbk.Text = TbkText;
        }

        /// <summary>
        /// 获取焦点，设为绿色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_ParameterConfig_Focus(object sender, RoutedEventArgs e)
        {
            //this.stackPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5FBA7D"));
            this.sh.Opacity = 0.5;
            if (CtrGetFocusEvent != null)
            {
                CtrGetFocusEvent(this.ControlTag);
            }
            GlobalData.Instance.GetKeyBoard();
        }
        /// <summary>
        /// 失去焦点，赋值通信协议，设为白色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_ParameterConfig_LostFocus(object sender, RoutedEventArgs e)
        {
            //this.stackPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E1E3"));
            this.sh.Opacity = 0;
            Regex regexParameterConfigurationConfirm = new Regex(@"^[0-9]+$");
            string strText = this.textBoxSet.Text;
            if (strText.Length == 0) strText = "0";
            if ((regexParameterConfigurationConfirm.Match(strText)).Success)
            {
                try
                {
                    if (GlobalData.Instance.systemType == SystemType.SIR)
                    {
                        short i16Text = Convert.ToInt16(strText);
                        byte[] tempByte = BitConverter.GetBytes(i16Text);
                        GlobalData.Instance.ConfigParameter[0] = (byte)TagTwo;
                        GlobalData.Instance.ConfigParameter[1] = (byte)ControlTag;
                        GlobalData.Instance.ConfigParameter[2] = tempByte[0];
                        GlobalData.Instance.ConfigParameter[3] = tempByte[1];
                    }
                    else
                    {
                        short i16Text = Convert.ToInt16(strText);
                        byte[] tempByte = BitConverter.GetBytes(i16Text);
                        GlobalData.Instance.ConfigParameter[0] = (byte)ControlTag;
                        GlobalData.Instance.ConfigParameter[1] = tempByte[0];
                        GlobalData.Instance.ConfigParameter[2] = tempByte[1];
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("参数数值范围异常:" + ex.Message);
                    this.textBoxSet.Text = "0";
                }

            }
            else
            {
                MessageBox.Show("参数为非数字");
                this.textBoxSet.Text = "0";
            }
        }

        private void tb_ParameterConfig_Focus(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.sh.Opacity = 0.5;
                if (CtrGetFocusEvent != null)
                {
                    CtrGetFocusEvent(this.ControlTag);
                }
                GlobalData.Instance.GetKeyBoard();
            }
        }
    }
}
