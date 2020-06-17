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
    public partial class SFIOTwo : UserControl
    {
        private static SFIOTwo _instance = null;
        private static readonly object syncRoot = new object();

        public static SFIOTwo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFIOTwo();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFIOTwo()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFIOTwo_Loaded;
        }

        private void SFIOTwo_Loaded(object sender, RoutedEventArgs e)
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
                this.FingerReversalAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107N23B0b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.RightFingerAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107N23B0b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.LeftFingerAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107N23B0b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.GraspReversing.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107N23B0b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.GraspAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107N23B0b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.DrillPipeInduction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N23B4b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElectricControlCabinetHeatingZ.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107N23B0b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.ElectricControlCabinetHeatingX.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107N23B0b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.DrillCollarLockTurn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107N23B0b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.MonkeyRoadCommutation.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["144b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.MonkeyRoadAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["144b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.DrillCollarLockCommutation.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["107N23B1b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.LeftRopeAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["144b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.RightRopeAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["144b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                #region 钻铤锁动作信号
                this.Left1DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["142b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Left2DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["142b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Left3DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["142b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Left4DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["142b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Left5DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["142b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Left6DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["142b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Left7DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["142b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Left8DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["142b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                this.Right1DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["143b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Right2DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["143b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Right3DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["143b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Right4DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["143b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Right5DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["143b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Right6DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["143b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Right7DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["143b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.Right8DrillCollarAction.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["143b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #endregion
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
