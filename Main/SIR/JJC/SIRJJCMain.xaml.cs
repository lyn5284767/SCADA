using COM.Common;
using ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// SIRJJCMain.xaml 的交互逻辑
    /// </summary>
    public partial class SIRJJCMain : UserControl
    {
        private static SIRJJCMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRJJCMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRJJCMain();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRJJCMain()
        {
            InitializeComponent();
            VariableBinding();
        }
        /// <summary>
        /// 变量绑定
        /// </summary>
        private void VariableBinding()
        {
            this.tbRightHandleSelect.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["504b2"], Mode = BindingMode.OneWay, Converter = new SIRRightHandleSelectCoverter() });
            this.tbWorkStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["IDWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRWorkModelCoverter() });
            this.tbOprModel.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["IDOperModel"], Mode = BindingMode.OneWay, Converter = new SIROprModelCoverter() });
            this.tbWorkLocation.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["IDWorkLocation"], Mode = BindingMode.OneWay, Converter = new SIRWorkLocationCoverter() });
            this.smHeart.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["504b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

            this.operateMode.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["IDOperModel"], Mode = BindingMode.OneWay, Converter = new SIROprModelCoverter() });
            this.operateMode.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["IDOperModel"], Mode = BindingMode.OneWay, Converter = new SIROprModelSelectCoverter() });
            this.smUpButtonPressure.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            SIRRealTimePressureCoverter sIRRealTimePressureCoverter = new SIRRealTimePressureCoverter();
            MultiBinding RealTimePressureMultiBind = new MultiBinding();
            RealTimePressureMultiBind.Converter = sIRRealTimePressureCoverter;
            RealTimePressureMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["IDUpButtonPressure"], Mode = BindingMode.OneWay });
            RealTimePressureMultiBind.Bindings.Add(new Binding("Text") { Source = this.tbUnitOne, Mode = BindingMode.OneWay });
            RealTimePressureMultiBind.NotifyOnSourceUpdated = true;
            this.tbRealTimePressure.SetBinding(TextBlock.TextProperty, RealTimePressureMultiBind);
            MultiBinding PressureSetMultiBind = new MultiBinding();
            PressureSetMultiBind.Converter = sIRRealTimePressureCoverter;
            PressureSetMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["IDUpButtonPressureSet"], Mode = BindingMode.OneWay });
            PressureSetMultiBind.Bindings.Add(new Binding("Text") { Source = this.tbUnitOne, Mode = BindingMode.OneWay });
            PressureSetMultiBind.NotifyOnSourceUpdated = true;
            this.tbPressureSet.SetBinding(TextBlock.TextProperty, PressureSetMultiBind);

            this.tbCalibrationStatus.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b3"], Mode = BindingMode.OneWay, Converter = new SIRCalibrationStatusCoverter() });
            this.tbRealTimeRatate.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDRotate"], Mode = BindingMode.OneWay});
            this.tbCylinderTrip.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDCylinderTrip"], Mode = BindingMode.OneWay });
            this.tbCylinderZero.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDCylinderZero"], Mode = BindingMode.OneWay });
            this.tbZeroAngleOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDRotateZero"], Mode = BindingMode.OneWay });
            this.tbRotateWaitOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDRotateWait"], Mode = BindingMode.OneWay });
            this.tbWellLocationOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDWellLocation"], Mode = BindingMode.OneWay });
            this.tbMouseLocationOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDMouseLocatio"], Mode = BindingMode.OneWay });

            this.tbInOutStatus.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b4"], Mode = BindingMode.OneWay, Converter = new SIRCalibrationStatusCoverter() });
            this.tbInOutCylinderTrip.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDInOutCylinderTrip"], Mode = BindingMode.OneWay });
            this.tbFlowRateValve.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDFlowRateValve"], Mode = BindingMode.OneWay });
            this.tbPressureRateValve.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDPressureRateValve"], Mode = BindingMode.OneWay });
            this.tbInOutMaxSpeedOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDInOutMaxSpeed"], Mode = BindingMode.OneWay });
            this.tbInOutCrawlSpeedOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDInOutCrawlSpeed"], Mode = BindingMode.OneWay });
            this.tbWellRetractLengthOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDWellRetractLength"], Mode = BindingMode.OneWay });
            this.tbMouseRetractLengthOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDMouseRetractLength"], Mode = BindingMode.OneWay });
            this.tbInOutCylinderZeroOut.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IDInOutCylinderZero"], Mode = BindingMode.OneWay });

            this.tbHandleX.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandLeftOrRight"], Mode = BindingMode.OneWay});
            this.tbHandleY.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandFrontOrBehind"], Mode = BindingMode.OneWay });
            this.smL.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smR.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smQ.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smB.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smEnable.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["517b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            //7.28 修改
            ArrowDirectionMultiConverter arrowDirectionMultiConverter = new ArrowDirectionMultiConverter();
            MultiBinding ArrowMultiBind = new MultiBinding();
            ArrowMultiBind.Converter = arrowDirectionMultiConverter;
            ArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b4"], Mode = BindingMode.OneWay });
            ArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b5"], Mode = BindingMode.OneWay });
            ArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b2"], Mode = BindingMode.OneWay });
            ArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b3"], Mode = BindingMode.OneWay });
            ArrowMultiBind.NotifyOnSourceUpdated = true;
            this.Arrow_EquiptStatus.SetBinding(Image.SourceProperty, ArrowMultiBind);
            //ArrowDirectionMultiConverter arrowDirectionMultiConverter = new ArrowDirectionMultiConverter();
            //MultiBinding upArrowMultiBind = new MultiBinding();
            //upArrowMultiBind.Converter = arrowDirectionMultiConverter;
            //upArrowMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandFrontOrBehind"], Mode = BindingMode.OneWay });
            //upArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b0"], Mode = BindingMode.OneWay });
            //upArrowMultiBind.ConverterParameter = "up";
            //upArrowMultiBind.NotifyOnSourceUpdated = true;
            //this.upArrow_EquiptStatus.SetBinding(Image.SourceProperty, upArrowMultiBind);
            //MultiBinding downArrowMultiBind = new MultiBinding();
            //downArrowMultiBind.Converter = arrowDirectionMultiConverter;
            //downArrowMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandFrontOrBehind"], Mode = BindingMode.OneWay });
            //downArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b0"], Mode = BindingMode.OneWay });
            //downArrowMultiBind.ConverterParameter = "down";
            //downArrowMultiBind.NotifyOnSourceUpdated = true;
            //this.downArrow_EquiptStatus.SetBinding(Image.SourceProperty, downArrowMultiBind);
            //MultiBinding leftArrowMultiBind = new MultiBinding();
            //leftArrowMultiBind.Converter = arrowDirectionMultiConverter;
            //leftArrowMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandLeftOrRight"], Mode = BindingMode.OneWay });
            //leftArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b0"], Mode = BindingMode.OneWay });
            //leftArrowMultiBind.ConverterParameter = "left";
            //leftArrowMultiBind.NotifyOnSourceUpdated = true;
            //this.leftArrow_EquiptStatus.SetBinding(Image.SourceProperty, leftArrowMultiBind);
            //MultiBinding rightArrowMultiBind = new MultiBinding();
            //rightArrowMultiBind.Converter = arrowDirectionMultiConverter;
            //rightArrowMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandLeftOrRight"], Mode = BindingMode.OneWay });
            //rightArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b0"], Mode = BindingMode.OneWay });
            //rightArrowMultiBind.ConverterParameter = "right";
            //rightArrowMultiBind.NotifyOnSourceUpdated = true;
            //this.rightArrow_EquiptStatus.SetBinding(Image.SourceProperty, rightArrowMultiBind);

            this.smArmReach.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smArmRetract.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smButtonReach.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smButtonRetract.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smSysReset.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smPliersUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smPliersDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smButtonIn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smButtonOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smHot.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smArmLeft.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smArmRight.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smButtonImple.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smButtonCancel.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smLockHook.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smRotate.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smInOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

            this.tubeType.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["IDDrillType"], Mode = BindingMode.OneWay, Converter = new SIR_JJC_DrillTypeConverter() });

            this.cbSafeLimit.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["535b6"], Mode = BindingMode.OneWay });
            this.smAllowIron.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smDFAntiCollision.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["308b1"], Mode = BindingMode.OneWay, Converter = new OppositeBoolTagConverter() });
        }

        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_OpState(object sender, EventArgs e)
        {
            //byte[] data;
            //if (this.operateMode.IsChecked)
            //{
            //    data = new byte[10] { 80, 16, 4, 2, 0, 0, 1, 0, 0, 0 };
            //}
            //else
            //{
            //    data = new byte[10] { 80, 16, 4, 1, 0, 0, 1, 0, 0, 0 };
            //}
            byte[] data = new byte[10] { 16, 1, 2, 0, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 减小压力设定
        /// </summary>
        private void PressureDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 23, 4, 0, 0, 1, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 增加压力设定
        /// </summary>
        private void PressureUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 23, 3, 0, 0, 1, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 管柱选择
        /// </summary>
        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {
            int tag = (sender as MenuItem).TabIndex;
            byte[] byteToSend = new byte[10] { 80, 16, 3, (byte)tag, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 旋转设定请求
        /// </summary>
        private void spRotateSetMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 23, 1, 0, 0, 1, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
            this.effRotateSelect.BorderBrush = new SolidColorBrush(Colors.Blue);
            var bc = new BrushConverter();
            this.effInOutSelect.BorderBrush = (Brush)bc.ConvertFrom("#F6F9FF");
        }
        /// <summary>
        /// 伸缩标定请求
        /// </summary>
        private void InOutSetMouseDown(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 23, 2, 0, 0, 1, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
            this.effInOutSelect.BorderBrush = new SolidColorBrush(Colors.Blue);
            var bc = new BrushConverter();
            this.effRotateSelect.BorderBrush = (Brush)bc.ConvertFrom("#F6F9FF");
        }
        /// <summary>
        /// 切换单位
        /// </summary>
        private void SwichUnit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.tbUnitOne.Text == "KN.M") this.tbUnitOne.Text = "kft.lbs";
            else this.tbUnitOne.Text = "KN.M";
            if (this.tbUnitTwo.Text == "KN.M") this.tbUnitTwo.Text = "kft.lbs";
            else this.tbUnitOne.Text = "kft.lbs";
        }
        /// <summary>
        /// 零位角度标定
        /// </summary>
        private void ZeroAngleSet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 23, 1, 0, 0, 2, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 获取键盘
        /// </summary>
        private void tbFocus(object sender, RoutedEventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }
        /// <summary>
        /// 失去焦点绑定值
        /// </summary>
        private void tbLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            Regex regexParameterConfigurationConfirm = new Regex(@"^[0-9]+$");
            string strText = tb.Text;
            if (strText.Length == 0) strText = "0";
            if ((regexParameterConfigurationConfirm.Match(strText)).Success)
            {
                try
                {
                    if (strText.Length == 0) strText = "0";
                    short i16Text = (short)(double.Parse(strText) * 10);

                    //short i16Text = Convert.ToInt16(strText);
                    byte[] tempByte = BitConverter.GetBytes(i16Text);
                    GlobalData.Instance.ConfigParameter[0] = (byte)int.Parse(tb.Tag.ToString());
                    GlobalData.Instance.ConfigParameter[1] = tempByte[0];
                    GlobalData.Instance.ConfigParameter[2] = tempByte[1];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("参数数值范围异常:" + ex.Message);
                    tb.Text = "0";
                }

            }
            else
            {
                MessageBox.Show("参数为非数字");
                tb.Text = "0";
            }
        }

        private byte[] bConfigParameter = new byte[3];
        private void ParamSet(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SendListToByte(new List<byte>() { 80, 16, 23, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2], 2 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 安全限制解除
        /// </summary>
        private void cbSafeLimit_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbSafeLimit.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 24, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                if (MessageBox.Show("确认解除钻台面对铁钻工的安全设置", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    byteToSend = new byte[10] { 80, 16, 1, 24, 1, 0, 0, 0, 0, 0 };
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
        }
    }
}
