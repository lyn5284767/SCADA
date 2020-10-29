using COM.Common;
using DatabaseLib;
using LiveCharts;
using LiveCharts.Wpf;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
    /// HSReport.xaml 的交互逻辑
    /// </summary>
    public partial class HSReport : UserControl
    {
        private static HSReport _instance = null;
        private static readonly object syncRoot = new object();
        public static HSReport Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new HSReport();
                        }
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// 系统压力值
        /// </summary>
        public SeriesCollection SystemPressSeries { get; set; }
        /// <summary>
        /// 系统压力时间
        /// </summary>
        public List<string> SystemPressLabels { get; set; }
        /// <summary>
        /// 主泵1流量
        /// </summary>
        public SeriesCollection MainFlowSeries { get; set; }
        /// <summary>
        /// 主泵1流量时间
        /// </summary>
        public List<string> MainFlowLabels { get; set; }
        /// <summary>
        /// 主泵2流量
        /// </summary>
        public SeriesCollection MainTwoFlowSeries { get; set; }
        /// <summary>
        /// 主泵2流量时间
        /// </summary>
        public List<string> MainTwoFlowLabels { get; set; }
        /// <summary>
        /// 油温
        /// </summary>
        public SeriesCollection OilTemSeries { get; set; }
        /// <summary>
        /// 油温时间
        /// </summary>
        public List<string> OilTemLabels { get; set; }
        /// <summary>
        /// 液压值
        /// </summary>
        public SeriesCollection OilLevelSeries { get; set; }
        /// <summary>
        /// 液压时间
        /// </summary>
        public List<string> OilLevelLabels { get; set; }

        public HSReport()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += HSReport_Loaded;
        }
        /// <summary>
        /// 页面加载事件
        /// </summary>
        private void HSReport_Loaded(object sender, RoutedEventArgs e)
        {
            IsExist = false;
            this.tbReportTime.Text = "报表生成时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            //BtnDay_Click(null, null);
        }

        private void VariableBinding()
        {
            this.tbMainOneTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["M1RunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbMainTwoTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["M2RunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbConstantVoltage.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["CRunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbDissipateHeat.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["DRunTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
        }
        bool IsExist = false;
        string pdfPath = string.Empty;
        /// <summary>
        /// 预览报表
        /// </summary>
        private void btn_PreviewReport(object sender, RoutedEventArgs e)
        {
            string filePath = System.Environment.CurrentDirectory + "\\HSReport";
            if (!IsExist || !File.Exists(pdfPath))
            {
                MessageBox.Show("请先生成报表");
                return;
            }
            Process.Start(pdfPath);
        }
        /// <summary>
        /// 生成报表
        /// </summary>
        private void btn_GenerateReport(object sender, RoutedEventArgs e)
        {
            string filePath = System.Environment.CurrentDirectory + "\\HSReport";
            if (!Directory.Exists(filePath + @"\image.jpg"))
            {
                Directory.CreateDirectory(filePath);
            }
            // 页面保存为图片
            FileStream fs = new FileStream(filePath + @"\image.jpg", FileMode.Create);
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)this.gdMain.ActualWidth, (int)this.gdMain.ActualHeight, 0, 0, PixelFormats.Pbgra32);
            bmp.Render(this.gdMain);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(fs);
            fs.Close();
            fs.Dispose();

            PdfDocument doc = new PdfDocument();

            doc.Pages.Add(new PdfPage());
            doc.Pages[0].Size = PageSize.QuadDemy;
            XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[0]);
            XImage img = XImage.FromFile(filePath + @"\image.jpg");
            xgr.DrawImage(img, 0, 0, img.Size.Width, img.Size.Height);
            doc.Save(filePath +"\\"+ DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf");
            doc.Close();
            IsExist = true;
            pdfPath = filePath + "\\" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";
            MessageBox.Show("报表生成成功!");
        }

        /// <summary>
        /// 查询一天内
        /// </summary>
        private void BtnDay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var bc = new BrushConverter();
                this.btnDay.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
                this.btnDay.Background = (Brush)bc.ConvertFrom("#326CF3");
                this.btnMonth.Foreground = (Brush)bc.ConvertFrom("#525252");
                this.btnMonth.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                DrawSystemPress(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
                DrawOilTem(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
                DrawOilLevel(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
                DrawMainFlow(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
                DrawMainTwoFlow(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
                DataContext = null;
                DataContext = this;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 绘制压力曲线
        /// </summary>
        private void DrawSystemPress(string Date)
        {
            LineSeries systemPressLine = new LineSeries();
            systemPressLine.LineSmoothness = 0;
            systemPressLine.PointGeometry = null;
            SystemPressLabels = new List<string>();
            systemPressLine.Values = new ChartValues<double>();
            SystemPressSeries = new SeriesCollection { };
            SystemPressSeries.Add(systemPressLine);
            string sql = string.Format("Select * from DateBaseReport Where Type = '{0}' and CreateTime>'{1}'", (int)SaveType.HS_Self_SystemPress, Date);
            List<DateBaseReport> systemPressList = DataHelper.Instance.ExecuteList<DateBaseReport>(sql);
            int sample = systemPressList.Count() / 200;
            double tmpMax = 0.0;
            double tmpMin = 0.0;
            string tmpMaxTime = string.Empty;
            string tmpMinTime = string.Empty;
            int i = 1;
            List<double> vList = new List<double>();
            if (sample != 0)
            {
                foreach (DateBaseReport dateBaseReport in systemPressList)
                {
                    double nowVal = double.Parse(dateBaseReport.Value);
                    string nowTime = dateBaseReport.CreateTime.ToString();
                    if (i % sample == 0) // 取余为零，标志一个周期结束
                    {
                        SystemPressLabels.Add(tmpMaxTime);
                        SystemPressSeries[0].Values.Add(tmpMax);
                        vList.Add(tmpMax);
                        SystemPressLabels.Add(tmpMinTime);
                        SystemPressSeries[0].Values.Add(tmpMin);
                        vList.Add(tmpMin);
                        tmpMax = 0.0;
                        tmpMin = 0.0;
                    }
                    else // 周期内取最大最小值
                    {
                        if (tmpMax <= nowVal)
                        {
                            tmpMax = nowVal;
                            tmpMaxTime = nowTime;
                        }
                        if (tmpMin >= nowVal)
                        {
                            tmpMin = nowVal;
                            tmpMinTime = nowTime;
                        }
                    }
                    i++;
                }
            }
            else
            {
                foreach (DateBaseReport dateBaseReport in systemPressList)
                {
                    double nowVal = double.Parse(dateBaseReport.Value);
                    string nowTime = dateBaseReport.CreateTime.ToString();
                    SystemPressLabels.Add(nowTime);
                    SystemPressSeries[0].Values.Add(nowVal);
                    vList.Add(tmpMax);
                }
            }
            this.lvcSystemPress.MinValue = vList.Min()-1;
            this.lvcSystemPress.MaxValue = vList.Max();
        }
        /// <summary>
        /// 绘制主泵1流量曲线
        /// </summary>
        private void DrawMainFlow(string Date)
        {
            LineSeries mainFlowLine = new LineSeries();
            mainFlowLine.LineSmoothness = 0;
            mainFlowLine.PointGeometry = null;
            MainFlowLabels = new List<string>();
            mainFlowLine.Values = new ChartValues<double>();
            MainFlowSeries = new SeriesCollection { };
            MainFlowSeries.Add(mainFlowLine);
            string sql = string.Format("Select * from DateBaseReport Where Type = '{0}' and CreateTime>'{1}'", (int)SaveType.HS_Self_MainFlow, Date);
            List<DateBaseReport> mainFlowList = DataHelper.Instance.ExecuteList<DateBaseReport>(sql);
            int sample = mainFlowList.Count() / 200;
            double tmpMax = 0.0;
            double tmpMin = 0.0;
            string tmpMaxTime = string.Empty;
            string tmpMinTime = string.Empty;
            int i = 1;
            List<double> vList = new List<double>();
            if (sample != 0)
            {
                foreach (DateBaseReport dateBaseReport in mainFlowList)
                {
                    double nowVal = double.Parse(dateBaseReport.Value);
                    string nowTime = dateBaseReport.CreateTime.ToString();
                    if (i % sample == 0) // 取余为零，标志一个周期结束
                    {
                        MainFlowLabels.Add(tmpMaxTime);
                        MainFlowSeries[0].Values.Add(tmpMax);
                        vList.Add(tmpMax);
                        MainFlowLabels.Add(tmpMinTime);
                        MainFlowSeries[0].Values.Add(tmpMin);
                        vList.Add(tmpMin);
                        tmpMax = 0.0;
                        tmpMin = 0.0;
                    }
                    else // 周期内取最大最小值
                    {
                        if (tmpMax <= nowVal)
                        {
                            tmpMax = nowVal;
                            tmpMaxTime = nowTime;
                        }
                        if (tmpMin >= nowVal)
                        {
                            tmpMin = nowVal;
                            tmpMinTime = nowTime;
                        }
                    }
                    i++;
                }
            }
            else
            {
                foreach (DateBaseReport dateBaseReport in mainFlowList)
                {
                    double nowVal = double.Parse(dateBaseReport.Value);
                    string nowTime = dateBaseReport.CreateTime.ToString();
                    MainFlowLabels.Add(nowTime);
                    MainFlowSeries[0].Values.Add(nowVal);
                    vList.Add(nowVal);
                }
            }
            this.lvcMainFlow.MinValue = vList.Min() - 1;
            this.lvcMainFlow.MaxValue = vList.Max();
        }

        /// <summary>
        /// 绘制主泵2流量曲线
        /// </summary>
        private void DrawMainTwoFlow(string Date)
        {
            LineSeries mainTwoFlowLine = new LineSeries();
            mainTwoFlowLine.LineSmoothness = 0;
            mainTwoFlowLine.PointGeometry = null;
            MainTwoFlowLabels = new List<string>();
            mainTwoFlowLine.Values = new ChartValues<double>();
            MainTwoFlowSeries = new SeriesCollection { };
            MainTwoFlowSeries.Add(mainTwoFlowLine);
            string sql = string.Format("Select * from DateBaseReport Where Type = '{0}' and CreateTime>'{1}'", (int)SaveType.HS_Self_MainTwoFlow, Date);
            List<DateBaseReport> mainFlowList = DataHelper.Instance.ExecuteList<DateBaseReport>(sql);
            int sample = mainFlowList.Count() / 200;
            double tmpMax = 0.0;
            double tmpMin = 0.0;
            string tmpMaxTime = string.Empty;
            string tmpMinTime = string.Empty;
            int i = 1;
            List<double> vList = new List<double>();
            if (sample != 0)
            {
                foreach (DateBaseReport dateBaseReport in mainFlowList)
                {
                    double nowVal = double.Parse(dateBaseReport.Value);
                    string nowTime = dateBaseReport.CreateTime.ToString();
                    if (i % sample == 0) // 取余为零，标志一个周期结束
                    {
                        MainTwoFlowLabels.Add(tmpMaxTime);
                        MainTwoFlowSeries[0].Values.Add(tmpMax);
                        vList.Add(tmpMax);
                        MainTwoFlowLabels.Add(tmpMinTime);
                        MainTwoFlowSeries[0].Values.Add(tmpMin);
                        vList.Add(tmpMin);
                        tmpMax = 0.0;
                        tmpMin = 0.0;
                    }
                    else // 周期内取最大最小值
                    {
                        if (tmpMax <= nowVal)
                        {
                            tmpMax = nowVal;
                            tmpMaxTime = nowTime;
                        }
                        if (tmpMin >= nowVal)
                        {
                            tmpMin = nowVal;
                            tmpMinTime = nowTime;
                        }
                    }
                    i++;
                }
            }
            else
            {
                foreach (DateBaseReport dateBaseReport in mainFlowList)
                {
                    double nowVal = double.Parse(dateBaseReport.Value);
                    string nowTime = dateBaseReport.CreateTime.ToString();
                    SystemPressLabels.Add(nowTime);
                    SystemPressSeries[0].Values.Add(nowVal);
                    vList.Add(nowVal);
                }
            }
            this.lvcMainTwoFlow.MinValue = vList.Min() - 1;
            this.lvcMainTwoFlow.MaxValue = vList.Max();
        }
        /// <summary>
        /// 绘制油温曲线
        /// </summary>
        private void DrawOilTem(string Date)
        {
            LineSeries oilTemLine = new LineSeries();
            oilTemLine.LineSmoothness = 0;
            oilTemLine.PointGeometry = null;
            OilTemLabels = new List<string>();
            oilTemLine.Values = new ChartValues<double>();
            OilTemSeries = new SeriesCollection { };
            OilTemSeries.Add(oilTemLine);
            string sql = string.Format("Select * from DateBaseReport Where Type = '{0}' and CreateTime>'{1}'", (int)SaveType.HS_Self_OilTmp, Date);
            List<DateBaseReport> OilTemList = DataHelper.Instance.ExecuteList<DateBaseReport>(sql);
            int sample = OilTemList.Count() / 200;
            double tmpMax = 0.0;
            double tmpMin = 0.0;
            string tmpMaxTime = string.Empty;
            string tmpMinTime = string.Empty;
            int i = 1;
            List<double> vList = new List<double>();
            if (sample != 0)
            {
                foreach (DateBaseReport dateBaseReport in OilTemList)
                {
                    double nowVal = double.Parse(dateBaseReport.Value);
                    string nowTime = dateBaseReport.CreateTime.ToString();
                    if (i % sample == 0) // 取余为零，标志一个周期结束
                    {
                        OilTemLabels.Add(tmpMaxTime);
                        OilTemSeries[0].Values.Add(tmpMax);
                        vList.Add(tmpMax);
                        OilTemLabels.Add(tmpMinTime);
                        OilTemSeries[0].Values.Add(tmpMin);
                        vList.Add(tmpMin);
                        tmpMax = 0.0;
                        tmpMin = 0.0;
                    }
                    else // 周期内取最大最小值
                    {
                        if (tmpMax <= nowVal)
                        {
                            tmpMax = nowVal;
                            tmpMaxTime = nowTime;
                        }
                        if (tmpMin >= nowVal)
                        {
                            tmpMin = nowVal;
                            tmpMinTime = nowTime;
                        }
                    }
                    i++;
                }
            }
            else
            {
                foreach (DateBaseReport dateBaseReport in OilTemList)
                {
                    double nowVal = double.Parse(dateBaseReport.Value);
                    string nowTime = dateBaseReport.CreateTime.ToString();
                    OilTemLabels.Add(nowTime);
                    OilTemSeries[0].Values.Add(nowVal);
                    vList.Add(nowVal);
                }
            }
            this.lvcOilTem.MinValue = vList.Min()-1;
            this.lvcOilTem.MaxValue = vList.Max();
        }
        /// <summary>
        /// 绘制液位曲线
        /// </summary>
        private void DrawOilLevel(string Date)
        {
            LineSeries oilLevelLine = new LineSeries();
            oilLevelLine.LineSmoothness = 0;
            oilLevelLine.PointGeometry = null;
            OilLevelLabels = new List<string>();
            oilLevelLine.Values = new ChartValues<double>();
            OilLevelSeries = new SeriesCollection { };
            OilLevelSeries.Add(oilLevelLine);
            string sql = string.Format("Select * from DateBaseReport Where Type = '{0}' and CreateTime>'{1}'", (int)SaveType.HS_Self_OilLevel, Date);
            List<DateBaseReport> OilLevelList = DataHelper.Instance.ExecuteList<DateBaseReport>(sql);
            int sample = OilLevelList.Count() / 200;
            double tmpMax = 0.0;
            double tmpMin = 0.0;
            string tmpMaxTime = string.Empty;
            string tmpMinTime = string.Empty;
            int i = 1;
            List<double> vList = new List<double>();
            if (sample != 0)
            {
                foreach (DateBaseReport dateBaseReport in OilLevelList)
                {
                    double nowVal = double.Parse(dateBaseReport.Value);
                    string nowTime = dateBaseReport.CreateTime.ToString();
                    if (i % sample == 0) // 取余为零，标志一个周期结束
                    {
                        OilLevelLabels.Add(tmpMaxTime);
                        OilLevelSeries[0].Values.Add(tmpMax);
                        vList.Add(tmpMax);
                        OilLevelLabels.Add(tmpMinTime);
                        OilLevelSeries[0].Values.Add(tmpMin);
                        vList.Add(tmpMin);
                        tmpMax = 0.0;
                        tmpMin = 0.0;
                    }
                    else // 周期内取最大最小值
                    {
                        if (tmpMax <= nowVal)
                        {
                            tmpMax = nowVal;
                            tmpMaxTime = nowTime;
                        }
                        if (tmpMin >= nowVal)
                        {
                            tmpMin = nowVal;
                            tmpMinTime = nowTime;
                        }
                    }
                    i++;
                }
            }
            else
            {
                foreach (DateBaseReport dateBaseReport in OilLevelList)
                {
                    double nowVal = double.Parse(dateBaseReport.Value);
                    string nowTime = dateBaseReport.CreateTime.ToString();
                    OilLevelLabels.Add(nowTime);
                    OilLevelSeries[0].Values.Add(nowVal);
                    vList.Add(nowVal);
                }
            }
            this.lvcOilLevel.MinValue = vList.Min()-1;
            this.lvcOilLevel.MaxValue = vList.Max();
        }
        /// <summary>
        /// 查询一月内
        /// </summary>
        private void BtnMounth_Click(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            this.btnDay.Foreground = (Brush)bc.ConvertFrom("#525252");
            this.btnDay.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnMonth.Foreground = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnMonth.Background = (Brush)bc.ConvertFrom("#326CF3");
            DrawSystemPress(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm"));
            DrawOilTem(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm"));
            DrawOilLevel(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm"));
            DrawMainFlow(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm"));
            DrawMainTwoFlow(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"));
            DataContext = null;
            DataContext = this;
        }
    }
}
