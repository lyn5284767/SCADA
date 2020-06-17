using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HBGKTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        UIControl_HBGK uiControl_HBGK1 ;
        UIControl_HBGK uiControl_HBGK2 ;

        protected override void OnClosing(CancelEventArgs e)
        {
            NETSDKDLL.IP_NET_DVR_Cleanup();
            PLAYERDLL.IP_TPS_ReleaseAll();
            base.OnClosing(e);
        }

        private void init1()
        {
            ChannelInfo ch = new ChannelInfo();
            ch.ChlID = "1";
            ch.nDeviceType = 1;//2表示NVR，1表示IPC
            ch.RemoteChannle = "1";
            ch.RemoteIP = "172.16.16.121";
            ch.RemotePort = 554;
            ch.RemoteUser = "admin";
            ch.RemotePwd = "123456";
            ch.nPlayPort = 1;
            ch.PtzPort = 8091;
            uiControl_HBGK1 = new UIControl_HBGK(ch);

        }
        private void init2()
        {
            ChannelInfo ch = new ChannelInfo();
            ch.ChlID = "1";
            ch.nDeviceType = 2;//2表示NVR，1表示IPC
            ch.RemoteChannle = "0";//第一通道
            ch.RemoteIP = "172.16.16.121";
            ch.RemotePort = 554;
            ch.RemoteUser ="admin";
            ch.RemotePwd = "123456";
            ch.nPlayPort = 2;
            ch.PtzPort = 8091;
            uiControl_HBGK2 = new UIControl_HBGK(ch);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (panel1.Controls.Count > 0)
            {
                UIControl_HBGK uiControl = (UIControl_HBGK)panel1.Controls[0];
                uiControl.ReleaseResources("1");
            }
            panel1.Controls.Clear();
            init1();
            uiControl_HBGK1.Dock = DockStyle.Fill;
            panel1.Controls.Add(uiControl_HBGK1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel2.Controls.Count > 0)
            {
                UIControl_HBGK uiControl = (UIControl_HBGK)panel2.Controls[0];
                uiControl.ReleaseResources("1");
            }
            init2();
            panel2.Controls.Clear();
            uiControl_HBGK2.Dock = DockStyle.Fill;
            panel2.Controls.Add(uiControl_HBGK2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (panel1.Controls.Count > 0)
            {
                UIControl_HBGK uiControl = (UIControl_HBGK)panel1.Controls[0];
                uiControl.ReleaseResources("1");
                panel1.Controls.Clear();
            }
           /* panel1.Controls.Clear();
            init2();
            uiControl_HBGK2.Dock = DockStyle.Fill;
            panel1.Controls.Add(uiControl_HBGK2);
            */
        }

        private void button4_Click(object sender, EventArgs e)
        {
             if (panel2.Controls.Count > 0)
            {
                UIControl_HBGK uiControl = (UIControl_HBGK)panel2.Controls[0];
                uiControl.ReleaseResources("1");
                panel2.Controls.Clear();
            }
          
            /*panel2.Controls.Clear();
            init1();
            uiControl_HBGK1.Dock = DockStyle.Fill;
            panel2.Controls.Add(uiControl_HBGK1);
             */
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
         
    }
}