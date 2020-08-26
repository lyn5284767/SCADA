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
                this.tbInButtonTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIRInButtonTime"], Mode = BindingMode.OneWay });
                this.tbOutButtonTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["BSSIROutButtonTime"], Mode = BindingMode.OneWay });
                #endregion

                #region 右手柄信息
                this.tbHandleX.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandLeftOrRight"], Mode = BindingMode.OneWay });
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

                this.smPressSenorError.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRetractOrReachSenorError.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSwitchError.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smEncoderError.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smGetOil.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smRightHandleActive.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["504b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smCloseButton.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOpenButton.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                #endregion

                #region 操作选择
                this.btnDrillerOpr.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["804b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnStart.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnInButtonModel.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnButtonMemory.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                #endregion

                #region 钳头动作
                this.btnNipperReset.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRoteNipperClamp.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRoteNipperRelease.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnUpNipperClamp.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnUpNipperRelease.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnDownNipperClamp.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["802b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnDownNipperRelease.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["803b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                #endregion

                #region 自动模式
                this.btnAutoInNipper.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnAutoOutNipper.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnGotoWell.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnGotoMouse.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnBack.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["801b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                #endregion

            }
            catch (Exception ex)
            { }
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
    }
}
