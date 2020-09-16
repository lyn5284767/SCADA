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
    /// SFSet.xaml 的交互逻辑
    /// </summary>
    public partial class SFSet : UserControl
    {
        private static SFSet _instance = null;
        private static readonly object syncRoot = new object();

        public static SFSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFSet();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFSet()
        {
            InitializeComponent();
        }
    }
}
