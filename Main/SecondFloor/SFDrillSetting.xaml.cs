using COM.Common;
using DatabaseLib;
using Main.Integration;
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
    /// DrillSetting.xaml 的交互逻辑
    /// </summary>
    public partial class SFDrillSetting : UserControl
    {
        private static SFDrillSetting _instance = null;
        private static readonly object syncRoot = new object();

        public static SFDrillSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFDrillSetting();
                        }
                    }
                }
                return _instance;
            }
        }

        SystemType systemType = SystemType.SecondFloor;
        private Regex regexFingerBeam = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        //public Amination amination = new Amination();
        public AminationNew aminationNew = new AminationNew();
        System.Threading.Timer timer;
        public SFDrillSetting()
        {
            InitializeComponent();
            //this.gdMain.Children.Add(amination);
            this.gdMain.Children.Add(aminationNew);
            aminationNew.SetDrillNumEvent += AminationNew_SetDrillNumEvent;
            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);//改成50ms 的时钟

            this.carPosistion.SetBinding(TextBox.TextProperty,new Binding("ShortTag") { Source = GlobalData.Instance.da["carRealPosition"], Mode = BindingMode.OneWay ,Converter = new CarPosCoverter()});//小车实际位置
            this.armPosistion.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["armRealPosition"], Mode = BindingMode.OneWay, Converter = new ArmPosCoverter() });//手臂实际位置
            //this.drcarPosistion.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drCarPos"], Mode = BindingMode.OneWay, Converter = new CarPosCoverter() });//小车实际位置
            this.drcarPosistion.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drCarPos"], Mode = BindingMode.OneWay});//小车实际位置
            //this.drarmPosistion.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drArmPos"], Mode = BindingMode.OneWay, Converter = new ArmPosCoverter() });//手臂实际位置
            this.drarmPosistion.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drArmPos"], Mode = BindingMode.OneWay});//手臂实际位置
            this.Loaded += SFDrillSetting_Loaded;

        }

        private void AminationNew_SetDrillNumEvent(byte number)
        {
            if (number < 16)
            {
                this.NowOrent.Text = "左";
                this.NowSelect.Text = number.ToString();
            }
            else if (number == 16)
            {
                this.NowOrent.Text = "左";
                this.NowSelect.Text = "钻铤";
            }
            else if (number > 16 && number < 32)
            {
                this.NowOrent.Text = "右";
                this.NowSelect.Text = (number - 16).ToString();
            }
            else if (number == 32)
            {
                this.NowOrent.Text = "右";
                this.NowSelect.Text = "钻铤"; 
            }
            else this.NowSelect.Text = "未选择";
        }

        private void SFDrillSetting_Loaded(object sender, RoutedEventArgs e)
        {
            if (systemType == SystemType.DrillFloor)
            {
                byte[] data = new byte[10] { 80, 33, 0, 0, 0, 0, 0, 0, 0, 33 };
                GlobalData.Instance.da.SendBytes(data);
            }
        }

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    //amination.LoadFingerBeamDrillPipe(systemType);
                    aminationNew.LoadFingerBeamDrillPipe(systemType);
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 弹出键盘
        /// </summary>
        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }

        /// <summary>
        /// 重新读取配置文件
        /// </summary>
        private void btn_SecondFloorReLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                //amination.InitRowsColoms(systemType);
                //SFMain.Instance.amination.InitRowsColoms(systemType);
                aminationNew.InitRowsColoms(systemType);
                SFMain.Instance.aminationNew.InitRowsColoms(systemType);
                //SFPositionCompensate.Instance.amination.InitRowsColoms(systemType);
                SFPositionCompensate.Instance.aminationNew.InitRowsColoms(systemType);
                //IngMain.Instance.amination.InitRowsColoms(systemType);
                IngMain.Instance.aminationNew.InitRowsColoms(systemType);
                this.NumFixTips.Text = "加载成功";
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private string configPath = System.Environment.CurrentDirectory + @"\Config.ini";
        const int STRINGMAX = 255;

        /// <summary>
        /// 标定小车最大位移
        /// </summary>
        private void btn_SecondMaxPosistion(object sender, RoutedEventArgs e)
        {
            try
            {
                if (systemType == SystemType.SecondFloor)
                {
                    int tmpNum = int.Parse(this.carPosistion.Text);
                    if (tmpNum < 500)
                    {
                        GlobalData.Instance.CarMinPosistion = int.Parse(this.carPosistion.Text);
                        WinAPI.WritePrivateProfileString("SECONDFLOOR", "CARMINPOSISTION", GlobalData.Instance.CarMinPosistion.ToString(), configPath);
                        
                        this.NumFixTips.Text = "小车标定成功";
                    }
                    else
                    {
                        GlobalData.Instance.CarMaxPosistion = int.Parse(this.carPosistion.Text);
                        WinAPI.WritePrivateProfileString("SECONDFLOOR", "CARMAXPOSISTION", GlobalData.Instance.CarMaxPosistion.ToString(), configPath);
                        this.NumFixTips.Text = "小车标定成功";
                    }
                }
                else if(systemType == SystemType.DrillFloor)
                {
                    int tmpNum = int.Parse(this.drcarPosistion.Text);
                    if (tmpNum < 300)
                    {
                        GlobalData.Instance.DRCarMinPosistion = int.Parse(this.drcarPosistion.Text);
                        WinAPI.WritePrivateProfileString("DRILLFLOOR", "CARMINPOSISTION", GlobalData.Instance.DRCarMinPosistion.ToString(), configPath);
                        this.NumFixTips.Text = "小车标定成功";
                    }
                    else
                    {
                        GlobalData.Instance.DRCarMaxPosistion = int.Parse(this.drcarPosistion.Text);
                        WinAPI.WritePrivateProfileString("DRILLFLOOR", "CARMAXPOSISTION", GlobalData.Instance.DRCarMaxPosistion.ToString(), configPath);
                        this.NumFixTips.Text = "小车标定成功";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 标定手臂最大位移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SecondArmMaxPosistion(object sender, RoutedEventArgs e)
        {
            try
            {
                if (systemType == SystemType.SecondFloor)
                {
                    GlobalData.Instance.ArmMaxPosistion = int.Parse(this.armPosistion.Text);
                    WinAPI.WritePrivateProfileString("SECONDFLOOR", "ARMMAXPOSISTION", GlobalData.Instance.ArmMaxPosistion.ToString(), configPath);
                    this.NumFixTips.Text = "手臂标定成功";
                }
                else if (systemType == SystemType.DrillFloor)
                {
                    GlobalData.Instance.DRArmMaxPosistion = int.Parse(this.drarmPosistion.Text);
                    WinAPI.WritePrivateProfileString("DRILLFLOOR", "ARMMAXPOSISTION", GlobalData.Instance.DRArmMaxPosistion.ToString(), configPath);
                    this.NumFixTips.Text = "手臂标定成功";
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
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
        /// 清空钻杠数目
        /// </summary>
        private void btn_DrillPipeCountCorrect_CancelAllPipe(object sender, RoutedEventArgs e)
        {
            if (this.cbSysTypeSelect.SelectedIndex == 0)
            {
                byte[] byteToSend = new byte[10] { 80, 1, 7, 10, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
                Thread.Sleep(50);
                byteToSend = new byte[10] { 80, 33, 7, 10, 0, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                if (systemType == SystemType.SecondFloor)
                {
                    byte[] byteToSend = SendByte(new List<byte> { 7, 10 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
                else if (systemType == SystemType.DrillFloor)
                {
                    byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 7, 10 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
        }

        /// <summary>
        /// 刷新钻杆数量
        /// </summary>
        private void btn_DrillPipeCountCorrect_FlashPipeCount(object sender, RoutedEventArgs e)
        {
            if (this.cbSysTypeSelect.SelectedIndex == 0)
            {
                RefreshPipeCount();
                Thread.Sleep(50);
                byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 7, 1 });
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                if (systemType == SystemType.SecondFloor)
                {
                    RefreshPipeCount();
                }
                else if (systemType == SystemType.DrillFloor)
                {
                    byte[] byteToSend = GlobalData.Instance.SendToDR(new List<byte> { 7, 1 });
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
        }

        /// <summary>
        /// 回主界面-刷新钻杆数量
        /// </summary>
        private void RefreshPipeCount()
        {
            byte[] byteToSend = SendByte(new List<byte> { 7, 1 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 取消钻杠
        /// </summary>
        private void btn_CancelDrillPipe(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = SendByte(new List<byte> { 7, 20 });
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        private Regex regexDrillPipeCountCorrect = new Regex(@"^[0-9]+$");
        private void btn_DrillPipeCountCorrect_SetPipeCount(object sender, RoutedEventArgs e)
        {
            if (this.NowSelect.Text == "钻铤") this.NowSelect.Text = "0";
            byte byteText = 0;
            string strText = this.NowSelect.Text;
            if ((regexDrillPipeCountCorrect.Match(strText)).Success)
            {
                try
                {
                    byteText = Convert.ToByte(strText);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("参数数值范围异常：" + ex.Message);
                    //tbDrillPipeCountCorrectFingerDirectNumber.Text = "0";
                    return;
                }

            }
            else
            {
                MessageBox.Show("参数为非数字");
                //tbDrillPipeCountCorrectFingerDirectNumber.Text = "0";
                return;
            }

            byte byteCountText = 0;
            strText = tbDrillPipeCountCorrectPipeCount.Text;
            
            if ((regexDrillPipeCountCorrect.Match(strText)).Success)
            {
                try
                {
                    byteCountText = Convert.ToByte(strText);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("参数数值范围异常:" + ex.Message);
                    tbDrillPipeCountCorrectPipeCount.Text = "0";
                    return;
                }

            }
            else
            {
                MessageBox.Show("参数为非数字");
                tbDrillPipeCountCorrectPipeCount.Text = "0";
                return;
            }


            if (this.NowOrent.Text == "右")
            {
                if (byteText == 0)
                {
                    byteText = 32;
                }
                else if (byteText >= 1 && byteText <= 10)
                {
                    byteText = (byte)(byteText + 16);
                }
                else
                {
                    MessageBox.Show("参数数值范围异常");
                    //tbDrillPipeCountCorrectFingerDirectNumber.Text = "0";
                    return;
                }
            }
            else if (this.NowOrent.Text == "左")
            {
                if (byteText == 0)
                {
                    byteText = 16;
                }
                else if (byteText >= 1 && byteText <= 10)
                {
                    //byteText = byteText;
                }
                else
                {
                    MessageBox.Show("参数数值范围异常");
                    //tbDrillPipeCountCorrectFingerDirectNumber.Text = "0";
                    return;
                }
            }
            else
            {
                return;
            }

            if (this.cbSysTypeSelect.SelectedIndex == 0)
            {
                byte[] byteToSend = new byte[10] { 80, 1, 7, 2, byteText, byteCountText, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
                Thread.Sleep(50);
                byteToSend = new byte[10] { 80, 33, 7, 2, byteText, byteCountText, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                byte[] byteToSend = new byte[10];
                if (systemType == SystemType.SecondFloor)
                {
                    byteToSend[0] = bHeadFirst;
                    byteToSend[1] = bHeadTwo;
                }
                else if (systemType == SystemType.DrillFloor)
                {
                    byteToSend[0] = 0x50;
                    byteToSend[1] = 0x21;
                }
                byteToSend[2] = 7;
                byteToSend[3] = 2;
                byteToSend[4] = byteText;
                byteToSend[5] = byteCountText;

                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (sender as ComboBox);
            if (comboBox.SelectedIndex == 1)
            {
                systemType = SystemType.SecondFloor;
                //amination.SystemChange(systemType);
                aminationNew.SystemChange(systemType);
                this.carPosistion.Visibility = Visibility.Visible;
                this.armPosistion.Visibility = Visibility.Visible;
                this.drarmPosistion.Visibility = Visibility.Collapsed;
                this.drcarPosistion.Visibility = Visibility.Collapsed;
            }
            else if (comboBox.SelectedIndex == 2)
            {
                systemType = SystemType.DrillFloor;
                //amination.SystemChange(systemType);
                aminationNew.SystemChange(systemType);
                this.carPosistion.Visibility = Visibility.Collapsed;
                this.armPosistion.Visibility = Visibility.Collapsed;
                this.drarmPosistion.Visibility = Visibility.Visible;
                this.drcarPosistion.Visibility = Visibility.Visible;
            }
            //amination.InitRowsColoms(systemType);
            //SFMain.Instance.amination.InitRowsColoms(systemType);
            aminationNew.InitRowsColoms(systemType);
            SFMain.Instance.aminationNew.InitRowsColoms(systemType);
            //SFPositionCompensate.Instance.amination.InitRowsColoms(systemType);
            SFPositionCompensate.Instance.aminationNew.InitRowsColoms(systemType);
            //IngMain.Instance.amination.InitRowsColoms(systemType);
            IngMain.Instance.aminationNew.InitRowsColoms(systemType);
        }

        public void SysTypeSelect(int index)
        {
            this.cbSysTypeSelect.SelectedIndex = index;
        }

        private void tbPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }

        private void btn_PluseDrill(object sender, RoutedEventArgs e)
        {
            tbDrillPipeCountCorrectPipeCount.Text = (int.Parse(tbDrillPipeCountCorrectPipeCount.Text) + 1).ToString();
        }

        private void btn_ReduceDrill(object sender, RoutedEventArgs e)
        {
            tbDrillPipeCountCorrectPipeCount.Text = (int.Parse(tbDrillPipeCountCorrectPipeCount.Text) - 1).ToString();

        }
    }
}
