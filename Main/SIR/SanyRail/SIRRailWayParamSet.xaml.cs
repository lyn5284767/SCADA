using COM.Common;
using ControlLibrary.InputControl;
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

namespace Main.SIR.SanyRail
{
    /// <summary>
    /// SIRRailWayParamSet.xaml 的交互逻辑
    /// </summary>
    public partial class SIRRailWayParamSet : UserControl
    {
        private static SIRRailWayParamSet _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRRailWayParamSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRRailWayParamSet();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRRailWayParamSet()
        {
            InitializeComponent();
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
                #region PWM参数
                this.twt1.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_MainTongsMotorInBtnElecOutput"], Mode = BindingMode.OneWay});
                this.twt1.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_MianTongsInOutBtnElec"], Mode = BindingMode.OneWay });
                this.twt2.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_MainTongsMotorInBtnElecOutput"], Mode = BindingMode.OneWay });
                this.twt2.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_MianTongsMoveElec"], Mode = BindingMode.OneWay});
                this.twt3.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsCloseElecOutput"], Mode = BindingMode.OneWay});
                this.twt3.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsCloseElec"], Mode = BindingMode.OneWay });
                this.twt4.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsOpenElecOutput"], Mode = BindingMode.OneWay });
                this.twt4.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsCloseElec"], Mode = BindingMode.OneWay });
                this.twt5.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_TongsForwardElecOutput"], Mode = BindingMode.OneWay });
                this.twt5.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_TongsForwardElec"], Mode = BindingMode.OneWay });
                this.twt6.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_TongsBackElecOutput"], Mode = BindingMode.OneWay });
                this.twt6.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_TongsBackElec"], Mode = BindingMode.OneWay });

                #endregion

                #region 上卸扣参数
                this.twt7.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_CycleSet"], Mode = BindingMode.OneWay});
                this.twt8.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_PressSet"], Mode = BindingMode.OneWay });
                this.twt9.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_InOutBtnTorque"], Mode = BindingMode.OneWay });

                #endregion
                #region 丝扣油参数
                this.twt10.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_ClearTimeSet"], Mode = BindingMode.OneWay });
                this.twt11.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_SprayTimeSet"], Mode = BindingMode.OneWay });

                #endregion
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 确定配置
        /// </summary>
        private void Button_Setting(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.SetParam[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SetParam;
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
