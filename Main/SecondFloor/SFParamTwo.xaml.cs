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

namespace Main.SecondFloor
{
    /// <summary>
    /// SFParamTwo.xaml 的交互逻辑
    /// </summary>
    public partial class SFParamTwo : UserControl
    {

        private static SFParamTwo _instance = null;
        private static readonly object syncRoot = new object();

        public static SFParamTwo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFParamTwo();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFParamTwo()
        {
            InitializeComponent();
            this.VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {

                this.twtL28.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SPDrillOneWidth"], Mode = BindingMode.OneWay });
                this.twtL29.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SPDrillOneSpace"], Mode = BindingMode.OneWay });
                this.twtL30.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SPDrillTwoWidth"], Mode = BindingMode.OneWay });
                this.twtL31.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SPDrillTwoSpace"], Mode = BindingMode.OneWay });
                this.twtL32.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SPDrillMaxSize"], Mode = BindingMode.OneWay });
                this.twtL50.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SPDrillNum"], Mode = BindingMode.OneWay });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void Twt_CtrGetFocusEvent(int crtTag)
        {

        }
        private byte[] bConfigParameter = new byte[3];
        private void ParamTwoSet(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 23, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2] });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
