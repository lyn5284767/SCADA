using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Common
{
    public class ReportData
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string DeviceNumber;
        /// <summary>
        /// 报告生成时间
        /// </summary>
        public int ReportGenerateTime;
        /// <summary>
        /// 开机时间
        /// </summary>
        public int PowerOnTime;
        /// <summary>
        /// 设备工作时间
        /// </summary>
        public int WorkTime;
        /// <summary>
        /// 自动下钻次数
        /// </summary>
        public int DrillDownCount;
        /// <summary>
        /// 自动起钻次数
        /// </summary>
        public int DrillUpCount;
        /// <summary>
        /// 机械手大钩互锁
        /// </summary>
        public int RobotBigHookInterLock;
        /// <summary>
        /// 机械手顶驱互锁
        /// </summary>
        public int RobotTopDriveInterlock;
        /// <summary>
        /// 机械手吊卡互锁
        /// </summary>
        public int RobotElevatorInterlock;
        /// <summary>
        /// 吊卡与大钩
        /// </summary>
        public int ElevatorBigHookInterlock;
        /// <summary>
        /// 机械手与挡绳
        /// </summary>
        public int RobotRetainingRopeInterlock;
        /// <summary>
        /// 机械手与指梁锁
        /// </summary>
        public int RobotFingerBeamLockInterlock;
        /// <summary>
        /// 二层台通信中断
        /// </summary>
        public int SecondFloorCommunication;
        /// <summary>
        /// 操作台通信中断
        /// </summary>
        public int OperationFloorCommunication;
        /// <summary>
        /// 小车电机报警
        /// </summary>
        public int CarMotorAlarm;
        /// <summary>
        /// 手臂电机报警
        /// </summary>
        public int ArmMotorAlarm;
        /// <summary>
        /// 回转电机报警
        /// </summary>
        public int RotateMotorAlarm;
        /// <summary>
        /// 抓手电机报警
        /// </summary>
        public int GripMotorAlarm;
        /// <summary>
        /// 手指电机报警
        /// </summary>
        public int FingerMotorAlarm;
        /// <summary>
        /// 钻铤锁电机报警
        /// </summary>
        public int DrillCollarMotorAlarm;
        /// <summary>
        /// 挡绳电机报警
        /// </summary>
        public int RetainingRopeMotorAlarm;
    }
    /// <summary>
    /// UDP发送模型
    /// </summary>
    public class UdpModel
    {
        /// <summary>
        /// 协议类型
        /// </summary>
        public UdpType UdpType { get; set; }
        /// <summary>
        /// 协议内容
        /// </summary>
        public string Content { get; set; }
    }

    public enum UdpType
    {
        PlayCamera = 0
    }

    public class DateBaseReport
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 记录值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 存储周期
        /// </summary>
        public int Cycle { get; set; }
    }

    public enum SaveType
    {
        HS_Self_SystemPress = 1,
        HS_Self_OilTmp = 2,
        HS_Self_OilLevel = 3
    }
}
