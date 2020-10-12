using COM.Common;
using ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
                // 6.30新增
                this.oobLink.SetBinding(OnOffButton.OnOffButtonCheckProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
                this.tbLink.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay, Converter = new LinkOpenOrCloseConverter() });
                MultiBinding LinkErrorMultiBind = new MultiBinding();
                LinkErrorMultiBind.Converter = new LinkErrorCoverter();
                LinkErrorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drLinkError"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b1"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b2"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.NotifyOnSourceUpdated = true;
                this.LinkError.SetBinding(TextBlock.TextProperty, LinkErrorMultiBind);
                // 操作模式-描述
                MultiBinding IngOprMultiBind = new MultiBinding();
                IngOprMultiBind.Converter = new IngOprCoverter();
                IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                IngOprMultiBind.NotifyOnSourceUpdated = true;
                this.operateMode.SetBinding(BasedSwitchButton.ContentDownProperty, IngOprMultiBind);
                // 操作模式-选择
                MultiBinding IngOprCheckMultiBind = new MultiBinding();
                IngOprCheckMultiBind.Converter = new IngOprCheckCoverter();
                IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                IngOprCheckMultiBind.NotifyOnSourceUpdated = true;
                this.operateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, IngOprCheckMultiBind);
                // 工作模式-描述
                MultiBinding IngWorkMultiBind = new MultiBinding();
                IngOprMultiBind.Converter = new IngWorkCoverter();
                IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
                IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                IngOprMultiBind.NotifyOnSourceUpdated = true;
                this.workMode.SetBinding(BasedSwitchButton.ContentDownProperty, IngOprMultiBind);
                // 工作模式-选择
                MultiBinding IngWorkCheckMultiBind = new MultiBinding();
                IngOprCheckMultiBind.Converter = new IngWorkCheckCoverter();
                IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                IngOprCheckMultiBind.NotifyOnSourceUpdated = true;
                this.workMode.SetBinding(BasedSwitchButton.IsCheckedProperty, IngOprCheckMultiBind);
                // 管柱选择
                IngDrillPipeTypeConverter ingDrillPipeTypeConverter = new IngDrillPipeTypeConverter();
                MultiBinding ingDrillPipeTypeMultiBind = new MultiBinding();
                ingDrillPipeTypeMultiBind.Converter = ingDrillPipeTypeConverter;
                ingDrillPipeTypeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drillPipeType"], Mode = BindingMode.OneWay });
                ingDrillPipeTypeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drdrillPipeType"], Mode = BindingMode.OneWay });
                ingDrillPipeTypeMultiBind.NotifyOnSourceUpdated = true;
                this.tubeType.SetBinding(TextBlock.TextProperty, ingDrillPipeTypeMultiBind);
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
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
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 联动打开关闭
        /// </summary>
        /// <param name="isChecked"></param>
        private void OnOffButton_CBCheckedEvent(bool isChecked)
        {
            if (isChecked)
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 0, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_OpState(object sender, EventArgs e)
        {
            byte[] sfbyteToSend;// 二层台
            byte[] drbyteToSend;// 钻台面
            byte[] sirbyteToSend;// 铁钻工
            if (this.operateMode.IsChecked)
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 5 });
                drbyteToSend = new byte[10] { 1, 32, 3, 31, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 4 });
                drbyteToSend = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(sfbyteToSend);
            Thread.Sleep(50);
            GlobalData.Instance.da.SendBytes(drbyteToSend);
            Thread.Sleep(50);
            GlobalData.Instance.da.SendBytes(sirbyteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_WorkModel(object sender, EventArgs e)
        {
            byte[] sfbyteToSend;// 二层台
            byte[] drbyteToSend;// 钻台面
            byte[] sirbyteToSend;// 铁钻工
            if (workMode.IsChecked)
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
                drbyteToSend = new byte[10] { 1, 32, 4, 41, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
                drbyteToSend = new byte[10] { 1, 32, 4, 40, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(sfbyteToSend);
            Thread.Sleep(50);
            GlobalData.Instance.da.SendBytes(drbyteToSend);
            Thread.Sleep(50);
            GlobalData.Instance.da.SendBytes(sirbyteToSend);
        }
        /// <summary>
        /// 管柱选择协议
        /// </summary>
        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, (byte)tag });
            GlobalData.Instance.da.SendBytes(byteToSend);
            Thread.Sleep(50);
            byteToSend = GlobalData.Instance.SendToDR(new List<byte>() { 3, (byte)tag });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
