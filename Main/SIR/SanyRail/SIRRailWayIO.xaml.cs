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

namespace Main.SIR.SanyRail
{
    /// <summary>
    /// SIRRailWayIO.xaml 的交互逻辑
    /// </summary>
    public partial class SIRRailWayIO : UserControl
    {
        private static SIRRailWayIO _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRRailWayIO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRRailWayIO();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRRailWayIO()
        {
            InitializeComponent();
        }
    }
}
