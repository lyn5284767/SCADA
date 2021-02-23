using DemoDriver;
using HBGKTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace COM.Common
{
    public class GlobalData
    {
        private static GlobalData _instance = null;
        private static readonly object syncRoot = new object();

        private GlobalData()
        {
        }

        public static GlobalData Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new GlobalData();
                        }
                    }
                }
                return _instance;
            }
        }
        public DAService da { get; set; }

        public enum TimerInter : int
        {
            /// <summary>
            /// 1分钟
            /// </summary>
            OneMinute = 60000
        }


        public byte[] ConfigParameter = new byte[4];
        public byte[] SetParam = new byte[10];
        /// <summary>
        /// 二层台移动X轴补偿
        /// </summary>
        public int CompensateX { get; set; }
        /// <summary>
        /// 二层台移动Y轴补偿
        /// </summary>
        public int CompensateY { get; set; }
        public int CarSize { get; set; } = 20;
        /// <summary>
        /// 小车最大位移
        /// </summary>
        public int CarMaxPosistion { get; set; }
        /// <summary>
        /// 小车最小位移
        /// </summary>
        public int CarMinPosistion { get; set; }
        /// <summary>
        /// 手臂最大位移
        /// </summary>
        public int ArmMaxPosistion { get; set; }
        /// <summary>
        /// 钻台面-小车最大位移
        /// </summary>
        public int DRCarMaxPosistion { get; set; }
        /// <summary>
        /// 钻台面-小车最小位移
        /// </summary>
        public int DRCarMinPosistion { get; set; }
        /// <summary>
        /// 钻台面-手臂最大位移
        /// </summary>
        public int DRArmMaxPosistion { get; set; }
        /// <summary>
        /// 第一行行高比其他行搞，计算小车高度
        /// </summary>
        public int FirstRowHeightCompensate { get; set; }
        /// <summary>
        /// 钻杆行数
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// 钻铤数目
        /// </summary>
        public int DrillNum { get; set; }

        public void GetKeyBoard()
        {
            string configPath = System.Environment.CurrentDirectory + "\\KeyBoard.exe";
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            bool exist = false;
            foreach (System.Diagnostics.Process process in processList)
            {
                if (process.ProcessName.Contains("KeyBoard"))
                {
                    exist = true;
                }
            }
            if (!exist)
            {
                System.Diagnostics.Process.Start(configPath, "1");
            }
            else
            {
                IntPtr TouchhWnd = new IntPtr(0);
                TouchhWnd = FindWindow(null, "MainWindow");
                if (TouchhWnd == IntPtr.Zero)
                    return;
                PostMessage(TouchhWnd, WM_SYSCOMMAND, SC_RESTORE, 0);
            }

        }

        public const Int32 WM_SYSCOMMAND = 274;
        private const int SC_RESTORE = 0xF120;     //还原 

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public ReportData reportData = new ReportData();

        public List<ICameraFactory> cameraList = new List<ICameraFactory>(); //摄像头

        public List<ChannelInfo> chList = new List<ChannelInfo>();//摄像头信息列表

        public SystemType systemType;

        public bool SIRHigh;

        public bool Ing { get; set; }
        /// <summary>
        /// 全局告警
        /// </summary>
        public bool HS_OilHigh { get; set; } = false;

        /// <summary>
        /// 组协议
        /// </summary>
        /// <param name="list">协议字符列表</param>
        /// <returns></returns>
        public byte[] SendByte(List<byte> list)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = 0x50; // 默认0位80
            byteToSend[1] = 0x01;   // 默认1位1
            // 2位之后传进来
            for (int i = 0; i < list.Count; i++)
            {
                byteToSend[i + 2] = list[i];
            }
            return byteToSend;
        }

        /// <summary>
        /// 上位机发钻台面
        /// </summary>
        /// <param name="list">协议字符列表</param>
        /// <returns></returns>
        public byte[] SendToDR(List<byte> list)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = 0x50;
            byteToSend[1] = 0x21;
            // 2位之后传进来
            for (int i = 0; i < list.Count; i++)
            {
                byteToSend[i + 2] = list[i];
            }
            return byteToSend;
        }

        /// <summary>
        /// 上位机发操作台
        /// </summary>
        /// <param name="list">协议字符列表</param>
        /// <returns></returns>
        public byte[] SendToOpr(List<byte> list)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = 0x01;
            byteToSend[1] = 0x20;
            // 2位之后传进来
            for (int i = 0; i < list.Count; i++)
            {
                byteToSend[i + 2] = list[i];
            }
            return byteToSend;
        }

        /// <summary>
        /// 上位机发操作台
        /// </summary>
        /// <param name="list">协议字符列表</param>
        /// <returns></returns>
        public byte[] SFSendToOpr(List<byte> list)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = 0x10;
            byteToSend[1] = 0x01;
            // 2位之后传进来
            for (int i = 0; i < list.Count; i++)
            {
                byteToSend[i + 2] = list[i];
            }
            return byteToSend;
        }

        /// <summary>
        /// 组协议
        /// </summary>
        /// <param name="list">协议字符列表</param>
        /// <returns></returns>
        public byte[] SendListToByte(List<byte> list)
        {
            byte[] byteToSend = new byte[10];
            for (int i = 0; i < list.Count; i++)
            {
                byteToSend[i] = list[i];
            }
            return byteToSend;
        }

        public bool ComunciationNormal = true;

        public int DrillLeftTotal = 0;
        /// <summary>
        /// 当前页面
        /// </summary>
        public string DRNowPage = string.Empty;
        /// <summary>
        /// 系统角色
        /// </summary>
        public SystemRole systemRole;
        /// <summary>
        /// 集成界面设备链表
        /// </summary>
        public LinkList<IngDeviceStatus> DeviceLink = new LinkList<IngDeviceStatus>();
    }

    // 系统类型
    public enum SystemType
    {
        /// <summary>
        /// 二层台
        /// </summary>
        SecondFloor = 0,
        /// <summary>
        /// 钻台面
        /// </summary>
        DrillFloor = 1,
        /// <summary>
        /// 铁道工
        /// </summary>
        SIR = 2,
        /// <summary>
        /// 猫道
        /// </summary>
        CatRoad = 3,
        /// <summary>
        /// 集成控制系统
        /// </summary>
        CIMS = 4,
        /// <summary>
        /// 液压站
        /// </summary>
        HydraulicStation = 5,
        /// <summary>
        /// 防喷/丝扣油
        /// </summary>
        ScrewThread = 6,
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknow = 99
    }

    public enum SystemRole
    {
        /// <summary>
        /// 调试员
        /// </summary>
        DebugMan = 0,
        /// <summary>
        /// 技术员
        /// </summary>
        TechMan = 1,
        /// <summary>
        /// 管理员
        /// </summary>
        AdminMan = 2,
        /// <summary>
        /// 操作员
        /// </summary>
        OperMan = 3,
    }
    /// <summary>
    /// 铁钻工类型
    /// </summary>
    public enum SIRType
    {
        /// <summary>
        /// 三一
        /// </summary>
        SANY=1,
        /// <summary>
        /// JJC
        /// </summary>
        JJC=2,
        /// <summary>
        /// 宝石
        /// </summary>
        BS=3,
        /// <summary>
        /// 江汉
        /// </summary>
        JH = 4,
        /// <summary>
        /// 三一轨道式
        /// </summary>
        SANYRailway=5
    }

    public enum DRType
    {
        /// <summary>
        /// 三一
        /// </summary>
        SANY=1,
        /// <summary>
        /// 杰瑞
        /// </summary>
        JR=2
    }

    public enum HSType
    {
        /// <summary>
        /// 三一
        /// </summary>
        SANY=1,
        /// <summary>
        /// 宝石
        /// </summary>
        BS=2,
        /// <summary>
        /// JJC
        /// </summary>
        JJC=3
    }

    public enum CatType
    {
        /// <summary>
        /// 三一
        /// </summary>
        SANY = 1,
        /// <summary>
        /// 宝石
        /// </summary>
        BS = 2,
        /// <summary>
        /// 宏达
        /// </summary>
        HD = 3,
        /// <summary>
        /// 胜利
        /// </summary>
        SL = 4
    }

    public enum Technique
    {
        /// <summary>
        /// 下钻
        /// </summary>
        DrillDown = 1,
        /// <summary>
        /// 上钻
        /// </summary>
        DrillUp=2
    }
}
