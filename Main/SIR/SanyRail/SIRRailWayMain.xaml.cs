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

namespace Main.SIR.SanyRail
{
    /// <summary>
    /// SIRRailWayMain.xaml 的交互逻辑
    /// </summary>
    public partial class SIRRailWayMain : UserControl
    {
        private static SIRRailWayMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRRailWayMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRRailWayMain();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRRailWayMain()
        {
            InitializeComponent();
        }

        private void btn_oprModel(object sender, EventArgs e)
        {

        }

        private void btn_workModel(object sender, EventArgs e)
        {

        }

        private void btn_PipeTypeModel(object sender, EventArgs e)
        {

        }

        private void btn_locationModel(object sender, EventArgs e)
        {

        }

        private void btn_SelectDrillPipe(object sender, RoutedEventArgs e)
        {

        }
    }
}
