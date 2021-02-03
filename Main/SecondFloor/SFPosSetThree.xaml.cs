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
    /// SFPosSetThree.xaml 的交互逻辑
    /// </summary>
    public partial class SFPosSetThree : UserControl
    {
        private static SFPosSetThree _instance = null;
        private static readonly object syncRoot = new object();

        public static SFPosSetThree Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFPosSetThree();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        byte[] ropeSetValue;
        byte[] monkeyRoadSetValue;
        public SFPosSetThree()
        {
            InitializeComponent();
            this.txtOpModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
            this.txtWorkModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
            this.txtRotateAngle_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });

            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
            this.twt133.SFSendProtocolEvent += Rope_SFSendProtocolEvent;
            this.twt134.SFSendProtocolEvent += Rope_SFSendProtocolEvent;
            this.twt135.SFSendProtocolEvent += Rope_SFSendProtocolEvent;
            this.twt136.SFSendProtocolEvent += Rope_SFSendProtocolEvent;

            this.twt137.SFSendProtocolEvent += MonkeyRoad_SFSendProtocolEvent;
        }
        /// <summary>
        /// 猴道设置协议
        /// </summary>
        /// <param name="SetParam">设置协议</param>
        private void MonkeyRoad_SFSendProtocolEvent(byte[] SetParam)
        {
            string strText = this.tbMonkeyRoadSet.Text;
            if (strText.Length == 0) strText = "0";
            short i16Text = Convert.ToInt16(strText);
            monkeyRoadSetValue = BitConverter.GetBytes(i16Text);
            if (monkeyRoadSetValue != null && monkeyRoadSetValue.Length == 2)
            {
                SetParam[4] = monkeyRoadSetValue[0];
                SetParam[5] = monkeyRoadSetValue[1];
                GlobalData.Instance.da.SendBytes(SetParam);
                this.tbMonkeyRoadSet.Text = "0";
            }
            else
            {
                GlobalData.Instance.da.SendBytes(SetParam);
                this.tbMonkeyRoadSet.Text = "0";
            }
        }
        /// <summary>
        /// 档绳设置协议
        /// </summary>
        /// <param name="SetParam">设置协议</param>
        private void Rope_SFSendProtocolEvent(byte[] SetParam)
        {
            string strText = this.tbRopeSet.Text;
            if (strText.Length == 0) strText = "0";
            short i16Text = Convert.ToInt16(strText);
            ropeSetValue = BitConverter.GetBytes(i16Text);
            if (ropeSetValue != null && ropeSetValue.Length == 2)
            {
                SetParam[4] = ropeSetValue[0];
                SetParam[5] = ropeSetValue[1];
                GlobalData.Instance.da.SendBytes(SetParam);
                this.tbRopeSet.Text = "0";
            }
            else
            {
                GlobalData.Instance.da.SendBytes(SetParam);
                this.tbRopeSet.Text = "0";
            }
        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    int paramNO = GlobalData.Instance.da["Con_Set1"].Value.Byte;
                    if (paramNO == 133) this.twt133.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左挡绳最小值
                    if (paramNO == 134) this.twt134.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左挡绳最大值
                    if (paramNO == 135) this.twt135.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右挡绳最小值
                    if (paramNO == 136) this.twt136.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右挡绳最大值

                    if (paramNO == 137) this.twt137.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//猴道伸出
                    if (paramNO == 138) this.twt138.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//猴道缩回
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 软键盘弹出
        /// </summary>
        private void tb_ParameterConfig_Focus(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                GlobalData.Instance.GetKeyBoard();
            }
        }
        /// <summary>
        /// 档绳设置值
        /// </summary>
        private void tbRopeSet_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string strText = this.tbRopeSet.Text;
                if (strText.Length == 0) strText = "0";
                short i16Text = Convert.ToInt16(strText);
                ropeSetValue = BitConverter.GetBytes(i16Text);
            }
            catch (Exception ex)
            {
                this.tbRopeSet.Text = "0";
                MessageBox.Show("超出设置范围");
            }
        }
        /// <summary>
        /// 猴道设置值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbMonkeyRoadSet_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string strText = this.tbMonkeyRoadSet.Text;
                if (strText.Length == 0) strText = "0";
                short i16Text = Convert.ToInt16(strText);
                monkeyRoadSetValue = BitConverter.GetBytes(i16Text);
            }
            catch (Exception ex)
            {
                this.tbMonkeyRoadSet.Text = "0";
                MessageBox.Show("超出设置范围");
            }
        }
    }
}
