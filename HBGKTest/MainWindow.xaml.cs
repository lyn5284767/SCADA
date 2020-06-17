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
using System.Windows.Forms;
using System.Windows.Forms.Integration;




namespace HBGKTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            //IntPtr intPtr =  this.videoPanel.Handle;
            //int i = 0; 


            //this.Loaded += MainWindow_Loaded;

            //System.Windows.Forms.Integration.WindowsFormsHost windowsFormHost = new System.Windows.Forms.Integration.WindowsFormsHost();
            //Form1 form1 = new Form1();
            //form1.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //form1.TopLevel = false;
            //form1.ControlBox = false;
            //form1.Dock = System.Windows.Forms.DockStyle.Fill;
            //windowsFormHost.Child = form1;
            //System.Windows.Controls.Panel.SetZIndex(windowsFormHost, -1);


            //System.Windows.Forms.Integration.WindowsFormsHost windowsFormHost = new System.Windows.Forms.Integration.WindowsFormsHost();
            //Form1 form1 = new Form1();
            //form1.TopLevel = false;
            //spCamera.Width = form1.Width;
            //spCamera.Height = form1.Height;

            //windowsFormHost.Width = form1.Width;
            //windowsFormHost.Height = form1.Height;

            //form1.TopLevel = false;
            //form1.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //form1.ControlBox = false;
            //form1.Dock = System.Windows.Forms.DockStyle.Fill;


            //windowsFormHost.Child = form1;

            //spCamera.Children.Add(windowsFormHost);


            //this.Loaded += Window_Loaded;





        }

        UIControl_HBGK1 uiControl_HBGK1;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn_click = new System.Windows.Controls.Button();
            btn_click.Name = "btn_click";
            btn_click.Width = 100;
            btn_click.Height = 100;
            btn_click.Content = "按钮";
            MyGrid.Children.Add(btn_click);
            btn_click.SetValue(Grid.RowProperty, 0);
            btn_click.SetValue(Grid.ColumnProperty, 0);

            //MyGrid.Children.Add(uiControl_HBGK1);
            //uiControl_HBGK1.SetValue(Grid.RowProperty, 0);
            //uiControl_HBGK1.SetValue(Grid.ColumnProperty, 0);


        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void init1()
        {
            ChannelInfo ch = new ChannelInfo();
            ch.ChlID = "1";
            ch.nDeviceType = 1;//2表示NVR，1表示IPC
            ch.RemoteChannle = "1";
            ch.RemoteIP = "172.16.16.121";
            ch.RemotePort = 554;
            ch.RemoteUser = "admin";
            ch.RemotePwd = "123456";
            ch.nPlayPort = 1;
            ch.PtzPort = 8091;
            uiControl_HBGK1 = new UIControl_HBGK1(ch);

        }

        private void Button_1_click(object sender, RoutedEventArgs e)
        {
            init1();

            //uiControlHBGK = uiControl_HBGK1;
            uiControl_HBGK1.Height = 480;
            uiControl_HBGK1.Width = 480;
            
            MyGrid.Children.Add(uiControl_HBGK1);
            uiControl_HBGK1.SetValue(Grid.RowProperty, 0);
            uiControl_HBGK1.SetValue(Grid.ColumnProperty, 0);






        }

        private void Button_2_click(object sender, RoutedEventArgs e)
        {
            MyGrid.Children.Clear();
        }

        private void Button_3_click(object sender, RoutedEventArgs e)
        {
            string filePath = @"D:\Work\video";
            string re = uiControl_HBGK1.TakePictures(filePath);
            System.Windows.MessageBox.Show(re);
        }

        private void Button_4_click(object sender, RoutedEventArgs e)
        {
            string filePath = @"D:\Work\video";
            string re = uiControl_HBGK1.StartKinescope(filePath);
            System.Windows.MessageBox.Show(re);

        }

        private void Button_5_click(object sender, RoutedEventArgs e)
        {
            string re = uiControl_HBGK1.StopKinescope();
            System.Windows.MessageBox.Show(re);

        }
    }
}
