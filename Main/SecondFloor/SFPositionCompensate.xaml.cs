using COM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Main.SecondFloor
{
    /// <summary>
    /// SFPositionCompensate.xaml 的交互逻辑
    /// </summary>
    public partial class SFPositionCompensate : UserControl
    {
        private static SFPositionCompensate _instance = null;
        private static readonly object syncRoot = new object();

        public static SFPositionCompensate Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFPositionCompensate();
                        }
                    }
                }
                return _instance;
            }
        }

        List<TextBox> carCompensaList = new List<TextBox>();
        List<TextBox> rotateCompensaList = new List<TextBox>();
        //public Amination amination = new Amination();
        public AminationNew aminationNew = new AminationNew();
        System.Threading.Timer timer;
        public SFPositionCompensate()
        {
            InitializeComponent();
            Init();
            this.Loaded += SFPositionCompensate_Loaded;
        }

        private void Init()
        {
            carCompensaList.Add(CarPosiCompensation1);
            carCompensaList.Add(CarPosiCompensation2);
            carCompensaList.Add(CarPosiCompensation3);
            carCompensaList.Add(CarPosiCompensation4);
            carCompensaList.Add(CarPosiCompensation5);
            carCompensaList.Add(CarPosiCompensation6);
            carCompensaList.Add(CarPosiCompensation7);
            carCompensaList.Add(CarPosiCompensation8);
            carCompensaList.Add(CarPosiCompensation9);
            carCompensaList.Add(CarPosiCompensation10);
            carCompensaList.Add(CarPosiCompensation11);
            carCompensaList.Add(CarPosiCompensation12);
            carCompensaList.Add(CarPosiCompensation13);
            carCompensaList.Add(CarPosiCompensation14);
            carCompensaList.Add(CarPosiCompensation15);
            carCompensaList.Add(CarPosiCompensation16);
            carCompensaList.Add(CarPosiCompensation17);
            carCompensaList.Add(CarPosiCompensation18);
            carCompensaList.Add(CarPosiCompensation19);
            carCompensaList.Add(CarPosiCompensation20);
            carCompensaList.Add(CarPosiCompensation21);
            carCompensaList.Add(CarPosiCompensation22);
            carCompensaList.Add(CarPosiCompensation23);
            carCompensaList.Add(CarPosiCompensation24);
            carCompensaList.Add(CarPosiCompensation25);
            carCompensaList.Add(CarPosiCompensation26);
            carCompensaList.Add(CarPosiCompensation27);
            carCompensaList.Add(CarPosiCompensation28);
            carCompensaList.Add(CarPosiCompensation29);
            carCompensaList.Add(CarPosiCompensation30);
            carCompensaList.Add(CarPosiCompensation31);
            carCompensaList.Add(CarPosiCompensation32);

            rotateCompensaList.Add(RotatePosiCompensation1);
            rotateCompensaList.Add(RotatePosiCompensation2);
            rotateCompensaList.Add(RotatePosiCompensation3);
            rotateCompensaList.Add(RotatePosiCompensation4);
            rotateCompensaList.Add(RotatePosiCompensation5);
            rotateCompensaList.Add(RotatePosiCompensation6);
            rotateCompensaList.Add(RotatePosiCompensation7);
            rotateCompensaList.Add(RotatePosiCompensation8);
            rotateCompensaList.Add(RotatePosiCompensation9);
            rotateCompensaList.Add(RotatePosiCompensation10);
            rotateCompensaList.Add(RotatePosiCompensation11);
            rotateCompensaList.Add(RotatePosiCompensation12);
            rotateCompensaList.Add(RotatePosiCompensation13);
            rotateCompensaList.Add(RotatePosiCompensation14);
            rotateCompensaList.Add(RotatePosiCompensation15);
            rotateCompensaList.Add(RotatePosiCompensation16);
            rotateCompensaList.Add(RotatePosiCompensation17);
            rotateCompensaList.Add(RotatePosiCompensation18);
            rotateCompensaList.Add(RotatePosiCompensation19);
            rotateCompensaList.Add(RotatePosiCompensation20);
            rotateCompensaList.Add(RotatePosiCompensation21);
            rotateCompensaList.Add(RotatePosiCompensation22);
            rotateCompensaList.Add(RotatePosiCompensation23);
            rotateCompensaList.Add(RotatePosiCompensation24);
            rotateCompensaList.Add(RotatePosiCompensation25);
            rotateCompensaList.Add(RotatePosiCompensation26);
            rotateCompensaList.Add(RotatePosiCompensation27);
            rotateCompensaList.Add(RotatePosiCompensation28);
            rotateCompensaList.Add(RotatePosiCompensation29);
            rotateCompensaList.Add(RotatePosiCompensation30);
            rotateCompensaList.Add(RotatePosiCompensation31);
            rotateCompensaList.Add(RotatePosiCompensation32);
            //this.spAm.Children.Add(amination);
            this.spAm.Children.Add(aminationNew);
            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);//改成50ms 的时钟
            VariableBinding();
        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    //amination.LoadFingerBeamDrillPipe(SystemType.SecondFloor);
                    aminationNew.LoadFingerBeamDrillPipe(SystemType.SecondFloor);
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void SFPositionCompensate_Loaded(object sender, RoutedEventArgs e)
        {
            //amination.InitRowsColoms(SystemType.SecondFloor);
            aminationNew.InitRowsColoms(SystemType.SecondFloor);
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            this.opMode_PositionCompensation.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
            this.rotateAngle_PositionCompensation.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });
        }

        public void ShowCarCompensationTxts(bool bShow = true)
        {
            int index = 0;
            Match match;
            Regex regexCompensationIndex = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            this.txtCarCompensaLeft.Visibility = bShow ? Visibility.Visible : Visibility.Hidden;
            this.txtCarCompensaRight.Visibility = bShow ? Visibility.Visible : Visibility.Hidden;

            foreach (var item in carCompensaList)
            {
                if (!bShow)
                {
                    item.Visibility = Visibility.Hidden;
                }
                else
                {
                    match = regexCompensationIndex.Match(item.Name.ToString());//找出所在的行数

                    if (match.Success)
                    {
                        index = int.Parse(match.Groups[1].Value);

                        if (index == 1 || index == 17)
                        {
                            item.Visibility = Visibility.Visible;
                        }
                        //else if ((index <= (this.amination.RowsCnt + 1)) || (index > 17 && index <= (this.amination.RowsCnt + 17)))
                        else if ((index <= (this.aminationNew.RowsCnt + 1)) || (index > 17 && index <= (this.aminationNew.RowsCnt + 17)))
                        {
                            item.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            item.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
        }

        public void ShowRotatePosiCompensaTxts(bool bShow = true)
        {
            int index = 0;
            Match match;
            Regex regexCompensationIndex = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            this.txtRotatePosiCompensaRight.Visibility = bShow ? Visibility.Visible : Visibility.Hidden;
            this.txtRotatePosiCompensaLeft.Visibility = bShow ? Visibility.Visible : Visibility.Hidden;

            foreach (var item in rotateCompensaList)
            {
                if (!bShow)
                {
                    item.Visibility = Visibility.Hidden;
                }
                else
                {
                    match = regexCompensationIndex.Match(item.Name.ToString());//找出所在的行数

                    if (match.Success)
                    {
                        index = int.Parse(match.Groups[1].Value);

                        if (index == 1 || index == 17)
                        {
                            item.Visibility = Visibility.Visible;
                        }
                        //else if ((index <= (this.amination.RowsCnt + 1)) || (index > 17 && index <= (this.amination.RowsCnt + 17)))
                        else if ((index <= (this.aminationNew.RowsCnt + 1)) || (index > 17 && index <= (this.aminationNew.RowsCnt + 17)))
                        {
                            item.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            item.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
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

        /// <summary>
        /// 选择小车位置补偿
        /// </summary>
        private void btn_CarPositionCompensation(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 19, 11, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);

            this.txtCompensationType.Visibility = Visibility.Hidden;
            this.menuCarCompensationSetting.IsEnabled = true;
            this.menuRotateCompensationSetting.IsEnabled = false;

            ShowCarCompensationTxts(true);
            ShowRotatePosiCompensaTxts(false);
        }
        /// <summary>
        /// 选择回转位置补偿
        /// </summary>
        private void btn_RotatePositionCompensation(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 19, 11, 3 });
            GlobalData.Instance.da.SendBytes(byteToSend);

            this.txtCompensationType.Visibility = Visibility.Hidden;
            this.menuRotateCompensationSetting.IsEnabled = true;
            this.menuCarCompensationSetting.IsEnabled = false;

            ShowCarCompensationTxts(false);
            ShowRotatePosiCompensaTxts(true);
        }

        #region 补偿模式设置
        /// <summary>
        /// 刷新补偿值
        /// </summary>
        private void btn_CompensationRefresh(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 19, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 写补偿值
        /// </summary>
        private void btn_CompensationSetting(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 19, 2 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 清除当前位置补偿值
        /// </summary>
        private void btn_CompensationClearCur(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 19, 3 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 清除当前所有补偿值
        /// </summary>
        private void btn_CompensationClearAll(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 19, 10 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        #endregion

        private void tbPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }
    }
}
