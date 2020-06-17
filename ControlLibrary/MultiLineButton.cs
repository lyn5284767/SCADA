using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ControlLibrary
{
    public class MultiLineThree:Button
    {
        public static readonly DependencyProperty ContentUpProperty =
   DependencyProperty.Register("ContentUp", typeof(object), typeof(MultiLineThree), null);

        public object ContentUp
        {
            get { return GetValue(ContentUpProperty); }
            set { SetValue(ContentUpProperty, value); }
        }

        public static readonly DependencyProperty ContentDownProperty =
DependencyProperty.Register("ContentDown", typeof(object), typeof(MultiLineThree), null);

        public object ContentDown
        {
            get { return GetValue(ContentDownProperty); }
            set { SetValue(ContentDownProperty, value); }
        }

    }

    public class MultiLineTwo : Button
    {
        public static readonly DependencyProperty ContentExProperty =
   DependencyProperty.Register("ContentEx", typeof(object), typeof(MultiLineTwo), null);

        public object ContentEx
        {
            get { return GetValue(ContentExProperty); }
            set { SetValue(ContentExProperty, value); }
        }
    }
}
