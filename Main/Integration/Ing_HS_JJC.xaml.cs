using COM.Common;
using ControlLibrary;
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
    /// Ing_HS_JJC.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_HS_JJC : UserControl
    {
        private static Ing_HS_JJC _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_HS_JJC Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_HS_JJC();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        public Ing_HS_JJC()
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
                //this.ControlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["735b0"], Mode = BindingMode.OneWay });
                //this.MainPumpOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b3"], Mode = BindingMode.OneWay });
                //this.MainPumpTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b4"], Mode = BindingMode.OneWay });
                //this.CyclePump.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b5"], Mode = BindingMode.OneWay });
                //this.ColdFan.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b6"], Mode = BindingMode.OneWay });
                //this.Hot.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["733b7"], Mode = BindingMode.OneWay });

             
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 主泵1启停
        /// </summary>
        private void btn_MainPumpOne(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.MainPumpOne.IsChecked) //当前启动状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 1, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前停止状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 1, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 主泵2启停
        /// </summary>
        private void btn_MainPumpTwo(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.MainPumpTwo.IsChecked) //当前启动状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 2, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前停止状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 2, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 冷却风机启停
        /// </summary>
        private void btn_ColdFan(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.ColdFan.IsChecked) //当前启动状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 6, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前停止状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 6, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 加热器启停
        /// </summary>
        private void btn_Hot(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.Hot.IsChecked) //当前启动状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 7, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前停止状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 7, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 循环泵启停
        /// </summary>
        private void btn_CyclePump(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.CyclePump.IsChecked) //当前启动状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 5, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前停止状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 5, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 控制模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ControlModel(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 8, 0, 0, 0, 0, 0, 0, 0 };

            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private int iTimeCnt = 0;//用来为时钟计数的变量
        BrushConverter bc = new BrushConverter();
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
                    this.MonitorAlarmStatus();
                    //SendByCycle();
                    if (this.tbAlarm.Text == "暂无告警") this.tbAlarm.Foreground = (Brush)bc.ConvertFrom("#000000");
                    else this.tbAlarm.Foreground = (Brush)bc.ConvertFrom("#E0496D");
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
        //Dictionary<string, byte> alarmKey = new Dictionary<string, byte>();
        List<AlarmInfo> alarmList = new List<AlarmInfo>();
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
            else
            {
                if (this.tbAlarm.Text == "网络连接失败！") this.tbAlarm.Text = "";
            }
            #endregion
        }

        private void InitAlarmKey()
        {
            alarmList.Add(new AlarmInfo() { TagName = "733b0", Description = "液压站急停", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "733b1", Description = "软启动器运行中", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "735b5", Description = "液位过低，请及时加注液压油", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "735b6", Description = "油温过高，请及时开启散热", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "735b7", Description = "油温过低，请开启加热", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "735b1", Description = "1号泵热继故障", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "735b2", Description = "2号泵热继故障", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "735b3", Description = "循环泵热继故障", NowType = 0 });
            alarmList.Add(new AlarmInfo() { TagName = "735b4", Description = "冷却泵热继故障", NowType = 0 });
        }

        /// <summary>
        /// 监控告警状态
        /// </summary>
        private void MonitorAlarmStatus()
        {
            foreach (AlarmInfo info in alarmList)
            {
                if (info.TagName == "733b0")
                {
                    if (!GlobalData.Instance.da[info.TagName].Value.Boolean)//有报警
                    {
                        if (info.NowType == 0) info.NowType = 1;// 前状态为未告警
                    }
                    else
                    {
                        info.NowType = 0;
                    }
                }
                else
                {
                    if (GlobalData.Instance.da[info.TagName].Value.Boolean)//有报警
                    {
                        if (info.NowType == 0) info.NowType = 1;// 前状态为未告警
                    }
                    else
                    {
                        info.NowType = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
            #region 告警

            // 告警列表!=0则有告警 
            if (alarmList.Where(w => w.NowType != 0).Count() > 0)
            {

                // 有告警且全部显示完成
                if (this.alarmList.Where(w => w.NowType == 1).Count() == 0)
                {
                    this.alarmList.Where(w => w.NowType == 2).ToList().ForEach(w => w.NowType = 1);
                }
                AlarmInfo tmp = this.alarmList.Where(w => w.NowType == 1).FirstOrDefault();
                if (tmp != null)
                {
                    this.tbAlarm.FontSize = 18;
                    this.tbAlarm.Text = tmp.Description;
                    tmp.NowType = 2;
                }
            }
            else
            {
                this.tbAlarm.Text = "暂无告警";
            }
            #endregion
        }
    }
}
