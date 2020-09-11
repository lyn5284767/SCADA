using COM.Common;
using ControlLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Binding = System.Windows.Data.Binding;

namespace Main.SecondFloor
{
    /// <summary>
    /// SFChart.xaml 的交互逻辑
    /// </summary>
    public partial class SFChart : System.Windows.Controls.UserControl
    {
        private static SFChart _instance = null;
        private static readonly object syncRoot = new object();

        public static SFChart Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFChart();
                        }
                    }
                }
                return _instance;
            }
        }

        Timer timerChart = new Timer();//改成50ms 的时钟

        public SFChart()
        {
            InitializeComponent();
            timerChart.Interval = 1000;
            timerChart.Tick += TimerChart_Tick;
            timerChart.Start();
            VariableBinding();
            this.Loaded += SFChart_Loaded;
        }

        private void SFChart_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void TimerChart_Tick(object sender, EventArgs e)
        {
            // 电机实时电流
            this.motorElecCurrentChart.AddPoints(GlobalData.Instance.da["109CarMotorElecCurrent"].Value.Int16, GlobalData.Instance.da["109ArmMotorElecCurrent"].Value.Int16, GlobalData.Instance.da["110RotateMotorElecCurrent"].Value.Int16);

            // 电机实时转速
            this.motorSpeedChart.AddPoints(GlobalData.Instance.da["110CarMotorSpeed"].Value.Int16, GlobalData.Instance.da["110ArmMotorSpeed"].Value.Int16, GlobalData.Instance.da["110RotateMotorSpeed"].Value.Int16);
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                this.carMotorElecCurrent.SetBinding(MessageShow.ContentProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["109CarMotorElecCurrent"], Mode = BindingMode.OneWay });
                this.armMotorElecCurrent.SetBinding(MessageShow.ContentProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["109ArmMotorElecCurrent"], Mode = BindingMode.OneWay });
                this.rotateMotorElecCurrent.SetBinding(MessageShow.ContentProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["110RotateMotorElecCurrent"], Mode = BindingMode.OneWay });

                this.carMotorRealTimeSpeed.SetBinding(MessageShow.ContentProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["110CarMotorSpeed"], Mode = BindingMode.OneWay });
                this.armMotorRealTimeSpeed.SetBinding(MessageShow.ContentProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["110ArmMotorSpeed"], Mode = BindingMode.OneWay });
                this.rotateMotorRealTimeSpeed.SetBinding(MessageShow.ContentProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["110RotateMotorSpeed"], Mode = BindingMode.OneWay });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
