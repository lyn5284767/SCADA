using COM.Common;
using ControlLibrary;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// SIRSelfMain.xaml 的交互逻辑
    /// </summary>
    public partial class SIRSelfMain : UserControl
    {
        private static SIRSelfMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRSelfMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRSelfMain();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        public SIRSelfMain()
        {
            InitializeComponent();
            VariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟

        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.oprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfOperationModelConverter() });
                this.oprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                this.workModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                this.workModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                this.PipeTypeModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeModelConverter() });
                this.PipeTypeModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeIsCheckConverter() });
                this.locationModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocationModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocationModelConverter() });
                this.locationModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLocationModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });

                this.tbOneKeyInButton.SetBinding(ToggleButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOneKeyInButton"], Mode = BindingMode.OneWay, Converter = new SIRSelfTwoToCheckConverter() });
                this.tbOneKeyOutButton.SetBinding(ToggleButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOneKeyOutButton"], Mode = BindingMode.OneWay, Converter = new SIRSelfTwoToCheckConverter() });

                this.smMainGapOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smMainGapOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smBackGapOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInButtonPosOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smInButtonPosTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOutButtonPosOne.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smOutButtonPosTwo.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["841b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSafeDoorReset.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["842b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.smSafeDoorClose.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["842b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                //this.tbControlModel.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbLoaction.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbOperModel.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbWorkModel.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbSafeDoor.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbGap.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });

                this.tbInButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbInButtonClampingForce.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonClampingForce.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonClampingForce"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbRotate.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfRotate"], Mode = BindingMode.OneWay });
                this.tbSysPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSysPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

                this.tbArmPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfArmPos"], Mode = BindingMode.OneWay });
                this.tbClampHeight.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfClampHeight"], Mode = BindingMode.OneWay });
                this.tbWorkCylinderPress.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkCylinderPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbBrakingCylinderPress.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrakingCylinderPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbInButtonTorque.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfInButtonTorque"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOutButtonTorque.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonTorque"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbInButtonCircle.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonCircle"], Mode = BindingMode.OneWay });
                this.tbOutButtonCircle.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonCircle"], Mode = BindingMode.OneWay });
                this.tbWorkTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfWorkTime"], Mode = BindingMode.OneWay });

                // 一键上扣
                MultiBinding sbInbuttonMultiBind = new MultiBinding();
                sbInbuttonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOneKeyInButton"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
                sbInbuttonMultiBind.ConverterParameter = "inButton";
                sbInbuttonMultiBind.NotifyOnSourceUpdated = true;
                this.sbInButton.SetBinding(StepBar.StepIndexProperty, sbInbuttonMultiBind);
                // 排杆
                MultiBinding sbOutButtonMultiBind = new MultiBinding();
                sbOutButtonMultiBind.Converter = new SIRSelfAutoModeStepCoverter();
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOneKeyOutButton"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfAutoStep"], Mode = BindingMode.OneWay });
                sbOutButtonMultiBind.ConverterParameter = "outButton";
                sbOutButtonMultiBind.NotifyOnSourceUpdated = true;
                this.sbOutButton.SetBinding(StepBar.StepIndexProperty, sbOutButtonMultiBind);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_oprModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.oprModel.IsChecked) //当前手动状态
            {
                byteToSend = new byte[10] { 23, 17, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前自动状态
            {
                byteToSend = new byte[10] { 23, 17, 1, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_workModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.workModel.IsChecked) //当前上扣模式
            {
                byteToSend = new byte[10] { 23, 17, 2, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前卸扣模式
            {
                byteToSend = new byte[10] { 23, 17, 2, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 管柱选择
        /// </summary>
        private void btn_PipeTypeModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.PipeTypeModel.IsChecked) //当前钻杠
            {
                byteToSend = new byte[10] { 23, 17, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前套管
            {
                byteToSend = new byte[10] { 23, 17, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工位选择
        /// </summary>
        private void btn_locationModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.locationModel.IsChecked) //当前井口
            {
                byteToSend = new byte[10] { 23, 17, 4, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前鼠洞
            {
                byteToSend = new byte[10] { 23, 17, 4, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 一键上扣
        /// </summary>
        private void OneKeyInButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tbn = sender as ToggleButton;
            byte[] byteToSend;
            if (tbn.IsChecked.Value) // 当前一键上扣
            {
                byteToSend = new byte[10] { 23, 17, 5, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 23, 17, 5, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 一键卸扣
        /// </summary>
        private void tbOneKeyOutButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tbn = sender as ToggleButton;
            byte[] byteToSend;
            if (tbn.IsChecked.Value) // 当前一键卸扣
            {
                byteToSend = new byte[10] { 23, 17, 6, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 23, 17, 6, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.Warnning();
                    this.Communcation();
                    if (GlobalData.Instance.da["SIRSelfOperModel"].Value.Byte == 2) // 自动模式
                    {
                        if (GlobalData.Instance.da["SIRSelfOneKeyInButton"].Value.Byte == 2)//一键上扣开启
                        {
                            this.spOneKeyInbutton.Visibility = Visibility.Visible;
                            this.spOneKeyOutButton.Visibility = Visibility.Collapsed;
                        }
                        else // 一键卸扣
                        {
                            this.spOneKeyInbutton.Visibility = Visibility.Collapsed;
                            this.spOneKeyOutButton.Visibility = Visibility.Visible;
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 融信数据
        /// </summary>
        private void Communcation()
        {
            #region 通信

            if (!GlobalData.Instance.ComunciationNormal) this.tbTips.Text = "网络连接失败！";
            #endregion
        }

        private void Warnning()
        {
            try
            { }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.DEBUG);
            }
        }
    }
}
