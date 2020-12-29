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

namespace Main.WellRepair.SIR_Self
{
    /// <summary>
    /// WR_SIR_IO.xaml 的交互逻辑
    /// </summary>
    public partial class WR_SIR_IO : UserControl
    {
        private static WR_SIR_IO _instance = null;
        private static readonly object syncRoot = new object();

        public static WR_SIR_IO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new WR_SIR_IO();
                        }
                    }
                }
                return _instance;
            }
        }
        public WR_SIR_IO()
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
                #region DI
                this.smTongsMid.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["833b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smPipeInductiveOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["833b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smPipeInductiveTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["833b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["833b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smHeart.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["833b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smAutoConfirm.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["838b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smPunchingBtnClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["838b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smPunchingBtnOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["838b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateBtnRotateClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["839b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateBtnRotateOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["839b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smArmUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["839b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smArmDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["839b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smArmRetract.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["839b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smArmReach.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["839b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smArmCorotation.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["806b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smArmReverse.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["806b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smUpTongsClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["806b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smUpTongsOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["806b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smDownTongsClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["806b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smDownTongsOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["806b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smThreadInBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["807b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smThreadOutBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["807b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smThreadClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["807b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smThreadOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["807b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateBtnClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["806b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateBtnOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["806b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateBtnUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["807b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["807b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                #endregion

                #region DO信号
                this.smCloseTongsClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["849b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smCloseTongsOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["849b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smPunchingTongsClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["849b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smPunchingTongsOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["849b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateTongsClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["849b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateTongsOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["849b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smDOArmUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["849b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smDOArmDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["849b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateBtnLeftClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["850b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateBtnLeftOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["850b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateTongsUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["850b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateTongsDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["850b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smPuchingBtnInBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["850b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smPuchingBtnOutBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["850b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
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
