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
    /// IngSet.xaml 的交互逻辑
    /// </summary>
    public partial class IngSet : UserControl
    {
        private static IngSet _instance = null;
        private static readonly object syncRoot = new object();

        public static IngSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngSet();
                        }
                    }
                }
                return _instance;
            }
        }
        public IngSet()
        {
            InitializeComponent();
        }
    }
}
