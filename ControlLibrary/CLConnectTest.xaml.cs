using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ControlLibrary
{
    /// <summary>
    /// CLConnectTest.xaml 的交互逻辑
    /// </summary>
    public partial class CLConnectTest : UserControl
    {
        public CLConnectTest()
        {
            InitializeComponent();
            Init();
            GetIP();
        }

        public static readonly RoutedCommand SendCommand;
        public static readonly DependencyProperty IsConnectedProperty;
        public static readonly DependencyProperty PortProperty;
        IConnect iConnectType;
        private ObservableCollection<DataForm> rcvSendData = new ObservableCollection<DataForm>();
        List<RadioButton> rcvRadioGroup;
        List<RadioButton> sendRadioGroup;
        private string _localIPAddress;
        public string LocalIPAddress
        {
            get { return _localIPAddress; }
            set { _localIPAddress = value; }
        }

        static CLConnectTest()
        {
            IsConnectedProperty = DependencyProperty.Register("IsConnected", typeof(bool), typeof(CLConnectTest), new PropertyMetadata((bool)false));
            PortProperty = DependencyProperty.Register("Port", typeof(int), typeof(CLConnectTest), new PropertyMetadata((int)8083));
            SendCommand = new RoutedCommand();
        }

        public bool IsConnected
        {
            get { return (bool)GetValue(IsConnectedProperty); } 
            set { SetValue(IsConnectedProperty, value); }
        }

        public int Port
        {
            get { return (int)GetValue(PortProperty); }
            set { SetValue(PortProperty, value); }
        }

        private void Init()
        {
            listDisplayData.ItemsSource = rcvSendData;
            ((INotifyCollectionChanged)listDisplayData.Items).CollectionChanged += ListView_CollectionChanged;

            rcvRadioGroup = new List<RadioButton>() { rcvRadioASCII , rcvRadioHEX };
            sendRadioGroup = new List<RadioButton>() { sendRadioASCII , sendRadioHEX };
        }

        private void ListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // scroll the new item into view   
                listDisplayData.ScrollIntoView(e.NewItems[0]);
            }
        }

        private void Btn_clcTxtSend(object sender, RoutedEventArgs e)
        {
            this.txtSend.Text = "";
        }

        private void Btn_clcTxtRcv(object sender, RoutedEventArgs e)
        {
            rcvSendData.Clear();
        }

        private void Btn_openConnect(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
            {
                if (iConnectType != null)
                    iConnectType.CloseConnect();
                IsConnected = false;
                return;
            }

            string strProtocol = cmbProtocolType.Text;
            string strIPAddress = cmbIpAddress.Text;

            switch (strProtocol)
            {
                case "UDP":
                    iConnectType = new UDPConnect(strIPAddress, Port);
                    break;
                case "Ping":
                    iConnectType = new PingTest(strIPAddress);
                    break;
                default:
                    iConnectType = new UDPConnect(strIPAddress, Port);
                    break;
            }

            iConnectType.GetRcvBufferEvent += DisPlayDataAsync;

            if (iConnectType.OpenConnect())
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
                return;
            }
        }

        /// <summary>
        /// 是否需要改 BeginInvoke 为 相关控件IsInvoke
        /// </summary>
        /// <param name="dt"></param>
        private void DisPlayDataAsync(DataForm dt)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                string strProtocol = cmbProtocolType.Text;
                if (string.Equals(strProtocol, "Ping"))
                {
                    rcvSendData.Add(new DataForm { Buffer = dt.Buffer, Length = dt.Length, IPPort = "[" + dt.DTime + "] " + dt.IPPort, DTime = dt.DTime, IsRS = dt.IsRS });
                }
                else
                {
                    var cb = rcvRadioGroup.First(x => x.IsChecked == true);
                    rcvSendData.Add(new DataForm { Buffer = dt.Buffer, Length = dt.Length, IPPort = "[" + dt.DTime + "]# RECV " + cb.Content + " FROM " + dt.IPPort, DTime = dt.DTime, IsRS = dt.IsRS });
                }
            }));
        }

        private void PartStringToIPPort(string s,out string ip, out int port)
        {
            string[] sArray = s.Split(new char[2] { ':','：' });
            ip = sArray[0];
            int.TryParse(sArray[1],out port);
        }

        private void Send_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (txtSend.Text != "" && IsConnected)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }

        }

        private void Send_Execute(object sender, ExecutedRoutedEventArgs e)
        {

            if (cmbProtocolType.Text == "UDP")
            {
                string strTemp = cmbRemoteHost.Text;
                string strRemoteIP;
                int iRemotePort;
                PartStringToIPPort(strTemp, out strRemoteIP, out iRemotePort);

                iConnectType.RemoteIPAddress = strRemoteIP;
                iConnectType.RemotePort = iRemotePort;
            }

            var cb = sendRadioGroup.Where(x => x.IsChecked == true).First();

            iConnectType.SendData(Encoding.UTF8.GetBytes(txtSend.Text));

            rcvSendData.Add(new DataForm { Buffer =txtSend.Text, Length = txtSend.Text.Length, IPPort = "[" + DateTime.Now + "]# SEND "+ cb.Content +" TO " + cmbRemoteHost.Text, DTime =DateTime.Now, IsRS = true });
        }

        private void GetIP()
        {
            List<string> ipList = new List<string>();
            List<string> remoteIPList = new List<string>();
            string hostName = Dns.GetHostName();
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);
            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipList.Add(ip.ToString());
                    _localIPAddress = ip.ToString();
                    remoteIPList.Add(ip.ToString() + " :8084");
                }

            }

            ipList.Add("127.0.0.1");
            remoteIPList.Add("127.0.0.1 :8084");
            cmbIpAddress.ItemsSource = ipList;
            cmbIpAddress.SelectedIndex = 0;
            cmbRemoteHost.ItemsSource = remoteIPList;
        }

        private void OtherAssist_Click(object sender, RoutedEventArgs e)
        {
            string appName = System.Environment.CurrentDirectory + @"\NetAssist.exe";
            Process.Start(appName);
        }
    }

    public class CmbSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            ComboBoxItem cbi = (ComboBoxItem)value;

            return cbi.Content.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ListBoxWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

           return double.Parse(string.Format("{0}", value)) -10;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsRcvSendToAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            bool bIsRcvSend = (bool)value;

            if (bIsRcvSend)
            {
                return "Left";
            }

            return "Right";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsRcvSendToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            bool bIsRcvSend = (bool)value;

            if (bIsRcvSend)
            {
                return "#0000FF";
            }

            return "#008000";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PortRangeRule : ValidationRule
    {
        public string PortOrCnt { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int number;
            if (!int.TryParse((string)value, out number))
            {
                return new ValidationResult(false, "非数值性数字");
            }

            if (string.Equals(PortOrCnt,"Port"))
            {
                if (number < 0 || number > 65535)
                {
                    string s = string.Format("超出端口范围（0 - 65535）");
                    return new ValidationResult(false, s);
                }

                return ValidationResult.ValidResult;
            }

            return ValidationResult.ValidResult;
        }
    }
}
