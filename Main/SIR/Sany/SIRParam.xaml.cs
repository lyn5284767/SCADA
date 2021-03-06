﻿using COM.Common;
using ControlLibrary;
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

namespace Main.SIR
{
    /// <summary>
    /// SIRParam.xaml 的交互逻辑
    /// </summary>
    public partial class SIRParam : UserControl
    {
        private static SIRParam _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRParam Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRParam();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRParam()
        {
            InitializeComponent();
            InitRole();
            VariableBinding();
        }
        /// <summary>
        /// 权限分级
        /// </summary>
        private void InitRole()
        {
            if (GlobalData.Instance.systemRole != SystemRole.DebugMan)
            {
                this.colOne.Visibility = Visibility.Collapsed;
                this.colTwo.Visibility = Visibility.Collapsed;
                //this.bdAutoParam.Visibility = Visibility.Collapsed;
                //this.twtL13.Visibility = Visibility.Collapsed;
                //this.twtL14.Visibility = Visibility.Collapsed;
                //this.twtL15.Visibility = Visibility.Collapsed;
                //this.twtL16.Visibility = Visibility.Collapsed; 
                //this.twtL17.Visibility = Visibility.Collapsed;
                //this.twtL18.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                #region 套管参数
                this.twtL5.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingInButtonPressSetValue"], Mode = BindingMode.OneWay});
                this.twtL6.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingInButtonProtect"], Mode = BindingMode.OneWay});
                this.twtL7.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingOutButtonPressSetValue"], Mode = BindingMode.OneWay});
                this.twtL8.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingOutButtonProtect"], Mode = BindingMode.OneWay});
                this.twtL5.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingInButtonPressSetValue"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                this.twtL6.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingInButtonProtect"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                this.twtL7.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingOutButtonPressSetValue"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                this.twtL8.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrushingOutButtonProtect"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                #endregion
                #region 钻杆类型
                this.twtL9.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonCircleSet"], Mode = BindingMode.OneWay });
                this.twtL10.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonCircleSet"], Mode = BindingMode.OneWay });
                this.twtL1.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeInButtonPressSetValue"], Mode = BindingMode.OneWay });
                this.twtL1.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeInButtonPressSetValue"], Mode = BindingMode.OneWay,Converter = new HighMpaToKNmConverter() });
                this.twtL2.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeInButtonProtect"], Mode = BindingMode.OneWay });
                this.twtL2.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeInButtonProtect"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                this.twtL11.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonSpeedSet"], Mode = BindingMode.OneWay});
                this.twtL11.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfInButtonSpeedSet"], Mode = BindingMode.OneWay, Converter = new CloseMpaToKNmConverter() });
                this.twtL12.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonSpeedSet"], Mode = BindingMode.OneWay });
                this.twtL12.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfOutButtonSpeedSet"], Mode = BindingMode.OneWay, Converter = new CloseMpaToKNmConverter() });
                this.twtL3.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeOutButtonPressSetValue"], Mode = BindingMode.OneWay });
                this.twtL4.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeOutButtonProtect"], Mode = BindingMode.OneWay});
                this.twtL4.SetBinding(TextWithTips.TransTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeOutButtonProtect"], Mode = BindingMode.OneWay, Converter = new HighMpaToKNmConverter() });
                #endregion

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
                this.twtR9.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfWorkCylinderLowAlarmSet"], Mode = BindingMode.OneWay });
                this.twtR10.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfBrakingCylinderLowAlarmSet"], Mode = BindingMode.OneWay });
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
                this.twtR19.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeUpHeightSet"], Mode = BindingMode.OneWay });
                this.twtR20.SetBinding(TextWithTips.ShowTextWithTipsProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRSelfPipeDownHeightSet"], Mode = BindingMode.OneWay });
                #endregion

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private void Button_tabItemParameterConfiguration_Confirm(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.SetParam[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SetParam;
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        //private byte[] bConfigParameter = new byte[4];
        ///// <summary>
        ///// 确定配置
        ///// </summary>
        //private void Button_Setting(object sender, RoutedEventArgs e)
        //{
        //    bConfigParameter = GlobalData.Instance.ConfigParameter;
        //    if (bConfigParameter[0] != 0)
        //    {
        //        byte[] byteToSend = new byte[] { 24, 17, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2], bConfigParameter[3], 0, 0, 0, 0 };
        //        GlobalData.Instance.da.SendBytes(byteToSend);
        //    }
        //}
    }
}
