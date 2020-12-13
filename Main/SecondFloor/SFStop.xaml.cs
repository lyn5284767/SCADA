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
    }
}
