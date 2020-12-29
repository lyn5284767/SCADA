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

namespace ControlLibrary.Figure
{
    /// <summary>
    /// WR_SIR_Self_Press.xaml 的交互逻辑
    /// </summary>
    public partial class WR_SIR_Self_Press : UserControl
    {
        RealtimeViewModel viewModel;
        public WR_SIR_Self_Press()
        {
            InitializeComponent();
            viewModel = (RealtimeViewModel)this.DataContext;
        }
        public void AddPoints(double v1,double v2)
        {
            viewModel.AddPoints(v1, v2, 0);
        }
    }
}
