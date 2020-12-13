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
    /// IngSecureSetting.xaml 的交互逻辑
    /// </summary>
    public partial class IngSecureOne : UserControl
    {
        private static IngSecureOne _instance = null;
        private static readonly object syncRoot = new object();

        public static IngSecureOne Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngSecureOne();
                        }
                    }
                }
                return _instance;
            }
        }
        public IngSecureOne()
        {
            InitializeComponent();
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
                #region 卡瓦锁
                // 吊卡关门与卡瓦互锁
                this.smElevatorLockKava.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["576b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbElevatorLockKava.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["576b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 铁钻工与卡瓦互锁
                this.smIronLockKava.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["576b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbIronLockKava.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["576b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 大钩与卡瓦互锁
                this.smHookLockKava.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["576b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbHookLockKava.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["576b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 顶驱与卡瓦互锁
                this.smTopLockKava.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["583b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbTopLockKava.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["583b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                #endregion
                #region 吊卡锁
                // 机械手在井口与吊卡互锁
                this.smHandLockElevaltor.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["576b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbHandLockElevaltor.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["576b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 卡瓦与吊卡互锁
                this.smKavaLockElevator.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["577b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbKavaLockElevator.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["577b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 顶驱与吊卡互锁
                this.smTopLockElevator.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["583b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbTopLockElevator.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["583b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });

                #endregion
                // 二层台与顶驱互锁
                this.smSFLockTop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["577b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbSFLockTop.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["577b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 顶驱与吊卡互锁
                this.smKavaLockTop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["583b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbKavaLockTop.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["583b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 顶驱与吊卡互锁
                this.smElevatorLockTop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["583b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbElevatorLockTop.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["583b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                #region 大钩锁
                // 吊卡关门锁大钩
                this.smElevatorLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["577b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbElevatorLockHook.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["577b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 顶驱与大钩互锁
                this.smTopLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["577b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbTopLockHook.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["577b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 二层台机械手与大钩互锁
                this.smSFLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["578b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbSFLockHook.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["578b0"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 钻台面机械手与大钩互锁
                this.smDRLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["578b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbDRLockHook.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["578b2"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 铁钻工与大钩互锁
                this.smIronLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["578b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbIronLockHook.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["578b4"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });
                // 铁钻工与大钩互锁
                this.smPreventBoxLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["578b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cbPreventBoxLockHook.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["578b6"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });

                #endregion

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        #region 卡瓦锁
        /// <summary>
        /// 吊卡与卡瓦互锁888
        /// </summary>
        private void CBElevatorLockKava_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbElevatorLockKava.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除吊卡允许卡瓦打开？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 1, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 1, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工卡瓦互锁
        /// </summary>
        private void CBIronLockKava_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbIronLockKava.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除铁钻工允许卡瓦打开？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 2, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 2, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 大钩与卡瓦互锁
        /// </summary>
        private void CBHookLockKava_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbHookLockKava.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除大钩允许卡瓦关闭？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 3, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 3, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void CBTopLockKava_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbTopLockKava.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除顶驱卡扣允许卡瓦打开？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 4, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 4, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        #region 吊卡锁
        /// <summary>
        /// 机械手在井口与吊卡互锁
        /// </summary>
        private void CBHandLockElevaltor_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbHandLockElevaltor.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除抓手禁止吊卡打开？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 11, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 11, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 卡瓦与吊卡互锁
        /// </summary>
        private void CBKavaLockElevator_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbKavaLockElevator.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除卡瓦禁止吊卡打开？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 12, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 12, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 顶驱卡扣允许吊卡打开
        /// </summary>
        private void CBTopLockElevator_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbTopLockElevator.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除顶驱卡扣允许吊卡打开？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 13, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 13, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion
        #region 顶驱互锁
        /// <summary>
        /// 二层台与顶驱互锁
        /// </summary>
        private void CBSFLockTop_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbSFLockTop.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除二层台与顶驱互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 21, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 21, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 卡瓦与顶驱解扣输出
        /// </summary>
        private void CBKavaLockTop_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbKavaLockTop.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认卡瓦与顶驱解扣输出？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 22, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 22, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 吊卡与顶驱解扣输出
        /// </summary>
        private void CBElevatorLockTop_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbElevatorLockTop.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认吊卡与顶驱解扣输出？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 23, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 23, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion
        #region 大钩互锁
        /// <summary>
        /// 吊卡关门锁大钩
        /// </summary>
        private void CBElevatorLockHook_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbElevatorLockHook.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除吊卡关门锁大钩？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 31, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 31, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// </summary>
        /// 顶驱与大钩互锁
        /// </summary>
        private void CBTopLockHook_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbTopLockHook.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除顶驱与大钩互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 32, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 32, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// </summary>
        /// 二层台机械手与大钩互锁
        /// </summary>
        private void CBSFLockHook_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbSFLockHook.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除二层台机械手与大钩互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 33, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 33, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// </summary>
        /// 钻台面机械手与大钩互锁
        /// </summary>
        private void CBDRLockHook_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbDRLockHook.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除钻台面机械手与大钩互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 34, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 34, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// </summary>
        /// 铁钻工与大钩互锁
        /// </summary>
        private void CBIronLockHook_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbIronLockHook.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除铁钻工与大钩互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 35, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 35, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 防喷盒与大钩互锁
        /// </summary>
        private void CBPreventBoxLockHook_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (!this.cbPreventBoxLockHook.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认解除防喷盒与大钩互锁？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = new byte[] { 16, 1, 26, 36, 1, 0, 0, 0, 0, 0 };
                }
                else
                {
                    return;
                }
            }
            else
            {
                byteToSend = new byte[] { 16, 1, 26, 36, 2, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IngLockList ingLock = new IngLockList();
            ingLock.ShowDialog();
        }
        /// <summary>
        /// 一键解除互锁
        /// </summary>
        private void Unlock_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[] { 16, 1, 21, 31, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
