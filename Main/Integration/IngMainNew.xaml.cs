using COM.Common;
using ControlLibrary;
using DevExpress.Mvvm.Native;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.Integration
{
    /// <summary>
    /// IngMainNew.xaml 的交互逻辑
    /// </summary>
    public partial class IngMainNew : UserControl
    {
        private static IngMainNew _instance = null;
        private static readonly object syncRoot = new object();

        public static IngMainNew Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngMainNew();
                        }
                    }
                }
                return _instance;
            }
        }
        // 流程定时器
        System.Threading.Timer ProcessTimer;
        Node<IngDeviceStatus> NowDeviceNode { get; set; }
        public IngMainNew()
        {
            InitializeComponent();
            Init();
            VariableBinding();
            this.Loaded += IngMainNew_Loaded;
        }

        private void Init()
        {
            GlobalData.Instance.Rows = GlobalData.Instance.da["DrillNums"].Value.Byte;
            GlobalData.Instance.DrillNum = GlobalData.Instance.da["103E23B5"].Value.Byte;
            ProcessTimer = new System.Threading.Timer(new TimerCallback(ProcessTimer_Elapsed), this, 2000, 50);//改成50ms 的时钟
            GlobalData.Instance.DeviceLink.Clear();
            if(GlobalData.Instance.da.GloConfig.SFType !=0) GlobalData.Instance.DeviceLink.Append(new IngDeviceStatus() { NowType = SystemType.SecondFloor, IsLoad = false,DeviceName="二层台" });
            if (GlobalData.Instance.da.GloConfig.DRType != 0) GlobalData.Instance.DeviceLink.Append(new IngDeviceStatus() { NowType = SystemType.DrillFloor, IsLoad = false, DeviceName = "钻台面" });
            if (GlobalData.Instance.da.GloConfig.SIRType != 0) GlobalData.Instance.DeviceLink.Append(new IngDeviceStatus() { NowType = SystemType.SIR, IsLoad = false, DeviceName = "铁钻工" });
            NowDeviceNode = GlobalData.Instance.DeviceLink.Head;
        }

        private void IngMainNew_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);

            string configPath = System.Environment.CurrentDirectory + "\\KeyBoard.exe";
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processList)
            {
                if (process.ProcessName.Contains("KeyBoard"))
                {
                    process.Kill();
                }
            }
        }
        private void VariableBinding()
        {
            try
            {
                #region 排杆
                // 二层台排杆
                MultiBinding sbDrillUpMultiBind = new MultiBinding();
                sbDrillUpMultiBind.Converter = new AutoModeStepCoverter();
                sbDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                sbDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
                sbDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
                sbDrillUpMultiBind.ConverterParameter = "one";
                sbDrillUpMultiBind.NotifyOnSourceUpdated = true;
                this.sbSFDrillUpStep.SetBinding(StepBar.StepIndexProperty, sbDrillUpMultiBind);
                // 钻台面排杆
                MultiBinding sbDRDrillDownMultiBind = new MultiBinding();
                sbDRDrillDownMultiBind.Converter = new DRStepCoverter();
                sbDRDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                sbDRDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                sbDRDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
                sbDRDrillDownMultiBind.NotifyOnSourceUpdated = true;
                this.sbDRDrillUpStep.SetBinding(StepBar.StepIndexProperty, sbDRDrillDownMultiBind);
                // 一键卸扣
                MultiBinding sbOutButtonMultiBind = new MultiBinding();
                sbOutButtonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.ConverterParameter = "outButton";
                sbOutButtonMultiBind.NotifyOnSourceUpdated = true;
                this.sbIronOutButton.SetBinding(StepBar.StepIndexProperty, sbOutButtonMultiBind);
                #endregion
                #region 送杆
                // 二层台送杆
                MultiBinding sbDrillDownMultiBind = new MultiBinding();
                sbDrillDownMultiBind.Converter = new AutoModeStepCoverter();
                sbDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                sbDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
                sbDrillDownMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E5AutoModelCurrentStep"], Mode = BindingMode.OneWay });
                sbDrillDownMultiBind.ConverterParameter = "one";
                sbDrillDownMultiBind.NotifyOnSourceUpdated = true;
                this.sbSFDrillDownStep.SetBinding(StepBar.StepIndexProperty, sbDrillDownMultiBind);
                //钻台面送杆
                MultiBinding sbDRDrillUpMultiBind = new MultiBinding();
                sbDRDrillUpMultiBind.Converter = new DRStepCoverter();
                sbDRDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                sbDRDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                sbDRDrillUpMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drAutoStep"], Mode = BindingMode.OneWay });
                sbDRDrillUpMultiBind.NotifyOnSourceUpdated = true;
                this.sbDRDrillDownStep.SetBinding(StepBar.StepIndexProperty, sbDRDrillUpMultiBind);
                // 一键上扣
                MultiBinding sbInbuttonMultiBind = new MultiBinding();
                sbInbuttonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.ConverterParameter = "inButton";
                sbInbuttonMultiBind.NotifyOnSourceUpdated = true;
                this.sbIronInButton.SetBinding(StepBar.StepIndexProperty, sbInbuttonMultiBind);
                #endregion
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 切换当前运行设备顶上去
        /// </summary>
        /// <param name="obj"></param>
        private void ProcessTimer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (NowDeviceNode.Data.NowType == SystemType.SecondFloor && !NowDeviceNode.Data.IsLoad)
                    {
                        InitPanel(NowDeviceNode);
                        this.bdMid.Child = IngSF.Instance;
                    }
                    else if (NowDeviceNode.Data.NowType == SystemType.DrillFloor && !NowDeviceNode.Data.IsLoad)
                    {
                        InitPanel(NowDeviceNode);
                        this.bdMid.Child = IngDR.Instance;
                    }
                    else if (NowDeviceNode.Data.NowType == SystemType.SIR && !NowDeviceNode.Data.IsLoad)
                    {
                        InitPanel(NowDeviceNode);
                        this.bdMid.Child = IngSIR.Instance;
                    }
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 设备切换后重新初始化面板
        /// </summary>
        private void InitPanel(Node<IngDeviceStatus> node)
        {
            NowDeviceNode.Data.IsLoad = true;
            this.tbNowDevice.Text = NowDeviceNode.Data.DeviceName;
            if (NowDeviceNode.Next != null)
                this.tbNextDevice.Text = NowDeviceNode.Next.Data.DeviceName;
            else
                this.tbNextDevice.Text = "无";
            if (NowDeviceNode.Data.NowType == SystemType.SecondFloor)
            {
                this.sbSFDrillDownStep.Visibility = Visibility.Visible;
                this.sbDRDrillDownStep.Visibility = Visibility.Collapsed;
                this.sbIronInButton.Visibility = Visibility.Collapsed;
            }
            else if (NowDeviceNode.Data.NowType == SystemType.DrillFloor)
            {
                this.sbSFDrillDownStep.Visibility = Visibility.Collapsed;
                this.sbDRDrillDownStep.Visibility = Visibility.Visible;
                this.sbIronInButton.Visibility = Visibility.Collapsed;
            }
            if (NowDeviceNode.Data.NowType == SystemType.SIR)
            {
                this.sbSFDrillDownStep.Visibility = Visibility.Collapsed;
                this.sbDRDrillDownStep.Visibility = Visibility.Collapsed;
                this.sbIronInButton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 联动设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbIng_Click(object sender, RoutedEventArgs e)
        {
            //UIElementCollection childrens = this.gdMain.Children;
            //foreach (UIElement ui in childrens)
            //{
            //    if (ui is Drawer)
            //    {
            //        ((ui as Drawer).Content as Grid).Children.Clear();
            //        ((ui as Drawer).Content as Grid).Children.Add(IngSet.Instance);

            //    }
            //}
            //this.DrawerBottom.IsOpen = true;
            //this.gdSet.Children.Clear();
            //this.gdSet.Children.Add(IngSet.Instance);
            IngSetWindow ingSet = new IngSetWindow();
            ingSet.ShowDialog();
        }

        private void tbSF_Click(object sender, RoutedEventArgs e)
        {
            //this.gdSet.Children.Clear();
            //this.gdSet.Children.Add(SFSet.Instance);
            //UIElementCollection childrens = this.gdMain.Children;
            //foreach (UIElement ui in childrens)
            //{
            //    if (ui is Grid && (ui as Grid).Name == "gdSet")
            //    {
            //        (ui as Grid).Children.Clear();
            //        (ui as Grid).Children.Add(SFSet.Instance);
            //    }
            //}
            IngSFSetWindow sfSet = new IngSFSetWindow();
            sfSet.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NowDeviceNode.Data.IsLoad = false;
            NowDeviceNode = NowDeviceNode.Next;
            if (NowDeviceNode == null) NowDeviceNode = GlobalData.Instance.DeviceLink.Head;
        }
    }
}
