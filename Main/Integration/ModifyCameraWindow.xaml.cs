using COM.Common;
using DatabaseLib;
using HBGKTest;
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
                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败");
                Log.Log4Net.AddLog(ex.ToString());
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
