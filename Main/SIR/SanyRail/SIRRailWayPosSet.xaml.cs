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
    /// SIRPosSet.xaml 的交互逻辑
    /// </summary>
    public partial class SIRRailWayPosSet : UserControl
    {
        private static SIRRailWayPosSet _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRRailWayPosSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRRailWayPosSet();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRRailWayPosSet()
        {
            InitializeComponent();
        }
    }
}
