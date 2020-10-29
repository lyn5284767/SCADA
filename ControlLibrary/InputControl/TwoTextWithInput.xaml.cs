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

namespace ControlLibrary.InputControl
{
    /// <summary>
    /// TwoTextWitnInput.xaml 的交互逻辑
    /// </summary>
    public partial class TwoTextWithInput : UserControl
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string TbkText { get; set; }
        /// <summary>
        /// 控件标志
        /// </summary>
        public string Head { get; set; }
        /// <summary>
        /// 控件输入值
        /// </summary>
        public int ControlText { get; set; }

        public Visibility TextTwoVisible { get; set; }


        public static readonly DependencyProperty TextOneShowTextProperty = DependencyProperty.Register("TextOneShowText", typeof(object), typeof(TwoTextWithInput), new PropertyMetadata(0));
        public static readonly DependencyProperty TextTwoShowTextProperty = DependencyProperty.Register("TextTwoShowText", typeof(object), typeof(TwoTextWithInput), new PropertyMetadata(0));
        /// <summary>
        /// 控件显示
        /// </summary>
        public object TextOneShowText
        {
            get { return GetValue(TextOneShowTextProperty); }
            set { SetValue(TextOneShowTextProperty, value); }
        }
        /// <summary>
        /// 控件显示
        /// </summary>
        public object TextTwoShowText
        {
            get { return GetValue(TextTwoShowTextProperty); }
            set { SetValue(TextTwoShowTextProperty, value); }
        }
        public TwoTextWithInput()
        {
            InitializeComponent();
            this.Loaded += TwoTextWitnInput_Loaded;
        }

        private void TwoTextWitnInput_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbk.Text = TbkText;
            this.textBoxShowTwo.Visibility = this.TextTwoVisible;
        }

        private void textBox_ParameterConfig_LostFocus(object sender, RoutedEventArgs e)
        {
            //this.stackPanel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E1E3"));
            this.sh.Opacity = 0;
            Regex regexParameterConfigurationConfirm = new Regex(@"^[0-9]+$");
            string strText = this.textBoxSet.Text;
            if (strText.Length == 0) strText = "0";
            short i16Text = Convert.ToInt16(strText);
            byte[] tempByte = BitConverter.GetBytes(i16Text);
            if ((regexParameterConfigurationConfirm.Match(strText)).Success)
            {
                try
                {
                    string[] strs = this.Head.Split(',');
                    for (int i = 0; i < strs.Length; i++) // 协议头
                    {
                        GlobalData.Instance.SetParam[i] = byte.Parse(strs[i]);
                    }
                    // 协议内容
                    GlobalData.Instance.SetParam[strs.Length] = tempByte[0];
                    GlobalData.Instance.SetParam[strs.Length + 1] = tempByte[1];
                    // 补零
                    for (int i = strs.Length + 2; i < 10; i++)
                    {
                        GlobalData.Instance.SetParam[i] = 0;
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
            this.sh.Opacity = 0.5;
            GlobalData.Instance.GetKeyBoard();
        }
    }
}
