﻿using COM.Common;
using ControlLibrary;
using DevExpress.Mvvm.Native;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
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

        Dictionary<string, byte> alarmKey = new Dictionary<string, byte>();
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool bCheckTwo = false;
        #endregion

        public IngMainNew()
        {
            InitializeComponent();
            VariableBinding();
            TotalVariableReBinding = new System.Threading.Timer(new TimerCallback(TotalVariableTimer), null, Timeout.Infinite, 50);
            TotalVariableReBinding.Change(0, 50);
            InitAlarmKey();
            this.Loaded += IngMain_Loaded;
        }

        private void IngMain_Loaded(object sender, RoutedEventArgs e)
        {
            InitPanel();
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
            #region 根据配置选择界面
            try
            {
                if (GlobalData.Instance.da.GloConfig.SFType == 1) // 二层台选择
                {
                    this.bdSF.Child = Ing_SF_Self.Instance;
                }
                if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY) // 钻台面选择
                {
                    this.bdDR.Child = Ing_DR_Self.Instance;
                }
                if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY) // 铁钻工选择
                {
                    this.bdSIR.Child = Ing_SIR_Self.Instance;
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.BS)
                {
                    this.bdSIR.Child = Ing_SIR_BS.Instance;
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.JJC)
                {
                    this.bdSIR.Child = Ing_SIR_JJC.Instance;
                }
                if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.SANY)// 液压站选择
                {
                    this.bdHS.Child = Ing_HS_Self.Instance;
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.JJC)
                {
                    this.bdHS.Child = Ing_HS_JJC.Instance;
                }
                if (GlobalData.Instance.da.GloConfig.CatType == (int)CatType.BS) // 猫道选择
                {
                    this.bdCat.Child = Ing_Cat_BS.Instance;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
            #endregion
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
                    this.sfWorkModel = GlobalData.Instance.da["workModel"].Value.Byte;
                    this.sfOprModel = GlobalData.Instance.da["operationModel"].Value.Byte;
                    this.sfTubesType = GlobalData.Instance.da["drillPipeType"].Value.Byte;
                }
                else // 没有二层台
                {
                    this.sfWorkModel = -1;
                    this.sfOprModel = -1;
                    this.sfTubesType = -1;
                }
                if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY) // 钻台面选择
                {
                    this.drWorkModel = GlobalData.Instance.da["drworkModel"].Value.Byte;
                    this.drOprModel = GlobalData.Instance.da["droperationModel"].Value.Byte;
                    this.drTubesType = GlobalData.Instance.da["drdrillPipeType"].Value.Byte;
                }
                else // 没有钻台面
                {
                    this.drWorkModel = -1;
                    this.drOprModel = -1;
                    this.drTubesType = -1;
                }
                if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY) // 铁钻工选择
                {
                    this.sirWorkModel = GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte;
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
            }));
        }

        /// <summary>
        /// 通信数据
        /// </summary>
        private void Communcation()
        {
            #region 通信

            //操作台控制器心跳
            if (GlobalData.Instance.da["508b5"].Value.Boolean == this.tmpStatus)
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
            this.tmpStatus = GlobalData.Instance.da["508b6"].Value.Boolean;

            if (!GlobalData.Instance.ComunciationNormal) this.tbAlarm.Text = "网络连接失败！";
            #endregion
        }

        private void Warnning()
        {
            try
            {
                // 存在的设备全为1，设备不存在为-1，则为送杆
                if ((this.sfWorkModel == 1 || this.sfWorkModel == -1) 
                    && (this.drWorkModel == 1 || this.drWorkModel == -1) 
                    && (this.sirWorkModel == 1 || this.sirWorkModel == -1))
                {

                    this.tbCurWork.Text = "送杆";
                    this.workMode.ContentDown = "送杆";
                    this.workMode.IsChecked = false;
                    alarmKey["设备工作模式不一致"] = 0;
                }// 存在的设备全为2，设备不存在为-1，则为排杆
                else if ((this.sfWorkModel == 2 || this.sfWorkModel == -1)
                    && (this.drWorkModel == 2 || this.drWorkModel == -1)
                    && (this.sirWorkModel == 2 || this.sirWorkModel == -1))
                {
                    this.tbCurWork.Text = "排杆";
                    this.workMode.ContentDown = "排杆";
                    this.workMode.IsChecked = true;
                    alarmKey["设备工作模式不一致"] = 0;
                }
                else
                {
                    this.tbCurWork.Text = "未知";
                    this.workMode.ContentDown = "未知";
                    this.workMode.IsChecked = false;
                    if (alarmKey["设备工作模式不一致"] == 0) alarmKey["设备工作模式不一致"] = 1;
                }

                // 存在的设备全为4，设备不存在为-1，则为手动
                if ((this.sfOprModel == 4 || this.sfOprModel == -1)
                    && (this.drOprModel == 4 || this.drOprModel == -1)
                    && (this.sirOprModel == 1 || this.sirOprModel == -1))
                {

                    this.tbCurOpr.Text = "手动";
                    this.operateMode.ContentDown = "手动";
                    this.operateMode.IsChecked = true;
                    alarmKey["设备操作模式不一致"] = 0;
                }// 存在的设备全为5，设备不存在为-1，则为自动
                else if ((this.sfOprModel == 5 || this.sfOprModel == -1)
                    && (this.drOprModel == 5 || this.drOprModel == -1)
                    && (this.sirOprModel == 2 || this.sirOprModel == -1))
                {
                    this.tbCurOpr.Text = "自动";
                    this.operateMode.ContentDown = "自动";
                    this.operateMode.IsChecked = false;
                    alarmKey["设备操作模式不一致"] = 0;
                }
                else
                {
                    this.tbCurOpr.Text = "未知";
                    this.operateMode.ContentDown = "未知";
                    this.operateMode.IsChecked = false;
                    if (alarmKey["设备操作模式不一致"] == 0) alarmKey["设备操作模式不一致"] = 1;
                }

                //二层台，钻台面都钻杆相同，或者有一个设备没配置
                if (this.sfTubesType == this.drTubesType || this.sfTubesType == -1 || this.drTubesType == -1)
                {
                    int bType = -1;
                    if (this.sfTubesType == this.drTubesType) bType = this.sfTubesType;
                    else if (this.sfTubesType == -1) bType = this.drTubesType;
                    else if (this.drTubesType == -1) bType = this.sfTubesType;
                    else this.tubeType.Text = "二层台/钻台面都未配置";
                    switch (bType)
                    {
                        case 35:
                            this.tubeType.Text = "3.5寸钻杆";
                            break;
                        case 40:
                            this.tubeType.Text = "4寸钻杆";
                            break;
                        case 45:
                            this.tubeType.Text = "4.5寸钻杆";
                            break;
                        case 50:
                            this.tubeType.Text = "5寸钻杆";
                            break;
                        case 55:
                            this.tubeType.Text = "5.5寸钻杆";
                            break;
                        case 60:
                            this.tubeType.Text = "6寸钻铤";
                            break;
                        case 65:
                            this.tubeType.Text = "6.5寸钻铤";
                            break;
                        case 70:
                            this.tubeType.Text = "7寸钻铤";
                            break;
                        case 75:
                            this.tubeType.Text = "7.5寸钻铤";
                            break;
                        case 80:
                            this.tubeType.Text = "8寸钻铤";
                            break;
                        case 90:
                            this.tubeType.Text = "9寸钻铤";
                            break;
                        case 100:
                            this.tubeType.Text = "10寸钻铤";
                            break;
                        case 110:
                            this.tubeType.Text = "11寸钻铤";
                            break;
                    }
                    alarmKey["管柱类型不一致"] = 0;
                }
                else
                {
                    this.tubeType.Text = "钻杆类型不一致";
                    if (alarmKey["管柱类型不一致"] == 0) alarmKey["管柱类型不一致"] = 1;
                }

                if (iTimeCnt % 10 == 0)
                {
                    if (alarmKey.Count > 0)
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
                        this.tbAlarm.Text = "";
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
                this.oobLink.SetBinding(OnOffButton.OnOffButtonCheckProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
                this.tbLink.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay, Converter = new LinkOpenOrCloseConverter() });
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

                this.drDestination.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDes"], Mode = BindingMode.OneWay, Converter = new DesTypeConverter() });
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
            if (isChecked)
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
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
            byte[] sfbyteToSend;// 二层台
            byte[] drbyteToSend;// 钻台面
            byte[] sirbyteToSend;// 铁钻工
            if (this.operateMode.IsChecked)
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 5 });
                drbyteToSend = new byte[10] { 1, 32, 3, 31, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 4 });
                drbyteToSend = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            }
            if (GlobalData.Instance.da.GloConfig.SFType == 1) // 二层台选择
            {
                GlobalData.Instance.da.SendBytes(sfbyteToSend);
                Thread.Sleep(50);
            }
            if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY) //钻台面选择
            {
                GlobalData.Instance.da.SendBytes(drbyteToSend);
                Thread.Sleep(50);
            }
            if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY) // 铁钻工选择
            {
                GlobalData.Instance.da.SendBytes(sirbyteToSend);
            }
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_WorkModel(object sender, EventArgs e)
        {
            byte[] sfbyteToSend;// 二层台
            byte[] drbyteToSend;// 钻台面
            byte[] sirbyteToSend;// 铁钻工
            if (workMode.IsChecked)
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
                drbyteToSend = new byte[10] { 1, 32, 4, 41, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
                drbyteToSend = new byte[10] { 1, 32, 4, 40, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            if (GlobalData.Instance.da.GloConfig.SFType == 1) // 二层台选择
            {
                GlobalData.Instance.da.SendBytes(sfbyteToSend);
                Thread.Sleep(50);
            }
            if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY) //钻台面选择
            {
                GlobalData.Instance.da.SendBytes(drbyteToSend);
                Thread.Sleep(50);
            }
            if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY) // 铁钻工选择
            {
                GlobalData.Instance.da.SendBytes(sirbyteToSend);
            }
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
    }
}
