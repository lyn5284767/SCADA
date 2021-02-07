using COM.Common;
using ControlLibrary;
using DatabaseLib;
using DevExpress.XtraPrinting.Native;
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

namespace Main.Integration
{
    /// <summary>
    /// IngMain.xaml 的交互逻辑
    /// </summary>
    public partial class IngMain : UserControl
    {
        private static IngMain _instance = null;
        private static readonly object syncRoot = new object();

        public static IngMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngMain();
                        }
                    }
                }
                return _instance;
            }
        }
        SystemType systemType = SystemType.SecondFloor;
        System.Threading.Timer TotalVariableReBinding;
        List<Grid> gdList = new List<Grid>();
        //System.Timers.Timer pageChange;
        int count = 0; // 进入页面发送协议次数
        private int iTimeCnt = 0;//用来为时钟计数的变量
        Dictionary<string, byte> alarmKey = new Dictionary<string, byte>();
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool bCheckTwo = false;
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

        public delegate void SetNowTechnique(Technique technique);
        public event SetNowTechnique SetNowTechniqueEvent;
        Technique tmpTechnique;
        Technique nowTechnique;
        public IngMain()
        {
            InitializeComponent();
            if(GlobalData.Instance.da["DrillNums"] !=null) GlobalData.Instance.Rows = GlobalData.Instance.da["DrillNums"].Value.Byte;
            if (GlobalData.Instance.da["103E23B5"] != null) GlobalData.Instance.DrillNum = GlobalData.Instance.da["103E23B5"].Value.Byte;
            InitAlarmKey();
            VariableBinding();
            this.Loaded += IngMain_Loaded;
            TotalVariableReBinding = new System.Threading.Timer(new TimerCallback(TotalVariableTimer), null, Timeout.Infinite, 50);
            TotalVariableReBinding.Change(0, 50);
        }

        private void IngMain_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);

            count = 0;
            GlobalData.Instance.DRNowPage = "IngMain";

            string configPath = System.Environment.CurrentDirectory + "\\KeyBoard.exe";
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processList)
            {
                if (process.ProcessName.Contains("KeyBoard"))
                {
                    process.Kill();
                }
            }
           
            InitAllModel();
        }

        private void VariableBinding()
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
        bool b459b0 = false; bool b459b1 = false;
        private void TotalVariableTimer(object value)
        {
            try
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

                    // 460b0=true联动开启
                    if (nowTechnique != tmpTechnique)
                    {
                        if (SetNowTechniqueEvent != null)
                        {
                            SetNowTechniqueEvent(nowTechnique);
                        }
                        tmpTechnique = nowTechnique;
                    }

                    this.Warnning();
                    this.Communcation();
                    this.MonitorSysStatus();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
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
        /// <summary>
        /// 新建模式
        /// </summary>

        private void tbModel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ModelSetWindow modelSetWindow = new ModelSetWindow();
            modelSetWindow.ShowDialog();
            InitAllModel();
        }
        /// <summary>
        /// 初始化所有模式/设备
        /// </summary>
        public void InitAllModel()
        {
            //gdList.Add(this.gdModelTwo);
            //gdList.Add(this.gdModelThree);
            //gdList.Add(this.gdModelFour);
            //gdList.Add(this.gdModelFive);
            //gdList.Add(this.gdModelSix);
            string sql = "Select * from GlobalModel Order by ID Desc";
            List<GlobalModel> list = DataHelper.Instance.ExecuteList<GlobalModel>(sql);
            if (list.Count > 0)
            {
                ModelDetailData data = new ModelDetailData(list[0]);
                if (this.gdModel.Children[0] is TextBlock)
                    this.gdModel.Children[0].Visibility = Visibility.Collapsed;
                data.StartFinishEvent += Data_StartFinishEvent;
                this.gdModel.Tag = list[0];
                this.gdModel.Children.Add(data);
            }
            //for (int i = 0; i < 5; i++)
            //{
            //    if (i<list.Count)
            //    {
            //        ModelDetailData data = new ModelDetailData(list[i]);
            //        if(gdList[i].Children[0] is TextBlock)
            //            gdList[i].Children[0].Visibility = Visibility.Collapsed;
            //        data.StartFinishEvent += Data_StartFinishEvent;
            //        gdList[i].Tag = list[i];
            //        gdList[i].Children.Add(data);
            //    }
            //    else
            //    {
            //        if (gdList[i].Children[0] is TextBlock)
            //            gdList[i].Children[0].Visibility = Visibility.Visible;
            //        if (gdList[i].Children.Count == 2)
            //        {
            //            gdList[i].Children.RemoveAt(1);
            //        }
            //    }
            //}
            // 修井
            if (GlobalData.Instance.da.GloConfig.SysType == 1)
            {
                if (GlobalData.Instance.da.GloConfig.SFType == 0) this.tbsf.Text = "二层台(未配置)";
                else if (GlobalData.Instance.da.GloConfig.SFType == 1) this.tbsf.Text = "二层台(三一)";
                else this.tbsf.Text = "二层台(错误)";

                if (GlobalData.Instance.da.GloConfig.DRType == 0) this.tbdr.Text = "钻台面(未配置)";
                else if (GlobalData.Instance.da.GloConfig.DRType == 1) this.tbdr.Text = "钻台面(三一)";
                else this.tbdr.Text = "钻台面(错误)";

                if (GlobalData.Instance.da.GloConfig.SIRType == 0) this.tbIron.Text = "铁钻工(未配置)";
                else if (GlobalData.Instance.da.GloConfig.SIRType == 1) this.tbIron.Text = "铁钻工(三一)";
                else this.tbIron.Text = "铁钻工(错误)";

                if (GlobalData.Instance.da.GloConfig.HydType == 0) this.tbHS.Text = "液压站(未配置)";
                else if (GlobalData.Instance.da.GloConfig.HydType == 1) this.tbHS.Text = "液压站(三一)";
                else this.tbHS.Text = "液压站(错误)";


                if (GlobalData.Instance.da.GloConfig.CatType == 0) this.tbCat.Text = "猫道(未配置)";
                else if (GlobalData.Instance.da.GloConfig.CatType == 1) this.tbCat.Text = "猫道(三一)";
                else this.tbCat.Text = "猫道(错误)";
            }
            else // 钻井
            {
                if (GlobalData.Instance.da.GloConfig.SFType == 0)
                {
                    this.tbsf.Text = "二层台(未配置)";
                    Ing_Bank ing_Bank = new Ing_Bank("二层台(未配置)");
                    this.gdSF.Children.Clear();
                    this.gdSF.Children.Add(ing_Bank);
                }
                else if (GlobalData.Instance.da.GloConfig.SFType == 1)
                {
                    this.tbsf.Text = "二层台(三一)";
                    this.gdSF.Children.Clear();
                    this.gdSF.Children.Add(Ing_SF_Self.Instance);
                }
                else this.tbsf.Text = "二层台(错误)";

                if (GlobalData.Instance.da.GloConfig.DRType == 0)
                {
                    this.tbdr.Text = "钻台面(未配置)";
                    Ing_Bank ing_Bank = new Ing_Bank("钻台面(未配置)");
                    this.gdDR.Children.Clear();
                    this.gdDR.Children.Add(ing_Bank);
                }
                else if (GlobalData.Instance.da.GloConfig.DRType == 1)
                {
                    this.tbdr.Text = "钻台面(三一)";
                    this.gdDR.Children.Clear();
                    this.gdDR.Children.Add(Ing_DR_Self.Instance);
                }
                else if (GlobalData.Instance.da.GloConfig.DRType == 2) this.tbdr.Text = "钻台面(杰瑞)";
                else this.tbdr.Text = "钻台面(错误)";

                if (GlobalData.Instance.da.GloConfig.SIRType == 0)
                {
                    this.tbIron.Text = "铁钻工(未配置)";
                    Ing_Bank ing_Bank = new Ing_Bank("铁钻工(未配置)");
                    this.gdSIR.Children.Clear();
                    this.gdSIR.Children.Add(ing_Bank);
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == 1)
                {
                    this.tbIron.Text = "铁钻工(三一)";
                    this.gdSIR.Children.Clear();
                    this.gdSIR.Children.Add(Ing_SIR_Self.Instance);
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == 2)
                {
                    this.tbIron.Text = "铁钻工(JJC)";
                    this.gdSIR.Children.Clear();
                    this.gdSIR.Children.Add(Ing_SIR_JJC.Instance);
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == 3)
                {
                    this.tbIron.Text = "铁钻工(宝石)";
                    this.gdSIR.Children.Clear();
                    this.gdSIR.Children.Add(Ing_SIR_BS.Instance);
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == 4) this.tbIron.Text = "铁钻工(江汉)";
                else if (GlobalData.Instance.da.GloConfig.SIRType == 5)
                {
                    this.tbIron.Text = "铁钻工(轨道)";
                    this.gdSIR.Children.Clear();
                    this.gdSIR.Children.Add(Ing_SIR_RailWay.Instance);
                }
                else this.tbIron.Text = "铁钻工(错误)";

                if (GlobalData.Instance.da.GloConfig.HydType == 0)
                {
                    this.tbHS.Text = "液压站(未配置)";
                    Ing_Bank ing_Bank = new Ing_Bank("液压站(未配置)");
                    this.gdHS.Children.Clear();
                    this.gdHS.Children.Add(ing_Bank);
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 1)
                {
                    this.tbHS.Text = "液压站(三一)";
                    this.gdHS.Children.Clear();
                    this.gdHS.Children.Add(Ing_HS_Self.Instance);
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 2) this.tbHS.Text = "液压站(宝石)";
                else if (GlobalData.Instance.da.GloConfig.HydType == 3)
                {
                    this.tbHS.Text = "液压站(JJC)";
                    this.gdHS.Children.Clear();
                    this.gdHS.Children.Add(Ing_HS_JJC.Instance);
                }
                else this.tbHS.Text = "液压站(错误)";

                if (GlobalData.Instance.da.GloConfig.CatType == 0)
                {
                    this.tbCat.Text = "猫道(未配置)";
                    Ing_Bank ing_Bank = new Ing_Bank("猫道(未配置)");
                    this.gdCat.Children.Clear();
                    this.gdCat.Children.Add(ing_Bank);
                }
                else if (GlobalData.Instance.da.GloConfig.CatType == 1) this.tbCat.Text = "猫道(三一)";
                else if (GlobalData.Instance.da.GloConfig.CatType == 2)
                {
                    this.tbCat.Text = "猫道(宝石)";
                    this.gdCat.Children.Clear();
                    this.gdCat.Children.Add(Ing_Cat_BS.Instance);
                }
                else if (GlobalData.Instance.da.GloConfig.CatType == 3) this.tbCat.Text = "液压站(宏达)";
                else this.tbCat.Text = "猫道(错误)";
            }
        }

        private void Data_StartFinishEvent(GlobalModel model)
        {
            var bc = new BrushConverter();

            //foreach (Grid gd in gdList)
            //{
                if (gdModel.Children.Count == 2)
                {
                    if (gdModel.Children[1] is ModelDetailData &&
                        (gdModel.Tag is GlobalModel) && model!=null && (gdModel.Tag as GlobalModel).ID == model.ID)
                    {
                        (gdModel.Children[1] as ModelDetailData).bdBg.Background = (Brush)bc.ConvertFrom("#72C9F6");
                    }
                    else
                    {
                        (gdModel.Children[1] as ModelDetailData).bdBg.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    }
                }
            //}
                //gdList.ForEach(o => o.Children[0].Background = (Brush)bc.ConvertFrom("#FFFFFF"));
                //gd.Background = (Brush)bc.ConvertFrom("#72C9F6");
            
        }
    }
}
