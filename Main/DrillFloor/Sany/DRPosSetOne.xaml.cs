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
    /// DRPosSetOne.xaml 的交互逻辑
    /// </summary>
    public partial class DRPosSetOne : UserControl
    {
        private static DRPosSetOne _instance = null;
        private static readonly object syncRoot = new object();

        public static DRPosSetOne Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DRPosSetOne();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Timers.Timer pageChange;
        int sendCount = 0;
        System.Threading.Timer timer;
        public DRPosSetOne()
        {
            InitializeComponent();
            this.Loaded += DRPosSetOne_Loaded;
            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
        }

        private void DRPosSetOne_Loaded(object sender, RoutedEventArgs e)
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
                    if (feedback == 1) this.twt1.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 2) this.twt2.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 3) this.twt3.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 4) this.twt4.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 5) this.twt5.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 6) this.twt6.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 7) this.twt7.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 8) this.twt8.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    //else if (feedback == 9) this.twtL9.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 10) this.twt10.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 11) this.twt11.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 12) this.twt12.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 13) this.twt13.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();

                    else if (feedback == 33) this.twt33.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 34) this.twt34.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 39) this.twt39.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 40) this.twt40.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 42) this.twt42.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 43) this.twt43.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 44) this.twt44.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 45) this.twt45.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 46) this.twt46.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 47) this.twt47.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();

                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
