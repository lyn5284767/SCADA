using COM.Common;
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

namespace ControlLibrary.InputControl
{
    /// <summary>
    /// SetControl.xaml 的交互逻辑
    /// </summary>
    public partial class SetControl : UserControl
    {
        /// <summary>
        ///  铁架工位置标定事件委托
        /// </summary>
        /// <param name="SetParam"></param>
        public delegate void SFSendProtocol(byte[] SetParam);

        public event SFSendProtocol SFSendProtocolEvent;
        /// <summary>
        /// 名称
        /// </summary>
        public string TbkText { get; set; }
        /// <summary>
        /// 协议头
        /// </summary>
        public string Head { get; set; }

        public string BtnOne { get; set; } = "标定";
        public string BtnTwo { get; set; } = "取消";

        public static readonly DependencyProperty SetControlShowProperty = DependencyProperty.Register("SetControlShow", typeof(object), typeof(SetControl), new PropertyMetadata(0));
        /// <summary>
        /// 控件显示
        /// </summary>
        public object SetControlShow
        {
            get { return GetValue(SetControlShowProperty); }
            set { SetValue(SetControlShowProperty, value); }
        }

        public SetControl()
        {
            InitializeComponent();
            this.Loaded += SetControl_Loaded;
        }

        private void SetControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbk.Text = this.TbkText;
            this.textBoxSet.Content = this.BtnOne;
            this.textBoxcanel.Content = this.BtnTwo;
        }

        private void TextBoxSet_MouseDown(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] strs = this.Head.Split(',');
                for (int i = 0; i < strs.Length; i++) // 协议头
                {
                    GlobalData.Instance.SetParam[i] = byte.Parse(strs[i]);
                }
                if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    // 协议内容
                    GlobalData.Instance.SetParam[strs.Length] = 1;
                }
                else if (GlobalData.Instance.systemType == SystemType.SecondFloor)
                {
                    GlobalData.Instance.SetParam[strs.Length] =2;
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    GlobalData.Instance.SetParam[strs.Length] = 2;
                }
                // 补零
                for (int i = strs.Length + 1; i < 10; i++)
                {
                    GlobalData.Instance.SetParam[i] = 0;
                }
                if (GlobalData.Instance.systemType == SystemType.SecondFloor && SFSendProtocolEvent != null)
                {
                    SFSendProtocolEvent(GlobalData.Instance.SetParam);
                }
                else
                {
                    GlobalData.Instance.da.SendBytes(GlobalData.Instance.SetParam);
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void TextBoxCancel_MouseDown(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] strs = this.Head.Split(',');
                for (int i = 0; i < strs.Length; i++) // 协议头
                {
                    GlobalData.Instance.SetParam[i] = byte.Parse(strs[i]);
                }
                if (GlobalData.Instance.systemType == SystemType.SIR)
                {
                    // 协议内容
                    GlobalData.Instance.SetParam[strs.Length] = 0;
                }
                else if (GlobalData.Instance.systemType == SystemType.SecondFloor)
                {
                    GlobalData.Instance.SetParam[strs.Length] = 1;
                }
                else if (GlobalData.Instance.systemType == SystemType.DrillFloor)
                {
                    GlobalData.Instance.SetParam[strs.Length] = 1;
                }
                // 补零
                for (int i = strs.Length + 1; i < 10; i++)
                {
                    GlobalData.Instance.SetParam[i] = 0;
                }
                GlobalData.Instance.da.SendBytes(GlobalData.Instance.SetParam);
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
