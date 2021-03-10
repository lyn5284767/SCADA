using COM.Common;
using ControlLibrary;
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

namespace Main.SecondFloor
{
    /// <summary>
    /// SFSecureSetting.xaml 的交互逻辑
    /// </summary>
    public partial class SFSecureSetting : UserControl
    {
        private static SFSecureSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static SFSecureSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFSecureSetting();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFSecureSetting()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFSecureSetting_Loaded;
        }

        private void SFSecureSetting_Loaded(object sender, RoutedEventArgs e)
        {
            //VariableBinding();
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.opModel_SecuritySettingsPage.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.workModel_SecuritySettingsPage.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
                this.rotateAngle_SecuritySettingsPage.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });

                this.WellheadfingerOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["503b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.WellheadarmOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["503b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ApproachingWellheadOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["503b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                this.checkBoxBigHookInterLockingOfRobot.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b5"], Mode = BindingMode.OneWay, Converter = new InterLockingConverter() });
                this.checkBoxManipulatorTopDriveInterlock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b6"], Mode = BindingMode.OneWay, Converter = new InterLockingConverter() });
                this.checkBoxElevatorClosingSignalShielding.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b7"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.checkBoxFingerBeamLockOpenConfirmation.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["116E1E2E4B5b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.checkBoxServoMotorOverloadLimit.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["116E1E2E4B5b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.checkBoxInterlockingOfManipulatorRopes.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["116E1E2E4B5b3"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.checkBoxRobotInspectionMode.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["508b7"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.checkBoxRoteTurnZero.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["21b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                this.checkBoxElevatorOpenLimitCancel.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["504b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });

                this.BigHookInterLockingAngleOfRobotShowValue.SetBinding(TextBox.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["153RobotBigHookInterlockAngle"], Mode = BindingMode.OneWay });
                this.ManipulatorTopDriveInterlockAngleShowValue.SetBinding(TextBox.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["154RobotTopDriveInterlockAngle"], Mode = BindingMode.OneWay });
                this.BigHookHeightShowValue.SetBinding(TextBox.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["156To159BigHookEncoderRealTimeValue"], Mode = BindingMode.OneWay });
                this.BigHookHeightSetValue.SetBinding(TextBox.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["160To163BigHookEncoderCalibrationValue"], Mode = BindingMode.OneWay });

                // 6.15新增
                this.HookStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbHandLockMonkey.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["21b5"], Mode = BindingMode.OneWay});

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 获取键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }

        const byte bHeadFirst = 0x50;
        const byte bHeadTwo = 0x01;
        private Regex regexParameterConfigurationConfirm = new Regex(@"^[0-9]+$");
        /// <summary>
        /// 组协议
        /// </summary>
        /// <param name="list">协议字符列表</param>
        /// <returns></returns>
        private byte[] SendByte(List<byte> list)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = bHeadFirst; // 默认0位80
            byteToSend[1] = bHeadTwo;   // 默认1位1
            // 2位之后传进来
            for (int i = 0; i < list.Count; i++)
            {
                byteToSend[i + 2] = list[i];
            }
            return byteToSend;
        }

        /// <summary>
        /// 指梁锁打开确认解除/设置
        /// </summary>
        private void FingerBeamLockOpenConfirm_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (checkBoxFingerBeamLockOpenConfirmation.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除指梁锁打开确认？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[10] { 80, 1, 21, 1, 1, 0, 0, 0, 0, 0 }; 
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[10] { 80, 1, 21, 1, 2, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 伺服电机过载限制解除/设置
        /// </summary>
        private void ServoMotorOverloadLimit_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (checkBoxServoMotorOverloadLimit.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除伺服电机过载限制？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[10] { 80, 1, 21, 3, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[10] { 80, 1, 21, 3, 2, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 机械手与挡绳互锁解除
        /// </summary>
        private void InterlockOfManipulatorRopes_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (checkBoxInterlockingOfManipulatorRopes.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除机械手挡绳互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = SendByte(new List<byte> { 21, 4, 1 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = SendByte(new List<byte> { 21, 4, 2 });
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 机械手与大钩互锁解除/设置
        /// </summary>
        private void BoxBigHookInterLockingOfRobot_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (checkBoxBigHookInterLockingOfRobot.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }

                MessageBoxResult result = MessageBox.Show("确认解除机械手大钩互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 5, 1 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 5, 2 });
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 机械手与顶驱互锁解除/设置
        /// </summary>
        private void ManipulatorTopDriverInterlock_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (checkBoxManipulatorTopDriveInterlock.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除机械手顶驱互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 6, 1 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 6, 2 });
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 吊卡关门信号屏蔽/开启
        /// </summary>
        private void ElevatorCloseSignShield_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.checkBoxElevatorClosingSignalShielding.IsChecked)
            {
                byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 7, 2 });
            }
            else
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认屏蔽吊卡关门信号？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 7, 1 });
                }
                else
                {
                    return;
                }
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 启用/取消机械手检修模式
        /// </summary>
        private void RobotInspectionMode_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (checkBoxRobotInspectionMode.IsChecked)
            {
                byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 8, 2 });
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("确认进入机械手检修模式？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 8, 1 });
                }
                else
                {
                    return;
                }
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 机械手与顶驱互锁角度设置
        /// </summary>
        private void btn_SetTopDriverInterlockAngle_Click(object sender, RoutedEventArgs e)
        {
            byte bTemp;
            if ((regexParameterConfigurationConfirm.Match(this.ManipulatorTopDriveInterlockAngleSetValue.Text)).Success)
            {
                try
                {
                    bTemp = Convert.ToByte(this.ManipulatorTopDriveInterlockAngleSetValue.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("参数数值范围异常:" + ex.Message);
                    this.ManipulatorTopDriveInterlockAngleSetValue.Text = "0";
                    return;
                }
            }
            else
            {
                MessageBox.Show("参数为非数字");
                this.ManipulatorTopDriveInterlockAngleSetValue.Text = "0";
                return;
            }
            byte[] byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 12, bTemp });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 大钩安全高度标定
        /// </summary>

        private void btn_BigHookHeight_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21,14, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 机械手与大钩互锁角度设置
        /// </summary>
        private void btn_SetBigHookInterLockAngle_Click(object sender, RoutedEventArgs e)
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

            byte[] byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 11, bTemp });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 回转回零限制
        /// </summary>
        private void CheckBoxRoteTurnZero_UserControlClicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.checkBoxRoteTurnZero.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认取消回转回零限制？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = SendByte(new List<byte> { 21, 21, 1 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("不允许设置！");
            }
        }
        /// <summary>
        /// 吊卡打开限制解除
        /// </summary>
        private void ElevatorOpenLimitCancel_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.checkBoxElevatorOpenLimitCancel.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认取消吊卡打开限制解除？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 13,1 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21, 13, 2 });
                GlobalData.Instance.da.SendBytes(byteToSend);

            }
        }
        /// <summary>
        /// 取消大钩高度标定
        /// </summary>
        private void btn_BigHookHeightCannel_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 21,14, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 机械手与猴道互锁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbHandLockMonkey_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbHandLockMonkey.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除机械手与猴道互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[10] { 80, 1, 21, 22, 1, 0, 0, 0, 0, 0 };
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[10] { 80, 1, 21, 22, 2, 0, 0, 0, 0, 0 }; 
                GlobalData.Instance.da.SendBytes(byteToSend);

            }
        }
    }
}
