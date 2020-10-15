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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.SIR.SanyRail
{
    /// <summary>
    /// SIRRailWayParamSet.xaml 的交互逻辑
    /// </summary>
    public partial class SIRRailWayParamSet : UserControl
    {
        private static SIRRailWayParamSet _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRRailWayParamSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRRailWayParamSet();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRRailWayParamSet()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 确定配置
        /// </summary>
        private void Button_Setting(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.SetParam[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SetParam;
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
