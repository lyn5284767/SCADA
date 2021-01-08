using COM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// SFPosSetTwo.xaml 的交互逻辑
    /// </summary>
    public partial class SFPosSetTwo : UserControl
    {
        private static SFPosSetTwo _instance = null;
        private static readonly object syncRoot = new object();

        public static SFPosSetTwo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFPosSetTwo();
                        }
                    }
                }
                return _instance;
            }
        }

        byte[] gripSetValue;
        byte[] FingerSetValue;
        System.Threading.Timer timer;
        public SFPosSetTwo()
        {
            InitializeComponent();
            this.txtOpModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
            this.txtWorkModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
            this.txtRotateAngle_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });
            this.tbGrip.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23GripMotorSampleValue"] });
            this.tbLeftFinger.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23LeftFingerMotorSampleValue"] });
            this.tbRightFinger.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103N23RightFingerMotorSampleValue"] });

            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
            if (GlobalData.Instance.da.GloConfig.SysType == 1) //修井
            {
                this.twt110.Visibility = Visibility.Collapsed;
                this.twt112.Visibility = Visibility.Collapsed;
                this.twt114.Visibility = Visibility.Collapsed;
                this.twt108.Visibility = Visibility.Collapsed;
                this.twt99.Visibility = Visibility.Collapsed;
                this.twt100.Visibility = Visibility.Collapsed;
                this.twt109.Visibility = Visibility.Collapsed;
                this.twt102.Visibility = Visibility.Collapsed;
                this.twt103.Visibility = Visibility.Collapsed;
                this.twt104.Visibility = Visibility.Collapsed;
                this.twt105.Visibility = Visibility.Collapsed;
                this.twt106.Visibility = Visibility.Collapsed;
            }
            this.twt97.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt110.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt112.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt98.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt114.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt108.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt99.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt100.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt109.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt101.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt102.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt103.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt104.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt105.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt106.SFSendProtocolEvent += SFSendProtocolEvent;
            this.twt107.SFSendProtocolEvent += SFSendProtocolEvent;

            this.twt129.SFSendProtocolEvent += Finger_SFSendProtocolEvent;
            this.twt130.SFSendProtocolEvent += Finger_SFSendProtocolEvent;
            this.twt131.SFSendProtocolEvent += Finger_SFSendProtocolEvent;
            this.twt132.SFSendProtocolEvent += Finger_SFSendProtocolEvent;
        }

        private void Finger_SFSendProtocolEvent(byte[] SetParam)
        {
            if (FingerSetValue.Length == 2)
            {
                SetParam[4] = FingerSetValue[0];
                SetParam[5] = FingerSetValue[1];
                GlobalData.Instance.da.SendBytes(SetParam);
                this.tbFingerSet.Text = "0";
            }
        }

        private void SFSendProtocolEvent(byte[] SetParam)
        {
            if (gripSetValue.Length == 2)
            {
                SetParam[4] = gripSetValue[0];
                SetParam[5] = gripSetValue[1];
                GlobalData.Instance.da.SendBytes(SetParam);
                this.tbGripSet.Text = "0";
            }
        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    int paramNO = GlobalData.Instance.da["Con_Set1"].Value.Byte;
                    if (paramNO == 97) this.twt97.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//3.5寸
                    if (paramNO == 110) this.twt110.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//4寸补偿
                    if (paramNO == 112) this.twt112.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//4.5寸补偿
                    if (paramNO == 98) this.twt98.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//5寸
                    if (paramNO == 114) this.twt114.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//5.5寸补偿
                    if (paramNO == 108) this.twt108.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//5寸7/8
                    if (paramNO == 99) this.twt99.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//6寸
                    if (paramNO == 100) this.twt100.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//6.5寸
                    if (paramNO == 109) this.twt109.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//6寸5/8
                    if (paramNO == 101) this.twt101.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//7寸
                    if (paramNO == 102) this.twt102.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//7.5寸
                    if (paramNO == 103) this.twt103.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//8寸
                    if (paramNO == 104) this.twt104.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//9寸
                    if (paramNO == 105) this.twt105.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//10寸
                    if (paramNO == 106) this.twt106.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//11寸
                    if (paramNO == 107) this.twt107.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//最大寸
                    if (paramNO == 129) this.twt129.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左手指最小值
                    if (paramNO == 130) this.twt130.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左手指最大值
                    if (paramNO == 131) this.twt131.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右手指最小值
                    if (paramNO == 132) this.twt132.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右手指最大值
                    
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void tb_ParameterConfig_Focus(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                GlobalData.Instance.GetKeyBoard();
            }
        }
        /// <summary>
        /// 抓手标定值失去焦点
        /// </summary>
        private void tbGripSet_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string strText = this.tbGripSet.Text;
                if (strText.Length == 0) strText = "0";
                short i16Text = Convert.ToInt16(strText);
                gripSetValue = BitConverter.GetBytes(i16Text);
            }
            catch (Exception ex)
            {
                this.tbGripSet.Text = "0";
                MessageBox.Show("超出设置范围");
            }
        }

        private void tbFingerSet_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string strText = this.tbFingerSet.Text;
                if (strText.Length == 0) strText = "0";
                short i16Text = Convert.ToInt16(strText);
                FingerSetValue = BitConverter.GetBytes(i16Text);
            }
            catch (Exception ex)
            {
                this.tbGripSet.Text = "0";
                MessageBox.Show("超出设置范围");
            }
        }
    }
}
