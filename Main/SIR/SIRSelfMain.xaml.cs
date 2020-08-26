using COM.Common;
using ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public SIRSelfMain()
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
                this.oprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfOperationModelConverter() });
                this.oprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfOperationModelIsCheckConverter() });
                this.workModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                this.workModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay});
                this.PipeTypeModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeModelConverter() });
                this.PipeTypeModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay });
                this.locationModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfLocationModelConverter() });
                this.locationModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay });

                this.tbOneKeyInButton.SetBinding(ToggleButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOneKeyInButton"], Mode = BindingMode.OneWay });
                this.tbOneKeyOutButton.SetBinding(ToggleButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOneKeyOutButton"], Mode = BindingMode.OneWay });

                //this.tbControlModel.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbLoaction.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbOperModel.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbWorkModel.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbSafeDoor.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                //this.tbGap.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfControlModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });

                this.tbInButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonPress"], Mode = BindingMode.OneWay });
                this.tbOutButtonPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonPress"], Mode = BindingMode.OneWay });
                this.tbInButtonClampingForce.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfInButtonClampingForce"], Mode = BindingMode.OneWay });
                this.tbOutButtonClampingForce.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonClampingForce"], Mode = BindingMode.OneWay });
                this.tbRotate.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfRotate"], Mode = BindingMode.OneWay });
                this.tbSysPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfSysPress"], Mode = BindingMode.OneWay });

                this.tbArmPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfArmPos"], Mode = BindingMode.OneWay });
                this.tbClampHeight.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfClampHeight"], Mode = BindingMode.OneWay });
                this.tbWorkCylinderPress.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkCylinderPress"], Mode = BindingMode.OneWay });
                this.tbBrakingCylinderPress.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrakingCylinderPress"], Mode = BindingMode.OneWay });
                this.tbInButtonTorque.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfInButtonTorque"], Mode = BindingMode.OneWay });
                this.tbOutButtonTorque.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonTorque"], Mode = BindingMode.OneWay });
                this.tbInButtonCircle.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonCircle"], Mode = BindingMode.OneWay });
                this.tbOutButtonCircle.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonCircle"], Mode = BindingMode.OneWay });
                this.tbWorkTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfWorkTime"], Mode = BindingMode.OneWay });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 控制模式
        /// </summary>
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.controlModel.IsChecked) //当前司钻
            {
                //byteToSend = new byte[10] { 24, 16, 2, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前本地
            {
                //byteToSend = new byte[10] { 24, 16, 2, 1, 0, 0, 0, 0, 0, 0 };
            }
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 工作模式
        /// </summary>
        private void btn_workModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.workModel.IsChecked) //当前上扣模式
            {
                byteToSend = new byte[10] { 24, 16, 2, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前卸扣模式
            {
                byteToSend = new byte[10] { 24, 16, 2, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        private void btn_oprModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.controlModel.IsChecked) //当前手动状态
            {
                byteToSend = new byte[10] { 24, 16, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前自动状态
            {
                byteToSend = new byte[10] { 24, 16, 1, 1, 0, 0, 0, 0, 0, 0 };
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
                byteToSend = new byte[10] { 24, 16, 4, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前鼠洞
            {
                byteToSend = new byte[10] { 24, 16, 4, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 管柱选择
        /// </summary>
        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 一键上扣
        /// </summary>
        private void OneKeyInButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tbn = sender as ToggleButton;
            byte[] byteToSend;
            if (tbn.IsChecked.Value) // 开启一键上扣
            {
                byteToSend = new byte[10] { 24, 16, 5, 1, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 24, 16, 5, 1, 0, 0, 0, 0, 0, 0 };
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
            if (tbn.IsChecked.Value) // 开启一键上扣
            {
                byteToSend = new byte[10] { 24, 16, 5, 2, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                byteToSend = new byte[10] { 24, 16, 5, 2, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
