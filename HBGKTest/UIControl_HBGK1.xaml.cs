using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using Path = System.IO.Path;
using System.Drawing;

namespace HBGKTest
{
    /// <summary>
    /// UIControl_HBGK1.xaml 的交互逻辑
    /// </summary>
    public partial class UIControl_HBGK1 : UserControl,ICameraFactory
    {
        IP_NET_DVR_DEVICEINFO deviceInfo = new IP_NET_DVR_DEVICEINFO();
        //播放Panel中心和鼠标坐标，两点的差距
        public float X = 0;
        public float Y = 0;
        //是否移动
        public bool isMove = false;
        //在pnlReplayVideo的鼠标形状
        //private System.Windows.Forms.Cursor sbFlag = System.Windows.Forms.Cursors.Default;
        ///// <summary>
        ///// 设置或获取鼠标样式
        ///// </summary>
        //public System.Windows.Forms.Cursor SbFlag
        //{
        //    get
        //    {
        //        return sbFlag;
        //    }
        //    set
        //    {
        //        sbFlag = value;
        //    }
        //}

        //是否控制pnlReplayVideo
        private bool isControl = false;
        //云台控制的速度比例
        double PSZspeed = 1.0;
        /// <summary>
        /// 用户编号，登陆的返回值
        /// </summary>
        private int m_userID = 0;
        /// <summary>
        /// 播放后的返回值
        /// </summary>
        public int m_realPlay = -1;
        public int m_bSetParam = -1;


        public bool haveLogin = false;


        fRealDataCallBack fReadlCallBack = null;
        fDecCallBackFunction fDecodecall = null;
        StatusEventCallBack fStatusCallBack = null;

        /// <summary>
        /// 速度
        /// </summary>
        private string speed = "6";

        
        public UIControl_HBGK1(ChannelInfo info)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.Info = info;
        }

        private static bool bHaveInitTwoDll = false;

        //public UIControl_HBGK1(ChannelInfo channelInfo)
        //{
        //    System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        //    InitializeComponent();
        //    this.infoChannel = channelInfo;
        //    if (!bHaveInitTwoDll)
        //    {
        //        bHaveInitTwoDll = true;
        //        PLAYERDLL.IP_TPS_Init();
        //        NETSDKDLL.IP_NET_DVR_Init();
        //    }
        //    fDecodecall = new fDecCallBackFunction(OnDecCallBackFunction);
        //    fReadlCallBack = new fRealDataCallBack(OnRealDataCallBack);
        //    fStatusCallBack = new StatusEventCallBack(OnStatusEventCallBack);

        //    try
        //    {
        //        if (!InitAndLogin())
        //        {
        //            return;
        //        }
        //        m_realPlay = RealPlayVideo();
        //        if (m_realPlay > 0)
        //        {
        //            //ILog.WriteEventLog("播放成功！");
        //            return;
        //        }
        //        else
        //        {
        //            int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
        //            MessageBox.Show("视频预览失败，错误编号：" + GetError(ID));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public int OnStatusEventCallBack(int lUser, int nStateCode, IntPtr pResponse, IntPtr pUser)
        {
            if (lUser == m_userID)
            {
                if (nStateCode == (int)enumNetSatateEvent.EVENT_LOGINOK)
                {
                    haveLogin = true;
                    if (m_realPlay <= 0)
                    {
                        m_realPlay = RealPlayVideo();
                    }
                }
                else if (nStateCode == (int)enumNetSatateEvent.EVENT_LOGINFAILED || nStateCode == (int)enumNetSatateEvent.EVENT_LOGIN_USERERROR)
                {
                    haveLogin = false;
                }
            }

            return 0;
        }

        public int OnDecCallBackFunction(int nPort, IntPtr pBuf, int nSize, ref FRAME_INFO pFrameInfo, IntPtr pUser, int nReserved2)
        {
            return 0;
        }
        #region ICCTVBrowse 成员

        //public event EventClickAxAVShow eventClickAxAVShow;

        //public event EventControlAV eventControlAV;

        //public event EventOnLBDown eventOnLBDown;

        //public event EventOnLBUp eventOnLBUp;


        //public event EventDoubleClick eventDoubleClick;

        //public event EventDMenuClick eventDMenuClick;

        public void SetChoosed(bool chooseFlag)
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        string saveFileFullPath = "";
        string newFileName = "";
        public string StartKinescope(string filePath)
        {
            try
            {
                string saveFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".hik";
                return StartKinescope(filePath, saveFileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string StartKinescope(string filePath, string fileName)
        {
            try
            {
                saveFileFullPath = filePath + "\\" + fileName;
                int startRecord = 0;

                //ILog.WriteEventLog("saveFileFullPath:" + saveFileFullPath);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                //startRecord = NETSDKDLL.IP_NET_DVR_StartRecord(m_realPlay, saveFileFullPath.Substring(0, saveFileFullPath.Length - 4) + ".avi", 1800, 1800);
                startRecord = NETSDKDLL.IP_NET_DVR_StartRecord(m_realPlay, saveFileFullPath, 100, 1800);
                //ILog.WriteEventLog("startRecord:" + startRecord);

                newFileName = Path.GetDirectoryName(saveFileFullPath);

                //ILog.WriteEventLog("newFileName:" + newFileName + "\\");
                if (startRecord > 0)
                {
                    return "开始录像,路径-" + newFileName;
                }
                else
                {
                    int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                    return "录像失败:" + ID + GetError(ID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string StopKinescope()
        {
            try
            {
                int stopRecord = 0;
                string result = "";
                stopRecord = NETSDKDLL.IP_NET_DVR_StopRecord(m_realPlay);
                Thread.Sleep(500);
                //ILog.WriteEventLog(("源文件：" + saveFileFullPath + ",目标文件：" + newFileName));

                if (stopRecord > 0)
                {
                    result = "停止录像";
                }
                else
                {
                    int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                    result = "停止录像失败:" + ID + GetError(ID);
                }
                return result;
            }
            catch (Exception)
            {
                return "录像或下载失败";
            }
        }
        public string TakePictures(string filePath, string fileName)
        {
            try
            {

                //ILog.WriteEventLog("进入截图+:" + filePath);
                string result = "";
                string tmpFileName = filePath + "\\" + DateTime.Now.ToFileTime().ToString() + ".bmp";


                //bool getPicture = NETSDKDLL.HB_SDVR_CapturePicture(m_realPlay, tmpFileName);
                int getPicture = PLAYERDLL.IP_TPS_CatchPic(m_realPlay, tmpFileName);
                Thread.Sleep(1000);


                System.Drawing.Image bmp = Bitmap.FromFile(tmpFileName);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                bmp.Save(filePath + "\\" + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                bmp.Dispose();

                try
                {
                    if (File.Exists(tmpFileName))
                    {
                        File.Delete(tmpFileName);
                    }
                }
                catch
                {

                }

                if (getPicture !=0)
                {

                    result = "截图成功，路径-" + filePath + "-ID:" + Info.ErrId + "-" + tmpFileName;
                }
                else
                {
                    int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                    result = "截图失败:" + ID + GetError(ID);
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                return "截图失败";
            }
        }

        //public string TakePictures(string filePath, string fileName)
        //{
        //    //try
        //    //{

        //    //    ILog.WriteEventLog("进入截图+:" + filePath);
        //    //    string result = "";
        //    //    string tmpFileName = filePath + "\\" + DateTime.Now.ToFileTime().ToString() + ".bmp";


        //    //    bool getPicture = NETSDKDLL.HB_SDVR_CapturePicture(m_realPlay, tmpFileName);
        //    //    Thread.Sleep(1000);


        //    //    Image bmp = Bitmap.FromFile(tmpFileName);

        //    //    if (!Directory.Exists(filePath))
        //    //    {
        //    //        Directory.CreateDirectory(filePath);
        //    //    }
        //    //    bmp.Save(filePath + "\\" + fileName, ImageFormat.Jpeg);

        //    //    bmp.Dispose();

        //    //    try
        //    //    {
        //    //        if (File.Exists(tmpFileName))
        //    //        {
        //    //            File.Delete(tmpFileName);
        //    //        }
        //    //    }
        //    //    catch
        //    //    {

        //    //    }

        //    //    if (getPicture)
        //    //    {

        //    //        result = "截图成功，路径-" + filePath + "-ID:" + infoChannel.ErrId + "-" + tmpFileName;
        //    //    }
        //    //    else
        //    //    {
        //    //        int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
        //    //        result = "截图失败:" + ID + GetError(ID);
        //    //    }
        //    //    return result;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    return "截图失败";
        //    //}
        //}

        public string TakePictures(string filePath)
        {
            try
            {
                string tmpFileName = DateTime.Now.ToFileTime().ToString() + ".jpg";
                return TakePictures(filePath, tmpFileName);
            }
            catch (Exception ex)
            {
                return "截图失败:" + ex.Message;
            }
        }

        public bool GetIsHaveVideo()
        {
            return true;
        }

        public bool GetIsHavePic()
        {
            return true;
        }
        private int actionType = 0;

        public bool PtzControl(string deviceID, string channelID, EnumCCTVControlType ecccTVCT, out string result)
        {
            try
            {
                int uSpeed = Convert.ToUInt16(int.Parse(this.speed) * PSZspeed);
                string xmlaction = string.Empty;
                int stResult = -1;
                bool isSpeed = false;
                switch (ecccTVCT)
                {
                    case EnumCCTVControlType.TILT_UP:
                        //actionType = 17;
                        xmlaction = "up";
                        isSpeed = true;
                        break;
                    case EnumCCTVControlType.TILT_DOWN:
                        //actionType = 18;
                        xmlaction = "down";
                        isSpeed = true;
                        break;
                    case EnumCCTVControlType.PAN_LEFT:
                        xmlaction = "left";
                        isSpeed = true;
                        //actionType = 21;
                        break;
                    case EnumCCTVControlType.PAN_RIGHT:
                        xmlaction = "right";
                        isSpeed = true;
                        //actionType = 22;
                        break;
                    case EnumCCTVControlType.Pan_NW:
                        xmlaction = "left_up";
                        isSpeed = true;
                        //actionType = 19;
                        break;
                    case EnumCCTVControlType.Pan_NE:
                        xmlaction = "right_up";
                        isSpeed = true;
                        //actionType = 20;
                        break;
                    case EnumCCTVControlType.Pan_SW:
                        xmlaction = "left_down";
                        isSpeed = true;
                        //actionType = 23;
                        break;
                    case EnumCCTVControlType.Pan_SE:
                        xmlaction = "right_down";
                        isSpeed = true;
                        //actionType = 24;
                        break;
                    case EnumCCTVControlType.ZOOM_IN:
                        xmlaction = "zoomtele";
                        //actionType = 11;
                        break;
                    case EnumCCTVControlType.ZOOM_OUT:
                        xmlaction = "zoomwide";
                        //actionType = 9;
                        break;
                    case EnumCCTVControlType.FOCUS_FAR:
                        xmlaction = "FocusFarAutoOff";
                        //actionType = 14;
                        break;
                    case EnumCCTVControlType.FOCUS_NEAR:
                        xmlaction = "FocusNearAutoOff";
                        //actionType = 13;
                        break;
                    case EnumCCTVControlType.IRIS_OPEN:
                        xmlaction = "IrisOpenAutoOff";
                        //actionType = 15;
                        break;
                    case EnumCCTVControlType.IRIS_CLOSE:
                        xmlaction = "IrisCloseAutoOff";
                        //actionType = 16;
                        break;
                    case EnumCCTVControlType.STOP:
                        xmlaction = "stop";
                        //actionType = 30;
                        break;
                    default:
                        xmlaction = "stop";
                        //actionType = 30;
                        break;

                }
                string xmlBody = "<xml>";
                xmlBody = xmlBody + "<cmd>" + xmlaction + "</cmd>";
                if (isSpeed)
                {
                    xmlBody = xmlBody + "<panspeed>" + uSpeed.ToString() + "</panspeed>";
                    xmlBody = xmlBody + "<tiltspeed>" + uSpeed.ToString() + "</tiltspeed>";
                }
                xmlBody = xmlBody + "</xml>";

                stResult = NETSDKDLL.IP_NET_DVR_PTZControlEx(m_userID, xmlBody);
                if (stResult < 0)
                {
                    result = "控制失败--stResult:" + stResult + ",infoChannel.MonitorName:" + Info.MonitorName + ",actionType:" + actionType;
                    int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                    //ILog.WriteErrorLog("之后错误ID：" + ID);
                    return false;
                }
                result = "控制成功";
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsPTZControl
        {
            get
            {
                return true;
            }

        }
        public bool RequestControl()
        {
            return false;
        }

        public bool StopControl()
        {
            return false;
        }

        public bool ReleaseResources(string stopClass)
        {
            try
            {
                StopRealPlayVideo(this.m_realPlay);
                //ClearupVideo();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        public bool VideoLogin()
        {
            return true;
        }
        public bool VideoLogOut()
        {
            LogoutVideo(m_userID);
            System.Threading.Thread.Sleep(200);
            return true;
        }
        public bool SetPreset(int presetId, out string result)
        {
            if (NETSDKDLL.IP_NET_DVR_PTZPreset(m_userID, 8, presetId) > 0)
            {
                result = "设置预置位成功";
                return true;

            }
            else
            {
                int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                result = "设置预置位失败" + ID + GetError(ID);
                return false;
            }
        }

        public bool GetPreset(int presetId, out string result)
        {
            if (NETSDKDLL.IP_NET_DVR_PTZPreset(m_userID, 39, presetId) > 0)
            {
                result = "调用预置位成功";
                return true;
            }
            else
            {
                int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                result = "调用预置位失败" + ID + GetError(ID);
                return false;
            }
        }

        public void SetControlSpeed(string controlSpeed)
        {
            this.speed = controlSpeed;
        }

        public ChannelInfo getChannel()
        {
            return Info;
        }

        #endregion

        public bool PTZPreset(EnumCCTVControlType ecccTVCT, int dwPresetIndex, out string result)
        {
            try
            {

                if (ecccTVCT == EnumCCTVControlType.SET_PRESET)
                {
                    if (NETSDKDLL.IP_NET_DVR_PTZPreset(
                         m_realPlay, 8, dwPresetIndex) > 0)
                    {
                        result = "设置预置位成功";
                    }
                    else
                    {
                        int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                        result = "设置预置位失败" + ID + GetError(ID);
                    }

                }
                else if (ecccTVCT == EnumCCTVControlType.GO_PRESET)
                {
                    if (NETSDKDLL.IP_NET_DVR_PTZPreset(
                         m_realPlay, 39, dwPresetIndex) > 0)
                    {

                        result = "调用预置位成功";
                    }
                    else
                    {
                        int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                        result = "调用预置位失败" + ID + GetError(ID);
                    }

                }
                else
                {
                    result = "";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int StopRealPlayVideo(int lRealHandle)
        {
            try
            {
                int nRet = NETSDKDLL.IP_NET_DVR_StopRealPlay(lRealHandle);
                PLAYERDLL.IP_TPS_DeleteStream(Info.nPlayPort);
                m_realPlay = -1;
                m_bSetParam = -1;
                return nRet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #region 退出视频
        //*****************************************************************************************
        /// <summary>
        /// 退出视频
        /// </summary>
        /// <param name="lUserID">用户ID</param>
        /// <returns>是否成功</returns>
        //*****************************************************************************************
        public int LogoutVideo(int lUserID)
        {
            try
            {
                int logOut = -1;
                if (lUserID != -1)
                {
                    logOut = NETSDKDLL.IP_NET_DVR_Logout(lUserID);
                }
                return logOut;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 清除视频
        //*****************************************************************************************
        /// <summary>
        /// 清除视频
        /// </summary>
        /// <returns>是否成功</returns>
        //*****************************************************************************************
        public int ClearupVideo()
        {
            try
            {

                NETSDKDLL.IP_NET_DVR_Cleanup();
                PLAYERDLL.IP_TPS_ReleaseAll();
                return 1;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                return -1;
            }
        }
        #endregion
        public bool StopVideo(string chlID, int userID)
        {
            try
            {
                if (StopRealPlayVideo(this.m_realPlay) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool InitAndLogin()
        {
            try
            {
                if (InitVideo() == 0)
                {
                    //if (m_userID != 0)
                    //{
                    //    NETSDKDLL.IP_NET_DVR_Cleanup();
                    //    NETSDKDLL.IP_NET_DVR_Logout(m_userID);
                    //    System.Threading.Thread.Sleep(200);
                    //    haveLogin = false;
                    //}

                    NETSDKDLL.IP_NET_DVR_SetStatusEventCallBack(fStatusCallBack, IntPtr.Zero);

                    m_userID = LoginVideo(this.Info.RemoteIP.Trim(),
                        (short)(Convert.ToInt32(this.Info.PtzPort)),
                        this.Info.RemoteUser.Trim(),
                        this.Info.RemotePwd.Trim(),
                        ref deviceInfo);//
                    if (m_userID == -1)
                    {
                        int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                        //ILog.WriteEventLog("登录失败，错误码：" + ID + GetError(ID));
                        return false;
                    }
                    return true;
                }
                else
                {
                    int ID = NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                    MessageBox.Show("初始化失败:" + ID + GetError(ID));
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


        private static bool bHaveInitVideo = false;

        public int InitVideo()
        {
            try
            {
                if (!bHaveInitVideo)
                {
                    bHaveInitVideo = true;
                    return NETSDKDLL.IP_NET_DVR_Init();
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int LoginVideo(string sDVRIP, short wDVRPort, string sUserName, string sPassword, ref IP_NET_DVR_DEVICEINFO lpDeviceInfo)
        {
            try
            {
                int logIn = -1;
                if (sDVRIP != "" && sUserName != "" && sPassword != "")
                {
                    logIn = NETSDKDLL.IP_NET_DVR_Login(sDVRIP, wDVRPort, sUserName, sPassword, ref lpDeviceInfo);
                }
                return logIn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public string GetError(int ErrorId)
        {
            return "";
        }


        public int RealPlayVideo()
        {
            try
            {
                if (m_realPlay == 0)
                {
                    NETSDKDLL.IP_NET_DVR_StopRealPlay(m_realPlay);
                    System.Threading.Thread.Sleep(200);
                }
                IP_NET_DVR_CLIENTINFO clientInfo = new IP_NET_DVR_CLIENTINFO();
                USRE_VIDEOINFO puser = new USRE_VIDEOINFO();
                clientInfo.lChannel = int.Parse(Info.ChlID);
                puser.pUserData = this.VideoPanel.Handle;
                puser.nVideoPort = Info.RemotePort;
                puser.bIsTcp = 1;
                string StrIP;
                if (this.Info.nDeviceType == 2)
                {
                    puser.nVideoChannle = (Info.nDeviceType << 16) | (int.Parse(Info.RemoteChannle) | (int.Parse(Info.ChlID) << 8));
                    StrIP = "tps://" + this.Info.RemoteIP;
                }
                else
                {
                    puser.nVideoChannle = 1;
                    StrIP = this.Info.RemoteIP;
                }
                m_realPlay = NETSDKDLL.IP_NET_DVR_RealPlayEx(m_userID, StrIP, Info.RemoteUser, Info.RemotePwd, fReadlCallBack, ref puser, 0);//"tps://" + 
                Info.nRealDataHandle = m_realPlay;
                return m_realPlay;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public int OnRealDataCallBack(int lRealHandle, int dwDataType, IntPtr pBuffer, int dwBufSize, ref FRAME_EXTDATA pExtData)
        {

            IntPtr pWnd = pExtData.pUserData;
            if (pWnd == this.VideoPanel.Handle)
            {
                if (dwDataType == 0)
                {
                    return PLAYERDLL.IP_TPS_InputVideoData(Info.nPlayPort, pBuffer, dwBufSize, pExtData.bIsKey, (int)pExtData.timestamp);
                }
                else if (dwDataType == 1)
                {
                    return PLAYERDLL.IP_TPS_InputAudioData(Info.nPlayPort, pBuffer, dwBufSize, (int)pExtData.timestamp);
                }
                else if (dwDataType == 2 && m_bSetParam == -1)
                {
                    STREAM_AV_PARAM avParam = new STREAM_AV_PARAM();
                    avParam = (STREAM_AV_PARAM)Marshal.PtrToStructure(pBuffer, avParam.GetType());
                    int size = Marshal.SizeOf(typeof(VIDEO_PARAM));
                    IntPtr pVideoParam = Marshal.AllocHGlobal(10240);
                    Marshal.StructureToPtr(avParam.videoParam, pVideoParam, false);
                    int nPortValue = Info.nPlayPort;
                    PLAYERDLL.IP_TPS_OpenStream(nPortValue, pVideoParam, size, 0, 40);
                    if (avParam.bHaveAudio != 0)
                    {
                        IntPtr pAudioParam = Marshal.AllocHGlobal(10240);
                        Marshal.StructureToPtr(avParam.audioParam, pAudioParam, false);
                        int size1 = Marshal.SizeOf(typeof(AUDIO_PARAM));
                        PLAYERDLL.IP_TPS_OpenStream(nPortValue, pAudioParam, size1, 1, 30);
                    }
                    Marshal.FreeHGlobal(pVideoParam);
                    //如果不想播放只需要解码后的数据，请将后面两行代码打开，后面第三行代码注释掉
                    //PLAYERDLL.IP_TPS_SetDecCallBack(nPortValue, fDecodecall, IntPtr.Zero);
                    //PLAYERDLL.IP_TPS_Play(nPortValue, IntPtr.Zero);
                    PLAYERDLL.IP_TPS_Play(nPortValue, this.VideoPanel.Handle);
                    m_bSetParam = 1;
                }
            }
            return 0;
        }


        private void VideoPanel_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void VideoPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void VideoPanel_MouseDown(object sender, MouseEventArgs e)
        {
            //VideoPanel.BorderStyle = BorderStyle.Fixed3D;
        }

        private void VideoPanel_MouseUp(object sender, MouseEventArgs e)
        {
            //VideoPanel.BorderStyle = BorderStyle.None;
        }
        /// <summary>
        /// 摄像头通道信息
        /// </summary>
        public ChannelInfo Info { get; set; }
        public bool InitCamera(ChannelInfo chInfo)
        {
            this.Info = chInfo;
            if (!bHaveInitTwoDll)
            {
                bHaveInitTwoDll = true;
                PLAYERDLL.IP_TPS_Init();
                NETSDKDLL.IP_NET_DVR_Init();
            }
            fDecodecall = new fDecCallBackFunction(OnDecCallBackFunction);
            fReadlCallBack = new fRealDataCallBack(OnRealDataCallBack);
            fStatusCallBack = new StatusEventCallBack(OnStatusEventCallBack);

            try
            {
                if (!InitAndLogin())
                {
                    return false;
                }
                m_realPlay = RealPlayVideo();
                if (m_realPlay > 0)
                {
                    //ILog.WriteEventLog("播放成功！");
                    return true;
                }
                else
                {
                    int ID = (int)NETSDKDLL.IP_NET_DVR_GetLastErrorCode(0);
                    MessageBox.Show("视频预览失败，错误编号：" + GetError(ID));
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                return false;
            }
        }

        public void PlayCamera()
        {
            RealPlayVideo();
        }

        public void StopCamera()
        {
            ReleaseResources("1");
            //PLAYERDLL.IP_TPS_ReleaseAll();
            //NETSDKDLL.IP_NET_DVR_Cleanup();
            bHaveInitTwoDll = false;
        }

        public void SaveFile(string filePath, string fileName)
        {
            StartKinescope(filePath, fileName);
        }

        public void StopFile()
        {
            StopKinescope();
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
