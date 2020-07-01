using COM.Common;
using ControlLibrary;
using DatabaseLib;
using DemoDriver;
using HBGKTest;
using HBGKTest.YiTongCamera;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

namespace Main.SecondFloor
{
    /// <summary>
    /// SFMain.xaml 的交互逻辑
    /// </summary>
    public partial class SFMain : UserControl
    {
        private static SFMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SFMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFMain();
                        }
                    }
                }
                return _instance;
            }
        }

        public delegate void FullScreenHandler(int camId);

        public event FullScreenHandler FullScreenEvent;

        System.Threading.Timer timerWarning;
        public static readonly DependencyProperty CommunicationProperty = DependencyProperty.Register("Communication", typeof(byte), typeof(SFMain), new PropertyMetadata((byte)0));//1代表通讯正常，2 代表人机界面--操作台通讯异常 3 操作台--二层台通讯异常
        public byte Communication
        {
            get { return (byte)GetValue(CommunicationProperty); }
            set { SetValue(CommunicationProperty, value); }
        }
        Dictionary<string, byte> WarnInfoOne;
        List<string> WarnInfoOneKeys;
        Dictionary<string, byte> WarnInfoThree;
        /// <summary>
        /// 开一个2秒后的定时器用于延时播放视频
        /// </summary>
        System.Timers.Timer cameraTimer;

        //public Amination amination = new Amination();

        public SFMain()
        {
            GlobalData.Instance.Rows = GlobalData.Instance.da["DrillNums"].Value.Byte;
            GlobalData.Instance.DrillNum = GlobalData.Instance.da["103E23B5"].Value.Byte;
            InitializeComponent();
            Init();
            this.Loaded += SFMain_Loaded;
        }

        private void SFMain_Loaded(object sender, RoutedEventArgs e)
        {
            //GlobalData.Instance.Rows = GlobalData.Instance.da["DrillNums"].Value.Byte;
            //GlobalData.Instance.DrillNum = GlobalData.Instance.da["103E23B5"].Value.Byte;
            if (GlobalData.Instance.systemRole == SystemRole.OperMan)
            {
                miSpDrill.Visibility = Visibility.Collapsed;
            }

            GlobalData.Instance.DRNowPage = "SFMain";
        }

        private void Init()
        {
            try
            {
                VariableBinding();
                InitControls();
                InitCameraInfo();

                string configPath = System.Environment.CurrentDirectory + "\\KeyBoard.exe";
                System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
                foreach (System.Diagnostics.Process process in processList)
                {
                    if (process.ProcessName.Contains("KeyBoard"))
                    {
                        process.Kill();
                    }
                }

                timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟

                //cameraTimer = new System.Timers.Timer(2 * 1000);
                //cameraTimer.Elapsed += CameraTimer_Elapsed;
                //cameraTimer.Enabled = true;
                amination.SendFingerBeamNumberEvent += Instance_SendFingerBeamNumberEvent;
                amination.SystemChange(SystemType.SecondFloor);
                //this.Am.Children.Add(amination);
                PlayCameraInThread();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        public void PlayCameraInThread()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2 * 1000);
                PlayCamera();
            });
        }

        private void Instance_SendFingerBeamNumberEvent(byte number)
        {
            if (GlobalData.Instance.da["operationModel"].Value.Byte == 5 || GlobalData.Instance.da["operationModel"].Value.Byte == 3)
            {
                byte[] byteToSend = SendByte(new List<byte> { 5, number });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        private void InitControls()
        {
            cameraInitImage1.Source = new BitmapImage(new Uri("../Images/camera.jpg", UriKind.Relative));

            gridCamera1.Children.Add(cameraInitImage1);

            cameraInitImage2.Source = new BitmapImage(new Uri("../Images/camera.jpg", UriKind.Relative));
            gridCamera2.Children.Add(cameraInitImage2);
        }

        #region 报表字段 各类标志
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private bool bCommunicationCheck = false; // 是否有中断标志
        private bool bPre506b5 = false;
        private bool bPre506b6 = false;
        private bool bPre506b7 = false;
        private bool bPre130b0 = false;
        private bool bPre130b1 = false;
        private bool bPre116B5b1 = false;
        private bool bPre116B5b3 = false;
        private byte bPreMotorAlarmPrompt = 0;
        private bool bPre103N23B7b0 = false;
        private bool bPre103N23B7b1 = false;
        private bool bPre103N23B7b2 = false;
        private bool bPre103N23B7b3 = false;
        private bool bPre103N23B7b4 = false;
        private bool bPre103N23B7b5 = false;
        private bool bPre105N2N22B4b0 = false;
        private bool bPre105N2N22B4b1 = false;
        private bool bPre105N2N22B4b2 = false;
        private bool bPre105N2N22B4b3 = false;
        private bool bPre105N2N22B4b4 = false;
        private bool bPre105N2N22B4b5 = false;
        private bool bPre105N2N22B4b6 = false;
        private bool bPre105N2N22B4b7 = false;
        private bool bPre105N2N22B5b0 = false;
        private bool bPre105N2N22B5b1 = false;
        private bool bPre105N2N22B5b2 = false;
        private bool bPre105N2N22B5b3 = false;
        private bool bPre105N2N22B5b4 = false;
        private bool bPre105N2N22B5b5 = false;
        private bool bPre105N2N22B5b6 = false;
        private bool bPre105N2N22B5b7 = false;
        private bool bPre105N2N22B6b0 = false;
        private bool bPre105N2N22B6b1 = false;
        private bool bPre105N2N22B6b2 = false;
        private bool bPre105N2N22B6b3 = false;
        private bool bPre105N2N22B6b4 = false;
        private bool bPre105N2N22B6b5 = false;
        private bool bPre105N2N22B6b6 = false;
        private bool bPre105N2N22B6b7 = false;
        private bool bPre123b0 = false;
        private bool bPre123b1 = false;
        private bool bPre123b2 = false;
        private bool bPre123b3 = false;
        private bool bPre123b4 = false;
        private bool bPre123b5 = false;
        private bool bPre123b6 = false;
        private bool bPre123b7 = false;
        #endregion
        private int iTimeCnt = 0;//用来为时钟计数的变量

        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Communcation();
                    this.Warnning();
                    this.ReportDataUpdate();
                    amination.LoadFingerBeamDrillPipe(SystemType.SecondFloor);
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
            // UDP通信，操作台/二层台心跳都正常，通信正常
            if (GlobalData.Instance.da.NetStatus && !GlobalData.Instance.da["508b6"].Value.Boolean && !GlobalData.Instance.da["508b5"].Value.Boolean)
            {
                //Communication = 1;
            }

            //if (!GlobalData.Instance.da.NetStatus) Communication = 0; // UDP建立成功/失败标志

            //操作台控制器心跳
            if (GlobalData.Instance.da["508b5"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 600)
                {
                    //Communication = 2;
                    this.warnOne.Text = "操作台信号中断";
                }
                if (!bCommunicationCheck && controlHeartTimes > 600)
                {
                    GlobalData.Instance.reportData.OperationFloorCommunication += 1;//the report
                    bCommunicationCheck = true;
                }
            }
            else
            {
                this.controlHeartTimes = 0;
            }
            this.tmpStatus = GlobalData.Instance.da["508b6"].Value.Boolean;

            //二层台控制器心跳
            if (GlobalData.Instance.da["508b6"].Value.Boolean)
            {
                //Communication = 3;
                this.warnOne.Text = "二层台信号中断";
                if (!bCommunicationCheck)
                {
                    GlobalData.Instance.reportData.OperationFloorCommunication += 1;//the report
                    bCommunicationCheck = true;
                }
            }
            else
            {
                bCommunicationCheck = false;
            }

            if (!GlobalData.Instance.ComunciationNormal) this.warnOne.Text = "网络连接失败！";
            #endregion
        }
        /// <summary>
        /// 更新报表数据
        /// </summary>
        private void ReportDataUpdate()
        {
            if (GlobalData.Instance.da["506b5"].Value.Boolean && !bPre506b5)
            {
                bPre506b5 = GlobalData.Instance.da["506b5"].Value.Boolean;
                GlobalData.Instance.reportData.RobotBigHookInterLock += 1;
            }
            else
            {
                bPre506b5 = GlobalData.Instance.da["506b5"].Value.Boolean;
            }



            if (GlobalData.Instance.da["506b6"].Value.Boolean && !bPre506b6)
            {
                bPre506b6 = GlobalData.Instance.da["506b6"].Value.Boolean;
                GlobalData.Instance.reportData.RobotTopDriveInterlock += 1;
            }
            else
            {
                bPre506b6 = GlobalData.Instance.da["506b6"].Value.Boolean;
            }


            if (GlobalData.Instance.da["506b7"].Value.Boolean && !bPre506b7)
            {
                bPre506b7 = GlobalData.Instance.da["506b7"].Value.Boolean;
                GlobalData.Instance.reportData.RobotElevatorInterlock += 1;
            }
            else
            {
                bPre506b7 = GlobalData.Instance.da["506b7"].Value.Boolean;
            }


            if (GlobalData.Instance.da["130b0"].Value.Boolean && GlobalData.Instance.da["130b1"].Value.Boolean && !(bPre130b0 && bPre130b1))
            {
                bPre130b0 = GlobalData.Instance.da["130b0"].Value.Boolean;
                bPre130b1 = GlobalData.Instance.da["130b1"].Value.Boolean;
                GlobalData.Instance.reportData.ElevatorBigHookInterlock += 1;
            }
            else
            {
                bPre130b0 = GlobalData.Instance.da["130b0"].Value.Boolean;
                bPre130b1 = GlobalData.Instance.da["130b1"].Value.Boolean;
            }


            if (GlobalData.Instance.da["operationModel"].Value.Byte != 8 && !GlobalData.Instance.da["116E1E2E4B5b1"].Value.Boolean && bPre116B5b1)
            {
                bPre116B5b1 = GlobalData.Instance.da["116E1E2E4B5b1"].Value.Boolean;
                GlobalData.Instance.reportData.RobotRetainingRopeInterlock += 1;
            }
            else
            {
                bPre116B5b1 = GlobalData.Instance.da["116E1E2E4B5b1"].Value.Boolean;
            }


            if (GlobalData.Instance.da["operationModel"].Value.Byte != 8 && !GlobalData.Instance.da["116E1E2E4B5b3"].Value.Boolean && bPre116B5b3)
            {
                bPre116B5b3 = GlobalData.Instance.da["116E1E2E4B5b3"].Value.Boolean;
                GlobalData.Instance.reportData.RobotFingerBeamLockInterlock += 1;
            }
            else
            {
                bPre116B5b3 = GlobalData.Instance.da["116E1E2E4B5b3"].Value.Boolean;
            }

            if (GlobalData.Instance.da["motorAlarmPrompt"].Value.Byte == 1 && GlobalData.Instance.da["motorAlarmPrompt"].Value.Byte != bPreMotorAlarmPrompt)
            {
                bPreMotorAlarmPrompt = GlobalData.Instance.da["motorAlarmPrompt"].Value.Byte;
                GlobalData.Instance.reportData.CarMotorAlarm += 1;
            }
            else if (GlobalData.Instance.da["motorAlarmPrompt"].Value.Byte == 2 && GlobalData.Instance.da["motorAlarmPrompt"].Value.Byte != bPreMotorAlarmPrompt)
            {
                bPreMotorAlarmPrompt = GlobalData.Instance.da["motorAlarmPrompt"].Value.Byte;
                GlobalData.Instance.reportData.ArmMotorAlarm += 1;
            }
            else if (GlobalData.Instance.da["motorAlarmPrompt"].Value.Byte == 3 && GlobalData.Instance.da["motorAlarmPrompt"].Value.Byte != bPreMotorAlarmPrompt)
            {
                bPreMotorAlarmPrompt = GlobalData.Instance.da["motorAlarmPrompt"].Value.Byte;
                GlobalData.Instance.reportData.RotateMotorAlarm += 1;
            }
            else
            {
                bPreMotorAlarmPrompt = GlobalData.Instance.da["motorAlarmPrompt"].Value.Byte;
            }

            if (GlobalData.Instance.da["103N23B7b0"].Value.Boolean && !bPre103N23B7b0 && GlobalData.Instance.da["Con_Set0"].Value.Byte == 23)
            {
                bPre103N23B7b0 = GlobalData.Instance.da["103N23B7b0"].Value.Boolean;
                GlobalData.Instance.reportData.GripMotorAlarm += 1;
            }
            else
            {
                bPre103N23B7b0 = GlobalData.Instance.da["103N23B7b0"].Value.Boolean;
            }


            if (GlobalData.Instance.da["103N23B7b1"].Value.Boolean && !bPre103N23B7b1 && GlobalData.Instance.da["Con_Set0"].Value.Byte == 23)
            {
                bPre103N23B7b1 = GlobalData.Instance.da["103N23B7b1"].Value.Boolean;
                GlobalData.Instance.reportData.FingerMotorAlarm += 1;
            }
            else
            {
                bPre103N23B7b1 = GlobalData.Instance.da["103N23B7b1"].Value.Boolean;
            }


            if (GlobalData.Instance.da["103N23B7b2"].Value.Boolean && !bPre103N23B7b2 && GlobalData.Instance.da["Con_Set0"].Value.Byte == 23)
            {
                bPre103N23B7b2 = GlobalData.Instance.da["103N23B7b2"].Value.Boolean;
                GlobalData.Instance.reportData.FingerMotorAlarm += 1;
            }
            else
            {
                bPre103N23B7b2 = GlobalData.Instance.da["103N23B7b2"].Value.Boolean;
            }


            if (GlobalData.Instance.da["103N23B7b3"].Value.Boolean && !bPre103N23B7b3 && GlobalData.Instance.da["Con_Set0"].Value.Byte == 23)
            {
                bPre103N23B7b3 = GlobalData.Instance.da["103N23B7b3"].Value.Boolean;
                GlobalData.Instance.reportData.GripMotorAlarm += 1;
            }
            else
            {
                bPre103N23B7b3 = GlobalData.Instance.da["103N23B7b3"].Value.Boolean;
            }


            if (GlobalData.Instance.da["103N23B7b4"].Value.Boolean && !bPre103N23B7b4 && GlobalData.Instance.da["Con_Set0"].Value.Byte == 23)
            {
                bPre103N23B7b4 = GlobalData.Instance.da["103N23B7b4"].Value.Boolean;
                GlobalData.Instance.reportData.FingerMotorAlarm += 1;
            }
            else
            {
                bPre103N23B7b4 = GlobalData.Instance.da["103N23B7b4"].Value.Boolean;
            }


            if (GlobalData.Instance.da["103N23B7b5"].Value.Boolean && !bPre103N23B7b5 && GlobalData.Instance.da["Con_Set0"].Value.Byte == 23)
            {
                bPre103N23B7b5 = GlobalData.Instance.da["103N23B7b5"].Value.Boolean;
                GlobalData.Instance.reportData.FingerMotorAlarm += 1;
            }
            else
            {
                bPre103N23B7b5 = GlobalData.Instance.da["103N23B7b5"].Value.Boolean;
            }

            // UDP模式下，无条件
            #region 120位
            //if (da["105N2N22B4b0"].Value.Boolean && !bPre105N2N22B4b0 && da["101B7b5"].Value.Boolean && da["operationModel"].Value.Byte != 2 && da["101B7b4"].Value.Boolean)
            // 1#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B4b0"].Value.Boolean && !bPre105N2N22B4b0)
            {
                bPre105N2N22B4b0 = GlobalData.Instance.da["105N2N22B4b0"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B4b0 = GlobalData.Instance.da["105N2N22B4b0"].Value.Boolean;
            }

            //if (da["105N2N22B4b1"].Value.Boolean && !bPre105N2N22B4b1 && da["101B7b5"].Value.Boolean && da["operationModel"].Value.Byte != 2 && da["101B7b4"].Value.Boolean)
            // 1#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B4b1"].Value.Boolean && !bPre105N2N22B4b1)
            {
                bPre105N2N22B4b1 = GlobalData.Instance.da["105N2N22B4b1"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B4b1 = GlobalData.Instance.da["105N2N22B4b1"].Value.Boolean;
            }

            //if (da["105N2N22B4b2"].Value.Boolean && !bPre105N2N22B4b2 && da["101B7b5"].Value.Boolean && da["operationModel"].Value.Byte != 2 && da["101B7b4"].Value.Boolean)
            // 2#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B4b2"].Value.Boolean && !bPre105N2N22B4b2)
            {
                bPre105N2N22B4b2 = GlobalData.Instance.da["105N2N22B4b2"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B4b2 = GlobalData.Instance.da["105N2N22B4b2"].Value.Boolean;
            }

            //if (da["105N2N22B4b3"].Value.Boolean && !bPre105N2N22B4b3 && da["101B7b5"].Value.Boolean && da["operationModel"].Value.Byte != 2 && da["101B7b4"].Value.Boolean)
            // 2#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B4b3"].Value.Boolean && !bPre105N2N22B4b3)
            {
                bPre105N2N22B4b3 = GlobalData.Instance.da["105N2N22B4b3"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B4b3 = GlobalData.Instance.da["105N2N22B4b3"].Value.Boolean;
            }


            //if (da["105N2N22B4b4"].Value.Boolean && !bPre105N2N22B4b4 && da["101B7b5"].Value.Boolean && da["operationModel"].Value.Byte != 2 && da["101B7b4"].Value.Boolean)
            // 3#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B4b4"].Value.Boolean && !bPre105N2N22B4b4)

            {
                bPre105N2N22B4b4 = GlobalData.Instance.da["105N2N22B4b4"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B4b4 = GlobalData.Instance.da["105N2N22B4b4"].Value.Boolean;
            }


            //if (da["105N2N22B4b5"].Value.Boolean && !bPre105N2N22B4b5 && da["101B7b5"].Value.Boolean && da["operationModel"].Value.Byte != 2 && da["101B7b4"].Value.Boolean)
            // 3#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B4b5"].Value.Boolean && !bPre105N2N22B4b5)
            {
                bPre105N2N22B4b5 = GlobalData.Instance.da["105N2N22B4b5"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B4b5 = GlobalData.Instance.da["105N2N22B4b5"].Value.Boolean;
            }


            //if (da["105N2N22B4b6"].Value.Boolean && !bPre105N2N22B4b6 && da["101B7b5"].Value.Boolean && da["operationModel"].Value.Byte != 2 && !da["101B7b4"].Value.Boolean)
            // 4#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B4b6"].Value.Boolean && !bPre105N2N22B4b6)
            {
                bPre105N2N22B4b6 = GlobalData.Instance.da["105N2N22B4b6"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B4b6 = GlobalData.Instance.da["105N2N22B4b6"].Value.Boolean;
            }


            //if (da["105N2N22B4b7"].Value.Boolean && !bPre105N2N22B4b7 && da["101B7b5"].Value.Boolean && da["operationModel"].Value.Byte != 2 && da["101B7b4"].Value.Boolean)
            // 4#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B4b7"].Value.Boolean && !bPre105N2N22B4b7)
            {
                bPre105N2N22B4b7 = GlobalData.Instance.da["105N2N22B4b7"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B4b7 = GlobalData.Instance.da["105N2N22B4b7"].Value.Boolean;
            }
            #endregion

            #region 121位
            //if (da["105N2N22B5b0"].Value.Boolean && !bPre105N2N22B5b0 && da["101B7b5"].Value.Boolean && da["operationModel"].Value.Byte != 2 && da["101B7b4"].Value.Boolean)
            // 5#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B5b0"].Value.Boolean && !bPre105N2N22B5b0)
            {
                bPre105N2N22B5b0 = GlobalData.Instance.da["105N2N22B5b0"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B5b0 = GlobalData.Instance.da["105N2N22B5b0"].Value.Boolean;
            }

            // 5#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B5b1"].Value.Boolean && !bPre105N2N22B5b1)
            {
                bPre105N2N22B5b1 = GlobalData.Instance.da["105N2N22B5b1"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B5b1 = GlobalData.Instance.da["105N2N22B5b1"].Value.Boolean;
            }

            // 6#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B5b2"].Value.Boolean && !bPre105N2N22B5b2)
            {
                bPre105N2N22B5b2 = GlobalData.Instance.da["105N2N22B5b2"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B5b2 = GlobalData.Instance.da["105N2N22B5b2"].Value.Boolean;
            }

            // 6#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B5b3"].Value.Boolean && !bPre105N2N22B5b3)
            {
                bPre105N2N22B5b3 = GlobalData.Instance.da["105N2N22B5b3"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B5b3 = GlobalData.Instance.da["105N2N22B5b3"].Value.Boolean;
            }

            // 7#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B5b4"].Value.Boolean && !bPre105N2N22B5b4)
            {
                bPre105N2N22B5b4 = GlobalData.Instance.da["105N2N22B5b4"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B5b4 = GlobalData.Instance.da["105N2N22B5b4"].Value.Boolean;
            }

            // 7#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B5b5"].Value.Boolean && !bPre105N2N22B5b5)
            {
                bPre105N2N22B5b5 = GlobalData.Instance.da["105N2N22B5b5"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B5b5 = GlobalData.Instance.da["105N2N22B5b5"].Value.Boolean;
            }

            // 8#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B5b6"].Value.Boolean && !bPre105N2N22B5b6)
            {
                bPre105N2N22B5b6 = GlobalData.Instance.da["105N2N22B5b6"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B5b6 = GlobalData.Instance.da["105N2N22B5b6"].Value.Boolean;
            }

            // 8#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B5b7"].Value.Boolean && !bPre105N2N22B5b7)
            {
                bPre105N2N22B5b7 = GlobalData.Instance.da["105N2N22B5b7"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B5b7 = GlobalData.Instance.da["105N2N22B5b7"].Value.Boolean;
            }
            #endregion

            #region 122 位
            // 9#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B6b0"].Value.Boolean && !bPre105N2N22B6b0)
            {
                bPre105N2N22B6b0 = GlobalData.Instance.da["105N2N22B6b0"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B6b0 = GlobalData.Instance.da["105N2N22B6b0"].Value.Boolean;
            }

            // 9#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B6b1"].Value.Boolean && !bPre105N2N22B6b1)
            {
                bPre105N2N22B6b1 = GlobalData.Instance.da["105N2N22B6b1"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B6b1 = GlobalData.Instance.da["105N2N22B6b1"].Value.Boolean;
            }

            // 10#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B6b2"].Value.Boolean && !bPre105N2N22B6b2)
            {
                bPre105N2N22B6b2 = GlobalData.Instance.da["105N2N22B6b2"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B6b2 = GlobalData.Instance.da["105N2N22B6b2"].Value.Boolean;
            }

            // 10#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B6b3"].Value.Boolean && !bPre105N2N22B6b3)
            {
                bPre105N2N22B6b3 = GlobalData.Instance.da["105N2N22B6b3"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B6b3 = GlobalData.Instance.da["105N2N22B6b3"].Value.Boolean;
            }

            // 11#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B6b4"].Value.Boolean && !bPre105N2N22B6b4)
            {
                bPre105N2N22B6b4 = GlobalData.Instance.da["105N2N22B6b4"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B6b4 = GlobalData.Instance.da["105N2N22B6b4"].Value.Boolean;
            }

            // 11#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B6b5"].Value.Boolean && !bPre105N2N22B6b5)
            {
                bPre105N2N22B6b5 = GlobalData.Instance.da["105N2N22B6b5"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B6b5 = GlobalData.Instance.da["105N2N22B6b5"].Value.Boolean;
            }

            // 12#12#钻铤锁打开卡滞
            if (GlobalData.Instance.da["105N2N22B6b6"].Value.Boolean && !bPre105N2N22B6b6)
            {
                bPre105N2N22B6b6 = GlobalData.Instance.da["105N2N22B6b6"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B6b6 = GlobalData.Instance.da["105N2N22B6b6"].Value.Boolean;
            }

            // 12#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["105N2N22B6b7"].Value.Boolean && !bPre105N2N22B6b7)
            {
                bPre105N2N22B6b7 = GlobalData.Instance.da["105N2N22B6b7"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre105N2N22B6b7 = GlobalData.Instance.da["105N2N22B6b7"].Value.Boolean;
            }
            #endregion

            #region 123 位
            // 13#钻铤锁打开卡滞
            if (GlobalData.Instance.da["123b0"].Value.Boolean && !bPre123b0)
            {
                bPre123b0 = GlobalData.Instance.da["123b0"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre123b0 = GlobalData.Instance.da["123b0"].Value.Boolean;
            }

            // 13#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["123b1"].Value.Boolean && !bPre123b1)
            {
                bPre123b1 = GlobalData.Instance.da["123b1"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre123b1 = GlobalData.Instance.da["123b1"].Value.Boolean;
            }

            // 14#钻铤锁打开卡滞
            if (GlobalData.Instance.da["123b2"].Value.Boolean && !bPre123b2)
            {
                bPre123b2 = GlobalData.Instance.da["123b2"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre123b2 = GlobalData.Instance.da["123b2"].Value.Boolean;
            }

            // 14#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["123b3"].Value.Boolean && !bPre123b3)
            {
                bPre123b3 = GlobalData.Instance.da["123b3"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre123b3 = GlobalData.Instance.da["123b3"].Value.Boolean;
            }

            // 15#钻铤锁打开卡滞
            if (GlobalData.Instance.da["123b4"].Value.Boolean && !bPre123b4)
            {
                bPre123b4 = GlobalData.Instance.da["123b4"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre123b4 = GlobalData.Instance.da["123b4"].Value.Boolean;
            }

            // 15#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["123b5"].Value.Boolean && !bPre123b5)
            {
                bPre123b5 = GlobalData.Instance.da["123b5"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre123b5 = GlobalData.Instance.da["123b5"].Value.Boolean;
            }

            // 16#钻铤锁打开卡滞
            if (GlobalData.Instance.da["123b6"].Value.Boolean && !bPre123b6)
            {
                bPre123b6 = GlobalData.Instance.da["123b6"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre123b6 = GlobalData.Instance.da["123b6"].Value.Boolean;
            }

            // 16#钻铤锁关闭卡滞
            if (GlobalData.Instance.da["123b7"].Value.Boolean && !bPre123b7)
            {
                bPre123b7 = GlobalData.Instance.da["123b7"].Value.Boolean;
                GlobalData.Instance.reportData.DrillCollarMotorAlarm += 1;
            }
            else
            {
                bPre123b7 = GlobalData.Instance.da["123b7"].Value.Boolean;
            }
            #endregion
        }

        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
            byte byteKey;
            foreach (string item in WarnInfoOneKeys)
            {
                if (item.Contains("103") && GlobalData.Instance.da["Con_Set0"].Value.Byte != 23)
                {
                    if (GlobalData.Instance.da[item].Value.Boolean)
                    {
                        WarnInfoOne.TryGetValue(GlobalData.Instance.da[item].Description, out byteKey);
                        if (byteKey == 0)//值为零 ，表示字典中还没有该提示信息，为 1 代表还没有显示，为 2 代表已经显示
                        {
                            WarnInfoOne.Add(GlobalData.Instance.da[item].Description, 1);
                        }
                    }
                    else
                    {
                        WarnInfoOne.TryGetValue(GlobalData.Instance.da[item].Description, out byteKey);
                        if (byteKey != 0)
                        {
                            WarnInfoOne.Remove(GlobalData.Instance.da[item].Description);
                        }
                    }
                }
                else
                {
                    if (GlobalData.Instance.da["Con_Set0"].Value.Byte != 23 && GlobalData.Instance.da["Con_Set0"].Value.Byte != 22 && (GlobalData.Instance.da["operationModel"].Value.Byte != 2))
                    {
                        if (GlobalData.Instance.da[item].Value.Boolean)
                        {
                            WarnInfoOne.TryGetValue(GlobalData.Instance.da[item].Description, out byteKey);
                            if (byteKey == 0)
                            {
                                WarnInfoOne.Add(GlobalData.Instance.da[item].Description, 1);
                            }
                        }
                        else
                        {
                            WarnInfoOne.TryGetValue(GlobalData.Instance.da[item].Description, out byteKey);
                            if (byteKey != 0)
                            {
                                WarnInfoOne.Remove(GlobalData.Instance.da[item].Description);
                            }
                        }
                    }
                }
            }
            if (!GlobalData.Instance.da["carMotorRetZeroStatus"].Value.Boolean || !GlobalData.Instance.da["armMotorRetZeroStatus"].Value.Boolean || !GlobalData.Instance.da["rotateMotorRetZeroStatus"].Value.Boolean)//主界面左下去提示
            {
                WarnInfoThree.TryGetValue("机械手未回零，位置未知，请注意防碰！", out byteKey);
                if (byteKey == 0)
                {
                    WarnInfoThree.Add("机械手未回零，位置未知，请注意防碰！", 1);
                }
            }
            else
            {
                WarnInfoThree.TryGetValue("机械手未回零，位置未知，请注意防碰！", out byteKey);
                if (byteKey != 0)
                {
                    WarnInfoThree.Remove("机械手未回零，位置未知，请注意防碰！");
                }
            }

            //if (Communication == 1)
            //{
                if (!GlobalData.Instance.da["101B7b1"].Value.Boolean)
                {
                    WarnInfoThree.TryGetValue("机械手已进入防碰区，请注意防碰！", out byteKey);
                    if (byteKey == 0)
                    {
                        WarnInfoThree.Add("机械手已进入防碰区，请注意防碰！", 1);
                    }
                }
                else
                {
                    WarnInfoThree.TryGetValue("机械手已进入防碰区，请注意防碰！", out byteKey);
                    if (byteKey != 0)
                    {
                        WarnInfoThree.Remove("机械手已进入防碰区，请注意防碰！");
                    }
                }
            //}
            //else
            //{
            //    WarnInfoThree.TryGetValue("机械手已进入防碰区，请注意防碰！", out byteKey);
            //    if (byteKey != 0)
            //    {
            //        WarnInfoThree.Remove("机械手已进入防碰区，请注意防碰！");
            //    }
            //}

            switch (GlobalData.Instance.da["promptInfo"].Value.Byte)
            {
                case 0:
                    warnTwo.Text = "";
                    break;
                case 1:
                    warnTwo.Text = "小车电机报警";
                    break;
                case 2:
                    warnTwo.Text = "手臂电机报警";
                    break;
                case 3:
                    warnTwo.Text = "回转电机报警";
                    break;
                case 6:
                    warnTwo.Text = "非安全条件，机械手禁止向井口移动！";
                    break;
                case 7:
                    warnTwo.Text = "吊卡未打开，机械手禁止缩回！";
                    break;
                case 8:
                    warnTwo.Text = "请将挡绳缩回后再回零！";
                    break;
                case 9:
                    warnTwo.Text = "抓手中有钻杆，手指已经打开，请注意安全！";
                    break;
                case 10:
                    warnTwo.Text = "此位置禁止打开手指！";
                    break;
                case 11:
                    warnTwo.Text = "此位置禁止打开抓手！";
                    break;
                case 12:
                    warnTwo.Text = "此位置抓手不允许关闭！";
                    break;
                case 13:
                    warnTwo.Text = "小车电机已回零完成！";
                    break;
                case 14:
                    warnTwo.Text = "手臂电机已回零完成!";
                    break;
                case 15:
                    warnTwo.Text = "回转电机（机械手）已完成回零！";
                    break;
                case 31:
                    warnTwo.Text = "抓手中有钻杆，请将钻杆取出后再回零！";
                    break;
                case 32:
                    warnTwo.Text = "请先将手臂电机回零！";
                    break;
                case 33:
                    warnTwo.Text = "请注意安全，小车电机正在回零……";
                    break;
                case 34:
                    warnTwo.Text = "请先将手臂缩回！";
                    break;
                case 35:
                    warnTwo.Text = "请注意安全，手臂电机正在回零……";
                    break;
                case 36:
                    warnTwo.Text = "请先将小车电机正在回零！";
                    break;
                case 37:
                    warnTwo.Text = "请注意安全，回转电机正在回零……";
                    break;
                case 38:
                    warnTwo.Text = "请将小车移动到中间靠井口位置！";
                    break;
                case 39:
                    warnTwo.Text = "所选择的电机已经回零完成！";
                    break;
                case 40:
                    warnTwo.Text = "机械手还未回零，请注意安全！";
                    break;
                case 41:
                    warnTwo.Text = "请先选择管柱类型！";
                    break;
                case 42:
                    warnTwo.Text = "请选择目标指梁号！";
                    break;
                case 43:
                    warnTwo.Text = "未感应到回转回零传感器,请检查!";
                    break;
                case 44:
                    warnTwo.Text = "所选指梁钻杆已满，请切换！";
                    break;
                case 45:
                    warnTwo.Text = "请按确认键启动！";
                    break;
                case 46:
                    warnTwo.Text = "请确认指梁锁已打开到位！";
                    break;
                case 47:
                    warnTwo.Text = "手指正在打开……";
                    break;
                case 48:
                    warnTwo.Text = "抓手正在打开……";
                    break;
                case 49:
                    warnTwo.Text = "请确认吊卡是否在二层台上方！";
                    break;
                case 50:
                    warnTwo.Text = "请确认吊卡已经打开！";
                    break;
                case 51:
                    warnTwo.Text = "吊卡未打开！";
                    break;
                case 52:
                    warnTwo.Text = "请操作手柄！";
                    break;
                case 53:
                    warnTwo.Text = "手指和抓手未打开！";
                    break;
                case 54:
                    warnTwo.Text = "自动排管完成！";
                    break;
                case 55:
                    warnTwo.Text = "所选指梁钻杆已空，请切换！";
                    break;
                case 56:
                    warnTwo.Text = "指梁内抓杆失败！";
                    break;
                case 57:
                    warnTwo.Text = "抓手正在关闭……";
                    break;
                case 58:
                    warnTwo.Text = "手指正在关闭……";
                    break;
                case 59:
                    warnTwo.Text = "人工确认吊卡关闭！";
                    break;
                case 60:
                    warnTwo.Text = "自动送杆完成！";
                    break;
                case 61:
                    warnTwo.Text = "请按确认键启动回收模式！";
                    break;
                case 62:
                    warnTwo.Text = "机械手已回收完成！";
                    break;
                case 65:
                    warnTwo.Text = "请先收回手臂再操作！";
                    break;
                case 66:
                    warnTwo.Text = "此位置不能进行手臂伸出和回转操作！";
                    break;
                case 67:
                    warnTwo.Text = "此位置不能旋转！";
                    break;
                case 68:
                    warnTwo.Text = "此位置不能伸出手臂！";
                    break;
                case 71:
                    warnTwo.Text = "请按确认键启动运输模式！";
                    break;
                case 72:
                    warnTwo.Text = "机械手已完成运输模式！";
                    break;
                case 73:
                    warnTwo.Text = "请谨慎确认钻杆已送入吊卡！";
                    break;
                case 74:
                    warnTwo.Text = "请谨慎确认手臂已伸到位！";
                    break;
                case 75:
                    warnTwo.Text = "请确认手指开合状态！";
                    break;
                case 81:
                    warnTwo.Text = "记录的两点太近！";
                    break;
                case 82:
                    warnTwo.Text = "示教过程存在双轴运动！";
                    break;
                case 83:
                    warnTwo.Text = "记录点数超出限制！";
                    break;
                case 84:
                    warnTwo.Text = "记录点数为零！";
                    break;
                case 85:
                    warnTwo.Text = "即将进入下一次示教循环！";
                    break;
                case 96:
                    warnTwo.Text = "小车电机动作卡滞！";
                    break;
                case 97:
                    warnTwo.Text = "手臂电机动作卡滞！";
                    break;
                case 98:
                    warnTwo.Text = "回转电机动作卡滞！";
                    break;
                default:
                    warnTwo.Text = "";
                    break;

            }

            if (iTimeCnt % 10 == 0)
            {
                if (WarnInfoThree.Count > 0)
                {
                    warnThree.FontSize = 14;
                    warnThree.Visibility = Visibility.Visible;

                    if (!WarnInfoThree.ContainsValue(1))
                    {
                        WarnInfoThree.Keys.ToList().ForEach(k => WarnInfoThree[k] = 1);
                    }

                    foreach (var key in WarnInfoThree.Keys.ToList())
                    {
                        if (WarnInfoThree[key] == 1)
                        {
                            warnThree.Text = key;
                            WarnInfoThree[key] = 2;
                            break;
                        }
                    }
                }
                else
                {
                    warnThree.Visibility = Visibility.Hidden;
                    warnThree.Text = "";
                }

                if (WarnInfoOne.Count > 0 && !WarnInfoOne.ContainsValue(1))//虽然字典中有值，但是没有需要显示的，于是全部重新置为1（重新需要显示）
                {
                    WarnInfoOne.Keys.ToList().ForEach(k => WarnInfoOne[k] = 1);
                }
                else if (WarnInfoOne.Count == 0)
                {
                    warnOne.Text = "";
                }

                foreach (var key in WarnInfoOne.Keys.ToList())
                {
                    if (WarnInfoOne[key] == 1)
                    {
                        warnOne.Text = key;
                        WarnInfoOne[key] = 2;
                        break;
                    }
                }
            }
            else
            {
                warnThree.FontSize = 20;
            }
        }

        private void VariableBinding()
        {
            this.rotateAngle.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });
            this.bigHookRealTimeValue.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["156To159BigHookEncoderRealTimeValue"], Mode = BindingMode.OneWay });
            this.bigHookCalibrationValue.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["160To163BigHookEncoderCalibrationValue"], Mode = BindingMode.OneWay });
            //this.leftFinger.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23LeftFingerMotorSampleValue"], Mode = BindingMode.OneWay });
            //this.rightFinger.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23RightFingerMotorSampleValue"], Mode = BindingMode.OneWay });
            this.gripMotor.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["GripActualValue"], Mode = BindingMode.OneWay });

            this.carMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.armMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.rotateMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["rotateMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.carMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
            this.armMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
            this.rotateMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["rotateMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });

            RopeMultiConverter leftRopeMultiConverter = new RopeMultiConverter();
            MultiBinding leftRopeMultiBind = new MultiBinding();
            leftRopeMultiBind.Converter = leftRopeMultiConverter;
            leftRopeMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b0"], Mode = BindingMode.OneWay });
            leftRopeMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b1"], Mode = BindingMode.OneWay });
            leftRopeMultiBind.NotifyOnSourceUpdated = true;
            this.leftRope.SetBinding(TextBlock.TextProperty, leftRopeMultiBind);
            RopeMultiConverter rightRopeMultiConverter = new RopeMultiConverter();
            MultiBinding rightRopeMultiBind = new MultiBinding();
            rightRopeMultiBind.Converter = rightRopeMultiConverter;
            rightRopeMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b2"], Mode = BindingMode.OneWay });
            rightRopeMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b3"], Mode = BindingMode.OneWay });
            rightRopeMultiBind.NotifyOnSourceUpdated = true;
            this.rightRope.SetBinding(TextBlock.TextProperty, rightRopeMultiBind);

            this.operateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
            this.operateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
            this.workMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
            this.workMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelIsCheckConverter() });

            this.bigHookCalibrationStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            //this.tubeType.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drillPipeType"], Mode = BindingMode.OneWay, Converter = new DrillPipeTypeConverter() });

            MultiBinding stepOneMultiBind = new MultiBinding();
            stepOneMultiBind.Converter = new AutoModeNowStepCoverter();
            stepOneMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            stepOneMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
            stepOneMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
            stepOneMultiBind.ConverterParameter = "one";
            stepOneMultiBind.NotifyOnSourceUpdated = true;
            this.stepOne.SetBinding(TextBlock.TextProperty, stepOneMultiBind);

            MultiBinding stepOneForeColorMultiBind = new MultiBinding();
            stepOneForeColorMultiBind.Converter = new AutoModeForeColorCoverter();
            stepOneForeColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            stepOneForeColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
            stepOneForeColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
            stepOneForeColorMultiBind.ConverterParameter = "foreColorOne";
            stepOneForeColorMultiBind.NotifyOnSourceUpdated = true;
            this.stepOne.SetBinding(TextBlock.ForegroundProperty, stepOneForeColorMultiBind);

            MultiBinding stepOneBackColorMultiBind = new MultiBinding();
            stepOneBackColorMultiBind.Converter = new AutoModeBackColorCoverter();
            stepOneBackColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            stepOneBackColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
            stepOneBackColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
            stepOneBackColorMultiBind.ConverterParameter = "backColorOne";
            stepOneBackColorMultiBind.NotifyOnSourceUpdated = true;
            this.bdOne.SetBinding(Border.BackgroundProperty, stepOneBackColorMultiBind);


            MultiBinding stepTwoMultiBind = new MultiBinding();
            stepTwoMultiBind.Converter = new AutoModeNowStepCoverter();
            stepTwoMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            stepTwoMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
            stepTwoMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
            stepTwoMultiBind.ConverterParameter = "two";
            stepTwoMultiBind.NotifyOnSourceUpdated = true;
            this.stepTwo.SetBinding(TextBlock.TextProperty, stepTwoMultiBind);

            MultiBinding stepTwoForeColorMultiBind = new MultiBinding();
            stepTwoForeColorMultiBind.Converter = new AutoModeForeColorCoverter();
            stepTwoForeColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            stepTwoForeColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
            stepTwoForeColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
            stepTwoForeColorMultiBind.ConverterParameter = "foreColorTwo";
            stepTwoForeColorMultiBind.NotifyOnSourceUpdated = true;
            this.stepTwo.SetBinding(TextBlock.ForegroundProperty, stepTwoForeColorMultiBind);

            MultiBinding stepTwoBackColorMultiBind = new MultiBinding();
            stepTwoBackColorMultiBind.Converter = new AutoModeBackColorCoverter();
            stepTwoBackColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            stepTwoBackColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
            stepTwoBackColorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
            stepTwoBackColorMultiBind.ConverterParameter = "backColorTwo";
            stepTwoBackColorMultiBind.NotifyOnSourceUpdated = true;
            this.bdTwo.SetBinding(Border.BackgroundProperty, stepTwoBackColorMultiBind);

            MultiBinding stepTipVisibilityMultiBind = new MultiBinding();
            stepTipVisibilityMultiBind.Converter = new AutoModeTipVisCoverter();
            stepTipVisibilityMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            stepTipVisibilityMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
            stepTipVisibilityMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
            stepTipVisibilityMultiBind.NotifyOnSourceUpdated = true;
            this.bdStepTip.SetBinding(Border.VisibilityProperty, stepTipVisibilityMultiBind);


            WarnInfoOne = new Dictionary<string, byte>();
            WarnInfoThree = new Dictionary<string, byte>();
            WarnInfoOneKeys = new List<string>()
            {
                "103N23B6b0",
                "103N23B6b1",
                "103N23B6b2",

                "103N23B7b0",
                "103N23B7b1",
                "103N23B7b2",
                "103N23B7b3",
                "103N23B7b4",
                "103N23B7b5",

                "105N2N22B4b0",
                "105N2N22B4b1",
                "105N2N22B4b2",
                "105N2N22B4b3",
                "105N2N22B4b4",
                "105N2N22B4b5",
                "105N2N22B4b6",
                "105N2N22B4b7",
                "105N2N22B5b0",

                "105N2N22B5b1",
                "105N2N22B5b2",
                "105N2N22B5b3",
                "105N2N22B5b4",
                "105N2N22B5b5",
                "105N2N22B5b6",
                "105N2N22B5b7",

                 "105N2N22B6b0",
                "105N2N22B6b1",
                "105N2N22B6b2",
                "105N2N22B6b3",
                "105N2N22B6b4",
                "105N2N22B6b5",
                "105N2N22B6b6",
                "105N2N22B6b7",

                "105N2N22B7b0",
                "105N2N22B7b1",
                "105N2N22B7b2",
                "105N2N22B7b3",

                "107N2N22B4b0",
                "107N2N22B4b1",
                "107N2N22B4b2",
                "107N2N22B4b3",
                "107N2N22B4b4",
                "107N2N22B4b5",
                "107N2N22B4b6",
                "107N2N22B4b7",

                "107N2N22B5b0",
                "107N2N22B5b1",
                "107N2N22B5b2",
                "107N2N22B5b3",
                "135b4",
                "135b5",
                "135b6",
                "135b7",

                "107N2N22B5b4",
                "107N2N22B5b5",

                "123b0",
                "123b1",
                "123b2",
                "123b3",
                "123b4",
                "123b5",
                "123b6",
                "123b7"
            };

            #region 6.15新增
            this.tbLeftFinger.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23LeftFingerMotorSampleValue"], Mode = BindingMode.OneWay });//左手值采样值
            this.tbRightFinger.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23RightFingerMotorSampleValue"], Mode = BindingMode.OneWay });//右手值采样值
            this.tbFingerOpen.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23GripMotorSampleValue"], Mode = BindingMode.OneWay });//抓手采样值
            this.tbX.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["carRealPosition"], Mode = BindingMode.OneWay, Converter = new CarPosCoverter() });//小车实际位置
            this.tbY.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["armRealPosition"], Mode = BindingMode.OneWay, Converter = new ArmPosCoverter() });//手臂实际位置
            this.spcialDrill.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["103b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

            MultiBinding drillTypeSelectMultiBind = new MultiBinding();
            drillTypeSelectMultiBind.Converter = new DrillTypeSelectCoverter();
            drillTypeSelectMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drillPipeType"], Mode = BindingMode.OneWay });
            drillTypeSelectMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["103b7"], Mode = BindingMode.OneWay });
            drillTypeSelectMultiBind.NotifyOnSourceUpdated = true;
            this.tubeType.SetBinding(TextBlock.TextProperty, drillTypeSelectMultiBind);

            MultiBinding leftFingerStatusCoverterMultiBind = new MultiBinding();
            leftFingerStatusCoverterMultiBind.Converter = new FingerStatusCoverter();
            leftFingerStatusCoverterMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105b3"], Mode = BindingMode.OneWay });
            leftFingerStatusCoverterMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105b5"], Mode = BindingMode.OneWay });
            leftFingerStatusCoverterMultiBind.NotifyOnSourceUpdated = true;
            this.leftFinger.SetBinding(TextBlock.TextProperty, leftFingerStatusCoverterMultiBind);

            MultiBinding rightFingerStatusCoverterMultiBind = new MultiBinding();
            rightFingerStatusCoverterMultiBind.Converter = new FingerStatusCoverter();
            rightFingerStatusCoverterMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105b4"], Mode = BindingMode.OneWay });
            rightFingerStatusCoverterMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["105b6"], Mode = BindingMode.OneWay });
            rightFingerStatusCoverterMultiBind.NotifyOnSourceUpdated = true;
            this.rightFinger.SetBinding(TextBlock.TextProperty, rightFingerStatusCoverterMultiBind);

            #endregion
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
        /// 切换操作模式：手动/自动
        /// </summary>
        private void btn_OpState(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.operateMode.IsChecked)
            {
                byteToSend = SendByte(new List<byte> { 1, 5 });
            }
            else
            {
                byteToSend = SendByte(new List<byte> { 1, 4 });
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_WorkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (workMode.IsChecked)
            {
                byteToSend = SendByte(new List<byte> { 2, 1 });
            }
            else
            {
                byteToSend = SendByte(new List<byte> { 2, 2 });
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        #region 档绳控制
        /// <summary>
        /// 档绳控制-左挡绳伸出
        /// </summary>
        private void btn_LeftRopeStretch(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 8, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 档绳控制-右挡绳伸出
        /// </summary>
        private void btn_RightRopeStretch(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 8, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 档绳控制-左右挡绳伸出
        /// </summary>
        private void btn_BothRopeStrech(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 8, 3 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 档绳控制-左挡绳关闭
        /// </summary>
        private void btn_LeftRopeRetract(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 8, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 档绳控制-右挡绳关闭
        /// </summary>
        private void btn_RightRopeRetract(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 8, 5 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 档绳控制-左右挡绳关闭
        /// </summary>
        private void btn_BothRopeRetract(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 8, 6 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        #region 加热控制
        /// <summary>
        /// 抓手加热
        /// </summary>
        private void btn_GripHeatStart(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 4, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 电控柜加热
        /// </summary>
        private void btn_ElecContrlCabinetHeatStart(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 4, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 抓手电控柜加热
        /// </summary>
        private void btn_AllHeatStart(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 4, 3 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 抓手加热取消
        /// </summary>
        private void btn_GripStopHeat(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 4, 11 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 电控柜加热取消
        /// </summary>
        private void btn_ElecContrCabinetStopHeat(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 4, 12 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 抓手电控柜加热取消
        /// </summary>
        private void btn_StopAllHeat(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 4, 13 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        #region 钻杆设置
        /// <summary>
        /// 回主界面-刷新钻杆数量
        /// </summary>
        private void RefreshPipeCount()
        {
            byte[] byteToSend = SendByte(new List<byte> { 7, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 清空钻杠数目
        /// </summary>
        private void btn_DrillPipeCountCorrect_CancelAllPipe(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 7, 10 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 取消钻杠
        /// </summary>
        private void btn_CancelDrillPipe(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 7, 20 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        /// <summary>
        /// 回收模式
        /// </summary>
        private void btn_EquipRecycling(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 1, 6 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 一键回零
        /// </summary>
        private void btn_AllMotorRetZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 清除报警/电机使能
        /// </summary>
        private void btn_MotorEnable(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        #region 管柱类型选择
        /// <summary>
        /// 3.5寸钻杆
        /// </summary>
        private void btn_SelectDrillPipe312(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 35 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 4寸钻杆
        /// </summary>
        private void btn_SelectDrillPipe4(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 40 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 4.5寸钻杆
        /// </summary>
        private void btn_SelectDrillPipe412(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 45 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 5寸钻杆
        /// </summary>
        private void btn_SelectDrillPipe5(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 50 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 5.5寸钻杆
        /// </summary>
        private void btn_SelectDrillPipe55(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 55 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 5 7/8寸钻杆
        /// </summary>
        private void btn_SelectDrillPipe578(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 57 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 6 5/8寸钻杆
        /// </summary>
        private void btn_SelectDrillPipe658(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 68 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 4.5寸钻铤
        /// </summary>
        private void btn_SelectDrillPipe45(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 45 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 6寸钻铤
        /// </summary>
        private void btn_SelectDrillPipe6(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 60 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 6.5寸钻铤
        /// </summary>
        private void btn_SelectDrillPipe65(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 65 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 7寸钻铤
        /// </summary>
        private void btn_SelectDrillPipe7(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 70 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 7.5寸钻铤
        /// </summary>
        private void btn_SelectDrillPipe75(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 75 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 8寸钻铤
        /// </summary>
        private void btn_SelectDrillPipe8(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 80 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 9寸钻铤
        /// </summary>
        private void btn_SelectDrillPipe9(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 90 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 10寸钻铤
        /// </summary>
        private void btn_SelectDrillPipe10(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 100 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 11寸钻铤
        /// </summary>
        private void btn_SelectDrillPipe11(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 3, 110 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        #region 摄像头操作
        System.Threading.Timer cameraSaveThreadTimer1; //摄像头录像线程
        System.Threading.Timer cameraSaveThreadTimer2;
        private string configPath = System.Environment.CurrentDirectory + @"\Config.ini";
        const int STRINGMAX = 255;

        Image cameraInitImage1 = new Image();
        Image cameraInitImage2 = new Image();
        /// <summary>
        /// 初始化摄像头信息
        /// </summary>
        private void InitCameraInfo()
        {
            ChannelInfo info1 = GetConfigPara("CAMERA1");
            ChannelInfo info2 = GetConfigPara("CAMERA2");
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
            InitCameraSaveTimeThread();
        }
        /// <summary>
        /// 初始化摄像头录像线程
        /// </summary>
        private void InitCameraSaveTimeThread()
        {
            cameraSaveThreadTimer1 = new System.Threading.Timer(new TimerCallback(CameraVideoSave1), null, Timeout.Infinite, 60000);
            cameraSaveThreadTimer2 = new System.Threading.Timer(new TimerCallback(CameraVideoSave2), null, Timeout.Infinite, 60000);
        }

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
        /// 摄像头1播放
        /// </summary>
        private void Button_CameraStart(object sender, RoutedEventArgs e)
        {
            gridCamera1.Children.Clear();
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 1).FirstOrDefault();
            CameraVideoStop1();
            ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 1).FirstOrDefault();
            cameraOne.InitCamera(info);
            CameraVideoStart1();
            cameraOne.SetSize(300, 400);
            if (cameraOne is UIControl_HBGK1)
            {
                gridCamera1.Children.Add(cameraOne as UIControl_HBGK1);
                //(cameraOne as UIControl_HBGK1).SetValue(Grid.RowProperty, 0);
                //(cameraOne as UIControl_HBGK1).SetValue(Grid.ColumnProperty, 0);
            }
            else if (cameraOne is YiTongCameraControl)
            {
                gridCamera1.Children.Add(cameraOne as YiTongCameraControl);
                //(cameraOne as YiTongCameraControl).SetValue(Grid.RowProperty, 0);
                //(cameraOne as YiTongCameraControl).SetValue(Grid.ColumnProperty, 0);
            }
            else
            {
                gridCamera1.Children.Add(cameraInitImage1);
            }
            viewboxCameral1.Height = 300;
            viewboxCameral1.Width = 403;
        }
        private void Button_CameraStart2(object sender, RoutedEventArgs e)
        {
            if (gridCamera2.Children.Count > 0)
            {
                ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 2).FirstOrDefault();
                CameraVideoStop2();
                gridCamera2.Children.Clear();
                ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 2).FirstOrDefault();
                cameraTwo.InitCamera(info);
                CameraVideoStart2();
                cameraTwo.SetSize(288, 352);
                if (cameraTwo is UIControl_HBGK1)
                {
                    gridCamera2.Children.Add(cameraTwo as UIControl_HBGK1);
                    //(cameraTwo as UIControl_HBGK1).SetValue(Grid.RowProperty, 0);
                    //(cameraTwo as UIControl_HBGK1).SetValue(Grid.ColumnProperty, 0);
                }
                else if (cameraTwo is YiTongCameraControl)
                {
                    gridCamera2.Children.Add(cameraTwo as YiTongCameraControl);
                    //(cameraTwo as YiTongCameraControl).SetValue(Grid.RowProperty, 0);
                    //(cameraTwo as YiTongCameraControl).SetValue(Grid.ColumnProperty, 0);
                }
                else
                {
                    gridCamera2.Children.Add(cameraInitImage1);
                }
            }

        }

        private void CameraVideoStop1()
        {
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 1).FirstOrDefault();
            cameraOne.StopCamera();
            cameraSaveThreadTimer1.Change(Timeout.Infinite, 60000);
        }

        private void CameraVideoStop2()
        {
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 2).FirstOrDefault();
            cameraOne.StopCamera();
            cameraSaveThreadTimer2.Change(Timeout.Infinite, 60000);
        }

        private void CameraVideoStart1()
        {
            cameraSaveThreadTimer1.Change(0, 60000);
        }

        private void CameraVideoStart2()
        {
            cameraSaveThreadTimer2.Change(0, 60000);
        }

        private void CameraVideoSave1(object value)
        {
            string str1 = System.Environment.CurrentDirectory;
            string filePath = str1 + "\\video" + "\\video1";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 1).FirstOrDefault();
            cameraOne.StopFile();
            cameraOne.SaveFile(filePath, fileName);
            DeleteOldFileName(filePath);
        }
        private void CameraVideoSave2(object value)
        {
            string str1 = System.Environment.CurrentDirectory;
            string filePath = str1 + "\\video" + "\\video2";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
            ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 2).FirstOrDefault();
            cameraTwo.StopFile();
            cameraTwo.SaveFile(filePath, fileName);
            DeleteOldFileName(filePath);
        }

        /// <summary>
        /// 删除最老的视频文件
        /// </summary>
        /// <param name="path"></param>
        private void DeleteOldFileName(string path)
        {
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            string[] disk = path.Split('\\');
            // 硬盘空间小于1G，开始清理录像
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == disk[0] + "\\" && drive.TotalFreeSpace / (1024 * 1024) < 1024)
                {
                    DirectoryInfo root = new DirectoryInfo(path);
                    var file = root.GetFiles().OrderBy(s => s.CreationTime).FirstOrDefault();
                    file.Delete();
                }
            }
        }

        /// <summary>
        /// 切换视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_CameraExchange(object sender, RoutedEventArgs e)
        {
            foreach (ICameraFactory cam in GlobalData.Instance.cameraList)
            {
                if (cam.Info.ID == 1) cam.Info.ID = 2;
                else if (cam.Info.ID == 2) cam.Info.ID = 1;
            }

            gridCamera1.Children.Clear();
            gridCamera2.Children.Clear();

            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 1).FirstOrDefault();
            //cameraOne.SetSize(300, 400);
            if (cameraOne is UIControl_HBGK1)
            {
                gridCamera1.Children.Add(cameraOne as UIControl_HBGK1);
            }
            else if (cameraOne is YiTongCameraControl)
            {
                gridCamera1.Children.Add(cameraOne as YiTongCameraControl);
            }
            else
            {
                gridCamera1.Children.Add(cameraInitImage1);
            }
            ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 2).FirstOrDefault();
            if (cameraTwo is UIControl_HBGK1)
            {
                gridCamera2.Children.Add(cameraTwo as UIControl_HBGK1);
            }
            else if (cameraTwo is YiTongCameraControl)
            {
                gridCamera2.Children.Add(cameraTwo as YiTongCameraControl);
            }
            else
            {
                gridCamera2.Children.Add(cameraInitImage1);
            }
            //viewboxCameral1.Height = 300;
            //viewboxCameral1.Width = 403;
        }
        /// <summary>
        /// 全屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_CameraFullScreen(object sender, RoutedEventArgs e)
        {
            //if (FullScreenEvent != null)
            //{
            //    FullScreenEvent();
            //}
        }

        /// <summary>
        /// 播放视频定时器
        /// </summary>
        private void CameraTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            cameraTimer.Stop();
            PlayCamera();
        }
        /// <summary>
        /// 跨线程调用播放视频
        /// </summary>
        public void PlayCamera()
        {
            this.gridCamera1.Dispatcher.Invoke(new PayOneDelegate(PlayOneAction), null);
            this.gridCamera2.Dispatcher.Invoke(new PayTwoDelegate(PlayTwoAction), null);
        }

        private delegate void PayOneDelegate();

        private void PlayOneAction()
        {
            gridCamera1.Children.Clear();
            SFCameraFullScreen.Instance.gridCamera1.Children.Clear();
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 1).FirstOrDefault();
            CameraVideoStop1();
            ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 1).FirstOrDefault();
            cameraOne.InitCamera(info);
            CameraVideoStart1();
            cameraOne.SetSize(220, 380);
            if (cameraOne is UIControl_HBGK1)
            {
                gridCamera1.Children.Add(cameraOne as UIControl_HBGK1);
                //(cameraOne as UIControl_HBGK1).SetValue(Grid.RowProperty, 0);
                //(cameraOne as UIControl_HBGK1).SetValue(Grid.ColumnProperty, 0);
            }
            else if (cameraOne is YiTongCameraControl)
            {
                gridCamera1.Children.Add(cameraOne as YiTongCameraControl);
                //(cameraOne as YiTongCameraControl).SetValue(Grid.RowProperty, 0);
                //(cameraOne as YiTongCameraControl).SetValue(Grid.ColumnProperty, 0);
            }
            else
            {
                gridCamera1.Children.Add(cameraInitImage1);
            }
            cameraOne.FullScreenEvent -= CameraOne_FullScreenEvent;
            cameraOne.FullScreenEvent += CameraOne_FullScreenEvent;
                //viewboxCameral1.Height = 300;
                //viewboxCameral1.Width = 403;
        }

        private void CameraOne_FullScreenEvent()
        {
            if (FullScreenEvent != null)
            {
                FullScreenEvent(1);
            }
        }


        private delegate void PayTwoDelegate();

        private void PlayTwoAction()
        {
            ICameraFactory cameraTwo = GlobalData.Instance.cameraList.Where(w => w.Info.ID == 2).FirstOrDefault();
            CameraVideoStop2();
            gridCamera2.Children.Clear();
            ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == 2).FirstOrDefault();
            cameraTwo.SetSize(220, 380);
            if (cameraTwo is UIControl_HBGK1)
            {
                gridCamera2.Children.Add(cameraTwo as UIControl_HBGK1);
                //(cameraTwo as UIControl_HBGK1).SetValue(Grid.RowProperty, 0);
                //(cameraTwo as UIControl_HBGK1).SetValue(Grid.ColumnProperty, 0);
            }
            else if (cameraTwo is YiTongCameraControl)
            {
                gridCamera2.Children.Add(cameraTwo as YiTongCameraControl);
                //(cameraTwo as YiTongCameraControl).SetValue(Grid.RowProperty, 0);
                //(cameraTwo as YiTongCameraControl).SetValue(Grid.ColumnProperty, 0);
            }
            else
            {
                gridCamera2.Children.Add(cameraInitImage1);
            }
            cameraTwo.InitCamera(info);
            cameraTwo.FullScreenEvent -= CameraTwo_FullScreenEvent;
            cameraTwo.FullScreenEvent += CameraTwo_FullScreenEvent;
            CameraVideoStart2();
        }

        private void CameraTwo_FullScreenEvent()
        {
            if (FullScreenEvent != null)
            {
                FullScreenEvent(2);
            }
        }
        #endregion
        /// <summary>
        /// 选择特殊指梁
        /// </summary>
        private void btn_SPSelect(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认选择特殊指梁", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                byte[] byteToSend = SendByte(new List<byte> { 20, 1, 0, 0, 2 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 特殊指梁取消
        /// </summary>
        private void btn_SPCancel(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 20, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 自动选择指梁左
        /// </summary>
        private void btn_DrillSelectAutoLeft(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 24, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 自动选择指梁右
        /// </summary>
        private void btn_DrillSelectAutoRight(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 24, 3 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 自动选择指梁:手动
        /// </summary>
        private void btn_DrillSelectManual(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 24, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 小车回零
        /// </summary>
        private void btn_CarToZero(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 13, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 手臂回零
        /// </summary>
        private void btn_ArmToZero(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 13, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 回转回零
        /// </summary>
        private void btn_RotateToZero(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 13, 3 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btn_AllToZero(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
