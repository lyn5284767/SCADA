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
        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.cbGapLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfGapLock"], Mode = BindingMode.OneWay });
                this.cbSafeDoorLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfSafeDoorLock"], Mode = BindingMode.OneWay});
                this.cbWellFendersLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWellFendersLock"], Mode = BindingMode.OneWay });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void BoxBigHookInterLockingOfRobot_Clicked(object sender, EventArgs e)
        {

        }

        private void ManipulatorTopDriverInterlock_Clicked(object sender, EventArgs e)
        {

        }

        private void ServoMotorOverloadLimit_Clicked(object sender, EventArgs e)
        {

        }

        private void InterlockOfManipulatorRopes_Clicked(object sender, EventArgs e)
        {

        }

        private void FingerBeamLockOpenConfirm_Clicked(object sender, EventArgs e)
        {

        }

        private void ElevatorCloseSignShield_Clicked(object sender, EventArgs e)
        {

        }

        private void ElevatorOpenLimitCancel_Clicked(object sender, EventArgs e)
        {

        }

        private void RobotInspectionMode_Clicked(object sender, EventArgs e)
        {

        }

        private void CheckBoxRoteTurnZero_UserControlClicked(object sender, EventArgs e)
        {

        }

        private void tb_GotFocus(object sender, MouseButtonEventArgs e)
        {

        }

        private void btn_SetBigHookInterLockAngle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_SetTopDriverInterlockAngle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_BigHookHeight_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_BigHookHeightCannel_Click(object sender, RoutedEventArgs e)
        {

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
                    byteToSend = GlobalData.Instance.SendByte(new List<byte> { 21, 6, 2 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 21, 6, 1 });
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
                    byteToSend = GlobalData.Instance.SendByte(new List<byte> { 21, 7, 2 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 21, 7, 1 });
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井口防碰互锁
        /// </summary>
        private void cbWellFendersLock_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbSafeDoorLock.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认关闭井口防碰互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SendByte(new List<byte> { 21, 8, 2 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 21, 8, 1 });
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
