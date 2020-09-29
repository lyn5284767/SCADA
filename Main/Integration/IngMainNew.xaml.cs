using COM.Common;
using ControlLibrary;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
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
        // 流程定时器
        System.Threading.Timer ProcessTimer;
        public IngMainNew()
        {
            InitializeComponent();
            Init();
            this.Loaded += IngMainNew_Loaded;
        }

        private void Init()
        {
            GlobalData.Instance.Rows = GlobalData.Instance.da["DrillNums"].Value.Byte;
            GlobalData.Instance.DrillNum = GlobalData.Instance.da["103E23B5"].Value.Byte;
            ProcessTimer = new System.Threading.Timer(new TimerCallback(ProcessTimer_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }

        private void IngMainNew_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);

            string configPath = System.Environment.CurrentDirectory + "\\KeyBoard.exe";
            System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processList)
            {
                if (process.ProcessName.Contains("KeyBoard"))
                {
                    process.Kill();
                }
            }
        }
        /// <summary>
        /// 切换当前运行设备顶上去
        /// </summary>
        /// <param name="obj"></param>
        private void ProcessTimer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                   
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 联动设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbIng_Click(object sender, RoutedEventArgs e)
        {
            //UIElementCollection childrens = this.gdMain.Children;
            //foreach (UIElement ui in childrens)
            //{
            //    if (ui is Drawer)
            //    {
            //        ((ui as Drawer).Content as Grid).Children.Clear();
            //        ((ui as Drawer).Content as Grid).Children.Add(IngSet.Instance);

            //    }
            //}
            //this.DrawerBottom.IsOpen = true;
            //this.gdSet.Children.Clear();
            //this.gdSet.Children.Add(IngSet.Instance);
            IngSetWindow ingSet = new IngSetWindow();
            ingSet.ShowDialog();
        }

        private void tbSF_Click(object sender, RoutedEventArgs e)
        {
            //this.gdSet.Children.Clear();
            //this.gdSet.Children.Add(SFSet.Instance);
            //UIElementCollection childrens = this.gdMain.Children;
            //foreach (UIElement ui in childrens)
            //{
            //    if (ui is Grid && (ui as Grid).Name == "gdSet")
            //    {
            //        (ui as Grid).Children.Clear();
            //        (ui as Grid).Children.Add(SFSet.Instance);
            //    }
            //}
            IngSFSetWindow sfSet = new IngSFSetWindow();
            sfSet.ShowDialog();
        }
    }
}
