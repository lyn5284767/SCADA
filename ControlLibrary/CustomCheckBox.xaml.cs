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
    /// CustomCheckBox.xaml 的交互逻辑
    /// </summary>
    public partial class CustomCheckBox : UserControl
    {
        public event EventHandler UserControlClicked;

        public CustomCheckBox()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty ContentStringProperty =
DependencyProperty.Register("ContentString", typeof(object), typeof(CustomCheckBox), null);

        public object ContentString
        {
            get { return GetValue(ContentStringProperty); }
            set { SetValue(ContentStringProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
DependencyProperty.Register("IsChecked", typeof(bool), typeof(CustomCheckBox), new PropertyMetadata((bool)false));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        private void btn_ClickButton(object sender, RoutedEventArgs e)
        {
            if (UserControlClicked != null)
            {
                UserControlClicked(this, EventArgs.Empty);
            }
        }
    }
}
