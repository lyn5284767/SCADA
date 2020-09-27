using COM.Common;
using DatabaseLib;
using DemoDriver;
using DevExpress.Charts.Native;
using HBGKTest;
using HBGKTest.YiTongCamera;
using Main.Cat;
using Main.DrillFloor;
using Main.HydraulicStation;
using Main.Integration;
using Main.SecondFloor;
using Main.SIR;
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
            ChannelInfo info1 = GetConfigPara("CAMERA1");
            ChannelInfo info2 = GetConfigPara("CAMERA2");
            ChannelInfo info3 = GetConfigPara("CAMERA3");
            ChannelInfo info4 = GetConfigPara("CAMERA4");
            ChannelInfo info5 = GetConfigPara("CAMERA5");
            ChannelInfo info6 = GetConfigPara("CAMERA6");
            if (info1 != null)
            {
                info1.ID = 1;
                GlobalData.Instance.chList.Add(info1);
            }
            if (info2 != null)
            {
                info2.ID = 2;
                GlobalData.Instance.chList.Add(info2);
            }
            if (info3 != null)
            {
                info3.ID = 3;
                GlobalData.Instance.chList.Add(info3);
            }
            if (info4 != null)
            {
                info4.ID = 4;
                GlobalData.Instance.chList.Add(info4);
            }
            if (info5 != null)
            {
                info5.ID = 5;
                GlobalData.Instance.chList.Add(info5);
            }
            if (info6 != null)
            {
                info6.ID = 6;
                GlobalData.Instance.chList.Add(info6);
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
        }

        /// <summary>
        /// 报表数据记录
        /// </summary>
        private void ReportTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if ((sender as System.Timers.Timer).Interval == 1000)
                {
                    (sender as System.Timers.Timer).Interval = 60 * 10 * 1000;
                }
                if (GlobalData.Instance.da.GloConfig.ReportRecord == 1)
                {
                    List<string> sqlList = new List<string>();
                    // 系统压力
                    double systemPress = GlobalData.Instance.da["MPressAI"].Value.Int16 / 10.0;
                    string sql = string.Format("Insert Into DateBaseReport (Name,Value,CreateTime,Type,Cycle) Values('{0}','{1}','{2}','{3}','{4}')", "自研液压站-系统压力",
                        systemPress, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), (int)SaveType.HS_Self_SystemPress, 10 * 60 * 1000);
                    sqlList.Add(sql);
                    // 油温
                    double oilTem = GlobalData.Instance.da["OilTemAI"].Value.Int16 / 10.0;
                    sql = string.Format("Insert Into DateBaseReport (Name,Value,CreateTime,Type,Cycle) Values('{0}','{1}','{2}','{3}','{4}')", "自研液压站-油温",
                        oilTem, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), (int)SaveType.HS_Self_OilTmp, 10 * 60 * 1000);
                    sqlList.Add(sql);
                    // 液压
                    double oilLevel = GlobalData.Instance.da["OilLevelAI"].Value.Int16 / 10.0;
                    sql = string.Format("Insert Into DateBaseReport (Name,Value,CreateTime,Type,Cycle) Values('{0}','{1}','{2}','{3}','{4}')", "自研液压站-液位",
                        oilLevel, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), (int)SaveType.HS_Self_OilLevel, 10 * 60 * 1000);
                    sqlList.Add(sql);
                    DataHelper.Instance.ExecuteNonQuery(sqlList.ToArray());
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        int heat = 0;
        bool b459b0 = false;
        bool b459b1 = false;
        void timer_Tick(object sender, EventArgs e)
        {
            this.timeLable.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"  ";
            if (DateTime.Now.Second % 5 == 0)
            {
                PingIp("172.16.16.147");
            }

            if (GlobalData.Instance.DRNowPage != "IngMain")
            {
                if (!GlobalData.Instance.da["459b0"].Value.Boolean) b459b0 = false;
                if (!GlobalData.Instance.da["459b1"].Value.Boolean) b459b1 = false;
                if (GlobalData.Instance.da["459b0"].Value.Boolean && GlobalData.Instance.DRNowPage != "SFMain" && !b459b0)
                {
                    MouseDownSF(null, null);
                    b459b0 = true;
                }
                else if (GlobalData.Instance.da["459b1"].Value.Boolean && GlobalData.Instance.DRNowPage != "DRMain" && !b459b1)
                {
                    MouseDR(null, null);
                    b459b1 = true;
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
                MouseIng(null, null);
                reportTimer = new System.Timers.Timer();
                reportTimer.Interval = 1000;
                reportTimer.Elapsed += ReportTimer_Elapsed;
                reportTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
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
        /// 二层台 主页
        /// </summary>
        private void MouseDownSF(object sender, MouseButtonEventArgs e)
        {
            GlobalData.Instance.Ing = false;
            this.spMain.Children.Clear();
            SFMain.Instance.FullScreenEvent -= Instance_FullScreenEvent;
            SFMain.Instance.FullScreenEvent += Instance_FullScreenEvent;
            this.spMain.Children.Add(SFMain.Instance);
            GlobalData.Instance.systemType = SystemType.SecondFloor;

            this.BottomColorSetting(this.bdSf, this.tbSf, this.bdHome);

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
        /// <summary>
        /// 主页
        /// </summary>
        private void MouseDownHome(object sender, MouseButtonEventArgs e)
        {
            //this.spMain.Children.Clear();
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
            }

        }

        /// <summary>
        /// 安全设置
        /// </summary>
        private void MouseDownSecureSetting(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.Ing)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(IngSecureMain.Instance);
                    this.BottomColorSetting(this.bdIng, this.tbIng, this.bdSecureSetting);
                    return;
                }
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFSecureSetting.Instance);
                    this.BottomColorSetting(this.bdSf, this.tbSf, this.bdSecureSetting);
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(DRSecureSetting.Instance);
                    this.BottomColorSetting(this.bdDR, this.tbDR, this.bdSecureSetting);
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(SIRSecureSetting.Instance);
                        this.BottomColorSetting(this.bdSIR, this.tbSIR, this.bdSecureSetting);
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
        private void MouseDownIO(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFIOMain.Instance);
                    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 22, 0 });
                    GlobalData.Instance.da.SendBytes(byteToSend);

                    this.BottomColorSetting(this.bdSf, this.tbSf, this.bdIO);
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
                        this.spMain.Children.Add(SIRSelfIO.Instance);
                        this.BottomColorSetting(this.bdSIR, this.tbSIR, this.bdIO);
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
        /// 钻杠数量设置
        /// </summary>
        private void MouseDownDrillSetting(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFDrillSetting.Instance);
                    SFDrillSetting.Instance.SysTypeSelect(1);
                    RefreshPipeCount();
                    this.BottomColorSetting(this.bdSf, this.tbSf, this.bdDrillSetting);
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFDrillSetting.Instance);
                    SFDrillSetting.Instance.SysTypeSelect(2);
                    byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 7, 1 });
                    GlobalData.Instance.da.SendBytes(byteToSend);

                    this.BottomColorSetting(this.bdDR, this.tbDR, this.bdDrillSetting); 
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
                        this.BottomColorSetting(this.bdHS, this.tbHS, this.bdDrillSetting);
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
        private void MouseDownDeviceStatus(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFDeviceStatus.Instance);
                    SFDeviceStatus.Instance.SwitchDeviceStatusPageEvent += Instance_SwitchDeviceStatusPageEvent;
                    gotoEquipStatusPage();
                    this.checkMaintain();
                    this.BottomColorSetting(this.bdSf, this.tbSf, this.bdDeviceStatus);
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(DRDeviceStatus.Instance);
                    this.BottomColorSetting(this.bdDR, this.tbDR, this.bdDeviceStatus);
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
                        this.BottomColorSetting(this.bdHS, this.tbHS, this.bdDeviceStatus);
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
                if (GlobalData.Instance.systemRole != SystemRole.DebugMan)
                {
                    MessageBox.Show("您不具备权限！");
                    return;
                }
                if (GlobalData.Instance.Ing)// 集成系统
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(IngParamSet.Instance);
                    this.BottomColorSetting(this.bdIng, this.tbIng, this.bdOther);
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
                        this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
                    }
                    else
                    {
                        MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    if (!DRToHandleModel()) return;
                    if (GlobalData.Instance.da["droperationModel"].Value.Byte != 5)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(DRParamSettingMain.Instance);
                        this.BottomColorSetting(this.bdDR, this.tbDR, this.bdOther);
                    }
                    else
                    {
                        MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(SIRParam.Instance);
                        this.BottomColorSetting(this.bdSIR, this.tbSIR, this.bdOther);
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
                    this.BottomColorSetting(this.bdIng, this.tbIng, this.bdOther);
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
                        this.spMain.Children.Add(SFPositionSetting.Instance);
                        this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
                    }
                    else
                    {
                        MessageBox.Show("手动模式切换失败，请回主界面再次尝试！！");
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    if (!DRToHandleModel()) return;
                    if (GlobalData.Instance.da["droperationModel"].Value.Byte !=5)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(DRPosSetting.Instance);
                        this.BottomColorSetting(this.bdDR, this.tbDR, this.bdOther);
                    }
                    else
                    {
                        MessageBox.Show("手动模式切换失败，请回主界面再次尝试！");
                    }
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                    {
                        this.spMain.Children.Clear();
                        this.spMain.Children.Add(SIRPosSetting.Instance);
                        this.BottomColorSetting(this.bdSIR, this.tbSIR, this.bdOther);
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
                        this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
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
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFRecordMain.Instance);
                    this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    MessageBox.Show("功能未开放!");
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    //if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                    //{
                    //    this.spMain.Children.Clear();
                    //    this.spMain.Children.Add(SIRSelfRecord.Instance);
                    //    this.BottomColorSetting(this.bdSIR, this.tbSIR, this.bdOther);
                    //    return;
                    //}
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
                        this.BottomColorSetting(this.bdHS, this.tbHS, this.bdOther);
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
                    this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
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
                    this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
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
                    this.BottomColorSetting(this.bdHS, this.tbHS, this.bdOther);
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
                if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFLowInfo.Instance);
                    this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFLowInfo.Instance);
                    this.BottomColorSetting(this.bdDR, this.tbDR, this.bdOther);
                }
                else if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFLowInfo.Instance);
                    this.BottomColorSetting(this.bdSIR, this.tbSIR, this.bdOther);
                    return;
                }
                else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(SFLowInfo.Instance);
                    this.BottomColorSetting(this.bdHS, this.tbHS, this.bdOther);
                    return;
                }
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
        /// 钻台面台
        /// </summary>
        private void MouseDR(object sender, MouseButtonEventArgs e)
        {
            try
            {
                GlobalData.Instance.Ing = false;
                // 自研
                if (GlobalData.Instance.da.GloConfig.DRType == 0)
                {
                    this.spMain.Children.Clear();
                    DRMain.Instance.DRFullScreenEvent -= Instance_DRFullScreenEvent;
                    DRMain.Instance.DRFullScreenEvent += Instance_DRFullScreenEvent;
                    this.spMain.Children.Add(DRMain.Instance);

                    GlobalData.Instance.systemType = SystemType.DrillFloor;
                    this.BottomColorSetting(this.bdDR, this.tbDR, this.bdHome);

                    // 不是手动模式（4）或自动模式（5）切换回主界面变为手动模式
                    if (!(GlobalData.Instance.da["droperationModel"].Value.Byte == 5 || GlobalData.Instance.da["droperationModel"].Value.Byte == 4))
                    {
                        byte[] byteToSend;
                        byteToSend = new byte[10] { 1, 32, 3, 31, 0, 0, 0, 0, 0, 0 };

                        GlobalData.Instance.da.SendBytes(byteToSend);
                    }
                }// 杰瑞
                else
                {
                    
                }
                SetBorderBackGround();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.ToString(), Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 集成系统
        /// </summary>
        private void MouseIng(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(IngMain.Instance);
                //this.spMain.Children.Add(IngMainNew.Instance);
                GlobalData.Instance.systemType = SystemType.SecondFloor;
                GlobalData.Instance.Ing = true;

                var bc = new BrushConverter();
                foreach (Border bd in FindVisualChildren<Border>(this.gdBottom))
                {
                    //bd.Background = (Brush)bc.ConvertFrom("#C4DEE8");
                    bd.Background = (Brush)bc.ConvertFrom("#FCFDFF");
                }

                foreach (TextBlock tb in FindVisualChildren<TextBlock>(this.gdBottom))
                {
                    tb.Foreground = (Brush)bc.ConvertFrom("#1F7AFF");
                }
                this.bdIng.Background = (Brush)bc.ConvertFrom("#80FFF8");
                this.tbIng.Foreground = (Brush)bc.ConvertFrom("#000000");

                this.bdHome.Background = (Brush)bc.ConvertFrom("#F4C8B3");
                SetBorderBackGround();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }
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
            { }
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

        private void BottomColorSetting(Border bdLeft,TextBlock tbLeft,Border bdRight)
        {
            var bc = new BrushConverter();
            foreach (Border bd in FindVisualChildren<Border>(this.gdBottom))
            {
                //bd.Background = (Brush)bc.ConvertFrom("#C4DEE8");
                if (bd.Name == "bdHome"|| bd.Name == "bdSf" || bd.Name == "bdDR" || bd.Name == "bdIng" || bd.Name == "bdHome" || bd.Name == "bdDrillSetting"
                    || bd.Name == "bdSecureSetting" || bd.Name == "bdIO" || bd.Name == "bdDeviceStatus" || bd.Name == "bdOther" || bd.Name=="bdSIR" || bd.Name == "bdHS"
                    || bd.Name== "bdCat")
                {
                    bd.Background = (Brush)bc.ConvertFrom("#FCFDFF");
                }
            }
     
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(this.gdBottom))
            {
                tb.Foreground = (Brush)bc.ConvertFrom("#1F7AFF");
            }
            bdRight.Background = (Brush)bc.ConvertFrom("#F4C8B3");
            bdLeft.Background = (Brush)bc.ConvertFrom("#80FFF8");
            tbLeft.Foreground = (Brush)bc.ConvertFrom("#000000");

            if (bdRight.Name == "bdOther")
            {
                this.bottomMenu.Background = (Brush)bc.ConvertFrom("#F4C8B3");
            }
            else
            {
                this.bottomMenu.Background = (Brush)bc.ConvertFrom("#FCFDFF");
            }

        }
        /// <summary>
        /// 铁钻工
        /// </summary>
        private void SIR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                GlobalData.Instance.Ing = false;
                this.spMain.Children.Clear();
                // 无
                if (GlobalData.Instance.da.GloConfig.SIRType == 0)
                {
                    MessageBox.Show("未配置铁钻工");
                    return;
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
                {
                    SIRSelfMain.Instance.FullScreenEvent -= Instance_SIRFullScreenEvent;
                    SIRSelfMain.Instance.FullScreenEvent += Instance_SIRFullScreenEvent;
                    this.spMain.Children.Add(SIRSelfMain.Instance);
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.JJC)
                {
                    this.spMain.Children.Add(SIRJJCMain.Instance);
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.BS)
                {
                    this.spMain.Children.Add(SIRBSMain.Instance);
                }
                else if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.JH)
                {
                  
                }

                GlobalData.Instance.systemType = SystemType.SIR;
                this.BottomColorSetting(this.bdSIR, this.tbSIR, this.bdHome);
                SetBorderBackGround();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                MessageBox.Show(ex.StackTrace);
            }

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
                    while (GlobalData.Instance.da["droperationModel"].Value.Byte != 4 && count < 5)
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
        /// 液压站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseHS(object sender, MouseButtonEventArgs e)
        {
            try
            {
                GlobalData.Instance.Ing = false;
                if (GlobalData.Instance.da.GloConfig.HydType == 0)
                {
                    MessageBox.Show("未配置液压站");
                    return;
                }// 自研
                if (GlobalData.Instance.da.GloConfig.HydType == 1)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(HSMain.Instance);
                }// 宝石
                else if (GlobalData.Instance.da.GloConfig.HydType == 2)
                {
                   
                }// JJC
                else if (GlobalData.Instance.da.GloConfig.HydType == 3)
                {

                }

                GlobalData.Instance.systemType = SystemType.HydraulicStation;
                this.BottomColorSetting(this.bdHS, this.tbHS, this.bdHome);
                SetBorderBackGround();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.ToString(), Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 选择猫道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                GlobalData.Instance.Ing = false;
                if (GlobalData.Instance.da.GloConfig.CatType == 0)
                {
                    MessageBox.Show("未配置猫道");
                    return;
                }
                // 自研
                else if (GlobalData.Instance.da.GloConfig.CatType == 1)
                {
         
                }
                // 宝石
                else if (GlobalData.Instance.da.GloConfig.CatType == 2)
                {
                    this.spMain.Children.Clear();
                    this.spMain.Children.Add(BSCatMain.Instance);
                }
                // 宏达
                else if (GlobalData.Instance.da.GloConfig.CatType == 3)
                {
                    
                }

                GlobalData.Instance.systemType = SystemType.CatRoad;
                this.BottomColorSetting(this.bdCat, this.tbCat, this.bdHome);
                SetBorderBackGround();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.ToString(), Log.InfoLevel.ERROR);
            }
        }

        private void SetBorderBackGround()
        {
            if (GlobalData.Instance.systemType == SystemType.SecondFloor)
            {
                this.imgDeviceStatus.Source = new BitmapImage(new Uri("Images/deviceStatus.png", UriKind.Relative));
                this.imgDrillSetting.Source = new BitmapImage(new Uri("Images/setting.png", UriKind.Relative));
                this.imgSecureSetting.Source = new BitmapImage(new Uri("Images/Secure.png", UriKind.Relative));
                this.imgIO.Source = new BitmapImage(new Uri("Images/IO.png", UriKind.Relative));
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
            {
                this.imgDeviceStatus.Source = new BitmapImage(new Uri("Images/deviceStatus.png", UriKind.Relative));
                this.imgDrillSetting.Source = new BitmapImage(new Uri("Images/setting.png", UriKind.Relative));
                this.imgSecureSetting.Source = new BitmapImage(new Uri("Images/Secure.png", UriKind.Relative));
                this.imgIO.Source = new BitmapImage(new Uri("Images/UnIO.png", UriKind.Relative));
            }
            else if (GlobalData.Instance.systemType == SystemType.CatRoad)
            {
                this.imgDeviceStatus.Source = new BitmapImage(new Uri("Images/UndeviceStatus.png", UriKind.Relative));
                this.imgDrillSetting.Source = new BitmapImage(new Uri("Images/Unsetting.png", UriKind.Relative));
                this.imgSecureSetting.Source = new BitmapImage(new Uri("Images/UnSecure.png", UriKind.Relative));
                this.imgIO.Source = new BitmapImage(new Uri("Images/UnIO.png", UriKind.Relative));
            }
            else if (GlobalData.Instance.systemType == SystemType.HydraulicStation)
            {
                this.imgDeviceStatus.Source = new BitmapImage(new Uri("Images/deviceStatus.png", UriKind.Relative));
                this.imgDrillSetting.Source = new BitmapImage(new Uri("Images/setting.png", UriKind.Relative));
                this.imgSecureSetting.Source = new BitmapImage(new Uri("Images/UnSecure.png", UriKind.Relative));
                this.imgIO.Source = new BitmapImage(new Uri("Images/UnIO.png", UriKind.Relative));
            }
            else if (GlobalData.Instance.systemType == SystemType.SIR)
            {
                this.imgDeviceStatus.Source = new BitmapImage(new Uri("Images/UndeviceStatus.png", UriKind.Relative));
                this.imgDrillSetting.Source = new BitmapImage(new Uri("Images/Unsetting.png", UriKind.Relative));
                this.imgSecureSetting.Source = new BitmapImage(new Uri("Images/Secure.png", UriKind.Relative));
                this.imgIO.Source = new BitmapImage(new Uri("Images/IO.png", UriKind.Relative));
            }
            else if (GlobalData.Instance.systemType == SystemType.CIMS)
            {
                this.imgDeviceStatus.Source = new BitmapImage(new Uri("Images/deviceStatus.png", UriKind.Relative));
                this.imgDrillSetting.Source = new BitmapImage(new Uri("Images/setting.png", UriKind.Relative));
                this.imgSecureSetting.Source = new BitmapImage(new Uri("Images/Secure.png", UriKind.Relative));
                this.imgIO.Source = new BitmapImage(new Uri("Images/IO.png", UriKind.Relative));
            }
        }
    }
}
