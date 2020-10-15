using COM.Common;
using ControlLibrary;
using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// IngHSSetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IngHSSetWindow : Window
    {
        bool bMainPumpOne = false;
        bool MainPumpOneCheck = false;
        bool bMainPumpTwo = false;
        bool MainPumpTwoCheck = false;
        System.Threading.Timer VariableReBinding;

        public IngHSSetWindow()
        {
            InitializeComponent();
            VariableBinding();
            VariableReBinding = new System.Threading.Timer(new TimerCallback(VariableTimer), null, Timeout.Infinite, 500);
            VariableReBinding.Change(0, 500);
        }

        private void VariableBinding()
        {
            try
            {
                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay });
                HyControlModelMuilCoverter hyControlModelMultiConverter = new HyControlModelMuilCoverter();
                MultiBinding hyControlModelMultiBind = new MultiBinding();
                hyControlModelMultiBind.Converter = hyControlModelMultiConverter;
                hyControlModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay });
                hyControlModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b6"], Mode = BindingMode.OneWay });
                hyControlModelMultiBind.NotifyOnSourceUpdated = true;
                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, hyControlModelMultiBind);
                HyControlModelTxtMuilCoverter hyControlModelTxtMultiConverter = new HyControlModelTxtMuilCoverter();
                MultiBinding hyControlModelTxtlMultiBind = new MultiBinding();
                hyControlModelTxtlMultiBind.Converter = hyControlModelTxtMultiConverter;
                hyControlModelTxtlMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay });
                hyControlModelTxtlMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b6"], Mode = BindingMode.OneWay });
                hyControlModelTxtlMultiBind.NotifyOnSourceUpdated = true;
                this.controlModel.SetBinding(BasedSwitchButton.ContentDownProperty, hyControlModelTxtlMultiBind);

                this.MainPumpOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b3"], Mode = BindingMode.OneWay });
                this.MainPumpTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b5"], Mode = BindingMode.OneWay });
                this.constantVoltagePump.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b7"], Mode = BindingMode.OneWay });
                this.dissipateHeat.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b1"], Mode = BindingMode.OneWay });
                this.hot.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b3"], Mode = BindingMode.OneWay });
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
            this.MainPumpOne.ContentDown = "切换中";
            this.MainPumpOneCheck = true;
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
            this.MainPumpTwo.ContentDown = "切换中";
            this.MainPumpTwoCheck = true;
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

        private void VariableTimer(object value)
        {
            if (this.bMainPumpOne != GlobalData.Instance.da["770b3"].Value.Boolean && this.MainPumpOneCheck)
            {
                this.MainPumpOne.Dispatcher.Invoke(new Action(() =>
                {
                    this.MainPumpOne.ContentDown = "1#主泵";
                }));
                MainPumpOneCheck = false;
            }
            bMainPumpOne = GlobalData.Instance.da["770b3"].Value.Boolean;
            if (this.bMainPumpTwo != GlobalData.Instance.da["770b5"].Value.Boolean && this.MainPumpTwoCheck)
            {
                this.MainPumpOne.Dispatcher.Invoke(new Action(() =>
                {
                    this.MainPumpTwo.ContentDown = "2#主泵";
                }));
                MainPumpTwoCheck = false;
            }
            bMainPumpTwo = GlobalData.Instance.da["770b5"].Value.Boolean;
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
