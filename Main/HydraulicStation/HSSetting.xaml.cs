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

namespace Main.HydraulicStation
{
    /// <summary>
    /// HSSetting.xaml 的交互逻辑
    /// </summary>
    public partial class HSSetting : UserControl
    {
        private static HSSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static HSSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new HSSetting();
                        }
                    }
                }
                return _instance;
            }
        }

        public HSSetting()
        {
            InitializeComponent();
        }

        private void btn_controlModel(object sender, EventArgs e)
        {

        }
    }
}
