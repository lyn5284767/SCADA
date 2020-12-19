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

namespace Main.DrillFloor
{
    /// <summary>
    /// DRDeviceStatus.xaml 的交互逻辑
    /// </summary>
    public partial class DRDeviceStatus : UserControl
    {

        private static DRDeviceStatus _instance = null;
        private static readonly object syncRoot = new object();

        public static DRDeviceStatus Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DRDeviceStatus();
                        }
                    }
                }
                return _instance;
            }
        }
        private bool bCommunicationCheck = false; // 是否有中断标志
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool tmpStatus = false; // 控制台心跳临时存储状态

        public static readonly DependencyProperty CommunicationProperty = DependencyProperty.Register("Communication", typeof(byte), typeof(DRDeviceStatus), new PropertyMetadata((byte)0));//1代表通讯正常，2 代表人机界面--操作台通讯异常 3 操作台--二层台通讯异常
        public byte Communication
        {
            get { return (byte)GetValue(CommunicationProperty); }
            set { SetValue(CommunicationProperty, value); }
        }

        public DRDeviceStatus()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += DRDeviceStatus_Loaded;
        }

        System.Timers.Timer pageChange;
        int sendCount = 0;
        private void DRDeviceStatus_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 36 };
            GlobalData.Instance.da.SendBytes(byteToSend);

            GlobalData.Instance.DRNowPage = "deviceStatus";
            sendCount = 0;
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
            if (GlobalData.Instance.da["drPageNum"].Value.Byte == 36 || sendCount>5 || GlobalData.Instance.DRNowPage != "deviceStatus")
            {
                pageChange.Stop();
            }
            else
            {
                byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 36 };
                GlobalData.Instance.da.SendBytes(data);
            }
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.opModel_EquiptStatusPage.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.workModel_EquiptStatusPage.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drworkModel"], Mode = BindingMode.OneWay, Converter = new DRWorkModelConverter() });
                this.workTime_EquipStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["sysDurTime"], Mode = BindingMode.OneWay, Converter = new UnitDivide10Converter() });
                this.drillDownCount_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["DrillDownTimes"], Mode = BindingMode.OneWay });
                this.drillUpCount_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["DrillUpTimes"], Mode = BindingMode.OneWay });

                this.carTurnZore.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.carFeeling.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["327b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.carStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDriverOneStatus"], Mode = BindingMode.OneWay, Converter = new MotorStatusWarnInfoConverter() });
                this.carLocation.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["drCarPulse"], Mode = BindingMode.OneWay });
                this.carError.SetBinding(TextBlock.TextProperty, new Binding("UShortTag") { Source = GlobalData.Instance.da["drDriverOneError"], Mode = BindingMode.OneWay });

                this.armFeeling.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["327b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.armStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDriverTwoStatus"], Mode = BindingMode.OneWay, Converter = new MotorStatusWarnInfoConverter() });
                this.armLocation.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["drHandPulse"], Mode = BindingMode.OneWay });
                this.armError.SetBinding(TextBlock.TextProperty, new Binding("UShortTag") { Source = GlobalData.Instance.da["drDriverTwoError"], Mode = BindingMode.OneWay });

                this.rotateTurnZore.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["324b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rotateFeeling.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["327b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rotateStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDriverThreeStatus"], Mode = BindingMode.OneWay, Converter = new MotorStatusWarnInfoConverter() });
                this.rotateLocation.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["drRotatePulse"], Mode = BindingMode.OneWay });
                this.rotateError.SetBinding(TextBlock.TextProperty, new Binding("UShortTag") { Source = GlobalData.Instance.da["drDriverThreeError"], Mode = BindingMode.OneWay });

                this.gridFeeling.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["327b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.gridStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drgripStatus"], Mode = BindingMode.OneWay, Converter = new GripConverter() });
                this.gridLocation.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["gridSample"], Mode = BindingMode.OneWay });
                GridErrorMultiConverter gridErrorMultiConverter = new GridErrorMultiConverter();
                MultiBinding gridErrorMultiBind = new MultiBinding();
                gridErrorMultiBind.Converter = gridErrorMultiConverter;
                gridErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["337b0"], Mode = BindingMode.OneWay });
                gridErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["337b1"], Mode = BindingMode.OneWay });
                gridErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["337b2"], Mode = BindingMode.OneWay });
                gridErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["337b3"], Mode = BindingMode.OneWay });
                gridErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["337b4"], Mode = BindingMode.OneWay });
                gridErrorMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["337b5"], Mode = BindingMode.OneWay });
                gridErrorMultiBind.NotifyOnSourceUpdated = true;
                this.gridError.SetBinding(TextBlock.TextProperty, gridErrorMultiBind);
                #region 安全信息
                this.pliersForbidOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["333b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ForbidMoveToWell.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["333b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ForbidHandRetract.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["333b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.WellLocation.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["334b1"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
                this.ForbidToCat.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["333b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.CatLocked.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["334b4"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
                this.smLink.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["334b7"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });

                #endregion
                this.handleX.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drX"], Mode = BindingMode.OneWay });
                this.handleY.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drY"], Mode = BindingMode.OneWay });
                this.btnLeft.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["427b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.btnRight.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["427b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.btnLeftBack.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["427b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.btnRightBack.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["427b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.btnEnable.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["427b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                this.gridOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["326b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.gridClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["326b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.carTurnLeft.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["326b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.carTurnRight.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["326b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HotSign.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["326b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.emergencyStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["327b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.confirmSign.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["326b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.carOverload.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["327b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rotateOverload.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["327b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.carMoveLimit.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["427b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                this.gridOpenOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["329b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.gridCloseOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["329b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.carTurnLeftOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["329b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.carTurnRightOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["329b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.cabinetHot.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["329b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.errorTips.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["329b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.confirmSignOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["329b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.mainValve.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["329b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.sysOil.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["457b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.stoControl.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["333b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                this.armReachElectIn.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["handReach"], Mode = BindingMode.OneWay });
                this.armReachElectOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drHandReachFeedback"], Mode = BindingMode.OneWay });//疑问
                this.armRetractElectIn.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["handRetract"], Mode = BindingMode.OneWay });
                this.armRetractElectOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drHandRetractFeedback"], Mode = BindingMode.OneWay });// 疑问

                this.raiseOpenOrClose.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["333b7"], Mode = BindingMode.OneWay, Converter = new InterLockingOppConverter() });

                this.cbCarOnOff.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["335b0"], Mode = BindingMode.OneWay});
                this.cbArmOnOff.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["335b1"], Mode = BindingMode.OneWay });
                this.cbRotateOnOff.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["335b2"], Mode = BindingMode.OneWay });
                this.cbGripOnOff.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["335b3"], Mode = BindingMode.OneWay });
                this.cbArmEmbraceOnOff.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["335b5"], Mode = BindingMode.OneWay });
                this.cbRotateEmbraceOnOff.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["335b6"], Mode = BindingMode.OneWay });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 融信数据
        /// </summary>
        private void Communcation()
        {
            #region 通信
            // UDP通信，操作台/二层台心跳都正常，通信正常
            if (GlobalData.Instance.da.NetStatus && !GlobalData.Instance.da["508b6"].Value.Boolean && !GlobalData.Instance.da["508b5"].Value.Boolean)
            {
                Communication = 1;
            }

            //if (!GlobalData.Instance.da.NetStatus) Communication = 0; // UDP建立成功/失败标志

            //操作台控制器心跳
            if (GlobalData.Instance.da["508b5"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 600)
                {
                    Communication = 2;
                }
                if (!bCommunicationCheck && controlHeartTimes > 600)
                {
                    bCommunicationCheck = true;
                }
            }
            else
            {
                this.controlHeartTimes = 0;
            }
            this.tmpStatus = GlobalData.Instance.da["508b6"].Value.Boolean;

            //钻台面心跳 --疑问
            if (GlobalData.Instance.da["508b6"].Value.Boolean)
            {
                Communication = 3;
                if (!bCommunicationCheck)
                {
                    bCommunicationCheck = true;
                }
            }
            else
            {
                bCommunicationCheck = false;
            }
            #endregion
        }
        /// <summary>
        /// 提效开启/关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBraiseOpenOrClose_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.raiseOpenOrClose.IsChecked)
            {
                MessageBoxResult result = MessageBox.Show("确认提效关闭？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 14, 1, 0 });
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("确认提效开启？", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 14, 1, 2 });
                }
                else
                {
                    return;
                }
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 小车回零
        /// </summary>
        private void btnCarTrunZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 13, 1});
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 回转回零
        /// </summary>
        private void btnRotateTrunZero(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 13, 3 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 电机使能
        /// </summary>
        private void btnMontorEnable(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 6, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 清除钻杆
        /// </summary>
        private void btnClearDrill(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认抓手已无钻杆，并清除此状态?", "提示信息", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 10, 1 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 清除报警
        /// </summary>
        private void btnClearError(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 6, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 一键运输-运输模式只操作左边
        /// </summary>
        private void btnroTransprot(object sender, MouseButtonEventArgs e)
        {
            if (GlobalData.Instance.DrillLeftTotal > 0)
            {
                MessageBox.Show("台面有钻杆，无法切换至运输模式！", "提示信息", MessageBoxButton.OK);
                return;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("确定进行回收?", "提示信息", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 1, 7 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
        }
        /// <summary>
        /// 排送杆清零
        /// </summary>
        private void btnClearDrillUpCount(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 22, 13 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 排送杆清零
        /// </summary>
        private void btnClearDrillDownCount(object sender, MouseButtonEventArgs e)
        {
            byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 22, 12 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 小车强制开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbCarOnOff_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.cbCarOnOff.IsChecked)
            {
                byteToSend = new byte[] { 80, 33, 8, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[] { 80, 33, 8, 1, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 手臂强制开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbArmOnOff_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.cbArmOnOff.IsChecked)
            {
                byteToSend = new byte[] { 80, 33, 8, 2, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[] { 80, 33, 8, 2, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 回转强制开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbRotateOnOff_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.cbRotateOnOff.IsChecked)
            {
                byteToSend = new byte[] { 80, 33, 8, 3, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[] { 80, 33, 8, 3, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbArmEmbraceOnOffClicked(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.cbArmEmbraceOnOff.IsChecked)
            {
                byteToSend = new byte[] { 80, 33, 8, 2, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[] { 80, 33, 8, 2, 2, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 回转抱闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbRotateEmbraceOnOff_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.cbRotateEmbraceOnOff.IsChecked)
            {
                byteToSend = new byte[] { 80, 33, 8, 3, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[] { 80, 33, 8, 3, 2, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 抓手强制开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbGripOnOff_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.cbGripOnOff.IsChecked)
            {
                byteToSend = new byte[] { 80, 33, 8, 4, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[] { 80, 33, 8, 4, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

    }
}
