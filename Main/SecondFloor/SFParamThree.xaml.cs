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
    /// SFParamThree.xaml 的交互逻辑
    /// </summary>
    public partial class SFParamThree : UserControl
    {
        private static SFParamThree _instance = null;
        private static readonly object syncRoot = new object();

        public static SFParamThree Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFParamThree();
                        }
                    }
                }
                return _instance;
            }
        }

        public SFParamThree()
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
                #region 配置参数
                this.twtR17.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["171LeftHandleConfigurationInformation"], Mode = BindingMode.OneWay });
                this.twtR18.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["172RightHandleConfigurationInformation"], Mode = BindingMode.OneWay });
                this.twtR19.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["KeyPanelSetting"], Mode = BindingMode.OneWay });
                this.twtR20.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["OperPanelSetting"], Mode = BindingMode.OneWay });
                this.twtR21.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["DRTelectrSetting"], Mode = BindingMode.OneWay });
                this.twtR22.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["Kava"], Mode = BindingMode.OneWay });
                this.twtR23.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["IDFactoryType"], Mode = BindingMode.OneWay });
                this.twtR24.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["CatType"], Mode = BindingMode.OneWay });
                this.twtR25.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["HSType"], Mode = BindingMode.OneWay });
                this.twtR26.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["DRType"], Mode = BindingMode.OneWay });
                #endregion
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private byte[] bConfigParameter = new byte[3];
        private void ParamThreeSet(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 23, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2] });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
