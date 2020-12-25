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
    /// WR_HS_Self_Water.xaml 的交互逻辑
    /// </summary>
    public partial class WR_HS_Self_Water : UserControl
    {
        RealtimeViewModel viewModel;
        public WR_HS_Self_Water()
        {
            InitializeComponent();
            viewModel = (RealtimeViewModel)this.DataContext;
        }

        public void AddPoints(double v1)
        {
            viewModel.AddPoints(v1);
        }
    }
}
