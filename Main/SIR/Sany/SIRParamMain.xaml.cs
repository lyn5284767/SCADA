using COM.Common;
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

namespace Main.SIR.Sany
{
    /// <summary>
    /// SIRParamMain.xaml 的交互逻辑
    /// </summary>
    public partial class SIRParamMain : UserControl
    {
        private static SIRParamMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRParamMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRParamMain();
                        }
                    }
                }
                return _instance;
            }
        }

        public int page = 1;

        public SIRParamMain()
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
                    this.ParamPage.Children.Add(SIRParamOne.Instance);
                }
                else if (page == 2)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Hidden;
                    this.ParamPage.Children.Add(SIRParamTwo.Instance);
                }
                // 只有调试员可以往后面翻页
                if (GlobalData.Instance.systemRole != SystemRole.DebugMan)
                {
                    this.right.Visibility = Visibility.Hidden;
                    return;
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
