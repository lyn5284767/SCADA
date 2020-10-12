using COM.Common;
using ControlLibrary;
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
    /// SFSetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IngSFSetWindow : Window
    {
        public IngSFSetWindow()
        {
            InitializeComponent();
            VariableBinding();
        }
        private void VariableBinding()
        {
            try
            {
                this.operateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.operateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
                this.workMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
                this.workMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelIsCheckConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 一键回零
        /// </summary>
        private void btn_AllMotorRetZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 点击使能
        /// </summary>
        private void btn_MotorEnable(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OpState(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.operateMode.IsChecked)
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 5 });
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 4 });
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_WorkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (workMode.IsChecked)
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
