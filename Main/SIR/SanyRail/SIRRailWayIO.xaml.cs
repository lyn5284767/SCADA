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

namespace Main.SIR.SanyRail
{
    /// <summary>
    /// SIRRailWayIO.xaml 的交互逻辑
    /// </summary>
    public partial class SIRRailWayIO : UserControl
    {
        private static SIRRailWayIO _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRRailWayIO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRRailWayIO();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRRailWayIO()
        {
            InitializeComponent();
            VariableBinding();
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                #region DI输入
                this.smStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["809b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOpenRight.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["809b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smCloseRight.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["809b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMainTongsSlowDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["809b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMainTongsStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["809b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smTongsAlignment.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["809b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMainTongsInBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["809b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMainTongsOutBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["809b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBackTongsInBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["810b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBackTongsOutBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["810b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #endregion

                #region DO输出
                this.smTongsUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["817b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smTongsDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["817b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smHigh.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["817b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smLow.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["817b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smCylinderDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["817b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMainTongsInBtnModel.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["817b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMainTongsOutBtnModel.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["817b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBackTongsInBtnModel.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["817b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBackTongsOutBtnModel.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["818b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smClear.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["818b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOilApply.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["818b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMotorStart.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["818b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smWork.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["818b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                #endregion

                #region 模拟量
                this.tbSysPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIR_RailWay_SystemPress"], Mode = BindingMode.OneWay });
                this.tbInBtnPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_InBtnPress"], Mode = BindingMode.OneWay });
                this.tbTorquePress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsPress"], Mode = BindingMode.OneWay });
                this.tbMainTongsInBtnElec.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_MainTongsMotorInBtnElecOutput"], Mode = BindingMode.OneWay });
                this.tbMainTongsOutBtnElec.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_MainTongsMotorOutBtnElecOutput"], Mode = BindingMode.OneWay });
                this.tbUpDownMove.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_TongsHeight"], Mode = BindingMode.OneWay });
                this.tbBackTongsCloseElec.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsCloseElecOutput"], Mode = BindingMode.OneWay });
                this.tbBackTongsOpenElec.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_BackTongsOpenElecOutput"], Mode = BindingMode.OneWay });
                this.tbHighSpeedPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_High"], Mode = BindingMode.OneWay });
                this.tbLowSpeedPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_Low"], Mode = BindingMode.OneWay });
                this.tbTongForwardElec.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_TongsForwardElecOutput"], Mode = BindingMode.OneWay });
                this.tbTongsBackElec.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_TongsBackElecOutput"], Mode = BindingMode.OneWay });

                #endregion
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
