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

        public string StandardValue { get; set; }
    }
    /// <summary>
    /// 自研液压站存储类型
    /// </summary>
    public enum SaveType
    {
        HS_Self_SystemPress = 1,
        HS_Self_OilTmp = 2,
        HS_Self_OilLevel = 3,
        HS_Self_MainFlow = 4,
        HS_Self_MainTwoFlow = 5,
        SIR_Self_DrillTorque = 6,
        SIR_Self_CosingTorque=7
    }
    /// <summary>
    /// 标准集成界面设备加载状态
    /// </summary>
    public class IngDeviceStatus
    { 
        /// <summary>
        /// 序号
        /// </summary>
        public int IndexID { get; set; }
        /// <summary>
        /// 当前设备类型
        /// </summary>
        public SystemType NowType { get; set; }
        /// <summary>
        /// 是否已经加载
        /// </summary>
        public bool IsLoad { get; set; }
        /// <summary>
        /// 当前设备名称
        /// </summary>
        public string DeviceName { get; set; }
    }

    #region 集成设备链表
    public class Node<T>
    {
        /// <summary>
        /// 当前内容
        /// </summary>
        public T Data { set; get; }    
        /// <summary>
        /// 下一链表
        /// </summary>
        public Node<T> Next { set; get; }    

        public Node(T item)
        {
            this.Data = item;
            this.Next = null;
        }

        public Node()
        {
            this.Data = default(T);
            this.Next = null;
        }
    }

    public class LinkList<T>
    {
        public Node<T> Head { set; get; } //单链表头

        //构造
        public LinkList()
        {
            Clear();
        }

        /// <summary>
        /// 求单链表的长度
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            Node<T> p = Head;
            int length = 0;
            while (p != null)
            {
                p = p.Next;
                length++;
            }
            return length;
        }

        /// <summary>
        /// 判断单键表是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            if (Head == null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 清空单链表
        /// </summary>
        public void Clear()
        {
            Head = null;
        }

        /// <summary>
        /// 获得当前位置单链表中结点的值
        /// </summary>
        /// <param name="i">结点位置</param>
        /// <returns></returns>
        public T GetNodeValue(int i)
        {
            if (IsEmpty() || i < 1 || i > GetLength())
            {
                Console.WriteLine("单链表为空或结点位置有误！");
                return default(T);
            }

            Node<T> A = new Node<T>();
            A = Head;
            int j = 1;
            while (A.Next != null && j < i)
            {
                A = A.Next;
                j++;
            }

            return A.Data;
        }

        /// <summary>
        /// 增加新元素到单链表末尾
        /// </summary>
        public void Append(T item)
        {
            Node<T> foot = new Node<T>(item);
            Node<T> A = new Node<T>();
            if (Head == null)
            {
                Head = foot;
                return;
            }
            A = Head;
            while (A.Next != null)
            {
                A = A.Next;
            }
            A.Next = foot;
        }

        /// <summary>
        /// 增加单链表插入的位置
        /// </summary>
        /// <param name="item">结点内容</param>
        /// <param name="n">结点插入的位置</param>
        public void Insert(T item, int n)
        {
            if (IsEmpty() || n < 1 || n > GetLength())
            {
                return;
            }

            if (n == 1)  //增加到头部
            {
                Node<T> H = new Node<T>(item);
                H.Next = Head;
                Head = H;
                return;
            }

            Node<T> A = new Node<T>();
            Node<T> B = new Node<T>();
            B = Head;
            int j = 1;
            while (B.Next != null && j < n)
            {
                A = B;
                B = B.Next;
                j++;
            }

            if (j == n)
            {
                Node<T> C = new Node<T>(item);
                A.Next = C;
                C.Next = B;
            }
        }

        /// <summary>
        /// 删除单链表结点
        /// </summary>
        /// <param name="i">删除结点位置</param>
        /// <returns></returns>
        public void Delete(int i)
        {
            if (IsEmpty() || i < 1 || i > GetLength())
            {
                return;
            }

            Node<T> A = new Node<T>();
            if (i == 1)   //删除头
            {
                A = Head;
                Head = Head.Next;
                return;
            }
            Node<T> B = new Node<T>();
            B = Head;
            int j = 1;
            while (B.Next != null && j < i)
            {
                A = B;
                B = B.Next;
                j++;
            }
            if (j == i)
            {
                A.Next = B.Next;
            }
        }

        /// <summary>
        /// 显示单链表
        /// </summary>
        public void Dispaly()
        {
            Node<T> A = new Node<T>();
            A = Head;
            while (A != null)
            {
                Console.WriteLine(A.Data);
                A = A.Next;
            }
        }
    }
    #endregion

    public class AlarmInfo
    {
        /// <summary>
        /// 告警对应的数据库TagName
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 告警描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 当前告警类型 0-无告警；1-告警且未显示；2-告警且已经显示
        /// </summary>
        public int NowType { get; set; }
        /// <summary>
        /// 是否需要重新扫描 true-需要;false-不需要
        /// </summary>
        public bool NeedCheck { get; set; }
    }
}
