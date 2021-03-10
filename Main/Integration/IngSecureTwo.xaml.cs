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
    /// IngSecureTwo.xaml 的交互逻辑
    /// </summary>
    public partial class IngSecureTwo : UserControl
    {
        private static IngSecureTwo _instance = null;
        private static readonly object syncRoot = new object();

        public static IngSecureTwo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngSecureTwo();
                        }
                    }
                }
                return _instance;
            }
        }
        public IngSecureTwo()
        {
            InitializeComponent();
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
                #region 铁架工锁
                // 机械手手指在井口打开使能与吊卡
                this.smFingerLockElevator.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b1"], Mode = BindingMode.OneWay, Converter = new BoolTagLockConverter() });
                this.cbFingerLockElevator.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 机械手回收与吊卡互锁
                this.smHandLockElevator.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b3"], Mode = BindingMode.OneWay, Converter = new BoolTagLockConverter() });
                this.cbHandLockElevator.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 机械手向井口移动锁大钩
                this.smHandLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b5"], Mode = BindingMode.OneWay, Converter = new BoolTagLockConverter() });
                this.cbHandLockHook.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                #endregion

                #region 扶杆臂锁
                // 机械手手指在井口打开使能与吊卡
                this.smDRFingerLockElevator.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b7"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbDRFingerLockElevator.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 机械手向井口移动锁大钩
                this.smDRHandLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b1"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbDRHandLockHook.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 铁钻工与扶杆臂互锁
                this.smIronLockDR.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b3"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbIronLockDR.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 防喷盒与扶杆臂互锁
                this.smPreventBoxLockDR.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b5"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbPreventBoxLockDR.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 行车左移
                this.smDRCarLock.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b7"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbDRCarLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                #endregion

                #region 大钩与铁钻工
                // 大钩与铁钻工互锁
                this.smHookLockIron.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b1"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbHookLockIron.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 扶杆臂与铁钻工互锁
                this.smDRLockIron.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b3"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbDRLockIron.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 防喷盒与铁钻工互锁
                this.smPreventBoxLockIron.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b5"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbPreventBoxLockIron.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 铁钻工上卸扣锁定
                this.smIronInOutBtnLock.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b7"], Mode = BindingMode.OneWay, Converter = new BoolTagLockConverter() });
                this.cbIronInOutBtnLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });

                #endregion

                #region 防喷盒
                // 大钩与防喷盒互锁
                this.smHookLockPreventBox.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b1"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbHookLockPreventBox.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 扶杆臂与防喷盒互锁
                this.smDRLockPreventBox.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b3"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbDRLockPreventBox.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 铁钻工与防喷盒互锁
                this.smIronLockPreventBox.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b5"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbIronLockPreventBox.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                #endregion
                // 禁止猫道推杆上扶杆臂
                this.smCatToDRLock.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b7"], Mode = BindingMode.OneWay, Converter = new BoolTagLockConverter() });
                this.cbCatToDRLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });

                // 缓冲臂与大钩互锁
                this.smHookLockHand.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["584b3"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagLockConverter() });
                this.cbHookLockHand.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["584b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        #region 铁架工锁
        /// <summary>
        /// 机械手手指在井口打开使能与吊卡
        /// </summary>
        private void CBFingerLockElevator_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbFingerLockElevator.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除机械手手指使能与吊卡互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 41, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 41, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 机械手回收与吊卡互锁
        /// </summary>
        private void CBHandLockElevator_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbHandLockElevator.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除机械手回收与吊卡互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 42, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 42, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 机械手向井口移动锁大钩
        /// </summary>
        private void CBHandLockHook_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbHandLockHook.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除机械手向井口移动锁大钩？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 43, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 43, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        #region 扶杆臂锁
        /// <summary>
        /// 扶杆臂手指使能与吊卡互锁
        /// </summary>
        private void CBDRFingerLockElevator_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbDRFingerLockElevator.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除机械手指使能锁吊卡？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 51, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 51, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 弃用
        /// </summary>
        private void CBDRHandLockElevator_Clicked(object sender, EventArgs e)
        {
            //byte[] byteToSend;

            //if (this.cbDRHandLockElevator.IsChecked)
            //{
            //    MessageBoxResult result = MessageBox.Show("确认解除机械手缩回使能锁大钩？", "提示信息", MessageBoxButton.YesNo);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        byteToSend = new byte[] { 16, 1, 26, 52, 1, 0, 0, 0, 0, 0 };
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    byteToSend = new byte[] { 16, 1, 26, 52, 2, 0, 0, 0, 0, 0 };
            //}

            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 机械手禁止向井口移动锁大钩
        /// </summary>
        private void CBDRHandLockHook_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbDRHandLockHook.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除扶杆臂向井口移动锁大钩？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 53, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 53, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工锁扶杆臂
        /// </summary>
        private void CBIronLockDR_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbIronLockDR.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除铁钻工与扶杆臂互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 54, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 54, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 防喷盒与扶杆臂
        /// </summary>
        private void CBPreventBoxLockDR_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbPreventBoxLockDR.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除防喷盒与扶杆臂互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 55, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 55, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 禁止行车左移
        /// </summary>
        private void CBDRCarLock_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbDRCarLock.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除扶杆臂机械手左移？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 56, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 56, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        #region 铁钻工锁
        /// <summary>
        /// 大钩与铁钻工互锁
        /// </summary>
        private void CBHookLockIron_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbHookLockIron.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除大钩与铁钻工互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 61, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 61, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 扶杆臂与铁钻工互锁
        /// </summary>
        private void CBDRLockIron_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbDRLockIron.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除扶杆臂与铁钻工互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 62, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 62, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 防喷盒与铁钻工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBPreventBoxLockIron_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbPreventBoxLockIron.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除防喷盒与铁钻工互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 63, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 63, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工上卸扣锁定
        /// </summary>
        private void CBIronInOutBtnLock_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbIronInOutBtnLock.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除铁钻工上卸扣锁定？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 64, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 64, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        #region 防喷盒
        /// <summary>
        /// 大钩与防喷盒互锁
        /// </summary>
        private void CBHookLockPreventBox_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbHookLockPreventBox.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除大钩与防喷盒互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 71, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 71, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 扶杆臂与防喷盒互锁
        /// </summary>
        private void CBDRLockPreventBox_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbDRLockPreventBox.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除扶杆臂与防喷盒互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 72, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 72, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工与防喷盒互锁
        /// </summary>
        private void CBIronLockPreventBox_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbIronLockPreventBox.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除铁钻工与防喷盒互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 73, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 73, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion
        /// <summary>
        /// 禁止猫道上扶杆臂
        /// </summary>
        private void CBCatToDRLock_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbCatToDRLock.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除猫道推杆上扶杆臂锁定？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 81, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 81, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IngLockList ingLock = new IngLockList();
            ingLock.ShowDialog();
        }
        /// <summary>
        /// 大钩低位禁止缓冲臂去井口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbHookLockHand_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbHookLockHand.IsChecked)
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备取消权限！", "提示信息", MessageBoxButton.OK);
                    return;
                }
                MessageBoxResult result = MessageBox.Show("确认解除大钩低位禁止缓冲臂去井口？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 74, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 74, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
