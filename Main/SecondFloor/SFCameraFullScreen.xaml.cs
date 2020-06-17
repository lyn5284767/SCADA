using COM.Common;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.SecondFloor
{
    /// <summary>
    /// SFCameraFullScreen.xaml 的交互逻辑
    /// </summary>
    public partial class SFCameraFullScreen : UserControl
    {
        private static SFCameraFullScreen _instance = null;
        private static readonly object syncRoot = new object();

        public static SFCameraFullScreen Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFCameraFullScreen();
                        }
                    }
                }
                return _instance;
            }
        }


        public delegate void CanelFullScreenHandler();

        public event CanelFullScreenHandler CanelFullScreenEvent;

        public SFCameraFullScreen()
        {
            InitializeComponent();
            //Button_CameraStart();
        }

        /// <summary>
        /// 摄像头1播放
        /// </summary>
        public void Button_CameraStart(int camId)
        {
            try
            {
                gridCamera1.Children.Clear();
                SFMain.Instance.gridCamera1.Children.Clear();
                SFMain.Instance.gridCamera2.Children.Clear();
                ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == camId).FirstOrDefault();
                CameraVideoStop1(camId);
                ChannelInfo info = GlobalData.Instance.chList.Where(w => w.ID == camId).FirstOrDefault();
                cameraOne.InitCamera(info);
                cameraOne.SetSize(600, 900);
                if (cameraOne is UIControl_HBGK1)
                {
                    gridCamera1.Children.Add(cameraOne as UIControl_HBGK1);
                    //(cameraOne as UIControl_HBGK1).SetValue(Grid.RowProperty, 0);
                    //(cameraOne as UIControl_HBGK1).SetValue(Grid.ColumnProperty, 0);
                }
                else if (cameraOne is YiTongCameraControl)
                {
                    gridCamera1.Children.Add(cameraOne as YiTongCameraControl);
                    //(cameraOne as YiTongCameraControl).SetValue(Grid.RowProperty, 0);
                    //(cameraOne as YiTongCameraControl).SetValue(Grid.ColumnProperty, 0);
                }
                viewboxCameral1.Height = 600;
                viewboxCameral1.Width = 900;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void CameraVideoStop1(int camId)
        {
            ICameraFactory cameraOne = GlobalData.Instance.cameraList.Where(w => w.Info.ID == camId).FirstOrDefault();
            cameraOne.StopCamera();
        }

        private void Button_CameraCancelFullScreen(object sender, RoutedEventArgs e)
        {
            if(CanelFullScreenEvent!=null)
            {
                CanelFullScreenEvent();
            }
        }
    }
}
