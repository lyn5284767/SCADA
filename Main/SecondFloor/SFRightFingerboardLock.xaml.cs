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
    public partial class SFRightFingerboardLock : UserControl
    {
        private static SFRightFingerboardLock _instance = null;
        private static readonly object syncRoot = new object();

        public static SFRightFingerboardLock Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFRightFingerboardLock();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFRightFingerboardLock()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFRightFingerboardLock_Loaded;
        }

        private void SFRightFingerboardLock_Loaded(object sender, RoutedEventArgs e)
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
                this.rightOneFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B4b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightOneFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B4b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightTwoFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B4b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightTwoFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B4b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightThreeFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B4b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightThreeFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B4b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFourFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B4b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFourFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B4b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFiveFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B5b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFiveFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B5b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightSixFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B5b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightSixFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B5b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightSevenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B5b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightSevenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B5b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightEightFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B5b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightEightFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B5b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightNineFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B6b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightNineFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B6b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightTenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B6b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightTenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B6b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightElevenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B6b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightElevenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B6b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightTwelveFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B6b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightTwelveFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B6b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightThirteenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B7b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightThirteenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B7b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFourteenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B7b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFourteenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B7b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFifteenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B7b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFifteenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B7b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightSixteenFingerLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B7b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightSixteenFingerLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["104N23B7b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
