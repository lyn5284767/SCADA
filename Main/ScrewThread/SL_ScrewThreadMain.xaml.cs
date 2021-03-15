using COM.Common;
using ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Main.ScrewThread
{
    /// <summary>
    /// SL_ScrewThreadMain.xaml 的交互逻辑
    /// </summary>
    public partial class SL_ScrewThreadMain : UserControl
    {
        private static SL_ScrewThreadMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SL_ScrewThreadMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SL_ScrewThreadMain();
                        }
                    }
                }
                return _instance;
            }
        }
        public SL_ScrewThreadMain()
        {
            InitializeComponent();
            VariableBinding();
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            this.tbCurAngle.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_Angle"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbCurPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_Move"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbWellAngle.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_WellAngle"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbWellPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_WellMove"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbMouseAngle.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_MosueAngle"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbMousePos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_MouseMove"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbPointAngle.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_OrginAngle"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbPointPos.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_OrginMove"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbDownTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_DescendTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbInTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_PerfusionTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbUpTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_RiseTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

            this.smEncoder.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["971b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smLeftBeyond.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["971b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smRightBeyond.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["971b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smWellReturn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["971b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smMouseReturn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["971b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smOrginReturn.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["971b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smRun.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["971b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smAuto.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["971b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.btnPreventBox.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["972b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
            this.btnScrew.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["972b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

            // 设置值
            Dictionary<int, string> dicNumToValue = new Dictionary<int, string>();
            dicNumToValue.Add(0, "无");
            dicNumToValue.Add(1, "缩回偏差位移");
            dicNumToValue.Add(2, "伸减速位置");
            dicNumToValue.Add(3, "缩减速位置");
            dicNumToValue.Add(4, "伸最大距离");
            dicNumToValue.Add(5, "伸减速速度");
            dicNumToValue.Add(6, "缩减速速度");
            dicNumToValue.Add(7, "角度误差设定");
            dicNumToValue.Add(8, "位移误差设定");
            dicNumToValue.Add(9, "回缩停止位置");
            dicNumToValue.Add(10, "回缩减速提前量");
            dicNumToValue.Add(11, "前伸减速提前量");
            dicNumToValue.Add(12, "自动伸缩速度");
            dicNumToValue.Add(13, "自动伸缩慢速");
            dicNumToValue.Add(14, "自动旋转速度");
            dicNumToValue.Add(15, "自动旋转慢速");
            dicNumToValue.Add(16, "下降时间");
            dicNumToValue.Add(17, "注脂时间");
            dicNumToValue.Add(18, "上升时间");
            this.cbSet.ItemsSource = dicNumToValue;
            this.cbSet.SelectedValuePath = "Key";
            this.cbSet.DisplayMemberPath = "Value";
            this.cbSet.SelectedIndex = 0;
            this.textBoxShow.SetBinding(TextBox.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Screw_SL_ParamValue"], Mode = BindingMode.OneWay });

        }
        /// <summary>
        /// 井口记忆位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWellMemory_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 1, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 鼠洞记忆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMouseMemory_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 1, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 原点记忆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrignMemory_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 1, 3, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 返回井口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackWell_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 2, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 返回鼠洞
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackMouse_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 2, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 返回原点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackOrgin_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 2, 3, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 防喷盒模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreventBox_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 4, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 丝扣油模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScrew_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 4, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 丝扣油自动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAuto_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 5, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 注油泵动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 6, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 索引值设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetConfirm_Click(object sender, RoutedEventArgs e)
        {
            Regex regexParameterConfigurationConfirm = new Regex(@"^[0-9]+$");
            string strInputText = this.tbInput.Text;
            if (strInputText.Length == 0) strInputText = "0";
            int setValue = this.cbSet.SelectedIndex;
            if ((regexParameterConfigurationConfirm.Match(strInputText)).Success)
            {
                short i16Text = Convert.ToInt16(strInputText);
                byte[] tempByte = BitConverter.GetBytes(i16Text);
                byte[] byteToSend = new byte[10] { 80, 64, 3, (byte)setValue, tempByte[0], tempByte[1], 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }
        /// <summary>
        /// 缩回偏差位移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRetractErrorMove_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 伸展最大距离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReachMaxDis_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 4, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 伸展减速位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReachSlowPos_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 缩回减速位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRetractSlowPos_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 3, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 伸展减速速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReachSlowSpeed_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 5, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 缩回减速速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRtetactSlowSpeed_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 6, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 角度误差
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAngleError_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 7, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 位移误差
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoveError_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 8, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 自动伸缩速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoStretchSpeed_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 12, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 自动伸缩慢速
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoStretchSlowSpeed_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 13, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 自动旋转速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoRotateSpeed_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 14, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 自动旋转慢速
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoRotateSlowSpeed_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 15, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 回缩减速提前量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRetractSlowLeadTime_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 10, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 前伸减速提前量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReachSlowLeadTime_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 11, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 回缩停止位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRetractStopPos_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 64, 3, 9, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 切换读取索引值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string strText = string.Empty;
            //if (this.cbSet.SelectedValue != null) strText = this.cbSet.SelectedValue.ToString();
            //if (strText.Length == 0) strText = "0";
            //short i16Text = (short)(double.Parse(strText) * 10);
            //byte[] tempByte = BitConverter.GetBytes(i16Text);
            //byte[] byteToSend = new byte[10] { 80, 64, 7, 1, tempByte[0], tempByte[1], 0, 0, 0, 0 };
            //GlobalData.Instance.da.SendBytes(byteToSend);

        }
        /// <summary>
        /// 获取键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_ParameterConfig_Focus(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                GlobalData.Instance.GetKeyBoard();
            }
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Read_Click(object sender, RoutedEventArgs e)
        {
            string strText = string.Empty;
            if (this.cbSet.SelectedValue != null) strText = this.cbSet.SelectedValue.ToString();
            if (strText.Length == 0) strText = "0";
            short i16Text = (short)(double.Parse(strText) * 1);
            byte[] tempByte = BitConverter.GetBytes(i16Text);
            byte[] byteToSend = new byte[10] { 80, 64, tempByte[0], tempByte[1], 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
