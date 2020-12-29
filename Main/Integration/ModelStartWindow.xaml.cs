using COM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

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
            tmpModel = model;
            this.tbCurTip.Text = "准备启动";
            stepTimer = new System.Threading.Timer(new TimerCallback(StepTimer_Elapsed), this, 500, 500);//改成50ms 的时钟
        }
        int i = 0;
        private void StepTimer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (this.pbper.Value == 0)
                    {
                        this.tbCurTip.Text = "开始启动";
                        this.pbper.Value = 10;
                    }
                    if (this.pbper.Value == 10)
                    {
                        this.tbCurTip.Text = "检查液压站模式";
                        this.pbper.Value = 15;
                    }
                    if (this.pbper.Value == 15) //检查液压站
                    {
                        if (GlobalData.Instance.da["771b5"].Value.Boolean && !GlobalData.Instance.da["771b6"].Value.Boolean)// 本地模式
                        {
                            this.tbCurTip.Text = "液压站处于本地控制模式，请切换到司钻再启动";
                        }
                        else if (!GlobalData.Instance.da["771b5"].Value.Boolean && GlobalData.Instance.da["771b6"].Value.Boolean) // 司钻模式
                        {
                            this.tbCurTip.Text = "泵准备启动";
                            this.pbper.Value = 20;
                        }
                        else // 分阀箱模式
                        {
                            byte[] byteToSend = new byte[10] { 0, 19, 1, 9, 0, 0, 0, 0, 0, 0 };// 疑问，所有切换都是这个协议？
                            GlobalData.Instance.da.SendBytes(byteToSend);
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
