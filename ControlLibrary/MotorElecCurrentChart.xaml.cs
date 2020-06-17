﻿using System;
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
    /// MotorElecCurrentChart.xaml 的交互逻辑
    /// </summary>
    public partial class MotorElecCurrentChart : UserControl
    {
        RealtimeViewModel viewModel;
        public MotorElecCurrentChart()
        {
            InitializeComponent();
            viewModel = (RealtimeViewModel)this.DataContext;
        }

        public void AddPoints(double v1,double v2,double v3)
        {
            viewModel.AddPoints(v1, v2, v3);
        }
    }
}
