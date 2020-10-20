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
        int Twinkle = 0; // 图片闪烁标志
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
            ProcessTimer.Change(0, 500);
            GlobalData.Instance.DeviceLink.Clear();
            if(GlobalData.Instance.da.GloConfig.SFType !=0) GlobalData.Instance.DeviceLink.Append(new IngDeviceStatus() { NowType = SystemType.SecondFloor, IsLoad = false,DeviceName="二层台" });
            if (GlobalData.Instance.da.GloConfig.DRType != 0) GlobalData.Instance.DeviceLink.Append(new IngDeviceStatus() { NowType = SystemType.DrillFloor, IsLoad = false, DeviceName = "钻台面" });
            if (GlobalData.Instance.da.GloConfig.SIRType != 0) GlobalData.Instance.DeviceLink.Append(new IngDeviceStatus() { NowType = SystemType.SIR, IsLoad = false, DeviceName = "铁钻工" });
            if (GlobalData.Instance.da.GloConfig.CatType != 0) GlobalData.Instance.DeviceLink.Append(new IngDeviceStatus() { NowType = SystemType.CatRoad, IsLoad = false, DeviceName = "猫道" });
            NowDeviceNode = GlobalData.Instance.DeviceLink.Head;
            this.bdMid.Child = IngSF.Instance;
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
                // 联动
                this.tbLink.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay, Converter = new LinkOpenOrCloseConverter() });
                // 6.30新增
                this.tbLink.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay, Converter = new LinkOpenOrCloseConverter() });
                MultiBinding LinkErrorMultiBind = new MultiBinding();
                LinkErrorMultiBind.Converter = new LinkErrorCoverter();
                LinkErrorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drLinkError"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b1"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b2"], Mode = BindingMode.OneWay });
                LinkErrorMultiBind.NotifyOnSourceUpdated = true;
                this.LinkError.SetBinding(TextBlock.TextProperty, LinkErrorMultiBind);
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
        /// 切换当前运行设备定时器
        /// </summary>
        /// <param name="obj"></param>
        private void ProcessTimer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    //if (NowDeviceNode.Data.NowType == SystemType.SecondFloor && !NowDeviceNode.Data.IsLoad) // 加载二层台
                    //{
                    //    InitPanel(NowDeviceNode);
                    //    this.bdMid.Child = IngSF.Instance;
                    //}
                    //else if (NowDeviceNode.Data.NowType == SystemType.DrillFloor && !NowDeviceNode.Data.IsLoad) // 加载钻台面
                    //{
                    //    InitPanel(NowDeviceNode);
                    //    this.bdMid.Child = IngDR.Instance;
                    //}
                    //else if (NowDeviceNode.Data.NowType == SystemType.SIR && !NowDeviceNode.Data.IsLoad) // 加载铁钻工
                    //{
                    //    InitPanel(NowDeviceNode);
                    //    this.bdMid.Child = IngSIR.Instance;
                    //}
                    //else if (NowDeviceNode.Data.NowType == SystemType.CatRoad && !NowDeviceNode.Data.IsLoad) // 加载猫道
                    //{
                    //    InitPanel(NowDeviceNode);
                    //    this.bdMid.Child = IngCat.Instance;
                    //}
                    GetNowDevice();
                    DeviceImgTwinkle();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 切换当前设备
        /// </summary>
        private void GetNowDevice()
        {
            // 联动模式根据操作台数据切换
            if (GlobalData.Instance.da["460b0"].Value.Boolean)
            {
                if (GlobalData.Instance.da["462b0"].Value.Boolean && NowDeviceNode.Data.NowType != SystemType.SecondFloor)
                {
                    InitPanel(SystemType.SecondFloor);
                    this.bdMid.Child = IngSF.Instance;
                }
                else if (GlobalData.Instance.da["462b1"].Value.Boolean && NowDeviceNode.Data.NowType != SystemType.DrillFloor)
                {
                    InitPanel(SystemType.DrillFloor);
                    this.bdMid.Child = IngDR.Instance;
                }
                else if (GlobalData.Instance.da["462b2"].Value.Boolean && NowDeviceNode.Data.NowType != SystemType.SIR)
                {
                    InitPanel(SystemType.SIR);
                    this.bdMid.Child = IngSIR.Instance;
                }
                else if (GlobalData.Instance.da["462b3"].Value.Boolean && NowDeviceNode.Data.NowType != SystemType.CatRoad)
                {
                    InitPanel(SystemType.CatRoad);
                    this.bdMid.Child = IngCat.Instance;
                }
            }
            else// 非联动模式，根据旋转按钮切换
            {
                if (GlobalData.Instance.da["459b0"].Value.Boolean && NowDeviceNode.Data.NowType != SystemType.SecondFloor)
                {
                    InitPanel(SystemType.SecondFloor);
                    this.bdMid.Child = IngSF.Instance;
                }
                else if (GlobalData.Instance.da["459b1"].Value.Boolean && NowDeviceNode.Data.NowType != SystemType.DrillFloor)
                {
                    InitPanel(SystemType.DrillFloor);
                    this.bdMid.Child = IngDR.Instance;
                }
                else if (GlobalData.Instance.da["459b2"].Value.Boolean && NowDeviceNode.Data.NowType != SystemType.SIR)
                {
                    InitPanel(SystemType.SIR);
                    this.bdMid.Child = IngSIR.Instance;
                }
                else if (GlobalData.Instance.da["459b3"].Value.Boolean && NowDeviceNode.Data.NowType != SystemType.CatRoad)
                {
                    InitPanel(SystemType.CatRoad);
                    this.bdMid.Child = IngCat.Instance;
                }
            }
        }
        /// <summary>
        /// 弃用
        /// </summary>
        private void InitSetpBar()
        {
            #region 步骤条相关
            if (GlobalData.Instance.da["operationModel"].Value.Byte == 5 && GlobalData.Instance.da["droperationModel"].Value.Byte == 5) // 自动模式
            {
                if (GlobalData.Instance.da["workModel"].Value.Byte == 2 && GlobalData.Instance.da["drworkModel"].Value.Byte == 2) // 排管
                {
                    this.tbNowOpr.Text = "排杆";
                    if (NowDeviceNode.Data.NowType == SystemType.SecondFloor)
                    {
                    }
                    else if (NowDeviceNode.Data.NowType == SystemType.DrillFloor)
                    {
                    }
                    else if (NowDeviceNode.Data.NowType == SystemType.SIR)
                    {
                        
                    }
                }
                else if (GlobalData.Instance.da["workModel"].Value.Byte == 1 && GlobalData.Instance.da["drworkModel"].Value.Byte == 1)// 送杆
                {
                    this.tbNowOpr.Text = "送杆"; 
                    if (NowDeviceNode.Data.NowType == SystemType.SecondFloor)
                    {
                        this.sbSFDrillDownStep.Visibility = Visibility.Visible;
                        this.sbDRDrillDownStep.Visibility = Visibility.Collapsed;
                        this.sbIronInButton.Visibility = Visibility.Collapsed;
                        this.sbIronOutButton.Visibility = Visibility.Collapsed;
                        this.sbDRDrillUpStep.Visibility = Visibility.Collapsed;
                        this.sbSFDrillUpStep.Visibility = Visibility.Collapsed;
                    }
                    else if (NowDeviceNode.Data.NowType == SystemType.DrillFloor)
                    {
                    }
                    else if (NowDeviceNode.Data.NowType == SystemType.SIR)
                    {

                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// 设备切换后重新初始化面板
        /// </summary>
        private void InitPanel(SystemType systemType)
        {
            try
            {
                Node<IngDeviceStatus> node = GlobalData.Instance.DeviceLink.Head;
                if (node != null)
                {
                    do // 遍历链表找到当前设备所处位置
                    {
                        if (node.Data.NowType == systemType)
                        {
                            NowDeviceNode = node;
                            NowDeviceNode.Data.IsLoad = true;
                        }
                        else
                        {
                            node.Data.IsLoad = false;
                        }
                        node = node.Next;
                    } while (node != null);
                    this.tbNowDevice.Text = NowDeviceNode.Data.DeviceName;
                    if (NowDeviceNode.Next != null)
                        this.tbNextDevice.Text = NowDeviceNode.Next.Data.DeviceName;
                    else
                        this.tbNextDevice.Text = "无";
                    if (GlobalData.Instance.da["workModel"].Value.Byte == 1 && GlobalData.Instance.da["drworkModel"].Value.Byte == 1)// 送杆
                    {
                        this.tbNowOpr.Text = "送杆";
                        this.spDrillDown.Visibility = Visibility.Visible;
                        this.spDrillUp.Visibility = Visibility.Collapsed;
                        if (NowDeviceNode.Data.NowType == SystemType.SecondFloor)
                        {
                            this.sbSFDrillDownStep.Visibility = Visibility.Visible;
                            this.sbDRDrillDownStep.Visibility = Visibility.Collapsed;
                            this.sbIronInButton.Visibility = Visibility.Collapsed;
                            this.btnCatConfirm.Visibility = Visibility.Collapsed;
                        }
                        else if (NowDeviceNode.Data.NowType == SystemType.DrillFloor)
                        {
                            this.sbSFDrillDownStep.Visibility = Visibility.Collapsed;
                            this.sbDRDrillDownStep.Visibility = Visibility.Visible;
                            this.sbIronInButton.Visibility = Visibility.Collapsed;
                            this.btnCatConfirm.Visibility = Visibility.Collapsed;
                        }
                        else if (NowDeviceNode.Data.NowType == SystemType.SIR)
                        {
                            this.sbSFDrillDownStep.Visibility = Visibility.Collapsed;
                            this.sbDRDrillDownStep.Visibility = Visibility.Collapsed;
                            this.sbIronInButton.Visibility = Visibility.Visible;
                            this.btnCatConfirm.Visibility = Visibility.Collapsed;
                        }
                        else if (NowDeviceNode.Data.NowType == SystemType.CatRoad)
                        {
                            this.sbSFDrillDownStep.Visibility = Visibility.Collapsed;
                            this.sbDRDrillDownStep.Visibility = Visibility.Collapsed;
                            this.sbIronInButton.Visibility = Visibility.Collapsed;
                            this.btnCatConfirm.Visibility = Visibility.Visible;
                        }
                    }
                    else if (GlobalData.Instance.da["workModel"].Value.Byte == 2 && GlobalData.Instance.da["drworkModel"].Value.Byte == 2)// 排杆
                    {
                        this.tbNowOpr.Text = "排杆";
                        this.spDrillDown.Visibility = Visibility.Collapsed;
                        this.spDrillUp.Visibility = Visibility.Visible;
                        if (NowDeviceNode.Data.NowType == SystemType.SecondFloor)
                        {
                            this.sbSFDrillUpStep.Visibility = Visibility.Visible;
                            this.sbDRDrillUpStep.Visibility = Visibility.Collapsed;
                            this.sbIronOutButton.Visibility = Visibility.Collapsed;
                            this.btnCatConfirm.Visibility = Visibility.Collapsed;
                        }
                        else if (NowDeviceNode.Data.NowType == SystemType.DrillFloor)
                        {
                            this.sbSFDrillUpStep.Visibility = Visibility.Collapsed;
                            this.sbDRDrillUpStep.Visibility = Visibility.Visible;
                            this.sbIronOutButton.Visibility = Visibility.Collapsed;
                            this.btnCatConfirm.Visibility = Visibility.Collapsed;
                        }
                        else if (NowDeviceNode.Data.NowType == SystemType.SIR)
                        {
                            this.sbSFDrillUpStep.Visibility = Visibility.Collapsed;
                            this.sbDRDrillUpStep.Visibility = Visibility.Collapsed;
                            this.sbIronOutButton.Visibility = Visibility.Visible;
                            this.btnCatConfirm.Visibility = Visibility.Collapsed;
                        }
                        else if (NowDeviceNode.Data.NowType == SystemType.CatRoad)
                        {
                            this.sbSFDrillUpStep.Visibility = Visibility.Collapsed;
                            this.sbDRDrillUpStep.Visibility = Visibility.Collapsed;
                            this.sbIronOutButton.Visibility = Visibility.Collapsed;
                            this.btnCatConfirm.Visibility = Visibility.Visible;
                        }
                    }
                    ChangeUseDeviceImg(NowDeviceNode.Data.NowType);
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 根据设备类型改变使用的设备图片-暂时弃用
        /// </summary>
        private void ChangeUseDeviceImg(SystemType systemType)
        {
            //if (systemType == SystemType.SecondFloor)
            //{
            //    this.imgSF.Source = new BitmapImage(new Uri("../Images/sfUse.png", UriKind.Relative));
            //    this.imgDR.Source = new BitmapImage(new Uri("../Images/dr.png", UriKind.Relative));
            //    this.imgSIR.Source = new BitmapImage(new Uri("../Images/sir.png", UriKind.Relative));
            //    this.imgCat.Source = new BitmapImage(new Uri("../Images/cat.png", UriKind.Relative));
            //}
            //else if (systemType == SystemType.DrillFloor)
            //{
            //    this.imgSF.Source = new BitmapImage(new Uri("../Images/sf.png", UriKind.Relative));
            //    this.imgDR.Source = new BitmapImage(new Uri("../Images/drUse.png", UriKind.Relative));
            //    this.imgSIR.Source = new BitmapImage(new Uri("../Images/sir.png", UriKind.Relative));
            //    this.imgCat.Source = new BitmapImage(new Uri("../Images/cat.png", UriKind.Relative));
            //}
            //else if (systemType == SystemType.SIR)
            //{
            //    this.imgSF.Source = new BitmapImage(new Uri("../Images/sf.png", UriKind.Relative));
            //    this.imgDR.Source = new BitmapImage(new Uri("../Images/dr.png", UriKind.Relative));
            //    this.imgSIR.Source = new BitmapImage(new Uri("../Images/sirUse.png", UriKind.Relative));
            //    this.imgCat.Source = new BitmapImage(new Uri("../Images/cat.png", UriKind.Relative));
            //}
            //else if (systemType == SystemType.CatRoad)
            //{
            //    this.imgSF.Source = new BitmapImage(new Uri("../Images/sf.png", UriKind.Relative));
            //    this.imgDR.Source = new BitmapImage(new Uri("../Images/dr.png", UriKind.Relative));
            //    this.imgSIR.Source = new BitmapImage(new Uri("../Images/sir.png", UriKind.Relative));
            //    this.imgCat.Source = new BitmapImage(new Uri("../Images/catUse.png", UriKind.Relative));
            //}
        }
        /// <summary>
        /// 使用设备闪烁
        /// </summary>
        private void DeviceImgTwinkle()
        {
            Twinkle++;
            if (Twinkle > 1000) Twinkle = 0;
            if (NowDeviceNode.Data.NowType == SystemType.SecondFloor)
            {
                if (Twinkle % 2 == 0) this.smSF.LampType = 0;
                else this.smSF.LampType = 1;
            }
            else if (NowDeviceNode.Data.NowType == SystemType.DrillFloor)
            {
                if (Twinkle % 2 == 0) this.smDR.LampType = 0;
                else this.smDR.LampType = 1;
            }
            else if (NowDeviceNode.Data.NowType == SystemType.SIR)
            {
                if (Twinkle % 2 == 0) this.smSIR.LampType = 0;
                else this.smSIR.LampType = 1;
            }
            else if (NowDeviceNode.Data.NowType == SystemType.CatRoad)
            {
                if (Twinkle % 2 == 0) this.smCat.LampType = 0;
                else this.smCat.LampType = 1;
            }
        }

        /// <summary>
        /// 联动设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbIng_Click(object sender, RoutedEventArgs e)
        {
            IngSetWindow ingSet = new IngSetWindow();
            ingSet.ShowDialog();
        }
        /// <summary>
        /// 二层台设置
        /// </summary>
        private void tbSF_Click(object sender, RoutedEventArgs e)
        {
            IngSFSetWindow sfSet = new IngSFSetWindow();
            sfSet.ShowDialog();
        }

        /// <summary>
        /// 钻台面设置
        /// </summary>
        private void tbDR_Click(object sender, RoutedEventArgs e)
        {
            IngDRWindow ingDRWindow = new IngDRWindow();
            ingDRWindow.ShowDialog();
        }
        /// <summary>
        /// 液压站设置
        /// </summary>
        private void tbHS_Click(object sender, RoutedEventArgs e)
        {
            IngHSSetWindow ingHSSetWindow = new IngHSSetWindow();
            ingHSSetWindow.ShowDialog();
        }
        /// <summary>
        /// 铁钻工设置
        /// </summary>
        private void tbSIR_Click(object sender, RoutedEventArgs e)
        {
            IngSIRSetWindow ingSIRSetWindow = new IngSIRSetWindow();
            ingSIRSetWindow.ShowDialog();
        }
        /// <summary>
        /// 手动切换设备
        /// </summary>
        private void CBDevice_Changed(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox cb = sender as System.Windows.Controls.ComboBox;
            if (cb.SelectedIndex == 0) // 二层台
            {
                InitPanel(SystemType.SecondFloor);
                this.bdMid.Child = IngSF.Instance;
            }
            else if (cb.SelectedIndex == 1) // 钻台面
            {
                InitPanel(SystemType.DrillFloor);
                this.bdMid.Child = IngDR.Instance;
            }
            else if (cb.SelectedIndex == 2) // 铁钻工
            {
                InitPanel(SystemType.SIR);
                this.bdMid.Child = IngSIR.Instance;
            }
            else if (cb.SelectedIndex == 3) // 
            {
                InitPanel(SystemType.CatRoad);
                this.bdMid.Child = IngCat.Instance;
            }
        }
        /// <summary>
        /// 猫道设置
        /// </summary>
        private void tbCat_Click(object sender, RoutedEventArgs e)
        {
            IngCatSetWindow ingCatSetWindow = new IngCatSetWindow();
            ingCatSetWindow.ShowDialog();
        }
    }
}
