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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.Cat
{
    /// <summary>
    /// SL_CatStatus.xaml 的交互逻辑
    /// </summary>
    public partial class SL_CatStatus : UserControl
    {
        private static SL_CatStatus _instance = null;
        private static readonly object syncRoot = new object();

        public static SL_CatStatus Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SL_CatStatus();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        public SL_CatStatus()
        {
            InitializeComponent();
            timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟

        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    BindField();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// 绑定变量
        /// </summary>
        private void BindField()
        {
            if (GlobalData.Instance.da["Cat_SL_Tag"] == null) return;
            if (GlobalData.Instance.da["Cat_SL_Tag"].Value.Byte == 1)
            {
                this.tbInseideUpDownCarValue.Text = (GlobalData.Instance.da["Cat_SL_CasingWinchLastValue"].Value.Byte / 10.0).ToString();
                this.tbInseidePushCarValue.Text = (GlobalData.Instance.da["Cat_SL_CasingCartLastValue"].Value.Byte / 10.0).ToString();
                this.tbOutseideUpDownCarValue.Text = (GlobalData.Instance.da["Cat_SL_PipeWinchLastValue"].Value.Byte / 10.0).ToString();
                this.tbOutseidePushCarValue.Text = (GlobalData.Instance.da["Cat_SL_PipeCartLastValue"].Value.Byte / 10.0).ToString();
                this.tbUpDownCarUpMaxValue.Text = (GlobalData.Instance.da["Cat_SL_WinchUpMax"].Value.Byte / 10.0).ToString();
                this.tbCarTurnFlowValue.Text = (GlobalData.Instance.da["Cat_SL_WinchSlowFlow"].Value.Int16 / 10.0).ToString();
            }
            else if (GlobalData.Instance.da["Cat_SL_Tag"].Value.Byte == 2)
            {
                this.tbUpDownCarBackValue.Text = (GlobalData.Instance.da["Cat_SL_WinchBackLimit"].Value.Byte / 10.0).ToString();
                this.tbPushCarFrontValue.Text = (GlobalData.Instance.da["Cat_SL_CartFrontLimit"].Value.Byte / 10.0).ToString();
                this.tbDrillCarBackValue.Text = (GlobalData.Instance.da["Cat_SL_DrillCarBackLimit"].Value.Byte / 10.0).ToString();
                this.tbPipeCarBackValue.Text = (GlobalData.Instance.da["Cat_SL_PipeCarBackLimit"].Value.Byte / 10.0).ToString();
            }
        }
    }
}
