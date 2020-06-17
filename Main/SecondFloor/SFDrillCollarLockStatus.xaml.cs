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
    /// SFDrillCollarLockStatus.xaml 的交互逻辑
    /// </summary>
    public partial class SFLeftFingerboardLock : UserControl
    {
        private static SFLeftFingerboardLock _instance = null;
        private static readonly object syncRoot = new object();

        public static SFLeftFingerboardLock Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFLeftFingerboardLock();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFLeftFingerboardLock()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFLeftFingerboardLock_Loaded;
        }

        private void SFLeftFingerboardLock_Loaded(object sender, RoutedEventArgs e)
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
                this.leftOneFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftOneFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftTwoFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftTwoFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftThreeFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftThreeFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFourFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFourFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFiveFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["108b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFiveFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["108b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftSixFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["108b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftSixFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["108b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftSevenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["108b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftSevenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["108b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftEightFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["108b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftEightFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["108b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftNineFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B2b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftNineFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B2b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftTenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B2b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftTenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B2b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftElevenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B2b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftElevenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B2b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftTwelveFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B2b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftTwelveFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B2b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftThirteenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B3b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftThirteenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B3b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFourteenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B3b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFourteenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B3b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFifteenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B3b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFifteenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B3b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftSixteenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B3b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftSixteenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B3b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
