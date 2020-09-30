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
    /// IngSIR.xaml 的交互逻辑
    /// </summary>
    public partial class IngSIR : UserControl
    {
        private static IngSIR _instance = null;
        private static readonly object syncRoot = new object();

        public static IngSIR Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngSIR();
                        }
                    }
                }
                return _instance;
            }
        }
        public IngSIR()
        {
            InitializeComponent();
        }
    }
}
