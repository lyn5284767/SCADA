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
    public partial class SFDrillCollarLock : UserControl
    {
        private static SFDrillCollarLock _instance = null;
        private static readonly object syncRoot = new object();

        public static SFDrillCollarLock Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFDrillCollarLock();
                        }
                    }
                }
                return _instance;
            }
        }

        int page = 1;
        public SFDrillCollarLock()
        {
            InitializeComponent();
            PageChange();
            this.Loaded += SFDrillCollarLock_Loaded;
        }

        private void SFDrillCollarLock_Loaded(object sender, RoutedEventArgs e)
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
                this.DrillCollarPage.Children.Clear();
                // 钻铤锁采样值
                if (page == 1)
                {
                    this.left.Visibility = Visibility.Hidden;
                    this.right.Visibility = Visibility.Visible;
                    this.DrillCollarPage.Children.Add(SFDrillCollarSample.Instance);
                    byte[] byteToSend = SendByte(new List<byte> { 22, 1 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
                else if (page == 2)
                {
                    this.left.Visibility = Visibility.Visible;
                    this.right.Visibility = Visibility.Hidden;
                    this.DrillCollarPage.Children.Add(SFDrillCollarLockStatus.Instance);
                    byte[] byteToSend = SendByte(new List<byte> { 22, 0 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
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
            page -= 1;
            PageChange();
        }

        private void Right_MouseDown(object sender, MouseButtonEventArgs e)
        {
            page += 1;
            PageChange();
        }
        const byte bHeadFirst = 0x50;
        const byte bHeadTwo = 0x01;
        /// <summary>
        /// 组协议
        /// </summary>
        /// <param name="list">协议字符列表</param>
        /// <returns></returns>
        private byte[] SendByte(List<byte> list)
        {
            byte[] byteToSend = new byte[10];
            byteToSend[0] = bHeadFirst; // 默认0位80
            byteToSend[1] = bHeadTwo;   // 默认1位1
            // 2位之后传进来
            for (int i = 0; i < list.Count; i++)
            {
                byteToSend[i + 2] = list[i];
            }
            return byteToSend;
        }
    }
}
