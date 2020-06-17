using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// StepControl.xaml 的交互逻辑
    /// </summary>
    public partial class StepControl : UserControl
    {
        public int SetpNum { get; set; }

        public static readonly DependencyProperty CtrWidthProperty = DependencyProperty.Register("CtrWidth", typeof(int), typeof(StepControl), new PropertyMetadata(500));
        public static readonly DependencyProperty SelectStepProperty = DependencyProperty.Register("SelectStep", typeof(int), typeof(StepControl), new PropertyMetadata(0));
        public int CtrWidth
        {
            get { return (int)this.GetValue(CtrWidthProperty); }
            set { this.SetValue(CtrWidthProperty, value); }
        }

        public int SelectStep
        {
            get { return (int)this.GetValue(SelectStepProperty); }
            set { this.SetValue(SelectStepProperty, value); }
        }
        public StepControl()
        {
            InitializeComponent();
            this.Loaded += StepControl_Loaded;
        }

        private void StepControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavList.Items.Count == 0)
            {
                for (int i = 0; i < SetpNum; i++)
                {
                    ListBoxItem listBoxItem = new ListBoxItem();

                    NavList.Items.Add(listBoxItem);
                }
            }
            //this.NavList.Width = CtrWidth;
        }
    }

    public class StepListBarWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListBox listBox = value as ListBox;
            if (listBox == null)
            {
                return Binding.DoNothing;
            }
            if (listBox.Items.Count == 0)
            {
                return 0;
            }
            double width = 500.0;
            if (listBox.ActualWidth > 0) width = listBox.ActualWidth;
            return width / listBox.Items.Count;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
