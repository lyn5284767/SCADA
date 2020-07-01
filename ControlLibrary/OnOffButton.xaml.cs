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
    /// OnOffButton.xaml 的交互逻辑
    /// </summary>
    public partial class OnOffButton : UserControl
    {
        public delegate void CBChecked(bool isChecked);

        public event CBChecked CBCheckedEvent;

        public static readonly DependencyProperty OnOffButtonCheckProperty = DependencyProperty.Register("OnOffButtonCheck", typeof(bool), typeof(OnOffButton), new PropertyMetadata(false));
        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool OnOffButtonCheck
        {
            get { return (bool)GetValue(OnOffButtonCheckProperty); }
            set { SetValue(OnOffButtonCheckProperty, value); }
        }

        public OnOffButton()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (CBCheckedEvent != null)
            {
                CBCheckedEvent(checkBox.IsChecked.Value);
            }
        }
    }
}
