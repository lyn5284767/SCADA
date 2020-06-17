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
using System.Timers;

namespace ControlLibrary
{
    /// <summary>
    /// Clock.xaml 的交互逻辑
    /// </summary>
    public partial class Clock : UserControl
    {
        Timer MarqueeTimer = new Timer();

        public Clock()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            MarqueeTimer.Interval = 1000;//文字移动的速度
            MarqueeTimer.Enabled = true;      //开启定时触发事件
            MarqueeTimer.Elapsed += new ElapsedEventHandler(MarqueeTimer_Elapsed);//绑定定时事件
        }

        void MarqueeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.time7Segment.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }));
        }
    }
}
