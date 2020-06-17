using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;

namespace ChromeEchart
{
    /// <summary>
    /// _2dLineChart.xaml 的交互逻辑
    /// </summary>
    public partial class _2dLineChart : UserControl
    {
        private ChromiumWebBrowser _browser1;
        private ChromiumWebBrowser _browser2;

        public _2dLineChart()
        {
            InitializeComponent();
            _browser1 = new ChromiumWebBrowser();
            _browser2 = new ChromiumWebBrowser();
            this.chart1.Children.Add(_browser1);
            this.chart2.Children.Add(_browser2);
            _browser1.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            _browser2.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
        }

        private void OnIsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_browser1.IsBrowserInitialized)
            {
                _browser1.Load(Directory.GetCurrentDirectory() + "/eCharts/ElectricCurrentChart.html");
            }

            if (_browser2.IsBrowserInitialized)
            {
                _browser2.Load(Directory.GetCurrentDirectory() + "/eCharts/RealTimeSpeedChart.html");
            }
        }

        public void AddElecCurrentPoints(float x,float y,float z)
        {
            string strPara = string.Format("addData({0},{1},{2})", x, y, z);
            if (_browser1.CanExecuteJavascriptInMainFrame)
            {
                _browser1.ExecuteScriptAsync(strPara);
            }
          
        }

        public void AddSpeedPoints(float x, float y, float z)
        {
            string strPara = string.Format("addData({0},{1},{2})", x, y, z);
            if (_browser2.CanExecuteJavascriptInMainFrame)
            {
                _browser2.ExecuteScriptAsync(strPara);
            }
           
        }
    }
}
