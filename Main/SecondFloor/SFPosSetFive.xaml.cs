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
    /// SFPosFive.xaml 的交互逻辑
    /// </summary>
    public partial class SFPosSetFive : UserControl
    {
        private static SFPosSetFive _instance = null;
        private static readonly object syncRoot = new object();

        public static SFPosSetFive Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFPosSetFive();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        byte[] rightDrillSetValue;
        public SFPosSetFive()
        {
            InitializeComponent();
            this.txtOpModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
            this.txtWorkModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
            this.txtRotateAngle_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });

            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
            this.twt149.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt150.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt151.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt152.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt165.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt166.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt167.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt168.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt161.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt162.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt163.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt164.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt173.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt174.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt175.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
            this.twt176.SFSendProtocolEvent += RightDrill_SFSendProtocolEvent;
        }

        /// <summary>
        /// 右钻铤配置协议
        /// </summary>
        /// <param name="SetParam">协议内容</param>
        private void RightDrill_SFSendProtocolEvent(byte[] SetParam)
        {
            if (rightDrillSetValue.Length == 2)
            {
                SetParam[4] = rightDrillSetValue[0];
                SetParam[5] = rightDrillSetValue[1];
                GlobalData.Instance.da.SendBytes(SetParam);
                this.tbRightDrillSet.Text = "0";
            }
        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    int paramNO = GlobalData.Instance.da["Con_Set1"].Value.Byte;
                    if (paramNO == 149) this.twt149.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右1#最小值
                    if (paramNO == 150) this.twt150.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右2#最小值
                    if (paramNO == 151) this.twt151.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右3#最小值
                    if (paramNO == 152) this.twt152.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右4#最小值
                    if (paramNO == 165) this.twt165.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右5#最小值
                    if (paramNO == 166) this.twt166.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右6#最小值
                    if (paramNO == 167) this.twt167.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右7#最小值
                    if (paramNO == 168) this.twt168.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右8#最小值

                    if (paramNO == 161) this.twt161.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左1#最大值
                    if (paramNO == 162) this.twt162.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左2#最大值
                    if (paramNO == 163) this.twt163.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左3#最大值
                    if (paramNO == 164) this.twt164.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左4#最大值
                    if (paramNO == 173) this.twt173.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左5#最大值
                    if (paramNO == 174) this.twt174.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左6#最大值
                    if (paramNO == 175) this.twt175.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左7#最大值
                    if (paramNO == 176) this.twt176.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左8#最大值

                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 启动软键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_ParameterConfig_Focus(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                GlobalData.Instance.GetKeyBoard();
            }
        }
        /// <summary>
        /// 有钻铤设置值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbLeftDrillSet_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string strText = this.tbRightDrillSet.Text;
                if (strText.Length == 0) strText = "0";
                short i16Text = Convert.ToInt16(strText);
                rightDrillSetValue = BitConverter.GetBytes(i16Text);
            }
            catch (Exception ex)
            {
                this.tbRightDrillSet.Text = "0";
                MessageBox.Show("超出设置范围");
            }
        }
    }
}
