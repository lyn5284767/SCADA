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
    public partial class SFIOThree : UserControl
    {
        private static SFIOThree _instance = null;
        private static readonly object syncRoot = new object();

        public static SFIOThree Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFIOThree();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFIOThree()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFIOThree_Loaded;
        }

        private void SFIOThree_Loaded(object sender, RoutedEventArgs e)
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
                this.EmergencyStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["129b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.FingerClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["129b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.FingerOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["129b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.OperateModeAutoHand.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["129b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.WorkModeRowDelivery.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["129b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.FingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["129b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.FingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["129b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["129b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["130b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.StartConfirm.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["130b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.GraspClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["130b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.GraspOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["130b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.KavaClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["130b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.KavaOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["130b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorLevel.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["130b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorVertical.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["130b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #region 503位
                this.ElevatorDrillResponse.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["503b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElevatorCloseSignal.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["503b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.FingerEnable.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["503b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HandStretchEnable.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["503b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.HandWellheadEnable.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["503b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.DrillFloorOrSecondFloor.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["503b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smKAVA.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["575b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                #endregion
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
