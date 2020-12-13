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

namespace Main.SIR
{
    /// <summary>
    /// SIRSelfIO.xaml 的交互逻辑
    /// </summary>
    public partial class SIRSelfIO : UserControl
    {
        private static SIRSelfIO _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRSelfIO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRSelfIO();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRSelfIO()
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
                #region 913
                this.smLocalOpr.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["913b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBackTongReach.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["913b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBackTongRetract.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["913b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smUpResetInBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["913b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smUpResetOutBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["913b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smDownResetInBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["913b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smDownRestOutBtn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["913b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["913b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                #endregion

                #region 914
                this.smCosingPress.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["914b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRotateFloat.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["914b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smAlarm.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["914b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                #endregion

                #region 937
                this.smBackTongsClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["937b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBakcTongsOpen.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["937b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smTongsFront.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["937b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smTongsBack.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["937b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #endregion

                #region 938
                this.smStopMove.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["938b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smPipeUpDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["938b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smTongsHigh.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["938b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smTongsLow.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["938b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                #endregion

                #region 模拟量
                this.tbBigTongsInBtnElec.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfPlierInButtonElectric"], Mode = BindingMode.OneWay });
                this.tbBigTongsOutBtnElec.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfPlierOutButtonElectric"], Mode = BindingMode.OneWay });
                this.tbRoteHandForwardElec.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSkeletonForwardElectric"], Mode = BindingMode.OneWay });
                this.tbSkeletonBackElectric.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSkeletonBackElectric"], Mode = BindingMode.OneWay });
                this.tbSkeletonUpElectric.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSkeletonUpElectric"], Mode = BindingMode.OneWay });
                this.tbSkeletonDownElectric.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSkeletonDownElectric"], Mode = BindingMode.OneWay });
                this.tbSkeletonReachElectric.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSkeletonReachElectric"], Mode = BindingMode.OneWay });
                this.tbSkeletonRetractElectric.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSkeletonRetractElectric"], Mode = BindingMode.OneWay });
                this.tbSplitValveBuildPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSplitValveBuildPress"], Mode = BindingMode.OneWay });
                this.tbPipePressAdjust.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfPipePressAdjust"], Mode = BindingMode.OneWay });

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
