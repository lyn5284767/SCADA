using COM.Common;
using ControlLibrary;
using DatabaseLib;
using DemoDriver;
using DevExpress.Charts.Native;
using HBGKTest;
using HBGKTest.YiTongCamera;
using Main.Cat;
using Main.DrillFloor;
using Main.DrillFloor.Sany;
using Main.HydraulicStation;
using Main.HydraulicStation.JJC;
using Main.Integration;
using Main.ScrewThread;
using Main.SecondFloor;
using Main.SIR;
using Main.SIR.Sany;
using Main.SIR.SanyRail;
using Main.WellRepair.DrillFloor;
using Main.WellRepair.HS_Self;
using Main.WellRepair.SecondFloor;
using Main.WellRepair.SIR_Self;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        BrushConverter brush = new BrushConverter();
        private DispatcherTimer timer;
        System.Timers.Timer serviceTimer;
        System.Timers.Timer reportTimer;
        System.Timers.Timer deviceTimer;

        LoadControl projectLoad = new LoadControl();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(10000000);   //时间间隔为一秒
            timer.Tick += new EventHandler(timer_Tick);

            serviceTimer = new System.Timers.Timer(60 * 60 * 1000);
            serviceTimer.Elapsed += ServiceTimer_Elapsed;
            serviceTimer.Enabled = true;
           
            InitCameraInfo();
         
        }

        /// <summary>
        /// 初始化摄像头信息
        /// </summary>
        private void InitCameraInfo()
        {
            string sql = " Select * from CameraInfo";
            List<CameraInfo> list = DataHelper.Instance.ExecuteList<CameraInfo>(sql);
            //ChannelInfo info1 = GetConfigPara("CAMERA1");
            //ChannelInfo info2 = GetConfigPara("CAMERA2");
            //ChannelInfo info3 = GetConfigPara("CAMERA3");
            //ChannelInfo info4 = GetConfigPara("CAMERA4");
            //ChannelInfo info5 = GetConfigPara("CAMERA5");
            //ChannelInfo info6 = GetConfigPara("CAMERA6");
            //if (info1 != null)
            //{
            //    info1.ID = 1;
            //    GlobalData.Instance.chList.Add(info1);
            //}
            //if (info2 != null)
            //{
            //    info2.ID = 2;
            //    GlobalData.Instance.chList.Add(info2);
            //}
            //if (info3 != null)
            //{
            //    info3.ID = 3;
            //    GlobalData.Instance.chList.Add(info3);
            //}
            //if (info4 != null)
            //{
            //    info4.ID = 4;
            //    GlobalData.Instance.chList.Add(info4);
            //}
            //if (info5 != null)
            //{
            //    info5.ID = 5;
            //    GlobalData.Instance.chList.Add(info5);
            //}
            //if (info6 != null)
            //{
            //    info6.ID = 6;
            //    GlobalData.Instance.chList.Add(info6);
            //}
            
            foreach (CameraInfo cameraInfo in list)
            {
                ChannelInfo ch1 = new ChannelInfo();
                ch1.ID = cameraInfo.Id;
                ch1.ChlID = cameraInfo.ChlId.ToString();
                ch1.nDeviceType = cameraInfo.NDeviceType;
                ch1.RemoteChannle = cameraInfo.REMOTECHANNLE.ToString();
                ch1.RemoteIP = cameraInfo.REMOTEIP;
                ch1.RemotePort = cameraInfo.REMOTEPORT;
                ch1.RemoteUser = cameraInfo.REMOTEUSER;
                ch1.RemotePwd = cameraInfo.REMOTEPWD;
                ch1.nPlayPort = cameraInfo.NPLAYPORT;
                ch1.PtzPort = cameraInfo.PTZPORT;
                ch1.CameraType = cameraInfo.CAMERATYPE;
                GlobalData.Instance.chList.Add(ch1);
            }
            foreach (ChannelInfo info in GlobalData.Instance.chList)
            {
                switch (info.CameraType)
                {
                    case 0:
                        {
                            GlobalData.Instance.cameraList.Add(new UIControl_HBGK1(info));
                            break;
                        }
                    case 1:
                        {
                            GlobalData.Instance.cameraList.Add(new YiTongCameraControl(info));
                            break;
                        }
                        
                }
            }
        }
        private string configPath = System.Environment.CurrentDirectory + @"\Config.ini";
        const int STRINGMAX = 255;
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns></returns>
        private ChannelInfo GetConfigPara(string cameraTag)
        {
            try
            {
                if (System.IO.File.Exists(configPath))
                {
                    StringBuilder sb = new StringBuilder(STRINGMAX);
                    string strChlID = "0";
                    string strNDeviceType = "0";
                    string strRemoteChannle = "0";
                    string strRemoteIP = "0.0.0.0";
                    string strRemotePort = "0";
                    string strRemoteUser = "0";
                    string strRemotePwd = "0";
                    string strNPlayPort = "0";
                    string strPtzPort = "0";
                    string strCameraType = "0";
                    ChannelInfo ch1 = new ChannelInfo();
                    WinAPI.GetPrivateProfileString(cameraTag, "CHLID", strChlID, sb, STRINGMAX, configPath);
                    strChlID = sb.ToString();
                    ch1.ChlID = strChlID;

                    WinAPI.GetPrivateProfileString(cameraTag, "NDEVICETYPE", strNDeviceType, sb, STRINGMAX, configPath);
                    strNDeviceType = sb.ToString();
                    int.TryParse(strNDeviceType, out ch1.nDeviceType);

                    WinAPI.GetPrivateProfileString(cameraTag, "REMOTECHANNLE", strRemoteChannle, sb, STRINGMAX, configPath);
                    strRemoteChannle = sb.ToString();
                    ch1.RemoteChannle = strRemoteChannle;

                    WinAPI.GetPrivateProfileString(cameraTag, "REMOTEIP", strRemoteIP, sb, STRINGMAX, configPath);
                    strRemoteIP = sb.ToString();
                    ch1.RemoteIP = strRemoteIP;

                    WinAPI.GetPrivateProfileString(cameraTag, "REMOTEPORT", strRemotePort, sb, STRINGMAX, configPath);
                    strRemotePort = sb.ToString();
                    int tmpRemotePort = 0;
                    int.TryParse(strRemotePort, out tmpRemotePort);
                    ch1.RemotePort = tmpRemotePort;

                    WinAPI.GetPrivateProfileString(cameraTag, "REMOTEUSER", strRemoteUser, sb, STRINGMAX, configPath);
                    strRemoteUser = sb.ToString();
                    ch1.RemoteUser = strRemoteUser;

                    WinAPI.GetPrivateProfileString(cameraTag, "REMOTEPWD", strRemotePwd, sb, STRINGMAX, configPath);
                    strRemotePwd = sb.ToString();
                    ch1.RemotePwd = strRemotePwd;

                    WinAPI.GetPrivateProfileString(cameraTag, "NPLAYPORT", strNPlayPort, sb, STRINGMAX, configPath);
                    strNPlayPort = sb.ToString();
                    int tmpNPlayPort = 0;
                    int.TryParse(strNPlayPort, out tmpNPlayPort);
                    ch1.nPlayPort = tmpNPlayPort;

                    WinAPI.GetPrivateProfileString(cameraTag, "PTZPORT", strPtzPort, sb, STRINGMAX, configPath);
                    strPtzPort = sb.ToString();
                    int tmpPtzPort = 0;
                    int.TryParse(strPtzPort, out tmpPtzPort);
                    ch1.PtzPort = tmpPtzPort;

                    WinAPI.GetPrivateProfileString(cameraTag, "CAMERATYPE", strCameraType, sb, STRINGMAX, configPath);
                    strCameraType = sb.ToString();
                    int cameraType = 0;
                    int.TryParse(strCameraType, out cameraType);
                    ch1.CameraType = cameraType;

                    return ch1;//正常返回。
                }
                else
                {
                    return null;//配置文件不存在
                }
            }
            catch (Exception e)
            {
                DataHelper.AddErrorLog(e);
                return null;//出现异常情况
            }
        }

        /// <summary>
        /// 定时清理日志文件
        /// </summary>
        private void ServiceTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string logPath = System.IO.Directory.GetCurrentDirectory() + @"\Logs";
            DirectoryInfo root = new DirectoryInfo(logPath);
            // 日志文件保留一个月
            foreach (FileInfo f in root.GetFiles())
            {
                if (f.CreationTime < DateTime.Now.AddDays(-30))
                {
                    f.Delete();
                }
            }
            string sql = string.Format("delete from DateBaseReport where CreateTime<'{0}'", DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss"));
            DataHelper.Instance.ExecuteNonQuery(sql);
        }

        private void DeviceTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.GetNowDevice();
                GloAlarm();
            }));
          
        }
        int inter = 0;
        /// <summary>
        /// 全局告警
        /// </summary>
        private void GloAlarm()
        {
            // 液压站油温过高
            if (GlobalData.Instance.da["774b1"].Value.Boolean)
            {
                //if (inter == 0)
                //{
                //    HandyControl.Controls.Growl.Info("液压油高温预警，请及时降温！");
                //}
                // 未显示告警，且定时器到点
                if (!GlobalData.Instance.HS_OilHigh && inter == 0)
                {
                    GlobalData.Instance.HS_OilHigh = true;
                    GlobalAlarm globalAlarm = new GlobalAlarm();
                    globalAlarm.info("液压油高温预警，请及时降温");
                    globalAlarm.ShowDialog();
                }

            }
            if (!GlobalData.Instance.HS_OilHigh) inter++;

            if (inter > 500) inter = 0;
        }

        int timeTick = 0;
        List<string> sqlList = new List<string>();
        /// <summary>
        /// 报表数据记录
        /// </summary>
        private void ReportTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.da.GloConfig.ReportRecord == 1)
                {
                    // 模拟数据
                    //Random random = new Random();
                    //double now = random.Next(0, 100) / 10.0;
                    //double now2 = now * random.Next(0, 5);
                    //double now3 = now * random.Next(0, 10);
                    //double now4 = now * random.Next(0, 15);
                    //double now5 = now * random.Next(0, 20);
                    string sql = string.Empty;
                    // 系统压力
                    double systemPress = GlobalData.Instance.da["MPressAI"].Value.Int16 / 10.0;
                    if (systemPress > 0)
                    {
                        sql = string.Format("Insert Into DateBaseReport (Name,Value,CreateTime,Type,Cycle) Values('{0}','{1}','{2}','{3}','{4}')", "自研液压站-系统压力",
                            systemPress, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)SaveType.HS_Self_SystemPress, 10 * 60 * 1000);
                        sqlList.Add(sql);
                    }
                    // 主泵1流量
                    double mainFlow = GlobalData.Instance.da["M1ValuePWMR"].Value.Int32 / 10.0;
                    if (mainFlow > 0)
                    {
                        sql = string.Format("Insert Into DateBaseReport (Name,Value,CreateTime,Type,Cycle) Values('{0}','{1}','{2}','{3}','{4}')", "自研液压站-主泵#1流量",
                            mainFlow, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)SaveType.HS_Self_MainFlow, 10 * 60 * 1000);
                        sqlList.Add(sql);
                    }
                    
                    // 主泵2流量
                    double mainTwoFlow = GlobalData.Instance.da["M2ValuePWMR"].Value.Int32 / 10.0;
                    if (mainTwoFlow > 0)
                    {
                        sql = string.Format("Insert Into DateBaseReport (Name,Value,CreateTime,Type,Cycle) Values('{0}','{1}','{2}','{3}','{4}')", "自研液压站-主泵#2流量",
                            mainTwoFlow, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)SaveType.HS_Self_MainTwoFlow, 10 * 60 * 1000);
                        sqlList.Add(sql);
                    }
                    if (timeTick == 60)
                    {
                        // 油温
                        double oilTem = GlobalData.Instance.da["OilTemAI"].Value.Int16 / 10.0;
                        if (oilTem > 0)
                        {
                            sql = string.Format("Insert Into DateBaseReport (Name,Value,CreateTime,Type,Cycle) Values('{0}','{1}','{2}','{3}','{4}')", "自研液压站-油温",
                                oilTem, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)SaveType.HS_Self_OilTmp, 10 * 60 * 1000);
                            sqlList.Add(sql);
                        }
                        // 液压
                        double oilLevel = GlobalData.Instance.da["OilLevelAI"].Value.Int16 / 10.0;
                        if (oilLevel > 0)
                        {
                            sql = string.Format("Insert Into DateBaseReport (Name,Value,CreateTime,Type,Cycle) Values('{0}','{1}','{2}','{3}','{4}')", "自研液压站-液位",
                                oilLevel, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)SaveType.HS_Self_OilLevel, 10 * 60 * 1000);
                            sqlList.Add(sql);
                        }
                    }
                    if (sqlList.Count > 400)
                    {
                        DataHelper.Instance.ExecuteNonQuery(sqlList.ToArray());
                        sqlList.Clear();
                    }
                    timeTick++;
                    if (timeTick > 60) timeTick = 0;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
       
        int heat = 0;
        void timer_Tick(object sender, EventArgs e)
        {
            this.timeLable.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"  ";
            if (DateTime.Now.Second % 5 == 0)
            {
                PingIp("172.16.16.147");
            }
            if (this.projectLoad.Visibility == Visibility.Visible)
            {
                LoadDevice(nowSystemType);
            }
           
            // 启用，改到联动主页面切换
            //if (GlobalData.Instance.DRNowPage != "IngMain")
            //{
            //    if (!GlobalData.Instance.da["459b0"].Value.Boolean) b459b0 = false;
            //    if (!GlobalData.Instance.da["459b1"].Value.Boolean) b459b1 = false;
            //    if (GlobalData.Instance.da["459b0"].Value.Boolean && GlobalData.Instance.DRNowPage != "SFMain" && !b459b0)
            //    {
            //        MouseDownSF(null, null);
            //        b459b0 = true;
            //    }
            //    else if (GlobalData.Instance.da["459b1"].Value.Boolean && GlobalData.Instance.DRNowPage != "DRMain" && !b459b1)
            //    {
            //        MouseDR(null, null);
            //        b459b1 = true;
            //    }
            //}
        }

        //void alarmtimer_Tick(object sender, EventArgs e)
        //{
        //    if (!GlobalData.Instance.da["575b0"].Value.Boolean && SFStop.Instance.Visibility != Visibility.Visible)
        //    {
        //        SFStop.Instance.info("请先标定大钩零位");
        //        SFStop.Instance.ShowDialog();
        //    }
        //    else
        //    {
        //        SFStop.Instance.Dispose();
        //    }
         
        //}

        /// <summary>
        /// 切换当前设备
        /// </summary>
        private void GetNowDevice()
        {
            // 联动模式根据操作台数据切换
            if (GlobalData.Instance.da["460b0"].Value.Boolean)
            {
                if (GlobalData.Instance.da["462b0"].Value.Boolean && nowSystemType != SystemType.SecondFloor)
                {
                    MouseDownSF(null, null);
                }
                else if (GlobalData.Instance.da["462b1"].Value.Boolean && nowSystemType != SystemType.DrillFloor)
                {
                    MouseDR(null, null);
                }
                else if (GlobalData.Instance.da["462b2"].Value.Boolean && nowSystemType != SystemType.SIR)
                {
                    SIR_MouseLeftButtonDown(null, null);
                }
                else if (GlobalData.Instance.da["462b3"].Value.Boolean && nowSystemType != SystemType.CatRoad)
                {
                    Cat_MouseLeftButtonDown(null, null);
                }
            }
            else// 非联动模式，根据旋转按钮切换
            {
                if (GlobalData.Instance.da["459b0"].Value.Boolean && nowSystemType != SystemType.SecondFloor)
                {
                    MouseDownSF(null, null);
                }
                else if (GlobalData.Instance.da["459b1"].Value.Boolean && nowSystemType != SystemType.DrillFloor)
                {
                    MouseDR(null, null);
                }
                else if (GlobalData.Instance.da["459b2"].Value.Boolean && nowSystemType != SystemType.SIR)
                {
                    SIR_MouseLeftButtonDown(null, null);
                }
                else if (GlobalData.Instance.da["459b3"].Value.Boolean && nowSystemType != SystemType.CatRoad)
                {
                    Cat_MouseLeftButtonDown(null, null);
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalData.Instance.da = new DemoDriver.DAService();
                IsLogin();
                //MouseDownSF(null, null);
                reportTimer = new System.Timers.Timer();
                reportTimer.Interval = 1000;
                reportTimer.Elapsed += ReportTimer_Elapsed;
                reportTimer.Enabled = true;

                deviceTimer = new System.Timers.Timer(200);
                deviceTimer.Elapsed += DeviceTimer_Elapsed;
                deviceTimer.Enabled = true;
                //SetUnUseDevice();
                InitSystem();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 初始化系统
        /// </summary>
        private void InitSystem()
        {
            this.tbIng.MaxDropDownHeight = 500;
            this.bdSf.MaxDropDownHeight = 500;
            // 集成系统
            if (GlobalData.Instance.da.GloConfig.IngSystem == 0)
            {
                this.spBottom.Visibility = Visibility.Visible;
                this.gdbottomSF.Visibility = Visibility.Collapsed;
                SetUseDevice();
                InitIngRole();
                MouseIng(null, null);
                if (GlobalData.Instance.da.GloConfig.SysType == 0)
                {
                    if (!GlobalData.Instance.da["575b0"].Value.Boolean)
                    {
                        SFStop.Instance.info("请先标定大钩零位");
                        SFStop.Instance.ShowDialog();
                    }
                }
            }// 二层台单机
            else if (GlobalData.Instance.da.GloConfig.IngSystem == 1)
            {
                this.spBottom.Visibility = Visibility.Collapsed;
                this.gdbottomSF.Visibility = Visibility.Visible;
                bdSFMain_Click(null, null);
                if (GlobalData.Instance.systemRole <= SystemRole.TechMan)
                {
                    this.bdSFOther.Visibility = Visibility.Visible;
                    this.bdSFVersion.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.bdSFOther.Visibility = Visibility.Collapsed;
                    this.bdSFVersion.Visibility = Visibility.Visible;
                }
            }
        }
        /// <summary>
        /// 初始化角色权限
        /// </summary>
        private void InitIngRole()
        {
            // 操作员
            if (GlobalData.Instance.systemRole == SystemRole.OperMan)
            {
                this.miIngSecure.Visibility = Visibility.Collapsed;
                this.miIngParam.Visibility = Visibility.Collapsed;
                this.miIngPosition.Visibility = Visibility.Collapsed;

                this.miSFSecure.Visibility = Visibility.Collapsed;
                this.miSFParam.Visibility = Visibility.Collapsed;
                this.miSFPosition.Visibility = Visibility.Collapsed;

                this.miDRSecure.Visibility = Visibility.Collapsed;
                this.miDRParam.Visibility = Visibility.Collapsed;
                this.miDRPosition.Visibility = Visibility.Collapsed;

                this.miSIRSecure.Visibility = Visibility.Collapsed;
                //this.miSIRParam.Visibility = Visibility.Collapsed;
                this.miSIRPosition.Visibility = Visibility.Collapsed;
            }// 管理员
            else if (GlobalData.Instance.systemRole == SystemRole.AdminMan)
            {
                this.miIngParam.Visibility = Visibility.Collapsed;
                this.miIngPosition.Visibility = Visibility.Collapsed;

                this.miSFParam.Visibility = Visibility.Collapsed;
                this.miSFPosition.Visibility = Visibility.Collapsed;

                this.miDRParam.Visibility = Visibility.Collapsed;
                this.miDRPosition.Visibility = Visibility.Collapsed;

                //this.miSIRParam.Visibility = Visibility.Collapsed;
                this.miSIRPosition.Visibility = Visibility.Collapsed;
            }// 技术员
            else if (GlobalData.Instance.systemRole == SystemRole.TechMan)
            {
                this.miIngParam.Visibility = Visibility.Collapsed;
                this.miSFParam.Visibility = Visibility.Collapsed;
                this.miDRPosition.Visibility = Visibility.Collapsed;
                //this.miSIRParam.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// 设置配置的集成设备
        /// </summary>
        private void SetUseDevice()
        {
            // 默认显示一个集成系统
            int TotalDeviceNum = 1;
            double TotalWidth = this.Width;
            if (GlobalData.Instance.da.GloConfig.SFType == 0)
            {
                this.bdSf.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.bdSf.Visibility = Visibility.Visible;
                TotalDeviceNum += 1;
            }
            if (GlobalData.Instance.da.GloConfig.DRType == 0)
            {
                this.bdDR.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.bdDR.Visibility = Visibility.Visible;
                TotalDeviceNum += 1;
            }
            if (GlobalData.Instance.da.GloConfig.SIRType == 0)
            {
                this.bdSIR.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.bdSIR.Visibility = Visibility.Visible;
                TotalDeviceNum += 1;
                // 自研
                if (GlobalData.Instance.da.GloConfig.SIRType == 1)
                {

                }// jjc
                else if (GlobalData.Instance.da.GloConfig.SIRType == 2)
                {
                    this.spSIR.Visibility = Visibility.Collapsed;
                }// 宝石
                else if (GlobalData.Instance.da.GloConfig.SIRType == 3)
                {
                    this.spSIR.Visibility = Visibility.Collapsed;
                }// 江汉
                else if (GlobalData.Instance.da.GloConfig.SIRType == 4)
                {
                    this.spSIR.Visibility = Visibility.Collapsed;
                }// 轨道式
                else if (GlobalData.Instance.da.GloConfig.SIRType == 5)
                {
                    this.miSIRRecord.Visibility = Visibility.Collapsed;
                    this.miSIRSecure.Visibility = Visibility.Collapsed;
                }
            }
            if (GlobalData.Instance.da.GloConfig.CatType == 0)
            {
                this.bdCat.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.bdCat.Visibility = Visibility.Visible;
                TotalDeviceNum += 1;
            }
            if (GlobalData.Instance.da.GloConfig.HydType == 0)
            {
                this.bdHS.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.bdHS.Visibility = Visibility.Visible;
                TotalDeviceNum += 1;
                // 自研
                if (GlobalData.Instance.da.GloConfig.HydType == 1)
                {

                }// 宝石
                else if (GlobalData.Instance.da.GloConfig.HydType == 2)
                {

                }// jjc
                else if (GlobalData.Instance.da.GloConfig.HydType == 3)
                {
                    this.spHS.Visibility = Visibility.Collapsed;
                }
            }
            if (GlobalData.Instance.da.GloConfig.PreventBoxType == 0)
            {
                this.bdScrewThread.Visibility = Visibility.Collapsed;
            }
            else if (GlobalData.Instance.da.GloConfig.PreventBoxType == 1)
            {
                this.bdScrewThread.Visibility = Visibility.Visible;
                TotalDeviceNum += 1;
            }
            double avgWidth = (TotalWidth - 100) / TotalDeviceNum;
            this.tbIng.Width = avgWidth;
            if (GlobalData.Instance.da.GloConfig.SFType != 0) this.bdSf.Width = avgWidth;
            if (GlobalData.Instance.da.GloConfig.DRType != 0) this.bdDR.Width = avgWidth;
            if (GlobalData.Instance.da.GloConfig.SIRType != 0) this.bdSIR.Width = avgWidth;
            if (GlobalData.Instance.da.GloConfig.CatType != 0) this.bdCat.Width = avgWidth;
            if (GlobalData.Instance.da.GloConfig.HydType != 0) this.bdHS.Width = avgWidth;
            if (GlobalData.Instance.da.GloConfig.PreventBoxType != 0) this.bdScrewThread.Width = avgWidth;
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void IsLogin()
        {
            this.Visibility = Visibility.Hidden;
            Login lg = new Login();
            lg.ShowDialog();

            if (lg.CurUsr.bSuccess)
            {
                this.Visibility = Visibility.Visible;
                //this.sf.showMode(lg.CurUsr._roleID);
                //this.usrName.Text = this.usrName.Text + lg.CurUsr._userName;
                timer.Start();
            }
        }
       
        /// <summary>
        /// 主页
        /// </summary>
        private void MouseDownHome(object sender, MouseButtonEventArgs e)
        {
            //this.spMain.Children.Clear();
            if(GlobalData.Instance.Ing) // 集成界面
            {
                MouseIng(null, null);
                return;
            }
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                //SFMain.Instance.FullScreenEvent -= Instance_FullScreenEvent;
                //SFMain.Instance.FullScreenEvent += Instance_FullScreenEvent;
                //this.spMain.Children.Add(SFMain.Instance);

                //RefreshPipeCount();
                //// 不是手动模式（4）或自动模式（5）切换回主界面变为手动模式
                //if (!(GlobalData.Instance.da["operationModel"].Value.Byte == 5 || GlobalData.Instance.da["operationModel"].Value.Byte == 4))
                //{
                //    byte[] byteToSend;
                //    byteToSend = SendByte(new List<byte> { 1, 4 });

                //    GlobalData.Instance.da.SendBytes(byteToSend);
                //}
                MouseDownSF(null, null);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
            {
                MouseDR(null, null);
            }
            else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
            {
                MouseHS(null, null);
            }
            else if (GlobalData.Instance.systemType == SystemType.SIR)
            {
                SIR_MouseLeftButtonDown(null, null);
            }
            else if (GlobalData.Instance.systemType == SystemType.CatRoad)
            {
                Cat_MouseLeftButtonDown(null, null);
            }
        }

        /// <summary>
        /// 回主界面-刷新钻杆数量
        /// </summary>
        private void RefreshPipeCount()
        {
            byte[] byteToSend = SendByte(new List<byte> { 7, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void Instance_FullScreenEvent(int camId)
        {
            SFMain.Instance.FullScreenEvent -= Instance_FullScreenEvent;
            SFMain.Instance.FullScreenEvent += Instance_FullScreenEvent;
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFCameraFullScreen.Instance);
            SFCameraFullScreen.Instance.CanelFullScreenEvent -= Instance_CanelFullScreenEvent;
            SFCameraFullScreen.Instance.CanelFullScreenEvent += Instance_CanelFullScreenEvent;
            SFCameraFullScreen.Instance.Button_CameraStart(camId);
        }
        private void Instance_SIRFullScreenEvent(int camId)
        {
            SIRSelfMain.Instance.FullScreenEvent -= Instance_SIRFullScreenEvent;
            SIRSelfMain.Instance.FullScreenEvent += Instance_SIRFullScreenEvent;
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SIRCameraFullScreen.Instance);
            SIRCameraFullScreen.Instance.CanelFullScreenEvent -= Instance_SIRCanelFullScreenEvent;
            SIRCameraFullScreen.Instance.CanelFullScreenEvent += Instance_SIRCanelFullScreenEvent;
            SIRCameraFullScreen.Instance.Button_CameraStart(camId);
        }


        private void Instance_CanelFullScreenEvent()
        {
            SFCameraFullScreen.Instance.CanelFullScreenEvent -= Instance_CanelFullScreenEvent;
            SFCameraFullScreen.Instance.CanelFullScreenEvent += Instance_CanelFullScreenEvent;
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFMain.Instance);
            SFCameraFullScreen.Instance.CanelFullScreenEvent -= Instance_CanelFullScreenEvent;
            SFCameraFullScreen.Instance.CanelFullScreenEvent += Instance_CanelFullScreenEvent;
            //SFMain.Instance.PlayCamera();
        }
        /// <summary>
        /// 钻台面-摄像头播放全屏
        /// </summary>
        private void Instance_DRFullScreenEvent(int camId)
        {
            DRMain.Instance.DRFullScreenEvent -= Instance_DRFullScreenEvent;
            DRMain.Instance.DRFullScreenEvent += Instance_DRFullScreenEvent;
            this.spMain.Children.Clear();
            DRCameraFullScreen.Instance.CanelFullScreenEvent -= Instance_DRCanelFullScreenEvent;
            DRCameraFullScreen.Instance.CanelFullScreenEvent += Instance_DRCanelFullScreenEvent;
            this.spMain.Children.Add(DRCameraFullScreen.Instance);
            DRCameraFullScreen.Instance.Button_CameraStart(camId);
        }

        /// <summary>
        /// 钻台面-摄像头播放取消全屏
        /// </summary>
        private void Instance_DRCanelFullScreenEvent()
        {
            DRCameraFullScreen.Instance.CanelFullScreenEvent -= Instance_DRCanelFullScreenEvent;
            DRCameraFullScreen.Instance.CanelFullScreenEvent += Instance_DRCanelFullScreenEvent;
            this.spMain.Children.Clear();           
            this.spMain.Children.Add(DRMain.Instance);
            DRMain.Instance.DRFullScreenEvent -= Instance_DRFullScreenEvent;
            DRMain.Instance.DRFullScreenEvent += Instance_DRFullScreenEvent;
            //DRMain.Instance.PlayCamera();
        }

        private void Instance_SIRCanelFullScreenEvent()
        {
            try
            {
                SIRCameraFullScreen.Instance.CanelFullScreenEvent -= Instance_SIRCanelFullScreenEvent;
                SIRCameraFullScreen.Instance.CanelFullScreenEvent += Instance_SIRCanelFullScreenEvent;
                this.spMain.Children.Clear();
                this.spMain.Children.Add(SIRSelfMain.Instance);
                SIRSelfMain.Instance.FullScreenEvent -= Instance_SIRFullScreenEvent;
                SIRSelfMain.Instance.FullScreenEvent += Instance_SIRFullScreenEvent;
                //SIRSelfMain.Instance.PlayCameraInThread();
            }
            catch(Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }

        }

        /// <summary>
        /// 安全设置
        /// </summary>
        private void MouseDownSecureSetting(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemRole == SystemRole.OperMan)
                {
                    MessageBox.Show("您不具备权限！");
                    return;
                }
                if (GlobalData.Instance.Ing)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(IngSecureMain.Instance);
                    this.mainTitle.Content = "SYAPS-集成系统:安全设置";
                    return;
                }
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFSecureSetting.Instance);
                    this.mainTitle.Content = "SYAPS-二层台:安全设置";
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(DRSecureSetting.Instance);
                    this.mainTitle.Content = "SYAPS-钻台面:安全设置";
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(SIRSecureSetting.Instance);
                        this.mainTitle.Content = "SYAPS-铁钻工:安全设置";
                        return;
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownExit(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认退出？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                this.Close();
                
                System.Windows.Application.Current.Shutdown();
                System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process process in processList)
                {
                    if (process.ProcessName.Contains("Main"))
                    {
                        process.Kill();
                    }
                }
            }
        }
        /// <summary>
        /// IO查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownIO(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFIOMain.Instance);
                    this.mainTitle.Content = "SYAPS-二层台:IO查询";
                    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 22, 0 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    MessageBox.Show("功能未开放!");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    if (GlobalData.Instance.da.GloConfig.SysType == 1)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(WR_SIR_IO.Instance);
                        this.mainTitle.Content = "SYAPS-铁钻工:IO查询";
                        return;
                    }
                    else
                    {
                        if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                        {
                            this.spMain.Children.Clear();
                            this.spMain.Children.Add(SIRSelfIO.Instance);
                            this.mainTitle.Content = "SYAPS-铁钻工:IO查询";
                            return;
                        }
                        else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANYRailway)
                        {
                            this.spMain.Children.Clear();
                            this.spMain.Children.Add(SIRRailWayIO.Instance);
                            this.mainTitle.Content = "SYAPS-铁钻工:IO查询";
                            return;
                        }
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }
        /// <summary>
        /// 钻杆数量设置
        /// </summary>
        private void MouseDownDrillSetting(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.Ing)
                {
                    this.mainTitle.Content = "SYAPS-集成系统:钻杆设置";
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFDrillSetting.Instance);
                    SFDrillSetting.Instance.SysTypeSelect(0);
                    RefreshPipeCount();
                }
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFDrillSetting.Instance);
                    this.mainTitle.Content = "SYAPS-二层台:钻杆设置";
                    SFDrillSetting.Instance.SysTypeSelect(1);
                    RefreshPipeCount();
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFDrillSetting.Instance);
                    this.mainTitle.Content = "SYAPS-钻台面:钻杆设置";
                    SFDrillSetting.Instance.SysTypeSelect(2);
                    byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 7, 1 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    if (GlobalData.Instance.da.GloConfig.HydType == 1)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(HSSetting.Instance);
                        this.mainTitle.Content = "SYAPS-液压站:设备设置";
                    }
                }
                else if(GlobalData.Instance.systemType == SystemType.CatRoad)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }
        /// <summary>
        /// 设备状态
        /// </summary>
        private void MouseDownDeviceStatus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFDeviceStatus.Instance);
                    this.mainTitle.Content = "SYAPS-二层台:设备状态";
                    SFDeviceStatus.Instance.SwitchDeviceStatusPageEvent += Instance_SwitchDeviceStatusPageEvent;
                    gotoEquipStatusPage();
                    this.checkMaintain();
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(DRDeviceStatus.Instance);
                    this.mainTitle.Content = "SYAPS-钻台面:设备状态";
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    if (GlobalData.Instance.da.GloConfig.HydType == 1)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(HSDeviceStatus.Instance);
                        this.mainTitle.Content = "SYAPS-液压站:设备状态";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }
        /// <summary>
        /// 设备状态-切换子页面
        /// </summary>
        /// <param name="pageId"></param>
        private void Instance_SwitchDeviceStatusPageEvent(int pageId)
        {
            this.spMain.Children.Clear();
            if (pageId == 1)
            {
                this.spMain.Children.Add(SFDrillCollarLock.Instance);
            }
            else if (pageId == 2)
            {
                this.spMain.Children.Add(SFFingerboardLock.Instance);
            }
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParamSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemRole != SystemRole.DebugMan && GlobalData.Instance.systemType != SystemType.SIR)
                {
                    MessageBox.Show("您不具备权限！");
                    return;
                }
                if (GlobalData.Instance.Ing)// 集成系统
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(IngParamSet.Instance);
                    this.mainTitle.Content = "SYAPS-集成系统:参数设置";
                    return;
                }
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    if (!SFToHandleModel()) return;
                    if (GlobalData.Instance.da["operationModel"].Value.Byte != 5)
                    {
                        byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 23, 0 });
                        GlobalData.Instance.da.SendBytes(byteToSend);

                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(SFParamMain.Instance);
                        this.mainTitle.Content = "SYAPS-二层台:参数设置";
                    }
                    else
                    {
                        MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    if (GlobalData.Instance.da.GloConfig.SysType == 1)
                    {
                        if (!DRToHandleModel()) return;
                        if (GlobalData.Instance.da["droperationModel"].Value.Byte != 5)
                        {
                            this.spMain.Children.Clear();
                            this.spMain.Children.Add(WR_ParamMain.Instance);
                            this.mainTitle.Content = "SYAPS-钻台面:参数设置";
                        }
                        else
                        {
                            MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                        }
                    }
                    else
                    {
                        if (!DRToHandleModel()) return;
                        if (GlobalData.Instance.da["droperationModel"].Value.Byte != 5)
                        {
                            this.spMain.Children.Clear();
                            this.spMain.Children.Add(DRParamSettingMain.Instance);
                            this.mainTitle.Content = "SYAPS-钻台面:参数设置";
                        }
                        else
                        {
                            MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                        }
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    if (GlobalData.Instance.da.GloConfig.SysType == 1)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(WR_SIR_ParamSet.Instance);
                        this.mainTitle.Content = "SYAPS-铁钻工:参数设置";
                        return;
                    }
                    else
                    {
                        if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                        {
                            this.spMain.Children.Clear();
                            this.spMain.Children.Add(SIRParamMain.Instance);
                            this.mainTitle.Content = "SYAPS-铁钻工:参数设置";
                            return;
                        }
                        else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANYRailway)
                        {
                            this.spMain.Children.Clear();
                            this.spMain.Children.Add(SIRRailWayParamSet.Instance);
                            this.mainTitle.Content = "SYAPS-铁钻工:参数设置";
                            return;
                        }
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }
        /// <summary>
        /// 位置标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemRole > SystemRole.TechMan)
                {
                    MessageBox.Show("您不具备权限！");
                    return;
                }
                if (GlobalData.Instance.Ing)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(IngPosSetting.Instance);
                    this.mainTitle.Content = "SYAPS-集成系统:位置标定";
                    return;
                }
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    if (!SFToHandleModel()) return;
                    if (GlobalData.Instance.da["operationModel"].Value.Byte != 5)
                    {
                        byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 12, 0 });
                        GlobalData.Instance.da.SendBytes(byteToSend);

                        this.spMain.Children.Clear();
                        //this.spMain.Children.Add(SFPositionSetting.Instance);
                        this.spMain.Children.Add(SFPosSetMain.Instance);
                        this.mainTitle.Content = "SYAPS-二层台:位置标定";
                    }
                    else
                    {
                        MessageBox.Show("手动模式切换失败，请回主界面再次尝试！！");
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    if (GlobalData.Instance.da.GloConfig.SysType == 1)
                    {
                        if (!DRToHandleModel()) return;
                        if (GlobalData.Instance.da["droperationModel"].Value.Byte != 5)
                        {
                            this.spMain.Children.Clear();
                            //this.spMain.Children.Add(DRPosSetting.Instance);
                            this.spMain.Children.Add(WR_PosSetMain.Instance);
                            this.mainTitle.Content = "SYAPS-钻台面:位置标定";
                        }
                        else
                        {
                            MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                        }
                    }
                    else
                    {
                        if (!DRToHandleModel()) return;
                        if (GlobalData.Instance.da["droperationModel"].Value.Byte != 5)
                        {
                            this.spMain.Children.Clear();
                            //this.spMain.Children.Add(DRPosSetting.Instance);
                            this.spMain.Children.Add(DRPosSetMain.Instance);
                            this.mainTitle.Content = "SYAPS-钻台面:位置标定";
                        }
                        else
                        {
                            MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                        }
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(SIRPosSetting.Instance);
                        this.mainTitle.Content = "SYAPS-铁钻工:位置标定";
                        return;
                    }
                    else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANYRailway)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(SIRRailWayPosSet.Instance);
                        this.mainTitle.Content = "SYAPS-铁钻工:位置标定";
                        return;
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }
        /// <summary>
        /// 位置补偿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionCompensate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemRole > SystemRole.AdminMan)
                {
                    MessageBox.Show("您不具备权限！");
                    return;
                }
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    if (!SFToHandleModel()) return;
                    if (GlobalData.Instance.da["operationModel"].Value.Byte != 5)
                    {
                        byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 9 });
                        GlobalData.Instance.da.SendBytes(byteToSend);
                        SFPositionCompensate.Instance.txtCompensationType.Visibility = Visibility.Visible;
                        SFPositionCompensate.Instance.menuCarCompensationSetting.IsEnabled = false;
                        SFPositionCompensate.Instance.menuRotateCompensationSetting.IsEnabled = false;
                        SFPositionCompensate.Instance.ShowRotatePosiCompensaTxts(false);
                        SFPositionCompensate.Instance.ShowCarCompensationTxts(false);

                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(SFPositionCompensate.Instance);
                        this.mainTitle.Content = "SYAPS-二层台:位置补偿";
                    }
                    else
                    {
                        MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    MessageBox.Show("功能未开放!");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }
        /// <summary>
        /// 纪录查询
        /// </summary>
        private void Record_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.Ing)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(IngRecord.Instance);
                    this.mainTitle.Content = "SYAPS-集成系统:记录查询";
                    return;
                }
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFRecordMain.Instance);
                    this.mainTitle.Content = "SYAPS-二层台:记录查询";
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    MessageBox.Show("功能未开放!");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(SIRRecord.Instance);
                        this.mainTitle.Content = "SYAPS-铁钻工:记录查询";
                        return;
                    }
                    MessageBox.Show("功能未开放!");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    if (GlobalData.Instance.da.GloConfig.HydType == 1)// 自研液压站
                    {
                        //this.spMain.Children.Clear();
                        //this.spMain.Children.Add(HSAlarm.Instance);
                        //this.BottomColorSetting(this.bdHS, this.tbHS, this.bdOther);
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(SIRSelfRecord.Instance);
                        this.mainTitle.Content = "SYAPS-液压站:记录查询";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void Chart_Clict(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFChart.Instance);
                    this.mainTitle.Content = "SYAPS-二层台:图表";
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    MessageBox.Show("功能未开放!");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    SFReport.Instance.ReportTextBlockShow();
                    this.spMain.Children.Add(SFReport.Instance);
                    this.mainTitle.Content = "SYAPS-二层台:报表";
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    MessageBox.Show("功能未开放!");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    MessageBox.Show("功能未开放");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(HSReport.Instance);
                    this.mainTitle.Content = "SYAPS-液压站:报表";
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }
        /// <summary>
        /// 下位机信息
        /// </summary>
        private void LowInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //    if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                //    {
                this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFLowInfo.Instance);
                //}
                //else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                //{
                //    this.spMain.Children.Clear();
                //    this.spMain.Children.Add(SFLowInfo.Instance);
                //}
                //else if (GlobalData.Instance.systemType == SystemType.SIR)
                //{
                //    this.spMain.Children.Clear();
                //    this.spMain.Children.Add(SFLowInfo.Instance);
                //    return;
                //}
                //else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                //{
                //    this.spMain.Children.Clear();
                //    this.spMain.Children.Add(SFLowInfo.Instance);
                //    return;
                //}
                this.mainTitle.Content = "SYAPS-下位机信息";
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }

        const byte bHeadFirst = 0x50;
        const byte bHeadTwo = 0x01;
        /// <summary>
        /// 组协议
        /// </summary>
        /// <param name="list">协议字符列表</param>
        /// <returns></returns>
        private byte[] SendByte(List<byte> list)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = bHeadFirst; // 默认0位80
            byteToSend[1] = bHeadTwo;   // 默认1位1
            // 2位之后传进来
            for (int i = 0; i < list.Count; i++)
            {
                byteToSend[i + 2] = list[i];
            }
            return byteToSend;
        }

        /// <summary>
        /// 设备状态界面
        /// </summary>
        private void gotoEquipStatusPage()
        {
            byte[] byteToSend = SendByte(new List<byte> { 22, 0 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void checkMaintain()
        {
            if (GlobalData.Instance.da["508b4"].Value.Boolean)
            {
                MessageBox.Show("关机部件保养周期到，请及时保养");
            }
        }
     
        /// <summary>
        /// 切换工艺模式
        /// </summary>
        /// <param name="technique">当前工艺</param>
        private void Instance_SetNowTechniqueEvent(Technique technique)
        {
            //if (technique == Technique.DrillDown)
            //{
            //    this.tbTechnique.Text = "模式:" + Environment.NewLine + "下钻";
            //    this.tbMidDeviceOne.Text = "防喷筒";
                
            //    Grid.SetColumn(this.bdSIR, 5);
            //    Grid.SetColumn(this.bdMidDeviceOne, 4);
            //    Grid.SetColumn(this.bdDR, 3);
            //    Grid.SetColumn(this.bdSf, 2);
            //}
            //else if (technique == Technique.DrillUp)
            //{
            //    this.tbTechnique.Text = "模式:" + Environment.NewLine + "起钻";
            //    this.tbMidDeviceOne.Text = "清扣和" + Environment.NewLine + "丝扣油";
            //    Grid.SetColumn(this.bdSIR, 2);
            //    Grid.SetColumn(this.bdMidDeviceOne, 3);
            //    Grid.SetColumn(this.bdDR, 4);
            //    Grid.SetColumn(this.bdSf, 5);
            //}
            //else
            //{
            //    this.tbTechnique.Text = "非联动";
            //}
        }

        /// <summary>
        ///  查找子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void PingIp(string strIP)
        {
            try
            {
                Ping ping = new Ping();
                ping.SendAsync(strIP, 500, null);
                ping.PingCompleted += new PingCompletedEventHandler(ping_Complete);
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private void ping_Complete(object sender, PingCompletedEventArgs k)
        {

            PingReply reply = k.Reply;

            if (reply.Status == IPStatus.Success)
            {
                heat = 0;
                GlobalData.Instance.ComunciationNormal = true;
            }

            else
            {
                heat += 1;
                if (heat > 3)
                {
                    GlobalData.Instance.ComunciationNormal = false;
                }
            }
        }

        private void BottomColorSetting(Border bdLeft,TextBlock tbLeft, Grid gdBottom)
        {
            var bc = new BrushConverter();
            foreach (Border bd in FindVisualChildren<Border>(gdBottom))
            {
                //bd.Background = (Brush)bc.ConvertFrom("#C4DEE8");
                //if (bd.Name == "bdHome"|| bd.Name == "bdSf" || bd.Name == "bdDR" || bd.Name == "bdIng" || bd.Name == "bdHome" || bd.Name == "bdDrillSetting"
                //    || bd.Name == "bdSecureSetting" || bd.Name == "bdIO" || bd.Name == "bdDeviceStatus" || bd.Name == "bdOther" || bd.Name=="bdSIR" || bd.Name == "bdHS"
                //    || bd.Name== "bdCat")
                //{
                    bd.Background = (Brush)bc.ConvertFrom("#FCFDFF");
                //}
            }
     
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(gdBottom))
            {
                tb.Foreground = (Brush)bc.ConvertFrom("#1F7AFF");
            }
            bdLeft.Background = (Brush)bc.ConvertFrom("#80FFF8");
            tbLeft.Foreground = (Brush)bc.ConvertFrom("#000000");

            //SetUnUseDevice();
        }

        private void BottomColorSetting(Border bdLeft, TextBlock tbLeft, StackPanel gdBottom)
        {
            var bc = new BrushConverter();
            foreach (Border bd in FindVisualChildren<Border>(gdBottom))
            {
                bd.Background = (Brush)bc.ConvertFrom("#FCFDFF");
            }

            foreach (TextBlock tb in FindVisualChildren<TextBlock>(gdBottom))
            {
                tb.Foreground = (Brush)bc.ConvertFrom("#1F7AFF");
            }
            bdLeft.Background = (Brush)bc.ConvertFrom("#80FFF8");
            tbLeft.Foreground = (Brush)bc.ConvertFrom("#000000");

            //SetUnUseDevice();
        }

        private void BottomColorSetting(HandyControl.Controls.SplitButton tbLeft, StackPanel gdBottom)
        {
            var bc = new BrushConverter();
            foreach (HandyControl.Controls.SplitButton bd in FindVisualChildren<HandyControl.Controls.SplitButton>(gdBottom))
            {
                bd.Background = (Brush)bc.ConvertFrom("#FCFDFF");
                bd.Foreground = (Brush)bc.ConvertFrom("#1F7AFF");
            }

            tbLeft.Background = (Brush)bc.ConvertFrom("#80FFF8");
            tbLeft.Foreground = (Brush)bc.ConvertFrom("#000000");

            //SetUnUseDevice();
        }
       
        /// <summary>
        /// 二层台-切换到手动模式
        /// </summary>
        private bool SFToHandleModel()
        {
            if (GlobalData.Instance.da["operationModel"].Value.Byte == 5)//这是判断如果是自动模式，需要切换到手动模式
            {
                MessageBoxResult result = MessageBox.Show("当前为自动模式需切换至手动模式！确认切换？", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 4 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                    int count = 0;
                    while (GlobalData.Instance.da["operationModel"].Value.Byte == 5 && count < 5)
                    {
                        count++;
                        Thread.Sleep(50);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 钻台面-切换到手动模式
        /// </summary>
        private bool DRToHandleModel()
        {
            if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5)//这是判断如果是自动模式，需要切换到手动模式
            {
                MessageBoxResult result = MessageBox.Show("需切换至手动模式！确认切换？", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    byte[] byteToSend = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };
                    GlobalData.Instance.da.SendBytes(byteToSend);
                    int count = 0;
                    while (GlobalData.Instance.da["droperationModel"].Value.Byte != 4 && count < 5)// 每隔50ms检查一次看是否切换成手动
                    {
                        count++;
                        Thread.Sleep(50);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
       
      
        /// <summary>
        /// 根据选择不同类型设备，显示可用和不可用功能
        /// </summary>
        private void SetBorderBackGround()
        {
            //this.menuDrillSetting.Header = "设备设置";
            //this.menuDrillSetting.Visibility = Visibility.Visible;
            //this.menuSecureSetting.Visibility = Visibility.Visible;
            //this.menuIO.Visibility = Visibility.Visible;
            //this.menuDownDeviceStatus.Visibility = Visibility.Visible;
            //this.menuParam.Visibility = Visibility.Visible;
            //this.menuPosition.Visibility = Visibility.Visible;
            //this.menuCompensate.Visibility = Visibility.Visible;
            //this.menuRecord.Visibility = Visibility.Visible;
            //this.menuChart.Visibility = Visibility.Visible;
            //this.menuReport.Visibility = Visibility.Visible;
            //if (GlobalData.Instance.Ing)
            //{
            //    this.menuDrillSetting.Header = "钻杆设置";
            //    this.menuIO.Visibility = Visibility.Collapsed;
            //    this.menuDownDeviceStatus.Visibility = Visibility.Collapsed;
            //    this.menuCompensate.Visibility = Visibility.Collapsed;
             
            //    this.menuChart.Visibility = Visibility.Collapsed;
            //    this.menuReport.Visibility = Visibility.Collapsed;
            //    return;
            //}
            //if (GlobalData.Instance.systemType == SystemType.SecondFloor)
            //{
            //    this.menuDrillSetting.Header = "钻杆设置";
            //}
            //else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
            //{
            //    this.menuDrillSetting.Header = "钻杆设置";
            //    this.menuIO.Visibility = Visibility.Collapsed;
            //    this.menuCompensate.Visibility = Visibility.Collapsed;
            //    this.menuRecord.Visibility = Visibility.Collapsed;
            //    this.menuChart.Visibility = Visibility.Collapsed;
            //    this.menuReport.Visibility = Visibility.Collapsed;
            //}
            //else if (GlobalData.Instance.systemType == SystemType.CatRoad)
            //{
            //    this.menuDrillSetting.Visibility = Visibility.Collapsed;
            //    this.menuSecureSetting.Visibility = Visibility.Collapsed;
            //    this.menuIO.Visibility = Visibility.Collapsed;
            //    this.menuDownDeviceStatus.Visibility = Visibility.Collapsed;
            //    this.menuParam.Visibility = Visibility.Collapsed;
            //    this.menuPosition.Visibility = Visibility.Collapsed;
            //    this.menuCompensate.Visibility = Visibility.Collapsed;
            //    this.menuRecord.Visibility = Visibility.Collapsed;
            //    this.menuChart.Visibility = Visibility.Collapsed;
            //    this.menuReport.Visibility = Visibility.Collapsed;
            //}
            //else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
            //{
            //    if (GlobalData.Instance.da.GloConfig.SysType == 1) //修井
            //    {
            //        if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.SANY)
            //        {
            //            this.menuSecureSetting.Visibility = Visibility.Collapsed;
            //            this.menuIO.Visibility = Visibility.Collapsed;
            //            this.menuParam.Visibility = Visibility.Collapsed;
            //            this.menuPosition.Visibility = Visibility.Collapsed;
            //            this.menuCompensate.Visibility = Visibility.Collapsed;
            //            this.menuChart.Visibility = Visibility.Collapsed;
            //        }
            //        else if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.JJC)
            //        {
            //            this.menuDrillSetting.Visibility = Visibility.Collapsed;
            //            this.menuSecureSetting.Visibility = Visibility.Collapsed;
            //            this.menuIO.Visibility = Visibility.Collapsed;
            //            this.menuDownDeviceStatus.Visibility = Visibility.Collapsed;
            //            this.menuParam.Visibility = Visibility.Collapsed;
            //            this.menuPosition.Visibility = Visibility.Collapsed;
            //            this.menuCompensate.Visibility = Visibility.Collapsed;
            //            this.menuRecord.Visibility = Visibility.Collapsed;
            //            this.menuChart.Visibility = Visibility.Collapsed;
            //            this.menuReport.Visibility = Visibility.Collapsed;
            //        }
            //    }
            //    else
            //    {
            //        if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.SANY)
            //        {
            //            this.menuSecureSetting.Visibility = Visibility.Collapsed;
            //            this.menuIO.Visibility = Visibility.Collapsed;
            //            this.menuParam.Visibility = Visibility.Collapsed;
            //            this.menuPosition.Visibility = Visibility.Collapsed;
            //            this.menuCompensate.Visibility = Visibility.Collapsed;
            //            this.menuChart.Visibility = Visibility.Collapsed;
            //        }
            //        else if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.JJC)
            //        {
            //            this.menuDrillSetting.Visibility = Visibility.Collapsed;
            //            this.menuSecureSetting.Visibility = Visibility.Collapsed;
            //            this.menuIO.Visibility = Visibility.Collapsed;
            //            this.menuDownDeviceStatus.Visibility = Visibility.Collapsed;
            //            this.menuParam.Visibility = Visibility.Collapsed;
            //            this.menuPosition.Visibility = Visibility.Collapsed;
            //            this.menuCompensate.Visibility = Visibility.Collapsed;
            //            this.menuRecord.Visibility = Visibility.Collapsed;
            //            this.menuChart.Visibility = Visibility.Collapsed;
            //            this.menuReport.Visibility = Visibility.Collapsed;
            //        }
            //    }

            //}
            //else if (GlobalData.Instance.systemType == SystemType.SIR)
            //{
            //    if (GlobalData.Instance.da.GloConfig.SysType == 1) //修井
            //    {
            //        this.menuDrillSetting.Visibility = Visibility.Collapsed;
            //        this.menuSecureSetting.Visibility = Visibility.Collapsed;
            //        this.menuDownDeviceStatus.Visibility = Visibility.Collapsed;
            //        this.menuPosition.Visibility = Visibility.Collapsed;
            //        this.menuCompensate.Visibility = Visibility.Collapsed;
            //        this.menuRecord.Visibility = Visibility.Collapsed;
            //        this.menuChart.Visibility = Visibility.Collapsed;
            //        this.menuReport.Visibility = Visibility.Collapsed;
            //    }
            //    else
            //    {
            //        if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
            //        {
            //            this.menuDrillSetting.Visibility = Visibility.Collapsed;
            //            this.menuDownDeviceStatus.Visibility = Visibility.Collapsed;
            //            this.menuCompensate.Visibility = Visibility.Collapsed;
            //            this.menuChart.Visibility = Visibility.Collapsed;
            //            this.menuReport.Visibility = Visibility.Collapsed;

            //        }
            //        else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANYRailway)
            //        {
            //            this.menuDrillSetting.Visibility = Visibility.Collapsed;
            //            this.menuSecureSetting.Visibility = Visibility.Collapsed;
            //            this.menuDownDeviceStatus.Visibility = Visibility.Collapsed;
            //            this.menuCompensate.Visibility = Visibility.Collapsed;
            //            this.menuRecord.Visibility = Visibility.Collapsed;
            //            this.menuChart.Visibility = Visibility.Collapsed;
            //            this.menuReport.Visibility = Visibility.Collapsed;

            //        }
            //        else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.JJC)
            //        {
            //            this.menuDrillSetting.Visibility = Visibility.Collapsed;
            //            this.menuSecureSetting.Visibility = Visibility.Collapsed;
            //            this.menuIO.Visibility = Visibility.Collapsed;
            //            this.menuDownDeviceStatus.Visibility = Visibility.Collapsed;
            //            this.menuParam.Visibility = Visibility.Collapsed;
            //            this.menuPosition.Visibility = Visibility.Collapsed;
            //            this.menuCompensate.Visibility = Visibility.Collapsed;
            //            this.menuRecord.Visibility = Visibility.Collapsed;
            //            this.menuChart.Visibility = Visibility.Collapsed;
            //            this.menuReport.Visibility = Visibility.Collapsed;
            //        }
            //    }
            //}
        }
        //BackgroundWorker bgMeet;
        //
        ///// <summary>
        ///// 加载设备
        ///// </summary>
        //private void LoadDevice(SystemType systemType)
        //{
        //    nowSystemType = systemType;
        //    bgMeet = new BackgroundWorker();
        //    //能否报告进度更新
        //    bgMeet.WorkerReportsProgress = true;
        //    //要执行的后台任务
        //    bgMeet.DoWork += new DoWorkEventHandler(bgMeet_DoWork);
        //    //进度报告方法
        //    bgMeet.ProgressChanged += new ProgressChangedEventHandler(bgMeet_ProgressChanged);
        //    //后台任务执行完成时调用的方法
        //    bgMeet.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgMeet_RunWorkerCompleted);
        //    bgMeet.RunWorkerAsync(); //任务启动
        //}

        ////执行任务
        //void bgMeet_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    //开始播放等待动画
        //    this.Dispatcher.Invoke(new Action(() =>
        //    {
        //        this.projectLoad.Visibility = System.Windows.Visibility.Visible;
        //        bgMeet.ReportProgress(100);
        //    }));

        //}
        ////报告任务进度
        //void bgMeet_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    Thread.Sleep(50000);
        //    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new LoadDeviceDele(LoadEvent), nowSystemType);
        //}
        ////任务执行完成后更新状态
        //void bgMeet_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    this.projectLoad.Visibility = System.Windows.Visibility.Collapsed;
        //}

        //public delegate void LoadDeviceDele(SystemType systemType);//定义委托
        SystemType nowSystemType = SystemType.Unknow;
        /// <summary>
        /// 定时器异步加载设备
        /// </summary>
        /// <param name="systemType"></param>
        private void LoadDevice(SystemType systemType)
        {
            if (systemType == SystemType.SIR)
            {
                try
                {
                    if (GlobalData.Instance.da.GloConfig.SysType == 1) // 修井
                    {
                        this.spMain.Children.Add(WR_SIR_Main.Instance);
                        GlobalData.Instance.Ing = false;
                    }
                    else
                    {
                        // 无
                        if (GlobalData.Instance.da.GloConfig.SIRType == 0)
                        {
                            MessageBox.Show("未配置铁钻工");
                            this.projectLoad.Visibility = Visibility.Collapsed;
                            return;
                        }
                        else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                        {
                            SIRSelfMain.Instance.FullScreenEvent -= Instance_SIRFullScreenEvent;
                            SIRSelfMain.Instance.FullScreenEvent += Instance_SIRFullScreenEvent;
                            this.spMain.Children.Add(SIRSelfMain.Instance);
                            GlobalData.Instance.Ing = false;
                        }
                        else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.JJC)
                        {
                            this.spMain.Children.Add(SIRJJCMain.Instance);
                            GlobalData.Instance.Ing = false;
                        }
                        else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.BS)
                        {
                            this.spMain.Children.Add(SIRBSMain.Instance);
                            GlobalData.Instance.Ing = false;
                        }
                        else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.JH)
                        {

                        }
                        else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANYRailway)
                        {
                            this.spMain.Children.Add(SIRRailWayMain.Instance);
                            GlobalData.Instance.Ing = false;
                        }
                    }

                    GlobalData.Instance.systemType = SystemType.SIR;
                    //this.BottomColorSetting(this.bdSIR, this.tbSIR, this.gdbottom);
                    this.BottomColorSetting(this.bdSIR, this.spBottom);
                    SetBorderBackGround();
                }
                catch (Exception ex)
                {
                    Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                    MessageBox.Show(ex.StackTrace);
                }
            }
            else if (systemType == SystemType.SecondFloor)
            {
                if (GlobalData.Instance.da.GloConfig.SysType == 1) // 修井
                {
                    GlobalData.Instance.Ing = false;
                    this.spMain.Children.Clear();
                    //WR_SFMain.Instance.FullScreenEvent -= Instance_FullScreenEvent;
                    //WR_SFMain.Instance.FullScreenEvent += Instance_FullScreenEvent;
                    this.spMain.Children.Add(WR_SFMain.Instance);
                    GlobalData.Instance.systemType = SystemType.SecondFloor;
                    if (GlobalData.Instance.da.GloConfig.IngSystem == 0)
                    {
                        this.BottomColorSetting(this.bdSf, this.spBottom);
                    }
                    else if (GlobalData.Instance.da.GloConfig.IngSystem == 1)
                    {
                        this.BottomColorSetting(this.bdSFMain, this.tbSFMain, this.gdbottomSF);
                    }

                    //this.BottomColorSetting(this.bdSf, this.tbSf, this.gdbottom);
                    //this.BottomColorSetting(this.bdSf, this.tbSf, this.spBottom);

                    RefreshPipeCount();
                    // 不是手动模式（4）或自动模式（5）切换回主界面变为手动模式
                    if (!(GlobalData.Instance.da["operationModel"].Value.Byte == 5 || GlobalData.Instance.da["operationModel"].Value.Byte == 4))
                    {
                        byte[] byteToSend;
                        byteToSend = SendByte(new List<byte> { 1, 4 });

                        GlobalData.Instance.da.SendBytes(byteToSend);
                    }
                    SetBorderBackGround();
                }
                else
                {
                    GlobalData.Instance.Ing = false;
                    this.spMain.Children.Clear();
                    SFMain.Instance.FullScreenEvent -= Instance_FullScreenEvent;
                    SFMain.Instance.FullScreenEvent += Instance_FullScreenEvent;
                    this.spMain.Children.Add(SFMain.Instance);
                    GlobalData.Instance.systemType = SystemType.SecondFloor;
                    if (GlobalData.Instance.da.GloConfig.IngSystem == 0)
                    {
                        //this.BottomColorSetting(this.bdSf, this.tbSf, this.gdbottom);
                        this.BottomColorSetting(this.bdSf, this.spBottom);
                    }
                    else if (GlobalData.Instance.da.GloConfig.IngSystem == 1)
                    {
                        this.BottomColorSetting(this.bdSFMain, this.tbSFMain, this.gdbottomSF);
                    }

                    RefreshPipeCount();
                    // 不是手动模式（4）或自动模式（5）切换回主界面变为手动模式
                    if (!(GlobalData.Instance.da["operationModel"].Value.Byte == 5 || GlobalData.Instance.da["operationModel"].Value.Byte == 4))
                    {
                        byte[] byteToSend;
                        byteToSend = SendByte(new List<byte> { 1, 4 });

                        GlobalData.Instance.da.SendBytes(byteToSend);
                    }
                    SetBorderBackGround();
                }
            }
            else if (systemType == SystemType.DrillFloor)
            {
                try
                {
                    if (GlobalData.Instance.da.GloConfig.SysType == 1) // 修井
                    {
                        // 自研
                        if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY)
                        {
                            GlobalData.Instance.Ing = false;
                            this.spMain.Children.Clear();
                            this.spMain.Children.Add(WR_DRMain.Instance);

                            GlobalData.Instance.systemType = SystemType.DrillFloor;
                            //this.BottomColorSetting(this.bdDR, this.tbDR, this.gdbottom);
                            this.BottomColorSetting(this.bdDR, this.spBottom);

                            // 不是手动模式（4）或自动模式（5）切换回主界面变为手动模式
                            if (!(GlobalData.Instance.da["droperationModel"].Value.Byte == 5 || GlobalData.Instance.da["droperationModel"].Value.Byte == 4))
                            {
                                byte[] byteToSend;
                                byteToSend = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };

                                GlobalData.Instance.da.SendBytes(byteToSend);
                            }
                        }// 杰瑞
                    else if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.JR)
                    {

                    }
                    else
                    {
                        MessageBox.Show("未配置钻台面");
                        this.projectLoad.Visibility = Visibility.Collapsed;
                    }
                }
                    else
                    {
                        // 自研
                        if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.SANY)
                        {
                            GlobalData.Instance.Ing = false;
                            this.spMain.Children.Clear();
                            DRMain.Instance.DRFullScreenEvent -= Instance_DRFullScreenEvent;
                            DRMain.Instance.DRFullScreenEvent += Instance_DRFullScreenEvent;
                            this.spMain.Children.Add(DRMain.Instance);

                            GlobalData.Instance.systemType = SystemType.DrillFloor;
                            //this.BottomColorSetting(this.bdDR, this.tbDR, this.gdbottom);
                            this.BottomColorSetting(this.bdDR, this.spBottom);

                            // 不是手动模式（4）或自动模式（5）切换回主界面变为手动模式
                            if (!(GlobalData.Instance.da["droperationModel"].Value.Byte == 5 || GlobalData.Instance.da["droperationModel"].Value.Byte == 4))
                            {
                                byte[] byteToSend;
                                byteToSend = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };

                                GlobalData.Instance.da.SendBytes(byteToSend);
                            }
                        }// 杰瑞
                        else if (GlobalData.Instance.da.GloConfig.DRType == (int)DRType.JR)
                        {

                        }
                        else
                        {
                            MessageBox.Show("未配置钻台面");
                            this.projectLoad.Visibility = Visibility.Collapsed;
                        }
                    }
                    SetBorderBackGround();
                }
                catch (Exception ex)
                {
                    Log.Log4Net.AddLog(ex.ToString(), Log.InfoLevel.ERROR);
                }
            }
            this.projectLoad.Visibility = Visibility.Collapsed;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (this.WindowState == WindowState.Maximized) this.WindowState = WindowState.Normal;
                if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Maximized;
            }
        }
        #region 二层台单机系统
        /// <summary>
        /// 二层台主页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdSFMain_Click(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(projectLoad);
            this.projectLoad.Visibility = System.Windows.Visibility.Visible;
            nowSystemType = SystemType.SecondFloor;
            this.mainTitle.Content = "SYAPS-二层台:主页";
        }
        /// <summary>
        /// 钻杆设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdSFDrillSet_Click(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFDrillSetting.Instance);
            this.mainTitle.Content = "SYAPS-二层台:钻杆设置";
            SFDrillSetting.Instance.SysTypeSelect(1);
            RefreshPipeCount();
            BottomColorSetting(this.bdSFDrillSet, this.tbSFDrillSet, this.gdbottomSF);
        }
        /// <summary>
        /// 设备状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdSFDeviceStatus_Click(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFDeviceStatus.Instance);
            this.mainTitle.Content = "SYAPS-二层台:设备状态";
            SFDeviceStatus.Instance.SwitchDeviceStatusPageEvent += Instance_SwitchDeviceStatusPageEvent;
            gotoEquipStatusPage();
            this.checkMaintain();
            BottomColorSetting(this.bdSFDeviceStatus, this.tbSFDeviceStatus, this.gdbottomSF);
        }
        /// <summary>
        /// IO查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdSFIO_Click(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFIOMain.Instance);
            this.mainTitle.Content = "SYAPS-二层台:IO查询";
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 22, 0 });
            GlobalData.Instance.da.SendBytes(byteToSend);
            BottomColorSetting(this.bdSFIO, this.tbSFIO, this.gdbottomSF);
        }
        /// <summary>
        /// 安全设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdSafeSet_Click(object sender, MouseButtonEventArgs e)
        {
            if (GlobalData.Instance.systemRole == SystemRole.OperMan)
            {
                MessageBox.Show("管理员以上级别才能访问，您不具备权限！");
                return;
            }
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFSecureSetting.Instance);
            this.mainTitle.Content = "SYAPS-二层台:安全设置";
            BottomColorSetting(this.bdSFSafeSet, this.tbSFSafeSet, this.gdbottomSF);
        }
        /// <summary>
        /// 记录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SFRecord_Click(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFRecordMain.Instance);
            this.mainTitle.Content = "SYAPS-二层台:记录查询";
            BottomColorSetting(this.bdSFRecord, this.tbSFRecord, this.gdbottomSF);
        }
        /// <summary>
        /// 图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSFFigure_Click(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFChart.Instance);
            this.mainTitle.Content = "SYAPS-二层台:图表";
            BottomColorSetting(this.bdSFFigure, this.tbSFFigure, this.gdbottomSF);
        }
        /// <summary>
        /// 报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdSFReport_Click(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            SFReport.Instance.ReportTextBlockShow();
            this.spMain.Children.Add(SFReport.Instance);
            this.mainTitle.Content = "SYAPS-二层台:报表";
            BottomColorSetting(this.bdSFReport, this.tbSFReport, this.gdbottomSF);
        }
        /// <summary>
        /// 版本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdSFVersion_Click(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFLowInfo.Instance);
            BottomColorSetting(this.bdSFVersion, this.tbSFVersion, this.gdbottomSF);
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SFParamSetting_Click(object sender, RoutedEventArgs e)
        {
            if (!SFToHandleModel()) return;
            if (GlobalData.Instance.da["operationModel"].Value.Byte != 5)
            {
                byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 23, 0 });
                GlobalData.Instance.da.SendBytes(byteToSend);

                this.spMain.Children.Clear();
                this.spMain.Children.Add(SFParamMain.Instance);
                this.mainTitle.Content = "SYAPS-二层台:参数设置";
            }
            else
            {
                MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
            }
        }
        /// <summary>
        /// 位置标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SFPositionSetting_Click(object sender, RoutedEventArgs e)
        {
            if (!SFToHandleModel()) return;
            if (GlobalData.Instance.da["operationModel"].Value.Byte != 5)
            {
                byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 12, 0 });
                GlobalData.Instance.da.SendBytes(byteToSend);

                this.spMain.Children.Clear();
                //this.spMain.Children.Add(SFPositionSetting.Instance);
                this.spMain.Children.Add(SFPosSetMain.Instance);
                this.mainTitle.Content = "SYAPS-二层台:位置标定";
            }
            else
            {
                MessageBox.Show("手动模式切换失败，请回主界面再次尝试！！");
            }
        }
        /// <summary>
        /// 版本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdSFVersion_Click(object sender, RoutedEventArgs e)
        {
            bdSFVersion_Click(null, null);
        }
        #endregion

        #region 集成页面
        /// <summary>
        /// 集成系统-主菜单 ，首次点击进入主页，再次点击弹出菜单列表
        /// </summary>
        private void MouseIng(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemType != SystemType.CIMS)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(IngMain.Instance);
                    //this.spMain.Children.Add(IngMainNew.Instance);
                    GlobalData.Instance.Ing = true;
                    IngMainNew.Instance.SetNowTechniqueEvent += Instance_SetNowTechniqueEvent;
                    IngMain.Instance.SetNowTechniqueEvent += Instance_SetNowTechniqueEvent;
                    GlobalData.Instance.systemType = SystemType.CIMS;

                    //this.BottomColorSetting(this.bdIng, this.tbIng, this.gdbottom);
                    this.BottomColorSetting(this.tbIng, this.spBottom);
                    SetBorderBackGround();
                    this.mainTitle.Content = "SYAPS-集成系统:主页";
                }
                else
                {
                    this.tbIng.IsDropDownOpen = true;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
        }
        /// <summary>
        /// 菜单列-主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownIng(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(IngMain.Instance);
            GlobalData.Instance.Ing = true;
            IngMain.Instance.SetNowTechniqueEvent += Instance_SetNowTechniqueEvent;
            GlobalData.Instance.systemType = SystemType.CIMS;

            this.mainTitle.Content = "SYAPS-集成系统:主页";
        }
        /// <summary>
        /// 记录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownRecord(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(IngRecord.Instance);
            this.mainTitle.Content = "SYAPS-集成系统:记录查询";
            return;
        }
        /// <summary>
        /// 安全配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownSecure(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(IngSecureMain.Instance);
            this.mainTitle.Content = "SYAPS-集成系统:安全设置";
            return;
        }
        /// <summary>
        /// 参数配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownParam(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(IngParamSet.Instance);
            this.mainTitle.Content = "SYAPS-集成系统:参数设置";
            return;
        }
        /// <summary>
        /// 位置标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownPosition(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(IngPosSetting.Instance);
            this.mainTitle.Content = "SYAPS-集成系统:位置标定";
            return;
        }
        /// <summary>
        /// 版本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownVersion(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFLowInfo.Instance);
        }
        #endregion

        #region 二层台

        /// <summary>
        /// 二层台 主页
        /// </summary>
        private void MouseDownSF(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.systemType != SystemType.SecondFloor)
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(projectLoad);
                this.projectLoad.Visibility = System.Windows.Visibility.Visible;
                nowSystemType = SystemType.SecondFloor;
                this.mainTitle.Content = "SYAPS-二层台:主页";
            }
            else
            {
                this.bdSf.IsDropDownOpen = true;
            }
        }
        /// <summary>
        /// 二层台-主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSFMain(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(projectLoad);
            this.projectLoad.Visibility = System.Windows.Visibility.Visible;
            nowSystemType = SystemType.SecondFloor;
            this.mainTitle.Content = "SYAPS-二层台:主页";
        }
        /// <summary>
        /// 钻杆设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSFDrillSet(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFDrillSetting.Instance);
            this.mainTitle.Content = "SYAPS-二层台:钻杆设置";
            SFDrillSetting.Instance.SysTypeSelect(1);
            RefreshPipeCount();
        }
        /// <summary>
        /// 设备状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSFDeviceStatus(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFDeviceStatus.Instance);
            this.mainTitle.Content = "SYAPS-二层台:设备状态";
            SFDeviceStatus.Instance.SwitchDeviceStatusPageEvent += Instance_SwitchDeviceStatusPageEvent;
            gotoEquipStatusPage();
            this.checkMaintain();
        }
        /// <summary>
        /// IO查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MouseSFIO(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFIOMain.Instance);
            this.mainTitle.Content = "SYAPS-二层台:IO查询";
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 22, 0 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 记录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSFRecord(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFRecordMain.Instance);
            this.mainTitle.Content = "SYAPS-二层台:记录查询";
        }
        /// <summary>
        /// 图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSFDiagram(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFChart.Instance);
            this.mainTitle.Content = "SYAPS-二层台:图表";
        }
        /// <summary>
        /// 报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSFReport(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            SFReport.Instance.ReportTextBlockShow();
            this.spMain.Children.Add(SFReport.Instance);
            this.mainTitle.Content = "SYAPS-二层台:报表";
        }
        /// <summary>
        /// 安全设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSFSafe(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFSecureSetting.Instance);
            this.mainTitle.Content = "SYAPS-集成系统:安全设置";
            return;
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSFParam(object sender, RoutedEventArgs e)
        {
            if (!SFToHandleModel()) return;
            if (GlobalData.Instance.da["operationModel"].Value.Byte != 5)
            {
                byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 23, 0 });
                GlobalData.Instance.da.SendBytes(byteToSend);

                this.spMain.Children.Clear();
                this.spMain.Children.Add(SFParamMain.Instance);
                this.mainTitle.Content = "SYAPS-二层台:参数设置";
            }
            else
            {
                MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
            }
        }
        /// <summary>
        /// 位置标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSFPosition(object sender, RoutedEventArgs e)
        {
            if (!SFToHandleModel()) return;
            if (GlobalData.Instance.da["operationModel"].Value.Byte != 5)
            {
                byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 12, 0 });
                GlobalData.Instance.da.SendBytes(byteToSend);

                this.spMain.Children.Clear();
                //this.spMain.Children.Add(SFPositionSetting.Instance);
                this.spMain.Children.Add(SFPosSetMain.Instance);
                this.mainTitle.Content = "SYAPS-二层台:位置标定";
            }
            else
            {
                MessageBox.Show("手动模式切换失败，请回主界面再次尝试！！");
            }
        }
        #endregion
        #region 钻台面
        /// <summary>
        /// 钻台面
        /// </summary>
        private void MouseDR(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.systemType != SystemType.DrillFloor)
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(projectLoad);
                this.projectLoad.Visibility = System.Windows.Visibility.Visible;
                nowSystemType = SystemType.DrillFloor;
                this.mainTitle.Content = "SYAPS-钻台面:主页";
            }
            else
            {
                this.bdDR.IsDropDownOpen = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDRMain(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(projectLoad);
            this.projectLoad.Visibility = System.Windows.Visibility.Visible;
            nowSystemType = SystemType.DrillFloor;
            this.mainTitle.Content = "SYAPS-钻台面:主页";
        }
        /// <summary>
        /// 钻杆设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDRDrillSet(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SFDrillSetting.Instance);
            this.mainTitle.Content = "SYAPS-钻台面:钻杆设置";
            SFDrillSetting.Instance.SysTypeSelect(2);
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 7, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 设备状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDRDeviceStatus(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(DRDeviceStatus.Instance);
            this.mainTitle.Content = "SYAPS-钻台面:设备状态";
        }
        /// <summary>
        /// 安全设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDRSafe(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(DRSecureSetting.Instance);
            this.mainTitle.Content = "SYAPS-钻台面:安全设置";
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDRParam(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.da.GloConfig.SysType == 1)
            {
                if (!DRToHandleModel()) return;
                if (GlobalData.Instance.da["droperationModel"].Value.Byte != 5)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(WR_ParamMain.Instance);
                    this.mainTitle.Content = "SYAPS-钻台面:参数设置";
                }
                else
                {
                    MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                }
            }
            else
            {
                if (!DRToHandleModel()) return;
                if (GlobalData.Instance.da["droperationModel"].Value.Byte != 5)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(DRParamSettingMain.Instance);
                    this.mainTitle.Content = "SYAPS-钻台面:参数设置";
                }
                else
                {
                    MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                }
            }
        }
        /// <summary>
        /// 位置标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDRPosition(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.da.GloConfig.SysType == 1)
            {
                if (!DRToHandleModel()) return;
                if (GlobalData.Instance.da["droperationModel"].Value.Byte != 5)
                {
                    this.spMain.Children.Clear();
                    //this.spMain.Children.Add(DRPosSetting.Instance);
                    this.spMain.Children.Add(WR_PosSetMain.Instance);
                    this.mainTitle.Content = "SYAPS-钻台面:位置标定";
                }
                else
                {
                    MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                }
            }
            else
            {
                if (!DRToHandleModel()) return;
                if (GlobalData.Instance.da["droperationModel"].Value.Byte != 5)
                {
                    this.spMain.Children.Clear();
                    //this.spMain.Children.Add(DRPosSetting.Instance);
                    this.spMain.Children.Add(DRPosSetMain.Instance);
                    this.mainTitle.Content = "SYAPS-钻台面:位置标定";
                }
                else
                {
                    MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                }
            }
        }
        #endregion

        #region 铁钻工
        /// <summary>
        /// 铁钻工
        /// </summary>
        private void SIR_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.systemType != SystemType.SIR)
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(projectLoad);
                this.projectLoad.Visibility = System.Windows.Visibility.Visible;
                nowSystemType = SystemType.SIR;
                this.mainTitle.Content = "SYAPS-铁钻工:主页";
            }
            else
            {
                this.bdSIR.IsDropDownOpen = true;
            }
        }
        /// <summary>
        /// 主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSIRMain(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(projectLoad);
            this.projectLoad.Visibility = System.Windows.Visibility.Visible;
            nowSystemType = SystemType.SIR;
            this.mainTitle.Content = "SYAPS-铁钻工:主页";
        }
        /// <summary>
        /// 记录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSIRRecord(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SIRRecord.Instance);
            this.mainTitle.Content = "SYAPS-铁钻工:记录查询";
            return;
        }
        /// <summary>
        /// IO查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSIRIO(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.da.GloConfig.SysType == 1)
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(WR_SIR_IO.Instance);
                this.mainTitle.Content = "SYAPS-铁钻工:IO查询";
                return;
            }
            else
            {
                if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SIRSelfIO.Instance);
                    this.mainTitle.Content = "SYAPS-铁钻工:IO查询";
                    return;
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANYRailway)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SIRRailWayIO.Instance);
                    this.mainTitle.Content = "SYAPS-铁钻工:IO查询";
                    return;
                }
            }
        }
        /// <summary>
        /// 安全设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSIRSecure(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(SIRSecureSetting.Instance);
            this.mainTitle.Content = "SYAPS-铁钻工:安全设置";
            return;
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSIRParam(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.da.GloConfig.SysType == 1)
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(WR_SIR_ParamSet.Instance);
                this.mainTitle.Content = "SYAPS-铁钻工:参数设置";
                return;
            }
            else
            {
                if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SIRParamMain.Instance);
                    this.mainTitle.Content = "SYAPS-铁钻工:参数设置";
                    return;
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANYRailway)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SIRRailWayParamSet.Instance);
                    this.mainTitle.Content = "SYAPS-铁钻工:参数设置";
                    return;
                }
            }
        }
        /// <summary>
        /// 位置标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseSIRPosition(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(SIRPosSetting.Instance);
                this.mainTitle.Content = "SYAPS-铁钻工:位置标定";
                return;
            }
            else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANYRailway)
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(SIRRailWayPosSet.Instance);
                this.mainTitle.Content = "SYAPS-铁钻工:位置标定";
                return;
            }
        }
        #endregion

        #region 猫道
        /// <summary>
        /// 选择猫道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cat_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            try
            {
                nowSystemType = SystemType.CatRoad;
                GlobalData.Instance.Ing = false;
                if (GlobalData.Instance.da.GloConfig.CatType == 0)
                {
                    MessageBox.Show("未配置猫道");
                    return;
                }
                //自研
                else if (GlobalData.Instance.da.GloConfig.CatType == (int)CatType.SANY)
                {

                }
                //宝石
                else if (GlobalData.Instance.da.GloConfig.CatType == (int)CatType.BS)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(BSCatMain.Instance);
                }
                //宏达
                else if (GlobalData.Instance.da.GloConfig.CatType == (int)CatType.HD)
                {

                }
                //胜利
                else if (GlobalData.Instance.da.GloConfig.CatType == (int)CatType.SL)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SL_CatMain.Instance);
                }

                GlobalData.Instance.systemType = SystemType.CatRoad;
                //this.BottomColorSetting(this.bdCat, this.tbCat, this.gdbottom);
                this.BottomColorSetting(this.bdCat, this.spBottom);
                SetBorderBackGround();
                this.mainTitle.Content = "SYAPS-猫道:主页";
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.ToString(), Log.InfoLevel.ERROR);
            }
        }
        #endregion

        #region 液压站
        /// <summary>
        /// 液压站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseHS(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.systemType != SystemType.HydraulicStation)
            {
                try
                {
                    if (GlobalData.Instance.da.GloConfig.SysType == 1)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(WR_HS_Self_Main.Instance);
                    }
                    else
                    {
                        GlobalData.Instance.Ing = false;
                        if (GlobalData.Instance.da.GloConfig.HydType == 0)
                        {
                            MessageBox.Show("未配置液压站");
                            return;
                        }// 自研
                        if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.SANY)
                        {
                            this.spMain.Children.Clear();
                            this.spMain.Children.Add(HSMain.Instance);
                        }// 宝石
                        else if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.BS)
                        {

                        }// JJC
                        else if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.JJC)
                        {
                            this.spMain.Children.Clear();
                            this.spMain.Children.Add(JJC_HSMain.Instance);
                        }
                        else
                        {
                            MessageBox.Show("未配置液压站");
                            return;
                        }
                    }

                    GlobalData.Instance.systemType = SystemType.HydraulicStation;
                    //this.BottomColorSetting(this.bdHS, this.tbHS, this.gdbottom);
                    this.BottomColorSetting(this.bdHS, this.spBottom);
                    SetBorderBackGround();
                    this.mainTitle.Content = "SYAPS-液压站:主页";
                }
                catch (Exception ex)
                {
                    Log.Log4Net.AddLog(ex.ToString(), Log.InfoLevel.ERROR);
                }
            }
            else
            {
                this.bdHS.IsDropDownOpen = true;
            }
        }
        /// <summary>
        /// 主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseHSMain(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.da.GloConfig.SysType == 1)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(WR_HS_Self_Main.Instance);
                }
                else
                {
                    GlobalData.Instance.Ing = false;
                    if (GlobalData.Instance.da.GloConfig.HydType == 0)
                    {
                        MessageBox.Show("未配置液压站");
                        return;
                    }// 自研
                    if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.SANY)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(HSMain.Instance);
                    }// 宝石
                    else if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.BS)
                    {

                    }// JJC
                    else if (GlobalData.Instance.da.GloConfig.HydType == (int)HSType.JJC)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(JJC_HSMain.Instance);
                    }
                    else
                    {
                        MessageBox.Show("未配置液压站");
                        return;
                    }
                }

                GlobalData.Instance.systemType = SystemType.HydraulicStation;
                //this.BottomColorSetting(this.bdHS, this.tbHS, this.gdbottom);
                this.BottomColorSetting(this.bdHS, this.spBottom);
                SetBorderBackGround();
                this.mainTitle.Content = "SYAPS-液压站:主页";
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.ToString(), Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 设备设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseHSDrillSet(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.da.GloConfig.HydType == 1)
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(HSSetting.Instance);
                this.mainTitle.Content = "SYAPS-液压站:设备设置";
            }
        }
        /// <summary>
        /// 设备状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseHSDeviceStatus(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.da.GloConfig.HydType == 1)
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(HSDeviceStatus.Instance);
                this.mainTitle.Content = "SYAPS-液压站:设备状态";
            }
        }
        /// <summary>
        /// 记录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseHSRecord(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.da.GloConfig.HydType == 1)// 自研液压站
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(SIRSelfRecord.Instance);
                this.mainTitle.Content = "SYAPS-液压站:记录查询";
            }
        }
        /// <summary>
        /// 报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseHSReport(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            this.spMain.Children.Add(HSReport.Instance);
            this.mainTitle.Content = "SYAPS-液压站:报表";
        }
        #endregion
        /// <summary>
        /// 丝扣防喷
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrewThread_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            try
            {
                nowSystemType = SystemType.ScrewThread;
                GlobalData.Instance.Ing = false;
                if (GlobalData.Instance.da.GloConfig.PreventBoxType == 0)
                {
                    MessageBox.Show("未配置丝扣防喷装置");
                    return;
                }
                else if (GlobalData.Instance.da.GloConfig.PreventBoxType == 1)
                {
                    //this.spMain.Children.Clear();
                    //this.spMain.Children.Add(SL_ScrewThreadMain.Instance);
                }
                else if (GlobalData.Instance.da.GloConfig.PreventBoxType == 2)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SL_ScrewThreadMain.Instance);
                }

                GlobalData.Instance.systemType = SystemType.ScrewThread;
                this.BottomColorSetting(this.bdScrewThread, this.spBottom);
                SetBorderBackGround();
                this.mainTitle.Content = "SYAPS-丝扣防喷:主页";
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.ToString(), Log.InfoLevel.ERROR);
            }
        }
    }
}
