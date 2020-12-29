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

namespace Main.WellRepair.SIR_Self
{
    /// <summary>
    /// WR_SIR_Main.xaml 的交互逻辑
    /// </summary>
    public partial class WR_SIR_Main : UserControl
    {
        private static WR_SIR_Main _instance = null;
        private static readonly object syncRoot = new object();

        public static WR_SIR_Main Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new WR_SIR_Main();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer ReportTimer;
        public WR_SIR_Main()
        {
            InitializeComponent();
            VariableBinding();
            ReportTimer = new System.Threading.Timer(new TimerCallback(ReportTimer_Elapse), null, 500, 1000);
        }

        /// <summary>
        /// 扭矩定时器
        /// </summary>
        /// <param name="value"></param>
        private void ReportTimer_Elapse(object value)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    double inBtntorque = GlobalData.Instance.da["WR_SIR_InBtnTorque"].Value.Byte / 10.0;
                    double outBtntorque = GlobalData.Instance.da["WR_SIR_OutBtnTorque"].Value.Byte / 10.0;
                    double closeTongsPress = GlobalData.Instance.da["WR_SIR_CloseTongsPress"].Value.Int32 / 10.0;
                    double inBtnPress = GlobalData.Instance.da["WR_SIR_InBtnPress"].Value.Int32 / 10.0;

                    this.torqueChart.AddPoints(inBtntorque, outBtntorque);
                    this.pressChart.AddPoints(closeTongsPress, inBtnPress);
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.oprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_OprModel"], Mode = BindingMode.OneWay, Converter = new WRSIRSelfOperationModelConverter() });
                this.oprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_OprModel"], Mode = BindingMode.OneWay, Converter = new WRSIRSelfIsCheckConverter() });
                this.workModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_WorkModel"], Mode = BindingMode.OneWay, Converter = new WRSIRSelfWorkModelConverter() });
                this.workModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_WorkModel"], Mode = BindingMode.OneWay, Converter = new WRSIRSelfIsCheckConverter() });
                this.controlModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_ContorlModel"], Mode = BindingMode.OneWay, Converter = new WRSIRSelfControlModelConverter() });
                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_ContorlModel"], Mode = BindingMode.OneWay, Converter = new WRSIRSelfIsCheckConverter() });

                // 螺纹旋向-cheeck
                MultiBinding rotateModelCheckMultiBind = new MultiBinding();
                rotateModelCheckMultiBind.Converter = new WRSIRSelfRotateCheckCoverter();
                rotateModelCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["805b4"], Mode = BindingMode.OneWay });
                rotateModelCheckMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["805b5"], Mode = BindingMode.OneWay });
                rotateModelCheckMultiBind.NotifyOnSourceUpdated = true;
                this.RotateModel.SetBinding(BasedSwitchButton.IsCheckedProperty, rotateModelCheckMultiBind);
                // 螺纹旋向-文字
                MultiBinding rotateModelMultiBind = new MultiBinding();
                rotateModelMultiBind.Converter = new WRSIRSelfRotateCoverter();
                rotateModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["805b4"], Mode = BindingMode.OneWay });
                rotateModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["805b5"], Mode = BindingMode.OneWay });
                rotateModelMultiBind.NotifyOnSourceUpdated = true;
                this.RotateModel.SetBinding(BasedSwitchButton.ContentDownProperty, rotateModelMultiBind);

                this.PipeType.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_PipeType"], Mode = BindingMode.OneWay, Converter = new WRSIRSelfPipeTypeConverter() });
                this.PipeType.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_PipeType"], Mode = BindingMode.OneWay, Converter = new WRSIRSelfPipeTypeIsCheckConverter() });

                this.tbLocation.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_Location"], Mode = BindingMode.OneWay, Converter = new WRSIRSelfLocationConverter() });

                this.tbInButtonTorque.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_InBtnTorque"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonTorque.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_OutBtnTorque"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbInButtonTimes.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_InBtnTimes"], Mode = BindingMode.OneWay });
                this.tbOutButtonTimes.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_OutBtnTimes"], Mode = BindingMode.OneWay });
                this.tbWorkTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["WR_SIR_WorkTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutBtnPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_OutBtnPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbInButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_InBtnPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbCloseTongsPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_CloseTongsPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

                this.tbReachDis.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_ReachDis"], Mode = BindingMode.OneWay });
                this.tbRotate.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["WR_SIR_Rotate"], Mode = BindingMode.OneWay });

                // 一键上扣
                MultiBinding sbInbuttonMultiBind = new MultiBinding();
                sbInbuttonMultiBind.Converter = new WRSIRSelfAutoModeStepCoverter();
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_OprModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_WorkModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_AutoInBtnStep"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.ConverterParameter = "inButton";
                sbInbuttonMultiBind.NotifyOnSourceUpdated = true;
                this.sbInButton.SetBinding(StepBar.StepIndexProperty, sbInbuttonMultiBind);
                // 一键卸扣
                MultiBinding sbOutButtonMultiBind = new MultiBinding();
                sbOutButtonMultiBind.Converter = new WRSIRSelfAutoModeStepCoverter();
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_OprModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_Self_WorkModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["WR_SIR_AutoOutBtnStep"], Mode = BindingMode.OneWay });
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

        private int iTimeCnt = 0;//用来为时钟计数的变量
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Warnning();
                    this.Communcation();
                    if (GlobalData.Instance.da["WR_SIR_Self_WorkModel"].Value.Byte == 0) // 自动模式-上扣
                    {
                        this.spOneKeyInbutton.Visibility = Visibility.Visible;
                        this.spOneKeyOutButton.Visibility = Visibility.Collapsed;
                    }
                    else if (GlobalData.Instance.da["WR_SIR_Self_WorkModel"].Value.Byte == 1) // 自动模式-卸扣
                    {
                        this.spOneKeyInbutton.Visibility = Visibility.Collapsed;
                        this.spOneKeyOutButton.Visibility = Visibility.Visible;
                    }

                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void Warnning()
        {
            try
            {

                if (GlobalData.Instance.da["WR_SIR_AutoInBtnStep"].Value.Byte == 100)
                {
                    this.tbOprTips.Text = "请确认钳体高度是否合适";
                }
                else if (GlobalData.Instance.da["WR_SIR_AutoInBtnStep"].Value.Byte == 103)
                {
                    this.tbOprTips.Text = "请确认旋扣已经旋紧";
                }
                else if (GlobalData.Instance.da["WR_SIR_AutoInBtnStep"].Value.Byte == 108)
                {
                    this.tbOprTips.Text = "请确认旋扣已经旋松";
                }
                else
                {
                    this.tbOprTips.Text = "无操作提示";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.DEBUG);
            }
        }

        /// <summary>
        /// 通信数据
        /// </summary>
        private void Communcation()
        {
            #region 通信

            if (!GlobalData.Instance.ComunciationNormal) this.tbTips.Text = "网络连接失败！";
            else
            {
                if (this.tbTips.Text == "网络连接失败！") this.tbTips.Text = "";
            }
            #endregion
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_oprModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.oprModel.IsChecked) //当前手动状态
            {
                byteToSend = new byte[10] { 80, 16, 19, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前自动状态
            {
                byteToSend = new byte[10] { 80, 16, 19, 0, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_workModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.workModel.IsChecked) //当前下杆状态
            {
                byteToSend = new byte[10] { 80, 16, 20, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前起杆状态
            {
                byteToSend = new byte[10] { 80, 16, 20, 0, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 控制模式
        /// </summary>
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.controlModel.IsChecked) //当前本地控制
            {
                byteToSend = new byte[10] { 80, 16, 21, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前远程控制
            {
                byteToSend = new byte[10] { 80, 16, 21, 0, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 螺纹旋向
        /// </summary>
        private void btn_RotateModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.RotateModel.IsChecked) //当前右旋
            {
                byteToSend = new byte[10] { 80, 16, 1, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前做旋
            {
                byteToSend = new byte[10] { 80, 16, 1, 0, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 鼠洞工位
        /// </summary>
        private void btn_Mouse(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 16, 10, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井口工位
        /// </summary>
        private void btn_Well(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 16, 10, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 待机位
        /// </summary>
        private void btn_Mid(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 16, 10, 0, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 管柱类型
        /// </summary>
        private void btn_PipeType(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.RotateModel.IsChecked) //当前油管
            {
                byteToSend = new byte[10] { 80, 16, 11, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前钻杆
            {
                byteToSend = new byte[10] { 80, 16, 11, 0, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
