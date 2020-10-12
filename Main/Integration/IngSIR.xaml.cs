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

namespace Main.Integration
{
    /// <summary>
    /// IngSIR.xaml 的交互逻辑
    /// </summary>
    public partial class IngSIR : UserControl
    {
        private static IngSIR _instance = null;
        private static readonly object syncRoot = new object();

        public static IngSIR Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngSIR();
                        }
                    }
                }
                return _instance;
            }
        }
        public IngSIR()
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
                //this.oprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfOperationModelConverter() });
                //this.oprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                //this.workModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                //this.workModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                //this.PipeTypeModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeModelConverter() });
                //this.PipeTypeModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeIsCheckConverter() });
                //this.locationModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocationModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocationModelConverter() });
                //this.locationModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocationModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });

               
                this.smMainGapOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMainGapTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBackGapOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInButtonPosOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInButtonPosTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOutButtonPosOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOutButtonPosTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSafeDoorReset.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["842b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSafeDoorClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["842b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.tbTongsGap.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfClampStatus"], Mode = BindingMode.OneWay, Converter = new SIRSelfTongsGapConverter() });
               
                this.tbInButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbInButtonClampingForce.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonClampingForce.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.tbRotate.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfRotateEncodePulse"], Mode = BindingMode.OneWay, Converter = new SIRSelfRotateConverter() });
                //this.tbSysPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSysPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

                //this.tbArmPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfArmPos"], Mode = BindingMode.OneWay, Converter = new TakeTenConverter() });
                //this.tbClampHeight.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfClampHeight"], Mode = BindingMode.OneWay, Converter = new TakeTenConverter() });
                //this.tbWorkCylinderPress.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkCylinderPress"], Mode = BindingMode.OneWay, Converter = new DivideHundredConverter() });
                //this.tbBrakingCylinderPress.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrakingCylinderPress"], Mode = BindingMode.OneWay, Converter = new DivideHundredConverter() });
               
                //this.tbInButtonCircle.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonCircle"], Mode = BindingMode.OneWay });
                //this.tbOutButtonCircle.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonCircle"], Mode = BindingMode.OneWay });
                //this.tbWorkTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfWorkTime"], Mode = BindingMode.OneWay });

                //// 一键上扣
                //MultiBinding sbInbuttonMultiBind = new MultiBinding();
                //sbInbuttonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
                //sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                //sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                //sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
                //sbInbuttonMultiBind.ConverterParameter = "inButton";
                //sbInbuttonMultiBind.NotifyOnSourceUpdated = true;
                //this.sbInButton.SetBinding(StepBar.StepIndexProperty, sbInbuttonMultiBind);
                //// 一键卸扣
                //MultiBinding sbOutButtonMultiBind = new MultiBinding();
                //sbOutButtonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
                //sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                //sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay });
                //sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
                //sbOutButtonMultiBind.ConverterParameter = "outButton";
                //sbOutButtonMultiBind.NotifyOnSourceUpdated = true;
                //this.sbOutButton.SetBinding(StepBar.StepIndexProperty, sbOutButtonMultiBind);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
