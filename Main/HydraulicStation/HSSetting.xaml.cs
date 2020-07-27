using COM.Common;
using ControlLibrary;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.HydraulicStation
{
    /// <summary>
    /// HSSetting.xaml 的交互逻辑
    /// </summary>
    public partial class HSSetting : UserControl
    {
        private static HSSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static HSSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new HSSetting();
                        }
                    }
                }
                return _instance;
            }
        }

        public HSSetting()
        {
            InitializeComponent();
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            { 
                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay});
                this.MainPumpOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b3"], Mode = BindingMode.OneWay });
                this.MainPumpTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b5"], Mode = BindingMode.OneWay });
                this.constantVoltagePump.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b7"], Mode = BindingMode.OneWay });
                this.dissipateHeat.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b1"], Mode = BindingMode.OneWay });
                this.hot.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b3"], Mode = BindingMode.OneWay });

                this.cbHotHand.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b0"], Mode = BindingMode.OneWay });
                this.cbHotAuto.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b1"], Mode = BindingMode.OneWay });
                this.cbDisHotHand.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b2"], Mode = BindingMode.OneWay });
                this.cbDisHotAuto.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b3"], Mode = BindingMode.OneWay });
                this.cbFanHand.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b6"], Mode = BindingMode.OneWay });
                this.cbFanAuto.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b7"], Mode = BindingMode.OneWay });

                //this.Iron.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b1"], Mode = BindingMode.OneWay });
                //this.Tongs.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b3"], Mode = BindingMode.OneWay });
                //this.DF.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b3"], Mode = BindingMode.OneWay });
                //this.BufferArm.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b7"], Mode = BindingMode.OneWay });

                this.btnLeftCatHeadRetract.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b5"], Mode = BindingMode.OneWay,Converter = new BtnColorCoverter() });
                this.btnLeftCatHeadClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnLeftCatHeadReach.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
               
                this.btnRightCatHeadRetract.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRightCatHeadClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRightCatHeadReach.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
               
                this.btnIron.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnIronCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnIronCloseMultiBind = new MultiBinding();
                btnIronCloseMultiBind.Converter = btnIronCloseMultiConverter;
                btnIronCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["768b1"], Mode = BindingMode.OneWay });
                btnIronCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["768b3"], Mode = BindingMode.OneWay });
                btnIronCloseMultiBind.NotifyOnSourceUpdated = true;
                this.btnIronClose.SetBinding(Button.BackgroundProperty, btnIronCloseMultiBind);
                this.btnRightCatHeadReach.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                this.btnDF.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnDFCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnDFCloseMultiBind = new MultiBinding();
                btnDFCloseMultiBind.Converter = btnDFCloseMultiConverter;
                btnDFCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b3"], Mode = BindingMode.OneWay });
                btnDFCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b5"], Mode = BindingMode.OneWay });
                btnDFCloseMultiBind.NotifyOnSourceUpdated = true;
                this.btnDFClose.SetBinding(Button.BackgroundProperty, btnDFCloseMultiBind);
                this.btnSpaceThree.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                this.btnWellBuffer.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnWellBufferCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnWellBufferCloseMultiBind = new MultiBinding();
                btnWellBufferCloseMultiBind.Converter = btnWellBufferCloseMultiConverter;
                btnWellBufferCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b7"], Mode = BindingMode.OneWay });
                btnWellBufferCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["770b1"], Mode = BindingMode.OneWay });
                btnWellBufferCloseMultiBind.NotifyOnSourceUpdated = true;
                this.btnWellBufferClose.SetBinding(Button.BackgroundProperty, btnWellBufferCloseMultiBind);
                this.btnSpaceFour.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
            }
            catch (Exception ex)
            {
                Log4Net.AddLog(ex.StackTrace, InfoLevel.ERROR);
            }
        }
        //司钻/本地控制
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            byteToSend = new byte[10] { 0, 19, 1, 9, 0, 0, 0, 0, 0, 0 };// 疑问，所有切换都是这个协议？
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 主泵1启动/停止
        /// </summary>
        private void btn_MainPumpOne(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpOne.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 主泵2启动/停止
        /// </summary>
        private void btn_MainPumpTwo(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpTwo.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 3, 0, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 4, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 恒压泵启动/停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_constantVoltagePump(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.constantVoltagePump.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 5, 0, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 6, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 散热泵启动/停止
        /// </summary>
        private void btn_dissipateHeat(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.dissipateHeat.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 7, 0, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 8, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 加热器启动/停止
        /// </summary>
        private void btn_hot(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.hot.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 9, 0, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 10, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 加热器手动设置
        /// </summary>
        private void cbHotHand_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 1, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 加热器自动设置
        /// </summary>
        private void cbHotAuto_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 1, 2, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 散热器手动设置
        /// </summary>
        private void cbDisHotHand_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 1, 3, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 散热器自动设置
        /// </summary>
        private void cbDisHotAuto_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 1, 4, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 风扇手动设置
        /// </summary>
        private void cbFanHand_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 1, 7, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 风扇自动设置
        /// </summary>
        private void cbFanuAuto_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 1, 8, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 系统复位
        /// </summary>
        private void BtnSysTurnZero_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 1, 5, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 报警消除
        /// </summary>
        private void BtnAlarmClear_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 1, 6, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 已切换到主泵1运行
        /// </summary>
        private void BtnTurnMainOne_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认已切换到主泵1运行？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 2, 2, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 已切换到主泵2运行
        /// </summary>
        private void BtnTurnMainTwo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认已切换到主泵2运行？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 2, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 主电机1加注黄油
        /// </summary>
        private void BtnMonitorOneGetOil_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认主电机1已加注黄油？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 2, 3, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 主电机2加注黄油
        /// </summary>
        private void BtnMonitorTwoGetOil_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认主电机2已加注黄油？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 2, 4, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 滤芯已更换
        /// </summary>
        private void BtnFilterReplace_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认滤芯已更换？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 2, 5, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 液压油已更换
        /// </summary>
        private void BtnOilReplace_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认液压油已更换？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 2, 6, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 漏油确认
        /// </summary>
        private void BtnOilLeakage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认无漏油？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                byte[] byteToSend = new byte[10] { 0, 19, 2, 7, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 左猫头伸
        /// </summary>
        private void BtnLeftCatReach_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 3, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左猫头缩
        /// </summary>
        private void BtnLeftCatRetract_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 4, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左猫头关
        /// </summary>
        private void BtnLeftCatClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 5, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头伸
        /// </summary>
        private void BtnRightCatReach_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 6, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头缩
        /// </summary>
        private void BtnRightCatRetract_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 7, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头关
        /// </summary>
        private void BtnRightCatClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 8, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Iron(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btn_Tongs(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btn_DF(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 9, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btn_BufferArm(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 10, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void BtnIron_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void BtnIronClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 3, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void BtnTong_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void BtnDF_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 10, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void BtnDFClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 12, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void BtnSpaceThree_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 11, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void BtnWellBuffer_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 13, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void BtnWellBufferCloseClick(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 15, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void BtnSpaceFour_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 14, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
