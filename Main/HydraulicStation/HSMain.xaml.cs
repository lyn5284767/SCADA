using COM.Common;
using HandyControl.Controls;
using Log;
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

namespace Main.HydraulicStation
{
    /// <summary>
    /// HSMain.xaml 的交互逻辑
    /// </summary>
    public partial class HSMain : UserControl
    {
        private static HSMain _instance = null;
        private static readonly object syncRoot = new object();

        public static HSMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new HSMain();
                        }
                    }
                }
                return _instance;
            }
        }

        System.Threading.Timer timerWarning;
        private int iTimeCnt = 0;
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private bool bCommunicationCheck = false; // 是否有中断标志
        private Dictionary<string,int> tipList = new Dictionary<string, int>(); //告警列表

        public HSMain()
        {
            InitializeComponent();

            this.VariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }

        /// <summary>
        /// 报警时钟
        /// </summary>
        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Warnning();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
            #region 告警
            int key = 0;
            if (GlobalData.Instance.da["774b0"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压油高温报警，请及时降温", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压油高温报警，请及时降温", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压油高温报警，请及时降温", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压油高温报警，请及时降温");
                }
            }

            if (GlobalData.Instance.da["774b1"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压油高温预警，请及时降温", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压油高温预警，请及时降温", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压油高温预警，请及时降温", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压油高温预警，请及时降温");
                }
            }
            if (GlobalData.Instance.da["774b2"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压油温度过低，请开启加热", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压油温度过低，请开启加热", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压油温度过低，请开启加热", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压油温度过低，请开启加热");
                }
            }
            if (GlobalData.Instance.da["774b3"].Value.Boolean)
            {
                this.tipList.TryGetValue("低液位预警，请及时加注液压油", out key);
                if (key == 0)
                {
                    this.tipList.Add("低液位预警，请及时加注液压油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("低液位预警，请及时加注液压油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("低液位预警，请及时加注液压油");
                }
            }
            if (GlobalData.Instance.da["774b4"].Value.Boolean)
            {
                this.tipList.TryGetValue("低液位报警，请及时加注液压油", out key);
                if (key == 0)
                {
                    this.tipList.Add("低液位报警，请及时加注液压油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("低液位报警，请及时加注液压油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("低液位报警，请及时加注液压油");
                }
            }
            if (GlobalData.Instance.da["774b5"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压位异常降低，请检测漏油", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压位异常降低，请检测漏油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压位异常降低，请检测漏油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压位异常降低，请检测漏油");
                }
            }
            if (GlobalData.Instance.da["774b6"].Value.Boolean)
            {
                this.tipList.TryGetValue("加热效果异常，请检测加热器", out key);
                if (key == 0)
                {
                    this.tipList.Add("加热效果异常，请检测加热器", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("加热效果异常，请检测加热器", out key);
                if (key != 0)
                {
                    this.tipList.Remove("加热效果异常，请检测加热器");
                }
            }
            if (GlobalData.Instance.da["775b0"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-主泵1已连续运行500小时，请切换主泵2使用", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-主泵1已连续运行500小时，请切换主泵2使用", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-主泵1已连续运行500小时，请切换主泵2使用", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-主泵1已连续运行500小时，请切换主泵2使用");
                }
            }
            if (GlobalData.Instance.da["775b1"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-主泵2已连续运行500小时，请切换主泵1使用", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-主泵2已连续运行500小时，请切换主泵1使用", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-主泵2已连续运行500小时，请切换主泵1使用", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-主泵2已连续运行500小时，请切换主泵1使用");
                }
            }
            if (GlobalData.Instance.da["775b2"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-主电机1已连续运行600小时，请加注黄油", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-主电机1已连续运行600小时，请加注黄油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-主电机1已连续运行600小时，请加注黄油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-主电机1已连续运行600小时，请加注黄油");
                }
            }
            if (GlobalData.Instance.da["775b3"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-主电机2已连续运行600小时，请加注黄油", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-主电机2已连续运行600小时，请加注黄油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-主电机2已连续运行600小时，请加注黄油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-主电机2已连续运行600小时，请加注黄油");
                }
            }
            if (GlobalData.Instance.da["775b4"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-距上次更换滤芯已经大于2000小时，请更换滤芯", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-距上次更换滤芯已经大于2000小时，请更换滤芯", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-距上次更换滤芯已经大于2000小时，请更换滤芯", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-距上次更换滤芯已经大于2000小时，请更换滤芯");
                }
            }
            if (GlobalData.Instance.da["775b5"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-距上次更换液压油已经大于2000小时，请更换液压油", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-距上次更换液压油已经大于2000小时，请更换液压油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-距上次更换液压油已经大于2000小时，请更换液压油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-距上次更换液压油已经大于2000小时，请更换液压油");
                }
            }
            if (GlobalData.Instance.da["775b6"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-油位下降异常，请检查是否漏油", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-油位下降异常，请检查是否漏油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-油位下降异常，请检查是否漏油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-油位下降异常，请检查是否漏油");
                }
            }

            if (iTimeCnt % 10 == 0)
            {
                this.tbTips.FontSize = 24;
            }
            else
            {
                this.tbTips.FontSize = 28;
            }
            #endregion

            //操作台控制器心跳
            if (GlobalData.Instance.da["504b7"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 60)
                {
                    this.tipList.TryGetValue("液压站与操作台信号中断", out key);
                    if (key == 0)
                    {
                        this.tipList.Add("液压站与操作台信号中断", 1);
                    }
                }
                else
                {
                    this.tipList.TryGetValue("液压站与操作台信号中断", out key);
                    if (key != 0)
                    {
                        this.tipList.Remove("液压站与操作台信号中断");
                    }
                }
                if (!bCommunicationCheck && controlHeartTimes > 60)
                {
                    bCommunicationCheck = true;
                }
            }
            else
            {
                this.controlHeartTimes = 0;
            }
            this.tmpStatus = GlobalData.Instance.da["504b7"].Value.Boolean;

            if (!GlobalData.Instance.ComunciationNormal)
            {
                this.tipList.TryGetValue("网络连接失败", out key);
                if (key == 0)
                {
                    this.tipList.Add("网络连接失败", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("网络连接失败", out key);
                if (key != 0)
                {
                    this.tipList.Remove("网络连接失败");
                }
            }

            if (iTimeCnt % 10 == 0)
            {
                if (this.tipList.Count > 0)
                {
                    this.tbTips.FontSize = 20;
                    this.tbTips.Visibility = Visibility.Visible;

                    if (!this.tipList.ContainsValue(1))
                    {
                        this.tipList.Keys.ToList().ForEach(k => this.tipList[k] = 1);
                    }

                    foreach (var tkey in this.tipList.Keys.ToList())
                    {
                        if (this.tipList[tkey] == 1)
                        {
                            this.tbTips.Text = tkey;
                            this.tipList[tkey] = 2;
                            break;
                        }
                    }
                }
                else
                {
                    this.tbTips.Visibility = Visibility.Hidden;
                    this.tbTips.Text = "";
                }
            }
            else
            {
                this.tbTips.FontSize = 28;
            }

        }

        private void VariableBinding()
        {
            try
            {
                // 主泵
                this.cpbMainPump.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["MPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbMainPump.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["MPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbMainPumpMultiBind = new MultiBinding();
                cpbMainPumpMultiBind.Converter = new ColorCoverter();
                cpbMainPumpMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["MPressAI"], Mode = BindingMode.OneWay });
                cpbMainPumpMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbMainPump, Mode = BindingMode.OneWay });
                cpbMainPumpMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbMainPump, Mode = BindingMode.OneWay });
                cpbMainPumpMultiBind.NotifyOnSourceUpdated = true;
                this.cpbMainPump.SetBinding(CircleProgressBar.ForegroundProperty, cpbMainPumpMultiBind);
                // 卡瓦
                this.cpbKavaPump.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SlipPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbKavaPump.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SlipPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbKavaPumpMultiBind = new MultiBinding();
                cpbKavaPumpMultiBind.Converter = new ColorCoverter();
                cpbKavaPumpMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["SlipPressAI"], Mode = BindingMode.OneWay });
                cpbKavaPumpMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbKavaPump, Mode = BindingMode.OneWay });
                cpbKavaPumpMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbKavaPump, Mode = BindingMode.OneWay });
                cpbKavaPumpMultiBind.NotifyOnSourceUpdated = true;
                this.cpbKavaPump.SetBinding(CircleProgressBar.ForegroundProperty, cpbKavaPumpMultiBind);
                // LS
                this.cpbLSPump.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["LSPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbLSPump.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["LSPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbLSPumpMultiBind = new MultiBinding();
                cpbLSPumpMultiBind.Converter = new ColorCoverter();
                cpbLSPumpMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["LSPressAI"], Mode = BindingMode.OneWay });
                cpbLSPumpMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbLSPump, Mode = BindingMode.OneWay });
                cpbLSPumpMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbLSPump, Mode = BindingMode.OneWay });
                cpbLSPumpMultiBind.NotifyOnSourceUpdated = true;
                this.cpbLSPump.SetBinding(CircleProgressBar.ForegroundProperty, cpbLSPumpMultiBind);
                // 平移
                this.cpbMovePump.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["TPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbMovePump.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["TPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbMovePumpMultiBind = new MultiBinding();
                cpbMovePumpMultiBind.Converter = new ColorCoverter();
                cpbMovePumpMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["TPressAI"], Mode = BindingMode.OneWay });
                cpbMovePumpMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbMovePump, Mode = BindingMode.OneWay });
                cpbMovePumpMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbMovePump, Mode = BindingMode.OneWay });
                cpbMovePumpMultiBind.NotifyOnSourceUpdated = true;
                this.cpbMovePump.SetBinding(CircleProgressBar.ForegroundProperty, cpbMovePumpMultiBind);
                // 铁钻工
                this.cpbIronPump.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IRPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbIronPump.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IRPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbIronPumpMultiBind = new MultiBinding();
                cpbIronPumpMultiBind.Converter = new ColorCoverter();
                cpbIronPumpMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["IRPressAI"], Mode = BindingMode.OneWay });
                cpbIronPumpMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbIronPump, Mode = BindingMode.OneWay });
                cpbIronPumpMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbIronPump, Mode = BindingMode.OneWay });
                cpbIronPumpMultiBind.NotifyOnSourceUpdated = true;
                this.cpbIronPump.SetBinding(CircleProgressBar.ForegroundProperty, cpbIronPumpMultiBind);
                // 大钳
                this.cpbTongPump.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["TongPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbTongPump.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["TongPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbTongPumpMultiBind = new MultiBinding();
                cpbTongPumpMultiBind.Converter = new ColorCoverter();
                cpbTongPumpMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["TongPressAI"], Mode = BindingMode.OneWay });
                cpbTongPumpMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbTongPump, Mode = BindingMode.OneWay });
                cpbTongPumpMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbTongPump, Mode = BindingMode.OneWay });
                cpbTongPumpMultiBind.NotifyOnSourceUpdated = true;
                this.cpbTongPump.SetBinding(CircleProgressBar.ForegroundProperty, cpbTongPumpMultiBind);
                // 猫头
                this.cpbCatHeadPump.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["CatPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbCatHeadPump.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["CatPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbCatHeadPumpMultiBind = new MultiBinding();
                cpbCatHeadPumpMultiBind.Converter = new ColorCoverter();
                cpbCatHeadPumpMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["CatPressAI"], Mode = BindingMode.OneWay });
                cpbCatHeadPumpMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbCatHeadPump, Mode = BindingMode.OneWay });
                cpbCatHeadPumpMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbCatHeadPump, Mode = BindingMode.OneWay });
                cpbCatHeadPumpMultiBind.NotifyOnSourceUpdated = true;
                this.cpbCatHeadPump.SetBinding(CircleProgressBar.ForegroundProperty, cpbCatHeadPumpMultiBind);
                // 钻台面
                this.cpbFPPump.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["FPPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbFPPump.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["FPPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbFPPumpMultiBind = new MultiBinding();
                cpbFPPumpMultiBind.Converter = new ColorCoverter();
                cpbFPPumpMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["FPPressAI"], Mode = BindingMode.OneWay });
                cpbFPPumpMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbFPPump, Mode = BindingMode.OneWay });
                cpbFPPumpMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbFPPump, Mode = BindingMode.OneWay });
                cpbFPPumpMultiBind.NotifyOnSourceUpdated = true;
                this.cpbFPPump.SetBinding(CircleProgressBar.ForegroundProperty, cpbFPPumpMultiBind);

                // 油温
                this.wpbOil.SetBinding(WaveProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["OilTemAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.wpbOil.SetBinding(WaveProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["OilTemAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding wpbOilMultiBind = new MultiBinding();
                wpbOilMultiBind.Converter = new ColorCoverter();
                wpbOilMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["OilTemAI"], Mode = BindingMode.OneWay });
                wpbOilMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.wpbOil, Mode = BindingMode.OneWay });
                wpbOilMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.wpbOil, Mode = BindingMode.OneWay });
                wpbOilMultiBind.NotifyOnSourceUpdated = true;
                this.wpbOil.SetBinding(WaveProgressBar.WaveFillProperty, wpbOilMultiBind);
                // 液位
                this.wpbHeight.SetBinding(WaveProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["OilLevelAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.wpbHeight.SetBinding(WaveProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["OilLevelAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding wpbHeightMultiBind = new MultiBinding();
                wpbHeightMultiBind.Converter = new ColorDescCoverter();
                wpbHeightMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["OilLevelAI"], Mode = BindingMode.OneWay });
                wpbHeightMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.wpbHeight, Mode = BindingMode.OneWay });
                wpbHeightMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.wpbHeight, Mode = BindingMode.OneWay });
                wpbHeightMultiBind.NotifyOnSourceUpdated = true;
                this.wpbHeight.SetBinding(WaveProgressBar.WaveFillProperty, wpbHeightMultiBind);

                this.imgMainOne.SetBinding(Image.SourceProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b2"], Mode = BindingMode.OneWay, Converter = new PumpImgConverter() });
                this.imgMainTwo.SetBinding(Image.SourceProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b4"], Mode = BindingMode.OneWay, Converter = new PumpImgConverter() });
                this.imgConstantVoltage.SetBinding(Image.SourceProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b6"], Mode = BindingMode.OneWay, Converter = new PumpImgConverter() });
                this.imgDissipateHeat.SetBinding(Image.SourceProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b0"], Mode = BindingMode.OneWay, Converter = new PumpImgConverter() });

                this.imghot.SetBinding(Image.SourceProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b2"], Mode = BindingMode.OneWay, Converter = new HotImgConverter() });
            }
            catch (Exception ex)
            {
                Log4Net.AddLog(ex.StackTrace, InfoLevel.ERROR);
            }
        }
    }
}
