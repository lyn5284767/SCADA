using COM.Common;
using ControlLibrary.InputControl;
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

namespace Main.SIR.Sany
{
    /// <summary>
    /// SIRParamOne.xaml 的交互逻辑
    /// </summary>
    public partial class SIRParamOne : UserControl
    {
        private static SIRParamOne _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRParamOne Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRParamOne();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRParamOne()
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
                #region 套管参数
                this.twtL5.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingInButtonPressSetValue"], Mode = BindingMode.OneWay });
                this.twtL6.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingInButtonProtect"], Mode = BindingMode.OneWay });
                this.twtL7.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingOutButtonPressSetValue"], Mode = BindingMode.OneWay });
                this.twtL8.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingOutButtonProtect"], Mode = BindingMode.OneWay });
                this.twtL5.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingInButtonPressSetValue"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                this.twtL6.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingInButtonProtect"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                this.twtL7.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingOutButtonPressSetValue"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                this.twtL8.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingOutButtonProtect"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                #endregion
                #region 钻杆类型
                this.twtL9.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonCircleSet"], Mode = BindingMode.OneWay });
                this.twtL10.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonCircleSet"], Mode = BindingMode.OneWay });
                this.twtL1.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeInButtonPressSetValue"], Mode = BindingMode.OneWay });
                this.twtL1.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeInButtonPressSetValue"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                this.twtL2.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeInButtonProtect"], Mode = BindingMode.OneWay });
                this.twtL2.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeInButtonProtect"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                this.twtL11.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonSpeedSet"], Mode = BindingMode.OneWay });
                this.twtL11.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonSpeedSet"], Mode = BindingMode.OneWay, Converter = new CloseMpaToKNmConverter() });
                this.twtL12.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonSpeedSet"], Mode = BindingMode.OneWay });
                this.twtL12.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonSpeedSet"], Mode = BindingMode.OneWay, Converter = new CloseMpaToKNmConverter() });
                this.twtL3.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeOutButtonPressSetValue"], Mode = BindingMode.OneWay });
                this.twtL4.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeOutButtonProtect"], Mode = BindingMode.OneWay });
                this.twtL4.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeOutButtonProtect"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                #endregion

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 确认配置
        /// </summary>
        private void Button_tabItemParameterConfiguration_Confirm(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.SetParam[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SetParam;
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
