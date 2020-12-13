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
    /// SIRCosingTorque.xaml 的交互逻辑
    /// </summary>
    public partial class SIRCosingTorque : UserControl
    {
        RealtimeViewModel viewModel;
        public SIRCosingTorque()
        {
            InitializeComponent();
            viewModel = (RealtimeViewModel)this.DataContext;
        }
        public void AddPoints(double v1)
        {
            viewModel.AddPoints(v1);
            this.txtTitle.Content = "套管扭矩(" + v1 + ")";
        }

        public void AddPoints(double v1,double v2)
        {
            viewModel.AddPoints(v1);
            this.txtTitle.Content = "套管扭矩(" + v1 + ")" + "  最大值:(" + v2 + ")";
        }

        public void ShowMax(double v)
        {
            this.txtTitle.Content = "套管扭矩(0)" + "  最大值:(" + v + ")";
        }

        public void ClearPoint()
        {
            viewModel.ClearPoint();
            this.txtTitle.Content = "套管扭矩(0)";
        }
    }
}
