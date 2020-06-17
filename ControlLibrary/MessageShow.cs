using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ControlLibrary
{
    public class MessageShow:Button
    {
        public static readonly DependencyProperty ContentUpProperty =
        DependencyProperty.Register("ContentUp", typeof(object), typeof(MessageShow), null);

        public object ContentUp
        {
            get { return GetValue(ContentUpProperty); }
            set { SetValue(ContentUpProperty, value); }
        }

        public static readonly DependencyProperty ContentDownProperty =
DependencyProperty.Register("ContentDown", typeof(object), typeof(MessageShow),null);

        public object ContentDown
        {
            get { return GetValue(ContentDownProperty); }
            set { SetValue(ContentDownProperty, value); }
        }

    }
}
