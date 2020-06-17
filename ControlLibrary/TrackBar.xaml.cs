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
    /// TrackBar.xaml 的交互逻辑
    /// </summary>
    public partial class TrackBar : UserControl
    {
        public static readonly DependencyProperty MaxValueProperty;
        public static readonly DependencyProperty MiniValueProperty;
        public static readonly DependencyProperty ValueProperty;


        public TrackBar()
        {
            InitializeComponent();
        }

        static TrackBar()
        {
            MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(int), typeof(TrackBar), new PropertyMetadata((int)10));
            MiniValueProperty = DependencyProperty.Register("MiniValue", typeof(int), typeof(TrackBar), new PropertyMetadata((int)0));
            ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(TrackBar), new PropertyMetadata((int)0));
        }

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public int MiniValue
        {
            get { return (int)GetValue(MiniValueProperty); }
            set { SetValue(MiniValueProperty, value); }
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set {
                SetValue(ValueProperty, value);
            }
        }

    }
}
