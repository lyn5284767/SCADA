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
    public partial class SymbolMapping : UserControl
    {

        public static readonly DependencyProperty LampTypeProperty;
        public static readonly DependencyProperty StrContentProperty;

        public SymbolMapping()
        {
            InitializeComponent();
        }

        static SymbolMapping()
        {
            LampTypeProperty = DependencyProperty.Register("LampType", typeof(byte), typeof(SymbolMapping), new PropertyMetadata((byte)0));
            StrContentProperty = DependencyProperty.Register("StrContent", typeof(string), typeof(SymbolMapping), new PropertyMetadata((string)"吊卡"));
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
