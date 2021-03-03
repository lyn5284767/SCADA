using COM.Common;
using DatabaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Main.Integration
{
    /// <summary>
    /// IngMainNew.xaml 的交互逻辑
    /// </summary>
    public partial class IngMainNew : UserControl
    {
        private static IngMainNew _instance = null;
        private static readonly object syncRoot = new object();

        public static IngMainNew Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngMainNew();
                        }
                    }
                }
                return _instance;
            }
        }
        #region Field
        /// <summary>
        /// 二层台工作模式 1：送杆；2排杆
        /// </summary>
        private int sfWorkModel { get; set; }
        /// <summary>
        /// 钻台面工作模式 1：送杆；2排杆
        /// </summary>
        private int drWorkModel { get; set; }
        /// <summary>
        /// 铁钻工工作模式 1：上扣；2卸扣
        /// </summary>
        private int sirWorkModel { get; set; }
        /// <summary>
        /// 二层台操作模式 1：送杆；2排杆
        /// </summary>
        private int sfOprModel { get; set; }
        /// <summary>
        /// 钻台面操作模式 1：送杆；2排杆
        /// </summary>
        private int drOprModel { get; set; }
        /// <summary>
        /// 铁钻工操作模式 1：上扣；2卸扣
        /// </summary>
        private int sirOprModel { get; set; }
        /// <summary>
        /// 二层台钻杆类型
        /// </summary>
        public int sfTubesType { get; set; }
        /// <summary>
        /// 钻台面钻杆类型
        /// </summary>
        public int drTubesType { get; set; }
        /// <summary>
        /// 钻台面目的地
        /// </summary>
        public int drDes { get; set; }
        /// <summary>
        /// 二层台当前选择指梁
        /// </summary>
        public int sfSelectDrill { get; set; }
        /// <summary>
        /// 钻台面当前选择指梁
        /// </summary>
        public int drSelectDrill { get; set; }
        BrushConverter bc = new BrushConverter();
        Dictionary<string, byte> alarmKey = new Dictionary<string, byte>();
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool bCheckTwo = false;

        public delegate void SetNowTechnique(Technique technique);
        public event SetNowTechnique SetNowTechniqueEvent;
        Technique tmpTechnique;
        Technique nowTechnique;

        private bool IsTopAllLock = false;
        private bool IsKavaAllLock = false;
        private bool IsElevatorAllLock = false;
        private bool IsHookAllLock = false;
        private bool IsSfAllLock = false;
        private bool IsDrAllLock = false;
        private bool IsSirAllLock = false;
        private bool IsPreventBoxAllLock = false;
        private bool IsCatAllLock = false;
        private bool TotalLock = false;
        #endregion

        public IngMainNew()
        {
            InitializeComponent();
            if (GlobalData.Instance.da["DrillNums"] != null)
                GlobalData.Instance.Rows = GlobalData.Instance.da["DrillNums"].Value.Byte;
            if (GlobalData.Instance.da["103E23B5"] != null)
                GlobalData.Instance.DrillNum = GlobalData.Instance.da["103E23B5"].Value.Byte;

            VariableBinding();
            TotalVariableReBinding = new System.Threading.Timer(new TimerCallback(TotalVariableTimer), null, Timeout.Infinite, 50);
            TotalVariableReBinding.Change(0, 50);
            InitAlarmKey();
            this.Loaded += IngMain_Loaded;
        }

        private void IngMain_Loaded(object sender, RoutedEventArgs e)
        {
            InitPanel();
            InitAllModel();
            string configPath = System.Environment.CurrentDirectory + "\\KeyBoard.exe";
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processList)
            {
                if (process.ProcessName.Contains("KeyBoard"))
                {
                    process.Kill();
                }
            }
        }

        private void InitPanel()
        {
            //#region 根据配置选择界面
            //try
            //{
            //    if (GlobalData.Instance.da.GloConfig.SFType == 1) // 二层台选择
            //    {
            //        this.gdSF.Children.Clear();
            //        this.gdSF.Children.Add(Ing_SF_Self.Instance);
            //    }
            //    else
            //    {
            //        Ing_Bank ing_Bank = new Ing_Bank("二层台(未配置)");
            //        this.gdSF.Children.Clear();
            //        this.gdSF.Children.Add(ing_Bank);
            //    }
            //    if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY) // 钻台面选择
            //    {
            //        this.gdDR.Children.Clear();
            //        this.gdDR.Children.Add(Ing_DR_Self.Instance);
            //    }
            //    else
            //    {
            //        Ing_Bank ing_Bank = new Ing_Bank("钻台面(未配置)");
            //        this.gdDR.Children.Clear();
            //        this.gdDR.Children.Add(ing_Bank);
            //    }
            //    if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY) // 铁钻工选择
            //    {
            //        this.gdSIR.Children.Clear();
            //        this.gdSIR.Children.Add(Ing_SIR_Self.Instance);
            //    }
            //    else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.BS)
            //    {
            //        this.gdSIR.Children.Clear();
            //        this.gdSIR.Children.Add(Ing_SIR_BS.Instance);
            //    }
            //    else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.JJC)
            //    {
            //        this.gdSIR.Children.Clear();
            //        this.gdSIR.Children.Add(Ing_SIR_JJC.Instance);
            //    }
            //    else
            //    {
            //        Ing_Bank ing_Bank = new Ing_Bank("铁钻工(未配置)");
            //        this.gdSIR.Children.Clear();
            //        this.gdSIR.Children.Add(ing_Bank);
            //        this.sirStatus.LampType = 0;
            //    }
            //    if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.SANY)// 液压站选择
            //    {
            //        this.gdHS.Children.Clear();
            //        this.gdHS.Children.Add(Ing_HS_Self.Instance);
            //    }
            //    else if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.JJC)
            //    {
            //        this.gdHS.Children.Clear();
            //        this.gdHS.Children.Add(Ing_HS_JJC.Instance);
            //    }
            //    else
            //    {
            //        Ing_Bank ing_Bank = new Ing_Bank("液压站(未配置)");
            //        this.gdHS.Children.Clear();
            //        this.gdHS.Children.Add(ing_Bank);
            //        this.hsStatus.LampType = 0;
            //    }
            //    if (GlobalData.Instance.da.GloConfig.CatType == (int)CatType.BS) // 猫道选择
            //    {
            //        this.gdCat.Children.Clear();
            //        this.gdCat.Children.Add(Ing_Cat_BS.Instance);
            //    }
            //    else
            //    {
            //        Ing_Bank ing_Bank = new Ing_Bank("猫道(未配置)");
            //        this.gdCat.Children.Clear();
            //        this.gdCat.Children.Add(ing_Bank);
            //        this.catStatus.LampType = 0;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            //}
            //#endregion
        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                TotalVariableBinding();
                IngVariableBinding();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        #region 总览
        System.Threading.Timer TotalVariableReBinding;
        private void TotalVariableBinding()
        {
            try
            {
                this.tbLinkStatus.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay, Converter = new LinkOpenOrCloseConverter() });
                this.bigHookRealTimeValue.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["156To159BigHookEncoderRealTimeValue"], Mode = BindingMode.OneWay, Converter = new DivideHundredConverter() });
                this.tbKava.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["575b6"], Mode = BindingMode.OneWay, Converter = new KavaConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private void InitAlarmKey()
        {
            alarmKey.Add("设备工作模式不一致", 0);
            alarmKey.Add("设备操作模式不一致", 0);
            alarmKey.Add("管柱类型不一致", 0);
            alarmKey.Add("二层台电机未使能或回零", 0);
            alarmKey.Add("钻台面电机未使能或回零", 0);
        }
        private int iTimeCnt = 0;//用来为时钟计数的变量
        /// <summary>
        /// 总览页变量绑定计时器
        /// </summary>
        /// <param name="value"></param>
        private void TotalVariableTimer(object value)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                iTimeCnt++;
                if (iTimeCnt > 1000) iTimeCnt = 0;
                if (GlobalData.Instance.da.GloConfig.SFType == 1) // 二层台选择
                {
                    if (GlobalData.Instance.da["workModel"] != null)
                        this.sfWorkModel = GlobalData.Instance.da["workModel"].Value.Byte;
                    if (GlobalData.Instance.da["operationModel"] != null)
                        this.sfOprModel = GlobalData.Instance.da["operationModel"].Value.Byte;
                    if (GlobalData.Instance.da["drillPipeType"] != null)
                        this.sfTubesType = GlobalData.Instance.da["drillPipeType"].Value.Byte;
                    if (GlobalData.Instance.da["pcFingerBeamNumberFeedback"] != null)
                        this.sfSelectDrill = GlobalData.Instance.da["pcFingerBeamNumberFeedback"].Value.Byte;
                }
                else // 没有二层台
                {
                    this.sfWorkModel = -1;
                    this.sfOprModel = -1;
                    this.sfTubesType = -1;
                    this.sfSelectDrill = -1;
                }
                if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY) // 钻台面选择
                {
                    if (GlobalData.Instance.da["drworkModel"] != null)
                        this.drWorkModel = GlobalData.Instance.da["drworkModel"].Value.Byte;
                    if (GlobalData.Instance.da["droperationModel"] != null)
                        this.drOprModel = GlobalData.Instance.da["droperationModel"].Value.Byte;
                    if (GlobalData.Instance.da["drdrillPipeType"] != null)
                        this.drTubesType = GlobalData.Instance.da["drdrillPipeType"].Value.Byte;
                    if (GlobalData.Instance.da["drDes"] != null)
                        this.drDes = GlobalData.Instance.da["drDes"].Value.Byte;
                    if (GlobalData.Instance.da["drSelectDrill"] != null)
                        this.drSelectDrill = GlobalData.Instance.da["drSelectDrill"].Value.Byte;
                }
                else // 没有钻台面
                {
                    this.drWorkModel = -1;
                    this.drOprModel = -1;
                    this.drTubesType = -1;
                    this.drDes = -1;
                    this.drSelectDrill = -1;
                }
                if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY) // 铁钻工选择
                {
                    if (GlobalData.Instance.da["SIRSelfWorkModel"] != null)
                        this.sirWorkModel = GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte;
                    if (GlobalData.Instance.da["SIRSelfOperModel"] != null)
                        this.sirOprModel = GlobalData.Instance.da["SIRSelfOperModel"].Value.Byte;
                }
                else // 没有铁钻工
                {
                    this.sirWorkModel = -1;
                    this.sirOprModel = -1;
                }
                if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.SANY)// 液压站选择
                {

                }
                if (GlobalData.Instance.da.GloConfig.CatType == (int)CatType.BS) // 猫道选择
                {

                }

                this.Warnning();
                this.Communcation();
                this.MonitorSysStatus();
                this.ShowLinkStatus();
                this.ShowLockStatus();
                this.ShowHand();
                if (this.tbAlarm.Text != "无告警信息") this.tbAlarm.Foreground = (Brush)bc.ConvertFrom("#E0496D");
                else this.tbAlarm.Foreground = (Brush)bc.ConvertFrom("#808080");
                if (this.tbOpr.Text != "无操作提示") this.tbAlarm.Foreground = (Brush)bc.ConvertFrom("#E0496D");
                else this.tbOpr.Foreground = (Brush)bc.ConvertFrom("#808080");
            }));
        }


        /// <summary>
        /// 通信数据
        /// </summary>
        private void Communcation()
        {
            #region 通信

            //操作台控制器心跳
            if (GlobalData.Instance.da["508b5"] != null && GlobalData.Instance.da["508b5"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 600)
                {
                    //Communication = 2;
                    this.tbAlarm.Text = "操作台信号中断";
                }
                if (!bCheckTwo && controlHeartTimes > 600)
                {
                    GlobalData.Instance.reportData.OperationFloorCommunication += 1;//the report
                    bCheckTwo = true;
                }
            }
            else
            {
                this.controlHeartTimes = 0;
                bCheckTwo = false;
            }
            if (GlobalData.Instance.da["508b5"] != null)
                this.tmpStatus = GlobalData.Instance.da["508b6"].Value.Boolean;

            if (!GlobalData.Instance.ComunciationNormal) this.tbAlarm.Text = "网络连接失败！";
            else
            {
                if (this.tbAlarm.Text == "网络连接失败！") this.tbAlarm.Text = "";
            }
            #endregion
        }

        private void Warnning()
        {
            try
            {
                if (iTimeCnt % 10 == 0)
                {
                    if (alarmKey.ContainsValue(1) || alarmKey.ContainsValue(2))
                    {
                        this.tbAlarm.FontSize = 14;
                        this.tbAlarm.Visibility = Visibility.Visible;

                        if (!alarmKey.ContainsValue(1) && alarmKey.ContainsValue(2)) // 如果没有显示为1的值，但是有显示为2的值，表示有告警且，显示循环完成，重置为1继续循环
                        {
                            foreach (var key in alarmKey.Keys.ToList())
                            {
                                if (alarmKey[key] == 2)
                                {
                                    alarmKey[key] = 1;
                                }
                            }
                        }

                        foreach (var key in alarmKey.Keys.ToList())
                        {
                            if (alarmKey[key] == 1)
                            {
                                this.tbAlarm.Text = key;
                                alarmKey[key] = 2;
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.tbAlarm.Visibility = Visibility.Hidden;
                        this.tbAlarm.Text = "暂无";
                    }
                }
                else
                {
                    this.tbAlarm.FontSize = 20;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void MonitorSysStatus()
        {
            try
            {
                // 存在的设备全为1，设备不存在为-1，则为送杆
                if ((this.sfWorkModel == 1 || this.sfWorkModel == -1)
                    && (this.drWorkModel == 1 || this.drWorkModel == -1)
                    && (this.sirWorkModel == 1 || this.sirWorkModel == -1))
                {

                    this.tbCurWork.Text = "送杆";
                    //this.workMode.ContentDown = "送杆";
                    //this.workMode.IsChecked = false;
                    alarmKey["设备工作模式不一致"] = 0;
                    nowTechnique = Technique.DrillDown;
                }// 存在的设备全为2，设备不存在为-1，则为排杆
                else if ((this.sfWorkModel == 2 || this.sfWorkModel == -1)
                    && (this.drWorkModel == 2 || this.drWorkModel == -1)
                    && (this.sirWorkModel == 2 || this.sirWorkModel == -1))
                {
                    this.tbCurWork.Text = "排杆";
                    //this.workMode.ContentDown = "排杆";
                    //this.workMode.IsChecked = true;
                    alarmKey["设备工作模式不一致"] = 0;
                    nowTechnique = Technique.DrillUp;
                }
                else
                {
                    this.tbCurWork.Text = "未知";
                    //this.workMode.ContentDown = "未知";
                    //this.workMode.IsChecked = false;
                    if (alarmKey["设备工作模式不一致"] == 0) alarmKey["设备工作模式不一致"] = 1;
                }
                // 存在的设备全为4，设备不存在为-1，则为手动
                if ((this.sfOprModel == 4 || this.sfOprModel == -1)
                    && (this.drOprModel == 4 || this.drOprModel == -1)
                    && (this.sirOprModel == 1 || this.sirOprModel == -1))
                {

                    this.tbCurOpr.Text = "手动";
                    //this.operateMode.ContentDown = "手动";
                    //this.operateMode.IsChecked = true;
                    alarmKey["设备操作模式不一致"] = 0;
                }// 存在的设备全为5，设备不存在为-1，则为自动
                else if ((this.sfOprModel == 5 || this.sfOprModel == -1)
                    && (this.drOprModel == 5 || this.drOprModel == -1)
                    && (this.sirOprModel == 2 || this.sirOprModel == -1))
                {
                    this.tbCurOpr.Text = "自动";
                    //this.operateMode.ContentDown = "自动";
                    //this.operateMode.IsChecked = false;
                    alarmKey["设备操作模式不一致"] = 0;
                }
                else
                {
                    this.tbCurOpr.Text = "未知";
                    //this.operateMode.ContentDown = "未知";
                    //this.operateMode.IsChecked = false;
                    if (alarmKey["设备操作模式不一致"] == 0) alarmKey["设备操作模式不一致"] = 1;
                }

                //二层台，钻台面都钻杆相同，或者有一个设备没配置
                if (this.sfTubesType == this.drTubesType || this.sfTubesType == -1 || this.drTubesType == -1)
                {
                    int bType = -1;
                    if (this.sfTubesType == this.drTubesType) bType = this.sfTubesType;
                    else if (this.sfTubesType == -1) bType = this.drTubesType;
                    else if (this.drTubesType == -1) bType = this.sfTubesType;
                    else this.tbCurTubesType.Text = "二层台/钻台面都未配置";
                    switch (bType)
                    {
                        case 35:
                            this.tbCurTubesType.Text = "3.5寸钻杆";
                            break;
                        case 40:
                            this.tbCurTubesType.Text = "4寸钻杆";
                            break;
                        case 45:
                            this.tbCurTubesType.Text = "4.5寸钻杆";
                            break;
                        case 50:
                            this.tbCurTubesType.Text = "5寸钻杆";
                            break;
                        case 55:
                            this.tbCurTubesType.Text = "5.5寸钻杆";
                            break;
                        case 60:
                            this.tbCurTubesType.Text = "6寸钻铤";
                            break;
                        case 65:
                            this.tbCurTubesType.Text = "6.5寸钻铤";
                            break;
                        case 70:
                            this.tbCurTubesType.Text = "7寸钻铤";
                            break;
                        case 75:
                            this.tbCurTubesType.Text = "7.5寸钻铤";
                            break;
                        case 80:
                            this.tbCurTubesType.Text = "8寸钻铤";
                            break;
                        case 90:
                            this.tbCurTubesType.Text = "9寸钻铤";
                            break;
                        case 100:
                            this.tbCurTubesType.Text = "10寸钻铤";
                            break;
                        case 110:
                            this.tbCurTubesType.Text = "11寸钻铤";
                            break;
                        default:
                            this.tbCurTubesType.Text = "未选择";
                            break;
                    }
                    alarmKey["管柱类型不一致"] = 0;
                }
                else
                {
                    this.tbCurTubesType.Text = "钻杆类型不一致";
                    if (alarmKey["管柱类型不一致"] == 0) alarmKey["管柱类型不一致"] = 1;
                    this.tbCurTubesType.Text = this.tbCurTubesType.Text;
                }
                // 目的地
                if (this.drDes == 1) this.tbCurDes.Text = "立根区";
                else if (drDes == 2) this.tbCurDes.Text = "猫道-井口";
                else if (drDes == 3) this.tbCurDes.Text = "猫道-鼠道";
                else if (drDes == -1) this.tbCurDes.Text = "未配置钻台面";
                else this.tbCurDes.Text = "未知";
                // 选择指梁

                if (sfSelectDrill == -1)// 二层台未配置
                {
                    if (drSelectDrill <= 16 && drSelectDrill >= 1)
                    {
                        this.tbCurSelectDrill.Text = "左" + drSelectDrill;
                    }
                    else if (drSelectDrill <= 32 && drSelectDrill > 16)
                    {
                        this.tbCurSelectDrill.Text = "右" + (drSelectDrill - 16);
                    }
                    else
                    {
                        this.tbCurSelectDrill.Text = "未知";
                    }
                }
                else
                {
                    if (sfSelectDrill <= 16 && sfSelectDrill >= 1)
                    {
                        this.tbCurSelectDrill.Text = "左" + sfSelectDrill;
                    }
                    else if (sfSelectDrill <= 32 && sfSelectDrill > 16)
                    {
                        this.tbCurSelectDrill.Text = "右" + (sfSelectDrill - 16);
                    }
                    else
                    {
                        this.tbCurSelectDrill.Text = "未知";
                    }
                }
                //// 460b0=true联动开启
                //if (nowTechnique != tmpTechnique)
                //{
                //    if (SetNowTechniqueEvent != null)
                //    {
                //        SetNowTechniqueEvent(nowTechnique);
                //    }
                //    tmpTechnique = nowTechnique;
                //}

                // 二层台电机回零使能状态
                if (GlobalData.Instance.da["carMotorRetZeroStatus"].Value.Boolean && GlobalData.Instance.da["armMotorRetZeroStatus"].Value.Boolean
                    && GlobalData.Instance.da["rotateMotorRetZeroStatus"].Value.Boolean && !GlobalData.Instance.da["carMotorWorkStatus"].Value.Boolean
                    && !GlobalData.Instance.da["armMotorWorkStatus"].Value.Boolean && !GlobalData.Instance.da["rotateMotorWorkStatus"].Value.Boolean)
                {
                    this.sfStatus.LampType = 1;
                    alarmKey["二层台电机未使能或回零"] = 0;
                }
                else
                {
                    this.sfStatus.LampType = 3;
                    if (alarmKey["二层台电机未使能或回零"] == 0) alarmKey["二层台电机未使能或回零"] = 1;
                }
                if (GlobalData.Instance.da["324b1"].Value.Boolean && GlobalData.Instance.da["324b5"].Value.Boolean
                    && GlobalData.Instance.da["324b0"].Value.Boolean && GlobalData.Instance.da["324b4"].Value.Boolean)
                {
                    this.drStatus.LampType = 1;
                    alarmKey["钻台面电机未使能或回零"] = 0;
                }
                else
                {
                    this.drStatus.LampType = 3;
                    if (alarmKey["钻台面电机未使能或回零"] == 0) alarmKey["钻台面电机未使能或回零"] = 1;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        #endregion

        #region 联动设置
        /// <summary>
        /// 联动变量绑定
        /// </summary>
        private void IngVariableBinding()
        {
            try
            {
                // 6.30新增
                //this.oobLink.SetBinding(OnOffButton.OnOffButtonCheckProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
                //this.tbLink.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay, Converter = new LinkOpenOrCloseConverter() });
                //MultiBinding LinkErrorMultiBind = new MultiBinding();
                //LinkErrorMultiBind.Converter = new LinkErrorCoverter();
                //LinkErrorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drLinkError"], Mode = BindingMode.OneWay });
                //LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
                //LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b1"], Mode = BindingMode.OneWay });
                //LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b2"], Mode = BindingMode.OneWay });
                //LinkErrorMultiBind.NotifyOnSourceUpdated = true;
                //this.LinkError.SetBinding(TextBlock.TextProperty, LinkErrorMultiBind);
                // 操作模式-描述
                //MultiBinding IngOprMultiBind = new MultiBinding();
                //IngOprMultiBind.Converter = new IngOprCoverter();
                //IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                //IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                //IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                //IngOprMultiBind.NotifyOnSourceUpdated = true;
                //this.operateMode.SetBinding(BasedSwitchButton.ContentDownProperty, IngOprMultiBind);
                //// 操作模式-选择
                //MultiBinding IngOprCheckMultiBind = new MultiBinding();
                //IngOprCheckMultiBind.Converter = new IngOprCheckCoverter();
                //IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                //IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                //IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                //IngOprCheckMultiBind.NotifyOnSourceUpdated = true;
                //this.operateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, IngOprCheckMultiBind);
                // 工作模式-描述
                //MultiBinding IngWorkMultiBind = new MultiBinding();
                //IngWorkMultiBind.Converter = new IngWorkCoverter();
                //IngWorkMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
                //IngWorkMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                //IngWorkMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                //IngWorkMultiBind.NotifyOnSourceUpdated = true;
                //this.workMode.SetBinding(BasedSwitchButton.ContentDownProperty, IngWorkMultiBind);
                // 工作模式-选择
                //MultiBinding IngWorkCheckMultiBind = new MultiBinding();
                //IngWorkCheckMultiBind.Converter = new IngWorkCheckCoverter();
                //IngWorkCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                //IngWorkCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                //IngWorkCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                //IngWorkCheckMultiBind.NotifyOnSourceUpdated = true;
                //this.workMode.SetBinding(BasedSwitchButton.IsCheckedProperty, IngWorkCheckMultiBind);
                // 管柱选择
                //IngDrillPipeTypeConverter ingDrillPipeTypeConverter = new IngDrillPipeTypeConverter();
                //MultiBinding ingDrillPipeTypeMultiBind = new MultiBinding();
                //ingDrillPipeTypeMultiBind.Converter = ingDrillPipeTypeConverter;
                //ingDrillPipeTypeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drillPipeType"], Mode = BindingMode.OneWay });
                //ingDrillPipeTypeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drdrillPipeType"], Mode = BindingMode.OneWay });
                //ingDrillPipeTypeMultiBind.NotifyOnSourceUpdated = true;
                //this.tubeType.SetBinding(TextBlock.TextProperty, ingDrillPipeTypeMultiBind);

                //this.drDestination.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDes"], Mode = BindingMode.OneWay, Converter = new DesTypeConverter() });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 联动打开关闭
        /// </summary>
        /// <param name="isChecked"></param>
        private void OnOffButton_CBCheckedEvent(bool isChecked)
        {
            if (!GlobalData.Instance.da["460b0"].Value.Boolean)// 联动开启
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else// 联动关闭
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 0, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
           
           
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_OpState(object sender, EventArgs e)
        {
            //if (!GlobalData.Instance.da["460b0"].Value.Boolean)
            //{
            //    System.Windows.MessageBox.Show("联动未开启，不允许设置");
            //    return;
            //}
            //byte[] sfbyteToSend;// 二层台
            //byte[] drbyteToSend;// 钻台面
            //byte[] sirbyteToSend;// 铁钻工
            //if (this.operateMode.IsChecked)
            //{
            //    sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 5 });
            //    drbyteToSend = new byte[10] { 1, 32, 3, 31, 0, 0, 0, 0, 0, 0 };
            //    sirbyteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //else
            //{
            //    sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 4 });
            //    drbyteToSend = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };
            //    sirbyteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //if (GlobalData.Instance.da.GloConfig.SFType == 1) // 二层台选择
            //{
            //    GlobalData.Instance.da.SendBytes(sfbyteToSend);
            //    Thread.Sleep(50);
            //}
            //if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY) //钻台面选择
            //{
            //    GlobalData.Instance.da.SendBytes(drbyteToSend);
            //    Thread.Sleep(50);
            //}
            //if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY) // 铁钻工选择
            //{
            //    GlobalData.Instance.da.SendBytes(sirbyteToSend);
            //}
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_WorkModel(object sender, EventArgs e)
        {
            //if (!GlobalData.Instance.da["460b0"].Value.Boolean)
            //{
            //    System.Windows.MessageBox.Show("联动未开启，不允许设置");
            //    return;
            //}
            //byte[] sfbyteToSend;// 二层台
            //byte[] drbyteToSend;// 钻台面
            //byte[] sirbyteToSend;// 铁钻工
            //if (workMode.IsChecked)
            //{
            //    sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
            //    drbyteToSend = new byte[10] { 1, 32, 4, 41, 0, 0, 0, 0, 0, 0 };
            //    sirbyteToSend = new byte[10] { 23, 17, 2, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //else
            //{
            //    sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
            //    drbyteToSend = new byte[10] { 1, 32, 4, 40, 0, 0, 0, 0, 0, 0 };
            //    sirbyteToSend = new byte[10] { 23, 17, 2, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //if (GlobalData.Instance.da.GloConfig.SFType == 1) // 二层台选择
            //{
            //    GlobalData.Instance.da.SendBytes(sfbyteToSend);
            //    Thread.Sleep(50);
            //}
            //if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY) //钻台面选择
            //{
            //    GlobalData.Instance.da.SendBytes(drbyteToSend);
            //    Thread.Sleep(50);
            //}
            //if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY) // 铁钻工选择
            //{
            //    GlobalData.Instance.da.SendBytes(sirbyteToSend);
            //}
        }
        /// <summary>
        /// 管柱选择协议
        /// </summary>
        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, (byte)tag });
            if (GlobalData.Instance.da.GloConfig.SFType == 1) // 二层台选择
            {
                GlobalData.Instance.da.SendBytes(byteToSend);
                Thread.Sleep(50);
            }
            byteToSend = GlobalData.Instance.SendToDR(new List<byte>() { 3, (byte)tag });

            if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY) //钻台面选择
            {
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 目的地选择
        /// </summary>
        private void btn_SelectDes(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 33, 11, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion
        /// <summary>
        /// 设置所选指梁
        /// </summary>
        private void btn_SetDrillNum(object sender, RoutedEventArgs e)
        {
            DrillSetWindow window = new DrillSetWindow();
            window.ShowDialog();
        }
        /// <summary>
        /// 零位标定
        /// </summary>
        private void tbZeroSet_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[] { 16, 1, 24, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            ShowTips();
        }
        /// <summary>
        /// 取消零位标定
        /// </summary>
        private void tbZeroUnSet_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[] { 16, 1, 24, 11, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            ShowTips();
        }

        private void ShowTips()
        {
            Thread.Sleep(500);
            string msg = GetMsg(GlobalData.Instance.da["155InterlockPromptMessageCode"].Value.Byte);
            if (msg != string.Empty)
            {
                System.Windows.MessageBox.Show(msg);
            }
        }

        private string GetMsg(int val)
        {
            if (val == 1) return "零位标定不合理";

            else return string.Empty;
        }

        private void cbLink_Clicked(object sender, EventArgs e)
        {
            //if(!this.cbLink.IsChecked)
            //{
            //    byte[] byteToSend = new byte[10] { 1, 32, 10, 1, 0, 0, 0, 0, 0, 0 };
            //    GlobalData.Instance.da.SendBytes(byteToSend);
            //}
            //else// 联动关闭
            //{
            //    byte[] byteToSend = new byte[10] { 1, 32, 10, 0, 0, 0, 0, 0, 0, 0 };
            //    GlobalData.Instance.da.SendBytes(byteToSend);
            //}
        }
        GlobalModel model;
        /// <summary>
        /// 初始化所有模式/设备
        /// </summary>
        public void InitAllModel()
        {
            string sql = "Select * from GlobalModel Order by ID Desc";
            List<GlobalModel> list = DataHelper.Instance.ExecuteList<GlobalModel>(sql);
            if (list.Count > 0)
            {
                model = list[0];
                if (model.HS_PumpType == 1) { this.tbHSSet.Text = "1#泵"; }
                else if (model.HS_PumpType == 2) { this.tbHSSet.Text = "2#泵"; }
                else if (model.HS_PumpType == 3) { this.tbHSSet.Text = "3泵"; }
                else { this.tbHSSet.Text = "未设置"; }

                if (model.WorkType == 1) { this.tbWorkModelSet.Text = "送杆"; }
                else if (model.WorkType == 2) { this.tbWorkModelSet.Text = "排杆"; }
                else { this.tbWorkModelSet.Text = "未设置"; }

                if (model.PipeType == 1)
                {
                    if (model.PipeSize == 28) this.tbPipeType.Text = "2 7/8" + Environment.NewLine + "寸钻杆";
                    else if (model.PipeSize == 35) this.tbPipeType.Text = "3 1/2"+ Environment.NewLine+ "寸钻杆";
                    else if (model.PipeSize == 40) this.tbPipeType.Text = "4寸" + Environment.NewLine + "钻杆";
                    else if (model.PipeSize == 45) this.tbPipeType.Text = "4 1/2" + Environment.NewLine + "寸钻杆";
                    else if (model.PipeSize == 50) this.tbPipeType.Text = "5寸" + Environment.NewLine + "钻杆";
                    else if (model.PipeSize == 55) this.tbPipeType.Text = "5 1/2" + Environment.NewLine + "寸钻杆";
                    else if (model.PipeSize == 57) this.tbPipeType.Text = "5 7/8" + Environment.NewLine + "寸钻杆";
                    else if (model.PipeSize == 68) this.tbPipeType.Text = "6 5/8" + Environment.NewLine + "寸钻杆";
                }
                else if (model.PipeType == 2)
                {
                    if (model.PipeSize == 35) this.tbPipeType.Text = "3 1/2" + Environment.NewLine + "寸钻铤";
                    else if (model.PipeSize == 45) this.tbPipeType.Text = "4 1/2" + Environment.NewLine + "寸钻铤";
                    else if (model.PipeSize == 60) this.tbPipeType.Text = "6寸" + Environment.NewLine + "钻铤";
                    else if (model.PipeSize == 65) this.tbPipeType.Text = "6 1/2" + Environment.NewLine + "寸钻铤";
                    else if (model.PipeSize == 70) this.tbPipeType.Text = "7寸" + Environment.NewLine + "钻铤";
                    else if (model.PipeSize == 75) this.tbPipeType.Text = "7 1/2" + Environment.NewLine + "寸钻铤";
                    else if (model.PipeSize == 80) this.tbPipeType.Text = "8寸" + Environment.NewLine + "钻铤";
                    else if (model.PipeSize == 90) this.tbPipeType.Text = "9寸" + Environment.NewLine + "钻铤";
                    else if (model.PipeSize == 100) this.tbPipeType.Text = "10寸" + Environment.NewLine + "钻铤";
                    else if (model.PipeSize == 110) this.tbPipeType.Text = "11寸" + Environment.NewLine + "钻铤";
                }

                if (model.DesType == 1) this.tbDesSet.Text = "立根区";
                else if (model.DesType == 2) this.tbDesSet.Text = "猫道-" + Environment.NewLine + "井口";
                else if (model.DesType == 3) this.tbDesSet.Text = "猫道-" + Environment.NewLine + "鼠洞";

                if (model.SelectDrill < 16 && model.SelectDrill>0)
                {
                    this.tbDrillSelectSet.Text = "左" + model.SelectDrill.ToString();
                }
                else if (model.SelectDrill == 16)
                {
                    this.tbDrillSelectSet.Text = "左钻铤";
                }
                else if (model.SelectDrill > 16 && model.SelectDrill < 32)
                {
                    this.tbDrillSelectSet.Text = "右" + (model.SelectDrill - 16).ToString();
                }
                else if (model.SelectDrill == 32)
                {
                    this.tbDrillSelectSet.Text = "右钻铤";
                }
            }
        }

        /// <summary>
        /// 液压站设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HSSet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LinkSetWindow linkSetWindow = new LinkSetWindow();
            string sql = "Select * from GlobalModel Order by ID Desc";
            List<GlobalModel> list = DataHelper.Instance.ExecuteList<GlobalModel>(sql);
            if (list.Count > 0)
            {
                linkSetWindow.globalModel = list[0];
            }
            linkSetWindow.ShowStep(0);
            linkSetWindow.ShowDialog();
            InitAllModel();
        }
        /// <summary>
        /// 工作模式设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkModel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LinkSetWindow linkSetWindow = new LinkSetWindow();
            string sql = "Select * from GlobalModel Order by ID Desc";
            List<GlobalModel> list = DataHelper.Instance.ExecuteList<GlobalModel>(sql);
            if (list.Count > 0)
            {
                linkSetWindow.globalModel = list[0];
            }
            linkSetWindow.ShowStep(1);
            linkSetWindow.ShowDialog();
            InitAllModel();
        }
        /// <summary>
        /// 管柱选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PipeType_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LinkSetWindow linkSetWindow = new LinkSetWindow();
            string sql = "Select * from GlobalModel Order by ID Desc";
            List<GlobalModel> list = DataHelper.Instance.ExecuteList<GlobalModel>(sql);
            if (list.Count > 0)
            {
                linkSetWindow.globalModel = list[0];
            }
            linkSetWindow.ShowStep(2);
            linkSetWindow.ShowDialog();
            InitAllModel();
        }
        /// <summary>
        /// 目的地设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Des_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LinkSetWindow linkSetWindow = new LinkSetWindow();
            string sql = "Select * from GlobalModel Order by ID Desc";
            List<GlobalModel> list = DataHelper.Instance.ExecuteList<GlobalModel>(sql);
            if (list.Count > 0)
            {
                linkSetWindow.globalModel = list[0];
            }
            linkSetWindow.ShowStep(4);
            linkSetWindow.ShowDialog();
            InitAllModel();
        }
        /// <summary>
        /// 管柱选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrillSelect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LinkSetWindow linkSetWindow = new LinkSetWindow();
            string sql = "Select * from GlobalModel Order by ID Desc";
            List<GlobalModel> list = DataHelper.Instance.ExecuteList<GlobalModel>(sql);
            if (list.Count > 0)
            {
                linkSetWindow.globalModel = list[0];
            }
            linkSetWindow.ShowStep(6);
            linkSetWindow.ShowDialog();
            InitAllModel();
        }
        int pbper = 10;
        int overtime = 5; // 默认超时时间5S
        DateTime protocolSendTime { get; set; } // 协议发送时间
        bool IsSend = false; // 协议是否发送标志
        #region step 2 启动液压站
        /// <summary>
        /// step 2 启动液压站
        /// </summary>
        private bool StartHS()
        {
            if (this.pbper == 10)
            {
                this.tbOpr.Text = "检查液压站模式";
                this.pbper = 15;
                return true;
            }
            if (this.pbper == 15) //1.检查液压站操作模式 15->20
            {
                CheckHSWorkModel();
                return true;
            }
            if (this.pbper == 20 || this.pbper == 25)// 2.启动液压站 20->30
            {
                StartHSPump();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查液压站操作模式
        /// </summary>
        private void CheckHSWorkModel()
        {
            try
            {
                if (GlobalData.Instance.da.GloConfig.HydType == 0)
                {
                    this.tbOpr.Text = "未设置液压站,不允许启动联动";
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 1)
                {
                    #region 切换到司钻
                    if (GlobalData.Instance.da["771b5"] == null || GlobalData.Instance.da["771b6"] == null)
                    {
                        this.tbOpr.Text = "自研液压站数据库出错";
                        return;
                    }

                    if (GlobalData.Instance.da["771b5"].Value.Boolean && !GlobalData.Instance.da["771b6"].Value.Boolean)// 本地模式
                    {
                        this.tbOpr.Text = "液压站处于本地控制模式，请切换到司钻再启动";
                    }
                    else if (!GlobalData.Instance.da["771b5"].Value.Boolean && GlobalData.Instance.da["771b6"].Value.Boolean) // 司钻模式
                    {
                        this.tbOpr.Text = "泵准备启动";
                        this.pbper = 20;
                        IsSend = false;
                    }
                    else // 分阀箱模式
                    {
                        byte[] byteToSend = new byte[10] { 0, 19, 52, 0, 1, 0, 0, 0, 0, 0 }; // 切换司钻房协议
                        string tips = "液压站模式切换超时，重新启动";
                        CheckOverTime(byteToSend, tips, 5);
                    }
                    #endregion
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 2)
                { }
                else if (GlobalData.Instance.da.GloConfig.HydType == 3)
                {
                    if (!GlobalData.Instance.da["734b1"].Value.Boolean)
                    {
                        this.tbOpr.Text = "泵准备启动";
                        this.pbper = 20;
                        IsSend = false;
                    }
                    else
                    {
                        this.tbOpr.Text = "液压站处于待机状态，正在启用";
                        byte[] byteToSend = new byte[10] { 0, 19, 52, 0, 1, 0, 0, 0, 0, 0 };
                        string tips = "液压站启用超时，正在重新启动";
                        CheckOverTime(byteToSend, tips, 5);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("液压站配置错误！请联系售后人员");
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.ToString());
            }
        }
        /// <summary>
        /// 启动液压站主泵
        /// </summary>
        private void StartHSPump()
        {
            try
            {
                if (GlobalData.Instance.da.GloConfig.HydType == 0)
                {
                    this.tbOpr.Text = "未设置液压站,不允许启动联动";
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 1)
                {
                    #region 自研液压站启动泵
                    if (model.HS_PumpType == 0)
                    {
                        this.tbOpr.Text = "模式中未设置液压站，请返回修改";
                    }
                    else if (model.HS_PumpType == 1)
                    {
                        if (!GlobalData.Instance.da["770b3"].Value.Boolean) // 1#泵启动
                        {
                            this.tbOpr.Text = "1#泵启动成功";
                            this.pbper = 30;
                            IsSend = false;
                        }
                        else
                        {
                            byte[] byteToSend = new byte[10] { 0, 19, 42, 0, 1, 0, 0, 0, 0, 0 }; ; // 1#泵启动协议
                            string tips = "1#泵启动超时，重新启动";
                            CheckOverTime(byteToSend, tips, 5);
                        }
                    }
                    else if (model.HS_PumpType == 2)
                    {
                        if (!GlobalData.Instance.da["770b5"].Value.Boolean) // 2#泵启动
                        {
                            this.tbOpr.Text = "2#泵启动成功";
                            this.pbper = 30;
                            IsSend = false;
                        }
                        else
                        {
                            byte[] byteToSend = new byte[10] { 0, 19, 44, 0, 1, 0, 0, 0, 0, 0 }; // 2#泵启动协议
                            string tips = "2#泵启动超时，重新启动";
                            CheckOverTime(byteToSend, tips, 5);
                        }
                    }
                    else if (model.HS_PumpType == 3)
                    {
                        if (this.pbper == 20)
                        {
                            if (!GlobalData.Instance.da["770b3"].Value.Boolean) // 1#泵启动
                            {
                                this.tbOpr.Text = "1#泵启动成功,准备启动2#泵";
                                this.pbper = 25;
                                IsSend = false;
                            }
                            else
                            {
                                byte[] byteToSend = new byte[10] { 0, 19, 42, 0, 1, 0, 0, 0, 0, 0 };  // 1#泵启动协议
                                string tips = "1#泵启动超时，重新启动";
                                CheckOverTime(byteToSend, tips, 5);
                            }
                        }
                        else if (this.pbper == 25)
                        {
                            if (!GlobalData.Instance.da["770b5"].Value.Boolean) // 2#泵启动
                            {
                                this.tbOpr.Text = "2#泵启动成功";
                                this.pbper = 30;
                                IsSend = false;
                            }
                            else
                            {
                                byte[] byteToSend = new byte[10] { 0, 19, 44, 0, 1, 0, 0, 0, 0, 0 }; // 2#泵启动协议
                                string tips = "2#泵启动超时，重新启动";
                                CheckOverTime(byteToSend, tips, 5);
                            }
                        }
                    }
                    else
                    {
                        this.tbOpr.Text = "模式中设置液压站错误，请返回修改";
                    }
                    #endregion
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 2)
                { }
                else if (GlobalData.Instance.da.GloConfig.HydType == 3)
                {
                    #region JJC液压站启动泵
                    if (model.HS_PumpType == 0)
                    {
                        this.tbOpr.Text = "模式中未设置液压站，请返回修改";
                    }
                    else if (model.HS_PumpType == 1)
                    {
                        if (!GlobalData.Instance.da["733b3"].Value.Boolean) // 1#泵启动
                        {
                            this.tbOpr.Text = "1#泵启动成功，准备检查电机状态";
                            this.pbper = 30;
                            IsSend = false;
                        }
                        else
                        {
                            byte[] byteToSend = new byte[10] { 0, 19, 1, 1, 0, 0, 0, 0, 0, 0 }; // 1#泵启动协议
                            string tips = "1#泵启动超时，重新启动";
                            CheckOverTime(byteToSend, tips, 5);
                        }
                    }
                    else if (model.HS_PumpType == 2)
                    {
                        if (!GlobalData.Instance.da["733b4"].Value.Boolean) // 2#泵启动
                        {
                            this.tbOpr.Text = "2#泵启动成功，准备检查电机状态";
                            this.pbper = 30;
                            IsSend = false;
                        }
                        else
                        {
                            byte[] byteToSend = new byte[10] { 0, 19, 2, 1, 0, 0, 0, 0, 0, 0 }; // 2#泵启动协议
                            string tips = "2#泵启动超时，重新启动";
                            CheckOverTime(byteToSend, tips, 5);
                        }
                    }
                    else if (model.HS_PumpType == 3)
                    {
                        if (this.pbper == 20)
                        {
                            if (!GlobalData.Instance.da["733b3"].Value.Boolean) // 1#泵启动
                            {
                                this.tbOpr.Text = "1#泵启动成功,准备启动2#泵";
                                this.pbper = 25;
                                IsSend = false;
                            }
                            else
                            {
                                byte[] byteToSend = new byte[10] { 0, 19, 1, 1, 0, 0, 0, 0, 0, 0 }; // 1#泵启动协议
                                string tips = "1#泵启动超时，重新启动";
                                CheckOverTime(byteToSend, tips, 5);
                            }
                        }
                        else if (this.pbper == 25)
                        {
                            if (!GlobalData.Instance.da["733b4"].Value.Boolean) // 2#泵启动
                            {
                                this.tbOpr.Text = "2#泵启动成功，准备检查电机状态";
                                this.pbper = 30;
                                IsSend = false;
                            }
                            else
                            {
                                byte[] byteToSend = new byte[10] { 0, 19, 2, 1, 0, 0, 0, 0, 0, 0 }; // 2#泵启动协议
                                string tips = "2#泵启动超时，重新启动";
                                CheckOverTime(byteToSend, tips, 5);
                            }
                        }
                    }
                    else
                    {
                        this.tbOpr.Text = "模式中设置液压站错误，请返回修改";
                    }
                    #endregion
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("液压站配置错误！请联系售后人员");
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.ToString());
            }
        }
        /// <summary>
        /// 启动液压站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartHS_Click(object sender, RoutedEventArgs e)
        {
            StartHS();
        }
        /// <summary>
        /// 检查液压站
        /// </summary>
        /// <returns></returns>
        private bool CheckHS()
        {
            bool hsSuccess = true;
            try
            {
                if (GlobalData.Instance.da.GloConfig.HydType == 1)
                {
                    #region 切换到司钻
                    if (GlobalData.Instance.da["771b5"] == null || GlobalData.Instance.da["771b6"] == null)
                    {
                        hsSuccess = false;
                    }
                    if (!GlobalData.Instance.da["771b5"].Value.Boolean && GlobalData.Instance.da["771b6"].Value.Boolean) // 司钻模式
                    {
                        hsSuccess = true;
                    }
                    else hsSuccess = false;
                    #endregion
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 3)
                {
                    if (!GlobalData.Instance.da["734b1"].Value.Boolean)
                    {
                        hsSuccess = true;
                    }
                    else
                    {
                        hsSuccess = false;
                    }
                }
                else
                {
                    hsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.ToString());
            }
            return hsSuccess;
            
        }
        #endregion

        #region Step 3 检查设备状态
        /// <summary>
        ///  Step 3 检查设备状态
        /// </summary>
        private bool CheckDeviceStatus()
        {
            // 1.检查二层台电机是否需要使能
            if (this.pbper == 30)
            {
                SFMonitorEnable();
                return true;
            }
            // 2.检查钻台面电机是否需要使能
            if (this.pbper == 35)
            {
                DRMonitorEnable();
                return true;
            }
            //// 3.检查铁钻工系统压力
            //if (this.pbper.Value == 40)
            //{
            //    if (GlobalData.Instance.da["SIRSelfSysPress"].Value.Int16 / 10 < 1)
            //    {
            //        this.tbCurTip.Text = "铁钻工压力过低，请检查液压站运行状态，再启动";
            //    }
            //    else
            //    {
            //        this.tbCurTip.Text = "铁钻工系统压力正常";
            //        this.pbper.Value = 45;
            //    }
            //    return true;
            //}
            // 4.检查二层台/钻台面回零情况
            if (this.pbper == 40)
            {
                SFAndDRTurnToZero();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 二层台电机使能
        /// </summary>
        private void SFMonitorEnable()
        {
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                #region 自研二层台
                if (!GlobalData.Instance.da["carMotorWorkStatus"].Value.Boolean
                       && !GlobalData.Instance.da["armMotorWorkStatus"].Value.Boolean
                       && !GlobalData.Instance.da["rotateMotorWorkStatus"].Value.Boolean)
                {
                    this.tbOpr.Text = "二层台电机正常，准备检查钻台面电机";
                    this.pbper = 35;
                    IsSend = false;
                }
                else
                {
                    this.tbOpr.Text = "二层台电机使能中";
                    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 6, 2 }); // 二层台使能协议
                    string tips = "二层台使能超时，重新使能";
                    CheckOverTime(byteToSend, tips, 5);
                }
                #endregion
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("未配置二层台，请联系售后进行配置");
            }
        }
        /// <summary>
        /// 钻台面电机使能
        /// </summary>
        private void DRMonitorEnable()
        {
            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                #region 自研钻台面
                if (GlobalData.Instance.da["324b1"].Value.Boolean && GlobalData.Instance.da["324b5"].Value.Boolean)
                {
                    this.tbOpr.Text = "钻台面电机正常，准备检查回零状态";
                    this.pbper = 40;
                    IsSend = false;
                }
                else
                {
                    this.tbOpr.Text = "钻台面电机使能中";
                    byte[] sendOne = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
                    byte[] sendTwo = GlobalData.Instance.SendToDR(new List<byte> { 6, 2 }); // 二层台使能协议
                    string tips = "钻台面使能超时，重新使能";
                    CheckOverTime(sendOne, sendTwo, tips, 5);
                }
                #endregion
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 2)
            {
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("未配置钻台面，请联系售后进行配置");
            }
        }
        /// <summary>
        /// 二层台/钻台面电机使能
        /// </summary>
        private void SFAndDRTurnToZero()
        {
            bool sfZero = false;
            bool drZero = false;
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                if (GlobalData.Instance.da["carMotorRetZeroStatus"].Value.Boolean && GlobalData.Instance.da["armMotorRetZeroStatus"].Value.Boolean
                    && GlobalData.Instance.da["rotateMotorRetZeroStatus"].Value.Boolean)
                {
                    sfZero = true;
                }
                else sfZero = false;
            }
            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                if (GlobalData.Instance.da["324b0"].Value.Boolean && GlobalData.Instance.da["324b4"].Value.Boolean)
                {
                    drZero = true;
                }
                else sfZero = false;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 2)
            { }

            if (sfZero && drZero)
            {
                this.tbOpr.Text = "二层台/钻台面已经回零，准备设置管柱类型";
                this.pbper = 50;
                IsSend = false;
            }
            else
            {
                this.tbOpr.Text = "二层台/钻台面回零中";
                byte[] sendOne = GlobalData.Instance.SendByte(new List<byte> { 13, 4 });
                byte[] sendTwo = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
                byte[] sendThree = GlobalData.Instance.SendToDR(new List<byte> { 13, 4 });
                string tips = "二层台/钻台面回零超时，重新回零";
                CheckOverTime(sendOne, sendTwo, sendThree, tips, 20);
            }
        }
        /// <summary>
        /// 设备回零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTurnToZore_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnTurnToZore.Background.ToString() == "#FFF5C244")
            {
                //MessageBox.Show("二层台和钻台面准备回零，注意安全!", "提示", MessageBoxButton.YesNo);
                CheckDeviceStatus();
            }
        }
        /// <summary>
        /// 检查电机使能/回零
        /// </summary>
        /// <returns></returns>
        private bool CheckMonitor()
        {
            bool hsSuccess = true;
            // 检查二层台
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            { hsSuccess = false; }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                #region 自研二层台
                if (!GlobalData.Instance.da["carMotorWorkStatus"].Value.Boolean
                       && !GlobalData.Instance.da["armMotorWorkStatus"].Value.Boolean
                       && !GlobalData.Instance.da["rotateMotorWorkStatus"].Value.Boolean)
                {
                    hsSuccess = true;
                }
                else
                {
                    hsSuccess = false;
                }
                #endregion
            }
            else
            {
                hsSuccess = false;
            }
            // 检查钻台面
            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            { hsSuccess = false; }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                #region 自研钻台面
                if (GlobalData.Instance.da["324b1"].Value.Boolean && GlobalData.Instance.da["324b5"].Value.Boolean)
                {
                    hsSuccess = true;
                }
                else
                {
                    hsSuccess = false;
                }
                #endregion
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 2)
            {
            }
            else
            {
                hsSuccess = false;
            }

            bool sfZero = false;
            bool drZero = false;
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                if (GlobalData.Instance.da["carMotorRetZeroStatus"].Value.Boolean && GlobalData.Instance.da["armMotorRetZeroStatus"].Value.Boolean
                    && GlobalData.Instance.da["rotateMotorRetZeroStatus"].Value.Boolean)
                {
                    sfZero = true;
                }
                else sfZero = false;
            }
            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                if (GlobalData.Instance.da["324b0"].Value.Boolean && GlobalData.Instance.da["324b4"].Value.Boolean)
                {
                    drZero = true;
                }
                else sfZero = false;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 2)
            { }

            if (sfZero && drZero)
            {
                hsSuccess = true;
            }
            else
            {
                hsSuccess = false;
            }
            return hsSuccess;

        }
        #endregion

        #region Step 4 互锁确认
        /// <summary>
        /// 互锁确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLockConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnTurnToZore.Background.ToString() == "#FFF5C244")
            {
                if (this.pbper == 50)
                {
                    string msg = string.Empty;
                    if (!IsTopAllLock) msg += "顶驱,";
                    if (!IsElevatorAllLock) msg += "吊卡,";
                    if (!IsHookAllLock) msg += "大钩,";
                    if (!IsSfAllLock) msg += "二层台,";
                    if (!IsDrAllLock) msg += "钻台面,";
                    if (!IsSirAllLock) msg += "铁钻工,";
                    if (!IsCatAllLock) msg += "猫道,";
                    if (!IsPreventBoxAllLock) msg += "防喷盒,";
                    if (!IsKavaAllLock) msg += "卡瓦,";
                    if (msg != string.Empty)
                    {
                        msg = "系统中存在" + msg + "互锁已解除，可能存在安全问题，确认继续开启联动?";
                        MessageBoxResult result = MessageBox.Show(msg, "提示", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            TotalLock = true;
                            this.pbper = 60;
                        }
                        else TotalLock = false;
                    }
                    else
                    {
                        this.pbper = 60;
                    }
                }
            }
        }
        private bool CheckLock()
        {
            bool lockSuccess = true;
            if ((IsTopAllLock && IsElevatorAllLock && IsHookAllLock && IsSfAllLock && IsDrAllLock
                && IsSirAllLock && IsCatAllLock && IsPreventBoxAllLock && IsKavaAllLock) || TotalLock)
            {
                lockSuccess = true;
            }
            else
            {
                lockSuccess = false;
            }
            return lockSuccess;
        }
        #endregion

        #region Step5 参数确认
        /// <summary>
        /// Step 4 选择目的地和钻杆类型
        /// </summary>
        private bool StartPipeTypeAndDes()
        {
            int pipeType = -1;
            // 尺寸>60，标志为特殊 或 尺寸<60,标志为普通 为钻杆
            if ((GlobalData.Instance.da["drillPipeType"].Value.Byte >= 60 && GlobalData.Instance.da["103b7"].Value.Boolean)
                || (GlobalData.Instance.da["drillPipeType"].Value.Byte < 60 && !GlobalData.Instance.da["103b7"].Value.Boolean))
            {
                pipeType = 1;
            }
            // 尺寸小于60，标志为特殊 或 尺寸>60,标志为普通 为钻铤
            if ((GlobalData.Instance.da["drillPipeType"].Value.Byte < 60 && GlobalData.Instance.da["103b7"].Value.Boolean)
                || (GlobalData.Instance.da["drillPipeType"].Value.Byte >= 60 && !GlobalData.Instance.da["103b7"].Value.Boolean))
            {
                pipeType = 2;
            }
            // 1.设置二层台钻杆
            if (this.pbper == 60)
            {
                SFPipeSet(pipeType);
                return true;
            }
            // 2.设置钻台面管柱类型
            if (this.pbper == 65)
            {
                DRPipeSet();
                return true;
            }
            // 3.设置钻台面的目的地
            if (this.pbper == 70)
            {
                DRDesTypeSet();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置二层台管柱类型
        /// </summary>
        /// <param name="pipeType"></param>
        private void SFPipeSet(int pipeType)
        {
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                // 管柱类型和管柱尺寸都正确
                if (this.model.PipeType == pipeType && this.model.PipeSize == GlobalData.Instance.da["drillPipeType"].Value.Byte)
                {
                    this.tbOpr.Text = "二层台管柱类型设置成功，准备设置钻台面管柱类型";
                    this.pbper = 65;
                    IsSend = false;
                }
                else
                {
                    byte[] byteToSend;
                    if ((this.model.PipeType == 1 && this.model.PipeSize >= 60) || (this.model.PipeType == 2 && this.model.PipeSize < 60))
                    {
                        byteToSend = new byte[] { 80, 1, 3, (byte)this.model.PipeSize, 0, 0, 1, 0, 0, 0 };
                    }
                    else
                    {
                        byteToSend = new byte[] { 80, 1, 3, (byte)this.model.PipeSize, 0, 0, 0, 0, 0, 0 };
                    }
                    string tips = "二层台管柱设置超时，正在重新设置";
                    CheckOverTime(byteToSend, tips, 5);
                }
            }
        }
        /// <summary>
        /// 设置钻台面管柱类型
        /// </summary>
        private void DRPipeSet()
        {
            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                if (GlobalData.Instance.da["drdrillPipeType"].Value.Byte == this.model.PipeSize)
                {
                    this.tbOpr.Text = "钻台面管柱类型设置成功，准备设置目的地";
                    this.pbper = 70;
                    IsSend = false;
                }
                else
                {
                    byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte>() { 3, (byte)this.model.PipeSize });
                    string tips = "钻台面管柱设置超时，正在重新设置";
                    CheckOverTime(byteToSend, tips, 5);
                }
            }
        }
        /// <summary>
        /// 设置目的地
        /// </summary>
        private void DRDesTypeSet()
        {
            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                if (this.model.DesType == GlobalData.Instance.da["drDes"].Value.Byte)
                {
                    this.tbOpr.Text = "目的地设置成功，准备设置操作模式";
                    this.pbper = 75;
                    IsSend = false;
                }
                else
                {
                    byte[] byteToSend = { 80, 33, 11, (byte)this.model.DesType, 0, 0, 0, 0, 0, 0 };
                    string tips = "设置目的地超时，正在重新设置";
                    CheckOverTime(byteToSend, tips, 5);
                }
            }
        }
        /// <summary>
        /// step 5 切换到自动模式
        /// </summary>
        private bool TurnToAuto()
        {
            if (this.pbper == 75)
            {
                AutoSet();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置自动模式
        /// </summary>
        private void AutoSet()
        {
            bool sfAuto = false;
            bool drAuto = false;
            bool sirAuto = false;
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            {
                sfAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                sfAuto = GlobalData.Instance.da["operationModel"].Value.Byte == 5 ? true : false;
            }

            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            {
                drAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                drAuto = GlobalData.Instance.da["droperationModel"].Value.Byte == 5 ? true : false;
            }

            if (GlobalData.Instance.da.GloConfig.SIRType == 0)
            {
                sirAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 1)
            {
                sirAuto = GlobalData.Instance.da["SIRSelfOperModel"].Value.Byte == 2 ? true : false;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 2)
            {
                sirAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 3)
            {
                sirAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 4)
            {
                sirAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 5)
            {
                sirAuto = true;
            }

            if (sfAuto && drAuto && sirAuto)
            {
                this.tbOpr.Text = "设备已出于自动模式，准备设置工作模式";
                this.pbper = 80;
                IsSend = false;
            }
            else
            {
                this.tbOpr.Text = "切换自动模式中";
                byte[] sfbyteToSend;// 二层台
                byte[] drbyteToSend;// 钻台面
                byte[] sirbyteToSend;// 铁钻工
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 5 });
                drbyteToSend = new byte[10] { 1, 32, 3, 31, 0, 0, 0, 0, 0, 0 };

                string tips = "切换自动模式超时，正在重新切换";
                if (!sfAuto) tips = "二层台切换自动模式失败，请确认二层台模式";
                else if (!drAuto) tips = "铁钻工切换自动模式失败，请确认二层台模式";
                if (GlobalData.Instance.da.GloConfig.SIRType == 1)
                {
                    sirbyteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
                    CheckOverTime(sfbyteToSend, drbyteToSend, sirbyteToSend, tips, 5);
                }
                else
                {
                    CheckOverTime(sfbyteToSend, drbyteToSend, tips, 5);
                }
            }
        }
        /// <summary>
        ///  step 6 选择工作模式
        /// </summary>
        private bool SelectWorkModel()
        {
            if (this.pbper == 80)
            {
                WorkModelSet();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 工作模式设备
        /// </summary>
        private void WorkModelSet()
        {
            int sfWorkModel = 0;
            int drWorkModel = 0;
            int sirWorkModel = 0;
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                sfWorkModel = GlobalData.Instance.da["workModel"].Value.Byte;
            }

            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            {
                drWorkModel = this.model.WorkType;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                drWorkModel = GlobalData.Instance.da["drworkModel"].Value.Byte;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 2)
            { }

            if (GlobalData.Instance.da.GloConfig.SIRType == 0)
            {
                sirWorkModel = this.model.WorkType;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 1)
            {
                if (GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 1)
                {
                    if (GlobalData.Instance.da["841b3"].Value.Boolean
                    && GlobalData.Instance.da["841b5"].Value.Boolean)
                    {
                        sirWorkModel = 1;
                    }
                    else
                    {
                        sirWorkModel = 0;
                        this.tbOpr.Text = "铁钻工远程切换上扣模式失败，请去本地切换";
                        return;
                    }

                }
                else if (GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 2)
                {
                    if (GlobalData.Instance.da["841b4"].Value.Boolean
                    && GlobalData.Instance.da["841b6"].Value.Boolean)
                    {
                        sirWorkModel = 2;
                    }
                    else
                    {
                        sirWorkModel = 0;
                        this.tbOpr.Text = "铁钻工远程切换卸扣模式失败，请去本地切换";
                        return;
                    }
                }
                else
                {
                    sirWorkModel = 0;
                }
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 2)
            {
                sirWorkModel = this.model.WorkType;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 3)
            { }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 4)
            { }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 5)
            { }

            if (this.model.WorkType == 1 && sfWorkModel == 1 && drWorkModel == 1 && sirWorkModel == 1) // 送杆
            {
                this.tbOpr.Text = "工作模式设置完成，准备选择指梁";
                this.pbper = 85;
                IsSend = false;
                return;
            }
            else if (this.model.WorkType == 2 && sfWorkModel == 2 && drWorkModel == 2 && sirWorkModel == 2) //排杆
            {
                this.tbOpr.Text = "工作模式设置完成，准备选择指梁";
                this.pbper = 85;
                IsSend = false;
                return;
            }
            else
            {
                string tips = "工作模式切换超时，重新切换中";
                byte[] sfbyteToSend;// 二层台
                byte[] drbyteToSend;// 钻台面
                byte[] sirbyteToSend;// 铁钻工
                if (this.model.WorkType == 1)
                {
                    sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
                    drbyteToSend = new byte[10] { 1, 32, 4, 41, 0, 0, 0, 0, 0, 0 };
                    if (GlobalData.Instance.da.GloConfig.SIRType == 1)
                    {
                        sirbyteToSend = new byte[10] { 23, 17, 2, 1, 0, 0, 0, 0, 0, 0 };
                        CheckOverTime(sfbyteToSend, drbyteToSend, sirbyteToSend, tips, 10);
                    }
                    else
                    {
                        CheckOverTime(sfbyteToSend, drbyteToSend, tips, 10);
                    }
                }
                else if (this.model.WorkType == 2)
                {
                    sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
                    drbyteToSend = new byte[10] { 1, 32, 4, 40, 0, 0, 0, 0, 0, 0 };
                    if (GlobalData.Instance.da.GloConfig.SIRType == 1)
                    {
                        sirbyteToSend = new byte[10] { 23, 17, 2, 2, 0, 0, 0, 0, 0, 0 };
                        CheckOverTime(sfbyteToSend, drbyteToSend, sirbyteToSend, tips, 10);
                    }
                    else
                    {
                        CheckOverTime(sfbyteToSend, drbyteToSend, tips, 10);
                    }
                }
                else
                {
                    this.tbOpr.Text = "工作模式设置有误，请返回模式设置中重新设置";
                }

            }
        }
        /// <summary>
        ///  step 7 选择指梁
        /// </summary>
        private bool SelectedDrill()
        {
            if (this.pbper == 85)
            {
                SelectDrillSet();
                return true;
            }
            return false;
        }
        int autoSelectDrill = 1; // 自动选择指梁，默认选择左1
        /// <summary>
        /// 设置选择的指梁
        /// </summary>
        private void SelectDrillSet()
        {
            int sfSelectDrillNum = 0;
            int drSelectDrillNum = 0;
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                sfSelectDrillNum = GlobalData.Instance.da["pcFingerBeamNumberFeedback"].Value.Byte;
            }

            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                drSelectDrillNum = GlobalData.Instance.da["drPCSelectDrill"].Value.Byte;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 2)
            { }

            if (this.model.SelectDrill > 0) // 手动选择指梁
            {
                if (this.model.SelectDrill == sfSelectDrillNum && this.model.SelectDrill == drSelectDrillNum)
                {
                    if (sfSelectDrillNum < 16) this.tbOpr.Text = "自动选择左" + sfSelectDrillNum + "指梁";
                    else if (sfSelectDrillNum == 16) this.tbOpr.Text = "自动选择左钻铤";
                    else if (sfSelectDrillNum > 16 && sfSelectDrillNum < 32) this.tbOpr.Text = "自动选择右" + (sfSelectDrillNum - 16) + "指梁";
                    else this.tbOpr.Text = "自动选择右钻铤";
                    this.pbper = 90;
                    IsSend = false;
                }
                else
                {
                    string tips = "选择指梁失败，正在重新选择";
                    if (!IsSend)
                    {
                        protocolSendTime = DateTime.Now;
                        IsSend = true;
                        overtime = 1;
                        byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 5, (byte)this.model.SelectDrill });
                        GlobalData.Instance.da.SendBytes(byteToSend);
                        Thread.Sleep(50);
                        byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 5, (byte)this.model.SelectDrill });
                        GlobalData.Instance.da.SendBytes(byteToSend);
                        //if (this.model.PipeType == 1)
                        //{
                        //    autoSelectDrill++;
                        //    if (autoSelectDrill == 32)
                        //    {
                        //        this.tbOpr.Text = "自动选择指梁失败，请人工选择";
                        //    }
                        //    if (autoSelectDrill > GlobalData.Instance.Rows && autoSelectDrill < 17) autoSelectDrill = 17;
                        //    //if (autoSelectDrill > 31) autoSelectDrill = 31;
                        //}
                        //else if (this.model.PipeType == 2)
                        //{
                        //    if (autoSelectDrill == 32)
                        //    {
                        //        this.tbOpr.Text = "自动选择指梁失败，请人工选择";
                        //    }
                        //    autoSelectDrill = 32;
                        //}
                    }
                    else
                    {
                        if ((DateTime.Now.Ticks - protocolSendTime.Ticks) / 10000000 >= overtime)
                        {
                            IsSend = false;
                            this.tbOpr.Text = tips;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 参数确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnParamConfirm_Click(object sender, RoutedEventArgs e)
        {
            StartPipeTypeAndDes();
            AutoSet();
            SelectWorkModel();
            SelectedDrill();
        }

        private bool CheckParam()
        {
            bool lockSuccess = true;
            #region 钻杆类型是否正确
            int pipeType = -1;
            // 尺寸>60，标志为特殊 或 尺寸<60,标志为普通 为钻杆
            if ((GlobalData.Instance.da["drillPipeType"].Value.Byte >= 60 && GlobalData.Instance.da["103b7"].Value.Boolean)
                || (GlobalData.Instance.da["drillPipeType"].Value.Byte < 60 && !GlobalData.Instance.da["103b7"].Value.Boolean))
            {
                pipeType = 1;
            }
            // 尺寸小于60，标志为特殊 或 尺寸>60,标志为普通 为钻铤
            if ((GlobalData.Instance.da["drillPipeType"].Value.Byte < 60 && GlobalData.Instance.da["103b7"].Value.Boolean)
                || (GlobalData.Instance.da["drillPipeType"].Value.Byte >= 60 && !GlobalData.Instance.da["103b7"].Value.Boolean))
            {
                pipeType = 2;
            }
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            {
                lockSuccess = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                // 管柱类型和管柱尺寸都正确
                if (this.model.PipeType == pipeType && this.model.PipeSize == GlobalData.Instance.da["drillPipeType"].Value.Byte)
                {
                    lockSuccess = true;
                }
                else
                {
                    return false;
                }
            }
            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            {
                lockSuccess = true;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                if (GlobalData.Instance.da["drdrillPipeType"].Value.Byte == this.model.PipeSize)
                {
                    lockSuccess = true;
                }
                else
                {
                    return false;
                }
            }
            #endregion
            #region 目的地是否正确
            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            {
                lockSuccess = true;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                if (this.model.DesType == GlobalData.Instance.da["drDes"].Value.Byte)
                {
                    lockSuccess = true;
                }
                else
                {
                    return false;
                }
            }
            #endregion
            #region 是否处于自动模式
            bool sfAuto = false;
            bool drAuto = false;
            bool sirAuto = false;
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            {
                sfAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                sfAuto = GlobalData.Instance.da["operationModel"].Value.Byte == 5 ? true : false;
            }

            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            {
                drAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                drAuto = GlobalData.Instance.da["droperationModel"].Value.Byte == 5 ? true : false;
            }

            if (GlobalData.Instance.da.GloConfig.SIRType == 0)
            {
                sirAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 1)
            {
                sirAuto = GlobalData.Instance.da["SIRSelfOperModel"].Value.Byte == 2 ? true : false;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 2)
            {
                sirAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 3)
            {
                sirAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 4)
            {
                sirAuto = true;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 5)
            {
                sirAuto = true;
            }

            if (sfAuto && drAuto && sirAuto)
            {
                lockSuccess = true;
            }
            else
            {
                return false;
            }
            #endregion
            #region 工作模式是否正确
            int sfWorkModel = 0;
            int drWorkModel = 0;
            int sirWorkModel = 0;
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                sfWorkModel = GlobalData.Instance.da["workModel"].Value.Byte;
            }

            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            {
                drWorkModel = this.model.WorkType;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                drWorkModel = GlobalData.Instance.da["drworkModel"].Value.Byte;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 2)
            { }

            if (GlobalData.Instance.da.GloConfig.SIRType == 0)
            {
                sirWorkModel = this.model.WorkType;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 1)
            {
                if (GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 1)
                {
                    if (GlobalData.Instance.da["841b3"].Value.Boolean
                    && GlobalData.Instance.da["841b5"].Value.Boolean)
                    {
                        sirWorkModel = 1;
                    }
                    else
                    {
                        sirWorkModel = 0;
                    }

                }
                else if (GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte == 2)
                {
                    if (GlobalData.Instance.da["841b4"].Value.Boolean
                    && GlobalData.Instance.da["841b6"].Value.Boolean)
                    {
                        sirWorkModel = 2;
                    }
                    else
                    {
                        sirWorkModel = 0;
                    }
                }
                else
                {
                    sirWorkModel = 0;
                }
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 2)
            {
                sirWorkModel = this.model.WorkType;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 3)
            { }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 4)
            { }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 5)
            { }

            if (this.model.WorkType == 1 && sfWorkModel == 1 && drWorkModel == 1 && sirWorkModel == 1) // 送杆
            {
                lockSuccess = true;
            }
            else if (this.model.WorkType == 2 && sfWorkModel == 2 && drWorkModel == 2 && sirWorkModel == 2) //排杆
            {
                lockSuccess = true;
            }
            else
            {
                return false;

            }
            #endregion
            #region 指梁是否正确
            int sfSelectDrillNum = 0;
            int drSelectDrillNum = 0;
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                sfSelectDrillNum = GlobalData.Instance.da["pcFingerBeamNumberFeedback"].Value.Byte;
            }

            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                drSelectDrillNum = GlobalData.Instance.da["drPCSelectDrill"].Value.Byte;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 2)
            { }

            if (this.model.SelectDrill > 0) // 手动选择指梁
            {
                if (this.model.SelectDrill == sfSelectDrillNum && this.model.SelectDrill == drSelectDrillNum)
                {
                    lockSuccess = true;
                }
                else
                {
                    return false;
                }
            }
            #endregion

            return lockSuccess;
        }
        #endregion
        /// <summary>
        /// 显示联动启动情况
        /// </summary>
        private void ShowLinkStatus()
        {
            // 液压站情况
            if(CheckHS()) this.btnStartHS.Background = (Brush)bc.ConvertFrom("#5DBADC");
            else this.btnStartHS.Background = (Brush)bc.ConvertFrom("#F5C244");
            // 电机回零/使能情况
            if (CheckMonitor()) this.btnTurnToZore.Background = (Brush)bc.ConvertFrom("#5DBADC");
            else this.btnTurnToZore.Background = (Brush)bc.ConvertFrom("#F5C244");
            // 互锁情况
            if (CheckLock()) this.btnLockConfirm.Background = (Brush)bc.ConvertFrom("#5DBADC");
            else this.btnLockConfirm.Background = (Brush)bc.ConvertFrom("#F5C244");
            // 参数情况
            if (CheckParam()) this.btnParamConfirm.Background = (Brush)bc.ConvertFrom("#5DBADC");
            else this.btnParamConfirm.Background = (Brush)bc.ConvertFrom("#F5C244");
            // 联动情况
            if (GlobalData.Instance.da["460b0"].Value.Boolean) this.btnLinkOpen.Background = (Brush)bc.ConvertFrom("#5DBADC");
            else this.btnLinkOpen.Background = (Brush)bc.ConvertFrom("#F5C244");
        }

        /// <summary>
        /// 检查协议是否超时
        /// </summary>
        /// <param name="byteToSend">发送协议</param>
        /// <param name="tips">超时提示</param>
        /// <param name="tick">超时时间</param>
        private void CheckOverTime(byte[] byteToSend, string tips, int tick)
        {
            if (!IsSend) // 未发送协议，发送协议，记录发送时间
            {
                protocolSendTime = DateTime.Now;
                IsSend = true;
                overtime = tick;
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else // 已经发送协议，但是状态没有切换过来，检查协议发送时间，超时则重新发送
            {
                if ((DateTime.Now.Ticks - protocolSendTime.Ticks) / 10000000 >= overtime)
                {
                    IsSend = false;
                    this.tbOpr.Text = tips;
                }
            }
        }

        /// <summary>
        /// 检查协议是否超时
        /// </summary>
        /// <param name="byteToSend">发送协议</param>
        /// <param name="tips">超时提示</param>
        private void CheckOverTime(byte[] byteToSend, byte[] byteToSendTwo, string tips, int ticks)
        {
            if (!IsSend) // 未发送协议，发送协议，记录发送时间
            {
                protocolSendTime = DateTime.Now;
                IsSend = true;
                overtime = ticks;
                GlobalData.Instance.da.SendBytes(byteToSend);
                Thread.Sleep(50);
                GlobalData.Instance.da.SendBytes(byteToSendTwo);
            }
            else // 已经发送协议，但是状态没有切换过来，检查协议发送时间，超时则重新发送
            {
                if ((DateTime.Now.Ticks - protocolSendTime.Ticks) / 10000000 >= overtime)
                {
                    IsSend = false;
                    this.tbOpr.Text = tips;
                }
            }
        }

        /// <summary>
        /// 检查协议是否超时
        /// </summary>
        /// <param name="byteToSend">发送协议</param>
        /// <param name="tips">超时提示</param>
        private void CheckOverTime(byte[] byteToSend, byte[] byteToSendTwo, byte[] byteToSendThree, string tips, int tick)
        {
            if (!IsSend) // 未发送协议，发送协议，记录发送时间
            {
                protocolSendTime = DateTime.Now;
                IsSend = true;
                overtime = tick;
                GlobalData.Instance.da.SendBytes(byteToSend);
                Thread.Sleep(50);
                GlobalData.Instance.da.SendBytes(byteToSendTwo);
                Thread.Sleep(50);
                GlobalData.Instance.da.SendBytes(byteToSendThree);
            }
            else // 已经发送协议，但是状态没有切换过来，检查协议发送时间，超时则重新发送
            {
                if ((DateTime.Now.Ticks - protocolSendTime.Ticks) / 10000000 >= overtime)
                {
                    IsSend = false;
                    this.tbOpr.Text = tips;
                }
            }
        }
        /// <summary>
        /// 查看互锁状态
        /// </summary>
        private void ShowLockStatus()
        {
            if (GlobalData.Instance.da.GloConfig.TopType == 0)
            {
                this.TopLock.LampType = 0;
                IsTopAllLock = true;
            }
            else
            {
                //顶驱互锁是否有解除
                if (!GlobalData.Instance.da["577b2"].Value.Boolean && !GlobalData.Instance.da["583b4"].Value.Boolean && !GlobalData.Instance.da["583b6"].Value.Boolean)
                {
                    this.TopLock.LampType = 1;
                    IsTopAllLock = true;
                }
                else
                {
                    this.TopLock.LampType = 3;
                    IsTopAllLock = false;
                }
            }

            if (GlobalData.Instance.da.GloConfig.ElevatorType == 0)
            {
                this.ElevatorLock.LampType = 0;
                IsElevatorAllLock = true;
            }
            else
            {
                //吊卡互锁是否有解除
                if (!GlobalData.Instance.da["576b6"].Value.Boolean && !GlobalData.Instance.da["577b0"].Value.Boolean && !GlobalData.Instance.da["583b2"].Value.Boolean)
                {
                    this.ElevatorLock.LampType = 1;
                    IsElevatorAllLock = true;
                }
                else
                {
                    this.ElevatorLock.LampType = 3;
                    IsElevatorAllLock = false;
                }
            }
            if (GlobalData.Instance.da.GloConfig.HookType == 0)
            {
                this.BigHookLock.LampType = 0;
                IsHookAllLock = true;
            }
            else
            { 
                //大钩互锁是否有解除
                if (!GlobalData.Instance.da["577b4"].Value.Boolean && !GlobalData.Instance.da["577b6"].Value.Boolean && !GlobalData.Instance.da["578b0"].Value.Boolean
                    && !GlobalData.Instance.da["577b2"].Value.Boolean && !GlobalData.Instance.da["577b4"].Value.Boolean && !GlobalData.Instance.da["578b6"].Value.Boolean)
                {
                    this.BigHookLock.LampType = 1;
                    IsHookAllLock = true;
                }
                else
                {
                    this.BigHookLock.LampType = 3;
                    IsHookAllLock = false;
                }
            }
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            {
                this.sfLock.LampType = 0;
                IsSfAllLock = true;
            }
            else
            {
                //二层台互锁是否有解锁
                if (!GlobalData.Instance.da["579b0"].Value.Boolean && !GlobalData.Instance.da["579b2"].Value.Boolean && !GlobalData.Instance.da["579b4"].Value.Boolean)
                {
                    this.sfLock.LampType = 1;
                    IsSfAllLock = true;
                }
                else
                {
                    this.sfLock.LampType = 3;
                    IsSfAllLock = false;
                }
            }
            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            {
                this.drLock.LampType = 0;
                IsDrAllLock = true;
            }
            else
            {
                //钻台面互锁是否有解锁
                if (!GlobalData.Instance.da["579b0"].Value.Boolean && !GlobalData.Instance.da["580b0"].Value.Boolean && !GlobalData.Instance.da["580b2"].Value.Boolean
                    && !GlobalData.Instance.da["580b4"].Value.Boolean && !GlobalData.Instance.da["580b6"].Value.Boolean)
                {
                    this.drLock.LampType = 1;
                    IsDrAllLock = true;
                }
                else
                {
                    this.drLock.LampType = 3;
                    IsDrAllLock = false;
                }
            }
            if (GlobalData.Instance.da.GloConfig.SIRType == 0)
            {
                this.sirLock.LampType = 0;
                IsSirAllLock = true;
            }
            else
            {
                //铁钻工互锁是否有解锁
                if (!GlobalData.Instance.da["581b0"].Value.Boolean && !GlobalData.Instance.da["581b2"].Value.Boolean && !GlobalData.Instance.da["581b4"].Value.Boolean
                    && !GlobalData.Instance.da["581b6"].Value.Boolean)
                {
                    this.sirLock.LampType = 1;
                    IsSirAllLock = true;
                }
                else
                {
                    this.sirLock.LampType = 3;
                    IsSirAllLock = false;
                }
            }
            if (GlobalData.Instance.da.GloConfig.PreventBoxType == 0)
            { 
                this.PreventBoxLock.LampType = 0;
                IsPreventBoxAllLock = true;
            }
            else
            {
                //防喷盒互锁是否有解锁
                if (!GlobalData.Instance.da["582b0"].Value.Boolean && !GlobalData.Instance.da["582b2"].Value.Boolean && !GlobalData.Instance.da["582b4"].Value.Boolean)
                {
                    this.PreventBoxLock.LampType = 1;
                    IsPreventBoxAllLock = true;
                }
                else
                {
                    this.PreventBoxLock.LampType = 3;
                    IsPreventBoxAllLock = false;
                }
            }
            if (GlobalData.Instance.da.GloConfig.KavaType == 0)
            {
                this.KavaLock.LampType = 0;
                IsKavaAllLock = true;
            }
            else
            {
                //卡瓦互锁是否有解锁
                if (!GlobalData.Instance.da["576b0"].Value.Boolean && !GlobalData.Instance.da["576b2"].Value.Boolean && !GlobalData.Instance.da["576b4"].Value.Boolean
                     && !GlobalData.Instance.da["583b0"].Value.Boolean)
                {
                    this.KavaLock.LampType = 1;
                    IsKavaAllLock = true;
                }
                else
                {
                    this.KavaLock.LampType = 3;
                    IsKavaAllLock = false;
                }
            }
            if (GlobalData.Instance.da.GloConfig.CatType == 0)
            {
                this.catLock.LampType = 0;
                IsCatAllLock = true;
            }
            else
            {
                //猫道互锁是否有解锁
                if (!GlobalData.Instance.da["582b6"].Value.Boolean)
                {
                    this.catLock.LampType = 1;
                    IsCatAllLock = true;
                }
                else
                {
                    this.catLock.LampType = 3;
                    IsCatAllLock = false;
                }
            }
        }
        /// <summary>
        /// 手性图表显示
        /// </summary>
        private void ShowHand()
        {
            if (this.tbHSSet.Text == "未设置")
            {
                if (this.tbHSSetHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbHSSetHand.Visibility = Visibility.Collapsed;
                else if (this.tbHSSetHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbHSSetHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbHSSetHand.Visibility = Visibility.Collapsed;
            }

            if (this.tbWorkModelSet.Text == "未设置")
            {
                if (this.tbWorkModelSetHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbWorkModelSetHand.Visibility = Visibility.Collapsed;
                else if (this.tbWorkModelSetHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbWorkModelSetHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbWorkModelSetHand.Visibility = Visibility.Collapsed;
            }

            if (this.tbPipeType.Text == "未设置")
            {
                if (this.tbPipeSetHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbPipeSetHand.Visibility = Visibility.Collapsed;
                else if (this.tbPipeSetHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbPipeSetHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbPipeSetHand.Visibility = Visibility.Collapsed;
            }

            if (this.tbDesSet.Text == "未设置")
            {
                if (this.tbDesSetHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbDesSetHand.Visibility = Visibility.Collapsed;
                else if (this.tbDesSetHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbDesSetHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbDesSetHand.Visibility = Visibility.Collapsed;
            }

            if (this.tbDrillSelectSet.Text == "未设置")
            {
                if (this.tbDrillSetHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbDrillSetHand.Visibility = Visibility.Collapsed;
                else if (this.tbDrillSetHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbDrillSetHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbDrillSetHand.Visibility = Visibility.Collapsed;
            }

            if (this.btnStartHS.Background.ToString() == "#FFF5C244")
            {
                if (this.tbStartHSHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbStartHSHand.Visibility = Visibility.Collapsed;
                else if (this.tbStartHSHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbStartHSHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbStartHSHand.Visibility = Visibility.Collapsed;
            }

            if (this.btnTurnToZore.Background.ToString() == "#FFF5C244")
            {
                if (this.tbSTurnZeroHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbSTurnZeroHand.Visibility = Visibility.Collapsed;
                else if (this.tbSTurnZeroHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbSTurnZeroHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbSTurnZeroHand.Visibility = Visibility.Collapsed;
            }

            if (this.btnLockConfirm.Background.ToString() == "#FFF5C244")
            {
                if (this.tbLockConfirmHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbLockConfirmHand.Visibility = Visibility.Collapsed;
                else if (this.tbLockConfirmHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbLockConfirmHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbLockConfirmHand.Visibility = Visibility.Collapsed;
            }

            if (this.btnParamConfirm.Background.ToString() == "#FFF5C244")
            {
                if (this.tbParamConfirmHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbParamConfirmHand.Visibility = Visibility.Collapsed;
                else if (this.tbParamConfirmHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbParamConfirmHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbParamConfirmHand.Visibility = Visibility.Collapsed;
            }

            if (this.btnLinkOpen.Background.ToString() == "#FFF5C244")
            {
                if (this.tbStartLinkHand.Visibility == Visibility.Visible && iTimeCnt / 10 % 2 == 0)
                    this.tbStartLinkHand.Visibility = Visibility.Collapsed;
                else if (this.tbStartLinkHand.Visibility == Visibility.Collapsed && iTimeCnt / 10 % 2 != 0)
                    this.tbStartLinkHand.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tbStartLinkHand.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// 开启联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLinkOpen_Click(object sender, RoutedEventArgs e)
        {
            string msg = "确认开启联动?";
            MessageBoxResult result = MessageBox.Show(msg, "提示", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
                UdpModel model = new UdpModel();
                model.UdpType = UdpType.StartLink;
                byte[] bytes = Encoding.UTF8.GetBytes(model.ToJson());
                GlobalData.Instance.da.SendDataToIPAndPort(bytes, GlobalData.Instance.da.GloConfig.UdpSendIP, GlobalData.Instance.da.GloConfig.UdpSendPort);
            }
        }
    }
}
