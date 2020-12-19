using Main.DrillFloor;
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

namespace Main.WellRepair.DrillFloor
{
    /// <summary>
    /// WR_ParamMain.xaml 的交互逻辑
    /// </summary>
    public partial class WR_ParamMain : UserControl
    {
        private static WR_ParamMain _instance = null;
        private static readonly object syncRoot = new object();

        public static WR_ParamMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new WR_ParamMain();
                        }
                    }
                }
                return _instance;
            }
        }

        public int page = 1;
        public WR_ParamMain()
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
                    this.ParamPage.Children.Add(WR_ParamSetOne.Instance);
                }
                else if (page == 2)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Visible;
                    this.ParamPage.Children.Add(WR_ParamSetTwo.Instance);
                }
                else if (page == 3)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Hidden;
                    this.ParamPage.Children.Add(WR_ParamSetThree.Instance);
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
