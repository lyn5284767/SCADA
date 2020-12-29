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

namespace Main.WellRepair.SIR_Self
{
    /// <summary>
    /// WR_ParamSet.xaml 的交互逻辑
    /// </summary>
    public partial class WR_ParamSet : UserControl
    {
        private static WR_ParamSet _instance = null;
        private static readonly object syncRoot = new object();

        public static WR_ParamSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new WR_ParamSet();
                        }
                    }
                }
                return _instance;
            }
        }
        public WR_ParamSet()
        {
            InitializeComponent();
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
                this.twt8.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_PunchingTorqueSet"], Mode = BindingMode.OneWay });
                this.twt6.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_UpDownTongsPressSet"], Mode = BindingMode.OneWay });
                this.twt9.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_RotateBtnPressSet"], Mode = BindingMode.OneWay });               
                this.twt17.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_RotateBtnTimeSet"], Mode = BindingMode.OneWay });
                this.twt18.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_PunchingCloseTimes"], Mode = BindingMode.OneWay });
                this.twt25.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_PunchingOpenTimes"], Mode = BindingMode.OneWay });

                this.twt2.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_DFClockwiseValue"], Mode = BindingMode.OneWay });
                this.twt2.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_DFClockwiseCurValue"], Mode = BindingMode.OneWay });
                this.twt3.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_DFAntiClockwiseValue"], Mode = BindingMode.OneWay });
                this.twt3.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_DFAntiClockwiseCurValue"], Mode = BindingMode.OneWay });
                this.twt4.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_TongsReachValue"], Mode = BindingMode.OneWay });
                this.twt4.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_TongsReachCurValue"], Mode = BindingMode.OneWay });
                this.twt5.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_TongsRetractValue"], Mode = BindingMode.OneWay });
                this.twt5.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_TongsRetractCurValue"], Mode = BindingMode.OneWay });
                //this.twt6.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_DFClockwiseValue"], Mode = BindingMode.OneWay });
                //this.twt6.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_DFClockwiseCurValue"], Mode = BindingMode.OneWay });
                this.twt7.SetBinding(TwoTextWithInput.TextOneShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_RotateBtnCloseAndOpenValue"], Mode = BindingMode.OneWay });
                this.twt7.SetBinding(TwoTextWithInput.TextTwoShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_RotateBtnCloseAndOpenCurValue"], Mode = BindingMode.OneWay });


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 待机位设置
        /// </summary>
        private void Stay_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 16, 22, 0, 0, 0, 0, 0, 0, 0 };

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井口位设置
        /// </summary>
        private void Well_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 16, 23, 0, 0, 0, 0, 0, 0, 0 };

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 鼠洞位设置
        /// </summary>
        private void Mouse_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 16, 24, 0, 0, 0, 0, 0, 0, 0 };

            GlobalData.Instance.da.SendBytes(byteToSend);
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
