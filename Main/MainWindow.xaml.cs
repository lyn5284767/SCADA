using COM.Common;
using Main.DrillFloor;
using Main.Integration;
using Main.SecondFloor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        System.Timers.Timer serviceTimer;

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
        int heat = 0;
        void timer_Tick(object sender, EventArgs e)
        {
            this.timeLable.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") +"  ";
            if (DateTime.Now.Second % 5 == 0)
            {
                if (!PingIp("172.16.16.147")) { heat += 1; }
                else
                {
                    heat = 0;
                    GlobalData.Instance.ComunciationNormal = true;
                }
                if (heat >= 3)
                {
                    GlobalData.Instance.ComunciationNormal = false;
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
          
            GlobalData.Instance.da = new DemoDriver.DAService();
            IsLogin();
            MouseDownSF(null, null);

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
            this.spMain.Children.Clear();
            SFMain.Instance.FullScreenEvent -= Instance_FullScreenEvent;
            SFMain.Instance.FullScreenEvent += Instance_FullScreenEvent;
            this.spMain.Children.Add(SFMain.Instance);
            GlobalData.Instance.systemType = SystemType.SecondFloor;

            this.BottomColorSetting(this.bdSf, this.tbSf, this.bdHome);

            RefreshPipeCount();
            // 不是手动模式（4）或自动模式（5）切换回主界面变为手动模式
            if (GlobalData.Instance.da["operationModel"].Value.Byte != 5 || GlobalData.Instance.da["operationModel"].Value.Byte != 4)
            {
                byte[] byteToSend;
                byteToSend = SendByte(new List<byte> { 1, 4 });

                GlobalData.Instance.da.SendBytes(byteToSend);
            }
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
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor) {
                MouseDR(null, null);
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
            this.spMain.Children.Clear();
            SFCameraFullScreen.Instance.CanelFullScreenEvent += Instance_CanelFullScreenEvent;
            this.spMain.Children.Add(SFCameraFullScreen.Instance);
            SFCameraFullScreen.Instance.Button_CameraStart(camId);
        }


        private void Instance_CanelFullScreenEvent()
        {
            SFCameraFullScreen.Instance.CanelFullScreenEvent -= Instance_CanelFullScreenEvent;
            this.spMain.Children.Clear();
            SFMain.Instance.FullScreenEvent += Instance_FullScreenEvent;
            this.spMain.Children.Add(SFMain.Instance);
            SFMain.Instance.PlayCamera();
        }
        /// <summary>
        /// 钻台面-摄像头播放全屏
        /// </summary>
        private void Instance_DRFullScreenEvent(int camId)
        {
            DRMain.Instance.DRFullScreenEvent -= Instance_DRFullScreenEvent;
            this.spMain.Children.Clear();
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
            this.spMain.Children.Clear();
            DRMain.Instance.DRFullScreenEvent += Instance_DRFullScreenEvent;
            this.spMain.Children.Add(DRMain.Instance);
            DRMain.Instance.PlayCamera();
        }

        /// <summary>
        /// 安全设置
        /// </summary>
        private void MouseDownSecureSetting(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                this.spMain.Children.Add(SFSecureSetting.Instance);
                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdSecureSetting);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
            {
                this.spMain.Children.Add(DRSecureSetting.Instance);
                this.BottomColorSetting(this.bdDR, this.tbDR, this.bdSecureSetting);
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownExit(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }
        /// <summary>
        /// IO查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDownIO(object sender, MouseButtonEventArgs e)
        {
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(SFIOMain.Instance);
                byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 22, 0 });
                GlobalData.Instance.da.SendBytes(byteToSend);

                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdIO);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor) {
                MessageBox.Show("功能未开放!");
                return;
            }
        }
        /// <summary>
        /// 钻杠数量设置
        /// </summary>
        private void MouseDownDrillSetting(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                this.spMain.Children.Add(SFDrillSetting.Instance);
                SFDrillSetting.Instance.SysTypeSelect(0);
                RefreshPipeCount();
                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdDrillSetting);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
            {
                this.spMain.Children.Add(SFDrillSetting.Instance);
                SFDrillSetting.Instance.SysTypeSelect(1);
                byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 7, 1 });
                GlobalData.Instance.da.SendBytes(byteToSend);

                this.BottomColorSetting(this.bdDR, this.tbDR, this.bdDrillSetting); ;
            }
        }
        /// <summary>
        /// 设备状态
        /// </summary>
        private void MouseDownDeviceStatus(object sender, MouseButtonEventArgs e)
        {
            this.spMain.Children.Clear();
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                this.spMain.Children.Add(SFDeviceStatus.Instance);
                SFDeviceStatus.Instance.SwitchDeviceStatusPageEvent += Instance_SwitchDeviceStatusPageEvent;
                gotoEquipStatusPage();
                this.checkMaintain();
                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdDeviceStatus);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
            {
                this.spMain.Children.Add(DRDeviceStatus.Instance);
                this.BottomColorSetting(this.bdDR, this.tbDR, this.bdDeviceStatus);
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
            if (GlobalData.Instance.systemRole != SystemRole.DebugMan)
            {
                MessageBox.Show("您不具备权限！");
                return;
            }
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                if (GlobalData.Instance.da["operationModel"].Value.Byte == 5)//这是判断如果是自动模式，需要切换到手动模式
                {
                    MessageBox.Show("当下模式为自动模式，如需参数配置，请切换至手动模式！");
                    return;
                }
                else
                {
                    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 23, 0 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
                this.spMain.Children.Clear();
                this.spMain.Children.Add(SFParamMain.Instance);
                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
            {
                if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5)//这是判断如果是自动模式，需要切换到手动模式
                {
                    MessageBox.Show("当下模式为自动模式，如需参数配置，请切换至手动模式！");
                    return;
                }
                this.spMain.Children.Clear();
                this.spMain.Children.Add(DRParamSettingMain.Instance);
                this.BottomColorSetting(this.bdDR, this.tbDR, this.bdOther);
            }
        }
        /// <summary>
        /// 位置标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionSetting_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.systemRole > SystemRole.TechMan)
            {
                MessageBox.Show("您不具备权限！");
                return;
            }
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                if (GlobalData.Instance.da["operationModel"].Value.Byte == 5)
                {
                    MessageBox.Show("当下模式为自动模式，如需位置标定，请切换至手动模式！");
                    return;
                }
                else
                {
                    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 12, 0 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
                this.spMain.Children.Clear();
                this.spMain.Children.Add(SFPositionSetting.Instance);
                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor) {
                MessageBox.Show("功能未开放!");
                return;
            }
        }
        /// <summary>
        /// 位置补偿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionCompensate_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.systemRole > SystemRole.AdminMan)
            {
                MessageBox.Show("您不具备权限！");
                return;
            }
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                if (GlobalData.Instance.da["operationModel"].Value.Byte != 4)//这是判断如果是自动模式，需要切换到手动模式
                {
                    MessageBox.Show("当下模式非手动模式，如需位置补偿，请切换至手动模式！");
                    return;
                }
                else
                {
                    byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 9 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                    SFPositionCompensate.Instance.txtCompensationType.Visibility = Visibility.Visible;
                    SFPositionCompensate.Instance.menuCarCompensationSetting.IsEnabled = false;
                    SFPositionCompensate.Instance.menuRotateCompensationSetting.IsEnabled = false;
                    SFPositionCompensate.Instance.ShowRotatePosiCompensaTxts(false);
                    SFPositionCompensate.Instance.ShowCarCompensationTxts(false);
                }
                this.spMain.Children.Clear();
                this.spMain.Children.Add(SFPositionCompensate.Instance);
                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor) {
                MessageBox.Show("功能未开放!");
                return;
            }
        }

        private void Record_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(SFRecordMain.Instance);
                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor) {
                MessageBox.Show("功能未开放!");
                return;
            }
        }

        private void Chart_Clict(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                this.spMain.Children.Clear();
                this.spMain.Children.Add(SFChart.Instance);
                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor) {
                MessageBox.Show("功能未开放!");
                return;
            }
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                this.spMain.Children.Clear();
                SFReport.Instance.ReportTextBlockShow();
                this.spMain.Children.Add(SFReport.Instance);
                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor) {
                MessageBox.Show("功能未开放!");
                return;
            }
        }
        /// <summary>
        /// 下位机信息
        /// </summary>
        private void LowInfo_Click(object sender, RoutedEventArgs e)
        {
            this.spMain.Children.Clear();
            if (GlobalData.Instance.systemType == SystemType.SecondFloor) //二层台
            {
                this.spMain.Children.Add(SFLowInfo.Instance);
                this.BottomColorSetting(this.bdSf, this.tbSf, this.bdOther);
            }
            else if (GlobalData.Instance.systemType == SystemType.DrillFloor) {
                this.spMain.Children.Add(SFLowInfo.Instance);
                this.BottomColorSetting(this.bdDR, this.tbDR, this.bdOther);
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
                this.spMain.Children.Clear();
                DRMain.Instance.DRFullScreenEvent -= Instance_DRFullScreenEvent;
                DRMain.Instance.DRFullScreenEvent += Instance_DRFullScreenEvent;
                this.spMain.Children.Add(DRMain.Instance);

                GlobalData.Instance.systemType = SystemType.DrillFloor;
                this.BottomColorSetting(this.bdDR, this.tbDR, this.bdHome);
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
            this.spMain.Children.Clear();
            this.spMain.Children.Add(IngMain.Instance);
            GlobalData.Instance.systemType = SystemType.SecondFloor;

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

        /// <summary>
        /// ping ip,测试能否ping通
        /// </summary>
        /// <param name="strIP">IP地址</param>
        /// <returns></returns>
        private bool PingIp(string strIP)
        {
            bool bRet = false;
            try
            {
                Ping pingSend = new Ping();
                PingReply reply = pingSend.Send(strIP, 200);
                if (reply.Status == IPStatus.Success)
                    bRet = true;
            }
            catch (Exception)
            {
                bRet = false;
            }
            return bRet;
        }

        private void BottomColorSetting(Border bdLeft,TextBlock tbLeft,Border bdRight)
        {
            var bc = new BrushConverter();
            foreach (Border bd in FindVisualChildren<Border>(this.gdBottom))
            {
                //bd.Background = (Brush)bc.ConvertFrom("#C4DEE8");
                if (bd.Name == "bdHome"|| bd.Name == "bdSf" || bd.Name == "bdDR" || bd.Name == "bdIng" || bd.Name == "bdHome" || bd.Name == "bdDrillSetting"
                    || bd.Name == "bdSecureSetting" || bd.Name == "bdIO" || bd.Name == "bdDeviceStatus" || bd.Name == "bdOther")
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
    }
}
