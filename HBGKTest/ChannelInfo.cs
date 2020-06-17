using System;

namespace HBGKTest
{
	/// <summary>
	///视频信息表单
	/// </summary>
    public class ChannelInfo
    {
        public int ID { get; set; }
        private string chlID = "";

        public int nDeviceType = 0;//0IPC,1DVS,2NVR

        public string ChlID
        {
            get { return chlID; }
            set { chlID = value; }
        }
        private string masterID = "";

        public string MasterID
        {
            get { return masterID; }
            set { masterID = value; }
        }
        private string masterName = "";

        public string MasterName
        {
            get { return masterName; }
            set { masterName = value; }
        }
        private EncoderType encoderType;

        public EncoderType EncoderType
        {
            get { return encoderType; }
            set { encoderType = value; }
        }
        private string encoderName = "";

        public string EncoderName
        {
            get { return encoderName; }
            set { encoderName = value; }
        }
        private int monitorId = 1;

        public int MonitorId
        {
            get { return monitorId; }
            set { monitorId = value; }
        }
        private string monitorName = "";

        public string MonitorName
        {
            get { return monitorName; }
            set { monitorName = value; }
        }
        private string defaultCamID = "";

        public string DefaultCamID
        {
            get { return defaultCamID; }
            set { defaultCamID = value; }
        }
        private string defaultCamName = "";

        public string DefaultCamName
        {
            get { return defaultCamName; }
            set { defaultCamName = value; }
        }
        private string remoteIP = "";

        public string RemoteIP
        {
            get { return remoteIP; }
            set { remoteIP = value; }
        }

        private int remotePort = 9998;

        public int RemotePort
        {
            get { return remotePort; }
            set { remotePort = value; }
        }
        private int iPtzPort = 8091;
        public int PtzPort
        {
            get { return iPtzPort; }
            set { iPtzPort = value; }
        }

        private string remoteUser = "";

        public string RemoteUser
        {
            get { return remoteUser; }
            set { remoteUser = value; }
        }
        private string remotePwd = "";

        public string RemotePwd
        {
            get { return remotePwd; }
            set { remotePwd = value; }
        }
        private string remoteChannle = "1";

        public string RemoteChannle
        {
            get { return remoteChannle; }
            set { remoteChannle = value; }
        }
        private string errId;

        public string ErrId
        {
            get { return errId; }
            set { errId = value; }
        }

        bool isMasterControl;

        public bool IsMasterControl
        {
            get { return isMasterControl; }
            set { isMasterControl = value; }
        }

        private int __nRealDataHandle = 1;

        public int nRealDataHandle
        {
            get { return __nRealDataHandle; }
            set { __nRealDataHandle = value; }
        }


        private int __nPlayPort = 1;

        public int nPlayPort
        {
            get { return __nPlayPort; }
            set { __nPlayPort = value; }
        }

        /// <summary>
        /// 摄像头类型 0-  ;1-一通
        /// </summary>
        public int CameraType { get; set; }
    }
}
