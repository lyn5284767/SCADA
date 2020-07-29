using COM.Common;
using ControlLibrary;
using DatabaseLib;
using HandyControl.Controls;
using LiveCharts;
using LiveCharts.Wpf;
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
using Separator = LiveCharts.Wpf.Separator;

namespace Main.HydraulicStation
{
    /// <summary>
    /// HSDeviceStatus.xaml 的交互逻辑
    /// </summary>
    public partial class HSDeviceStatus : UserControl
    {
        private static HSDeviceStatus _instance = null;
        private static readonly object syncRoot = new object();

        public static HSDeviceStatus Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new HSDeviceStatus();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Windows.Forms.Timer timerChart = new System.Windows.Forms.Timer();
        //System.Threading.Timer timerChart;
        //public SeriesCollection SeriesCollection { get; set; }
        //public List<string> Labels { get; set; }
        //private double _trend;
        //private double[] temp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public HSDeviceStatus()
        {
            InitializeComponent();
            VariableBinding();
            timerChart.Interval = 1000;
            timerChart.Tick += TimerChart_Tick;
            timerChart.Start();
            //实例化一条折线图
            //LineSeries mylineseries = new LineSeries();
            ////设置折线的标题
            //mylineseries.Title = "主泵压力曲线";
            //mylineseries.FontFamily = new FontFamily("微软雅黑");
            //mylineseries.Foreground = new SolidColorBrush(Colors.Black);
            ////折线图直线形式
            //mylineseries.LineSmoothness = 0;
            ////折线图的无点样式
            //mylineseries.PointGeometry = null;
            ////添加横坐标
            //Labels = new List<string> { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
            ////添加折线图的数据
            //mylineseries.Values = new ChartValues<double>(temp);
            //SeriesCollection = new SeriesCollection { };
            //SeriesCollection.Add(mylineseries);
            //_trend = 8;
            //DataContext = this;
            //timerChart = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 1000);
        }

        private void TimerChart_Tick(object sender, EventArgs e)
        {
            // 电机实时电流
            this.chart.AddPoints(GlobalData.Instance.da["MPressAI"].Value.Int16/10.0);
        }
        /// <summary>
        /// 画图时钟
        /// </summary>
        /// <param name="obj"></param>
        //private void Timer_Elapsed(object obj)
        //{
        //    try
        //    {
        //        Dispatcher.BeginInvoke(new Action(() =>
        //        {
        //            var r = new Random();
        //            _trend = r.Next(0, 100);
        //            Labels.Add(DateTime.Now.ToString("HH:mm:ss"));
        //            Labels.RemoveAt(0);
        //            //更新纵坐标数据
        //            SeriesCollection[0].Values.Add(_trend);
        //            SeriesCollection[0].Values.RemoveAt(0);
        //        }));
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
        //    }
        //}

        private void VariableBinding()
        {
            try
            {
                this.smMainPumpUnLoad.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smKavaUnLoad.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMove.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSpareOil.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                
                this.tbMainOneTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["M1RunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbMainTwoTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["M2RunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbConstantVoltage.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["CRunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbDissipateHeat.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["DRunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                // 1#主泵斜盘
                this.cpbMainOnePumpSwash.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["M1ValuePWMR"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbMainOnePumpSwash.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["M1ValuePWMR"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbMainOnePumpSwashMultiBind = new MultiBinding();
                cpbMainOnePumpSwashMultiBind.Converter = new ColorCoverter();
                cpbMainOnePumpSwashMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["M1ValuePWMR"], Mode = BindingMode.OneWay });
                cpbMainOnePumpSwashMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbMainOnePumpSwash, Mode = BindingMode.OneWay });
                cpbMainOnePumpSwashMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbMainOnePumpSwash, Mode = BindingMode.OneWay });
                cpbMainOnePumpSwashMultiBind.NotifyOnSourceUpdated = true;
                this.cpbMainOnePumpSwash.SetBinding(CircleProgressBar.ForegroundProperty, cpbMainOnePumpSwashMultiBind);
                // 2#主泵斜盘
                this.cpbMainTwoPumpSwash.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["M2ValuePWMR"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbMainTwoPumpSwash.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["M2ValuePWMR"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbMainTwoPumpSwashMultiBind = new MultiBinding();
                cpbMainTwoPumpSwashMultiBind.Converter = new ColorCoverter();
                cpbMainTwoPumpSwashMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["M2ValuePWMR"], Mode = BindingMode.OneWay });
                cpbMainTwoPumpSwashMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbMainTwoPumpSwash, Mode = BindingMode.OneWay });
                cpbMainTwoPumpSwashMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbMainTwoPumpSwash, Mode = BindingMode.OneWay });
                cpbMainTwoPumpSwashMultiBind.NotifyOnSourceUpdated = true;
                this.cpbMainTwoPumpSwash.SetBinding(CircleProgressBar.ForegroundProperty, cpbMainTwoPumpSwashMultiBind);
                // 铁钻工
                this.cpbIron.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY1APWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbIron.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY1APWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbIronMultiBind = new MultiBinding();
                cpbIronMultiBind.Converter = new ColorCoverter();
                cpbIronMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["DY1APWM"], Mode = BindingMode.OneWay });
                cpbIronMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbIron, Mode = BindingMode.OneWay });
                cpbIronMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbIron, Mode = BindingMode.OneWay });
                cpbIronMultiBind.NotifyOnSourceUpdated = true;
                this.cpbIron.SetBinding(CircleProgressBar.ForegroundProperty, cpbIronMultiBind);
                // 大钳
                this.cpbTongs.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY1BPWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbTongs.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY1BPWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbTongsMultiBind = new MultiBinding();
                cpbTongsMultiBind.Converter = new ColorCoverter();
                cpbTongsMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["DY1BPWM"], Mode = BindingMode.OneWay });
                cpbTongsMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbTongs, Mode = BindingMode.OneWay });
                cpbTongsMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbTongs, Mode = BindingMode.OneWay });
                cpbTongsMultiBind.NotifyOnSourceUpdated = true;
                this.cpbTongs.SetBinding(CircleProgressBar.ForegroundProperty, cpbTongsMultiBind);
                // 猫头
                this.cpbCatHead.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY2APWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbCatHead.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY2APWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbCatHeadMultiBind = new MultiBinding();
                cpbCatHeadMultiBind.Converter = new ColorCoverter();
                cpbCatHeadMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["DY2APWM"], Mode = BindingMode.OneWay });
                cpbCatHeadMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbCatHead, Mode = BindingMode.OneWay });
                cpbCatHeadMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbCatHead, Mode = BindingMode.OneWay });
                cpbCatHeadMultiBind.NotifyOnSourceUpdated = true;
                this.cpbCatHead.SetBinding(CircleProgressBar.ForegroundProperty, cpbCatHeadMultiBind);
                // 缓冲臂
                this.cpbBufferArm.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY2BPWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbBufferArm.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY2BPWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbBufferArmMultiBind = new MultiBinding();
                cpbBufferArmMultiBind.Converter = new ColorCoverter();
                cpbBufferArmMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["DY2BPWM"], Mode = BindingMode.OneWay });
                cpbBufferArmMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbBufferArm, Mode = BindingMode.OneWay });
                cpbBufferArmMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbBufferArm, Mode = BindingMode.OneWay });
                cpbBufferArmMultiBind.NotifyOnSourceUpdated = true;
                this.cpbBufferArm.SetBinding(CircleProgressBar.ForegroundProperty, cpbBufferArmMultiBind);
                // 钻台面
                this.cpbDF.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY3APWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbDF.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY3APWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbDFMultiBind = new MultiBinding();
                cpbDFMultiBind.Converter = new ColorCoverter();
                cpbDFMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["DY3APWM"], Mode = BindingMode.OneWay });
                cpbDFMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbDF, Mode = BindingMode.OneWay });
                cpbDFMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbDF, Mode = BindingMode.OneWay });
                cpbDFMultiBind.NotifyOnSourceUpdated = true;
                this.cpbDF.SetBinding(CircleProgressBar.ForegroundProperty, cpbDFMultiBind);
                // 备用
                this.cpbSpare.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY3BPWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbSpare.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DY3BPWM"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                MultiBinding cpbSpareMultiBind = new MultiBinding();
                cpbSpareMultiBind.Converter = new ColorCoverter();
                cpbSpareMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["DY3BPWM"], Mode = BindingMode.OneWay });
                cpbSpareMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbSpare, Mode = BindingMode.OneWay });
                cpbSpareMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbSpare, Mode = BindingMode.OneWay });
                cpbSpareMultiBind.NotifyOnSourceUpdated = true;
                this.cpbSpare.SetBinding(CircleProgressBar.ForegroundProperty, cpbDFMultiBind);
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
