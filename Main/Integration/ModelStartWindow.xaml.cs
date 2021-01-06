using COM.Common;
using DevExpress.DirectX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace Main.Integration
{
    /// <summary>
    /// ModelStartWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModelStartWindow : Window
    {
        GlobalModel tmpModel { get; set; }
        int overtime = 5; // 默认超时时间5S
        DateTime protocolSendTime { get; set; } // 协议发送时间
        bool IsSend = false; // 协议是否发送标志
        int autoSelectDrill = 1; // 自动选择指梁，默认选择左1
        public delegate void StartSuccess();
        public event StartSuccess StartSuccessEvent;
        public ModelStartWindow()
        {
            InitializeComponent();
        }
        System.Threading.Timer stepTimer;
        public ModelStartWindow(GlobalModel model)
            : this()
        {
            tmpModel = model;
            this.Loaded += ModelStartWindow_Loaded;
        }

        private void ModelStartWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartModel(tmpModel);
        }
        /// <summary>
        /// 启动模式
        /// </summary>
        /// <param name="model"></param>
        public void StartModel(GlobalModel model)
        {
            tmpModel = model;
            if (model.PipeType == 1)
            {
                autoSelectDrill = 1;
            }
            else if (model.PipeType == 2)
            {
                autoSelectDrill = 17;
            }
            if (this.tmpModel.SelectDrill > 0)
            {
                autoSelectDrill = this.tmpModel.SelectDrill;
            }
            this.tbCurTip.Text = "准备启动";
            stepTimer = new System.Threading.Timer(new TimerCallback(StepTimer_Elapsed), this, 500, 500);
        }
        private void StepTimer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    #region step 1 准备
                    if (this.pbper.Value == 0)  
                    {
                        this.tbCurTip.Text = "开始启动";
                        this.pbper.Value = 10;
                    }
                    #endregion
                    StartHS();
                    CheckDeviceStatus();
                    StartPipeTypeAndDes();
                    TurnToAuto();
                    SelectWorkModel();
                    SelectedDrill();
                    StartLinkModel();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        #region step 2 启动液压站
        /// <summary>
        /// step 2 启动液压站
        /// </summary>
        private void StartHS()
        {
            if (this.pbper.Value == 10)
            {
                this.tbCurTip.Text = "检查液压站模式";
                this.pbper.Value = 15;
                return;
            }
            if (this.pbper.Value == 15) //1.检查液压站操作模式 15->20
            {
                CheckHSWorkModel();
                return;
            }
            if (this.pbper.Value == 20 || this.pbper.Value == 25)// 2.启动液压站 20->30
            {
                StartHSPump();
                return;
            }
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
                    this.tbCurTip.Text = "未设置液压站,不允许启动联动";
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 1)
                {
                    #region 切换到司钻
                    if (GlobalData.Instance.da["771b5"].Value.Boolean && !GlobalData.Instance.da["771b6"].Value.Boolean)// 本地模式
                    {
                        this.tbCurTip.Text = "液压站处于本地控制模式，请切换到司钻再启动";
                    }
                    else if (!GlobalData.Instance.da["771b5"].Value.Boolean && GlobalData.Instance.da["771b6"].Value.Boolean) // 司钻模式
                    {
                        this.tbCurTip.Text = "泵准备启动";
                        this.pbper.Value = 20;
                        IsSend = false;
                    }
                    else // 分阀箱模式
                    {
                        byte[] byteToSend = new byte[10] { 0, 19, 1, 9, 0, 0, 0, 0, 0, 0 }; // 切换司钻房协议
                        string tips = "液压站模式切换超时，重新启动";
                        CheckOverTime(byteToSend, tips, 5);
                    }
                    #endregion
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 2)
                { }
                else if (GlobalData.Instance.da.GloConfig.HydType == 3)
                { }
                else
                {
                    MessageBox.Show("液压站配置错误！请联系售后人员");
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
                    this.tbCurTip.Text = "未设置液压站,不允许启动联动";
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 1)
                {
                    #region 自研液压站启动泵
                    if (tmpModel.HS_PumpType == 0)
                    {
                        this.tbCurTip.Text = "模式中未设置液压站，请返回修改";
                    }
                    else if (tmpModel.HS_PumpType == 1)
                    {
                        if (!GlobalData.Instance.da["770b3"].Value.Boolean) // 1#泵启动
                        {
                            this.tbCurTip.Text = "1#泵启动成功，准备检查电机状态";
                            this.pbper.Value = 30;
                            IsSend = false;
                        }
                        else
                        {
                            byte[] byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 }; // 1#泵启动协议
                            string tips = "1#泵启动超时，重新启动";
                            CheckOverTime(byteToSend, tips,5);
                        }
                    }
                    else if (tmpModel.HS_PumpType == 2)
                    {
                        if (!GlobalData.Instance.da["770b5"].Value.Boolean) // 2#泵启动
                        {
                            this.tbCurTip.Text = "2#泵启动成功，准备检查电机状态";
                            this.pbper.Value = 30;
                            IsSend = false;
                        }
                        else
                        {
                            byte[] byteToSend = new byte[10] { 0, 19, 3, 3, 0, 0, 0, 0, 0, 0 }; // 2#泵启动协议
                            string tips = "2#泵启动超时，重新启动";
                            CheckOverTime(byteToSend, tips,5);
                        }
                    }
                    else if (tmpModel.HS_PumpType == 3)
                    {
                        if (this.pbper.Value == 20)
                        {
                            if (!GlobalData.Instance.da["770b3"].Value.Boolean) // 1#泵启动
                            {
                                this.tbCurTip.Text = "1#泵启动成功,准备启动2#泵";
                                this.pbper.Value = 25;
                                IsSend = false;
                            }
                            else
                            {
                                byte[] byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 }; // 1#泵启动协议
                                string tips = "1#泵启动超时，重新启动";
                                CheckOverTime(byteToSend, tips,5);
                            }
                        }
                        else if (this.pbper.Value == 25)
                        {
                            if (!GlobalData.Instance.da["770b5"].Value.Boolean) // 2#泵启动
                            {
                                this.tbCurTip.Text = "2#泵启动成功，准备检查电机状态";
                                this.pbper.Value = 30;
                                IsSend = false;
                            }
                            else
                            {
                                byte[] byteToSend = new byte[10] { 0, 19, 3, 3, 0, 0, 0, 0, 0, 0 }; // 2#泵启动协议
                                string tips = "2#泵启动超时，重新启动";
                                CheckOverTime(byteToSend, tips,5);
                            }
                        }
                    }
                    else
                    {
                        this.tbCurTip.Text = "模式中设置液压站错误，请返回修改";
                    }
                    #endregion
                }
                else if (GlobalData.Instance.da.GloConfig.HydType == 2)
                { }
                else if (GlobalData.Instance.da.GloConfig.HydType == 3)
                { }
                else
                {
                    MessageBox.Show("液压站配置错误！请联系售后人员");
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.ToString());
            }
        }
        #endregion

        #region Step 3 检查设备状态
        /// <summary>
        ///  Step 3 检查设备状态
        /// </summary>
        private void CheckDeviceStatus()
        {
            // 1.检查二层台电机是否需要使能
            if (this.pbper.Value == 30)
            {
                SFMonitorEnable();
                return;
            }
            // 2.检查钻台面电机是否需要使能
            if (this.pbper.Value == 35)
            {
                DRMonitorEnable();
                return;
            }
            // 3.检查铁钻工系统压力
            if (this.pbper.Value == 40)
            {
                if (GlobalData.Instance.da["SIRSelfSysPress"].Value.Int16 / 10 < 1)
                {
                    this.tbCurTip.Text = "铁钻工压力过低，请检查液压站运行状态，再启动";
                }
                else
                {
                    this.tbCurTip.Text = "铁钻工系统压力正常";
                    this.pbper.Value = 45;
                }
                return;
            }
            // 4.检查二层台/钻台面回零情况
            if (this.pbper.Value == 45)
            {
                SFAndDRTurnToZero();
                return;
            }
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
                    this.tbCurTip.Text = "二层台电机正常，准备检查钻台面电机";
                    this.pbper.Value = 35;
                    IsSend = false;
                }
                else
                {
                    this.tbCurTip.Text = "二层台电机使能中";
                    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 6, 2 }); // 二层台使能协议
                    string tips = "二层台使能超时，重新使能";
                    CheckOverTime(byteToSend, tips,5);
                }
                #endregion
            }
            else
            {
                MessageBox.Show("未配置二层台，请联系售后进行配置");
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
                    this.tbCurTip.Text = "钻台面电机正常，准备检查回零状态";
                    this.pbper.Value = 40;
                    IsSend = false;
                }
                else
                {
                    this.tbCurTip.Text = "钻台面电机使能中";
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
                MessageBox.Show("未配置钻台面，请联系售后进行配置");
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
                this.tbCurTip.Text = "二层台/钻台面已经回零，准备设置管柱类型";
                this.pbper.Value = 50;
                IsSend = false;
            }
            else
            {
                this.tbCurTip.Text = "二层台/钻台面回零中";
                byte[] sendOne = GlobalData.Instance.SendByte(new List<byte> { 13, 4 });
                byte[] sendTwo = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
                byte[] sendThree = GlobalData.Instance.SendToDR(new List<byte> { 13, 4 });
                string tips = "二层台/钻台面回零超时，重新回零";
                CheckOverTime(sendOne, sendTwo, sendThree, tips, 20);
            }
        }
        #endregion

        #region Step 4 选择目的地和钻杆类型
        /// <summary>
        /// Step 4 选择目的地和钻杆类型
        /// </summary>
        private void StartPipeTypeAndDes()
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
            if (this.pbper.Value == 50)
            {
                SFPipeSet(pipeType);
                return;
            }
            // 2.设置钻台面管柱类型
            if (this.pbper.Value == 55)
            {
                DRPipeSet();
                return;
            }
            // 3.设置钻台面的目的地
            if (this.pbper.Value == 60)
            {
                DRDesTypeSet();
                return;
            }
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
                if (this.tmpModel.PipeType == pipeType && this.tmpModel.PipeSize == GlobalData.Instance.da["drillPipeType"].Value.Byte)
                {
                    this.tbCurTip.Text = "二层台管柱类型设置成功，准备设置钻台面管柱类型";
                    this.pbper.Value = 55;
                    IsSend = false;
                }
                else
                {
                    byte[] byteToSend;
                    if ((this.tmpModel.PipeType == 1 && this.tmpModel.PipeSize >= 60) || (this.tmpModel.PipeType == 2 && this.tmpModel.PipeSize < 60))
                    {
                        byteToSend = new byte[] { 80, 1, 3, (byte)this.tmpModel.PipeSize, 0, 0, 1, 0, 0, 0 };
                    }
                    else
                    {
                        byteToSend = new byte[] { 80, 1, 3, (byte)this.tmpModel.PipeSize, 0, 0, 0, 0, 0, 0 };
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
                if (GlobalData.Instance.da["drdrillPipeType"].Value.Byte == this.tmpModel.PipeSize)
                {
                    this.tbCurTip.Text = "钻台面管柱类型设置成功，准备设置目的地";
                    this.pbper.Value = 60;
                    IsSend = false;
                }
                else
                {
                    byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte>() { 3, (byte)this.tmpModel.PipeSize });
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
                if (this.tmpModel.DesType == GlobalData.Instance.da["drDes"].Value.Byte)
                {
                    this.tbCurTip.Text = "目的地设置成功，准备设置操作模式";
                    this.pbper.Value = 65;
                    IsSend = false;
                }
                else
                {
                    byte[] byteToSend = { 80, 33, 11, (byte)this.tmpModel.DesType, 0, 0, 0, 0, 0, 0 };
                    string tips = "设置目的地超时，正在重新设置";
                    CheckOverTime(byteToSend, tips, 5);
                }
            }
        }
        #endregion

        #region step 5 切换到自动模式
        /// <summary>
        /// step 5 切换到自动模式
        /// </summary>
        private void TurnToAuto()
        {
            if (this.pbper.Value == 65)
            {
                AutoSet();
                return;
            }
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
            { }
            else if (GlobalData.Instance.da.GloConfig.SFType == 1)
            {
                sfAuto = GlobalData.Instance.da["operationModel"].Value.Byte == 5 ? true : false;
            }

            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                drAuto = GlobalData.Instance.da["droperationModel"].Value.Byte == 5 ? true : false;
            }

            if (GlobalData.Instance.da.GloConfig.SIRType == 0)
            { }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 1)
            {
                sirAuto = GlobalData.Instance.da["SIRSelfOperModel"].Value.Byte == 5 ? true : false;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 2)
            { }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 3)
            { }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 4)
            { }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 5)
            { }

            if (sfAuto && drAuto && sirAuto)
            {
                this.tbCurTip.Text = "设备已出于自动模式，准备设置工作模式";
                this.pbper.Value = 70;
                IsSend = false;
            }
            else
            {
                this.tbCurTip.Text = "切换自动模式中";
                byte[] sfbyteToSend;// 二层台
                byte[] drbyteToSend;// 钻台面
                byte[] sirbyteToSend;// 铁钻工
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 5 });
                drbyteToSend = new byte[10] { 1, 32, 3, 31, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
                string tips = "切换自动模式超时，正在重新切换";
                CheckOverTime(sfbyteToSend, drbyteToSend, sirbyteToSend, tips, 5);
            }
        }
        #endregion

        #region step 6 选择工作模式
        /// <summary>
        ///  step 6 选择工作模式
        /// </summary>
        private void SelectWorkModel()
        {
            if (this.pbper.Value == 70)
            {
                WorkModelSet();
                return;
            }
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
                
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 1)
            {
                drWorkModel = GlobalData.Instance.da["drworkModel"].Value.Byte;
            }
            else if (GlobalData.Instance.da.GloConfig.DRType == 2)
            { }

            if (GlobalData.Instance.da.GloConfig.SIRType == 0)
            { }
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
                        this.tbCurTip.Text = "铁钻工远程切换上扣模式失败，请去本地切换";
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
                        this.tbCurTip.Text = "铁钻工远程切换卸扣模式失败，请去本地切换";
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
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 3)
            { }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 4)
            { }
            else if (GlobalData.Instance.da.GloConfig.SIRType == 5)
            { }

            if (this.tmpModel.WorkType == 1 && sfWorkModel == 1 && drWorkModel == 1 && sirWorkModel == 1) // 送杆
            {
                this.tbCurTip.Text = "工作模式设置完成，准备选择指梁";
                this.pbper.Value = 80;
                IsSend = false;
                return;
            }
            else if (this.tmpModel.WorkType == 2 && sfWorkModel == 2 && drWorkModel == 2 && sirWorkModel == 2) //排杆
            {
                this.tbCurTip.Text = "工作模式设置完成，准备选择指梁";
                this.pbper.Value = 80;
                IsSend = false;
                return;
            }
            else
            {
                string tips = "工作模式切换超时，重新切换中";
                byte[] sfbyteToSend;// 二层台
                byte[] drbyteToSend;// 钻台面
                byte[] sirbyteToSend;// 铁钻工
                if (this.tmpModel.WorkType == 1)
                {
                    sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
                    drbyteToSend = new byte[10] { 1, 32, 4, 41, 0, 0, 0, 0, 0, 0 };
                    sirbyteToSend = new byte[10] { 23, 17, 2, 1, 0, 0, 0, 0, 0, 0 };
                    CheckOverTime(sfbyteToSend, drbyteToSend, sirbyteToSend, tips, 10);
                }
                else if (this.tmpModel.WorkType == 2)
                {
                    sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
                    drbyteToSend = new byte[10] { 1, 32, 4, 40, 0, 0, 0, 0, 0, 0 };
                    sirbyteToSend = new byte[10] { 23, 17, 2, 2, 0, 0, 0, 0, 0, 0 };
                    CheckOverTime(sfbyteToSend, drbyteToSend, sirbyteToSend, tips, 10);
                }
                else
                {
                    this.tbCurTip.Text = "工作模式设置有误，请返回模式设置中重新设置";
                }
               
            }
        }
        #endregion

        #region step 7 选择指梁
        /// <summary>
        ///  step 7 选择指梁
        /// </summary>
        private void SelectedDrill()
        {
            if (this.pbper.Value == 80)
            {
                SelectDrillSet();
                return;
            }
        }
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

            // 自动选择指梁
            if (this.tmpModel.SelectDrill == -1)
            {
                if (CheckSelectDrill(sfSelectDrillNum, drSelectDrillNum))
                {
                    if (sfSelectDrillNum < 16) this.tbCurTip.Text = "自动选择左" + sfSelectDrillNum + "指梁";
                    else if (sfSelectDrillNum == 16) this.tbCurTip.Text = "自动选择左钻铤";
                    else if (sfSelectDrillNum > 16 && sfSelectDrillNum < 32) this.tbCurTip.Text = "自动选择右" + (sfSelectDrillNum - 16) + "指梁";
                    else this.tbCurTip.Text = "自动选择右钻铤";
                    this.pbper.Value = 90;
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
                        byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 5, (byte)autoSelectDrill });
                        GlobalData.Instance.da.SendBytes(byteToSend);
                        Thread.Sleep(50);
                        byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 5, (byte)autoSelectDrill });
                        GlobalData.Instance.da.SendBytes(byteToSend);
                        if (this.tmpModel.PipeType == 1)
                        {
                            autoSelectDrill++;
                            if (autoSelectDrill == 32)
                            {
                                this.tbCurTip.Text = "自动选择指梁失败，请人工选择";
                            }
                            if (autoSelectDrill > GlobalData.Instance.Rows && autoSelectDrill < 17) autoSelectDrill = 17;
                            //if (autoSelectDrill > 31) autoSelectDrill = 31;
                        }
                        else if (this.tmpModel.PipeType == 2)
                        {
                            if (autoSelectDrill == 32)
                            {
                                this.tbCurTip.Text = "自动选择指梁失败，请人工选择";
                            }
                            autoSelectDrill = 32;
                        }
                    }
                    else
                    {
                        if ((DateTime.Now.Ticks - protocolSendTime.Ticks) / 10000000 >= overtime)
                        {
                            IsSend = false;
                            this.tbCurTip.Text = tips;
                        }
                    }
                }
            }
            else if (this.tmpModel.SelectDrill > 0) // 手动选择指梁
            {
                if (this.tmpModel.SelectDrill == sfSelectDrillNum && this.tmpModel.SelectDrill == drSelectDrillNum)
                {
                    if (sfSelectDrillNum < 16) this.tbCurTip.Text = "自动选择左" + sfSelectDrillNum + "指梁";
                    else if (sfSelectDrillNum == 16) this.tbCurTip.Text = "自动选择左钻铤";
                    else if (sfSelectDrillNum > 16 && sfSelectDrillNum < 32) this.tbCurTip.Text = "自动选择右" + (sfSelectDrillNum - 16) + "指梁";
                    else this.tbCurTip.Text = "自动选择右钻铤";
                    this.pbper.Value = 90;
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
                        byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 5, (byte)autoSelectDrill });
                        GlobalData.Instance.da.SendBytes(byteToSend);
                        Thread.Sleep(50);
                        byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 5, (byte)autoSelectDrill });
                        GlobalData.Instance.da.SendBytes(byteToSend);
                        if (this.tmpModel.PipeType == 1)
                        {
                            autoSelectDrill++;
                            if (autoSelectDrill == 32)
                            {
                                this.tbCurTip.Text = "自动选择指梁失败，请人工选择";
                            }
                            if (autoSelectDrill > GlobalData.Instance.Rows && autoSelectDrill < 17) autoSelectDrill = 17;
                            //if (autoSelectDrill > 31) autoSelectDrill = 31;
                        }
                        else if (this.tmpModel.PipeType == 2)
                        {
                            if (autoSelectDrill == 32)
                            {
                                this.tbCurTip.Text = "自动选择指梁失败，请人工选择";
                            }
                            autoSelectDrill = 32;
                        }
                    }
                    else
                    {
                        if ((DateTime.Now.Ticks - protocolSendTime.Ticks) / 10000000 >= overtime)
                        {
                            IsSend = false;
                            this.tbCurTip.Text = tips;
                        }
                    }
                }
            }
        }
        #endregion

        #region Step 8 启动联动模式
        /// <summary>
        /// Step 8 启动联动模式
        /// </summary>
        private void StartLinkModel()
        {
            if (this.pbper.Value == 90)
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
                this.pbper.Value = 100;
                this.tbCurTip.Text = "设置完成，开启联动";
                return;
            }
            if (this.pbper.Value == 100)
            {
                this.Close();
                if (StartSuccessEvent != null)
                {
                    StartSuccessEvent();
                }
            }
        }
        #endregion
        /// <summary>
        /// 检测选择的指梁是否正确
        /// </summary>
        /// <param name="sfSelectDrillNum">二层台当前选择指梁</param>
        /// <param name="drSelectDrillNum">钻台面当前选择指梁</param>
        /// <returns>true-选择正确；false-选择失败</returns>
        private bool CheckSelectDrill(int sfSelectDrillNum, int drSelectDrillNum)
        {
            if (this.tmpModel.PipeType == 1) // 选择钻杆
            {
                // 选择指梁相同 and 选择钻杆在设定的指梁数内 and 没有报警信号
                if (sfSelectDrillNum == drSelectDrillNum
                    && ((sfSelectDrillNum >= 1 && sfSelectDrillNum < GlobalData.Instance.Rows + 1) || (sfSelectDrillNum >= 17 && sfSelectDrillNum < GlobalData.Instance.Rows + 17))
                    && GlobalData.Instance.da["promptInfo"].Value.Byte != 44 && GlobalData.Instance.da["promptInfo"].Value.Byte != 55
                    && GlobalData.Instance.da["drTipsCode"].Value.Byte != 44 && GlobalData.Instance.da["drTipsCode"].Value.Byte != 55)
                {
                    return true;
                }
                else return false;
            }
            else if (this.tmpModel.PipeType == 2) // 选择钻铤
            {
                // 选择指梁相同 and 选择钻杆在设定的指梁数内 and 没有报警信号
                if (sfSelectDrillNum == drSelectDrillNum
                    && (sfSelectDrillNum == 17 || sfSelectDrillNum == 32)
                    && GlobalData.Instance.da["promptInfo"].Value.Byte != 44 && GlobalData.Instance.da["promptInfo"].Value.Byte != 55
                    && GlobalData.Instance.da["drTipsCode"].Value.Byte != 44 && GlobalData.Instance.da["drTipsCode"].Value.Byte != 55)
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
        /// <summary>
        /// 检查协议是否超时
        /// </summary>
        /// <param name="byteToSend">发送协议</param>
        /// <param name="tips">超时提示</param>
        /// <param name="tick">超时时间</param>
        private void CheckOverTime(byte[] byteToSend, string tips,int tick)
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
                    this.tbCurTip.Text = tips;
                }
            }
        }

        /// <summary>
        /// 检查协议是否超时
        /// </summary>
        /// <param name="byteToSend">发送协议</param>
        /// <param name="tips">超时提示</param>
        private void CheckOverTime(byte[] byteToSend,byte[] byteToSendTwo, string tips,int ticks)
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
                    this.tbCurTip.Text = tips;
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
                    this.tbCurTip.Text = tips;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            stepTimer.Change(-1, 0);
        }
    }

}
