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
using System.Windows.Shapes;

namespace Main.HydraulicStation.Sany
{
    /// <summary>
    /// MaintainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainWindow : Window
    {
        public MaintainWindow()
        {
            InitializeComponent();
            this.btnTurnMainOne.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
            this.btnTurnMainTwo.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
            this.btnMonitorOneGetOil.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
            this.btnMonitorTwoGetOil.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
            this.btnFilterReplace.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
            this.btnOilReplace.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
            this.btnOilLeakage.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

        }

        /// <summary>
        /// 已切换到主泵1运行
        /// </summary>
        private void BtnTurnMainOne_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认已切换到主泵1运行？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 2, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 36, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 已切换到主泵2运行
        /// </summary>
        private void BtnTurnMainTwo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认已切换到主泵2运行？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 1, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 35, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 主电机1加注黄油
        /// </summary>
        private void BtnMonitorOneGetOil_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认主电机1已加注黄油？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 3, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 37, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 主电机2加注黄油
        /// </summary>
        private void BtnMonitorTwoGetOil_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认主电机2已加注黄油？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 4, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 38, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 滤芯已更换
        /// </summary>
        private void BtnFilterReplace_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认滤芯已更换？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 5, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 39, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 液压油已更换
        /// </summary>
        private void BtnOilReplace_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认液压油已更换？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 6, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 40, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 漏油确认
        /// </summary>
        private void BtnOilLeakage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认无漏油？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 7, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 41, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        private void btn_close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
