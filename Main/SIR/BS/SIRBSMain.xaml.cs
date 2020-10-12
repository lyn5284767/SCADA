using COM.Common;
using ControlLibrary;
using HandyControl.Controls;
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
    /// SIRBSMain.xaml 的交互逻辑
    /// </summary>
    public partial class SIRBSMain : UserControl
    {
        private static SIRBSMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRBSMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRBSMain();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRBSMain()
        {
            InitializeComponent();
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
                #region 参数设置
                // 上扣扭矩
                this.cpbInButtonTorque.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRTorqueShow"], Mode = BindingMode.OneWay});
                this.cpbInButtonTorque.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRTorqueShow"], Mode = BindingMode.OneWay});
                MultiBinding cpbInButtonTorqueMultiBind = new MultiBinding();
                cpbInButtonTorqueMultiBind.Converter = new ColorCoverter();
                cpbInButtonTorqueMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRTorqueShow"], Mode = BindingMode.OneWay });
                cpbInButtonTorqueMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbInButtonTorque, Mode = BindingMode.OneWay });
                cpbInButtonTorqueMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbInButtonTorque, Mode = BindingMode.OneWay });
                cpbInButtonTorqueMultiBind.NotifyOnSourceUpdated = true;
                this.cpbInButtonTorque.SetBinding(CircleProgressBar.ForegroundProperty, cpbInButtonTorqueMultiBind);
                // 系统压力
                this.cpbSysPress.SetBinding(CircleProgressBar.ValueProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRSysPress"], Mode = BindingMode.OneWay });
                this.cpbSysPress.SetBinding(CircleProgressBar.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRSysPress"], Mode = BindingMode.OneWay });
                MultiBinding cpbSysPressMultiBind = new MultiBinding();
                cpbSysPressMultiBind.Converter = new ColorCoverter();
                cpbSysPressMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRSysPress"], Mode = BindingMode.OneWay });
                cpbSysPressMultiBind.Bindings.Add(new Binding("Minimum") { Source = this.cpbSysPress, Mode = BindingMode.OneWay });
                cpbSysPressMultiBind.Bindings.Add(new Binding("Maximum") { Source = this.cpbSysPress, Mode = BindingMode.OneWay });
                cpbSysPressMultiBind.NotifyOnSourceUpdated = true;
                this.cpbInButtonTorque.SetBinding(CircleProgressBar.ForegroundProperty, cpbSysPressMultiBind);

                this.tbInButtonTorqueSet.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRInButtonTorqueSet"], Mode = BindingMode.OneWay });
                this.tbReachOrRetractDis.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRReachOrRetractDis"], Mode = BindingMode.OneWay });
                this.tbRotateAngle.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRRotateAngle"], Mode = BindingMode.OneWay });
                //this.tbInButtonTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRInButtonTime"], Mode = BindingMode.OneWay });
                //this.tbOutButtonTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIROutButtonTime"], Mode = BindingMode.OneWay });
                this.tbInButtonTimeSetValue.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRInButtonTime"], Mode = BindingMode.OneWay });
                this.tbOutButtonTimeSetValue.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIROutButtonTime"], Mode = BindingMode.OneWay });
                this.tbInButtonTimesShow.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIROutButtonTimes"], Mode = BindingMode.OneWay });

                this.cbDRSafeLimit.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["505b6"], Mode = BindingMode.OneWay });
                #endregion

                #region 右手柄信息 弃用
                //this.tbHandleX.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandLeftOrRight"], Mode = BindingMode.OneWay });
                //this.tbHandleY.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandFrontOrBehind"], Mode = BindingMode.OneWay });
                //this.smL.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.smR.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.smQ.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.smB.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.smEnable.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["517b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                ////7.28 修改
                //ArrowDirectionMultiConverter arrowDirectionMultiConverter = new ArrowDirectionMultiConverter();
                //MultiBinding ArrowMultiBind = new MultiBinding();
                //ArrowMultiBind.Converter = arrowDirectionMultiConverter;
                //ArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b4"], Mode = BindingMode.OneWay });
                //ArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b5"], Mode = BindingMode.OneWay });
                //ArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b2"], Mode = BindingMode.OneWay });
                //ArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b3"], Mode = BindingMode.OneWay });
                //ArrowMultiBind.NotifyOnSourceUpdated = true;
                //this.Arrow_EquiptStatus.SetBinding(Image.SourceProperty, ArrowMultiBind);

                //this.smCloseButton.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.smOpenButton.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.smUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.smDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #endregion

                #region 状态反馈
                this.smStop.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smPressSenorError.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRetractOrReachSenorError.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSwitchError.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smEncoderError.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smGetOil.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRightHandleActive.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["504b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #endregion

                #region 动作选择
                this.cbTorqueMemory.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["535b0"], Mode = BindingMode.OneWay });
                this.cbTongsReset.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["535b1"], Mode = BindingMode.OneWay });
                this.cbRotateBtnClose.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["535b2"], Mode = BindingMode.OneWay });
                this.cbRotateBtnOpen.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["535b3"], Mode = BindingMode.OneWay });
                this.cbUpBtnClose.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["535b6"], Mode = BindingMode.OneWay });
                this.cbUpBtnOpen.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["535b7"], Mode = BindingMode.OneWay });
                this.cbDownBtnClose.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["536b0"], Mode = BindingMode.OneWay });
                this.cbDownBtnOpen.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["536b1"], Mode = BindingMode.OneWay });
                this.cbWellSet.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["536b2"], Mode = BindingMode.OneWay });
                this.cbMouseSet.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["536b3"], Mode = BindingMode.OneWay });
                this.cbStaySet.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["536b4"], Mode = BindingMode.OneWay });

                #endregion

                #region 操作选择
                this.btnDrillerOpr.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnStart.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnInbuttonModel.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnInbuttonModel.SetBinding(Button.ContentProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["BSSIRInButtonModel"], Mode = BindingMode.OneWay, Converter = new InButtonModelCoverter() });
                this.btnAutoInButton.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnAutoOutButton.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnAutoToWell.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnAutoToMouse.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnAutoBack.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnAutoToStay.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                //this.btnInButtonModel.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnButtonMemory.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                #endregion

                #region 钳头动作
                //this.btnNipperReset.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnRoteNipperClamp.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnRoteNipperRelease.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnUpNipperClamp.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnUpNipperRelease.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnDownNipperClamp.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnDownNipperRelease.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                #endregion

                #region 自动模式 弃用
                //this.btnAutoInNipper.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnAutoOutNipper.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnGotoWell.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnGotoMouse.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.btnBack.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                #endregion

                #region 当前动作
                this.btnTorqueMemory.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnTongsReset.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRotateBtnClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRotateBtnOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnUpBtnClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnUpBtnOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnDownBtnClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnDownBtnOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnInBtn.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnOutBtn.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRotateIn.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRotateOut.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnUp.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnDown.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                #endregion

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void BtnTurnMainOne_Click(object sender, RoutedEventArgs e)
        {

        }

        #region 参数设置
        /// <summary>
        /// 上扣旋扣时间
        /// </summary>
        private void BtnInButtonTime_Click(object sender, RoutedEventArgs e)
        {
            int val = 0;
            int.TryParse(this.tbInButtonTimeSetValue.Text, out val);
            if (val > 0)
            {
                int paramOne = val % 256;
                int paramTwo = val / 256;
                byte[] data = new byte[10] { 80, 16, 2, 11, (byte)paramOne, (byte)paramTwo, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(data);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("数据解析错误");
            }
        }
        /// <summary>
        /// 卸扣旋扣时间
        /// </summary>
        private void BtnOutButtonTime_Click(object sender, RoutedEventArgs e)
        {
            int val = 0;
            int.TryParse(this.tbOutButtonTimeSetValue.Text, out val);
            if (val > 0)
            {
                int paramOne = val % 256;
                int paramTwo = val / 256;
                byte[] data = new byte[10] { 80, 16, 2, 12, (byte)paramOne, (byte)paramTwo, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(data);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("数据解析错误");
            }
        }
        /// <summary>
        /// 冲扣次数
        /// </summary>
        private void BtnInButtonTimes_Click(object sender, RoutedEventArgs e)
        {
            int val = 0;
            int.TryParse(this.tbInButtonTimesShow.Text, out val);
            if (val > 0)
            {
                int paramOne = val % 256;
                int paramTwo = val / 256;
                byte[] data = new byte[10] { 80, 16, 2, 13, (byte)paramOne, (byte)paramTwo, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(data);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("数据解析错误");
            }
        }
        #endregion

        #region 操作/选择
        /// <summary>
        /// 司钻操作
        /// </summary>
        private void BtnDrillerOpr_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 2, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 启动
        /// </summary>
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 8, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 上扣模式
        /// </summary>
        private void BtnInButtonModel_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 6, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 扭矩记忆
        /// </summary>
        private void BtnButtonMemory_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 17, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 自动上扣
        /// </summary>
        private void BtnAutoInButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 4, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 自动卸扣
        /// </summary>
        private void BtnAutoOutButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 5, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 自动去井口
        /// </summary>
        private void BtnAutoToWell_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 自动去鼠洞
        /// </summary>
        private void BtnAutoToMouse_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 自动返回
        /// </summary>
        private void BtnAutoBack_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 3, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 自动去待命位
        /// </summary>
        private void BtnAutoToStay_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 10, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        #endregion

        #region 钳头动作
        /// <summary>
        /// 钳头复位
        /// </summary>
        private void BtnbtnNipperReset_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 7, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 旋扣钳夹紧
        /// </summary>
        private void BtnRoteNipperClamp_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 11, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 旋扣钳松开
        /// </summary>
        private void BtnRoteNipperRelease_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 12, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 上钳夹紧
        /// </summary>
        private void BtnUpNipperClamp_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 13, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 上钳松开
        /// </summary>
        private void BtnUpNipperRelease_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 14, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 下钳夹紧
        /// </summary>
        private void BtnDownNipperClamp_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 15, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 下钳松开
        /// </summary>
        private void BtnDownNipperRelease_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 16, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        #endregion

        #region 自动模式
        /// <summary>
        /// 自动上扣
        /// </summary>
        private void BtnAutoInNipper_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 4, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 自动卸扣
        /// </summary>
        private void BtnAutoOutNipper_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 5, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 去井口
        /// </summary>
        private void BtnGotoWell_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 去鼠洞
        /// </summary>
        private void BtnGotoMouse_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 返回
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 3, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        #endregion

        #region 位置标定
        /// <summary>
        /// 井口位置
        /// </summary>
        private void BtnWellLocation_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 18, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 鼠洞位置位置
        /// </summary>
        private void BtnMouseLocation_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 19, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 待命位置
        /// </summary>
        private void BtnStayLocation_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 1, 20, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        #endregion

        #region 动作选择
        /// <summary>
        /// 扭矩记忆
        /// </summary>
        private void cbTorqueMemory_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbTorqueMemory.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 21, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 钳头复位
        /// </summary>
        private void cbTongsReset_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbTongsReset.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 21, 2, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 旋扣钳夹紧
        /// </summary>
        private void cbRotateBtnClose_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbRotateBtnClose.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 21, 4, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 旋扣钳松开
        /// </summary>
        private void cbRotateBtnOpen_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbRotateBtnOpen.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 21, 6, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 上钳夹紧
        /// </summary>
        private void cbUpBtnClose_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbUpBtnClose.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 21, 64, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 上钳松开
        /// </summary>
        private void cbUpBtnOpen_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbUpBtnOpen.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 21, 128, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 下钳夹紧
        /// </summary>
        private void cbDownBtnClose_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbUpBtnOpen.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 22, 1, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 下钳松开
        /// </summary>
        private void cbDownBtnOpen_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbUpBtnOpen.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 22, 2, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井口标定
        /// </summary>
        private void cbWellSet_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbUpBtnOpen.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 22, 4, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 鼠洞标定
        /// </summary>
        private void cbMouseSet_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbUpBtnOpen.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 22, 8, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 待命标定
        /// </summary>
        private void cbStaySet_Clicked(object sender, EventArgs e)
        {
            byte[] byteToSend;

            if (this.cbUpBtnOpen.IsChecked)
            {
                byteToSend = new byte[10] { 80, 16, 1, 23, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 80, 16, 1, 22, 16, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion
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
                byte[] byteToSend = new byte[10] { 80, 16, 1, 24, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("确认解除钻台面对铁钻工的安全设置?", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    byte[] byteToSend = new byte[10] { 80, 16, 1, 24, 1, 0, 0, 0, 0, 0 };
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
        }
    }
}
