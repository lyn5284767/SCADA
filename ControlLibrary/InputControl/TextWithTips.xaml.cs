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
    /// TextWithTips.xaml 的交互逻辑
    /// </summary>
    public partial class TextWithTips : UserControl
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
        /// 最大值
        /// </summary>
        public double MaxVal { get; set; } = 100;
        /// <summary>
        /// 最小值
        /// </summary>
        public double MinVal { get; set; } = 0;
        /// <summary>
        /// 第一列单位
        /// </summary>
        public string UnitOne { get; set;}
        /// <summary>
        /// 第二列单位
        /// </summary>
        public string UnitTwo { get; set; }
        /// <summary>
        /// 第三列单位
        /// </summary>
        public string UnitThree { get; set; }

        public bool HidenTwo { get; set; }

        public bool Multiply { get; set; } = false;

        public static readonly DependencyProperty ShowTextWithTipsProperty = DependencyProperty.Register("ShowTextTips", typeof(object), typeof(TextWithTips), new PropertyMetadata(0));
        /// <summary>
        /// 控件显示
        /// </summary>
        public object ShowTextTips
        {
            get { return GetValue(ShowTextWithTipsProperty); }
            set { SetValue(ShowTextWithTipsProperty, value); }
        }

        public static readonly DependencyProperty TransTextWithTipsProperty = DependencyProperty.Register("TransTextWithTips", typeof(object), typeof(TextWithTips), new PropertyMetadata(0));
        /// <summary>
        /// 控件显示
        /// </summary>
        public object TransTextWithTips
        {
            get { return GetValue(TransTextWithTipsProperty); }
            set { SetValue(TransTextWithTipsProperty, value); }
        }
        public TextWithTips()
        {
            InitializeComponent();
            this.Loaded += TextBlockWithTextBox_Loaded;
        }

        private void TextBlockWithTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbk.Text = TbkText;
            this.tbInputUnit.Text = UnitThree;
            this.tbTwoUnit.Text = UnitTwo;
            this.tbOneUnit.Text = UnitOne;
            if (this.HidenTwo)
            {
                this.tbTwoUnit.Visibility = Visibility.Collapsed;
                this.textBoxTrans.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 获取焦点，设为绿色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_ParameterConfig_Focus(object sender, RoutedEventArgs e)
        {
            this.sh.Opacity = 0.5;

            GlobalData.Instance.GetKeyBoard();
        }
        /// <summary>
        /// 失去焦点，赋值通信协议，设为白色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_ParameterConfig_LostFocus(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            this.bd.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.sh.Opacity = 0;
            string strText = this.textBoxSet.Text;
            if (strText.Length == 0) strText = "0";
            short i16Text = (short)(double.Parse(strText));
            if (i16Text > MaxVal || i16Text < MinVal)
            {
                MessageBox.Show(string.Format("参数应在{0}到{1}之间,超出范围", MaxVal, MinVal));
                return;
            }
            if (Multiply)
            {
                i16Text = (short)(double.Parse(strText) * 10);
            }
            byte[] tempByte = BitConverter.GetBytes(i16Text);
            try
            {
                string[] strs = this.Head.Split(',');
                for (int i = 0; i < strs.Length; i++) // 协议头
                {
                    GlobalData.Instance.SetParam[i] = byte.Parse(strs[i]);
                }
                if (this.Head == "24,17,1,1" || this.Head == "24,17,2,1")
                {
                    double input = double.Parse(strText);
                    short data = (short)(((-6 * input * input) / 100000 + 2.1019 * input + 2.1023) * 10);// 发操作台最后*10
                    tempByte = BitConverter.GetBytes(data);
                }
                if (this.Head == "24,17,3,3")
                {
                    double input = double.Parse(strText);
                    short data = (short)(0.14449 * input * 10);// 发操作台最后*10
                    tempByte = BitConverter.GetBytes(data);
                }
                if (this.Head == "24,17,2,3")
                {
                    double input = double.Parse(strText);
                    if (input >= 8.5)
                    {
                        short data = (short)(0.14449 * input * 10);// 发操作台最后*10
                        tempByte = BitConverter.GetBytes(data);
                    }
                    else
                    {
                        short data = (short)(((-6 * input * input) / 100000 + 2.1019 * input + 2.1023) * 10);// 发操作台最后*10
                        tempByte = BitConverter.GetBytes(data);
                    }
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

        private void tb_ParameterConfig_Focus(object sender, MouseButtonEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            this.sh.Opacity = 0.5;
            this.bd.Background = (Brush)bc.ConvertFrom("#4E80C8");
            GlobalData.Instance.GetKeyBoard();
        }
    }
}
