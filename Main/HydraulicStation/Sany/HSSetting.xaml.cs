using COM.Common;
using ControlLibrary;
using DataService;
using Log;
using Main.HydraulicStation.Sany;
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
    /// HSSetting.xaml 的交互逻辑
    /// </summary>
    public partial class HSSetting : UserControl
    {
        private static HSSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static HSSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new HSSetting();
                        }
                    }
                }
                return _instance;
            }
        }

        System.Threading.Timer timerWarning;
        System.Threading.Timer VariableReBinding;
        bool bMainPumpOne = false;
        bool MainPumpOneCheck = false;
        bool bMainPumpTwo = false;
        bool MainPumpTwoCheck = false;
        public HSSetting()
        {
            InitializeComponent();
            VariableBinding();
            VariableReBinding = new System.Threading.Timer(new TimerCallback(VariableTimer), null, Timeout.Infinite, 500);
            VariableReBinding.Change(0, 500);
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 500);//改成50ms 的时钟
            InitAlarmKey();
        }

        private void VariableBinding()
        {
            try
            {
                #region 处理与mouse与click事件冲突
                //btnLeftCatHeadReach.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnLeftCatReach_Click), true);
                //btnLeftCatHeadReach.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnLeftCatHeadReach_MouseUp), true);
                //btnLeftCatHeadRetract.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnLeftCatRetract_Click), true);
                //btnLeftCatHeadRetract.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnLeftCatHeadRetract_MouseUp), true);
                //btnRightCatHeadReach.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnRightCatReach_Click), true);
                //btnRightCatHeadReach.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnRightCatHeadReach_MouseUp), true);
                //btnRightCatHeadRetract.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnRightCatRetract_Click), true);
                //btnRightCatHeadRetract.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnRightCatHeadRetract_MouseUp), true);
                //btnRotateCatHeadLeft.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(btnRotateCatHeadLeft_Click), true);
                //btnRotateCatHeadLeft.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnRotateCatHeadLeft_MouseUp), true);
                //btnRotateCatHeadRight.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(btnRotateCatHeadRight_Click), true);
                //btnRotateCatHeadRight.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnRotateCatHeadRight_MouseUp), true);
                #endregion

                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay});
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
                this.constantVoltagePump.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b7"], Mode = BindingMode.OneWay });
                this.dissipateHeat.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b1"], Mode = BindingMode.OneWay });
                this.hot.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b3"], Mode = BindingMode.OneWay });
                this.Arm.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["794b1"], Mode = BindingMode.OneWay });
                this.SlurryBox.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["794b3"], Mode = BindingMode.OneWay });

                this.cbHotHand.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b0"], Mode = BindingMode.OneWay });
                this.cbHotAuto.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b1"], Mode = BindingMode.OneWay });
                this.cbDisHotHand.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b2"], Mode = BindingMode.OneWay });
                this.cbDisHotAuto.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b3"], Mode = BindingMode.OneWay });
                this.cbFanHand.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b6"], Mode = BindingMode.OneWay });
                this.cbFanAuto.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b7"], Mode = BindingMode.OneWay });

                //this.Iron.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b1"], Mode = BindingMode.OneWay });
                //this.Tongs.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b3"], Mode = BindingMode.OneWay });
                //this.DF.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b3"], Mode = BindingMode.OneWay });
                //this.BufferArm.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b7"], Mode = BindingMode.OneWay });

                //this.btnLeftCatHeadRetract.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b5"], Mode = BindingMode.OneWay,Converter = new BtnColorCoverter() });
                //this.btnLeftCatHeadClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnLeftCatHeadReach.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
               
                //this.btnRightCatHeadRetract.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnRightCatHeadClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnRightCatHeadReach.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                //this.btnIron.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnIronCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnIronCloseMultiBind = new MultiBinding();
                btnIronCloseMultiBind.Converter = btnIronCloseMultiConverter;
                btnIronCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["768b1"], Mode = BindingMode.OneWay });
                btnIronCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["768b3"], Mode = BindingMode.OneWay });
                btnIronCloseMultiBind.NotifyOnSourceUpdated = true;
                //this.btnIronClose.SetBinding(Button.BackgroundProperty, btnIronCloseMultiBind);
                //this.btnTong.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                //this.btnDF.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnDFCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnDFCloseMultiBind = new MultiBinding();
                btnDFCloseMultiBind.Converter = btnDFCloseMultiConverter;
                btnDFCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b3"], Mode = BindingMode.OneWay });
                btnDFCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b5"], Mode = BindingMode.OneWay });
                btnDFCloseMultiBind.NotifyOnSourceUpdated = true;
                //this.btnDFClose.SetBinding(Button.BackgroundProperty, btnDFCloseMultiBind);
                //this.btnSpaceThree.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                //this.btnWellBuffer.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnWellBufferCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnWellBufferCloseMultiBind = new MultiBinding();
                btnWellBufferCloseMultiBind.Converter = btnWellBufferCloseMultiConverter;
                btnWellBufferCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b7"], Mode = BindingMode.OneWay });
                btnWellBufferCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["770b1"], Mode = BindingMode.OneWay });
                btnWellBufferCloseMultiBind.NotifyOnSourceUpdated = true;
                //this.btnWellBufferClose.SetBinding(Button.BackgroundProperty, btnWellBufferCloseMultiBind);
                //this.btnSpaceFour.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                //this.btnTurnMainOne.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnTurnMainTwo.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnMonitorOneGetOil.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnMonitorTwoGetOil.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnFilterReplace.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnOilReplace.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnOilLeakage.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["773b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
            
                //this.tbLeftCatHead.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["CatPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.tbRightCatHead.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["CatPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.tbIronTongs.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IRPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.tbDFSpThree.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["FPPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

                //this.btnElevatorOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnElevatorClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                ////this.tbMainPumpUnload.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["MPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.btnCraneOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnCraneClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                ////this.tbKavaUnload.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SlipPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.btnWellMoveOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnWellMoveClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.tbWellMove.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["TPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.btnSpareOneOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnSpareOneClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnIronOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnIronClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.tbIron.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IRPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.btnTongsOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnTongsClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.tbTongs.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["TongPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.btnDROpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnDRClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.tbDR.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["FPPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.btnArmOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnArmClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.tbArm.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["FPPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.btnLeftCarOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnLeftCarClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnRightCarOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnRightCarClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnRotateCatHeadLeft.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["794b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnRotateCatHeadStop.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["794b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnRotateCatHeadRight.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["794b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.tbCancelLevel.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["795b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.tbCancelTmp.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["795b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.tbSysTurnZero.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.tbAlarmClear.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["772b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                #region 设备状态
                this.cpbMainPump.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["MPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbKavaPump.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SlipPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbLSPump.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["LSPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbMovePump.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["TPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbIronPump.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IRPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbTongPump.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["TongPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbCatHeadPump.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["CatPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.cpbFPPump.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["FPPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.wpbOil.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["OilTemAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.wpbHeight.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["OilLevelAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

                #endregion
            }
            catch (Exception ex)
            {
                Log4Net.AddLog(ex.StackTrace, InfoLevel.ERROR);
            }
        }
        List<AlarmInfo> alarmList = new List<AlarmInfo>();
        /// <summary>
        /// 绑定告警变量
        /// </summary>
        private void InitAlarmKey()
        {
            alarmList.Add(new AlarmInfo() { TagName = "771b7", Description = "液压站急停", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "774b0", Description = "液压油高温报警，请及时降温", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "774b1", Description = "液压油高温预警，请及时降温", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "774b2", Description = "液压油温度过低，请开启加热", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "774b3", Description = "低液位预警，请及时加注液压油", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "774b4", Description = "低液位报警，请及时加注液压油", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "774b5", Description = "液压位异常降低，请检测漏油", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "774b6", Description = "加热效果异常，请检测加热器", NowType = 0, NeedCheck = true });

            alarmList.Add(new AlarmInfo() { TagName = "775b0", Description = "主泵1已连续运行500小时，请切换主泵2使用", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "775b1", Description = "主泵2已连续运行500小时，请切换主泵1使用", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "775b2", Description = "主电机1已连续运行600小时，请加注黄油", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "775b3", Description = "主电机2已连续运行600小时，请加注黄油", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "775b4", Description = "距上次更换滤芯已经大于2000小时，请更换滤芯", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "775b5", Description = "距上次更换液压油已经大于2000小时，请更换液压油", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "775b6", Description = "油位下降异常，请检查是否漏油", NowType = 0, NeedCheck = true });
            // 2021.02.02新增
            alarmList.Add(new AlarmInfo() { TagName = "773b7", Description = "主电机1过载，需消除复位", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "774b7", Description = "主电机2过载，需消除复位", NowType = 0, NeedCheck = true });

            alarmList.Add(new AlarmInfo() { TagName = "792b0", Description = "司钻房通信故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "792b1", Description = "分阀箱通信故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "792b2", Description = "卡瓦压力传感器故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "792b3", Description = "LS压力传感器故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "792b4", Description = "平移压力传感器故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "792b5", Description = "油温传感器故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "792b6", Description = "液位传感器故障", NowType = 0, NeedCheck = true });

            alarmList.Add(new AlarmInfo() { TagName = "793b0", Description = "铁钻工压力传感器故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "793b1", Description = "大钳压力传感器故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "793b2", Description = "钻台面压力传感器故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "793b3", Description = "猫道压力传感器故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "793b4", Description = "主压力传感器故障", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "793b5", Description = "恒压泵未合闸", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "793b6", Description = "散热泵未合闸", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "793b7", Description = "加热泵未合闸", NowType = 0, NeedCheck = true });
        }

        /// <summary>
        /// 监控告警状态
        /// </summary>
        private void MonitorAlarmStatus()
        {
            foreach (AlarmInfo info in alarmList)
            {
                if (GlobalData.Instance.da[info.TagName] != null)
                {
                    if (GlobalData.Instance.da[info.TagName].Value.Boolean)//有报警
                    {
                        if (info.NowType == 0) info.NowType = 1;// 前状态为未告警
                    }
                    else
                    {
                        info.NowType = 0;
                    }
                }
            }
        }
        //司钻/本地控制
        private void btn_controlModel(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //byteToSend = new byte[10] { 0, 19, 1, 9, 0, 0, 0, 0, 0, 0 };// 疑问，所有切换都是这个协议？
            //GlobalData.Instance.da.SendBytes(byteToSend);
            byte[] byteToSend;
            if (this.controlModel.IsChecked) //当前本地
            {
                byteToSend = new byte[10] { 0, 19, 52, 0, 1, 0, 0, 0, 0, 0 };
            }
            else//当前司钻状态
            {
                byteToSend = new byte[10] { 0, 19, 53, 0, 1, 0, 0, 0, 0, 0 };
            }
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
                //byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
                byteToSend = new byte[10] { 0, 19, 42, 0, 1, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                //byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
                byteToSend = new byte[10] { 0, 19, 43, 0, 1, 0, 0, 0, 0, 0 };
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
                //byteToSend = new byte[10] { 0, 19, 3, 3, 0, 0, 0, 0, 0, 0 };
                byteToSend = new byte[10] { 0, 19, 44, 0, 1, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                //byteToSend = new byte[10] { 0, 19, 3, 4, 0, 0, 0, 0, 0, 0 };
                byteToSend = new byte[10] { 0, 19, 45, 0, 1, 0, 0, 0, 0, 0 };
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
        private void btn_constantVoltagePump(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.constantVoltagePump.IsChecked) //当前停止状态
            {
                //byteToSend = new byte[10] { 0, 19, 3, 5, 0, 0, 0, 0, 0, 0 };
                byteToSend = new byte[10] { 0, 19, 46, 0, 1, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                //byteToSend = new byte[10] { 0, 19, 3, 6, 0, 0, 0, 0, 0, 0 };
                byteToSend = new byte[10] { 0, 19, 47, 0, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 散热泵启动/停止
        /// </summary>
        private void btn_dissipateHeat(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.dissipateHeat.IsChecked) //当前停止状态
            {
                //byteToSend = new byte[10] { 0, 19, 3, 7, 0, 0, 0, 0, 0, 0 };
                byteToSend = new byte[10] { 0, 19, 48, 0, 1, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                //byteToSend = new byte[10] { 0, 19, 3, 8, 0, 0, 0, 0, 0, 0 };
                byteToSend = new byte[10] { 0, 19, 49, 0, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 加热器启动/停止
        /// </summary>
        private void btn_hot(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.hot.IsChecked) //当前停止状态
            {
                //byteToSend = new byte[10] { 0, 19, 3, 9, 0, 0, 0, 0, 0, 0 };
                byteToSend = new byte[10] { 0, 19, 50, 0, 1, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                //byteToSend = new byte[10] { 0, 19, 3, 10, 0, 0, 0, 0, 0, 0 };
                byteToSend = new byte[10] { 0, 19, 51, 0, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 加热器手动设置
        /// </summary>
        private void cbHotHand_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 1, 1, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 1, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 加热器自动设置
        /// </summary>
        private void cbHotAuto_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 1, 2, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 2, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 散热器手动设置
        /// </summary>
        private void cbDisHotHand_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 1, 3, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 3, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 散热器自动设置
        /// </summary>
        private void cbDisHotAuto_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 1, 4, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 4, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 风扇手动设置
        /// </summary>
        private void cbFanHand_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 1, 7, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 7, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 风扇自动设置
        /// </summary>
        private void cbFanuAuto_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 1, 8, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 8, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 系统复位
        /// </summary>
        private void BtnSysTurnZero_Click(object sender, RoutedEventArgs e)
        {
            //byte[] byteToSend = new byte[10] { 0, 19, 1, 5, 0, 0, 0, 0, 0, 0 };
            byte[] byteToSend = new byte[10] { 0, 19, 5, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 报警消除
        /// </summary>
        private void BtnAlarmClear_Click(object sender, RoutedEventArgs e)
        {
            //byte[] byteToSend = new byte[10] { 0, 19, 1, 6, 0, 0, 0, 0, 0, 0 };
            byte[] byteToSend = new byte[10] { 0, 19, 6, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 已切换到主泵1运行
        /// </summary>
        private void BtnTurnMainOne_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认已切换到主泵1运行？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 2, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 36, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 已切换到主泵2运行
        /// </summary>
        private void BtnTurnMainTwo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认已切换到主泵2运行？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 1, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 35, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 主电机1加注黄油
        /// </summary>
        private void BtnMonitorOneGetOil_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认主电机1已加注黄油？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 3, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 37, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 主电机2加注黄油
        /// </summary>
        private void BtnMonitorTwoGetOil_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认主电机2已加注黄油？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 4, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 38, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 滤芯已更换
        /// </summary>
        private void BtnFilterReplace_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认滤芯已更换？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 5, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 39, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 液压油已更换
        /// </summary>
        private void BtnOilReplace_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认液压油已更换？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 6, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 40, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 漏油确认
        /// </summary>
        private void BtnOilLeakage_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确认无漏油？", "提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 2, 7, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 41, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        bool leftCatReach = false;
        bool leftCatRetract = false;
        bool rightCatReach = false;
        bool rightCatRetract = false;
        bool rotateCatLeft = false;
        bool rotateCatRight = false;
        /// <summary>
        /// 循环发送命令
        /// </summary>
        private void SendByCycle()
        {
            if (leftCatReach) // 左猫头伸
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 4, 4, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 21, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            if (leftCatRetract) // 左猫头缩
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 4, 5, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 21, 0, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            if (rightCatReach) // 右猫头伸
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 4, 7, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 22, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            if (rightCatRetract) // 右猫头缩
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 4, 8, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 22, 0, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }

            if (rotateCatLeft) // 旋转猫头左转
            {
                byte[] byteToSend = new byte[10] { 0, 19, 23, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            if (rotateCatRight) // 旋转猫头右转
            {
                byte[] byteToSend = new byte[10] { 0, 19, 23, 0, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 左猫头伸
        /// </summary>
        private void BtnLeftCatReach_Click(object sender, RoutedEventArgs e)
        {
            leftCatReach = true;
        }

        //private void BtnLeftCatReach_Click(object sender, MouseButtonEventArgs e)
        //{
        //    byte[] byteToSend = new byte[10] { 0, 19, 4, 4, 0, 0, 0, 0, 0, 0 };
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        /// <summary>
        /// 左猫头伸-按键抬起关闭
        /// </summary>
        private void btnLeftCatHeadReach_MouseUp(object sender, RoutedEventArgs e)
        {
            leftCatReach = false;
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 6, 0, 0, 0, 0, 0, 0 };
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左猫头缩
        /// </summary>
        private void BtnLeftCatRetract_Click(object sender, RoutedEventArgs e)
        {
            leftCatRetract = true;
        }
        /// <summary>
        /// 左猫头缩-按键抬起关闭
        /// </summary>
        private void btnLeftCatHeadRetract_MouseUp(object sender, RoutedEventArgs e)
        {
            leftCatRetract = false;
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 6, 0, 0, 0, 0, 0, 0 };
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左猫头关
        /// </summary>
        private void BtnLeftCatClose_Click(object sender, RoutedEventArgs e)
        {
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 5, 0, 0, 0, 0, 0, 0 };
            byte[] byteToSend = new byte[10] { 0, 19, 21, 0, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头伸
        /// </summary>
        private void BtnRightCatReach_Click(object sender, RoutedEventArgs e)
        {
            rightCatReach = true;
        }
        /// <summary>
        /// 右猫头伸-抬起关闭
        /// </summary>
        private void btnRightCatHeadReach_MouseUp(object sender, RoutedEventArgs e)
        {
            rightCatReach = false;
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 9, 0, 0, 0, 0, 0, 0 };
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头缩
        /// </summary>
        private void BtnRightCatRetract_Click(object sender, RoutedEventArgs e)
        {
            rightCatRetract = true;
        }
        /// <summary>
        /// 右猫头缩-按键抬起关闭
        /// </summary>
        private void btnRightCatHeadRetract_MouseUp(object sender, RoutedEventArgs e)
        {
            rightCatRetract = false;
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 9, 0, 0, 0, 0, 0, 0 };
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头关
        /// </summary>
        private void BtnRightCatClose_Click(object sender, RoutedEventArgs e)
        {
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 9, 0, 0, 0, 0, 0, 0 };
            byte[] byteToSend = new byte[10] { 0, 19, 22, 0, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Iron(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btn_Tongs(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btn_DF(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 9, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btn_BufferArm(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 10, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工打开
        /// </summary>
        private void BtnIron_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 大钳打开
        /// </summary>
        private void BtnTong_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钻台面打开
        /// </summary>
        private void BtnDF_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 10, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钻台面/备用3关闭
        /// </summary>
        private void BtnDFClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 12, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 备用3打开
        /// </summary>
        private void BtnSpaceThree_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 11, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井架缓冲打开
        /// </summary>
        private void BtnWellBuffer_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 13, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井架缓冲/备用4关闭
        /// </summary>
        private void BtnWellBufferCloseClick(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 15, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 备用4打开
        /// </summary>
        private void BtnSpaceFour_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 4, 14, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        bool colorTag = false;
        private int iTimeCnt = 0;//用来为时钟计数的变量
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    SendByCycle();
                    //colorTag = !colorTag;
                    //var bc = new BrushConverter();
                    //if (GlobalData.Instance.da["775b1"].Value.Boolean)
                    //{
                    //    if (colorTag) this.btnTurnMainOne.Background = (Brush)bc.ConvertFrom("#F73C14");
                    //    else this.btnTurnMainOne.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //}
                    //else
                    //{
                    //    this.btnTurnMainOne.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //}
                    //if (GlobalData.Instance.da["775b0"].Value.Boolean)
                    //{
                    //    if (colorTag) this.btnTurnMainTwo.Background = (Brush)bc.ConvertFrom("#F73C14");
                    //    else this.btnTurnMainTwo.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //}
                    //else
                    //{
                    //    this.btnTurnMainTwo.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //}
                    //if (GlobalData.Instance.da["775b2"].Value.Boolean)
                    //{
                    //    if (colorTag) this.btnMonitorOneGetOil.Background = (Brush)bc.ConvertFrom("#F73C14");
                    //    else this.btnMonitorOneGetOil.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //}
                    //else
                    //{
                    //    this.btnMonitorOneGetOil.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //}
                    //if (GlobalData.Instance.da["775b3"].Value.Boolean)
                    //{
                    //    if (colorTag) this.btnMonitorTwoGetOil.Background = (Brush)bc.ConvertFrom("#F73C14");
                    //    else this.btnMonitorTwoGetOil.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //}
                    //else this.btnMonitorTwoGetOil.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //if (GlobalData.Instance.da["775b4"].Value.Boolean)
                    //{
                    //    if (colorTag) this.btnFilterReplace.Background = (Brush)bc.ConvertFrom("#F73C14");
                    //    else this.btnFilterReplace.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //}
                    //else this.btnFilterReplace.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //if (GlobalData.Instance.da["775b5"].Value.Boolean)
                    //{
                    //    if (colorTag) this.btnOilReplace.Background = (Brush)bc.ConvertFrom("#F73C14");
                    //    else this.btnOilReplace.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //}
                    //else this.btnOilReplace.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //if (GlobalData.Instance.da["775b6"].Value.Boolean)
                    //{
                    //    if (colorTag) this.btnOilLeakage.Background = (Brush)bc.ConvertFrom("#F73C14");
                    //    else this.btnOilLeakage.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    //}
                    //else this.btnOilLeakage.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    #region 告警
                    if (iTimeCnt % 10 == 0)
                    {
                        this.tbAlarm.FontSize = 18;
                        this.tbAlarm.Visibility = Visibility.Visible;
                        // 告警列表!=0则有告警 
                        if (alarmList.Where(w => w.NowType != 0).Count() > 0)
                        {

                            // 有告警且全部显示完成
                            if (this.alarmList.Where(w => w.NowType == 1).Count() == 0)
                            {
                                this.alarmList.Where(w => w.NowType == 2).ToList().ForEach(w => w.NowType = 1);
                            }
                            AlarmInfo tmp = this.alarmList.Where(w => w.NowType == 1).FirstOrDefault();
                            if (tmp != null)
                            {
                                this.tbAlarm.FontSize = 18;
                                this.tbAlarm.Text = tmp.Description;
                                tmp.NowType = 2;
                            }
                        }
                        else
                        {
                            this.tbAlarm.Text = "暂无告警";
                        }
                    }
                    else
                    {
                        this.tbAlarm.FontSize = 20;
                    }
                    #endregion
                    MonitorAlarmStatus();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void VariableTimer(object value)
        {
            if (GlobalData.Instance.da["770b3"] == null || GlobalData.Instance.da["770b5"] == null)
            {
                Log.Log4Net.AddLog("770b3或770b5为空");
                return;
            }
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
        /// <summary>
        /// 吊卡打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnElevatorOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 9, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 吊卡关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnElevatorClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 10, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 吊机打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCraneOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 11, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 吊机关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCraneClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 12, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井架平移打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWellMoveOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 13, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井架平移关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWellMoveClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 14, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 备用油源1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSpareOneOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 15, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btnSpareOneClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 16, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工油源打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIronOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 17, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 铁钻工关闭
        /// </summary>
        private void BtnIronClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 18, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 大钳打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTongsOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 19, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 大钳关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTongsClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 20, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钻台面打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDROpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 27, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btnDRClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 28, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 备用油源2打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeftCarOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 29, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 备用油源2关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeftCarClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 30, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 缓冲臂打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnArmOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 31, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 缓冲臂关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnArmClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 32, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 备用油源3打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRightCarOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 33, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 备用油源3关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRightCarClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 34, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 取消液位限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelWaterLevel_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 100, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 取消温度限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelTmp_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 101, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 泥浆盒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlurryBox_hot(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.SlurryBox.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 56, 0, 1, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 57, 0, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 旋转猫头停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRotateCatHeadStop_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 23, 0, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 旋转猫头左转-按下
        /// </summary>
        private void btnRotateCatHeadLeft_Click(object sender, RoutedEventArgs e)
        {
            rotateCatLeft = true;
        }
        /// <summary>
        /// 旋转猫头左转-抬起
        /// </summary>
        private void btnRotateCatHeadLeft_MouseUp(object sender, RoutedEventArgs e)
        {
            rotateCatLeft = false;
        }

        /// <summary>
        /// 旋转猫头左转-按下
        /// </summary>
        private void btnRotateCatHeadRight_Click(object sender, RoutedEventArgs e)
        {
            rotateCatRight = true;
        }
        /// <summary>
        /// 旋转猫头左转-抬起
        /// </summary>
        private void btnRotateCatHeadRight_MouseUp(object sender, RoutedEventArgs e)
        {
            rotateCatRight = false;
        }
        /// <summary>
        /// 缓冲臂启停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Arm_Click(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.Arm.IsChecked) //当前停止状态
            {
                byteToSend = new byte[10] { 0, 19, 54, 0, 1, 0, 0, 0, 0, 0 };
            }
            else//当前启动状态
            {
                byteToSend = new byte[10] { 0, 19, 55, 0, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btnOilOne_Click(object sender, RoutedEventArgs e)
        {
            OilOneWindow oneWindow = new OilOneWindow();
            oneWindow.ShowDialog();
        }

        private void btnOilTwo_Click(object sender, RoutedEventArgs e)
        {
            OilTwoWindow oilTwoWindow = new OilTwoWindow();
            oilTwoWindow.ShowDialog();
        }

        private void Maintain_Click(object sender, MouseButtonEventArgs e)
        {
            MaintainWindow maintainWindow = new MaintainWindow();
            maintainWindow.ShowDialog();
        }
    }
}
