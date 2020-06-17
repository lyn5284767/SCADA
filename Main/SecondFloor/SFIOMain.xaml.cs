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

namespace Main.SecondFloor
{
    /// <summary>
    /// SFIOOne.xaml 的交互逻辑
    /// </summary>
    public partial class SFIOMain : UserControl
    {
        private static SFIOMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SFIOMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFIOMain();
                        }
                    }
                }
                return _instance;
            }
        }

        public int page = 1;

        public SFIOMain()
        {
            InitializeComponent();
            PageChange();
            this.Loaded += SFIOMain_Loaded;
        }

        private void SFIOMain_Loaded(object sender, RoutedEventArgs e)
        {
            //PageChange();
        }
        /// <summary>
        /// IO页面切换
        /// </summary>
        private void PageChange()
        {
            try
            {
                this.IOPage.Children.Clear();
                if (page == 1)
                {
                    this.left.Visibility = Visibility.Hidden;
                    this.right.Visibility = Visibility.Visible;
                    this.IOPage.Children.Add(SFIOOne.Instance);
                }
                else if (page == 2)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Visible;
                    this.IOPage.Children.Add(SFIOTwo.Instance);
                }
                else if (page == 3)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Visible;
                    this.IOPage.Children.Add(SFIOThree.Instance);
                }
                else if (page == 4)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Hidden;
                    this.IOPage.Children.Add(SFIOFour.Instance);
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 上一页
        /// </summary>
        private void Left_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.page -= 1;
            PageChange();
        }
        /// <summary>
        /// 下一页
        /// </summary>
        private void Right_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.page += 1;
            PageChange();
        }
    }
}
