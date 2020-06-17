using COM.Common;
using ControlLibrary;
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
    /// DRParamFour.xaml 的交互逻辑
    /// </summary>
    public partial class DRParamTwo : UserControl
    {
        private static DRParamTwo _instance = null;
        private static readonly object syncRoot = new object();

        public static DRParamTwo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DRParamTwo();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        public DRParamTwo()
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
            byte[] byteToSend = new byte[10] { 80, 33, 12, 0, 0, 0, 1, 0, 0, 38 };
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
            if (GlobalData.Instance.da["drPageNum"].Value.Byte == 38 || sendCount>5 || GlobalData.Instance.DRNowPage != "paramTwo")
            {
                pageChange.Stop();
            }
            else
            {
                byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 38 };
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
                        this.twtL1.ShowText = GlobalData.Instance.da["drDeviceModelTwo"].ToString();// 设备机型
                        this.twtL2.ShowText = GlobalData.Instance.da["drDriverType"].ToString(); // 驱动器型号
                        this.twtL4.ShowText = GlobalData.Instance.da["drDrillSetting"].ToString(); // 左右钻铤有误
                        this.twtL5.ShowText = GlobalData.Instance.da["drDrillNum"].ToString(); // 单边钻铤数量
                        this.twtR6.ShowText = GlobalData.Instance.da["drCarMoveDis"].ToString(); // 小车单圈行走值
                        this.twtR7.ShowText = GlobalData.Instance.da["drCarMoveReduce"].ToString(); // 小车减速比
                        this.twtR8.ShowText = GlobalData.Instance.da["drCarMaxSpeed"].ToString(); // 小车空载最大速度
                        this.twtR9.ShowText = GlobalData.Instance.da["drCarWithDrillOneMaxSpeed"].ToString(); // 小车钻杆最大速度
                        this.twtR10.ShowText = GlobalData.Instance.da["drCarWithDrillTwoMaxSpeed"].ToString(); // 小车钻铤最大速度
                        this.twtR11.ShowText = GlobalData.Instance.da["drHandMoveReduce"].ToString(); // 手臂减速比
                        this.twtR12.ShowText = GlobalData.Instance.da["drHandMaxSpeed"].ToString(); // 手臂空载最大速度
                        this.twtR13.ShowText = GlobalData.Instance.da["drHandWithDrillOneMaxSpeed"].ToString(); // 手臂钻杆最大速度
                        this.twtR14.ShowText = GlobalData.Instance.da["drHandWithDrillTwoMaxSpeed"].ToString(); // 手臂钻铤最大速度
                        this.twtR15.ShowText = GlobalData.Instance.da["drRotateMoveReduce"].ToString(); // 回转减速比
                        this.twtR16.ShowText = GlobalData.Instance.da["drRotateMaxSpeed"].ToString(); // 回转空载最大速度
                        this.twtR17.ShowText = GlobalData.Instance.da["drRotateWithDrillOneMaxSpeed"].ToString(); // 回转钻杠最大速度
                        this.twtR18.ShowText = GlobalData.Instance.da["drRotateWithDrillTwoMaxSpeed"].ToString(); // 回转钻铤最大速度
                    }
                    else if (groupNO == 2)
                    {
                        this.twtL19.ShowText = GlobalData.Instance.da["drDrillOneWidth"].ToString();// 钻杠指梁宽度
                        this.twtL20.ShowText = GlobalData.Instance.da["drDrillOneSpace"].ToString(); // 钻杠指梁壁厚
                        this.twtL21.ShowText = GlobalData.Instance.da["drDrillTwoWidth"].ToString(); // 钻铤指梁宽度
                        this.twtL22.ShowText = GlobalData.Instance.da["drDrillTwoSpace"].ToString(); // 钻铤指梁厚度
                        this.twtL23.ShowText = GlobalData.Instance.da["drDrillSettingNum"].ToString(); // 钻铤设置值
                        this.twtR24.ShowText = GlobalData.Instance.da["drArmMaxPos"].ToString(); // 手臂最大距离
                        this.twtL25.ShowText = GlobalData.Instance.da["drRowsNum"].ToString(); // 单边钻杆排数
                        this.twtL36.ShowText = GlobalData.Instance.da["dr3Num"].ToString(); // 3寸钻杠容量
                        this.twtL37.ShowText = GlobalData.Instance.da["dr3AndHalfNum"].ToString(); // 3.5寸钻杆容量
                        this.twtL39.ShowText = GlobalData.Instance.da["dr4Num"].ToString(); // 4寸钻杆容量
                        this.twtL40.ShowText = GlobalData.Instance.da["dr4AndHalfNum"].ToString(); // 4.5寸钻杆容量
                        this.twtL41.ShowText = GlobalData.Instance.da["dr5Num"].ToString(); // 5寸钻杠容量
                        this.twtL42.ShowText = GlobalData.Instance.da["dr5AndHalfNum"].ToString(); // 5.5寸钻杠容量
                        this.twtL28.ShowText = GlobalData.Instance.da["drDeviceYear"].ToString(); // 设备年份
                        this.twtL29.ShowText = GlobalData.Instance.da["drDeviceCarNO"].ToString(); // 设备机号
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
