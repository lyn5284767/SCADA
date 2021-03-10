using COM.Common;
using ControlLibrary;
using System;
using System.Collections.Generic;
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

namespace Main.SecondFloor
{
    /// <summary>
    /// SFReport.xaml 的交互逻辑
    /// </summary>
    public partial class SFReport : UserControl
    {
        private static SFReport _instance = null;
        private static readonly object syncRoot = new object();

        public static SFReport Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFReport();
                        }
                    }
                }
                return _instance;
            }
        }

        public SFReport()
        {
            InitializeComponent();
            // 设备编码
            DeviceEncodeMultiConverter deviceEncodeMultiConverter = new DeviceEncodeMultiConverter();
            MultiBinding deviceEncodeMultiBind = new MultiBinding();
            deviceEncodeMultiBind.Converter = deviceEncodeMultiConverter;
            deviceEncodeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["DeviceYear"], Mode = BindingMode.OneWay });
            deviceEncodeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["DeviceModel"], Mode = BindingMode.OneWay });
            deviceEncodeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["DeviceCarNum"], Mode = BindingMode.OneWay }); ;
            deviceEncodeMultiBind.NotifyOnSourceUpdated = true;
            deviceEncodeMultiBind.ConverterParameter = "RP";
            this.textBlockReportDeviceNumber.SetBinding(TextBlock.TextProperty, deviceEncodeMultiBind);
            this.Loaded += SFReport_Loaded;
        }

        private void SFReport_Loaded(object sender, RoutedEventArgs e)
        {
            ReportTextBlockShow();
        }

        public void ReportTextBlockShow()
        {
            try
            {
                DateTime dt = DateTime.Now;
                this.tbReportTime.Text = string.Format("报表生成时间：{0}年{1}月{2}日", dt.Year, dt.Month, dt.Day);
                GlobalData.Instance.reportData.PowerOnTime = GlobalData.Instance.da["165To168DeviceBootTime"].Value.Int32;
                GlobalData.Instance.reportData.WorkTime = GlobalData.Instance.da["106N23WorkTime"].Value.Int32;
                GlobalData.Instance.reportData.DrillDownCount = GlobalData.Instance.da["114N23N1DrillDownCount"].Value.Int32;
                GlobalData.Instance.reportData.DrillUpCount = GlobalData.Instance.da["114N23N1DrillUpCount"].Value.Int32;

                this.textBlockReportPowerOnTime.Text = "开机时间：" + (GlobalData.Instance.reportData.PowerOnTime / 10.0).ToString();
                this.textBlockReportWorkTime.Text = "工作时间：" + (GlobalData.Instance.reportData.WorkTime / 10.0).ToString();
                this.textBlockReportDrillDownCount.Text = "自动下钻次数：" + GlobalData.Instance.reportData.DrillDownCount.ToString();
                this.textBlockReportDrillUpCount.Text = "自动起钻次数：" + GlobalData.Instance.reportData.DrillUpCount.ToString();
                this.textBlockReportRobotBigHookInterLock.Text = "机械手与大钩互锁解除：" + GlobalData.Instance.reportData.RobotBigHookInterLock.ToString();
                this.textBlockReportRobotTopDriveInterlock.Text = "机械手与顶驱互锁解除：" + GlobalData.Instance.reportData.RobotTopDriveInterlock.ToString();
                this.textBlockReportRobotElevatorInterlock.Text = "吊卡关门信号屏蔽：" + GlobalData.Instance.reportData.ElevatorClose.ToString();
                //this.textBlockReportElevatorBigHookInterlock.Text = "吊卡与大钩互锁解除：" + GlobalData.Instance.reportData.ElevatorBigHookInterlock.ToString();
                this.textBlockReportRobotRetainingRopeInterlock.Text = "机械手与挡绳互锁解除：" + GlobalData.Instance.reportData.RobotRetainingRopeInterlock.ToString();
                this.textBlockReportRobotFingerBeamLockInterlock.Text = "指梁锁打开确认：" + GlobalData.Instance.reportData.RobotFingerBeamLockInterlock.ToString();
                this.textBlockReportSecondFloorCommunication.Text = "铁架工通讯中断：" + GlobalData.Instance.reportData.SecondFloorCommunication.ToString();
                this.textBlockReportOperationFloorCommunication.Text = "操作台通讯中断：" + GlobalData.Instance.reportData.OperationFloorCommunication.ToString();
                this.textBlockReportCarMotorAlarm.Text = "小车电机报警：" + GlobalData.Instance.reportData.CarMotorAlarm.ToString();
                this.textBlockReportArmMotorAlarm.Text = "手臂电机报警：" + GlobalData.Instance.reportData.ArmMotorAlarm.ToString();
                this.textBlockReportRotateMotorAlarm.Text = "回转电机报警：" + GlobalData.Instance.reportData.RotateMotorAlarm.ToString();
                this.textBlockReportGripMotorAlarm.Text = "抓手电机报警：" + GlobalData.Instance.reportData.GripMotorAlarm.ToString();
                this.textBlockReportFingerMotorAlarm.Text = "手指电机报警：" + GlobalData.Instance.reportData.FingerMotorAlarm.ToString();
                this.textBlockReportDrillCollarMotorAlarm.Text = "钻铤锁电机报警：" + GlobalData.Instance.reportData.DrillCollarMotorAlarm.ToString();
                this.textBlockReportRetainingRopeMotorAlarm.Text = "挡绳电机报警：" + GlobalData.Instance.reportData.RetainingRopeMotorAlarm.ToString();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 预览报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PreviewReport(object sender, RoutedEventArgs e)
        {
            string savePath = GenerateReport(GlobalData.Instance.reportData);
            Report report = new Report();
            report.CreateNewDocumentVisible(savePath); //模板路径
        }
        /// <summary>
        /// 生成报表
        /// </summary>
        private void btn_GenerateReport(object sender, RoutedEventArgs e)
        {
            GenerateReport(GlobalData.Instance.reportData);
        }

        private string GenerateReport(ReportData rD)
        {
            string filePath = System.Environment.CurrentDirectory + "\\Report";
            string templatePath = filePath + "\\Template\\ReportTemplate.dot";
            string savePath = "";
            try
            {
                //string filePath = System.Environment.CurrentDirectory + "\\Report";
                //string templatePath = filePath + "\\Template\\ReportTemplate.dot";
                Report report = new Report();
                report.CreateNewDocument(templatePath); //模板路径 "yyyy年MM月dd HH时mm分ss秒"
                report.InsertValue("BookMark_DeviceNumber", rD.DeviceNumber);//在书签“BookMark_DeviceNumber ”处插入值
                report.InsertValue("BookMark_ReportGenerateTime", DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒"));
                report.InsertValue("BookMark_PowerOnTime", (rD.PowerOnTime / 10.0).ToString());
                report.InsertValue("BookMark_WorkTime", (rD.WorkTime / 10.0).ToString());
                report.InsertValue("BooKMark_DrillDownCount", rD.DrillDownCount.ToString());
                report.InsertValue("BookMark_DrillUpCount", rD.DrillUpCount.ToString());
                report.InsertValue("BookMark_RobotBigHookInterLock", rD.RobotBigHookInterLock.ToString());
                report.InsertValue("BookMark_RobotTopDriveInterlock", rD.RobotTopDriveInterlock.ToString());
                report.InsertValue("BookMark_RobotElevatorInterlock", rD.ElevatorClose.ToString());
                report.InsertValue("BookMark_ElevatorBigHookInterlock", rD.ElevatorBigHookInterlock.ToString());
                report.InsertValue("BookMark_RobotRetainingRopeInterlock", rD.RobotRetainingRopeInterlock.ToString());
                report.InsertValue("BookMark_RobotFingerBeamLockInterlock", rD.RobotFingerBeamLockInterlock.ToString());
                report.InsertValue("BookMark_SecondFloorCommunication", rD.SecondFloorCommunication.ToString());
                report.InsertValue("BookMark_OperationFloorCommunication", rD.OperationFloorCommunication.ToString());
                report.InsertValue("BookMark_CarMotorAlarm", rD.CarMotorAlarm.ToString());
                report.InsertValue("BookMark_ArmMotorAlarm", rD.ArmMotorAlarm.ToString());
                report.InsertValue("BookMark_RotateMotorAlarm", rD.RotateMotorAlarm.ToString());
                report.InsertValue("BookMark_GripMotorAlarm", rD.GripMotorAlarm.ToString());
                report.InsertValue("BookMark_FingerMotorAlarm", rD.FingerMotorAlarm.ToString());
                report.InsertValue("BookMark_DrillCollarMotorAlarm", rD.DrillCollarMotorAlarm.ToString());
                report.InsertValue("BookMark_RetainingRopeMotorAlarm", rD.RetainingRopeMotorAlarm.ToString());
                //string savePath = filePath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                savePath = filePath + "\\" + DateTime.Now.ToString("yyyy-MM-dd,HH：mm：ss") + ".doc";
                report.SaveDocument(savePath); //文档路径
                System.Windows.MessageBox.Show("报告文件生成成功");

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            return savePath;

        }
    }
}
