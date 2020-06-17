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
    /// SFRecordMain.xaml 的交互逻辑
    /// </summary>
    public partial class SFRecordMain : UserControl
    {

        private static SFRecordMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SFRecordMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFRecordMain();
                        }
                    }
                }
                return _instance;
            }
        }
        public SFRecordMain()
        {
            InitializeComponent();
            PageChange();
            this.Loaded += SFRecordMain_Loaded;
        }

        private void SFRecordMain_Loaded(object sender, RoutedEventArgs e)
        {
            //PageChange();
        }
        int page = 1;
        /// <summary>
        /// 页面切换
        /// </summary>
        private void PageChange()
        {
            try
            {
                this.RecordPage.Children.Clear();
                if (page == 1)
                {
                    this.left.Visibility = Visibility.Hidden;
                    this.right.Visibility = Visibility.Visible;
                    this.RecordPage.Children.Add(SFRecordOne.Instance);
                }
                else if (page == 2)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Visible;
                    this.RecordPage.Children.Add(SFRecordTwo.Instance);
                }
                else if (page == 3)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Hidden;
                    this.RecordPage.Children.Add(SFRecordThree.Instance);
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
