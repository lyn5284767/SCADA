using DatabaseLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SDK_HANDLE = System.Int32;

namespace HBGKTest.YiTongCamera
{
    /// <summary>
    /// YiTongCameraControl.xaml 的交互逻辑
    /// </summary>
    public partial class YiTongCameraControl : UserControl,ICameraFactory
    {
        public struct DEV_INFO
        {
            public int nListNum;           //position in the list
            public SDK_HANDLE lLoginID;         //login handle
            public int lID;             //device ID
            public string szDevName;        //device name
            public string szIpaddress;      //device IP
            public string szUserName;       //user name
            public string szPsw;            //pass word
            public int nPort;               //port number
            public int nTotalChannel;       //total channel
            public SDK_CONFIG_NET_COMMON_V2 NetComm;                  // net config
            public H264_DVR_DEVICEINFO NetDeviceInfo;
            public byte bSerialID;//be SerialNumber login
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szSerIP;//server ip
            public int nSerPort;           //server port
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string szSerialInfo;  //SerialNumber
            public bool bStartAlarm;
        }

        public struct CHANNEL_INFO
        {
            string szChnnelName;            // 通道名称.
            public int nChnnID;                         // 用于地图节点管理
            public int nChannelNo;                          // 通道号.
            public int bUserRight;                          // 用户权限(使能).
            public int PreViewChannel;                      // 预览通道
            public int nStreamType;                     // 码流类型
            public DEV_INFO DeviceInfo;                         // 设备信息.
            public int nCombinType;                     // 组合编码模式
            public int dwTreeItem;                          //记录设备树中的节点句柄，可以节省查找事件
            public int nFlag;                               //1为选择为录像 0 为没有被选择 2 为正在录像
            public int nWndIndex;
        };

        public DEV_INFO devInfo = new DEV_INFO(); // 设备信息

        public CHANNEL_INFO ChannelInfo = new CHANNEL_INFO(); // 渠道信息
        public SDK_HANDLE m_iPlayhandle;    //play handle
        public SDK_HANDLE m_lLogin; //login handle
        public int m_iChannel; //play channel
        bool m_bRecord; //is recording or not
        private NetSDK.fDisConnect disCallback;

        public YiTongCameraControl(ChannelInfo info)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.Info = info;
        }

        public int InitSDK()
        {
            //initialize
            NetSDK.g_config.nSDKType = NetSDK.SDK_TYPE.SDK_TYPE_GENERAL;

            //disCallback = new NetSDK.fDisConnect(DisConnectBackCallFunc);
            //GC.KeepAlive(disCallback);
            int bResult = NetSDK.H264_DVR_Init(disCallback, this.VideoPanel.Handle);
            NetSDK.H264_DVR_SetConnectTime(1000, 1);
            return bResult;
        }
        public bool ExitSDK()
        {
            return NetSDK.H264_DVR_Cleanup();
        }


        //void DisConnectBackCallFunc(SDK_HANDLE lLoginID, string pchDVRIP, int nDVRPort, IntPtr dwUser)
        //{
        //    for (int i = 0; i < 16; i++)
        //    {
        //        if (lLoginID == m_videoform[i].GetLoginHandle())
        //        {
        //            m_videoform[i].OnDisconnct();
        //        }
        //    }


        //    foreach (DEV_INFO devinfo in dictDevInfo.Values)
        //    {
        //        if (devinfo.lLoginID == lLoginID)
        //        {
        //            NetSDK.H264_DVR_Logout(lLoginID);
        //            dictDevInfo.Remove(devinfo.lLoginID);
        //            dictDiscontDev.Add(devinfo.lLoginID, devinfo);
        //            break;
        //        }
        //    }

        //    if (dictDiscontDev.Count > 0)
        //    {

        //        timerDisconnect.Enabled = true;
        //        timerDisconnect.Start();
        //    }
        //}

        ///// <summary>
        ///// 初始化摄像头参数
        ///// </summary>
        ///// <returns></returns>
        //public bool InintCamera(string cameraNum)
        //{
        //    try
        //    {
        //        ChannelInfo chInfo = GetConfigPara(cameraNum);
        //        if (chInfo != null)
        //        {
        //            H264_DVR_DEVICEINFO dvrdevInfo = new H264_DVR_DEVICEINFO();
        //            dvrdevInfo.Init();
        //            int nError;
        //            SDK_HANDLE nLoginID = NetSDK.H264_DVR_Login(chInfo.RemoteIP, ushort.Parse(chInfo.RemotePort.ToString()), chInfo.RemoteUser, chInfo.RemotePwd, ref dvrdevInfo, out nError, SocketStyle.TCPSOCKET);
        //            if (nLoginID > 0)
        //            {
        //                devInfo.szDevName = "YiTong";
        //                devInfo.lLoginID = nLoginID;
        //                devInfo.nPort = Int32.Parse(chInfo.RemotePort.ToString());
        //                devInfo.szIpaddress = chInfo.RemoteIP;
        //                devInfo.szUserName = chInfo.RemoteUser;
        //                devInfo.szPsw = chInfo.RemotePwd;
        //                devInfo.NetDeviceInfo = dvrdevInfo;

        //                ChannelInfo.nChannelNo = 0;
        //                ChannelInfo.nWndIndex = -1;
        //            }
        //            else //摄像头登录失败
        //            {
        //                return false;
        //            }
        //        }
        //        else // 未读取摄像头信息
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
        //        return false;
        //    }
        //}
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns>摄像头配置信息</returns>
        private ChannelInfo GetConfigPara(string cameraNum)
        {
            try
            {
                ChannelInfo chInfo = new ChannelInfo();
                string configPath = System.Environment.CurrentDirectory + @"\Config.ini";
                int STRINGMAX = 255;
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

                    WinAPI.GetPrivateProfileString("CAMERA1", "CHLID", strChlID, sb, STRINGMAX, configPath);
                    strChlID = sb.ToString();
                    chInfo.ChlID = strChlID;

                    WinAPI.GetPrivateProfileString("CAMERA1", "NDEVICETYPE", strNDeviceType, sb, STRINGMAX, configPath);
                    strNDeviceType = sb.ToString();
                    int.TryParse(strNDeviceType, out chInfo.nDeviceType);

                    WinAPI.GetPrivateProfileString("CAMERA1", "REMOTECHANNLE", strRemoteChannle, sb, STRINGMAX, configPath);
                    strRemoteChannle = sb.ToString();
                    chInfo.RemoteChannle = strRemoteChannle;

                    WinAPI.GetPrivateProfileString("CAMERA1", "REMOTEIP", strRemoteIP, sb, STRINGMAX, configPath);
                    strRemoteIP = sb.ToString();
                    chInfo.RemoteIP = strRemoteIP;

                    WinAPI.GetPrivateProfileString("CAMERA1", "REMOTEPORT", strRemotePort, sb, STRINGMAX, configPath);
                    strRemotePort = sb.ToString();
                    int tmpRemotePort = 0;
                    int.TryParse(strRemotePort, out tmpRemotePort);
                    chInfo.RemotePort = tmpRemotePort;

                    WinAPI.GetPrivateProfileString("CAMERA1", "REMOTEUSER", strRemoteUser, sb, STRINGMAX, configPath);
                    strRemoteUser = sb.ToString();
                    chInfo.RemoteUser = strRemoteUser;

                    WinAPI.GetPrivateProfileString("CAMERA1", "REMOTEPWD", strRemotePwd, sb, STRINGMAX, configPath);
                    strRemotePwd = sb.ToString();
                    chInfo.RemotePwd = strRemotePwd;

                    WinAPI.GetPrivateProfileString("CAMERA1", "NPLAYPORT", strNPlayPort, sb, STRINGMAX, configPath);
                    strNPlayPort = sb.ToString();
                    int tmpNPlayPort = 0;
                    int.TryParse(strNPlayPort, out tmpNPlayPort);
                    chInfo.nPlayPort = tmpNPlayPort;

                    WinAPI.GetPrivateProfileString("CAMERA1", "PTZPORT", strPtzPort, sb, STRINGMAX, configPath);
                    strPtzPort = sb.ToString();
                    int tmpPtzPort = 0;
                    int.TryParse(strPtzPort, out tmpPtzPort);
                    chInfo.PtzPort = tmpPtzPort;

                    return chInfo;//正常返回。
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

        //private void PlayCamera()
        //{
        //    long iRealHandle = ConnectRealPlay(ref devInfo, ChannelInfo.nChannelNo);
        //}
        /// <summary>
        /// 连接并播放视频
        /// </summary>
        /// <param name="pDev"></param>
        /// <param name="nChannel"></param>
        /// <returns></returns>
        public long ConnectRealPlay(ref DEV_INFO pDev, int nChannel)
        {
            if (m_iPlayhandle != -1)
            {

                if (0 != NetSDK.H264_DVR_StopRealPlay(m_iPlayhandle, (uint)0))
                {
                    //TRACE("H264_DVR_StopRealPlay fail m_iPlayhandle = %d", m_iPlayhandle);
                }
            }

            H264_DVR_CLIENTINFO playstru = new H264_DVR_CLIENTINFO();

            playstru.nChannel = nChannel;
            playstru.nStream = 0;
            playstru.nMode = 0;
            playstru.hWnd = this.VideoPanel.Handle;
            m_iPlayhandle = NetSDK.H264_DVR_RealPlay(pDev.lLoginID, ref playstru);
            if (m_iPlayhandle <= 0)
            {
                Int32 dwErr = NetSDK.H264_DVR_GetLastError();
                StringBuilder sTemp = new StringBuilder("");
                sTemp.AppendFormat("access {0} channel{1} fail, dwErr = {2}", pDev.szDevName, nChannel, dwErr);
                MessageBox.Show(sTemp.ToString());
            }
            else
            {
                NetSDK.H264_DVR_MakeKeyFrame(pDev.lLoginID, nChannel, 0);
            }
            m_lLogin = pDev.lLoginID;
            m_iChannel = nChannel;
            return m_iPlayhandle;
        }

        /// <summary>
        /// 保存录像
        /// </summary>
        /// <returns></returns>
        public bool SaveRecord(string filePath, string fileName)
        {
            if (m_iPlayhandle <= 0)
            {
                return false;
            }

            DateTime time = DateTime.Now;
            String cFilename = fileName;
            if (m_bRecord)
            {

                if (NetSDK.H264_DVR_StopLocalRecord(m_iPlayhandle))
                {
                    m_bRecord = false;
                }
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(filePath);
                if (!dir.Exists)
                {
                    dir.Create();
                }

                if (NetSDK.H264_DVR_StartLocalRecord(m_iPlayhandle, filePath + "/"+fileName, (int)MEDIA_FILE_TYPE.MEDIA_FILE_AVI))
                {
                    m_bRecord = true;
                }
            }

            return true;
        }

       public ChannelInfo Info { get; set; }

        /// <summary>
        /// 初始化摄像头
        /// </summary>
        /// <returns></returns>
        public bool InitCamera(ChannelInfo chInfo)
        {
            try
            {
                if (chInfo != null)
                {
                    InitSDK();
                    H264_DVR_DEVICEINFO dvrdevInfo = new H264_DVR_DEVICEINFO();
                    dvrdevInfo.Init();
                    int nError;
                    SDK_HANDLE nLoginID = NetSDK.H264_DVR_Login(chInfo.RemoteIP, ushort.Parse(chInfo.RemotePort.ToString()), chInfo.RemoteUser, chInfo.RemotePwd, ref dvrdevInfo, out nError, SocketStyle.TCPSOCKET);
                    if (nLoginID > 0)
                    {
                        devInfo.szDevName = "YiTong";
                        devInfo.lLoginID = nLoginID;
                        devInfo.nPort = Int32.Parse(chInfo.RemotePort.ToString());
                        devInfo.szIpaddress = chInfo.RemoteIP;
                        devInfo.szUserName = chInfo.RemoteUser;
                        devInfo.szPsw = chInfo.RemotePwd;
                        devInfo.NetDeviceInfo = dvrdevInfo;

                        ChannelInfo.nChannelNo = 0;
                        ChannelInfo.nWndIndex = -1;
                        PlayCamera();
                    }
                    else //摄像头登录失败
                    {
                        MessageBox.Show("一通摄像头调用失败，请检查配置！");
                        return false;
                    }
                }
                else // 未读取摄像头信息
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                return false;
            }
        }

        public void StopCamera()
        {
            NetSDK.H264_DVR_StopRealPlay(m_iPlayhandle, (uint)0);
            NetSDK.H264_DVR_Logout(devInfo.lLoginID);
            //ExitSDK();
        }

        public void SaveFile(string filePath,string fileName)
        {
            bool suc = SaveRecord(filePath, fileName);
        }

        public void StopFile()
        {
            if (NetSDK.H264_DVR_StopLocalRecord(m_iPlayhandle))
            {
                m_bRecord = false;
            }
        }
        /// <summary>
        /// 设置界面大小
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void SetSize(double height, double width)
        {
            this.Height = height;
            this.Width = width;
        }

        public void PlayCamera()
        {
            long iRealHandle = ConnectRealPlay(ref devInfo, ChannelInfo.nChannelNo);
        }

        public event ChangeVideo ChangeVideoEvent;

        private void VideoPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (FullScreenEvent != null)
                {
                    FullScreenEvent();
                }
            }
            else
            {
                if (ChangeVideoEvent != null)
                {
                    ChangeVideoEvent();
                }
            }
        }

        public event FullScreen FullScreenEvent;

        private void VideoPanel_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (FullScreenEvent != null)
            {
                FullScreenEvent();
            }
        }
    }
}
