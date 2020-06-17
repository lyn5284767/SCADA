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

namespace Main
{
    /// <summary>
    /// SFDrillCollarSample.xaml 的交互逻辑
    /// </summary>
    public partial class SFDrillCollarSample : UserControl
    {
        private static SFDrillCollarSample _instance = null;
        private static readonly object syncRoot = new object();

        public static SFDrillCollarSample Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFDrillCollarSample();
                        }
                    }
                }
                return _instance;
            }
        }

        public SFDrillCollarSample()
        {
            InitializeComponent();
            VariableBinding();
            this.Loaded += SFDrillCollarSample_Loaded;
        }

        private void SFDrillCollarSample_Loaded(object sender, RoutedEventArgs e)
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
                #region 钻铤锁电机采样值
                this.txt_drillCollarSampleLeft1.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["112N23E1DrillCollarMotorSamplingN1"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleLeft2.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["112N23E1DrillCollarMotorSamplingN2"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleLeft3.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["112N23E1DrillCollarMotorSamplingN3"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleLeft4.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["112N23E1DrillCollarMotorSamplingN4"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleLeft5.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["113N23E1DrillCollarMotorSamplingN5"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleLeft6.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["113N23E1DrillCollarMotorSamplingN6"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleLeft7.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["113N23E1DrillCollarMotorSamplingN7"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleLeft8.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["113N23E1DrillCollarMotorSamplingN8"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleRight1.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["114N23E1DrillCollarMotorSamplingN9"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleRight2.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["114N23E1DrillCollarMotorSamplingN10"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleRight3.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["114N23E1DrillCollarMotorSamplingN11"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleRight4.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["114N23E1DrillCollarMotorSamplingN12"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleRight5.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["114N23E1DrillCollarMotorSamplingN13"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleRight6.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["114N23E1DrillCollarMotorSamplingN14"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleRight7.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["114N23E1DrillCollarMotorSamplingN15"], Mode = BindingMode.OneWay });
                this.txt_drillCollarSampleRight8.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["114N23E1DrillCollarMotorSamplingN16"], Mode = BindingMode.OneWay });
                this.txt_retainingRopeSampleLeft.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["111N23E1LeftRetainingRopeMotorSampling"], Mode = BindingMode.OneWay });
                this.txt_retainingRopeSampleRight.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["111N23E1RightRetainingRopeMotorSampling"], Mode = BindingMode.OneWay });
                #endregion
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
