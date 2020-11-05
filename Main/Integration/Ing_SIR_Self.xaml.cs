using COM.Common;
using ControlLibrary;
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

namespace Main.Integration
{
    /// <summary>
    /// Ing_SIR_Self.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_SIR_Self : UserControl
    {
        private static Ing_SIR_Self _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_SIR_Self Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_SIR_Self();
                        }
                    }
                }
                return _instance;
            }
        }
        public Ing_SIR_Self()
        {
            InitializeComponent();
            SIRVariableBinding();
            SIRVariableReBinding = new System.Threading.Timer(new TimerCallback(SIRVariableTimer), null, Timeout.Infinite, 500);
            SIRVariableReBinding.Change(0, 500);
        }

        #region 铁钻工
        bool workModelCheck = false;
        byte bworkModel = 0;
        System.Threading.Timer SIRVariableReBinding;

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void SIRVariableBinding()
        {
            try
            {
                this.siroprModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfOperationModelConverter() });
                this.siroprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOperModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                this.sirworkModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                this.sirworkModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfIsCheckConverter() });
                this.sirPipeTypeModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeModelConverter() });
                this.sirPipeTypeModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeType"], Mode = BindingMode.OneWay, Converter = new SIRSelfPipeTypeIsCheckConverter() });
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
        private void btn_SIRoprModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.siroprModel.IsChecked) //当前手动状态
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
        private void btn_SIRworkModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.sirworkModel.IsChecked) //当前上扣模式
            {
                byteToSend = new byte[10] { 23, 17, 2, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前卸扣模式
            {
                byteToSend = new byte[10] { 23, 17, 2, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
            this.sirworkModel.ContentDown = "切换中";
            workModelCheck = true;
        }
        /// <summary>
        /// 管柱选择
        /// </summary>
        private void btn_SIRPipeTypeModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.sirPipeTypeModel.IsChecked) //当前钻杠
            {
                byteToSend = new byte[10] { 23, 17, 3, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前套管
            {
                byteToSend = new byte[10] { 23, 17, 3, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void SIRVariableTimer(object value)
        {
            if (GlobalData.Instance.da.GloConfig.SIRType == (int)SIRType.SANY)
            {
                if (bworkModel != GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte && workModelCheck)
                {
                    this.sirworkModel.Dispatcher.Invoke(new Action(() =>
                    {
                        this.sirworkModel.SetBinding(BasedSwitchButton.ContentDownProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkModel"], Mode = BindingMode.OneWay, Converter = new SIRSelfWorkModelConverter() });
                    }));
                    workModelCheck = false;
                }
                bworkModel = GlobalData.Instance.da["SIRSelfWorkModel"].Value.Byte;
            }
        }
        #endregion
    }
}
