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

namespace Main.Cat
{
    /// <summary>
    /// ZY_CatMain.xaml 的交互逻辑
    /// </summary>
    public partial class ZY_CatMain : UserControl
    {
        private static ZY_CatMain _instance = null;
        private static readonly object syncRoot = new object();

        public static ZY_CatMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ZY_CatMain();
                        }
                    }
                }
                return _instance;
            }
        }
        public ZY_CatMain()
        {
            InitializeComponent();
            VariableBinding();
        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["703b0"], Mode = BindingMode.OneWay});
            this.smStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["702b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smReliefValve.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["702b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smUpArmOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smUpArmBack.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smVStraighten.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smVOverTurn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smPlatRaise.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smPlatDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smPlatForward.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smPlatBackOff.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
        }
        /// <summary>
        /// 遥控/司钻
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 9, 0, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
