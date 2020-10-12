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

namespace Main.Integration
{
    /// <summary>
    /// IngDR.xaml 的交互逻辑
    /// </summary>
    public partial class IngDR : UserControl
    {
        private static IngDR _instance = null;
        private static readonly object syncRoot = new object();

        public static IngDR Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngDR();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        public IngDR()
        {
            InitializeComponent();
            timer = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);
            aminationNew.SendFingerBeamNumberEvent += Instance_SendFingerBeamNumberEvent;
            aminationNew.SystemChange(SystemType.DrillFloor);
            this.Loaded += IngDR_Loaded; ;
        }

        private void IngDR_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 1, 32, 1, 10, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);

            byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 30, 30 };
            GlobalData.Instance.da.SendBytes(data);
            this.aminationNew.InitRowsColoms(SystemType.DrillFloor);
        }

        /// <summary>
        /// 50ms定时器
        /// </summary>
        /// <param name="obj"></param>
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    aminationNew.LoadFingerBeamDrillPipe(SystemType.SecondFloor);
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void Instance_SendFingerBeamNumberEvent(byte number)
        {
            if (GlobalData.Instance.da["operationModel"].Value.Byte == 5 || GlobalData.Instance.da["operationModel"].Value.Byte == 3)
            {
                byte[] byteToSend = GlobalData.Instance.SendByte(new List<byte> { 5, number });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            Thread.Sleep(50);
            if (GlobalData.Instance.da["droperationModel"].Value.Byte == 5 || GlobalData.Instance.da["droperationModel"].Value.Byte == 3)
            {
                byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 5, number });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
    }
}
