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
    /// SIRPosSetting.xaml 的交互逻辑
    /// </summary>
    public partial class SIRPosSetting : UserControl
    {
        private static SIRPosSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRPosSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRPosSetting();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRPosSetting()
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
                this.twtL1.SetBinding(TextBlockWithBtn.ShowTxtProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfWellPosSet"], Mode = BindingMode.OneWay });
                this.twtL2.SetBinding(TextBlockWithBtn.ShowTxtProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIRSelfMousePosSet"], Mode = BindingMode.OneWay });
                this.twtL3.SetBinding(TextBlockWithBtn.ShowTxtProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfTopPosSet"], Mode = BindingMode.OneWay });
                this.twtL4.SetBinding(TextBlockWithBtn.ShowTxtProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfStayPosSet"], Mode = BindingMode.OneWay });
                this.twtL5.SetBinding(TextBlockWithBtn.ShowTxtProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfRecoveryPosSet"], Mode = BindingMode.OneWay });
                this.twtL6.SetBinding(TextBlockWithBtn.ShowTxtProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfPipePosSet"], Mode = BindingMode.OneWay });
                this.twtL7.SetBinding(TextBlockWithBtn.ShowTxtProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfAntiInterferePosSet"], Mode = BindingMode.OneWay });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void TwtL1_textBoxSetMouseDownEvent(int crtTag, string txt)
        {
            if (crtTag > 0)
            {
                //if (txt == "读取")
                //{
                //    byte[] byteToSend = GlobalData.Instance.SendListToByte(new List<byte>() { 80, 33, 12, (byte)crtTag, 0, 0, 2 });
                //    GlobalData.Instance.da.SendBytes(byteToSend);
                //}
                //else
                //{
                //    byte[] byteToSend = GlobalData.Instance.SendListToByte(new List<byte>() { 80, 33, 12, (byte)crtTag, 0, 0, 1 });
                //    GlobalData.Instance.da.SendBytes(byteToSend);
                //}
                byte[] byteToSend = new byte[] { 24, 17, 8, (byte)crtTag, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        private byte[] bConfigParameter = new byte[4];
        private void Button_Setting(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = new byte[] { 24, 17, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2], bConfigParameter[3], 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
