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

namespace Main.SIR
{
    /// <summary>
    /// SIRSecureSetting.xaml 的交互逻辑
    /// </summary>
    public partial class SIRSecureSetting : UserControl
    {
        private static SIRSecureSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRSecureSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRSecureSetting();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRSecureSetting()
        {
            InitializeComponent();
            VariableBinding();
        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.cbGapLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfGapLock"], Mode = BindingMode.OneWay, Converter = new SIRSelfLockIsCheckConverter() });
                this.cbSafeDoorLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfSafeDoorLock"], Mode = BindingMode.OneWay, Converter = new SIRSelfLockIsCheckConverter() });
                //this.cbWellFendersLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWellFendersLock"], Mode = BindingMode.OneWay, Converter = new SIRSelfLockIsCheckConverter() });
                this.tbWellStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWellFendersLock"], Mode = BindingMode.OneWay, Converter = new SIRSelfWellStatusConverter() });
                this.tbSIRStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLockStatusShow"], Mode = BindingMode.OneWay, Converter = new SIRSelfSIRStatusConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 缺口互锁
        /// </summary>
        private void cbGapLock_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbGapLock.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认关闭缺口互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 23, 17, 7, 2, 0, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 23, 17, 7, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 安全门互锁
        /// </summary>
        private void cbSafeDoorLock_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbSafeDoorLock.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认关闭安全门互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 23, 17, 8, 2, 0, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 23, 17, 8, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井口防碰互锁
        /// </summary>
        private void cbWellFendersLock_Clicked(object sender, EventArgs e)
        {
            //byte[] byteToSend;

            //if (this.cbWellFendersLock.IsChecked)
            //{
            //    if (GlobalData.Instance.systemRole == SystemRole.OperMan)
            //    {
            //        MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
            //        return;
            //    }
            //    MessageBoxResult result = MessageBox.Show("确认关闭井口防碰互锁？", "提示信息", MessageBoxButton.YesNo);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        byteToSend = new byte[] { 23, 17, 9, 2, 0, 0, 0, 0, 0, 0 };
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    byteToSend = new byte[] { 23, 17, 9, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
