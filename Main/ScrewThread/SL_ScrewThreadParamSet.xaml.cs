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

namespace Main.ScrewThread
{
    /// <summary>
    /// SL_ScrewThreadParamSet.xaml 的交互逻辑
    /// </summary>
    public partial class SL_ScrewThreadParamSet : UserControl
    {
        private static SL_ScrewThreadParamSet _instance = null;
        private static readonly object syncRoot = new object();

        public static SL_ScrewThreadParamSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SL_ScrewThreadParamSet();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        public SL_ScrewThreadParamSet()
        {
            InitializeComponent();
            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
            this.twt1.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt2.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt3.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt4.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt5.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt6.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt7.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt8.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt9.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt10.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt11.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt12.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt13.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt14.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt15.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt16.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt17.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
            this.twt18.SFSendProtocolEvent += ScrewThreadSendProtocolEvent;
        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    int paramNO = GlobalData.Instance.da["ScrewThreadBackVal"].Value.Byte;
                    if (paramNO == 1) this.twt1.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 2) this.twt2.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 3) this.twt3.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 4) this.twt4.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//

                    if (paramNO == 5) this.twt5.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 6) this.twt6.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 7) this.twt7.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 8) this.twt8.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 9) this.twt9.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 10) this.twt10.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//

                    if (paramNO == 11) this.twt11.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 12) this.twt12.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 13) this.twt13.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 14) this.twt14.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 15) this.twt15.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 16) this.twt16.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//

                    if (paramNO == 17) this.twt17.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                    if (paramNO == 18) this.twt18.SetControlShow = GlobalData.Instance.da["Screw_SL_ParamValue"].Value.Int32;//
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void ScrewThreadSendProtocolEvent(byte[] SetParam)
        {
            string strText = this.tbScrewThreadSet.Text;
            if (strText.Length == 0) strText = "0";
            short i16Text = Convert.ToInt16(strText);
            ropeSetValue = BitConverter.GetBytes(i16Text);
            if (ropeSetValue != null && ropeSetValue.Length == 2)
            {
                SetParam[4] = ropeSetValue[0];
                SetParam[5] = ropeSetValue[1];
                GlobalData.Instance.da.SendBytes(SetParam);
                this.tbScrewThreadSet.Text = "0";
            }
            else
            {
                GlobalData.Instance.da.SendBytes(SetParam);
                this.tbScrewThreadSet.Text = "0";
            }
        }
        private byte[] bConfigParameter = new byte[3];
        private void Button_tabItemParameterConfiguration_Confirm(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = new byte[10] { 80, 64, 3, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2], 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 获取键盘
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
        byte[] ropeSetValue;
        private void tbRopeSet_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string strText = this.tbScrewThreadSet.Text;
                if (strText.Length == 0) strText = "0";
                short i16Text = Convert.ToInt16(strText);
                ropeSetValue = BitConverter.GetBytes(i16Text);
            }
            catch (Exception ex)
            {
                this.tbScrewThreadSet.Text = "0";
                MessageBox.Show("超出设置范围");
            }
        }
    }
}
