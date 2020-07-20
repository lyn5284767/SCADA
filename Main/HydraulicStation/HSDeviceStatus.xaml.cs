using DatabaseLib;
using LiveCharts;
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

        System.Threading.Timer timerChart;
        public List<string> Labels = new List<string>();
        public SeriesCollection SeriesCollection = new SeriesCollection { };
        private double _trend;
        public HSDeviceStatus()
        {
            InitializeComponent();
            ////实例化一条折线图
            //LineSeries mylineseries = new LineSeries();
            ////设置折线的标题
            //mylineseries.Title = "Temp";
            ////折线图直线形式
            //mylineseries.LineSmoothness = 0;
            ////折线图的无点样式
            //mylineseries.PointGeometry = null;
            timerChart = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 1000);
        }
        /// <summary>
        /// 画图时钟
        /// </summary>
        /// <param name="obj"></param>
        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var r = new Random();
                    _trend = r.Next(0, 100);
                    Labels.Add(DateTime.Now.ToString("HH:mm:ss"));
                    //SeriesCollection[0].Values.Add(_trend);
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
