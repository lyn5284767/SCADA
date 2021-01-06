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
using System.Windows.Shapes;

namespace Main.SecondFloor
{
    /// <summary>
    /// SFStop.xaml 的交互逻辑
    /// </summary>
    public partial class SFStop : Window,IDisposable
    {
        private static SFStop _instance = null;
        private static readonly object syncRoot = new object();

        public static SFStop Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFStop();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFStop()
        {
            InitializeComponent();
            this.tbZeroSet.SetBinding(BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["575b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
        }

        public void info(string txt)
        {
            this.tbinfo.Text = txt;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {         
            this.Dispose();
        }

        public void Dispose()
        {
            this.Close();
            _instance = null;
        }
        /// <summary>
        /// 零位标定
        /// </summary>
        private void ZeroSet_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =  MessageBox.Show("确认标定零位?", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                byte[] byteToSend = new byte[] { 16, 1, 24, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 取消零位标定
        /// </summary>
        private void ZeroCancek_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认取消标定零位?", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                byte[] byteToSend = new byte[] { 16, 1, 24, 11, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
