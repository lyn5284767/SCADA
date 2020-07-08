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
    /// SFDeviceStatus.xaml 的交互逻辑
    /// </summary>
    public partial class SFDeviceStatus : UserControl
    {

        public delegate void SwitchDeviceStatusPageHandler(int pageId);

        public event SwitchDeviceStatusPageHandler SwitchDeviceStatusPageEvent;

        private static SFDeviceStatus _instance = null;
        private static readonly object syncRoot = new object();

        public static SFDeviceStatus Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFDeviceStatus();
                        }
                    }
                }
                return _instance;
            }
        }


        public SFDeviceStatus()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFDeviceStatus_Loaded;
        }

        private void SFDeviceStatus_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalData.Instance.systemRole > SystemRole.TechMan)
            {
                this.miClearDrillDownCnt.Visibility = Visibility.Collapsed;
                this.miClearDrillUpCnt.Visibility = Visibility.Collapsed;
                this.miClearStartUpTime.Visibility = Visibility.Collapsed;
                this.miClearWorkTime.Visibility = Visibility.Collapsed;
            }
            //VariableBinding();
        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.opModel_EquiptStatusPage.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
                this.wokeModel_EquiptStatusPage.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
                this.startUpTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["165To168DeviceBootTime"], Mode = BindingMode.OneWay, Converter = new UnitDivide10Converter() });

                this.drillDownCount_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["114N23N1DrillDownCount"], Mode = BindingMode.OneWay });
                this.drillUpCount_EquipStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["114N23N1DrillUpCount"], Mode = BindingMode.OneWay });
                this.workTime_EquipStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["106N23WorkTime"], Mode = BindingMode.OneWay, Converter = new UnitDivide10Converter() });

                this.lampType_carMotorRet_EquiptStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["carMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.lampType_carMotorProximitySwitch_EquiptStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N23B4b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.lampType_RotateMotorRet_EquipStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.lampType_RotateMotorProximitySwitch_EquiptStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N23B4b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.lampType_ArmMotorRet_EquiptStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["armMotorRetZeroStatus"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.lampType_ArmMotorProximitySwitch_EquiptStatus.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["105N23B4b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
                this.txt_carMotorPosi_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["111N23N1CarMotorCurrentPositionPulse"], Mode = BindingMode.OneWay });
                this.txt_CarMotorWarnCode_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("UShortTag") { Source = GlobalData.Instance.da["111N23N1CarMotorAlarmCode"], Mode = BindingMode.OneWay });
                this.txt_ArmMotorPosi_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["112N23N1ArmMotorCurrentPositionPulse"], Mode = BindingMode.OneWay });
                this.txt_ArmMotorWarnCode_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("UShortTag") { Source = GlobalData.Instance.da["112N23N1ArmMotorAlarmCode"], Mode = BindingMode.OneWay });
                this.txt_RotateMotorPosi_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["113N23N1RotateMotorCurrentPositionPulse"], Mode = BindingMode.OneWay });
                this.txt_RotateMotorWarnCode_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("UShortTag") { Source = GlobalData.Instance.da["113N23N1RotateMotorAlarmCode"], Mode = BindingMode.OneWay });
                this.txt_HandleX_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["142LeftHandleLeftRightSize"], Mode = BindingMode.OneWay });
                this.txt_HandleY_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["144LeftHandleForwardBackwardSize"], Mode = BindingMode.OneWay });
                this.txt_RotateMotorStatus_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["113NRotateMotorStatus"], Mode = BindingMode.OneWay, Converter = new MotorStatusWarnInfoConverter() });
                this.txt_CarMotorStatus_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111N23N1CarMotorStatus"], Mode = BindingMode.OneWay, Converter = new MotorStatusWarnInfoConverter() });
                this.txt_ArmMotorStatus_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["112N23N1ArmMotorStatus"], Mode = BindingMode.OneWay, Converter = new MotorStatusWarnInfoConverter() });

                #region 操作台按键数据
                BoolTagConverter boolTagConverter = new BoolTagConverter();
                //this.lampType_LeftButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("ByteTag") { Source = da["145LeftHandleButtonData"], Mode = BindingMode.OneWay, Converter = new HandleKeyConverter(), ConverterParameter = "left" });
                //this.lampType_RightButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("ByteTag") { Source = da["145LeftHandleButtonData"], Mode = BindingMode.OneWay, Converter = new HandleKeyConverter(), ConverterParameter = "right" });
                //this.lampType_EnableButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = da["146LeftHandleEnable"], Mode = BindingMode.OneWay, Converter = new HandleKeyConverter(), ConverterParameter = "enable" });
                this.lampType_LeftButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleLeftBtn"], Mode = BindingMode.OneWay, Converter = boolTagConverter });
                this.lampType_RightButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleRightBtn"], Mode = BindingMode.OneWay, Converter = boolTagConverter });
                this.lampType_FrontButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleFrontBtn"], Mode = BindingMode.OneWay, Converter = boolTagConverter });
                this.lampType_BehindButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleBehindBtn"], Mode = BindingMode.OneWay, Converter = boolTagConverter });
                this.lampType_EnableButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["146LeftHandleEnable"], Mode = BindingMode.OneWay, Converter = boolTagConverter });
                this.lampType_BackButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleBackBtn"], Mode = BindingMode.OneWay, Converter = boolTagConverter });
                this.lampType_UpButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleUpBtn"], Mode = BindingMode.OneWay, Converter = boolTagConverter });
                this.lampType_DownButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleDownBtn"], Mode = BindingMode.OneWay, Converter = boolTagConverter });
                this.lampType_UpSeesawButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleUpSeesawBtn"], Mode = BindingMode.OneWay, Converter = boolTagConverter });
                this.lampType_DownSeesawButton_EquiptStatus.SetBinding(SymbolMappingV.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleDownSeesawBtn"], Mode = BindingMode.OneWay, Converter = boolTagConverter });

                ArrowDirectionMultiConverter arrowDirectionMultiConverter = new ArrowDirectionMultiConverter();
                MultiBinding upArrowMultiBind = new MultiBinding();
                upArrowMultiBind.Converter = arrowDirectionMultiConverter;
                upArrowMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["144LeftHandleForwardBackwardSize"], Mode = BindingMode.OneWay });
                upArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleFrontBtn"], Mode = BindingMode.OneWay });
                upArrowMultiBind.ConverterParameter = "up";
                upArrowMultiBind.NotifyOnSourceUpdated = true;
                this.upArrow_EquiptStatus.SetBinding(Image.SourceProperty, upArrowMultiBind);

                MultiBinding downArrowMultiBind = new MultiBinding();
                downArrowMultiBind.Converter = arrowDirectionMultiConverter;
                downArrowMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["144LeftHandleForwardBackwardSize"], Mode = BindingMode.OneWay });
                downArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleBehindBtn"], Mode = BindingMode.OneWay });
                downArrowMultiBind.ConverterParameter = "down";
                downArrowMultiBind.NotifyOnSourceUpdated = true;
                this.downArrow_EquiptStatus.SetBinding(Image.SourceProperty, downArrowMultiBind);

                MultiBinding leftArrowMultiBind = new MultiBinding();
                leftArrowMultiBind.Converter = arrowDirectionMultiConverter;
                leftArrowMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["142LeftHandleLeftRightSize"], Mode = BindingMode.OneWay });
                leftArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleLeftBtn"], Mode = BindingMode.OneWay });
                leftArrowMultiBind.ConverterParameter = "left";
                leftArrowMultiBind.NotifyOnSourceUpdated = true;
                this.leftArrow_EquiptStatus.SetBinding(Image.SourceProperty, leftArrowMultiBind);

                MultiBinding rightArrowMultiBind = new MultiBinding();
                rightArrowMultiBind.Converter = arrowDirectionMultiConverter;
                rightArrowMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["142LeftHandleLeftRightSize"], Mode = BindingMode.OneWay });
                rightArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["LeftHandleRightBtn"], Mode = BindingMode.OneWay });
                rightArrowMultiBind.ConverterParameter = "right";
                rightArrowMultiBind.NotifyOnSourceUpdated = true;
                this.rightArrow_EquiptStatus.SetBinding(Image.SourceProperty, rightArrowMultiBind);
                #endregion

                #region 版本信息
                // 设备编码
                DeviceEncodeMultiConverter deviceEncodeMultiConverter = new DeviceEncodeMultiConverter();
                MultiBinding deviceEncodeMultiBind = new MultiBinding();
                deviceEncodeMultiBind.Converter = deviceEncodeMultiConverter;
                deviceEncodeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["DeviceYear"], Mode = BindingMode.OneWay });
                deviceEncodeMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["DeviceModel"], Mode = BindingMode.OneWay });
                deviceEncodeMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["DeviceCarNum"], Mode = BindingMode.OneWay }); ;
                deviceEncodeMultiBind.NotifyOnSourceUpdated = true;
                deviceEncodeMultiBind.ConverterParameter = "RP";
                this.tbDeviceEncode.SetBinding(TextBlock.TextProperty, deviceEncodeMultiBind);
                // 二层台版本
                SecondVersionMultiConverter secondVersionMultiConverter = new SecondVersionMultiConverter();
                MultiBinding secondVersionMultiBind = new MultiBinding();
                secondVersionMultiBind.Converter = secondVersionMultiConverter;
                secondVersionMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SecondMainVersion"], Mode = BindingMode.OneWay });
                secondVersionMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SecondOldVersion"], Mode = BindingMode.OneWay });
                secondVersionMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SecondModifyVersion"], Mode = BindingMode.OneWay }); ;
                secondVersionMultiBind.NotifyOnSourceUpdated = true;
                secondVersionMultiBind.ConverterParameter = "SRP-V";
                this.tbSecondVersion.SetBinding(TextBlock.TextProperty, secondVersionMultiBind);
                // 二层台版本日期
                SecondVersionDateMultiConverter secondVersionDateMultiConverter = new SecondVersionDateMultiConverter();
                MultiBinding secondVersionDateMultiBind = new MultiBinding();
                secondVersionDateMultiBind.Converter = secondVersionDateMultiConverter;
                secondVersionDateMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SecondVersionYear"], Mode = BindingMode.OneWay });
                secondVersionDateMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SecondVersionMonth"], Mode = BindingMode.OneWay });
                secondVersionDateMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["SecondVersionDay"], Mode = BindingMode.OneWay }); ;
                secondVersionDateMultiBind.NotifyOnSourceUpdated = true;
                this.tbSecondVersionDate.SetBinding(TextBlock.TextProperty, secondVersionDateMultiBind);
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

        private void MenuItem_EquipmentStatus_DrillCollar_Click(object sender, RoutedEventArgs e)
        {
            if (SwitchDeviceStatusPageEvent != null)
            {
                SwitchDeviceStatusPageEvent(1);
            }
        }

        private void MenuItem_EquipmentStatus_FingerBeam_Click(object sender, RoutedEventArgs e)
        {
            if (SwitchDeviceStatusPageEvent != null)
            {
                SwitchDeviceStatusPageEvent(2);
            }
        }

        private void MenutItem_EquipmentStatus_ReturnHomePage_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 设备保养确认
        /// </summary>
        private void btn_ConfirmEquiptMaintenance(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 22, 9 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 开机时间清零
        /// </summary>
        private void btn_ClearStartUpTime(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 22, 10 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 工作时间清零
        /// </summary>
        private void btn_ClearWorkTime(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 22, 11 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 下钻次数清零
        /// </summary>
        private void btn_ClearDrillDownCnt(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 22, 12 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 起钻次数清零
        /// </summary>
        private void btn_ClearDrillUpCnt(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 22, 13 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
