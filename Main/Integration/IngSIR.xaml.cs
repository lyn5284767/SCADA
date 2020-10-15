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
    /// IngSIR.xaml 的交互逻辑
    /// </summary>
    public partial class IngSIR : UserControl
    {
        private static IngSIR _instance = null;
        private static readonly object syncRoot = new object();

        public static IngSIR Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngSIR();
                        }
                    }
                }
                return _instance;
            }
        }
        private int iTimeCnt = 0;//用来为时钟计数的变量
        System.Threading.Timer timerWarning;
        public IngSIR()
        {
            InitializeComponent();
            VariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                //this.oprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfOperationModelConverter() });
                //this.oprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                //this.workModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                //this.workModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                //this.PipeTypeModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeModelConverter() });
                //this.PipeTypeModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeIsCheckConverter() });
                //this.locationModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocationModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocationModelConverter() });
                //this.locationModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocationModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });

               
                this.smMainGapOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMainGapTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBackGapOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInButtonPosOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInButtonPosTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOutButtonPosOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOutButtonPosTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSafeDoorReset.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["842b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSafeDoorClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["842b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.tbTongsGap.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfClampStatus"], Mode = BindingMode.OneWay, Converter = new SIRSelfTongsGapConverter() });
               
                this.tbInButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbInButtonClampingForce.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonClampingForce.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbRotate.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfRotateEncodePulse"], Mode = BindingMode.OneWay, Converter = new SIRSelfRotateConverter() });
               
                this.tbArmPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfArmPos"], Mode = BindingMode.OneWay, Converter = new TakeTenConverter() });
                this.tbClampHeight.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfClampHeight"], Mode = BindingMode.OneWay, Converter = new TakeTenConverter() });
                
                this.tbInButtonTorque.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfInButtonTorque"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonTorque.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonTorque"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbInButtonCircle.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonCircle"], Mode = BindingMode.OneWay });
                this.tbOutButtonCircle.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonCircle"], Mode = BindingMode.OneWay });
                
                //// 一键上扣
                //MultiBinding sbInbuttonMultiBind = new MultiBinding();
                //sbInbuttonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
                //sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                //sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                //sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
                //sbInbuttonMultiBind.ConverterParameter = "inButton";
                //sbInbuttonMultiBind.NotifyOnSourceUpdated = true;
                //this.sbInButton.SetBinding(StepBar.StepIndexProperty, sbInbuttonMultiBind);
                //// 一键卸扣
                //MultiBinding sbOutButtonMultiBind = new MultiBinding();
                //sbOutButtonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
                //sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                //sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                //sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
                //sbOutButtonMultiBind.ConverterParameter = "outButton";
                //sbOutButtonMultiBind.NotifyOnSourceUpdated = true;
                //this.sbOutButton.SetBinding(StepBar.StepIndexProperty, sbOutButtonMultiBind);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

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
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 融信数据
        /// </summary>
        private void Communcation()
        {
            #region 通信

            if (!GlobalData.Instance.ComunciationNormal) this.tbTips.Text = "网络连接失败！";
            #endregion
        }
        Dictionary<string, byte> WarnInfo = new Dictionary<string, byte>();
        private void Warnning()
        {
            try
            {

                byte alarmTips = GlobalData.Instance.da["SIRSelfAlarm"].Value.Byte;
                if (iTimeCnt % 10 != 0)
                {
                    switch (alarmTips)
                    {
                        case 120:
                            this.tbTips.Text = "工作缸气源气压过低告警";
                            break;
                        case 121:
                            this.tbTips.Text = "制动缸气源气压过低告警";
                            break;
                        case 1:
                            this.tbTips.Text = "背钳复位故障";
                            break;
                        case 102:
                            this.tbTips.Text = "工况选择错误";
                            break;
                        case 61:
                            this.tbTips.Text = "背钳夹紧故障";
                            break;
                        case 2:
                            this.tbTips.Text = "为达到上扣扭矩";
                            break;
                        case 3:
                            this.tbTips.Text = "未达到上扣圈数";
                            break;
                        case 4:
                            this.tbTips.Text = "未达到紧扣扭矩";
                            break;
                        case 5:
                            this.tbTips.Text = "未检测到背钳复位信号";
                            break;
                        case 6:
                            this.tbTips.Text = "未检测到安全门打开信号";
                            break;
                        case 12:
                            this.tbTips.Text = "钻杆未卸开";
                            break;
                        case 13:
                            this.tbTips.Text = "钻杆打滑或空杠卸扣";
                            break;
                        case 21:
                            this.tbTips.Text = "系统压力故障";
                            break;
                        case 70:
                            this.tbTips.Text = "当前手臂伸缩运动停止";
                            break;
                        case 71:
                            this.tbTips.Text = "当前钳体升降运动停止";
                            break;
                        case 75:
                            this.tbTips.Text = "当前钳体旋转运动停止";
                            break;
                    }
                }
                else
                {
                    //tbTips.Visibility = Visibility.Hidden;
                    tbTips.Text = "";
                }
                byte oprTips = GlobalData.Instance.da["SIRSelfOprInfo"].Value.Byte;
                switch (oprTips)
                {
                    case 60:
                        this.tbOprTips.Text = "工况选择结束后，请执行上扣对缺确认";
                        break;
                    case 61:
                        this.tbOprTips.Text = "工况选择结束后，请执行卸扣对缺确认";
                        break;
                    case 50:
                        this.tbOprTips.Text = "进入井口工位后切换自动模式";
                        break;
                    case 51:
                        this.tbOprTips.Text = "进入井口工位后切换自动模式";
                        break;
                    case 20:
                        this.tbOprTips.Text = "请检测钳头缺口状态";
                        break;
                    case 30:
                        this.tbOprTips.Text = "请确认安全门打开状态";
                        break;
                    case 29:
                        this.tbOprTips.Text = "请确认接箍高度";
                        break;
                    case 32:
                        this.tbOprTips.Text = "请确认井口安全";
                        break;
                    case 33:
                        this.tbOprTips.Text = "请将钳体缺口对正";
                        break;
                    case 34:
                        this.tbOprTips.Text = "请人工确认缺口状态";
                        break;
                    case 35:
                        this.tbOprTips.Text = "请人工确认安全门状态";
                        break;
                    case 101:
                        this.tbOprTips.Text = "请注意紧急停止";
                        break;
                    case 99:
                        this.tbOprTips.Text = "请退出自动模式检测故障";
                        break;
                    default:
                        this.tbOprTips.Text = "";
                        break;
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
