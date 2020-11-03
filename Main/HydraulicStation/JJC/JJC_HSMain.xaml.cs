using COM.Common;
using ControlLibrary;
using HBGKTest;
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

namespace Main.HydraulicStation.JJC
{
    /// <summary>
    /// JJC_HSMain.xaml 的交互逻辑
    /// </summary>
    public partial class JJC_HSMain : UserControl
    {
        private static JJC_HSMain _instance = null;
        private static readonly object syncRoot = new object();

        public static JJC_HSMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new JJC_HSMain();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        public JJC_HSMain()
        {
            InitializeComponent();
            VariableBinding();
            InitAlarmKey();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }

        private void VariableBinding()
        {
            try
            {
                #region 处理与mouse与click事件冲突
                btnLeftCatHeadReach.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnLeftCatReach_Click), true);
                btnLeftCatHeadReach.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(BtnLeftCatHeadReach_MouseUp), true);
                btnLeftCatHeadRetract.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnLeftCatRetract_Click), true);
                btnLeftCatHeadRetract.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(BtnLeftCatHeadRetract_MouseUp), true);
                btnRightCatHeadReach.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnRightCatReach_Click), true);
                btnRightCatHeadReach.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(BtnRightCatHeadReach_MouseUp), true);
                btnRightCatHeadRetract.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnRightCatRetract_Click), true);
                btnRightCatHeadRetract.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(BtnRightCatHeadRetract_MouseUp), true);
                btnRotateCatHeadReach.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnRotateCatReach_Click), true);
                btnRotateCatHeadReach.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(BtnRotateCatHeadReach_MouseUp), true);
                btnRotateCatHeadRetract.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnRotateCatRetract_Click), true);
                btnRotateCatHeadRetract.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(BtnRotateCatHeadRetract_MouseUp), true);
                #endregion
                this.tbSoftErrorCode.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_SoftStartErrorCode"], Mode = BindingMode.OneWay });
                this.tbSystemStatus.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b0"], Mode = BindingMode.OneWay, Converter = new HS_JJC_SystemStatusConverter() });
                MultiBinding hs_JJC_SoftMultiBind = new MultiBinding();
                hs_JJC_SoftMultiBind.Converter = new HS_JJC_SoftConverter();
                hs_JJC_SoftMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["733b1"], Mode = BindingMode.OneWay });
                hs_JJC_SoftMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["733b2"], Mode = BindingMode.OneWay });
                hs_JJC_SoftMultiBind.NotifyOnSourceUpdated = true;
                this.tbSoftStart.SetBinding(TextBlock.TextProperty, hs_JJC_SoftMultiBind);
   
                this.ControlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["735b0"], Mode = BindingMode.OneWay });
                this.MainPumpOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b3"], Mode = BindingMode.OneWay });
                this.MainPumpTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b4"], Mode = BindingMode.OneWay });
                this.CyclePump.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b5"], Mode = BindingMode.OneWay });
                this.ColdFan.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b6"], Mode = BindingMode.OneWay });
                this.Hot.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b7"], Mode = BindingMode.OneWay });

                MultiBinding hs_JJC_WorkModelMultiBind = new MultiBinding();
                hs_JJC_WorkModelMultiBind.Converter = new HS_JJC_WorkModelConverter();
                hs_JJC_WorkModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["734b0"], Mode = BindingMode.OneWay });
                hs_JJC_WorkModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["734b1"], Mode = BindingMode.OneWay });
                hs_JJC_WorkModelMultiBind.NotifyOnSourceUpdated = true;
                this.tbSelectModel.SetBinding(TextBlock.TextProperty, hs_JJC_WorkModelMultiBind);

                this.btnLeftCatHeadReach.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["734b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnLeftCatHeadRetract.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["734b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRightCatHeadReach.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["734b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRightCatHeadRetract.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["734b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRotateCatHeadReach.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["734b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRotateCatHeadRetract.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["734b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                this.tbHotStartTmp.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_HotStartTmp"], Mode = BindingMode.OneWay,Converter=new DivideTenConverter()});
                this.tbHotStopTmp.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_HotStopTmp"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbCurOilLevel.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_CurOilLevel"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbLowOilAlarm.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_LowOilAlarm"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbLowStopPump.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_LowStopPump"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbCurSystemPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_CurSystemPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbCurCatPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_CurCatPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbCurCatOutputElec.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_CurCatOutputElec"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbMainOneTotolTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_MainOneTotolTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbMainOneCurTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_MainOneCurTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbMainTwoTotolTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_MainTwoTotolTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbMainTwoCurTime.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["HS_JJC_MainTwoCurTime"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private int iTimeCnt = 0;//用来为时钟计数的变量
        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Warnning();
                    this.Communcation();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool bCheckTwo = false;
        Dictionary<string, byte> alarmKey = new Dictionary<string, byte>();
        /// <summary>
        /// 通信数据
        /// </summary>
        private void Communcation()
        {
            #region 通信

            //操作台控制器心跳
            if (GlobalData.Instance.da["508b5"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 600)
                {
                    //Communication = 2;
                    this.tbAlarm.Text = "操作台信号中断";
                }
                if (!bCheckTwo && controlHeartTimes > 600)
                {
                    GlobalData.Instance.reportData.OperationFloorCommunication += 1;//the report
                    bCheckTwo = true;
                }
            }
            else
            {
                this.controlHeartTimes = 0;
                bCheckTwo = false;
            }
            this.tmpStatus = GlobalData.Instance.da["508b6"].Value.Boolean;

            if (!GlobalData.Instance.ComunciationNormal) this.tbAlarm.Text = "网络连接失败！";
            #endregion
        }

        private void InitAlarmKey()
        {
            alarmKey.Add("1号泵热继故障", 0);
            alarmKey.Add("2号泵热继故障", 0);
            alarmKey.Add("循环泵热继故障", 0);
            alarmKey.Add("冷却热继故障", 0);
            alarmKey.Add("液位过低", 0);
            alarmKey.Add("油温过高", 0);
            alarmKey.Add("油温过低", 0);
        }
        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
            if (GlobalData.Instance.da["735b1"].Value.Boolean)//1号泵热继故障
            {
                if(alarmKey["1号泵热继故障"] == 0) alarmKey["1号泵热继故障"] = 1;
            }
            else
            {
                alarmKey["1号泵热继故障"] = 0;
            }
            if (GlobalData.Instance.da["735b2"].Value.Boolean)//2号泵热继故障
            {
                if (alarmKey["2号泵热继故障"] == 0) alarmKey["2号泵热继故障"] = 1;
            }
            else
            {
                alarmKey["2号泵热继故障"] = 0;
            }
            if (GlobalData.Instance.da["735b3"].Value.Boolean)//循环泵热继故障
            {
                if (alarmKey["循环泵热继故障"] == 0) alarmKey["循环泵热继故障"] = 1;
            }
            else
            {
                alarmKey["循环泵热继故障"] = 0;
            }
            if (GlobalData.Instance.da["735b4"].Value.Boolean)//冷却热继故障
            {
                if (alarmKey["冷却热继故障"] == 0) alarmKey["冷却热继故障"] = 1;
            }
            else
            {
                alarmKey["冷却热继故障"] = 0;
            }
            if (GlobalData.Instance.da["735b5"].Value.Boolean)//液位过低
            {
                if (alarmKey["液位过低"] == 0) alarmKey["液位过低"] = 1;
            }
            else
            {
                alarmKey["液位过低"] = 0;
            }
            if (GlobalData.Instance.da["735b6"].Value.Boolean)//油温过高
            {
                if (alarmKey["油温过高"] == 0) alarmKey["油温过高"] = 1;
            }
            else
            {
                alarmKey["油温过高"] = 0;
            }
            if (GlobalData.Instance.da["735b7"].Value.Boolean)//油温过低
            {
                if (alarmKey["油温过低"] == 0) alarmKey["油温过低"] = 1;
            }
            else
            {
                alarmKey["油温过低"] = 0;
            }

            if (iTimeCnt % 10 == 0)
            {
                if (alarmKey.Count > 0)
                {
                    this.tbAlarm.FontSize = 14;
                    this.tbAlarm.Visibility = Visibility.Visible;

                    if (!alarmKey.ContainsValue(1) && alarmKey.ContainsValue(2)) // 如果没有显示为1的值，但是有显示为2的值，表示有告警且，显示循环完成，重置为1继续循环
                    {
                        foreach (var key in alarmKey.Keys.ToList())
                        {
                            if (alarmKey[key] == 2)
                            {
                                alarmKey[key] = 1;
                            }
                        }
                    }

                    foreach (var key in alarmKey.Keys.ToList())
                    {
                        if (alarmKey[key] == 1)
                        {
                            this.tbAlarm.Text = key;
                            alarmKey[key] = 2;
                            break;
                        }
                    }
                }
                else
                {
                    this.tbAlarm.Visibility = Visibility.Hidden;
                    this.tbAlarm.Text = "";
                }
            }
            else
            {
                this.tbAlarm.FontSize = 20;
            }
        }
        /// <summary>
        /// 主泵1启停
        /// </summary>
        private void btn_MainPumpOne(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpOne.IsChecked) //当前启动状态
            {
                byteToSend = new byte[10] { 20, 5, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前停止状态
            {
                byteToSend = new byte[10] { 20, 5, 1, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 主泵2启停
        /// </summary>
        private void btn_MainPumpTwo(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.MainPumpTwo.IsChecked) //当前启动状态
            {
                byteToSend = new byte[10] { 20, 5, 2, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前停止状态
            {
                byteToSend = new byte[10] { 20, 5, 2, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 冷却风机启停
        /// </summary>
        private void btn_ColdFan(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.ColdFan.IsChecked) //当前启动状态
            {
                byteToSend = new byte[10] { 20, 5, 6, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前停止状态
            {
                byteToSend = new byte[10] { 20, 5, 6, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 加热器启停
        /// </summary>
        private void btn_Hot(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.Hot.IsChecked) //当前启动状态
            {
                byteToSend = new byte[10] { 20, 5, 7, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前停止状态
            {
                byteToSend = new byte[10] { 20, 5, 7, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 循环泵启停
        /// </summary>
        private void btn_CyclePump(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.CyclePump.IsChecked) //当前启动状态
            {
                byteToSend = new byte[10] { 20, 5, 5, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前停止状态
            {
                byteToSend = new byte[10] { 20, 5, 5, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 设置猫头压力百分比
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strText = this.tbCatPressPer.Text;
                if (strText.Length == 0) strText = "0";
                short i16Text = (short)(double.Parse(strText) * 10);
                byte[] tempByte = BitConverter.GetBytes(i16Text);

                byte[] byteToSend = new byte[10] { 20, 5, 4, 1, tempByte[0], tempByte[1], 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            catch (Exception ex)
            { 
                MessageBox.Show("请输入数字!");
            }
        }

        private void tb_ParameterConfig_Focus(object sender, MouseButtonEventArgs e)
        {
            GlobalData.Instance.GetKeyBoard();
        }
        /// <summary>
        /// 控制模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ControlModel(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 8, 0, 0, 0, 0, 0, 0, 0 };

            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        #region 猫头控制
        /// <summary>
        /// 左猫头上升
        /// </summary>
        private void BtnLeftCatReach_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 1, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左猫头上升-按键抬起关闭
        /// </summary>
        private void BtnLeftCatHeadReach_MouseUp(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左猫头下降
        /// </summary>
        private void BtnLeftCatRetract_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 2, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左猫头下降-按键抬起关闭
        /// </summary>
        private void BtnLeftCatHeadRetract_MouseUp(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 左猫头关
        /// </summary>
        private void BtnLeftCatClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头上升
        /// </summary>
        private void BtnRightCatReach_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 1, 2, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头上升-按键抬起关闭
        /// </summary>
        private void BtnRightCatHeadReach_MouseUp(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 0, 2, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头下降
        /// </summary>
        private void BtnRightCatRetract_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 2, 2, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头下降-按键抬起关闭
        /// </summary>
        private void BtnRightCatHeadRetract_MouseUp(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 0, 2, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 右猫头关
        /// </summary>
        private void BtnRightCatClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 0, 2, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 旋转猫头正转
        /// </summary>
        private void BtnRotateCatReach_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 1, 3, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 旋转猫头正转-按键抬起关闭
        /// </summary>
        private void BtnRotateCatHeadReach_MouseUp(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 0, 3, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 旋转猫头反转
        /// </summary>
        private void BtnRotateCatRetract_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 2, 3, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 旋转猫头反转-按键抬起关闭
        /// </summary>
        private void BtnRotateCatHeadRetract_MouseUp(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 0, 3, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 旋转猫头关闭
        /// </summary>
        private void BtnRotateCatClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 0, 3, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion
        /// <summary>
        /// 待机模式选择
        /// </summary>
        private void btn_SelectStayWorkModel(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 1, 4, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 猫头供油模式
        /// </summary>
        private void btn_SelectCatWorkModel(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 20, 5, 3, 2, 4, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
