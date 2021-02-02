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
    /// Ing_HS_Self.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_HS_Self : UserControl
    {
        private static Ing_HS_Self _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_HS_Self Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_HS_Self();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timerWarning;
        private int iTimeCnt = 0;
        private int controlHeartTimes = 0; // 控制台心跳次数
        private bool tmpStatus = false; // 控制台心跳临时存储状态
        private bool bCommunicationCheck = false; // 是否有中断标志
        private Dictionary<string, int> tipList = new Dictionary<string, int>(); //告警列表
        public Ing_HS_Self()
        {
            InitializeComponent();
            HSVariableBinding();
            //HSVariableReBinding = new System.Threading.Timer(new TimerCallback(HSVariableTimer), null, Timeout.Infinite, 500);
            //HSVariableReBinding.Change(0, 500);
            timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟
        }

        #region 液压站
        bool bMainPumpOne = false;
        bool MainPumpOneCheck = false;
        bool bMainPumpTwo = false;
        bool MainPumpTwoCheck = false;
        System.Threading.Timer HSVariableReBinding;
        private void HSVariableBinding()
        {
            try
            {
                //this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay });
                //HyControlModelMuilCoverter hyControlModelMultiConverter = new HyControlModelMuilCoverter();
                //MultiBinding hyControlModelMultiBind = new MultiBinding();
                //hyControlModelMultiBind.Converter = hyControlModelMultiConverter;
                //hyControlModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay });
                //hyControlModelMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b6"], Mode = BindingMode.OneWay });
                //hyControlModelMultiBind.NotifyOnSourceUpdated = true;
                //this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, hyControlModelMultiBind);
                //HyControlModelTxtMuilCoverter hyControlModelTxtMultiConverter = new HyControlModelTxtMuilCoverter();
                //MultiBinding hyControlModelTxtlMultiBind = new MultiBinding();
                //hyControlModelTxtlMultiBind.Converter = hyControlModelTxtMultiConverter;
                //hyControlModelTxtlMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b5"], Mode = BindingMode.OneWay });
                //hyControlModelTxtlMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["771b6"], Mode = BindingMode.OneWay });
                //hyControlModelTxtlMultiBind.NotifyOnSourceUpdated = true;
                //this.controlModel.SetBinding(BasedSwitchButton.ContentDownProperty, hyControlModelTxtlMultiBind);
                //this.MainPumpOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b3"], Mode = BindingMode.OneWay });
                //this.MainPumpTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b5"], Mode = BindingMode.OneWay });
                //this.HSConstantVoltagePump.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b7"], Mode = BindingMode.OneWay });
                //this.HSDissipateHeat.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b1"], Mode = BindingMode.OneWay });
                //this.HSHot.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b3"], Mode = BindingMode.OneWay });
                this.tbMainPumpOne.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b2"], Mode = BindingMode.OneWay, Converter = new PumpStartConverter() });
                this.tbMainPumpTwo.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b4"], Mode = BindingMode.OneWay, Converter = new PumpStartConverter() });
                this.tbConstantVoltage.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b6"], Mode = BindingMode.OneWay, Converter = new PumpStartConverter() });
                this.tbDissipateHeat.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["771b0"], Mode = BindingMode.OneWay, Converter = new PumpStartConverter() });

                this.tbOil.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["OilTemAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbLevel.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["OilLevelAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        //司钻/本地控制
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            byteToSend = new byte[10] { 0, 19, 1, 9, 0, 0, 0, 0, 0, 0 };// 疑问，所有切换都是这个协议？
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 主泵1启动/停止
        /// </summary>
        private void btn_MainPumpOne(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.MainPumpOne.IsChecked) //当前停止状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 3, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前启动状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 3, 2, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
            //this.MainPumpOne.ContentDown = "切换中";
            //this.MainPumpOneCheck = true;
        }
        /// <summary>
        /// 主泵2启动/停止
        /// </summary>
        private void btn_MainPumpTwo(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.MainPumpTwo.IsChecked) //当前停止状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 3, 3, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前启动状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 3, 4, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
            //this.MainPumpTwo.ContentDown = "切换中";
            //this.MainPumpTwoCheck = true;
        }
        /// <summary>
        /// 恒压泵启动/停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_HSConstantVoltagePump(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.HSConstantVoltagePump.IsChecked) //当前停止状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 3, 5, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前启动状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 3, 6, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 散热泵启动/停止
        /// </summary>
        private void btn_HSDissipateHeat(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.HSDissipateHeat.IsChecked) //当前停止状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 3, 7, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前启动状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 3, 8, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 加热器启动/停止
        /// </summary>
        private void btn_HSHot(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.HSHot.IsChecked) //当前停止状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 3, 9, 0, 0, 0, 0, 0, 0 };
            //}
            //else//当前启动状态
            //{
            //    byteToSend = new byte[10] { 0, 19, 3, 10, 0, 0, 0, 0, 0, 0 };
            //}
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void HSVariableTimer(object value)
        {
            //if (this.bMainPumpOne != GlobalData.Instance.da["770b3"].Value.Boolean && this.MainPumpOneCheck)
            //{
            //    this.MainPumpOne.Dispatcher.Invoke(new Action(() =>
            //    {
            //        this.MainPumpOne.ContentDown = "1#主泵";
            //    }));
            //    MainPumpOneCheck = false;
            //}
            //bMainPumpOne = GlobalData.Instance.da["770b3"].Value.Boolean;
            //if (this.bMainPumpTwo != GlobalData.Instance.da["770b5"].Value.Boolean && this.MainPumpTwoCheck)
            //{
            //    this.MainPumpOne.Dispatcher.Invoke(new Action(() =>
            //    {
            //        this.MainPumpTwo.ContentDown = "2#主泵";
            //    }));
            //    MainPumpTwoCheck = false;
            //}
            //bMainPumpTwo = GlobalData.Instance.da["770b5"].Value.Boolean;
        }
        #endregion

        /// <summary>
        /// 报警时钟
        /// </summary>
        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Warnning();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 告警信号
        /// </summary>
        private void Warnning()
        {
            #region 告警
            int key = 0;
            if (GlobalData.Instance.da["771b7"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站急停", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站急停", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站急停", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站急停");
                }
            }

            if (GlobalData.Instance.da["774b0"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压油高温报警，请及时降温", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压油高温报警，请及时降温", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压油高温报警，请及时降温", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压油高温报警，请及时降温");
                }
            }

            if (GlobalData.Instance.da["774b1"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压油高温预警，请及时降温", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压油高温预警，请及时降温", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压油高温预警，请及时降温", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压油高温预警，请及时降温");
                }
            }
            if (GlobalData.Instance.da["774b2"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压油温度过低，请开启加热", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压油温度过低，请开启加热", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压油温度过低，请开启加热", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压油温度过低，请开启加热");
                }
            }
            if (GlobalData.Instance.da["774b3"].Value.Boolean)
            {
                this.tipList.TryGetValue("低液位预警，请及时加注液压油", out key);
                if (key == 0)
                {
                    this.tipList.Add("低液位预警，请及时加注液压油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("低液位预警，请及时加注液压油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("低液位预警，请及时加注液压油");
                }
            }
            if (GlobalData.Instance.da["774b4"].Value.Boolean)
            {
                this.tipList.TryGetValue("低液位报警，请及时加注液压油", out key);
                if (key == 0)
                {
                    this.tipList.Add("低液位报警，请及时加注液压油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("低液位报警，请及时加注液压油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("低液位报警，请及时加注液压油");
                }
            }
            if (GlobalData.Instance.da["774b5"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压位异常降低，请检测漏油", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压位异常降低，请检测漏油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压位异常降低，请检测漏油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压位异常降低，请检测漏油");
                }
            }
            if (GlobalData.Instance.da["774b6"].Value.Boolean)
            {
                this.tipList.TryGetValue("加热效果异常，请检测加热器", out key);
                if (key == 0)
                {
                    this.tipList.Add("加热效果异常，请检测加热器", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("加热效果异常，请检测加热器", out key);
                if (key != 0)
                {
                    this.tipList.Remove("加热效果异常，请检测加热器");
                }
            }
            if (GlobalData.Instance.da["775b0"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-主泵1已连续运行500小时，请切换主泵2使用", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-主泵1已连续运行500小时，请切换主泵2使用", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-主泵1已连续运行500小时，请切换主泵2使用", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-主泵1已连续运行500小时，请切换主泵2使用");
                }
            }
            if (GlobalData.Instance.da["775b1"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-主泵2已连续运行500小时，请切换主泵1使用", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-主泵2已连续运行500小时，请切换主泵1使用", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-主泵2已连续运行500小时，请切换主泵1使用", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-主泵2已连续运行500小时，请切换主泵1使用");
                }
            }
            if (GlobalData.Instance.da["775b2"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-主电机1已连续运行600小时，请加注黄油", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-主电机1已连续运行600小时，请加注黄油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-主电机1已连续运行600小时，请加注黄油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-主电机1已连续运行600小时，请加注黄油");
                }
            }
            if (GlobalData.Instance.da["775b3"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-主电机2已连续运行600小时，请加注黄油", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-主电机2已连续运行600小时，请加注黄油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-主电机2已连续运行600小时，请加注黄油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-主电机2已连续运行600小时，请加注黄油");
                }
            }
            if (GlobalData.Instance.da["775b4"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-距上次更换滤芯已经大于2000小时，请更换滤芯", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-距上次更换滤芯已经大于2000小时，请更换滤芯", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-距上次更换滤芯已经大于2000小时，请更换滤芯", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-距上次更换滤芯已经大于2000小时，请更换滤芯");
                }
            }
            if (GlobalData.Instance.da["775b5"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-距上次更换液压油已经大于2000小时，请更换液压油", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-距上次更换液压油已经大于2000小时，请更换液压油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-距上次更换液压油已经大于2000小时，请更换液压油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-距上次更换液压油已经大于2000小时，请更换液压油");
                }
            }
            if (GlobalData.Instance.da["775b6"].Value.Boolean)
            {
                this.tipList.TryGetValue("液压站-油位下降异常，请检查是否漏油", out key);
                if (key == 0)
                {
                    this.tipList.Add("液压站-油位下降异常，请检查是否漏油", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("液压站-油位下降异常，请检查是否漏油", out key);
                if (key != 0)
                {
                    this.tipList.Remove("液压站-油位下降异常，请检查是否漏油");
                }
            }

            if (iTimeCnt % 10 == 0)
            {
                this.tbTips.FontSize = 24;
            }
            else
            {
                this.tbTips.FontSize = 28;
            }
            #endregion

            //操作台控制器心跳
            if (GlobalData.Instance.da["504b7"].Value.Boolean == this.tmpStatus)
            {
                this.controlHeartTimes += 1;
                if (this.controlHeartTimes > 60)
                {
                    this.tipList.TryGetValue("液压站与操作台信号中断", out key);
                    if (key == 0)
                    {
                        this.tipList.Add("液压站与操作台信号中断", 1);
                    }
                }
                else
                {
                    this.tipList.TryGetValue("液压站与操作台信号中断", out key);
                    if (key != 0)
                    {
                        this.tipList.Remove("液压站与操作台信号中断");
                    }
                }
                if (!bCommunicationCheck && controlHeartTimes > 60)
                {
                    bCommunicationCheck = true;
                }
            }
            else
            {
                this.controlHeartTimes = 0;
            }
            this.tmpStatus = GlobalData.Instance.da["504b7"].Value.Boolean;

            if (!GlobalData.Instance.ComunciationNormal)
            {
                this.tipList.TryGetValue("网络连接失败", out key);
                if (key == 0)
                {
                    this.tipList.Add("网络连接失败", 1);
                }
            }
            else
            {
                this.tipList.TryGetValue("网络连接失败", out key);
                if (key != 0)
                {
                    this.tipList.Remove("网络连接失败");
                }
            }

            if (iTimeCnt % 10 == 0)
            {
                if (this.tipList.Count > 0)
                {
                    this.tbTips.FontSize = 20;
                    this.tbTips.Visibility = Visibility.Visible;

                    if (!this.tipList.ContainsValue(1))
                    {
                        this.tipList.Keys.ToList().ForEach(k => this.tipList[k] = 1);
                    }

                    foreach (var tkey in this.tipList.Keys.ToList())
                    {
                        if (this.tipList[tkey] == 1)
                        {
                            this.tbTips.Text = tkey;
                            this.tipList[tkey] = 2;
                            break;
                        }
                    }
                }
                else
                {
                    this.tbTips.Visibility = Visibility.Hidden;
                    this.tbTips.Text = "";
                }
            }
            else
            {
                this.tbTips.FontSize = 28;
            }

        }
    }
}
