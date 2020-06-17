using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Timers;
using DataService;
using System.Net;
using System.Net.Sockets;
using DatabaseLib;

namespace DemoDriver
{
    [Description("二层台机械手UDP协议")]
    public sealed class SecondFloorPLCDriver : IPLCDriver, IMultiReadWrite
    {
        IConnect con = new UDPConnect(DataHelper.LocalIP, DataHelper.LocalPort, DataHelper.RemoteIP, DataHelper.RemotePort,true);
        int _rack;
        int _slot;
        string _IP;
        object _async = new object();
        DateTime _closeTime = DateTime.Now;

        static string GetLocalIP()
        {
            string hostName = Dns.GetHostName();
            string localIPAddress = "";
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);

            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                { 
                    localIPAddress = ip.ToString();
                    break;
                }
            }

            return localIPAddress;
        }

        public int Limit
        {
            get
            {
                return 1500;
            }
        }

        short _id;
        public short ID
        {
            get
            {
                return _id;
            }
        }

        bool _closed = true;
        public bool IsClosed
        {
            get
            {
                return _closed;
            }
        }

        public int PDU
        {
            get
            {
                return 1500;//240
            }
        }

        int _timeOut = 1000;
        public int TimeOut
        {
            get
            {
                return _timeOut;
            }
            set
            {
                _timeOut = value;
            }
        }

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string ServerName
        {
            get
            {
                return _IP;
            }
            set
            {
                _IP = value;
            }
        }

        public int Rack
        {
            get
            {
                return _rack;
            }
            set
            {
                _rack = value;
            }
        }

        public int Slot
        {
            get
            {
                return _slot;
            }
            set
            {
                _slot = value;
            }
        }

        public SecondFloorPLCDriver(IDataServer server, short id, string name)
        {
            _id = id;
            _server = server;
            _name = name;
        }

        List<IGroup> _groups = new List<IGroup>();
        public IEnumerable<IGroup> Groups
        {
            get { return _groups; }
        }

        IDataServer _server;
        public IDataServer Parent
        {
            get { return _server; }
        }

        public IGroup AddGroup(string name, short id, int updateRate, float deadBand, bool active)//该函数负责 把从数据库中读取的 Itag 根据GroupID 归为一组，有自己的一组刷新周期 
        {
            NetBytePLCGroup grp = new NetBytePLCGroup(id, name, updateRate, active, this);
            _groups.Add(grp);//所有的组别 都加入到驱动里面的 _groups
            return grp;
        }

        public bool RemoveGroup(IGroup grp)
        {
            grp.IsActive = false;
            return _groups.Remove(grp);
        }

        public string GetAddress(DeviceAddress address)
        {

            string addr = "";
             
            switch (address.VarType)
            {
                case DataType.NONE:
                    return addr;
                case DataType.BOOL:
                    return string.Concat(addr, address.Start, ".", address.Bit);//开关型
                case DataType.BYTE:
                    return string.Concat(addr, "B", address.Start);//字节型
                case DataType.SHORT:
                    return string.Concat(addr, "H", address.Start);//短整型
                case DataType.WORD:
                    return string.Concat(addr, "W", address.Start);//单字型
                case DataType.DWORD:
                    return string.Concat(addr, "D", address.Start);//双子型
                case DataType.INT:
                    return string.Concat(addr, "I", address.Start);//长整型
                case DataType.FLOAT:
                    return string.Concat(addr, "F", address.Start);//浮点型
                case DataType.SYS:
                    return string.Concat(addr, "C", address.Start);//系统型
                case DataType.STR:
                    return string.Concat(addr, "S", address.Start);//ASCII 字符串
                default:
                    return addr;
            }
        }

        public DeviceAddress GetDeviceAddress(string address)
        {
            DeviceAddress plcAddr = new DeviceAddress();
            if (string.IsNullOrEmpty(address) || address.Length < 2) return plcAddr;

            plcAddr.Area = 0;
            plcAddr.DBNumber = 0;

            int index = address.IndexOf('.');

            if (index > 0)
            {
                int start = int.Parse(address.Substring(0, index));
                byte bit = byte.Parse(address.RightFrom(index));
                plcAddr.Start = bit > 8 ? start+1: start;
                plcAddr.Bit = (byte)(bit > 7 ? bit - 8 : bit);
            }
            else
            {
                plcAddr.Start = int.Parse(address.Substring(1));
            }

            return plcAddr;
        }

        public bool Connect()
        {
            lock (_async)
            {
                if (!_closed) return true;
                //double sec = (DateTime.Now - _closeTime).TotalMilliseconds;
                //if (sec < 6000)
                //    System.Threading.Thread.Sleep(6000 - (int)sec);
                
                if (con.IsClosed)
                {
                    if (con.OpenConnect())
                    {
                        _closed = false;
                        return true;
                    }
                    else
                    {
                        _closed = true;
                        return false;
                    }
                    
                }
            }
            _closed = true;
            return false;
        }

        public void Dispose()
        {
            lock (_async)
            {
                if (con != null)
                {
                    con.CloseConnect();
                }

                foreach (IGroup grp in _groups)
                {
                    grp.Dispose();
                }
                _closed = true;
            }
        }

        /// <summary>
        /// 主通信流程：Step 3：根据设定获取UDP远端存入内存数据
        /// </summary>
        /// <param name="address">数据在内存中存储信息</param>
        /// <param name="len">存储长度</param>
        /// <returns>远端返回数据</returns>
        public byte[] ReadBytes(DeviceAddress address, ushort len)//从PLC中读取自己数组
        {
            if (!con.IsClosed)
            {
                byte[] buffer = new byte[len];
                int res = -1;
                lock (_async)
                {
                    res = con.IsClosed ? -1 : con.Readbytes(address.Start, len, buffer);
                }
                if (res == 0)
                    return buffer;
                _closed = true;
                _closeTime = DateTime.Now;
                if (OnError != null)
                {
                    OnError(this, new IOErrorEventArgs(daveStrerror(res)));
                    _closeTime = DateTime.Now;
                }
            }
            return null;
        }

        string daveStrerror(int code)
        {
            switch (code)
            {
                case -1: return "UDP 连接已经关闭!";
                case -2: return "DeviceAddress 地址参数超出范围!";
                default: return "no message defined!";
            }
        }

        public ItemData<string> ReadString(DeviceAddress address, ushort size = 0xFF)
        {
            if (!con.IsClosed)
            {
                byte[] buffer = new byte[size];
                int res = -1;
                lock (_async)
                {
                    res = con.Readbytes( address.Start, size, buffer);
                }
                if (res == 0)
                    return new ItemData<string>(Utility.ConvertToStringEx(buffer), 0, QUALITIES.QUALITY_GOOD);
                _closed = true;
                _closeTime = DateTime.Now;
                if (OnError != null)
                {
                    OnError(this, new IOErrorEventArgs(daveStrerror(res)));
                }
            }
            return new ItemData<string>(string.Empty, 0, QUALITIES.QUALITY_NOT_CONNECTED);
        }

        public ItemData<int> ReadInt32(DeviceAddress address)
        {
            if (!con.IsClosed)
            {
                byte[] buffer = new byte[4];
                int res = -1;
                lock (_async)
                {
                    res = con.Readbytes(address.Start, 4, buffer);
                    if (res == 0) return new ItemData<int>(BitConverter.ToInt32(buffer,0), 0, QUALITIES.QUALITY_GOOD);
                }
                _closed = true;
                _closeTime = DateTime.Now;
                if (OnError != null)
                {
                    OnError(this, new IOErrorEventArgs(daveStrerror(res)));
                }
            }
            return new ItemData<int>(0, 0, QUALITIES.QUALITY_NOT_CONNECTED); ;
        }

        public ItemData<uint> ReadUInt32(DeviceAddress address)
        {
            if (!con.IsClosed)
            {
                byte[] buffer = new byte[4];
                int res = -1;
                lock (_async)
                {
                    res = con.Readbytes(address.Start, 4, buffer);
                    if (res == 0) return new ItemData<uint>(BitConverter.ToUInt32(buffer,0), 0, QUALITIES.QUALITY_GOOD);
                }
                _closed = true;
                _closeTime = DateTime.Now;
                if (OnError != null)
                {
                    OnError(this, new IOErrorEventArgs(daveStrerror(res)));
                }
            }
            return new ItemData<uint>(0, 0, QUALITIES.QUALITY_NOT_CONNECTED); ;
        }

        public ItemData<ushort> ReadUInt16(DeviceAddress address)
        {
            if (!con.IsClosed)
            {
                byte[] buffer = new byte[2];
                int res = -1;
                lock (_async)
                {
                    res = con.Readbytes(address.Start, 2, buffer);
                    if (res == 0) return new ItemData<ushort>(BitConverter.ToUInt16(buffer,0), 0, QUALITIES.QUALITY_GOOD);
                }
                _closed = true;
                _closeTime = DateTime.Now;
                if (OnError != null)
                {
                    OnError(this, new IOErrorEventArgs(daveStrerror(res)));
                }
            }
            return new ItemData<ushort>(0, 0, QUALITIES.QUALITY_NOT_CONNECTED); ;
        }

        public ItemData<short> ReadInt16(DeviceAddress address)
        {
            if (!con.IsClosed)
            {
                byte[] buffer = new byte[2];
                int res = -1; 
                lock (_async)
                {
                    res = con.Readbytes(address.Start, 2, buffer);
                    if (res == 0) return new ItemData<short>(BitConverter.ToInt16(buffer,0), 0, QUALITIES.QUALITY_GOOD);
                }
                _closed = true;
                _closeTime = DateTime.Now;
                if (OnError != null)
                {
                    OnError(this, new IOErrorEventArgs(daveStrerror(res)));
                }
            }
            return new ItemData<short>(0, 0, QUALITIES.QUALITY_NOT_CONNECTED); ;
        }

        public ItemData<byte> ReadByte(DeviceAddress address)
        {
            if (!con.IsClosed)
            {
                int res = -1;
                byte[] buffer = new byte[1];
                lock (_async)
                {
                    res = con.Readbytes(address.Start, 1, buffer);
                    if (res == 0)
                        return new ItemData<byte>((byte)buffer[0], 0, QUALITIES.QUALITY_GOOD);
                }
                _closed = true;
                _closeTime = DateTime.Now;
                if (OnError != null)
                {
                    OnError(this, new IOErrorEventArgs(daveStrerror(res)));
                }
            }
            return new ItemData<byte>(0, 0, QUALITIES.QUALITY_NOT_CONNECTED); ;
        }

        public ItemData<bool> ReadBit(DeviceAddress address)
        {
            int res = -1;
            if (!con.IsClosed)
            {
                lock (_async)
                {
                    bool bRet = false;
                    res = con.Readbits(address.Start,address.Bit, out bRet);//修改了原地址上的Bug
                    if (res == 0) return new ItemData<bool>(bRet, 0, QUALITIES.QUALITY_GOOD);
                }
                _closed = true;
                _closeTime = DateTime.Now;
                if (OnError != null) { OnError(this, new IOErrorEventArgs(daveStrerror(res))); }
            }
            return new ItemData<bool>(false, 0, QUALITIES.QUALITY_NOT_CONNECTED);
        }

        public ItemData<float> ReadFloat(DeviceAddress address)
        {
            if (!con.IsClosed)
            {
                byte[] buffer = new byte[4];
                int res = -1;
                lock (_async)
                {
                    res = con.Readbytes( address.Start, 4, buffer);
                    if (res == 0) return new ItemData<float>(BitConverter.ToSingle(buffer,0), 0, QUALITIES.QUALITY_GOOD);
                }
                _closed = true;
                _closeTime = DateTime.Now;
                if (OnError != null)
                {
                    OnError(this, new IOErrorEventArgs(daveStrerror(res))); _closeTime = DateTime.Now;
                }
            }
            return new ItemData<float>(0, 0, QUALITIES.QUALITY_NOT_CONNECTED); ;
        }


        public ItemData<object> ReadValue(DeviceAddress address)
        {
            return this.ReadValueEx(address);
        }

        public int WriteBit(DeviceAddress address, bool bit)
        {
            throw new NotImplementedException();
        }

        public int WriteBits(DeviceAddress address, byte value)
        {
            throw new NotImplementedException();
        }

        public int WriteInt16(DeviceAddress address, short value)
        {
            throw new NotImplementedException();
        }

        public int WriteUInt16(DeviceAddress address, ushort value)
        {
            throw new NotImplementedException();
        }

        public int WriteUInt32(DeviceAddress address, uint value)
        {
            throw new NotImplementedException();
        }

        public int WriteInt32(DeviceAddress address, int value)
        {
            throw new NotImplementedException();
        }

        public int WriteFloat(DeviceAddress address, float value)
        {
            throw new NotImplementedException();
        }

        public int WriteString(DeviceAddress address, string str)
        {
            throw new NotImplementedException();
        }

        public int WriteValue(DeviceAddress address, object value)
        {
            return this.WriteValueEx(address, value);
        }

        public int WriteBytes(DeviceAddress address, byte[] bit)
        {
            throw new NotImplementedException();
        }
        public ItemData<Storage>[] ReadMultiple(DeviceAddress[] addrsArr)
        {
            return ReadMultipleInternal(addrsArr);
        }

        ItemData<Storage>[] ReadMultipleInternal(DeviceAddress[] addrsArr)
        {
            int len = addrsArr.Length;

            lock (_async)
            {
                if (con.IsClosed) return null;

                ItemData<Storage>[] itemArr = new ItemData<Storage>[len];
                bool bTemp = false;
                byte[] btTemp;
                for (int i = 0; i < len; i++)
                {
                        itemArr[i].Quality = QUALITIES.QUALITY_GOOD;
                        switch (addrsArr[i].VarType)
                        {
                            case DataType.BOOL:
                                con.Readbits(addrsArr[i].Start, addrsArr[i].Bit, out bTemp);
                                itemArr[i].Value.Boolean = bTemp;
                                break;
                            case DataType.BYTE:
                                btTemp = new byte[1];
                                con.Readbytes(addrsArr[i].Start, 1,btTemp);
                                itemArr[i].Value.Byte = (byte)btTemp[0];
                                break;
                            case DataType.WORD:
                                btTemp = new byte[2];
                                con.Readbytes(addrsArr[i].Start, 2, btTemp);
                                itemArr[i].Value.Word = BitConverter.ToUInt16(btTemp, 0);
                                break;
                            case DataType.SHORT:
                                btTemp = new byte[2];
                                con.Readbytes(addrsArr[i].Start, 2, btTemp);
                                itemArr[i].Value.Int16 = BitConverter.ToInt16(btTemp, 0);
                                break;
                            case DataType.DWORD:
                                btTemp = new byte[4];
                                con.Readbytes(addrsArr[i].Start, 4, btTemp);
                                itemArr[i].Value.DWord = BitConverter.ToUInt32(btTemp, 0);
                                break;
                            case DataType.INT:
                                btTemp = new byte[4];
                                con.Readbytes(addrsArr[i].Start, 4, btTemp);
                                itemArr[i].Value.Int32 = BitConverter.ToInt32(btTemp, 0);
                                break;
                            case DataType.FLOAT:
                                btTemp = new byte[4];
                                con.Readbytes(addrsArr[i].Start, 4, btTemp);
                                itemArr[i].Value.Single = BitConverter.ToSingle(btTemp, 0);
                                break;
                    }
                }
                return itemArr;
            }
        }

        public int WriteMultiple(DeviceAddress[] addrArr, object[] buffer)
        {
            throw new NotImplementedException();
        }

        int WriteMultipleInternal(DeviceAddress[] addrArr, object[] buffer)
        {
            throw new NotImplementedException();
        }

        public bool SendBytes(byte[] bytes)
        {
            if (con != null && !con.IsClosed)
            {
                con.SendData(bytes);
                return true;
            }

            return false;
        }

        public event IOErrorEventHandler OnError;
    }

}
