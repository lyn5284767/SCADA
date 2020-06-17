using System;
using System.Runtime.InteropServices;

namespace DataService
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceAddress : IComparable<DeviceAddress>
    {
        public int Area;//区域号
        public int Start;//起始位置
        public ushort DBNumber;//区块号
        public ushort DataSize;//数据长度
        public ushort CacheIndex;//在缓存字节中的索引，这是唯一需要计算才能得到的值，其他字段是可以直接赋值的
        public byte Bit;//位号，取得是第几个位
        public DataType VarType;//数据类型 整形/布尔/浮点数
        public ByteOrder ByteOrder;//大端和小端

        public DeviceAddress(int area, ushort dbnumber, ushort cIndex, int start, ushort size, byte bit, DataType type, ByteOrder order = ByteOrder.None)
        {
            Area = area;
            DBNumber = dbnumber;
            CacheIndex = cIndex;
            Start = start;
            DataSize = size;
            Bit = bit;
            VarType = type;
            ByteOrder = order;
        }

        public static readonly DeviceAddress Empty = new DeviceAddress(0, 0, 0, 0, 0, 0, DataType.NONE);

        public int CompareTo(DeviceAddress other)
        {
            return this.Area > other.Area ? 1 :
                this.Area < other.Area ? -1 :
                this.DBNumber > other.DBNumber ? 1 :
                this.DBNumber < other.DBNumber ? -1 :
                this.Start > other.Start ? 1 :
                this.Start < other.Start ? -1 :
                this.Bit > other.Bit ? 1 :
                this.Bit < other.Bit ? -1 : 0;
        }
    }

}
