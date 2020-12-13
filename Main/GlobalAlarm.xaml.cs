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

namespace Main
{
    /// <summary>
    /// GlobalAlarm.xaml 的交互逻辑
    /// </summary>
    public partial class GlobalAlarm : Window
    {
        public GlobalAlarm()
        {
            InitializeComponent();
        }

        public void info(string txt)
        {
            this.tbinfo.Text = txt;
        }
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.Instance.HS_OilHigh = false;
            this.Close();
            
        }
    }
}
