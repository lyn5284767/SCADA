using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseLib
{
    public delegate void  GetRcvBuffer(DataForm dt);

    public interface IConnect
    {
        string LocalIPAddress{ get;set;}
        int LocalPort{get;set;}
        string RemoteIPAddress{get;set;}
        int RemotePort{get;set;}
        bool IsClosed { get; }
        bool HaveHeartBeat { get; set; }
        int RcvByteCnt { get; set; }
        int RcvByteSumCnt { get; set; }
        int SendByteCnt { get; set; }
        int SendByteSumCnt { get; set; }

        bool OpenConnect();
        void CloseConnect();
        void RcvData();
        void SendData(byte[] buffer);
        int Readbytes(int index,int len, byte[] retBuffer);
        int Readbits(int index, int bitOrder, out bool bRet);
        void ClearStatistics();

        event GetRcvBuffer GetRcvBufferEvent;
    }

    public class PingTest : IConnect
    {
        private string _localIPAddress = "127.0.0.1";
        private Ping pinger;
        DataForm dt = new DataForm();
        public string LocalIPAddress
        {
            get { return _localIPAddress; }
            set
            {
                IPAddress temp;
                if (IPAddress.TryParse(value, out temp))
                {
                    _localIPAddress = value;
                }
                else
                {
                    _localIPAddress = "127.0.0.1";
                }
            }
        }

        public int LocalPort { get{throw new NotImplementedException(); } set{throw new NotImplementedException(); } }
        public string RemoteIPAddress { get {throw new NotImplementedException(); } set {throw new NotImplementedException(); } }
        public int RemotePort { get {throw new NotImplementedException(); } set {throw new NotImplementedException(); } }
        public bool IsClosed { get { throw new NotImplementedException(); } }
        public int RcvByteCnt { get{throw new NotImplementedException(); } set {throw new NotImplementedException(); } }
        public int SendByteCnt { get{throw new NotImplementedException(); } set {throw new NotImplementedException(); } }
        public int RcvByteSumCnt { get {throw new NotImplementedException(); } set {throw new NotImplementedException(); } }
        public int SendByteSumCnt { get {throw new NotImplementedException(); } set {throw new NotImplementedException(); } }
        public bool HaveHeartBeat { get{throw new NotImplementedException(); } set {throw new NotImplementedException(); } }

        public event GetRcvBuffer GetRcvBufferEvent;

        public void CloseConnect()
        {
            if (pinger != null)
            {
                pinger.Dispose();
                pinger = null;
            }
        }

        public bool OpenConnect()
        {
            bool pingable = false;

            try
            {
                pinger = new Ping();

                if (GetRcvBufferEvent != null)
                {
                    string strTemp = "正在Ping " + LocalIPAddress.ToString();
                    dt.SetValue(true, strTemp, "", strTemp.Length);
                    GetRcvBufferEvent(dt);
                }

                PingReply reply = pinger.Send(LocalIPAddress);

                if (GetRcvBufferEvent != null)
                {
                    string strTemp = "来自 " + LocalIPAddress.ToString() + " 的回复:";
                    string strBackInfo = "Status :  " + reply.Status + "  Time : " + reply.RoundtripTime.ToString() + "  Address : " + reply.Address;
                    dt.SetValue(false, strBackInfo, strTemp, strBackInfo.Length);
                    GetRcvBufferEvent(dt);
                }

                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException e)
            {
                MessageBox.Show("PingException: "+e.Message);
                pingable = false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                    pinger = null;
                }
            }

            return pingable;
        }

        public void RcvData()
        {
            throw new NotImplementedException();
        }

        public void SendData(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public int Readbytes(int index, int len, byte[] retBuffer)
        {
            throw new NotImplementedException();
        }

        public int Readbits(int index, int bitOrder, out bool bRet)
        {
            throw new NotImplementedException();
        }

        public void ClearStatistics()
        {
            throw new NotImplementedException();
        }

        public PingTest(string localIP)
        {
            LocalIPAddress = localIP;
        }
    }

    public class UDPConnect : IConnect
    {

        internal Socket udpSend;//用来发送
        internal Socket udpRecive;//用来接收  分开来处理业务逻辑

        private bool bUseAsPro = false;
        private EndPoint toPoint;
        private EndPoint fromPoint = new IPEndPoint(IPAddress.Any, 0);
        /// <summary>
        /// 远端数据
        /// </summary>
        private byte[] rcvBuffer;
        Thread tRcv;
        public event GetRcvBuffer GetRcvBufferEvent;
        private string _localIPAddress = "127.0.0.1";
        DataForm dt = new DataForm();
        public string LocalIPAddress
        {
            get { return _localIPAddress; }
            set
            {
                IPAddress temp;
                if (IPAddress.TryParse(value, out temp))
                {
                    _localIPAddress = value;
                }
                else
                {
                    _localIPAddress = "127.0.0.1";
                }
            }
        }

        private int _localPort = 8083;
        public int LocalPort
        {
            get { return _localPort; }
            set
            {
                if (value > 0 && value < 65535)
                {
                    _localPort = value;
                }
                else
                {
                    _localPort = 8083;
                }
            }
        }

        private string _remoteIPAddress = "127.0.0.1";
        public string RemoteIPAddress
        {
            get { return _remoteIPAddress; }
            set
            {
                if (IPCheck(value))
                {
                    _remoteIPAddress =  value.Trim();
                }
                else
                {
                    _remoteIPAddress = "127.0.0.1";
                }
            }
        }

        private int _remotePort = 8084;
        public int RemotePort
        {
            get { return _remotePort; }
            set
            {
                if (value > 0 && value < 65535)
                {
                    _remotePort = value;
                }
                else
                {
                    _remotePort = 8084;
                }
            }
        }

        public bool IsClosed
        {
            get
            {
                return udpSend == null || udpRecive == null/* || !udpSend.Connected || !udpRecive.Connected*/;
            }
        }

        int _rcvByteCnt = 0;
        public int RcvByteCnt
        {
            get { return _rcvByteCnt; }
            set { _rcvByteCnt = value; }
        }

        int _sendByteCnt = 0;
        public int SendByteCnt
        {
            get { return _sendByteCnt; }
            set { _sendByteCnt = value; }
        }

        int _rcvByteSumCnt = 0;
        public int RcvByteSumCnt
        {
            get { return _rcvByteSumCnt; }
            set { _rcvByteSumCnt = value; }
        }

        int _sendByteSumCnt = 0;
        public int SendByteSumCnt
        {
            get { return _sendByteSumCnt; }
            set { _sendByteSumCnt = value; }
        }

        private System.Timers.Timer _timerCheckHeartBeat = new System.Timers.Timer();
        bool _haveHeartBeat = false;
        DateTime _rcvTime = DateTime.Now;
        public bool HaveHeartBeat
        {
            get { return _haveHeartBeat; }
            set { _haveHeartBeat = value; }
        }

        public bool IPCheck(string IP)
        {
            return Regex.IsMatch(IP.Trim(), @"^(\d{1,3}.){3}(\d{1,3})$");
        }

        public UDPConnect(string localIP,int localPort,string remoteIP,int remotePort,bool _bUseAsPro = false)
        {
            LocalIPAddress = localIP;
            LocalPort = localPort;
            RemoteIPAddress = remoteIP;
            RemotePort = remotePort;
            bUseAsPro = _bUseAsPro;

            _timerCheckHeartBeat.Elapsed += _timerCheckHeartBeat_Elapsed;
            _timerCheckHeartBeat.Interval = 500;//500ms检测一次是否有接收到数据更新
            _timerCheckHeartBeat.Enabled = true;
            _timerCheckHeartBeat.Start();
        }

        private void _timerCheckHeartBeat_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan diff = DateTime.Now - _rcvTime;

            if (diff.Milliseconds > 1000)
            {
                HaveHeartBeat = false;
            }
            else
            {
                HaveHeartBeat = true;
            }
        }

        public UDPConnect(string localIP, int localPort, bool _bUseAsPro = false)
        {
            LocalIPAddress = localIP;
            LocalPort = localPort;
            bUseAsPro = _bUseAsPro;


            _timerCheckHeartBeat.Elapsed += _timerCheckHeartBeat_Elapsed;
            _timerCheckHeartBeat.Interval = 1000;//一秒钟检测一次是否有接收到数据更新
            _timerCheckHeartBeat.Enabled = true;
            _timerCheckHeartBeat.Start();
        }

        public void CloseConnect()
        {
            if (tRcv.IsAlive)
            {
                tRcv.Abort();
                tRcv = null;
            }

            if (udpRecive != null)
            {
                udpRecive.Close();
                udpRecive = null;
            }

            if (udpSend != null)
            {
                udpSend.Close();
                udpSend = null;
            }
        }

        public bool OpenConnect()
        {
            lock (this)
            {
                try
                {
                    if (udpRecive != null)
                    {
                        udpRecive.Dispose();
                    }

                    if (udpSend != null)
                    {
                        udpSend.Dispose();
                    }

                    udpRecive = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    udpRecive.Bind(new IPEndPoint(IPAddress.Parse(LocalIPAddress), LocalPort));

                    if (rcvBuffer == null)
                    {
                        rcvBuffer = new byte[udpRecive.ReceiveBufferSize];
                    }

                    udpSend = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                    tRcv = new Thread(RcvData) { Name = "Thread_Rcv", IsBackground = true };
                    tRcv.Priority = ThreadPriority.Highest;
                    tRcv.Start();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);

                    return false;
                }

                return true;
            }

        }

        /// <summary>
        /// 主通信流程：Step 5：从UDP远端读取数据存入rcvBuffer中
        /// </summary>
        public void RcvData()
        {
            if (udpRecive == null) return;

            int length = 0;
            // 临时存储远端数据
            byte[] rcvBufferTemp = new byte[udpRecive.ReceiveBufferSize];

            while (true)
            {  
                length = udpRecive.ReceiveFrom(rcvBufferTemp, ref fromPoint);

                if (length > 0)
                {
                    _rcvTime = DateTime.Now;
                    RcvByteCnt = length;
                    RcvByteSumCnt += length;
                        

                    if (DataHelper.IsSaveBytes)
                    {
                        Log.LogBytes.WriteBytesToFile(rcvBufferTemp,length);
                    }

                    if (bUseAsPro)
                    {
                        //加入对收到的字节序列  头、校验和、帧尾的判读，并计数 正确的帧数 和校验和失败的 帧数
                        if (true)
                        {
                            Array.Copy(rcvBufferTemp, rcvBuffer, length);//程序会一直往内存中的缓存里面写入数据，但是PLCGroup是按周期把里面的数据取出后作为新接收到的数据 和 Itag 列表里面的值进行比较。
                        }
                    }
                    else
                    {
                        Array.Copy(rcvBufferTemp, rcvBuffer, length);
                    }

                    if ((GetRcvBufferEvent != null) && (length > 0))
                    {
                        dt.SetValue(false, Encoding.UTF8.GetString(rcvBuffer, 0, length), fromPoint.ToString(), length);
                        GetRcvBufferEvent(dt);
                    }
                }
            };
        }

        public void SendData(byte[] buffer)
        {
            if (toPoint == null)
            {
                try
                {
                    toPoint = new IPEndPoint(IPAddress.Parse(RemoteIPAddress), RemotePort);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            try
            {

                int iSend = udpSend.SendTo(buffer, toPoint);//返回实际发送的字节数
                SendByteCnt = iSend;
                SendByteSumCnt += iSend;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 主通信流程：Step 4：复制内存信息，存入PLC内存
        /// </summary>
        /// <param name="index">读取开始位置</param>
        /// <param name="len">读取长度</param>
        /// <param name="retBuffer">PLC数据</param>
        /// <returns></returns>
        public int Readbytes(int index, int len, byte[] retBuffer)
        {
            if (IsClosed)
            {
                return -1;
            }

            if ((index > (rcvBuffer.Length -1)) || (index + len > rcvBuffer.Length - 1))
            {
                return -2;
            }

            Array.Copy(rcvBuffer, index, retBuffer, 0, len);

            return 0;

        }

        public int Readbits(int index, int bitCnt, out bool bRet)
        {
            if (IsClosed)
            {
                bRet = false;
                return -1;
            }

            if (index > (rcvBuffer.Length - 1))
            {
                bRet = false;
                return -2;
            }

            byte bTemp = rcvBuffer[index];

            if ((bTemp & (1<< bitCnt)) > 0)
            {
                bRet = true;
            }
            else
            {
                bRet = false;
            }

            return 0;
        }

        public void ClearStatistics()
        {
            RcvByteCnt = 0;
            SendByteCnt = 0;
            SendByteSumCnt = 0;
            RcvByteSumCnt = 0;
        }
    }
}
