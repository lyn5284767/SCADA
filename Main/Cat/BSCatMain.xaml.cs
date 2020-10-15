using COM.Common;
using ControlLibrary;
using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// BSCatMain.xaml 的交互逻辑
    /// </summary>
    public partial class BSCatMain : UserControl
    {
        private static BSCatMain _instance = null;
        private static readonly object syncRoot = new object();

        public static BSCatMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new BSCatMain();
                        }
                    }
                }
                return _instance;
            }
        }

        System.Threading.Timer timerWarning;
        public BSCatMain()
        {
            InitializeComponent();
            VariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }

        private void VariableBinding()
        {
            try
            {
                this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b0"], Mode = BindingMode.OneWay });
                this.MainPumpOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b1"], Mode = BindingMode.OneWay,Converter= new CheckedIsFalseConverter() });
                this.MainPumpTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b3"], Mode = BindingMode.OneWay, Converter = new CheckedIsFalseConverter() });
                this.LeftOrRight.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["706b1"], Mode = BindingMode.OneWay});
                this.InOrOut.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["706b3"], Mode = BindingMode.OneWay });

                this.cbDRSafeLimit.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["505b7"], Mode = BindingMode.OneWay });
                this.cbIgnoreLimit.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b6"], Mode = BindingMode.OneWay });
                this.cbSelectPipe.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b7"], Mode = BindingMode.OneWay });

                this.tbLeftOrRight.SetBinding(TextBox.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandLeftOrRight"], Mode = BindingMode.OneWay });
                this.tbFrontOrBehind.SetBinding(TextBox.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandFrontOrBehind"], Mode = BindingMode.OneWay });
                this.smL.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smR.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smQ.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smB.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smEnable.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["517b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

                // 7.27修改
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

                this.smRight.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["504b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBigCarEncode.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInDangerArea.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSmallCarEncode.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.smSpare.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["504b2"], Mode = BindingMode.OneWay });

                this.btnStart.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                this.smLeftOrRight.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["505b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInOrOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["505b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smAutoUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["509b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smAutoDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["509b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smAutoIn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["509b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smAutoOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["509b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOnkeyUseDrill.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOnKeyUnuserDrill.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBentUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBentDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smTiltUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smTiltDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOrganKickOut.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOrganRetract.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["510b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            }
            catch (Exception ex)
            {
                Log4Net.AddLog(ex.StackTrace, InfoLevel.ERROR);
            }
        }

        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Warning();
                    if (!GlobalData.Instance.ComunciationNormal) this.tbTips.Text = "网络连接失败！";
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private void Warning()
        {
            if (GlobalData.Instance.da["504b2"].Value.Boolean)
            {
                if (!GlobalData.Instance.da["334b4"].Value.Boolean && !GlobalData.Instance.da["505b7"].Value.Boolean)
                {
                    this.tbTips.Text = "禁止猫道前进，请先将钻台面回收至安全位置！";
                }
            }
            else
            {
                this.tbTips.Text = "请先将旋钮选择猫道！";
            }
        }
        /// <summary>
        /// 控制模式本地/司钻
        /// </summary>
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 8, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 1#泵启动/停止
        /// </summary>
        private void btn_MainPumpOne(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 5, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 2#泵启动/停止
        /// </summary>
        private void btn_MainPumpTwo(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 5, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左右选择
        /// </summary>
        private void btn_LeftOrRight(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.LeftOrRight.IsChecked) //当前左
            {
                byteToSend = new byte[10] { 80, 48, 6, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前右
            {
                byteToSend = new byte[10] { 80, 48, 6, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 内外选择
        /// </summary>
        private void btn_InOrLeft(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.LeftOrRight.IsChecked) //当前内
            {
                byteToSend = new byte[10] { 80, 48, 7, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前外
            {
                byteToSend = new byte[10] { 80, 48, 7, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 忽略限制
        /// </summary>
        private void cbIgnoreLimit_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 2, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 选择套管
        /// </summary>
        private void cbSelectPipe_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 3, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 启动
        /// </summary>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 1, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 辅助上升
        /// </summary>
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 4, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 辅助下降
        /// </summary>
        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 4, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 多根踢出
        /// </summary>
        private void btnKickOut_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 4, 5, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钻台面安全限制解除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDRSafeLimit_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked.Value)
            {
                byte[] byteToSend = new byte[10] { 80, 48, 9, 0, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                MessageBoxResult result =  MessageBox.Show("确认解除钻台面对猫道得安全设置?", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    byte[] byteToSend = new byte[10] { 80, 48, 9, 0, 1, 0, 0, 0, 0, 0 };
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
        }
    }
}
