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

namespace Main.Integration
{
    /// <summary>
    /// IngSetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IngSetWindow : Window
    {
        public IngSetWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalData.Instance.DeviceLink.Clear();
                if (this.cbSF.IsChecked.Value) GlobalData.Instance.DeviceLink.Append(new IngDeviceStatus() { NowType = SystemType.SecondFloor, IsLoad = false, DeviceName = "二层台" });
                if (this.cbDR.IsChecked.Value) GlobalData.Instance.DeviceLink.Append(new IngDeviceStatus() { NowType = SystemType.DrillFloor, IsLoad = false, DeviceName = "钻台面" });
                if (this.cbSIR.IsChecked.Value) GlobalData.Instance.DeviceLink.Append(new IngDeviceStatus() { NowType = SystemType.SIR, IsLoad = false, DeviceName = "铁钻工" });
                MessageBox.Show("配置成功");
            }
            catch (Exception ex)
            {
         
            }
        }
    }
}
