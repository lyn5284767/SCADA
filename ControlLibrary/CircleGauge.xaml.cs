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
    /// CircleGauge.xaml 的交互逻辑
    /// </summary>
    public partial class CircleGauge : UserControl
    {
        public static readonly DependencyProperty CurValueProperty;
        public static readonly DependencyProperty TxtProperty;

        public CircleGauge()
        { 
            InitializeComponent();
        }

        static CircleGauge()
        {
            CurValueProperty = DependencyProperty.Register("CurValue", typeof(int), typeof(CircleGauge), new PropertyMetadata((int)0));
            TxtProperty = DependencyProperty.Register("Txt", typeof(string), typeof(CircleGauge), new PropertyMetadata("压力表"));
        }

        public int CurValue
        {
            get { return (int)GetValue(CurValueProperty); }
            set { SetValue(CurValueProperty, value); }
        }

        public string Txt
        {
            get { return (string)GetValue(TxtProperty); }
            set { SetValue(TxtProperty, value); }
        }
    }
}
