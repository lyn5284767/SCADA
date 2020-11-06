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
    /// DrillSetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DrillSetWindow : Window
    {
        public DrillSetWindow()
        {
            InitializeComponent();
            this.aminationNew.InitRowsColoms(COM.Common.SystemType.SecondFloor);
            this.aminationNew.SendFingerBeamNumberEvent += AminationNew_SendFingerBeamNumberEvent;
        }

        private void AminationNew_SendFingerBeamNumberEvent(byte number)
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
