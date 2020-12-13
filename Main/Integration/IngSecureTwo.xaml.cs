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
                #region 二层台锁
                // 机械手手指在井口打开使能与吊卡
                this.smFingerLockElevator.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbFingerLockElevator.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 机械手回收与吊卡互锁
                this.smHandLockElevator.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbHandLockElevator.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 机械手向井口移动锁大钩
                this.smHandLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbHandLockHook.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                #endregion

                #region 钻台面锁
                // 机械手手指在井口打开使能与吊卡
                this.smDRFingerLockElevator.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbDRFingerLockElevator.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["579b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 机械手向井口移动锁大钩
                this.smDRHandLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbDRHandLockHook.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 铁钻工与钻台面互锁
                this.smIronLockDR.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbIronLockDR.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 防喷盒与钻台面互锁
                this.smPreventBoxLockDR.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbPreventBoxLockDR.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 行车左移
                this.smDRCarLock.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbDRCarLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["580b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                #endregion

                #region 大钩与铁钻工
                // 大钩与铁钻工互锁
                this.smHookLockIron.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbHookLockIron.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 钻台面与铁钻工互锁
                this.smDRLockIron.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbDRLockIron.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 防喷盒与铁钻工互锁
                this.smPreventBoxLockIron.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbPreventBoxLockIron.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 铁钻工上卸扣锁定
                this.smIronInOutBtnLock.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbIronInOutBtnLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["581b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });

                #endregion

                #region 防喷盒
                // 大钩与防喷盒互锁
                this.smHookLockPreventBox.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbHookLockPreventBox.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 钻台面与防喷盒互锁
                this.smDRLockPreventBox.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbDRLockPreventBox.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 铁钻工与防喷盒互锁
                this.smIronLockPreventBox.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbIronLockPreventBox.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                #endregion
                // 禁止猫道推杆上钻台面
                this.smCatToDRLock.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbCatToDRLock.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["582b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        #region 二层台锁
        /// <summary>
        /// 机械手手指在井口打开使能与吊卡
        /// </summary>
        private void CBFingerLockElevator_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbFingerLockElevator.IsChecked)
            {
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

        #region 钻台面锁
        /// <summary>
        /// 钻台面手指使能与吊卡互锁
        /// </summary>
        private void CBDRFingerLockElevator_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbDRFingerLockElevator.IsChecked)
            {
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
                MessageBoxResult result = MessageBox.Show("确认解除钻台面向井口移动锁大钩？", "提示信息", MessageBoxButton.YesNo);
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
        /// 铁钻工锁钻台面
        /// </summary>
        private void CBIronLockDR_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbIronLockDR.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除铁钻工与钻台面互锁？", "提示信息", MessageBoxButton.YesNo);
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
        /// 防喷盒与钻台面
        /// </summary>
        private void CBPreventBoxLockDR_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbPreventBoxLockDR.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除防喷盒与钻台面互锁？", "提示信息", MessageBoxButton.YesNo);
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
                MessageBoxResult result = MessageBox.Show("确认解除钻台面机械手左移？", "提示信息", MessageBoxButton.YesNo);
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
        /// 钻台面与铁钻工互锁
        /// </summary>
        private void CBDRLockIron_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbDRLockIron.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除钻台面与铁钻工互锁？", "提示信息", MessageBoxButton.YesNo);
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
        /// 钻台面与防喷盒互锁
        /// </summary>
        private void CBDRLockPreventBox_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbDRLockPreventBox.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除钻台面与防喷盒互锁？", "提示信息", MessageBoxButton.YesNo);
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
        /// 禁止猫道上钻台面
        /// </summary>
        private void CBCatToDRLock_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbCatToDRLock.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除猫道推杆上钻台面锁定？", "提示信息", MessageBoxButton.YesNo);
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
    }
}
