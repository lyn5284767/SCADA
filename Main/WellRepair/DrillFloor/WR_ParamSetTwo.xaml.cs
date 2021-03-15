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

namespace Main.WellRepair.DrillFloor
{
    /// <summary>
    /// WR_ParamSetTwo.xaml 的交互逻辑
    /// </summary>
    public partial class WR_ParamSetTwo : UserControl
    {
        private static WR_ParamSetTwo _instance = null;
        private static readonly object syncRoot = new object();

        public static WR_ParamSetTwo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new WR_ParamSetTwo();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        public WR_ParamSetTwo()
        {
            InitializeComponent();
            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
            this.Loaded += DRParamTwo_Loaded;
            VariableBinding();
        }

        System.Timers.Timer pageChange;
        int sendCount = 0;
        private void DRParamTwo_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 33, 12, 0, 0, 0, 1, 0, 0, 39 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            sendCount = 0;
            GlobalData.Instance.DRNowPage = "paramTwo";
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
            if (GlobalData.Instance.da["drPageNum"].Value.Byte == 39 || sendCount > 5 || GlobalData.Instance.DRNowPage != "paramTwo")
            {
                pageChange.Stop();
            }
            else
            {
                byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 39 };
                GlobalData.Instance.da.SendBytes(data);
            }
        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    int groupNO = GlobalData.Instance.da["drGroup"].Value.Byte;
                    if (groupNO == 1)
                    {
                        this.twtL71.ShowText = GlobalData.Instance.da["drDrillLength"].ToString();// 钻杆长度
                        this.twtL72.ShowText = GlobalData.Instance.da["WR_BottomToWell"].ToString(); // 底座距井口距离
                        this.twtL73.ShowText = GlobalData.Instance.da["WR_BottomToFirst"].ToString(); // 底座距第一根距离
                        this.twtL74.ShowText = GlobalData.Instance.da["WR_RootingA"].ToString(); // 立根区参数A XL1
                        this.twtL75.ShowText = GlobalData.Instance.da["WR_RootingB"].ToString(); // 立根区参数B XL2
                        this.twtL76.ShowText = GlobalData.Instance.da["WR_RootingC"].ToString(); // 立根区参数C YL1
                        this.twtL77.ShowText = GlobalData.Instance.da["WR_CarLeftFirstDeviation"].ToString(); // 小车左第一根偏移
                        this.twtL78.ShowText = GlobalData.Instance.da["WR_CarLeftFirstSpace"].ToString(); // 小车左第一根间距
                        this.twtL79.ShowText = GlobalData.Instance.da["WR_CarRightFirstDeviation"].ToString(); // 小车右第一根偏移
                        this.twtL80.ShowText = GlobalData.Instance.da["WR_CarRightFirstSpace"].ToString(); // 小车右第一根间距
                        this.twtL81.ShowText = GlobalData.Instance.da["WR_ArmFirstDeviation"].ToString(); // 手臂第一根偏移
                        this.twtL82.ShowText = GlobalData.Instance.da["WR_ArmFirstSpace"].ToString(); // 手臂第一根间距
                        //this.twtL83.ShowText = GlobalData.Instance.da["drHandWithDrillTwoMaxSpeed"].ToString(); // 起钻小车偏移
                        //this.twtL84.ShowText = GlobalData.Instance.da["drRotateMoveReduce"].ToString(); // 下钻小车偏移



                        this.twtR51.ShowText = GlobalData.Instance.da["WR_CarMaxSpeed"].ToString(); // 回转空载最大速度
                        this.twtR52.ShowText = GlobalData.Instance.da["WR_WR_CarMinSpeed"].ToString(); // 回转钻杆最大速度
                        this.twtR53.ShowText = GlobalData.Instance.da["WR_ArmMaxSpeed"].ToString(); // 回转钻铤最大速度
                        this.twtR54.ShowText = GlobalData.Instance.da["WR_ArmMinSpeed"].ToString(); // 回转空载最大速度
                        this.twtR55.ShowText = GlobalData.Instance.da["WR_RotateMaxSpeed"].ToString(); // 回转钻杆最大速度
                        this.twtR56.ShowText = GlobalData.Instance.da["WR_RotateMinSpeed"].ToString(); // 回转钻铤最大速度
                    }

                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            //this.twtL28.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDeviceYear"], Mode = BindingMode.OneWay});
            //this.twtL29.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drDeviceCarNO"], Mode = BindingMode.OneWay });
            //this.twtL1.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drDeviceModelTwo"], Mode = BindingMode.OneWay, UpdateSourceTrigger = UpdateSourceTrigger.Explicit});
            //this.twtL2.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDriverType"], Mode = BindingMode.OneWay });
        }

        private void Twt_CtrGetFocusEvent(int crtTag)
        {
        }

        private byte[] bConfigParameter = new byte[3];
        /// <summary>
        /// 参数设置
        /// </summary>
        private void ParamTwoSet(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SendListToByte(new List<byte>() { 80, 33, 23, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2], 0 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
