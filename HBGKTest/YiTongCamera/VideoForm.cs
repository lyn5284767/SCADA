using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using SDK_HANDLE = System.Int32;

namespace HBGKTest.YiTongCamera
{
    public struct DEV_INFO
    {
        public int nListNum;		   //position in the list
        public SDK_HANDLE lLoginID;			//login handle
        public int lID;				//device ID
        public string szDevName;		//device name
        public string szIpaddress;		//device IP
        public string szUserName;		//user name
        public string szPsw;			//pass word
        public int nPort;				//port number
        public int nTotalChannel;		//total channel
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
        string szChnnelName;			// 通道名称.
        public int nChnnID;							// 用于地图节点管理
        public int nChannelNo;							// 通道号.
        public int bUserRight;							// 用户权限(使能).
        public int PreViewChannel;						// 预览通道
        public int nStreamType;						// 码流类型
        public DEV_INFO DeviceInfo;							// 设备信息.
        public int nCombinType;						// 组合编码模式
        public int dwTreeItem;							//记录设备树中的节点句柄，可以节省查找事件
        public int nFlag;								//1为选择为录像 0 为没有被选择 2 为正在录像
        public int nWndIndex;
    };
    public partial class VideoForm : UserControl
    {
	    int m_nIndex;	//index	
	    bool m_bRecord;	//is recording or not
	    bool m_bSound;

        public SDK_HANDLE m_iPlayhandle;	//play handle
        public SDK_HANDLE m_lLogin; //login handle
	    public int m_iChannel; //play channel
        public SDK_HANDLE m_iTalkhandle;

	    public void SetWndIndex(int nIndex)
	    {
		    m_nIndex = nIndex;
	    }
	    public long ConnectRealPlay( ref DEV_INFO pDev, int nChannel)
        {
            if(m_iPlayhandle != -1)
	        {

                if (0 != NetSDK.H264_DVR_StopRealPlay(m_iPlayhandle, (uint)this.Handle))
		        {
			        //TRACE("H264_DVR_StopRealPlay fail m_iPlayhandle = %d", m_iPlayhandle);
		        }
		        if(m_bSound)
		        {
			        OnCloseSound();
		        }
	        }

	        H264_DVR_CLIENTINFO playstru = new H264_DVR_CLIENTINFO();

	        playstru.nChannel = nChannel;
	        playstru.nStream = 0;
	        playstru.nMode = 0;
	        playstru.hWnd=this.Handle;
            m_iPlayhandle = NetSDK.H264_DVR_RealPlay(pDev.lLoginID, ref playstru);	
	        if(m_iPlayhandle <= 0 )
	        {
                Int32 dwErr = NetSDK.H264_DVR_GetLastError();
                    StringBuilder sTemp = new StringBuilder("");
			        sTemp.AppendFormat("access {0} channel{1} fail, dwErr = {2}",pDev.szDevName,nChannel, dwErr);
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

        public void GetColor(out int nBright, out int nContrast, out int nSaturation, out int nHue)
        {
            if (m_iPlayhandle <= 0)
            {
                nBright = -1;
                nContrast = -1;
                nSaturation = -1;
                nHue = -1;
                return;
            }
            uint nRegionNum = 0;
            NetSDK.H264_DVR_LocalGetColor(m_iPlayhandle, nRegionNum, out nBright, out nContrast, out nSaturation, out nHue);
        }
	    public void SetColor(int nBright, int nContrast, int nSaturation, int nHue)
        {
            NetSDK.H264_DVR_LocalSetColor(m_iPlayhandle, 0, nBright, nContrast, nSaturation, nHue);
        }

	    public long GetHandle()
	    {
		    return m_iPlayhandle;
	    }
	    public bool OnOpenSound()
        {
            if (NetSDK.H264_DVR_OpenSound(m_iPlayhandle))
            {
                m_bSound = true;
                return true;
            }
            return false;
        }
        public bool OnCloseSound()
        {
            if (NetSDK.H264_DVR_CloseSound(m_iPlayhandle))
            {
                m_bSound = false;
                return true;
            }
            return false;
        }
	    public bool SaveRecord()
        {
            if ( m_iPlayhandle <= 0 )
	        {
		        return false;
	        }
	
            DateTime time = DateTime.Now;
            String cFilename = String.Format(@"{0}\\record\\{1}{2}{3}_{4}{5}{6}.h264",
                                                        "c:",
                                                        time.Year,
                                                        time.Month,
                                                        time.Day,
                                                        time.Hour,
                                                        time.Minute,
                                                        time.Second);
	        if ( m_bRecord )
	        {

                if (NetSDK.H264_DVR_StopLocalRecord(m_iPlayhandle))
		        {
			        m_bRecord = false;
			        MessageBox.Show(@"stop record OK.");
		        }
	        }
	        else
	        {
                int nTemp = 0;
                string strPr = "\\";
		        for(;;)
		        {
                    int nIndex = cFilename.IndexOfAny(strPr.ToCharArray(), nTemp);
                    if (nIndex == -1)
                    {
                        break;
                    }
                    String str = cFilename.Substring(0,nIndex+1);
                    nTemp = nIndex + 1; nTemp = nIndex + 1;
                    DirectoryInfo dir = new DirectoryInfo(str);
                    if ( !dir.Exists )
                    {
                        dir.Create();
                    }

		        }

                if (NetSDK.H264_DVR_StartLocalRecord(m_iPlayhandle, cFilename, (int)MEDIA_FILE_TYPE.MEDIA_FILE_NONE))
		        {
			        m_bRecord = true ;
			        MessageBox.Show(@"start record OK.");
		        }
		        else
		        {
			        MessageBox.Show(@"start record fail.");
		        }
	        }

	        return true;
        }

        public long GetLoginHandle()
        {
            return m_lLogin;
        }
        public void OnDisconnct()
        {
            if (m_iPlayhandle > 0)
            {
                NetSDK.H264_DVR_StopRealPlay(m_iPlayhandle, (uint)this.Handle);
                m_iPlayhandle = -1;

            }
            if (m_bSound)
            {
                OnCloseSound();
            }
            m_lLogin = -1;
        }


        public void drawOSD(int nPort, IntPtr hDc)
        {
            if (m_strInfoFrame[nPort] !="")
            {    
                //改变字体颜色
                FontFamily fontfamily = new FontFamily(@"Arial");
                Font newFont = new Font(fontfamily, 16,FontStyle.Bold);
                SolidBrush brush =  new SolidBrush(Color.Red);
       

                Graphics graphic = Graphics.FromHdc(hDc);
                graphic.DrawString("TEST", newFont,brush,10,10);            
            }
        }

        public int SetDevChnColor(ref SDK_CONFIG_VIDEOCOLOR pVideoColor)
        {
            IntPtr ptr = new IntPtr();
            Marshal.StructureToPtr(pVideoColor, ptr, true);
            return NetSDK.H264_DVR_SetDevConfig(m_lLogin, (uint)SDK_CONFIG_TYPE.E_SDK_VIDEOCOLOR, m_iChannel, ptr, (uint)Marshal.SizeOf(pVideoColor), 3000);
         
        }
        static void videoInfoFramCallback(int nPort, int nType, string pBuf,int nSize, IntPtr nUser)
        {
            //收到信息帧, 0x03 代表GPRS信息
      
            if (nType == 0x03)
            {
                VideoForm form = new VideoForm();
                Marshal.PtrToStructure(nUser, form);
                form.m_strInfoFrame[nPort] = pBuf;
            }
        }
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
	    public string[] m_strInfoFrame;


        public VideoForm()
        {
            InitializeComponent();
        }

        //private void VideoForm_Click(object sender, EventArgs e)
        //{
        //    ClientDemo clientdemo = (ClientDemo)this.Parent;
        //    clientdemo.SetActiveWnd(m_nIndex);
        //    clientdemo.comboBox1.Focus();
        //}

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // MessageBox.Show("");
        }

        //private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if ( -1 != m_nIndex)
        //    {
        //        NetSDK.H264_DVR_StopRealPlay(m_iPlayhandle,(uint)this.Handle);
        //        m_iPlayhandle = 0;
        //        //ClientDemo clientdemo = (ClientDemo)this.Parent;
        //        //clientdemo.DrawActivePage(false);

        //        foreach (TreeNode node in clientdemo.devForm.DevTree.Nodes)
        //        {
        //            if (node.Name == "Device")
        //            {
        //                foreach (TreeNode channelnode in node.Nodes)
        //                {
        //                    if (channelnode.Name == "Channel")
        //                    {
        //                        CHANNEL_INFO chInfo = (CHANNEL_INFO)channelnode.Tag;
        //                        if (chInfo.nWndIndex == m_nIndex)
        //                        {
        //                            chInfo.nWndIndex = -1;
        //                            channelnode.Tag = chInfo;
        //                            break;
        //                        }
        //                    }

        //                }
        //            }
                   
        //        }

        //    }
           
        //}

        //private void catchPictureToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if ( m_nIndex > -1 && m_iPlayhandle > 0)
        //    {
        //        String strPath;
        //        ClientDemo clientdemo = (ClientDemo)this.Parent;
        //        foreach (TreeNode node in clientdemo.devForm.DevTree.Nodes)
        //        {
        //            if (node.Name == "Device")
        //            {
        //                foreach (TreeNode channelnode in node.Nodes)
        //                {
        //                    if (channelnode.Name == "Channel")
        //                    {
        //                        CHANNEL_INFO chInfo = (CHANNEL_INFO)channelnode.Tag;
        //                        if (chInfo.nWndIndex == m_nIndex)
        //                        {
        //                            DateTime dt = System.DateTime.Now;
        //                            int y = dt.Year;
        //                            int m = dt.Month;
        //                            int d = dt.Day;
        //                            int h = dt.Hour;
        //                            int min = dt.Minute;
        //                            int s = dt.Second;
        //                            strPath = String.Format(".\\Pictures\\{0}_{1}_{2}{3}{4}{5}{6}{7}.bmp", node.Text,chInfo.nChannelNo + 1, y, m, d, h, min, s );
        //                            bool bCatch = NetSDK.H264_DVR_LocalCatchPic(m_iPlayhandle, strPath);

        //                            if ( bCatch )
        //                            {
        //                                System.Diagnostics.Process.Start(strPath);
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Catch Picture error !");
        //                            }
        //                            break;
        //                        }
        //                    }

        //                }
        //            } 
        //        } 
        //    }
        //}

        private void soundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ( m_iPlayhandle <= 0 )
            {
                return;
            }
            ToolStripMenuItem menuSound = (ToolStripMenuItem)sender;
            if ( menuSound.Checked )   
            {
                if (  NetSDK.H264_DVR_CloseSound(m_iPlayhandle) )
                {
                    menuSound.Checked = false;
                }
            }
            else
            {
                if (NetSDK.H264_DVR_OpenSound(m_iPlayhandle))
                {
                    menuSound.Checked = true;
                }
               
            }
        }

        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ( m_iPlayhandle <= 0 )
            {
                return;
            }
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            menu.Checked = !menu.Checked;
            if (menu.Checked)   
            {
                String strPath;
                DateTime dt = System.DateTime.Now;
                int y = dt.Year;
                int m = dt.Month;
                int d = dt.Day;
                int h = dt.Hour;
                int min = dt.Minute;
                int s = dt.Second;
                strPath = String.Format(".\\Download\\{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}.h264", y, m, d, h, min, s);
                if (!NetSDK.H264_DVR_StartLocalRecord(m_iPlayhandle, strPath, 0))
                {
                    menu.Checked = false;
                }
            }
            else
            {
                NetSDK.H264_DVR_StopLocalRecord(m_iPlayhandle);
            }
        }

        private void talkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_iPlayhandle <= 0)
            {
                return;
            }
            ToolStripMenuItem menuTalk = (ToolStripMenuItem)sender;
            if (menuTalk.Checked)
            {
                if (NetSDK.H264_DVR_StopVoiceCom(m_iTalkhandle))
                {
                    menuTalk.Checked = false;
                }
            }
            else
            {
                m_iTalkhandle = NetSDK.H264_DVR_StartLocalVoiceCom(m_lLogin);
                if (m_iTalkhandle > 0)
                {
                    menuTalk.Checked = true;
                }

            }
        }

        /// <summary>
        /// 初始化摄像头参数
        /// </summary>
        public void InitCamera()
        {
            try
            {
                H264_DVR_DEVICEINFO dvrdevInfo = new H264_DVR_DEVICEINFO();
                dvrdevInfo.Init();
                //SDK_HANDLE nLoginID = NetSDK.H264_DVR_Login(textBoxIP.Text.Trim(), ushort.Parse(textBoxport.Text.Trim()), textBoxUsername.Text, textBoxPassword.Text, ref dvrdevInfo, out nError, SocketStyle.TCPSOCKET);
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        //private ChannelInfo GetConfigPara()
        //{
        //    ChannelInfo info = new ChannelInfo();
        //    string configPath = System.Environment.CurrentDirectory + @"\Config.ini";
        //    if (System.IO.File.Exists(configPath))
        //    {
        //        StringBuilder sb = new StringBuilder(255);
        //    }
        //    return 
    }
}
