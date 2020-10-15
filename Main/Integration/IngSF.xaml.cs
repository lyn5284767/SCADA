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
    /// IngSF.xaml 的交互逻辑
    /// </summary>
    public partial class IngSF : UserControl
    {
        private static IngSF _instance = null;
        private static readonly object syncRoot = new object();

        public static IngSF Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngSF();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        Dictionary<string, byte> AlarmKeys = new Dictionary<string, byte>();
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private bool bCommunicationCheck = false; // 是否有中断标志
        private int iTimeCnt = 0;//用来为时钟计数的变量
        List<string> AlarmList;
        #region 报表字段 各类标志
        private bool bCheckTwo = false;
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
        public IngSF()
        {
            InitializeComponent();
            Init();
            VariableBinding();
            timer = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);
            aminationNew.SendFingerBeamNumberEvent += Instance_SendFingerBeamNumberEvent;
            aminationNew.SystemChange(SystemType.SecondFloor);
            this.Loaded += IngSF_Loaded;
        }

        private void IngSF_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 1, 32, 1, 11, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            //systemType = SystemType.SecondFloor;
            aminationNew.InitRowsColoms(SystemType.SecondFloor);
        }

        private void VariableBinding()
        {
            this.carMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.armMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.rotateMotorRetZero.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["rotateMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.carMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
            this.armMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
            this.rotateMotorWorkStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["rotateMotorWorkStatus"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
        }
        /// <summary>
        /// 设置指梁
        /// </summary>
        /// <param name="number">指梁号</param>
        private void Instance_SendFingerBeamNumberEvent(byte number)
        {
            if (GlobalData.Instance.da["operationModel"].Value.Byte == 5 || GlobalData.Instance.da["operationModel"].Value.Byte == 3)
            {
                byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 5, number });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            Thread.Sleep(50);
            if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5 || GlobalData.Instance.da["droperationModel"].Value.Byte == 3)
            {
                byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 5, number });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        private void Init()
        {
            AlarmList = new List<string>()
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
        /// 50ms定时器
        /// </summary>
        /// <param name="obj"></param>
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    aminationNew.LoadFingerBeamDrillPipe(SystemType.SecondFloor);
                    this.ReportDataUpdate();
                    GetAlarmAndOpr();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 报警和操作提示
        /// </summary>
        private void GetAlarmAndOpr()
        {
            #region 操作提示
            switch (GlobalData.Instance.da["promptInfo"].Value.Byte)
            {
                case 0:
                    this.tbOprTips.Text = "";
                    break;
                case 1:
                    this.tbOprTips.Text = "小车电机报警";
                    break;
                case 2:
                    this.tbOprTips.Text = "手臂电机报警";
                    break;
                case 3:
                    this.tbOprTips.Text = "回转电机报警";
                    break;
                case 6:
                    this.tbOprTips.Text = "非安全条件，机械手禁止向井口移动！";
                    break;
                case 7:
                    this.tbOprTips.Text = "吊卡未打开，机械手禁止缩回！";
                    break;
                case 8:
                    this.tbOprTips.Text = "请将挡绳缩回后再回零！";
                    break;
                case 9:
                    this.tbOprTips.Text = "抓手中有钻杆，手指已经打开，请注意安全！";
                    break;
                case 10:
                    this.tbOprTips.Text = "此位置禁止打开手指！";
                    break;
                case 11:
                    this.tbOprTips.Text = "此位置禁止打开抓手！";
                    break;
                case 12:
                    this.tbOprTips.Text = "此位置抓手不允许关闭！";
                    break;
                case 13:
                    this.tbOprTips.Text = "小车电机已回零完成！";
                    break;
                case 14:
                    this.tbOprTips.Text = "手臂电机已回零完成!";
                    break;
                case 15:
                    this.tbOprTips.Text = "回转电机（机械手）已完成回零！";
                    break;
                case 31:
                    this.tbOprTips.Text = "抓手中有钻杆，请将钻杆取出后再回零！";
                    break;
                case 32:
                    this.tbOprTips.Text = "请先将手臂电机回零！";
                    break;
                case 33:
                    this.tbOprTips.Text = "请注意安全，小车电机正在回零……";
                    break;
                case 34:
                    this.tbOprTips.Text = "请先将手臂缩回！";
                    break;
                case 35:
                    this.tbOprTips.Text = "请注意安全，手臂电机正在回零……";
                    break;
                case 36:
                    this.tbOprTips.Text = "请先将小车电机正在回零！";
                    break;
                case 37:
                    this.tbOprTips.Text = "请注意安全，回转电机正在回零……";
                    break;
                case 38:
                    this.tbOprTips.Text = "请将小车移动到中间靠井口位置！";
                    break;
                case 39:
                    this.tbOprTips.Text = "所选择的电机已经回零完成！";
                    break;
                case 40:
                    this.tbOprTips.Text = "机械手还未回零，请注意安全！";
                    break;
                case 41:
                    this.tbOprTips.Text = "请先选择管柱类型！";
                    break;
                case 42:
                    this.tbOprTips.Text = "请选择目标指梁号！";
                    break;
                case 43:
                    this.tbOprTips.Text = "未感应到回转回零传感器,请检查!";
                    break;
                case 44:
                    this.tbOprTips.Text = "所选指梁钻杆已满，请切换！";
                    break;
                case 45:
                    this.tbOprTips.Text = "请按确认键启动！";
                    break;
                case 46:
                    this.tbOprTips.Text = "请确认指梁锁已打开到位！";
                    break;
                case 47:
                    this.tbOprTips.Text = "手指正在打开……";
                    break;
                case 48:
                    this.tbOprTips.Text = "抓手正在打开……";
                    break;
                case 49:
                    this.tbOprTips.Text = "请确认吊卡是否在二层台上方！";
                    break;
                case 50:
                    this.tbOprTips.Text = "请确认吊卡已经打开！";
                    break;
                case 51:
                    this.tbOprTips.Text = "吊卡未打开！";
                    break;
                case 52:
                    this.tbOprTips.Text = "请操作手柄！";
                    break;
                case 53:
                    this.tbOprTips.Text = "手指和抓手未打开！";
                    break;
                case 54:
                    this.tbOprTips.Text = "自动排管完成！";
                    break;
                case 55:
                    this.tbOprTips.Text = "所选指梁钻杆已空，请切换！";
                    break;
                case 56:
                    this.tbOprTips.Text = "指梁内抓杆失败！";
                    break;
                case 57:
                    this.tbOprTips.Text = "抓手正在关闭……";
                    break;
                case 58:
                    this.tbOprTips.Text = "手指正在关闭……";
                    break;
                case 59:
                    this.tbOprTips.Text = "人工确认吊卡关闭！";
                    break;
                case 60:
                    this.tbOprTips.Text = "自动送杆完成！";
                    break;
                case 61:
                    this.tbOprTips.Text = "请按确认键启动回收模式！";
                    break;
                case 62:
                    this.tbOprTips.Text = "机械手已回收完成！";
                    break;
                case 65:
                    this.tbOprTips.Text = "请先收回手臂再操作！";
                    break;
                case 66:
                    this.tbOprTips.Text = "此位置不能进行手臂伸出和回转操作！";
                    break;
                case 67:
                    this.tbOprTips.Text = "此位置不能旋转！";
                    break;
                case 68:
                    this.tbOprTips.Text = "此位置不能伸出手臂！";
                    break;
                case 71:
                    this.tbOprTips.Text = "请按确认键启动运输模式！";
                    break;
                case 72:
                    this.tbOprTips.Text = "机械手已完成运输模式！";
                    break;
                case 73:
                    this.tbOprTips.Text = "请谨慎确认钻杆已送入吊卡！";
                    break;
                case 74:
                    this.tbOprTips.Text = "请谨慎确认手臂已伸到位！";
                    break;
                case 75:
                    this.tbOprTips.Text = "请确认手指开合状态！";
                    break;
                case 81:
                    this.tbOprTips.Text = "记录的两点太近！";
                    break;
                case 82:
                    this.tbOprTips.Text = "示教过程存在双轴运动！";
                    break;
                case 83:
                    this.tbOprTips.Text = "记录点数超出限制！";
                    break;
                case 84:
                    this.tbOprTips.Text = "记录点数为零！";
                    break;
                case 85:
                    this.tbOprTips.Text = "即将进入下一次示教循环！";
                    break;
                case 96:
                    this.tbOprTips.Text = "小车电机动作卡滞！";
                    break;
                case 97:
                    this.tbOprTips.Text = "手臂电机动作卡滞！";
                    break;
                case 98:
                    this.tbOprTips.Text = "回转电机动作卡滞！";
                    break;
                default:
                    this.tbOprTips.Text = "";
                    break;
            }
            #endregion

            #region 告警提示
            byte byteKey;
            foreach (string item in AlarmList)
            {
                if (item.Contains("103") && GlobalData.Instance.da["Con_Set0"].Value.Byte != 23)
                {
                    if (GlobalData.Instance.da[item].Value.Boolean)
                    {
                        this.AlarmKeys.TryGetValue(GlobalData.Instance.da[item].Description, out byteKey);
                        if (byteKey == 0)//值为零 ，表示字典中还没有该提示信息，为 1 代表还没有显示，为 2 代表已经显示
                        {
                            this.AlarmKeys.Add(GlobalData.Instance.da[item].Description, 1);
                        }
                    }
                    else
                    {
                        this.AlarmKeys.TryGetValue(GlobalData.Instance.da[item].Description, out byteKey);
                        if (byteKey != 0)
                        {
                            this.AlarmKeys.Remove(GlobalData.Instance.da[item].Description);
                        }
                    }
                }
                else
                {
                    if (GlobalData.Instance.da["Con_Set0"].Value.Byte != 23 && GlobalData.Instance.da["Con_Set0"].Value.Byte != 22 && (GlobalData.Instance.da["operationModel"].Value.Byte != 2))
                    {
                        if (GlobalData.Instance.da[item].Value.Boolean)
                        {
                            this.AlarmKeys.TryGetValue(GlobalData.Instance.da[item].Description, out byteKey);
                            if (byteKey == 0)
                            {
                                this.AlarmKeys.Add(GlobalData.Instance.da[item].Description, 1);
                            }
                        }
                        else
                        {
                            this.AlarmKeys.TryGetValue(GlobalData.Instance.da[item].Description, out byteKey);
                            if (byteKey != 0)
                            {
                                this.AlarmKeys.Remove(GlobalData.Instance.da[item].Description);
                            }
                        }
                    }
                }
            }

            if (!GlobalData.Instance.da["carMotorRetZeroStatus"].Value.Boolean || !GlobalData.Instance.da["armMotorRetZeroStatus"].Value.Boolean || !GlobalData.Instance.da["rotateMotorRetZeroStatus"].Value.Boolean)//主界面左下去提示
            {
                this.AlarmKeys.TryGetValue("机械手未回零，位置未知，请注意防碰！", out byteKey);
                if (byteKey == 0)
                {
                    this.AlarmKeys.Add("机械手未回零，位置未知，请注意防碰！", 1);
                }
            }
            else
            {
                this.AlarmKeys.TryGetValue("机械手未回零，位置未知，请注意防碰！", out byteKey);
                if (byteKey != 0)
                {
                    this.AlarmKeys.Remove("机械手未回零，位置未知，请注意防碰！");
                }
            }

            if (!GlobalData.Instance.da["101B7b1"].Value.Boolean)
            {
                this.AlarmKeys.TryGetValue("机械手已进入防碰区，请注意防碰！", out byteKey);
                if (byteKey == 0)
                {
                    this.AlarmKeys.Add("机械手已进入防碰区，请注意防碰！", 1);
                }
            }
            else
            {
                this.AlarmKeys.TryGetValue("机械手已进入防碰区，请注意防碰！", out byteKey);
                if (byteKey != 0)
                {
                    this.AlarmKeys.Remove("机械手已进入防碰区，请注意防碰！");
                }
            }

            if (iTimeCnt % 10 == 0)
            {
                if (this.AlarmKeys.Count > 0)
                {
                    this.tbAlarmTips.FontSize = 14;
                    this.tbAlarmTips.Visibility = Visibility.Visible;

                    if (!this.AlarmKeys.ContainsValue(1))
                    {
                        this.AlarmKeys.Keys.ToList().ForEach(k => this.AlarmKeys[k] = 1);
                    }

                    foreach (var key in this.AlarmKeys.Keys.ToList())
                    {
                        if (this.AlarmKeys[key] == 1)
                        {
                            this.tbAlarmTips.Text = key;
                            this.AlarmKeys[key] = 2;
                            break;
                        }
                    }
                }
                else
                {
                    this.tbAlarmTips.Visibility = Visibility.Hidden;
                    this.tbAlarmTips.Text = "";
                }
            }
            else
            {
                this.tbAlarmTips.FontSize = 18;
            }
            #endregion
            #region 通信
            //操作台控制器心跳
            if (GlobalData.Instance.da["508b5"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 600)
                {
                    //Communication = 2;
                    this.tbAlarmTips.Text = "操作台信号中断";
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

            //二层台控制器心跳
            if (GlobalData.Instance.da["508b6"].Value.Boolean)
            {
                //Communication = 3;
                this.tbAlarmTips.Text = "二层台信号中断";
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

            if (!GlobalData.Instance.ComunciationNormal) this.tbAlarmTips.Text = "网络连接失败！";
            #endregion

        }
    }
}
