using COM.Common;
using Log;
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
    /// OilTwoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OilTwoWindow : Window
    {
        public OilTwoWindow()
        {
            InitializeComponent();
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
               
                //this.btnIron.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnIronCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnIronCloseMultiBind = new MultiBinding();
                btnIronCloseMultiBind.Converter = btnIronCloseMultiConverter;
                btnIronCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["768b1"], Mode = BindingMode.OneWay });
                btnIronCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["768b3"], Mode = BindingMode.OneWay });
                btnIronCloseMultiBind.NotifyOnSourceUpdated = true;
                //this.btnIronClose.SetBinding(Button.BackgroundProperty, btnIronCloseMultiBind);
                //this.btnTong.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                //this.btnDF.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnDFCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnDFCloseMultiBind = new MultiBinding();
                btnDFCloseMultiBind.Converter = btnDFCloseMultiConverter;
                btnDFCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b3"], Mode = BindingMode.OneWay });
                btnDFCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b5"], Mode = BindingMode.OneWay });
                btnDFCloseMultiBind.NotifyOnSourceUpdated = true;
                //this.btnDFClose.SetBinding(Button.BackgroundProperty, btnDFCloseMultiBind);
                //this.btnSpaceThree.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                //this.btnWellBuffer.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnWellBufferCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnWellBufferCloseMultiBind = new MultiBinding();
                btnWellBufferCloseMultiBind.Converter = btnWellBufferCloseMultiConverter;
                btnWellBufferCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b7"], Mode = BindingMode.OneWay });
                btnWellBufferCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["770b1"], Mode = BindingMode.OneWay });
                btnWellBufferCloseMultiBind.NotifyOnSourceUpdated = true;
                //this.btnWellBufferClose.SetBinding(Button.BackgroundProperty, btnWellBufferCloseMultiBind);
                //this.btnSpaceFour.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnIronOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnIronClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.tbIron.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IRPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.btnTongsOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnTongsClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.tbTongs.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["TongPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.btnDROpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnDRClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.tbDR.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["FPPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.btnArmOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnArmClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
              
            }
            catch (Exception ex)
            {
                Log4Net.AddLog(ex.StackTrace, InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 铁钻工油源打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIronOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 17, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工关闭
        /// </summary>
        private void BtnIronClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 18, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 大钳打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTongsOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 19, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 大钳关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTongsClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 20, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钻台面打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDROpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 27, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btnDRClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 28, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 缓冲臂打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnArmOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 31, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 缓冲臂关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnArmClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 32, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
