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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.Integration
{
    /// <summary>
    /// Ing_SF_Self.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_SF_Self : UserControl
    {
        private static Ing_SF_Self _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_SF_Self Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_SF_Self();
                        }
                    }
                }
                return _instance;
            }
        }
        public Ing_SF_Self()
        {
            InitializeComponent();
            SFVariableBinding();
        }

        #region 二层台
        /// <summary>
        /// 二层台变量
        /// </summary>
        private void SFVariableBinding()
        {
            try
            {
                this.sfoperateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.sfoperateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
                this.sfworkMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
                this.sfworkMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelIsCheckConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 一键回零
        /// </summary>
        private void btn_SFAllMotorRetZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 点击使能
        /// </summary>
        private void btn_SFMotorEnable(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SFOpState(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.sfoperateMode.IsChecked)
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
        private void btn_SFWorkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (sfworkMode.IsChecked)
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion
    }
}
