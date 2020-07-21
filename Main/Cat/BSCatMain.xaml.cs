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

namespace Main.Cat
{
    /// <summary>
    /// BSCatMain.xaml 的交互逻辑
    /// </summary>
    public partial class BSCatMain : UserControl
    {
        private static BSCatMain _instance = null;
        private static readonly object syncRoot = new object();

        public static BSCatMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new BSCatMain();
                        }
                    }
                }
                return _instance;
            }
        }

        public BSCatMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 控制模式本地/司钻
        /// </summary>
        private void btn_controlModel(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 1#泵启动/停止
        /// </summary>
        private void btn_MainPumpOne(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 1#泵启动/停止
        /// </summary>
        private void btn_MainPumpTwo(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 左右选择
        /// </summary>
        private void btn_LeftOrRight(object sender, EventArgs e)
        {

        }
    }
}
