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

namespace Main.DrillFloor.Sany
{
    /// <summary>
    /// DRPosThree.xaml 的交互逻辑
    /// </summary>
    public partial class DRPosThree : UserControl
    {
        private static DRPosThree _instance = null;
        private static readonly object syncRoot = new object();

        public static DRPosThree Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DRPosThree();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Timers.Timer pageChange;
        int sendCount = 0;
        System.Threading.Timer timer;
        public DRPosThree()
        {
            InitializeComponent();
            this.Loaded += DRPosThree_Loaded;
            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
        }

        private void DRPosThree_Loaded(object sender, RoutedEventArgs e)
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
        /// 切换页面发送指令，多次发送以防切换失败
        /// </summary>
        private void PageChange_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            sendCount++;
            if (GlobalData.Instance.da["drPageNum"].Value.Byte == 36 || sendCount > 5 || GlobalData.Instance.DRNowPage != "paramOne")
            {
                pageChange.Stop();
            }
            else
            {
                byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 36 };
                GlobalData.Instance.da.SendBytes(data);
            }
        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    int feedback = GlobalData.Instance.da["drCalibrationNumFeedback"].Value.Byte;
                    if (feedback == 49) this.twt49.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 50) this.twt50.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 51) this.twt51.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 52) this.twt52.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 53) this.twt53.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 54) this.twt54.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 55) this.twt55.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 56) this.twt56.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 57) this.twt57.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 58) this.twt58.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 59) this.twt59.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 60) this.twt60.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();

                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
