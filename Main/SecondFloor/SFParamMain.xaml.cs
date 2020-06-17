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
    /// SFParamMain.xaml 的交互逻辑
    /// </summary>
    public partial class SFParamMain : UserControl
    {
        private static SFParamMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SFParamMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFParamMain();
                        }
                    }
                }
                return _instance;
            }
        }

        public int page = 1;

        public SFParamMain()
        {
            InitializeComponent();
            PageChange();
        }

        /// <summary>
        /// IO页面切换
        /// </summary>
        private void PageChange()
        {
            try
            {
                this.ParamPage.Children.Clear();
                if (page == 1)
                {
                    this.left.Visibility = Visibility.Hidden;
                    this.right.Visibility = Visibility.Visible;
                    this.ParamPage.Children.Add(SFParamOne.Instance);
                }
                else if (page == 2)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Visible;
                    this.ParamPage.Children.Add(SFParamTwo.Instance);
                }
                else if (page == 3)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Hidden;
                    this.ParamPage.Children.Add(SFParamThree.Instance);
                }
                //else if (page == 4)
                //{
                //    this.left.Visibility = Visibility.Visible;
                //    this.right.Visibility = Visibility.Hidden;
                //    this.ParamPage.Children.Add(DRParamFour.Instance);
                //}
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
