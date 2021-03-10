using COM.Common;
using DatabaseLib;
using DemoDriver;
using HBGKTest;
using HBGKTest.YiTongCamera;
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
using System.Windows.Shapes;

namespace Main.Integration
{
    /// <summary>
    /// ModifyCameraWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyCameraWindow : Window
    {
        /// <summary>
        /// 当前摄像头ID
        /// </summary>
        private int Id = 0;
        /// <summary>
        /// 最大摄像头ID
        /// </summary>
        private int MaxId = 0;
        
        public ModifyCameraWindow()
        {
            InitializeComponent();
            Id = 1;
            MaxId = GlobalData.Instance.cameraList.Count();
            InitCameraInfo(Id);
        }

        /// <summary>
        /// 初始化摄像头信息
        /// </summary>
        private void InitCameraInfo(int camId)
        {
            ICameraFactory cam = GlobalData.Instance.cameraList.Where(w => w.Info.ID == camId).FirstOrDefault();
            if (camId == 1) this.Title = "铁架工一";
            if (camId == 2) this.Title = "铁架工二";
            if (camId == 3) this.Title = "扶杆臂一";
            if (camId == 4) this.Title = "扶杆臂二";
            if (camId == 5) this.Title = "铁钻工一";
            if (camId == 6) this.Title = "铁钻工二";
            if (cam != null)
            {
                this.tbCameraIP.Text = cam.Info.RemoteIP;
                this.tbCameraPort.Text = cam.Info.RemotePort.ToString();
                this.tbCameraUser.Text = cam.Info.RemoteUser;
                this.tbCameraPwd.Text = cam.Info.RemotePwd;
                this.tbPlayPort.Text = cam.Info.nPlayPort.ToString();
                this.cbCameraType.SelectedIndex = cam.Info.CameraType;
            }
        }
        /// <summary>
        /// 获取软键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }
        /// <summary>
        /// 切换摄像头类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbCameraType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex == 0)
            {
                this.tbCameraPort.Text = "554";
                this.tbCameraUser.Text = "admin";
                this.tbCameraPwd.Text = "123456";
                //if (GlobalInfo.Instance.CameraList.Count > 0)
                //{
                //    this.tbPlayPort.Text = (GlobalInfo.Instance.CameraList.Max(m => m.Info.nPlayPort) + 1).ToString();
                //}
                //else
                //{
                //    this.tbPlayPort.Text = "1";
                //}
            }
            else if (cb.SelectedIndex == 1)
            {
                this.tbCameraPort.Text = "34567";
                this.tbCameraUser.Text = "admin";
                this.tbCameraPwd.Text = "";
                //if (GlobalInfo.Instance.CameraList.Count > 0)
                //{
                //    this.tbPlayPort.Text = (GlobalInfo.Instance.CameraList.Max(m => m.Info.nPlayPort) + 1).ToString();
                //}
                //else
                //{
                //    this.tbPlayPort.Text = "1";
                //}
            }
        }
        /// <summary>
        /// 保存摄像头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ICameraFactory cam = GlobalData.Instance.cameraList.Where(w => w.Info.ID == Id).FirstOrDefault();
                if (cam != null)
                {
                    cam.Info.RemoteIP = this.tbCameraIP.Text;
                    cam.Info.RemotePort = int.Parse(this.tbCameraPort.Text);
                    cam.Info.RemoteUser = this.tbCameraUser.Text;
                    cam.Info.RemotePwd = this.tbCameraPwd.Text;
                    cam.Info.nPlayPort = int.Parse(this.tbPlayPort.Text);
                    cam.Info.CameraType = this.cbCameraType.SelectedIndex;
                    string sql = string.Format("Update CameraInfo set RemoteIP='{0}',RemotePort='{1}',RemoteUser='{2}',RemotePwd='{3}',nPlayPort='{4}',CameraType='{5}' Where Id='{6}'",
                        cam.Info.RemoteIP, cam.Info.RemotePort, cam.Info.RemoteUser, cam.Info.RemotePwd, cam.Info.nPlayPort, cam.Info.CameraType,cam.Info.ID);
                    DataHelper.Instance.ExecuteNonQuery(sql);
                }
                InitCameraInfo();
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败");
                Log.Log4Net.AddLog(ex.ToString());
            }
        }


        /// <summary>
        /// 初始化摄像头信息
        /// </summary>
        private void InitCameraInfo()
        {
            GlobalData.Instance.cameraList.Clear();
            string sql = " Select * from CameraInfo";
            List<CameraInfo> list = DataHelper.Instance.ExecuteList<CameraInfo>(sql);
            foreach (CameraInfo cameraInfo in list)
            {
                ChannelInfo ch1 = new ChannelInfo();
                ch1.ID = cameraInfo.Id;
                ch1.ChlID = cameraInfo.ChlId.ToString();
                ch1.nDeviceType = cameraInfo.NDeviceType;
                ch1.RemoteChannle = cameraInfo.REMOTECHANNLE.ToString();
                ch1.RemoteIP = cameraInfo.REMOTEIP;
                ch1.RemotePort = cameraInfo.REMOTEPORT;
                ch1.RemoteUser = cameraInfo.REMOTEUSER;
                ch1.RemotePwd = cameraInfo.REMOTEPWD;
                ch1.nPlayPort = cameraInfo.NPLAYPORT;
                ch1.PtzPort = cameraInfo.PTZPORT;
                ch1.CameraType = cameraInfo.CAMERATYPE;
                GlobalData.Instance.chList.Add(ch1);
            }
            foreach (ChannelInfo info in GlobalData.Instance.chList)
            {
                switch (info.CameraType)
                {
                    case 0:
                        {
                            GlobalData.Instance.cameraList.Add(new UIControl_HBGK1(info));
                            break;
                        }
                    case 1:
                        {
                            GlobalData.Instance.cameraList.Add(new YiTongCameraControl(info));
                            break;
                        }

                }
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 下一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Id += 1;
            if (Id > MaxId) Id = 1;
            InitCameraInfo(Id);
        }

        private void btnPre_Click(object sender, RoutedEventArgs e)
        {
            Id -= 1;
            if (Id < 1) Id = MaxId;
            InitCameraInfo(Id);
        }
    }
}
