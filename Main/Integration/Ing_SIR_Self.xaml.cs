using COM.Common;
using ControlLibrary;
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

namespace Main.Integration
{
    /// <summary>
    /// Ing_SIR_Self.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_SIR_Self : UserControl
    {
        private static Ing_SIR_Self _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_SIR_Self Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_SIR_Self();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        public Ing_SIR_Self()
        {
            InitializeComponent();
            SIRVariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟

            SIRVariableReBinding = new System.Threading.Timer(new TimerCallback(SIRVariableTimer), null, Timeout.Infinite, 500);
            SIRVariableReBinding.Change(0, 500);
        }

        #region 铁钻工
        bool workModelCheck = false;
        byte bworkModel = 0;
        System.Threading.Timer SIRVariableReBinding;

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void SIRVariableBinding()
        {
            try
            {
                //this.siroprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfOperationModelConverter() });
                //this.siroprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                //this.sirworkModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                //this.sirworkModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                //this.sirPipeTypeModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeModelConverter() });
                //this.sirPipeTypeModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeIsCheckConverter() });
                //this.locationModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocationModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocationModelConverter() });
                //this.locationModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocationModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                //this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocalOrRemote"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocalOrRemoreCheckConverter() });
                //this.controlModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocalOrRemote"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocalOrRemoreConverter() });
                //this.tbRotate.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfRotateEncodePulse"], Mode = BindingMode.OneWay, Converter = new SIRSelfRotateConverter() });
                //this.tbArmPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfArmPos"], Mode = BindingMode.OneWay, Converter = new TakeTenConverter() });
                this.smInButtonPosOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInButtonPosTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOutButtonPosOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOutButtonPosTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_SIRoprModel(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.siroprModel.IsChecked) //当前手动状态
            //{
            //    byteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前自动状态
            //{
            //    byteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_SIRworkModel(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.sirworkModel.IsChecked) //当前上扣模式切换卸扣
            //{
            //    byteToSend = new byte[10] { 23, 17, 2, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前卸扣模式切换上扣
            //{
            //    byteToSend = new byte[10] { 23, 17, 2, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
            //this.sirworkModel.ContentDown = "切换中";
            //workModelCheck = true;
        }
        /// <summary>
        /// 管柱选择
        /// </summary>
        private void btn_SIRPipeTypeModel(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.sirPipeTypeModel.IsChecked) //当前钻杆
            //{
            //    byteToSend = new byte[10] { 23, 17, 3, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前套管
            //{
            //    byteToSend = new byte[10] { 23, 17, 3, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void SIRVariableTimer(object value)
        {
            //if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
            //{
            //    if (bworkModel != GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte && workModelCheck)
            //    {
            //        this.sirworkModel.Dispatcher.Invoke(new Action(() =>
            //        {
            //            this.sirworkModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
            //        }));
            //        workModelCheck = false;
            //    }
            //    bworkModel = GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte;
            //}
        }
        #endregion

        /// <summary>
        /// 工位选择
        /// </summary>
        private void btn_locationModel(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.locationModel.IsChecked) //当前井口
            //{
            //    byteToSend = new byte[10] { 23, 17, 4, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前鼠洞
            //{
            //    byteToSend = new byte[10] { 23, 17, 4, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 控制模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_controlModel(object sender, EventArgs e)
        {
            //byte[] drbyteToSend;
            //byte[] sirbyteToSend;
            //byte[] sfbyteToSend;
            //if (this.controlModel.IsChecked)
            //{
            //    drbyteToSend = new byte[10] { 1, 32, 2, 20, 0, 0, 0, 0, 0, 0 }; // 扶杆臂-遥控切司钻
            //    sirbyteToSend = new byte[10] { 23, 17, 10, 2, 0, 0, 0, 0, 0, 0 }; // 铁钻工-司钻切遥控
            //    sfbyteToSend = new byte[10] { 16, 1, 27, 1, 1, 0, 0, 0, 0, 0 };// 铁架工-遥控切司钻
            //    GlobalData.Instance.da.SendBytes(drbyteToSend);
            //    Thread.Sleep(50);
            //    GlobalData.Instance.da.SendBytes(sirbyteToSend);
            //    Thread.Sleep(50);
            //    GlobalData.Instance.da.SendBytes(sfbyteToSend);
            //}
            //else
            //{
            //    drbyteToSend = new byte[10] { 23, 17, 10, 1, 0, 0, 0, 0, 0, 0 };// 铁钻工-遥控切司钻
            //    GlobalData.Instance.da.SendBytes(drbyteToSend);

            //}
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
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        Dictionary<string, byte> WarnInfo = new Dictionary<string, byte>();
        BrushConverter bc = new BrushConverter();
        private void Warnning()
        {
            try
            {

                byte alarmTips = GlobalData.Instance.da["SIRSelfAlarm"].Value.Byte;
                this.tbSIRAlarm.Foreground = (Brush)bc.ConvertFrom("#E0496D");
                this.tbSIROpr.Foreground = (Brush)bc.ConvertFrom("#E0496D");
                //if (iTimeCnt % 10 != 0)
                //{
                switch (alarmTips)
                    {
                        case 15:
                            this.tbSIRAlarm.Text = "背钳复位故障";
                            break;
                        case 16:
                            this.tbSIRAlarm.Text = "工况选择故障";
                            break;
                        case 17:
                            this.tbSIRAlarm.Text = "背钳夹紧压力过低";
                            break;
                        case 18:
                            this.tbSIRAlarm.Text = "关门传感器故障";
                            break;
                        case 19:
                            this.tbSIRAlarm.Text = "未到上扣扭矩";
                            break;
                        case 20:
                            this.tbSIRAlarm.Text = "未到上扣圈数";
                            break;
                        case 21:
                            this.tbSIRAlarm.Text = "未到紧扣扭矩";
                            break;
                        case 22:
                            this.tbSIRAlarm.Text = "背钳传感器故障";
                            break;
                        case 23:
                            this.tbSIRAlarm.Text = "开门传感器故障";
                            break;
                        case 24:
                            this.tbSIRAlarm.Text = "钻杆未卸开";
                            break;
                        case 25:
                            this.tbSIRAlarm.Text = "卸扣打滑";
                            break;
                        case 26:
                            this.tbSIRAlarm.Text = "液压系统压力过低";
                            break;
                        case 27:
                            this.tbSIRAlarm.Text = "手臂伸缩卡滞";
                            break;
                        case 28:
                            this.tbSIRAlarm.Text = "钳体升降卡滞";
                            break;
                        case 29:
                            this.tbSIRAlarm.Text = "钳体回转限位已发生";
                            break;
                        case 30:
                            this.tbSIRAlarm.Text = "工作缸气压过低警告";
                            break;
                        case 31:
                            this.tbSIRAlarm.Text = "制动缸气压过低警告";
                            break;
                        default:
                            tbSIRAlarm.Text = "暂无告警";
                            this.tbSIRAlarm.Foreground = (Brush)bc.ConvertFrom("#000000");
                            break;
                    }
                //}
                //else
                //{
                //    //tbSIRAlarm.Visibility = Visibility.Hidden;
                //    //tbSIRAlarm.Text = "暂无告警";
                //    //this.tbSIRAlarm.Foreground = (Brush)bc.ConvertFrom("#000000");
                //}
                byte oprTips = GlobalData.Instance.da["SIRSelfOprInfo"].Value.Byte;
                switch (oprTips)
                {
                    case 20:
                        this.tbSIROpr.Text = "上扣工况选择后请执行自动对缺确认";
                        break;
                    case 21:
                        this.tbSIROpr.Text = "卸扣工况选择后请执行自动对缺确认";
                        break;
                    case 22:
                        this.tbSIROpr.Text = "请确认缺口复位状态";
                        break;
                    case 23:
                        this.tbSIROpr.Text = "在井口工位请确认接箍高度";
                        break;
                    case 24:
                        this.tbSIROpr.Text = "当前工况请使用手动上扣";
                        break;
                    case 25:
                        this.tbSIROpr.Text = "传感器异常请重新复位缺口";
                        break;
                    case 26:
                        this.tbSIROpr.Text = "请人工确认缺口复位状态";
                        break;
                    case 27:
                        this.tbSIROpr.Text = "当前工况请使用手动卸扣";
                        break;
                    case 28:
                        this.tbSIROpr.Text = "注意:当前系统状态为紧急停止!";
                        break;
                    case 29:
                        this.tbSIROpr.Text = "模式切换了!";
                        break;
                    case 30:
                        this.tbSIROpr.Text = "请退出检查故障!";
                        break;
                    case 31:
                        this.tbSIROpr.Text = "铁钻工不在上扣工位";
                        break;
                    case 32:
                        this.tbSIROpr.Text = "铁钻工不在卸扣工位";
                        break;
                    case 33:
                        this.tbSIROpr.Text = "安全互锁已启动";
                        break;
                    case 34:
                        this.tbSIROpr.Text = "安全互锁已解除";
                        break;
                    case 35:
                        this.tbSIROpr.Text = "司钻模式下回转调节启用";
                        break;
                    case 36:
                        this.tbSIROpr.Text = "遥控模式下回转调节启用";
                        break;
                    case 37:
                        this.tbSIROpr.Text = "已到钻杆顶销位";
                        break;
                    case 38:
                        this.tbSIROpr.Text = "手臂禁止伸出";
                        break;
                    case 39:
                        this.tbSIROpr.Text = "禁止上卸扣";
                        break;
                    default:
                        this.tbSIROpr.Text = "暂无操作提示";
                        this.tbSIROpr.Foreground = (Brush)bc.ConvertFrom("#000000");
                        break;
                }
                //上扣指示灯亮，但是未卸扣工作模式 
                if (GlobalData.Instance.da["841b3"].Value.Boolean && GlobalData.Instance.da["841b5"].Value.Boolean
                && GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 2)
                {
                    this.tbSIRAlarm.Text = "卸扣模式不一致，请切换手/自动";
                }
                else
                {
                    if (this.tbSIRAlarm.Text == "卸扣模式不一致，请切换手/自动")
                    {
                        this.tbSIRAlarm.Text = "暂无告警";
                        this.tbSIRAlarm.Foreground = (Brush)bc.ConvertFrom("#000000");
                    }
                }

                if (GlobalData.Instance.da["841b4"].Value.Boolean && GlobalData.Instance.da["841b6"].Value.Boolean
            && GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 1)
                {
                    this.tbSIRAlarm.Text = "上扣模式不一致，请切换手/自动";
                }
                else
                {
                    if (this.tbSIRAlarm.Text == "上扣模式不一致，请切换手/自动")
                    {
                        this.tbSIRAlarm.Text = "暂无告警";
                        this.tbSIRAlarm.Foreground = (Brush)bc.ConvertFrom("#000000");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.DEBUG);
            }
        }
    }
}
