using COM.Common;
using ControlLibrary;
using HandyControl.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.SIR.SanyRail
{
    /// <summary>
    /// SIRRailWayMain.xaml 的交互逻辑
    /// </summary>
    public partial class SIRRailWayMain : UserControl
    {
        private static SIRRailWayMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRRailWayMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRRailWayMain();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer ReportTimer;
        System.Threading.Timer timerWarning;
        public SIRRailWayMain()
        {
            InitializeComponent();
            VariableBinding();
            ReportTimer = new System.Threading.Timer(new TimerCallback(ReportTimer_Elapse), null, 500, 2000);
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }
        private void VariableBinding()
        {
            try
            {
                #region 旋转开关
                this.oprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_OprModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayOperationModelConverter() });
                this.oprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_OprModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayIsCheckConverter() });
                this.workModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_WorkModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayWorkModelConverter() });
                this.workModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_WorkModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayIsCheckedConverter() });
                this.controlModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_ControlModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayControlModelConverter() });
                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_ControlModel"], Mode = BindingMode.OneWay, Converter = new SIRRailWayIsCheckConverter() });
                // 高低档切换
                MultiBinding highOrLowModelMultiBind = new MultiBinding();
                highOrLowModelMultiBind.Converter = new SIRRailWayHighOrLowCoverter();
                highOrLowModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["811b0"], Mode = BindingMode.OneWay });
                highOrLowModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["811b1"], Mode = BindingMode.OneWay });
                highOrLowModelMultiBind.NotifyOnSourceUpdated = true;
                this.highOrLowModel.SetBinding(BasedSwitchButton.ContentDownProperty, highOrLowModelMultiBind);
                MultiBinding highOrLowModelCheckMultiBind = new MultiBinding();
                highOrLowModelCheckMultiBind.Converter = new SIRRailWayHighOrLowCheckCoverter();
                highOrLowModelCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["811b0"], Mode = BindingMode.OneWay });
                highOrLowModelCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["811b1"], Mode = BindingMode.OneWay });
                highOrLowModelCheckMultiBind.NotifyOnSourceUpdated = true;
                this.highOrLowModel.SetBinding(BasedSwitchButton.IsCheckedProperty, highOrLowModelCheckMultiBind);
                #endregion

                #region 类型选择
                this.tbDrillType.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_DrillType"], Mode = BindingMode.OneWay, Converter = new SIRRailWayDrillTypeConverter() });
                this.tbDrillRote.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Direction"], Mode = BindingMode.OneWay, Converter = new SIRRailWayDirectionConverter() });
                this.tbLocation.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Location"], Mode = BindingMode.OneWay, Converter = new SIRRailWayLocationConverter() });
                this.tbSpraySet.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Spray"], Mode = BindingMode.OneWay, Converter = new SIRRailWaySprayConverter() });

                #endregion

                #region 主参数
                this.tbSysPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_OilPress"], Mode = BindingMode.OneWay });
                this.tbInBtnPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_InBtnPress"], Mode = BindingMode.OneWay });
                this.tbHighPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_High"], Mode = BindingMode.OneWay });
                this.tbLowPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_Low"], Mode = BindingMode.OneWay });
                this.tbTorquePress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsPress"], Mode = BindingMode.OneWay });
                //this.tbInWorkTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsPress"], Mode = BindingMode.OneWay });
                this.tbUpDownMove.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_TongsHeight"], Mode = BindingMode.OneWay });
                #endregion

                // 一键上扣
                MultiBinding sbInbuttonMultiBind = new MultiBinding();
                sbInbuttonMultiBind.Converter = new SIRRailWayAutoModeStepCoverter();
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_OprModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_WorkModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Step"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.ConverterParameter = "inButton";
                sbInbuttonMultiBind.NotifyOnSourceUpdated = true;
                this.sbInButton.SetBinding(StepBar.StepIndexProperty, sbInbuttonMultiBind);
                // 一键卸扣
                MultiBinding sbOutButtonMultiBind = new MultiBinding();
                sbOutButtonMultiBind.Converter = new SIRRailWayAutoModeStepCoverter();
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_OprModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_WorkModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIR_RailWay_Step"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.ConverterParameter = "outButton";
                sbOutButtonMultiBind.NotifyOnSourceUpdated = true;
                this.sbOutButton.SetBinding(StepBar.StepIndexProperty, sbOutButtonMultiBind);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 图标定时器
        /// </summary>
        /// <param name="value"></param>
        private void ReportTimer_Elapse(object value)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    double drillTore = GlobalData.Instance.da["SIE_RailWay_OilPress"].Value.Int16 / 1.0;
                    double cosingTorque = GlobalData.Instance.da["SIE_RailWay_BackTongsPress"].Value.Int16 / 1.0;
                    this.PressChart.AddPoints(drillTore);
                    this.TorqueChart.AddPoints(cosingTorque);
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private int iTimeCnt = 0;//用来为时钟计数的变量
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    //this.Warnning();
                    //this.Communcation();
                    if (GlobalData.Instance.da["SIR_RailWay_OprModel"].Value.Byte == 1) // 自动模式
                    {
                        if (GlobalData.Instance.da["SIR_RailWay_WorkModel"].Value.Byte == 1)//上扣
                        {
                            this.spOneKeyInbutton.Visibility = Visibility.Visible;
                            this.spOneKeyOutButton.Visibility = Visibility.Collapsed;
                        }
                        else // 卸扣
                        {
                            this.spOneKeyInbutton.Visibility = Visibility.Collapsed;
                            this.spOneKeyOutButton.Visibility = Visibility.Visible;
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_oprModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.oprModel.IsChecked) //当前手动状态
            {
                byteToSend = new byte[10] { 80, 16, 13, 3, 0, 0, 0, 0, 0, 0 };
            }
            else//当前自动状态
            {
                byteToSend = new byte[10] { 80, 16, 13, 4, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_workModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.workModel.IsChecked) //当前卸扣
            {
                byteToSend = new byte[10] { 80, 16, 13, 5, 0, 0, 0, 0, 0, 0 };
            }
            else//当前上扣
            {
                byteToSend = new byte[10] { 80, 16, 13, 6, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 控制模式
        /// </summary>
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.controlModel.IsChecked) //当前近控
            {
                byteToSend = new byte[10] { 80, 16, 13, 7, 0, 0, 0, 0, 0, 0 };
            }
            else//当前远控
            {
                byteToSend = new byte[10] { 80, 16, 13, 8, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 高低档切换
        /// </summary>
        private void btn_highOrLowModel(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.controlModel.IsChecked) //当前高档
            //{
            //    byteToSend = new byte[10] { 80, 16, 13, 7, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前低档
            //{
            //    byteToSend = new byte[10] { 80, 16, 13, 8, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钻杆类型选择
        /// </summary>
        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 16, 13, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左旋
        /// </summary>
        private void btn_TurnLeft(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 16, 13, 1, 0, 0, 0, 0, 0, 0 };

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右旋
        /// </summary>
        private void btn_TurnRight(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 16, 13, 2, 0, 0, 0, 0, 0, 0 };

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工位选择
        /// </summary>
        private void btn_SelectLocation(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 16, 13, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 喷涂启停
        /// </summary>
        private void btn_SelectSpray(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 16, 13, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
