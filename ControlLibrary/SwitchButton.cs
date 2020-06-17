using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ControlLibrary
{
    public class SwitchButton: CheckBox
    {
        public static readonly DependencyProperty ContentLeftProperty =
DependencyProperty.Register("ContentLeft", typeof(object), typeof(SwitchButton), null);

        public object ContentLeft
        {
            get { return GetValue(ContentLeftProperty); }
            set { SetValue(ContentLeftProperty, value); }
        }

        public static readonly DependencyProperty ContentRightProperty =
DependencyProperty.Register("ContentRight", typeof(object), typeof(SwitchButton), null);

        public object ContentRight
        {
            get { return GetValue(ContentRightProperty); }
            set { SetValue(ContentRightProperty, value); }
        }

        public static readonly DependencyProperty ContentDownProperty =
DependencyProperty.Register("ContentDown", typeof(object), typeof(SwitchButton), null);

        public object ContentDown
        {
            get { return GetValue(ContentDownProperty); }
            set { SetValue(ContentDownProperty, value); }
        }

//        public static readonly DependencyProperty IsCheckedProperty =
//DependencyProperty.Register("IsChecked", typeof(bool), typeof(SwitchButton), new PropertyMetadata((bool)false));

//        public bool IsChecked
//        {
//            get { return (bool)GetValue(ContentDownProperty); }
//            set { SetValue(ContentDownProperty, value); }
//        }

    }
}
