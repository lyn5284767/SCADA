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
    /// SymbolMapping.xaml 的交互逻辑
    /// </summary>
    public partial class SymbolMappingV : UserControl
    {

        public static readonly DependencyProperty LampTypeProperty;
        public static readonly DependencyProperty StrContentProperty;

        public SymbolMappingV()
        {
            InitializeComponent();
        }

        static SymbolMappingV()
        {
            LampTypeProperty = DependencyProperty.Register("LampType", typeof(byte), typeof(SymbolMappingV), new PropertyMetadata((byte)0));
            StrContentProperty = DependencyProperty.Register("StrContent", typeof(string), typeof(SymbolMappingV), new PropertyMetadata((string)"吊卡"));
        }

        public byte LampType
        {
            get { return (byte)GetValue(LampTypeProperty); }
            set { SetValue(LampTypeProperty, value); }
        }

        public string StrContent
        {
            get { return (string)GetValue(StrContentProperty); }
            set { SetValue(StrContentProperty, value); }
        }
    }
}
