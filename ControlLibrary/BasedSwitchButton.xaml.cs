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
    /// BasedSwitchButton.xaml 的交互逻辑
    /// </summary>
    public partial class BasedSwitchButton : UserControl
    {
        public event EventHandler UserControlClicked;

        public BasedSwitchButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ContentLeftProperty =
DependencyProperty.Register("ContentLeft", typeof(object), typeof(BasedSwitchButton), null);

        public object ContentLeft
        {
            get { return GetValue(ContentLeftProperty); }
            set { SetValue(ContentLeftProperty, value); }
        }

        public static readonly DependencyProperty ContentRightProperty =
DependencyProperty.Register("ContentRight", typeof(object), typeof(BasedSwitchButton), null);

        public object ContentRight
        {
            get { return GetValue(ContentRightProperty); }
            set { SetValue(ContentRightProperty, value); }
        }

        public static readonly DependencyProperty ContentDownProperty =
DependencyProperty.Register("ContentDown", typeof(object), typeof(BasedSwitchButton), null);

        public object ContentDown
        {
            get { return GetValue(ContentDownProperty); }
            set { SetValue(ContentDownProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
DependencyProperty.Register("IsChecked", typeof(bool), typeof(BasedSwitchButton), new PropertyMetadata((bool)false));

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
