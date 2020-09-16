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

namespace Main.Integration
{
    /// <summary>
    /// IngMainNew.xaml 的交互逻辑
    /// </summary>
    public partial class IngMainNew : UserControl
    {
        private static IngMainNew _instance = null;
        private static readonly object syncRoot = new object();

        public static IngMainNew Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngMainNew();
                        }
                    }
                }
                return _instance;
            }
        }
        public IngMainNew()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 联动设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbIng_Click(object sender, RoutedEventArgs e)
        {
            this.gdSet.Children.Clear();
            this.gdSet.Children.Add(IngSet.Instance);
        }

        private void tbSF_Click(object sender, RoutedEventArgs e)
        {
            this.gdSet.Children.Clear();
            this.gdSet.Children.Add(SFSet.Instance);
        }
    }
}
