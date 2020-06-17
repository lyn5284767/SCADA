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

namespace ControlLibrary
{
    /// <summary>
    /// TrafficLights.xaml 的交互逻辑
    /// </summary>
    public partial class TrafficLights : UserControl
    {
        public static readonly DependencyProperty IndexProperty;

        public TrafficLights()
        {
            InitializeComponent();
        }

        static TrafficLights()
        {
            IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(TrafficLights), new PropertyMetadata((int)0));
        }

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }
    }
}
