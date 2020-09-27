using COM.Common;
using ControlLibrary;
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
        public IngPosSetting()
        {
            InitializeComponent();
            VariableBinding();
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.cbZeroSet.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["575b0"], Mode = BindingMode.OneWay });
                this.cbHighSet.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["575b1"], Mode = BindingMode.OneWay });
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
        }

        /// <summary>
        /// 零位标定
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
        }
        /// <summary>
        /// 高位标定
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
    }
}
