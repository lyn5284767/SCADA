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
    /// ParamSetting.xaml 的交互逻辑
    /// </summary>
    public partial class SFParamOne : UserControl
    {
        private static SFParamOne _instance = null;
        private static readonly object syncRoot = new object();

        public static SFParamOne Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFParamOne();
                        }
                    }
                }
                return _instance;
            }
        }

        public SFParamOne()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += ParamSetting_Loaded;
        }

        private void ParamSetting_Loaded(object sender, RoutedEventArgs e)
        {
            //VariableBinding();
        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                #region 配置参数
                this.twtL1.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["103E23B3"], Mode = BindingMode.OneWay });
                this.twtL2.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DeviceModel"], Mode = BindingMode.OneWay });
                this.twtL3.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["103E23B2"], Mode = BindingMode.OneWay });
                this.twtL4.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["JibModel"], Mode = BindingMode.OneWay });
                this.twtL5.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["MachineHandRestContinueTime"], Mode = BindingMode.OneWay });
                this.twtLDeviceYear.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["DeviceYear"], Mode = BindingMode.OneWay });
                this.twtLDeviceEncode.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["DeviceCarNum"], Mode = BindingMode.OneWay });
                this.twtL6.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["107E23DrillPipeFingerBeamWallThick"], Mode = BindingMode.OneWay });
                this.twtL7.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["107E23DrillPipeFingerBeamWidth"], Mode = BindingMode.OneWay });
                this.twtL8.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["107E23DrillCollarFingerBeamWallThick"], Mode = BindingMode.OneWay });
                this.twtL9.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["107E23DrillCollarFingerBeamWidth"], Mode = BindingMode.OneWay });
                this.twtL10.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["103E23B4"], Mode = BindingMode.OneWay });
                this.twtL11.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["103E23B5"], Mode = BindingMode.OneWay });
                this.twtL12.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["LimitElectric"], Mode = BindingMode.OneWay });
                //this.twtL13.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = Global.Instance.da["108E23Inch238DrillPipeCapacity"], Mode = BindingMode.OneWay });
                //this.twtL14.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = Global.Instance.da["108E23Inch25DrillPipeCapacity"], Mode = BindingMode.OneWay });
                //this.twtL15.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = Global.Instance.da["108E23Inch278DrillPipeCapcity"], Mode = BindingMode.OneWay });
                //this.twtL16.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = Global.Instance.da["108E23Inch3DrillPipeCapacity"], Mode = BindingMode.OneWay });
                this.twtL17.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111E23Inch35DrillPipeCapacity"], Mode = BindingMode.OneWay });
                //this.twtL18.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = Global.Instance.da["111E23Inch378DrillPipeCapacity"], Mode = BindingMode.OneWay });
                this.twtL19.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111E23Inch4DrillPipeCapacity"], Mode = BindingMode.OneWay });
                this.twtL20.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111E23Inch45DrillPipeCapacity"], Mode = BindingMode.OneWay });
                this.twtL21.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111E23Inch5DrillPipeCapacity"], Mode = BindingMode.OneWay });
                this.twtL22.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111E23Inch55DrillPipeCapacity"], Mode = BindingMode.OneWay });
                this.twtL23.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["6Inch5/8DrillPipeCapacity"], Mode = BindingMode.OneWay });

                this.twtR1.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["108E23MaxDrillPipeTypeSize"], Mode = BindingMode.OneWay });
                this.twtR2.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["108E23MaxColumn"], Mode = BindingMode.OneWay });
                this.twtR3.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["103E23CarMotorSingleWork"], Mode = BindingMode.OneWay });
                this.twtR4.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["104E23CarMotorReduceRatio"], Mode = BindingMode.OneWay });
                this.twtR5.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["104E23CarMotorNoLoadMaxSpeed"], Mode = BindingMode.OneWay });
                this.twtR6.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["104E23CarMotorLoadDrillPipeMaxSpeed"], Mode = BindingMode.OneWay });
                this.twtR7.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["104E23CarMotorLoadDrillCollarMaxSpeed"], Mode = BindingMode.OneWay });
                this.twtR8.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["105E23ArmReduceRatio"], Mode = BindingMode.OneWay });
                this.twtR9.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["105E23NoLoadMaxSpeed"], Mode = BindingMode.OneWay });
                this.twtR10.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["105E23LoadDrillPipeMaxSpeed"], Mode = BindingMode.OneWay });
                this.twtR11.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["105E23LoadDrillCollarMaxSpeed"], Mode = BindingMode.OneWay });
                this.twtR12.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["109ArmHorizalMaxDistance"], Mode = BindingMode.OneWay });
                this.twtR13.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["106E23RotateReduceRatio"], Mode = BindingMode.OneWay });
                this.twtR14.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["106E23RotateMotorNoLoadMaxSpeed"], Mode = BindingMode.OneWay });
                this.twtR15.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["106E23RotateMotorLoadDrillPipeMaxSpeed"], Mode = BindingMode.OneWay });
                this.twtR16.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["106E23RotateMotorLoadDrillCollarMaxSpeed"], Mode = BindingMode.OneWay });
                //this.twtR17.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["171LeftHandleConfigurationInformation"], Mode = BindingMode.OneWay });
                //this.twtR18.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["172RightHandleConfigurationInformation"], Mode = BindingMode.OneWay });
                //this.twtR19.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["KeyPanelSetting"], Mode = BindingMode.OneWay });
                //this.twtR20.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["OperPanelSetting"], Mode = BindingMode.OneWay });
                //this.twtR21.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["DRTelectrSetting"], Mode = BindingMode.OneWay });
                ////this.twtR22.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = Global.Instance.da["IntegrateOrSingle"], Mode = BindingMode.OneWay });
                #endregion
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        const byte bHeadFirst = 0x50;
        const byte bHeadTwo = 0x01;
        /// <summary>
        /// 组协议
        /// </summary>
        /// <param name="list">协议字符列表</param>
        /// <returns></returns>
        private byte[] SendByte(List<byte> list)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = bHeadFirst; // 默认0位80
            byteToSend[1] = bHeadTwo;   // 默认1位1
            // 2位之后传进来
            for (int i = 0; i < list.Count; i++)
            {
                byteToSend[i + 2] = list[i];
            }
            return byteToSend;
        }
        private byte[] bConfigParameter = new byte[3];
        private void Button_tabItemParameterConfiguration_Confirm(object sender, RoutedEventArgs e)
        {
            bConfigParameter = GlobalData.Instance.ConfigParameter;
            if (bConfigParameter[0] != 0)
            {
                byte[] byteToSend = SendByte(new List<byte> { 23, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2] });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
