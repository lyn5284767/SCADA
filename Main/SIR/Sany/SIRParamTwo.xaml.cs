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
    /// SIRParamTwo.xaml 的交互逻辑
    /// </summary>
    public partial class SIRParamTwo : UserControl
    {
        private static SIRParamTwo _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRParamTwo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRParamTwo();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRParamTwo()
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
                #region 自动对缺参数
                this.twtL13.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonBeginSpeedSet"], Mode = BindingMode.OneWay });
                this.twtL14.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonMidSpeedSet"], Mode = BindingMode.OneWay });
                this.twtL15.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonBufferSpeedSet"], Mode = BindingMode.OneWay });
                this.twtL16.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonBeginSpeedSet"], Mode = BindingMode.OneWay });
                this.twtL17.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonMidSpeedSet"], Mode = BindingMode.OneWay });
                this.twtL18.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonBufferSpeedSet"], Mode = BindingMode.OneWay });
                #endregion

                #region 悬臂运动参数
                this.twtR1.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfReachSpeedSet"], Mode = BindingMode.OneWay });
                this.twtR2.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfRetractSpeedSet"], Mode = BindingMode.OneWay });
                this.twtR3.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfClimbSpeedSet"], Mode = BindingMode.OneWay });
                this.twtR4.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfClimbCircleSet"], Mode = BindingMode.OneWay });
                this.twtR5.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfStableReachSpeedSet"], Mode = BindingMode.OneWay });
                this.twtR6.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfStableRetractSpeedSet"], Mode = BindingMode.OneWay });
                this.twtR7.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBufferSpeedSet"], Mode = BindingMode.OneWay });
                this.twtR8.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfResponseCircleSet"], Mode = BindingMode.OneWay });
                this.twtR9.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkCylinderLowAlarmSet"], Mode = BindingMode.OneWay,Converter = new DivideTenConverter() });
                this.twtR10.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrakingCylinderLowAlarmSet"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                #endregion

                #region 回转运动参数
                this.twtR11.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkLeftRotateSet"], Mode = BindingMode.OneWay });
                this.twtR12.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWirkRightRotateSet"], Mode = BindingMode.OneWay });
                this.twtR13.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfNotWorkLeftRotateSet"], Mode = BindingMode.OneWay });
                this.twtR14.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfNotWirkRightRotateSet"], Mode = BindingMode.OneWay });
                this.twtR15.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfLeftMaxRotatePosSet"], Mode = BindingMode.OneWay });
                this.twtR16.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfRightMaxRotatePosSet"], Mode = BindingMode.OneWay });
                #endregion

                #region 立柱参数
                this.twtR17.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeUpSpeedSet"], Mode = BindingMode.OneWay });
                this.twtR18.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeDownSpeedSet"], Mode = BindingMode.OneWay });
                this.twtR19.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfPipeUpHeightSet"], Mode = BindingMode.OneWay });
                this.twtR20.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIRSelfPipeDownHeightSet"], Mode = BindingMode.OneWay });
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
