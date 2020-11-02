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
        public IngMainNew()
        {
            InitializeComponent();
            VariableBinding();
            SIRVariableReBinding = new System.Threading.Timer(new TimerCallback(SIRVariableTimer), null, Timeout.Infinite, 500);
            SIRVariableReBinding.Change(0, 500);
            HSVariableReBinding = new System.Threading.Timer(new TimerCallback(HSVariableTimer), null, Timeout.Infinite, 500);
            HSVariableReBinding.Change(0, 500);
            this.Loaded += IngMain_Loaded;
        }

        private void IngMain_Loaded(object sender, RoutedEventArgs e)
        {
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
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                IngVariableBinding();
                SFVariableBinding();
                DRVariableBinding();
                SIRVariableBinding();
                HSVariableBinding();
                CatVariableBinding();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        #region 联动设置
        /// <summary>
        /// 联动变量绑定
        /// </summary>
        private void IngVariableBinding()
        {
            try
            {
                // 6.30新增
                this.oobLink.SetBinding(OnOffButton.OnOffButtonCheckProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
                this.tbLink.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay, Converter = new LinkOpenOrCloseConverter() });
                //MultiBinding LinkErrorMultiBind = new MultiBinding();
                //LinkErrorMultiBind.Converter = new LinkErrorCoverter();
                //LinkErrorMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drLinkError"], Mode = BindingMode.OneWay });
                //LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b0"], Mode = BindingMode.OneWay });
                //LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b1"], Mode = BindingMode.OneWay });
                //LinkErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["460b2"], Mode = BindingMode.OneWay });
                //LinkErrorMultiBind.NotifyOnSourceUpdated = true;
                //this.LinkError.SetBinding(TextBlock.TextProperty, LinkErrorMultiBind);
                // 操作模式-描述
                MultiBinding IngOprMultiBind = new MultiBinding();
                IngOprMultiBind.Converter = new IngOprCoverter();
                IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                IngOprMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                IngOprMultiBind.NotifyOnSourceUpdated = true;
                this.operateMode.SetBinding(BasedSwitchButton.ContentDownProperty, IngOprMultiBind);
                // 操作模式-选择
                MultiBinding IngOprCheckMultiBind = new MultiBinding();
                IngOprCheckMultiBind.Converter = new IngOprCheckCoverter();
                IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                IngOprCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                IngOprCheckMultiBind.NotifyOnSourceUpdated = true;
                this.operateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, IngOprCheckMultiBind);
                // 工作模式-描述
                MultiBinding IngWorkMultiBind = new MultiBinding();
                IngWorkMultiBind.Converter = new IngWorkCoverter();
                IngWorkMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay });
                IngWorkMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay });
                IngWorkMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                IngWorkMultiBind.NotifyOnSourceUpdated = true;
                this.workMode.SetBinding(BasedSwitchButton.ContentDownProperty, IngWorkMultiBind);
                // 工作模式-选择
                MultiBinding IngWorkCheckMultiBind = new MultiBinding();
                IngWorkCheckMultiBind.Converter = new IngWorkCheckCoverter();
                IngWorkCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
                IngWorkCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
                IngWorkCheckMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                IngWorkCheckMultiBind.NotifyOnSourceUpdated = true;
                this.workMode.SetBinding(BasedSwitchButton.IsCheckedProperty, IngWorkCheckMultiBind);
                // 管柱选择
                IngDrillPipeTypeConverter ingDrillPipeTypeConverter = new IngDrillPipeTypeConverter();
                MultiBinding ingDrillPipeTypeMultiBind = new MultiBinding();
                ingDrillPipeTypeMultiBind.Converter = ingDrillPipeTypeConverter;
                ingDrillPipeTypeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drillPipeType"], Mode = BindingMode.OneWay });
                ingDrillPipeTypeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drdrillPipeType"], Mode = BindingMode.OneWay });
                ingDrillPipeTypeMultiBind.NotifyOnSourceUpdated = true;
                this.tubeType.SetBinding(TextBlock.TextProperty, ingDrillPipeTypeMultiBind);

                this.drDestination.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDes"], Mode = BindingMode.OneWay, Converter = new DesTypeConverter() });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 联动打开关闭
        /// </summary>
        /// <param name="isChecked"></param>
        private void OnOffButton_CBCheckedEvent(bool isChecked)
        {
            if (isChecked)
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 1, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                byte[] byteToSend = new byte[10] { 1, 32, 10, 0, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_OpState(object sender, EventArgs e)
        {
            byte[] sfbyteToSend;// 二层台
            byte[] drbyteToSend;// 钻台面
            byte[] sirbyteToSend;// 铁钻工
            if (this.operateMode.IsChecked)
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 5 });
                drbyteToSend = new byte[10] { 1, 32, 3, 31, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 4 });
                drbyteToSend = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(sfbyteToSend);
            Thread.Sleep(50);
            GlobalData.Instance.da.SendBytes(drbyteToSend);
            Thread.Sleep(50);
            GlobalData.Instance.da.SendBytes(sirbyteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_WorkModel(object sender, EventArgs e)
        {
            byte[] sfbyteToSend;// 二层台
            byte[] drbyteToSend;// 钻台面
            byte[] sirbyteToSend;// 铁钻工
            if (workMode.IsChecked)
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
                drbyteToSend = new byte[10] { 1, 32, 4, 41, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                sfbyteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
                drbyteToSend = new byte[10] { 1, 32, 4, 40, 0, 0, 0, 0, 0, 0 };
                sirbyteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(sfbyteToSend);
            Thread.Sleep(50);
            GlobalData.Instance.da.SendBytes(drbyteToSend);
            Thread.Sleep(50);
            GlobalData.Instance.da.SendBytes(sirbyteToSend);
        }
        /// <summary>
        /// 管柱选择协议
        /// </summary>
        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 3, (byte)tag });
            GlobalData.Instance.da.SendBytes(byteToSend);
            Thread.Sleep(50);
            byteToSend = GlobalData.Instance.SendToDR(new List<byte>() { 3, (byte)tag });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 目的地选择
        /// </summary>
        private void btn_SelectDes(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 33, 11, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        #region 二层台
        /// <summary>
        /// 二层台变量
        /// </summary>
        private void SFVariableBinding()
        {
            try
            {
                this.sfoperateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.sfoperateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
                this.sfworkMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
                this.sfworkMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelIsCheckConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 一键回零
        /// </summary>
        private void btn_SFAllMotorRetZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 点击使能
        /// </summary>
        private void btn_SFMotorEnable(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SFOpState(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.sfoperateMode.IsChecked)
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 5 });
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 1, 4 });
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_SFWorkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (sfworkMode.IsChecked)
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 1 });
            }
            else
            {
                byteToSend = GlobalData.Instance.SendByte(new List<byte> { 2, 2 });
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        #region 钻台面
        private void DRVariableBinding()
        {
            try
            {
                this.droperateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.droperateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelIsCheckConverter() });
                this.drworkMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
                this.drworkMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new WorkModelIsCheckConverter() });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 一键回零
        /// </summary>
        private void btn_DRAllMotorRetZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 13, 4 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 一键使能
        /// </summary>
        private void btn_DRMotorEnable(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_DROpState(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.droperateMode.IsChecked)
            {
                byteToSend = new byte[10] { 1, 32, 3, 31, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 1, 32, 3, 30, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_DRWorkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.drworkMode.IsChecked)
            {
                byteToSend = new byte[10] { 1, 32, 4, 41, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 1, 32, 4, 40, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion

        #region 铁钻工
        bool workModelCheck = false;
        byte bworkModel = 0;
        System.Threading.Timer SIRVariableReBinding;

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void SIRVariableBinding()
        {
            try
            {
                this.siroprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfOperationModelConverter() });
                this.siroprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                this.sirworkModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                this.sirworkModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                this.sirPipeTypeModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeModelConverter() });
                this.sirPipeTypeModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeIsCheckConverter() });
             }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_SIRoprModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.siroprModel.IsChecked) //当前手动状态
            {
                byteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前自动状态
            {
                byteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_SIRworkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.sirworkModel.IsChecked) //当前上扣模式
            {
                byteToSend = new byte[10] { 23, 17, 2, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前卸扣模式
            {
                byteToSend = new byte[10] { 23, 17, 2, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
            this.sirworkModel.ContentDown = "切换中";
            workModelCheck = true;
        }
        /// <summary>
        /// 管柱选择
        /// </summary>
        private void btn_SIRPipeTypeModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.sirPipeTypeModel.IsChecked) //当前钻杠
            {
                byteToSend = new byte[10] { 23, 17, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前套管
            {
                byteToSend = new byte[10] { 23, 17, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void SIRVariableTimer(object value)
        {
            if (bworkModel != GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte && workModelCheck)
            {
                this.sirworkModel.Dispatcher.Invoke(new Action(() =>
                {
                    this.sirworkModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                }));
                workModelCheck = false;
            }
            bworkModel = GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte;
        }
        #endregion

        #region 液压站
        bool bMainPumpOne = false;
        bool MainPumpOneCheck = false;
        bool bMainPumpTwo = false;
        bool MainPumpTwoCheck = false;
        System.Threading.Timer HSVariableReBinding;
        private void HSVariableBinding()
        {
            try
            {
                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay });
                HyControlModelMuilCoverter hyControlModelMultiConverter = new HyControlModelMuilCoverter();
                MultiBinding hyControlModelMultiBind = new MultiBinding();
                hyControlModelMultiBind.Converter = hyControlModelMultiConverter;
                hyControlModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay });
                hyControlModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b6"], Mode = BindingMode.OneWay });
                hyControlModelMultiBind.NotifyOnSourceUpdated = true;
                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, hyControlModelMultiBind);
                HyControlModelTxtMuilCoverter hyControlModelTxtMultiConverter = new HyControlModelTxtMuilCoverter();
                MultiBinding hyControlModelTxtlMultiBind = new MultiBinding();
                hyControlModelTxtlMultiBind.Converter = hyControlModelTxtMultiConverter;
                hyControlModelTxtlMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay });
                hyControlModelTxtlMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b6"], Mode = BindingMode.OneWay });
                hyControlModelTxtlMultiBind.NotifyOnSourceUpdated = true;
                this.controlModel.SetBinding(BasedSwitchButton.ContentDownProperty, hyControlModelTxtlMultiBind);

                this.MainPumpOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b3"], Mode = BindingMode.OneWay });
                this.MainPumpTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b5"], Mode = BindingMode.OneWay });
                this.HSConstantVoltagePump.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b7"], Mode = BindingMode.OneWay });
                this.HSDissipateHeat.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b1"], Mode = BindingMode.OneWay });
                this.HSHot.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b3"], Mode = BindingMode.OneWay });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        //司钻/本地控制
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            byteToSend = new byte[10] { 0, 19, 1, 9, 0, 0, 0, 0, 0, 0 };// 疑问，所有切换都是这个协议？
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 主泵1启动/停止
        /// </summary>
        private void btn_MainPumpOne(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpOne.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
            this.MainPumpOne.ContentDown = "切换中";
            this.MainPumpOneCheck = true;
        }
        /// <summary>
        /// 主泵2启动/停止
        /// </summary>
        private void btn_MainPumpTwo(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpTwo.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 3, 0, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 4, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
            this.MainPumpTwo.ContentDown = "切换中";
            this.MainPumpTwoCheck = true;
        }
        /// <summary>
        /// 恒压泵启动/停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_HSConstantVoltagePump(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.HSConstantVoltagePump.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 5, 0, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 6, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 散热泵启动/停止
        /// </summary>
        private void btn_HSDissipateHeat(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.HSDissipateHeat.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 7, 0, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 8, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 加热器启动/停止
        /// </summary>
        private void btn_HSHot(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.HSHot.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 9, 0, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 3, 10, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void HSVariableTimer(object value)
        {
            if (this.bMainPumpOne != GlobalData.Instance.da["770b3"].Value.Boolean && this.MainPumpOneCheck)
            {
                this.MainPumpOne.Dispatcher.Invoke(new Action(() =>
                {
                    this.MainPumpOne.ContentDown = "1#主泵";
                }));
                MainPumpOneCheck = false;
            }
            bMainPumpOne = GlobalData.Instance.da["770b3"].Value.Boolean;
            if (this.bMainPumpTwo != GlobalData.Instance.da["770b5"].Value.Boolean && this.MainPumpTwoCheck)
            {
                this.MainPumpOne.Dispatcher.Invoke(new Action(() =>
                {
                    this.MainPumpTwo.ContentDown = "2#主泵";
                }));
                MainPumpTwoCheck = false;
            }
            bMainPumpTwo = GlobalData.Instance.da["770b5"].Value.Boolean;
        }
        #endregion

        #region 猫道
        private void CatVariableBinding()
        {
            try
            {
                this.CatControlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b0"], Mode = BindingMode.OneWay });
                this.CatMainPumpOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b1"], Mode = BindingMode.OneWay, Converter = new CheckedIsFalseConverter() });
                this.CatMainPumpTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b3"], Mode = BindingMode.OneWay, Converter = new CheckedIsFalseConverter() });
                this.CatLeftOrRight.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["706b1"], Mode = BindingMode.OneWay });
                this.CatInOrOut.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["706b3"], Mode = BindingMode.OneWay });

                this.cbDRSafeLimit.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["505b7"], Mode = BindingMode.OneWay });
                this.cbIgnoreLimit.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b6"], Mode = BindingMode.OneWay });
                this.cbSelectPipe.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b7"], Mode = BindingMode.OneWay });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 控制模式本地/司钻
        /// </summary>
        private void btn_CatcontrolModel(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 8, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 1#泵启动/停止
        /// </summary>
        private void btn_CatMainPumpOne(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 5, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 2#泵启动/停止
        /// </summary>
        private void btn_CatMainPumpTwo(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 5, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左右选择
        /// </summary>
        private void btn_CatLeftOrRight(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.CatLeftOrRight.IsChecked) //当前左
            {
                byteToSend = new byte[10] { 80, 48, 6, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前右
            {
                byteToSend = new byte[10] { 80, 48, 6, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 内外选择
        /// </summary>
        private void btn_CatInOrLeft(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.CatLeftOrRight.IsChecked) //当前内
            {
                byteToSend = new byte[10] { 80, 48, 7, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前外
            {
                byteToSend = new byte[10] { 80, 48, 7, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 钻台面安全限制解除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDRSafeLimit_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked.Value)
            {
                byte[] byteToSend = new byte[10] { 80, 48, 9, 0, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("确认解除钻台面对猫道得安全设置?", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    byte[] byteToSend = new byte[10] { 80, 48, 9, 0, 1, 0, 0, 0, 0, 0 };
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
        }

        /// <summary>
        /// 忽略限制
        /// </summary>
        private void cbIgnoreLimit_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 2, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 选择套管
        /// </summary>
        private void cbSelectPipe_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 3, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion
    }
}
