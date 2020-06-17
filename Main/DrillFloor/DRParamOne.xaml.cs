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

namespace Main.DrillFloor
{
    /// <summary>
    /// DRParamOne.xaml 的交互逻辑
    /// </summary>
    public partial class DRParamOne : UserControl
    {
        private static DRParamOne _instance = null;
        private static readonly object syncRoot = new object();

        public static DRParamOne Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DRParamOne();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        public DRParamOne()
        {
            InitializeComponent();
            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
            this.tbGripCurrent.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["gridSample"], Mode = BindingMode.OneWay });
            this.Loaded += DRParamOne_Loaded;
        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    int feedback = GlobalData.Instance.da["drCalibrationNumFeedback"].Value.Byte;
                    if (feedback == 1) this.twtL1.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if(feedback == 2) this.twtL2.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 3) this.twtL3.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 4) this.twtL4.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 5) this.twtL5.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 6) this.twtL6.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 7) this.twtL7.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 8) this.twtL8.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    //else if (feedback == 9) this.twtL9.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 10) this.twtL10.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 11) this.twtL11.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 12) this.twtL12.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 13) this.twtL13.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();

                    else if (feedback == 33) this.twtL33.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 34) this.twtL34.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 39) this.twtL39.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 40) this.twtL40.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 42) this.twtL42.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 43) this.twtL43.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 44) this.twtL44.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 45) this.twtL45.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 46) this.twtL46.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 47) this.twtL47.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();

                    else if (feedback == 65) this.twtR65.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 66) this.twtR66.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 70) this.twtR70.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 71) this.twtR71.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 72) this.twtR72.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 74) this.twtR74.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 76) this.twtR76.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 77) this.twtR77.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 78) this.twtR78.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 79) this.twtR79.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();

                    else if (feedback == 49) this.twtR49.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 50) this.twtR50.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 51) this.twtR51.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 52) this.twtR52.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 53) this.twtR53.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 54) this.twtR54.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 55) this.twtR55.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 56) this.twtR56.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 57) this.twtR57.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 58) this.twtR58.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 59) this.twtR59.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 60) this.twtR60.ShowTxt = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        System.Timers.Timer pageChange;
        int sendCount = 0;
        /// <summary>
        /// 打开页面发送给下位机
        /// </summary>
        private void DRParamOne_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 33, 12, 0, 0, 0, 1, 0, 0, 36 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            sendCount = 0;
            GlobalData.Instance.DRNowPage = "paramOne";
            pageChange = new System.Timers.Timer(500);
            pageChange.Elapsed += PageChange_Elapsed; ;
            pageChange.Enabled = true;
        }

        /// <summary>
        /// 切换页面发送指令
        /// </summary>
        private void PageChange_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            sendCount++;
            if (GlobalData.Instance.da["drPageNum"].Value.Byte == 36|| sendCount>5 || GlobalData.Instance.DRNowPage != "paramOne")
            {
                pageChange.Stop();
            }
            else
            {
                byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 36 };
                GlobalData.Instance.da.SendBytes(data);
            }
        }

        private void TwtL1_textBoxSetMouseDownEvent(int crtTag, string txt)
        {
            if (crtTag > 0)
            {
                if (txt == "读取")
                {
                    byte[] byteToSend = GlobalData.Instance.SendListToByte(new List<byte>() { 80, 33, 12, (byte)crtTag, 0, 0, 2 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
                else
                {
                    byte[] byteToSend = GlobalData.Instance.SendListToByte(new List<byte>() { 80, 33, 12, (byte)crtTag, 0, 0, 1 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
        }
    }
}
