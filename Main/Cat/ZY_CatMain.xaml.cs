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

            #region 键盘
            this.runFingerClose.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["708b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runFingerOpen.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["708b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runDrillClose.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["708b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runDrillOpen.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["708b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runElevtorClose.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["708b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runElevtorOpen.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["708b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runElevtorHor.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["708b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runElevtorVer.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["708b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runAutoUp.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["709b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runAutoDown.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["709b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runKavaClose.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["709b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runKavaOpen.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["709b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runRingsRunOut.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["709b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runRingsReturn.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["709b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runUpHandMove.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["710b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runUpHandReturn.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["710b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runVLeft.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["710b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.runVRight.SetBinding(Run.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["710b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

            #endregion
        }
        /// <summary>
        /// 遥控/司钻
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 16, 1, 2, 0, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
