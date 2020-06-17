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
    public partial class SFDrillCollarLockStatus : UserControl
    {
        private static SFDrillCollarLockStatus _instance = null;
        private static readonly object syncRoot = new object();

        public static SFDrillCollarLockStatus Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFDrillCollarLockStatus();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFDrillCollarLockStatus()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFDrillCollarLockStatus_Loaded;
        }

        private void SFDrillCollarLockStatus_Loaded(object sender, RoutedEventArgs e)
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
                #region 钻铤锁打开/关闭
                this.leftOneDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B0b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftOneDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B0b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftTwoDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B0b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftTwoDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B0b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftThreeDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B0b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftThreeDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B0b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFourDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B0b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFourDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B0b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFiveDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B1b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftFiveDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B1b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftSixDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B1b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftSixDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B1b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftSevenDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B1b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftSevenDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B1b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftEightDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B1b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftEightDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B1b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                this.rightOneDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B2b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightOneDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B2b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightTwoDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B2b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightTwoDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B2b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightThreeDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B2b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightThreeDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B2b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFourDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B2b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFourDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B2b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFiveDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["118b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightFiveDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["118b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightSixDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["118b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightSixDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["118b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightSevenDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["118b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightSevenDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["118b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightEightDrillCollarLockOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["118b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightEightDrillCollarLockClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["118b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                this.leftRopeOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.leftRopeClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightRopeOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.rightRopeClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N2N23B3b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #endregion
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
