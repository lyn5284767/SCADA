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
    /// SFIOOne.xaml 的交互逻辑
    /// </summary>
    public partial class SFIOFour : UserControl
    {
        private static SFIOFour _instance = null;
        private static readonly object syncRoot = new object();

        public static SFIOFour Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFIOFour();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFIOFour()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFIOFour_Loaded;
        }

        private void SFIOFour_Loaded(object sender, RoutedEventArgs e)
        {
            //VariableBinding();
        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                #region 506位
                this.LocalOrIntegrateModel.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HookUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HookDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HookStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HookEncodeStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HandLockHookSetting.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HandLockTopDriveSetting.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorCloseSignalShield.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["506b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #endregion

                #region 507位
                this.OverloadIndication.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["507b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorClosed.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["507b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ConfirmationIndicator.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["507b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.BuzzerAlarm.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["507b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ManipulatorLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["507b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorOilSource.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["507b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorLevelOutput.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["507b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorVerticalOutput.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["507b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #endregion

                #region 508位
                //this.ElevatorWithPole.SetBinding(SymbolMapping.LampTypeProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["164ElevatorStatus"], Mode = BindingMode.OneWay, Converter = new ElevatorWithPoleConverter() });
                this.ElevatorCloseOutput.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["508b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorOpenOutput.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["508b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.KawaCloseOutput.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["508b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.KawaOpenOutput.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["508b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HandMaintainTips.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["508b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.OperFloorHeat.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["508b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.SecondFloorHeat.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["508b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HandRepairModel.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["508b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #endregion
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
