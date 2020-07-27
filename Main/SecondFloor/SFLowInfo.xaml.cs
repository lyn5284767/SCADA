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

namespace Main.SecondFloor
{
    /// <summary>
    /// SFLowInfo.xaml 的交互逻辑
    /// </summary>
    public partial class SFLowInfo : UserControl
    {
        private static SFLowInfo _instance = null;
        private static readonly object syncRoot = new object();

        public static SFLowInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFLowInfo();
                        }
                    }
                }
                return _instance;
            }
        }


        public SFLowInfo()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFLowInfo_Loaded;
        }

        private void SFLowInfo_Loaded(object sender, RoutedEventArgs e)
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
                // 操作台版本号
                OperVersionMultiConverter operVersionMultiConverter = new OperVersionMultiConverter();
                MultiBinding operVersionMultiBind = new MultiBinding();
                operVersionMultiBind.Converter = operVersionMultiConverter;
                operVersionMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["OperMainVersion"], Mode = BindingMode.OneWay });
                operVersionMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["OperOldVersion"], Mode = BindingMode.OneWay });
                operVersionMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["OperModifyVersion"], Mode = BindingMode.OneWay }); ;
                operVersionMultiBind.NotifyOnSourceUpdated = true;
                this.tbOperVersion.SetBinding(TextBlock.TextProperty, operVersionMultiBind);
                // 操作台版本日期
                OperVersionDateMultiConverter operVersionDateMultiConverter = new OperVersionDateMultiConverter();
                MultiBinding operVersionDateMultiBind = new MultiBinding();
                operVersionDateMultiBind.Converter = operVersionDateMultiConverter;
                operVersionDateMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["OperVersionYear"], Mode = BindingMode.OneWay });
                operVersionDateMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["OperVersionMonth"], Mode = BindingMode.OneWay });
                operVersionDateMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["OperVersionDay"], Mode = BindingMode.OneWay }); ;
                operVersionDateMultiBind.NotifyOnSourceUpdated = true;
                this.tbOperVersionDate.SetBinding(TextBlock.TextProperty, operVersionDateMultiBind);


                // 钻台面设备编码
                DeviceEncodeMultiConverter drDeviceEncodeMultiConverter = new DeviceEncodeMultiConverter();
                MultiBinding drDeviceEncodeMultiBind = new MultiBinding();
                drDeviceEncodeMultiBind.Converter = drDeviceEncodeMultiConverter;
                drDeviceEncodeMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDeviceYear"], Mode = BindingMode.OneWay });
                drDeviceEncodeMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["drDeviceModel"], Mode = BindingMode.OneWay });
                drDeviceEncodeMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["drDeviceCarNO"], Mode = BindingMode.OneWay }); ;
                drDeviceEncodeMultiBind.NotifyOnSourceUpdated = true;
                drDeviceEncodeMultiBind.ConverterParameter = "FP";
                this.drDeviceEncode.SetBinding(TextBlock.TextProperty, drDeviceEncodeMultiBind);

                // 钻台面版本
                SecondVersionMultiConverter drVersionMultiConverter = new SecondVersionMultiConverter();
                MultiBinding drVersionMultiBind = new MultiBinding();
                drVersionMultiBind.Converter = drVersionMultiConverter;
                drVersionMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drMainVer"], Mode = BindingMode.OneWay });
                drVersionMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSecondVer"], Mode = BindingMode.OneWay });
                drVersionMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drModifyVer"], Mode = BindingMode.OneWay }); ;
                drVersionMultiBind.NotifyOnSourceUpdated = true;
                drVersionMultiBind.ConverterParameter = "SFP-V";
                this.drVersion.SetBinding(TextBlock.TextProperty, drVersionMultiBind);
                // 钻台面版本日期
                OperVersionDateMultiConverter drVersionDateMultiConverter = new OperVersionDateMultiConverter();
                MultiBinding drVersionDateMultiBind = new MultiBinding();
                drVersionDateMultiBind.Converter = drVersionDateMultiConverter;
                drVersionDateMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drVerYear"], Mode = BindingMode.OneWay });
                drVersionDateMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drVerMonth"], Mode = BindingMode.OneWay });
                drVersionDateMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drVerDay"], Mode = BindingMode.OneWay }); ;
                drVersionDateMultiBind.NotifyOnSourceUpdated = true;
                this.drVersionDate.SetBinding(TextBlock.TextProperty, drVersionDateMultiBind);

                this.startUpTime.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["165To168DeviceBootTime"], Mode = BindingMode.OneWay, Converter = new UnitDivide10Converter() });
                this.drillDownCount_EquiptStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["114N23N1DrillDownCount"], Mode = BindingMode.OneWay });
                this.drillUpCount_EquipStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["114N23N1DrillUpCount"], Mode = BindingMode.OneWay });
                this.workTime_EquipStatus.SetBinding(TextBlock.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["106N23WorkTime"], Mode = BindingMode.OneWay, Converter = new UnitDivide10Converter() });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
