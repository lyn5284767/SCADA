using COM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main.Integration
{
    /// <summary>
    /// ModelStartWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModelStartWindow : Window
    {
        GlobalModel tmpModel { get; set; }
        public ModelStartWindow()
        {
            InitializeComponent();
        }
        System.Threading.Timer stepTimer;
        public ModelStartWindow(GlobalModel model)
            : this()
        {
            tmpModel = model;
            this.Loaded += ModelStartWindow_Loaded;
            stepTimer = new System.Threading.Timer(new TimerCallback(StepTimer_Elapsed), this, 2000, 500);//改成50ms 的时钟
        }

        private void ModelStartWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartModel(tmpModel);
        }
        /// <summary>
        /// 启动模式
        /// </summary>
        /// <param name="model"></param>
        public void StartModel(GlobalModel model)
        {
            
        }

        private void StepTimer_Elapsed(object obj)
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
    }
}
