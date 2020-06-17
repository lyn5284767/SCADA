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
    public partial class SFIOOne : UserControl
    {
        private static SFIOOne _instance = null;
        private static readonly object syncRoot = new object();

        public static SFIOOne Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFIOOne();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFIOOne()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFIOOne_Loaded;
        }

        private void SFIOOne_Loaded(object sender, RoutedEventArgs e)
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
                this.FingerBeamLockChangeDirct.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107N23B0b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left1FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B0b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left2FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B0b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left3FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B0b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left4FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B0b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left5FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B0b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left6FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B0b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left7FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B0b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left8FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B0b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left9FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B1b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left10FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B1b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left11FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B1b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left12FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B1b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left13FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B1b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left14FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B1b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.left15FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B1b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                this.right1FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B2b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right2FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B2b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right3FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B2b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right4FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B2b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right5FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B2b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right6FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B2b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right7FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B2b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right8FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B2b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right9FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B3b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right10FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B3b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right11FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B3b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right12FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B3b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right13FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B3b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right14FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B3b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.right15FingerLockAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["106N23B3b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
