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
    /// DRParamThree.xaml 的交互逻辑
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
        public DRParamTwo()
        {
            InitializeComponent();
            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
            this.Loaded += DRParamFour_Loaded;
        }
        System.Threading.Timer timer;
        System.Timers.Timer pageChange;
        int sendCount = 0;
        private void DRParamFour_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 33, 12, 0, 0, 0, 1, 0, 0, 39 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            sendCount = 0;
            GlobalData.Instance.DRNowPage = "paramThree";
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
            if (GlobalData.Instance.da["drPageNum"].Value.Byte == 39 || sendCount>5 || GlobalData.Instance.DRNowPage != "paramThree")
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
                        this.twtL51.ShowText = GlobalData.Instance.da["drReachMaxElectric"].ToString();// 伸出最大电流
                        this.twtL52.ShowText = GlobalData.Instance.da["drReachMinElectric"].ToString(); // 伸出最小电流
                        this.twtL53.ShowText = GlobalData.Instance.da["drReachSpeedRaise"].ToString(); // 伸出加速度
                        this.twtL54.ShowText = GlobalData.Instance.da["drReachSpeedDow"].ToString(); // 伸出减速度
                        this.twtL55.ShowText = GlobalData.Instance.da["drReachSpeedCircle"].ToString(); // 伸出加减速周期
                        this.twtL56.ShowText = GlobalData.Instance.da["drReachSpeedReducePoint"].ToString(); // 伸出缓冲距离

                        this.twtL57.ShowText = GlobalData.Instance.da["drRetractMaxElectric"].ToString(); // 缩回最大电流
                        this.twtL58.ShowText = GlobalData.Instance.da["drRetractMinElectric"].ToString(); // 缩回最小电流
                        this.twtL59.ShowText = GlobalData.Instance.da["drRetractSpeedRaise"].ToString(); // 缩回加速度
                        this.twtL60.ShowText = GlobalData.Instance.da["drRetractSpeedDow"].ToString(); // 缩回减速度
                        this.twtL61.ShowText = GlobalData.Instance.da["drRetractSpeedCircle"].ToString(); // 缩回加减速周期
                        this.twtL62.ShowText = GlobalData.Instance.da["drRetractReducePoint"].ToString(); // 缩回缓冲距离
                        this.twtL63.ShowText = GlobalData.Instance.da["drSideMoveTime"].ToString(); // 侧移工作时间
                        this.twtL65.ShowText = GlobalData.Instance.da["drReachLoadElectric"].ToString(); // 伸出缓冲电流
                        this.twtL66.ShowText = GlobalData.Instance.da["drRetractLoadElectric"].ToString(); // 缩回缓冲电流
                    }
                    else if (groupNO == 2)
                    {
                        this.twtR71.ShowText = GlobalData.Instance.da["drDrillLength"].ToString();// 钻杆长度
                        this.twtR72.ShowText = GlobalData.Instance.da["drFirstToWellLength"].ToString(); // 第一根距井口距离
                        this.twtR73.ShowText = GlobalData.Instance.da["drLeftOneDev"].ToString(); // 小车左第一根偏移
                        this.twtR74.ShowText = GlobalData.Instance.da["drLeftSlopeAdjust"].ToString(); // 小车左调整斜率
                        this.twtR75.ShowText = GlobalData.Instance.da["drRightOneDev"].ToString(); // 小车右第一根偏移
                        this.twtR76.ShowText = GlobalData.Instance.da["drRightSlopeAdjust"].ToString(); // 小车右调整斜率
                        this.twtR77.ShowText = GlobalData.Instance.da["drDrillUpSlope"].ToString(); // 起钻小车偏移
                        this.twtR78.ShowText = GlobalData.Instance.da["drDrillDownSlope"].ToString(); // 下钻小车偏移
                        this.twtR79.ShowText = GlobalData.Instance.da["dr3FirstFix"].ToString(); // 3寸第一根手臂修正
                        this.twtR80.ShowText = GlobalData.Instance.da["dr4FirstFix"].ToString(); // 4寸第一根手臂修正
                        this.twtR81.ShowText = GlobalData.Instance.da["dr5FirstFix"].ToString(); // 5寸第一根手臂修正
                        //this.twtR82.ShowText = GlobalData.Instance.da["dr5Num"].ToString(); // 预留
                        this.twtR83.ShowText = GlobalData.Instance.da["drArmOneDev"].ToString(); // 手臂第一根偏移
                        this.twtR84.ShowText = GlobalData.Instance.da["drArmSlopeFix"].ToString(); // 手臂调整斜率
                        this.twtR85.ShowText = GlobalData.Instance.da["drDrillDownSlope2"].ToString(); // 下钻小车偏移2
                        this.twtR86.ShowText = GlobalData.Instance.da["drDrillDownSlope3"].ToString(); // 下钻小车偏移3
                    }
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void Twt_CtrGetFocusEvent(int crtTag)
        {

        }
        private byte[] bConfigParameter = new byte[3];
        /// <summary>
        /// 参数设置
        /// </summary>
        private void ParamThreeSet(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SendListToByte(new List<byte>() { 80, 33, 23, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2], 2 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
