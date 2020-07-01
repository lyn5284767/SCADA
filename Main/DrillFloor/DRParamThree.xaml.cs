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
    /// DRParamTwo.xaml 的交互逻辑
    /// </summary>
    public partial class DRParamThree : UserControl
    {
        private static DRParamThree _instance = null;
        private static readonly object syncRoot = new object();

        public static DRParamThree Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DRParamThree();
                        }
                    }
                }
                return _instance;
            }
        }

        //System.Threading.Timer timer;
        public DRParamThree()
        {
            InitializeComponent();
            VariableBinding();
            //timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
            this.Loaded += DRParamThree_Loaded;
        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
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
            try
            {
                this.twtL91.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["dr3WidthFix"], Mode = BindingMode.OneWay }); // 3寸宽度修正
                this.twtL92.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["dr3AndHalfWidthFix"], Mode = BindingMode.OneWay }); // 3.5寸宽度修正
                this.twtL93.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["dr4WidthFix"], Mode = BindingMode.OneWay }); // 4寸宽度修正
                this.twtL94.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["dr4AndHalfWidthFix"], Mode = BindingMode.OneWay }); // 4.5寸宽度修正
                this.twtL95.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["dr5WidthFix"], Mode = BindingMode.OneWay }); // 5寸宽度修正
                this.twtL96.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["dr5AndHalfWidthFix"], Mode = BindingMode.OneWay }); // 5.5寸宽度修正

                this.twtL1.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drRaiseEffectModel"], Mode = BindingMode.OneWay }); // 提效模式
                this.twtL2.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drGripOpenCloseModel"], Mode = BindingMode.OneWay }); // 抓手开合模式
                this.twtL3.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drOperActionAlarm"], Mode = BindingMode.OneWay }); // 运行动作报警
                this.twtL4.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drSysDelay"], Mode = BindingMode.OneWay }); // 系统延时
                this.twtL5.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drTimeFix"], Mode = BindingMode.OneWay }); // 时间修正
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        System.Timers.Timer pageChange;
        int sendCount = 0;
        private void DRParamThree_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 33, 12, 0, 0, 0, 1, 0, 0, 40 };
            GlobalData.Instance.da.SendBytes(byteToSend);
            sendCount = 0;
            GlobalData.Instance.DRNowPage = "paramFour";
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
            if (GlobalData.Instance.da["drPageNum"].Value.Byte == 40 || sendCount >5 || GlobalData.Instance.DRNowPage != "paramFour")
            {
                pageChange.Stop();
            }
            else
            {
                byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 40 };
                GlobalData.Instance.da.SendBytes(data);
            }
        }

        private byte[] bConfigParameter = new byte[3];
        private void Twt_CtrGetFocusEvent(int crtTag)
        {
        }

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
        /// <summary>
        /// 参数设置右侧
        /// </summary>
        private void ParamThreeSet2(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SendListToByte(new List<byte>() { 80, 33, 14, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2], 2 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
