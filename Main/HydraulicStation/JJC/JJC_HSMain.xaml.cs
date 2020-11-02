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

namespace Main.HydraulicStation.JJC
{
    /// <summary>
    /// JJC_HSMain.xaml 的交互逻辑
    /// </summary>
    public partial class JJC_HSMain : UserControl
    {
        private static JJC_HSMain _instance = null;
        private static readonly object syncRoot = new object();

        public static JJC_HSMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new JJC_HSMain();
                        }
                    }
                }
                return _instance;
            }
        }

        public JJC_HSMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 主泵1启停
        /// </summary>
        private void btn_MainPumpOne(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpOne.IsChecked) //当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 主泵2启停
        /// </summary>
        private void btn_MainPumpTwo(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpOne.IsChecked) //当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 冷却风机启停
        /// </summary>
        private void btn_ColdFan(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpOne.IsChecked) //当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 加热器启停
        /// </summary>
        private void btn_Hot(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpOne.IsChecked) //当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 左猫头升降
        /// </summary>
        private void btn_LeftCat(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpOne.IsChecked) //当前上升
            {
                byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前下降
            {
                byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 右猫头升降
        /// </summary>
        private void btn_RightCat(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpOne.IsChecked) //当前上升
            {
                byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前下降
            {
                byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 旋转猫头正反转
        /// </summary>
        private void btn_RotateCat(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpOne.IsChecked) //当前正转
            {
                byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前反转
            {
                byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btn_SelectWorkModel(object sender, RoutedEventArgs e)
        {

        }

        private void btn_SelectCatOilModel(object sender, RoutedEventArgs e)
        {

        }
    }
}
