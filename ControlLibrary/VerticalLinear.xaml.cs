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
    /// VerticalLinear.xaml 的交互逻辑
    /// </summary> 
    public partial class VerticalLinear : UserControl
    {
        public static readonly DependencyProperty CurValueProperty;

        public VerticalLinear()
        {
            InitializeComponent();
        }

        static VerticalLinear()
        {
            CurValueProperty = DependencyProperty.Register("CurValue", typeof(int), typeof(VerticalLinear), new PropertyMetadata((int)50));
        }

        public int CurValue
        {
            get { return (int)GetValue(CurValueProperty); }
            set { SetValue(CurValueProperty, value); }
        }
    }
}
