using COM.Common;
using ControlLibrary;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.DrillFloor
{
    /// <summary>
    /// DRSecureSetting.xaml 的交互逻辑
    /// </summary>
    public partial class DRSecureSetting : UserControl
    {
        private static DRSecureSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static DRSecureSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DRSecureSetting();
                        }
                    }
                }
                return _instance;
            }
        }
        public DRSecureSetting()
        {
            InitializeComponent();
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
                this.cbBigHookInterLockingOfPulling.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["370b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.cbBigHookInterLockingOfRobot.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["370b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.cbManipulatorTopDriveInterlock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["370b5"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.cbMotorSaveLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["370b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.BigHookInterLockingAngleOfRobotShowValue.SetBinding(TextBox.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drHookLockAngle"], Mode = BindingMode.OneWay });
                this.TopDriveLockShowValue.SetBinding(TextBox.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drTopDriveAngle"], Mode = BindingMode.OneWay });

                this.HookSettingStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drHookSetStatus"], Mode = BindingMode.OneWay, Converter = new HookSetStatusConverter() });
                this.HookNowValue.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["156To159BigHookEncoderRealTimeValue"], Mode = BindingMode.OneWay });

                this.cbPliersForbid.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["441b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.cbForbidToWell.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["441b1"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.cbForbidRetract.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["441b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.cbCarLinkLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["336b5"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 机械手吊卡互锁
        /// </summary>
        private void CBBigHookInterLockingOfPulling_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbBigHookInterLockingOfPulling.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除机械手吊卡互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 2, 1 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 2, 2 });
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 机械手大钩互锁
        /// </summary>
        private void CBBigHookInterLockingOfRobot_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbBigHookInterLockingOfRobot.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除机械手大钩互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 5, 1 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 5, 2 });
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 机械手顶驱互锁
        /// </summary>
        private void CBManipulatorTopDriverInterlock_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbManipulatorTopDriveInterlock.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除机械手顶驱互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 6, 1 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 6, 2 });
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 伺服电机安全互锁
        /// </summary>
        private void CBMotorSaveLock_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbMotorSaveLock.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除伺服电机安全互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 3, 1 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 3, 2 });
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 获取键盘
        /// </summary>
        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }
        private Regex regexParameterConfigurationConfirm = new Regex(@"^[0-9]+$");
        /// <summary>
        /// 大钩互锁角度值
        /// </summary>
        private void BigHookLockSetting(object sender, MouseButtonEventArgs e)
        {
            byte bTemp;
            if ((regexParameterConfigurationConfirm.Match(this.BigHookInterLockingAngleOfRobotSettingValue.Text)).Success)
            {
                try
                {
                    bTemp = Convert.ToByte(this.BigHookInterLockingAngleOfRobotSettingValue.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("参数数值范围异常:" + ex.Message);
                    this.BigHookInterLockingAngleOfRobotSettingValue.Text = "0";
                    return;
                }
            }
            else
            {
                MessageBox.Show("参数为非数字");
                this.BigHookInterLockingAngleOfRobotSettingValue.Text = "0";
                return;
            }

            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 12, bTemp });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 顶驱角度设置
        /// </summary>
        private void BigTopDriveSetting(object sender, MouseButtonEventArgs e)
        {
            byte bTemp;
            if ((regexParameterConfigurationConfirm.Match(this.TopDriveLockSettingValue.Text)).Success)
            {
                try
                {
                    bTemp = Convert.ToByte(this.TopDriveLockSettingValue.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("参数数值范围异常:" + ex.Message);
                    this.TopDriveLockSettingValue.Text = "0";
                    return;
                }
            }
            else
            {
                MessageBox.Show("参数为非数字");
                this.TopDriveLockSettingValue.Text = "0";
                return;
            }

            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 6, bTemp });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 大钩标定值设定
        /// </summary>
        private void HookValueSetting(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 21, 13});
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钳头禁止-疑问第四位传什么
        /// </summary>
        private void CBPliersForbid_Clicked(object sender, EventArgs e)
        {
            if (this.cbPliersForbid.IsChecked)
            {
                byte[] byteToSend = new byte[10] { 2, 32, 31, 0, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                byte[] byteToSend = new byte[10] { 2, 32, 31, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 禁止向井口移动-疑问第四位传什么
        /// </summary>
        private void CBForbidToWell_Clicked(object sender, EventArgs e)
        {
            if (this.cbForbidToWell.IsChecked)
            {
                byte[] byteToSend = new byte[10] { 2, 32, 32, 0, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                byte[] byteToSend = new byte[10] { 2, 32, 32, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 禁止机械手缩回-疑问第四位传什么
        /// </summary>
        private void CBForbidRetract_Clicked(object sender, EventArgs e)
        {
            if (this.cbForbidRetract.IsChecked)
            {
                byte[] byteToSend = new byte[10] { 2, 32, 33, 0, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                byte[] byteToSend = new byte[10] { 2, 32, 33, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 行车联动互锁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBCarLinkLock_Clicked(object sender, EventArgs e)
        {
            if (this.cbCarLinkLock.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认屏蔽猫道行车联动互锁", "提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byte[] byteToSend = new byte[10] { 80, 33, 21, 5, 1, 0, 0, 0, 0, 0 };
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
            else
            {
                byte[] byteToSend = new byte[10] { 80, 33, 21, 5, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
