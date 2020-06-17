using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Common
{
    public class ReportData
    {
        public string DeviceNumber;
        public int ReportGenerateTime;
        public int PowerOnTime;
        /// <summary>
        /// 设备工作时间
        /// </summary>
        public int WorkTime;
        public int DrillDownCount;
        public int DrillUpCount;
        public int RobotBigHookInterLock;
        public int RobotTopDriveInterlock;
        public int RobotElevatorInterlock;
        public int ElevatorBigHookInterlock;
        public int RobotRetainingRopeInterlock;
        public int RobotFingerBeamLockInterlock;
        public int SecondFloorCommunication;
        public int OperationFloorCommunication;
        public int CarMotorAlarm;
        public int ArmMotorAlarm;
        public int RotateMotorAlarm;
        public int GripMotorAlarm;
        public int FingerMotorAlarm;
        public int DrillCollarMotorAlarm;
        public int RetainingRopeMotorAlarm;
    }
}
