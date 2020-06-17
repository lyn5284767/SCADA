using System;
using System.Runtime.InteropServices;

namespace DataService
{
    /// <summary>
    /// 结构体，对Tag进行包装，是否归档，更新周期，最大/小 值，更新频率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TagMetaData : IComparable<TagMetaData>
    {
        public bool Archive;//归档

        public DataType DataType;//数据类型

        public ushort Size;//字节的长度

        public short ID;

        public short GroupID;

        public float Maximum;

        public float Minimum;

        public int Cycle;

        /// <summary>
        /// 这个字符串地址，需要和DeviceAddress进行转换
        /// </summary>
        public string Address;

        public string Name;

        public string Description;

        public TagMetaData(short id, short grpId, string name, string address,string description,
            DataType type, ushort size, bool archive = false, float max = 0, float min = 0, int cycle = 0)
        {
            ID = id;
            GroupID = grpId;
            Name = name;
            Address = address;
            DataType = type;
            Size = size;
            Archive = archive;
            Maximum = max;
            Minimum = min;
            Cycle = cycle;
            Description = description;
        }

        public int CompareTo(TagMetaData other)
        {
            return this.ID.CompareTo(other.ID);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// ？？结构体  估计用来对变量Itag 进行处理
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Scaling : IComparable<Scaling>
    {
        public short ID;

        public ScaleType ScaleType;

        public float EUHi;

        public float EULo;

        public float RawHi;

        public float RawLo;

        public Scaling(short id, ScaleType type, float euHi, float euLo, float rawHi, float rawLo)
        {
            ID = id;
            ScaleType = type;
            EUHi = euHi;
            EULo = euLo;
            RawHi = rawHi;
            RawLo = rawLo;
        }

        public int CompareTo(Scaling other)
        {
            return ID.CompareTo(other.ID);
        }

        public static readonly Scaling Empty = new Scaling { ScaleType = ScaleType.None };//位置参数 初始化
    }

    public struct ItemData<T>
    {
        public T Value;
        public long TimeStamp;
        public QUALITIES Quality;

        public ItemData(T value, long timeStamp, QUALITIES quality)
        {
            Value = value;
            TimeStamp = timeStamp;
            Quality = quality;
        }
    }

    /// <summary>
    /// ？？枚举类型
    /// </summary>
    public enum ScaleType : byte
    {
        None = 0,
        Linear = 1,
        SquareRoot = 2
    }
}
