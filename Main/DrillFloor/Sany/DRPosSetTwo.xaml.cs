﻿using COM.Common;
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
    /// DRPosSetTwo.xaml 的交互逻辑
    /// </summary>
    public partial class DRPosSetTwo : UserControl
    {
        private static DRPosSetTwo _instance = null;
        private static readonly object syncRoot = new object();

        public static DRPosSetTwo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DRPosSetTwo();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Timers.Timer pageChange;
        int sendCount = 0;
        System.Threading.Timer timer;
        public DRPosSetTwo()
        {
            InitializeComponent();
            this.Loaded += DRPosSetTwo_Loaded; ;
            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
            this.tbGripCurrent.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["gridSample"], Mode = BindingMode.OneWay });

        }

        private void DRPosSetTwo_Loaded(object sender, RoutedEventArgs e)
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
                    if (feedback == 65) this.twt65.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 66) this.twt66.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 70) this.twt70.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 71) this.twt71.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 72) this.twt72.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 74) this.twt74.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 76) this.twt76.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 77) this.twt77.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 78) this.twt78.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 79) this.twt79.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();

                    else if (feedback == 97) this.twt97.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 98) this.twt98.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                    else if (feedback == 99) this.twt99.SetControlShow = GlobalData.Instance.da["drCarlibrationFeedback"].ToString();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}