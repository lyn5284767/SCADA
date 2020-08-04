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

namespace Main.SIR
{
    /// <summary>
    /// SIRSelfMain.xaml 的交互逻辑
    /// </summary>
    public partial class SIRSelfMain : UserControl
    {
        private static SIRSelfMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRSelfMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRSelfMain();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRSelfMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 控制模式
        /// </summary>
        private void btn_controlModel(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_workModel(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_oprModel(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 工位选择
        /// </summary>
        private void btn_locationModel(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 管柱选择
        /// </summary>
        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {

        }
    }
}
