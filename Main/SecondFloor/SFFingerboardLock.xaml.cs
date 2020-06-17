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

namespace Main.SecondFloor
{
    /// <summary>
    /// DrillCollarLock.xaml 的交互逻辑
    /// </summary>
    public partial class SFFingerboardLock : UserControl
    {
        private static SFFingerboardLock _instance = null;
        private static readonly object syncRoot = new object();

        public static SFFingerboardLock Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFFingerboardLock();
                        }
                    }
                }
                return _instance;
            }
        }

        int page = 1;
        public SFFingerboardLock()
        {
            InitializeComponent();
            PageChange();
            this.Loaded += SFDrillRodLock_Loaded;
        }

        private void SFDrillRodLock_Loaded(object sender, RoutedEventArgs e)
        {
            //PageChange();
        }

        /// <summary>
        /// 页面切换
        /// </summary>
        private void PageChange()
        {
            try
            {
                this.FingerboardLockPage.Children.Clear();
                // 左指梁锁
                if (page == 1)
                {
                    this.left.Visibility = Visibility.Hidden;
                    this.right.Visibility = Visibility.Visible;
                    this.FingerboardLockPage.Children.Add(SFLeftFingerboardLock.Instance);
                }// 右指梁锁
                else if (page == 2)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Hidden;
                    this.FingerboardLockPage.Children.Add(SFRightFingerboardLock.Instance);
                }
                byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 22, 0 });
                GlobalData.Instance.da.SendBytes(byteToSend);
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
            page -= 1;
            PageChange();
        }

        private void Right_MouseDown(object sender, MouseButtonEventArgs e)
        {
            page += 1;
            PageChange();
        }
    }
}
