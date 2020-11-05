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

namespace Main.Integration
{
    /// <summary>
    /// Ing_SIR_JJC.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_SIR_JJC : UserControl
    {
        private static Ing_SIR_JJC _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_SIR_JJC Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_SIR_JJC();
                        }
                    }
                }
                return _instance;
            }
        }
        public Ing_SIR_JJC()
        {
            InitializeComponent();
            VariableBinding();
        }

        /// <summary>
        /// 变量绑定
        /// </summary>
        private void VariableBinding()
        {
            SIRRealTimePressureCoverter sIRRealTimePressureCoverter = new SIRRealTimePressureCoverter();
            MultiBinding RealTimePressureMultiBind = new MultiBinding();
            RealTimePressureMultiBind.Converter = sIRRealTimePressureCoverter;
            RealTimePressureMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["IDUpButtonPressure"], Mode = BindingMode.OneWay });
            RealTimePressureMultiBind.Bindings.Add(new Binding("Text") { Source = this.tbUnitOne, Mode = BindingMode.OneWay });
            RealTimePressureMultiBind.NotifyOnSourceUpdated = true;
            this.tbRealTimePressure.SetBinding(TextBlock.TextProperty, RealTimePressureMultiBind);
            MultiBinding PressureSetMultiBind = new MultiBinding();
            PressureSetMultiBind.Converter = sIRRealTimePressureCoverter;
            PressureSetMultiBind.Bindings.Add(new Binding("ShortTag") { Source = GlobalData.Instance.da["IDUpButtonPressureSet"], Mode = BindingMode.OneWay });
            PressureSetMultiBind.Bindings.Add(new Binding("Text") { Source = this.tbUnitOne, Mode = BindingMode.OneWay });
            PressureSetMultiBind.NotifyOnSourceUpdated = true;
            this.tbPressureSet.SetBinding(TextBlock.TextProperty, PressureSetMultiBind);         
        }

        /// <summary>
        /// 切换单位
        /// </summary>
        private void SwichUnit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.tbUnitOne.Text == "KN.M") this.tbUnitOne.Text = "kft.lbs";
            else this.tbUnitOne.Text = "KN.M";
            if (this.tbUnitTwo.Text == "KN.M") this.tbUnitTwo.Text = "kft.lbs";
            else this.tbUnitOne.Text = "kft.lbs";
        }

        /// <summary>
        /// 减小压力设定
        /// </summary>
        private void PressureDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 23, 4, 0, 0, 1, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
        /// <summary>
        /// 增加压力设定
        /// </summary>
        private void PressureUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            byte[] data = new byte[10] { 80, 16, 23, 3, 0, 0, 1, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(data);
        }
    }
}
