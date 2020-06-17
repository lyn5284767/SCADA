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
    /// SFPositionSetting.xaml 的交互逻辑
    /// </summary>
    public partial class SFPositionSetting : UserControl
    {
        private static SFPositionSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static SFPositionSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFPositionSetting();
                        }
                    }
                }
                return _instance;
            }
        }

        public SFPositionSetting()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFPositionSetting_Loaded;
        }

        private void SFPositionSetting_Loaded(object sender, RoutedEventArgs e)
        {
            //VariableBinding();
        }
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            this.txtOpModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
            this.txtWorkModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
            this.txtRotateAngle_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });

            this.returnSelectParaName.SetBinding(TextBox.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["Con_Set1"], Mode = BindingMode.OneWay, Converter = new ReturnSelectParaConverter() });
            this.returnSelectParaValue.SetBinding(TextBox.TextProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["108N23PositionCalibrationValue"], Mode = BindingMode.OneWay });
        }

        const byte bHeadFirst = 0x50;
        const byte bHeadTwo = 0x01;
        /// <summary>
        /// 参数读取
        /// </summary>
        private void btn_ParaRead_LocationCalibration(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = bHeadFirst;
            byteToSend[1] = bHeadTwo;
            byteToSend[2] = 12;
            byteToSend[3] = (byte)selectParaName.TabIndex;
            byteToSend[6] = 1;

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        private void btn_ParaWrite_LocationCalibration(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = bHeadFirst;
            byteToSend[1] = bHeadTwo;
            byteToSend[2] = 12;
            byteToSend[3] = (byte)selectParaName.TabIndex;
            byteToSend[6] = 2;

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 选择设置类型
        /// </summary>
        private void btn_SelectPara_LocationCalibration(object sender, RoutedEventArgs e)
        {
            MenuItem menuObject = (MenuItem)sender;
            if (menuObject != null)
            {
                selectParaName.Text = menuObject.Header.ToString();
                selectParaName.TabIndex = menuObject.TabIndex;
            }
        }
    }
}
