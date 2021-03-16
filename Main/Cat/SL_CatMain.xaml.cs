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

namespace Main.Cat
{
    /// <summary>
    /// SL_CatMain.xaml 的交互逻辑
    /// </summary>
    public partial class SL_CatMain : UserControl
    {
        private static SL_CatMain _instance = null;
        private static readonly object syncRoot = new object();

        public static SL_CatMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SL_CatMain();
                        }
                    }
                }
                return _instance;
            }
        }
        public SL_CatMain()
        {
            InitializeComponent();
            VariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟
            InitAlarmKey();
        }
        System.Threading.Timer timerWarning;
        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            this.controlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["702b7"], Mode = BindingMode.OneWay });
            this.oprModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["703b0"], Mode = BindingMode.OneWay });
            this.LeftOrRight.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b6"], Mode = BindingMode.OneWay });

            //上/甩钻
            MultiBinding slCatDrillOprCoverterMultiBind = new MultiBinding();
            slCatDrillOprCoverterMultiBind.Converter = new SLCatDrillOprCoverter();
            slCatDrillOprCoverterMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["703b1"], Mode = BindingMode.OneWay });
            slCatDrillOprCoverterMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["703b2"], Mode = BindingMode.OneWay });
            slCatDrillOprCoverterMultiBind.NotifyOnSourceUpdated = true;
            this.drillOpr.SetBinding(BasedSwitchButton.IsCheckedProperty, slCatDrillOprCoverterMultiBind);
            this.smLeftGripInside.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b7"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smLeftGripOutside.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["702b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smLeftBoardPush.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["702b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smLeftBoardRetract.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["702b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smRightGripInside.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["702b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smRightGripOutside.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["702b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smRightBoardPush.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["702b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smRightBoardRetract.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["702b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });

            this.tbOnePumpPerPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_PumpOnePrePress"], Mode = BindingMode.OneWay,Converter = new DivideTenConverter() });
            this.tbOnePumpFlow.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_PumpOneFlow"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbOnePumpPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_PumpOnePress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbTwoPumpPerPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_PumpTwoPrePress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbTwoPumpFlow.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_PumpTwoFlow"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbTwoPumpPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_PumpTwoPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbBigCarPos.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["Cat_SL_WinchPos"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbPushCarPos.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["Cat_SL_CartPos"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbOilTmp.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_OilTmp"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            this.tbDrillType.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b5"], Mode = BindingMode.OneWay ,Converter = new SLCatDrillTypeConverter()});

            this.tbOnePumpStatus.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["703b4"], Mode = BindingMode.OneWay, Converter = new SLCatStartConverter() });
            this.tbTwoPumpStatus.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["703b5"], Mode = BindingMode.OneWay, Converter = new SLCatStartConverter() });
            this.tbFan.SetBinding(TextBlock.TextProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["703b6"], Mode = BindingMode.OneWay, Converter = new SLCatStartConverter() });
            #region 6键右手柄
            this.tbSixLeftOrRight.SetBinding(TextBox.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandLeftOrRight"], Mode = BindingMode.OneWay });
            this.tbSixFrontOrBehind.SetBinding(TextBox.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["RgihtHandFrontOrBehind"], Mode = BindingMode.OneWay });

            ArrowDirectionMultiConverter sixarrowDirectionMultiConverter = new ArrowDirectionMultiConverter();
            MultiBinding SixArrowMultiBind = new MultiBinding();
            SixArrowMultiBind.Converter = sixarrowDirectionMultiConverter;
            SixArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b4"], Mode = BindingMode.OneWay });
            SixArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b5"], Mode = BindingMode.OneWay });
            SixArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b2"], Mode = BindingMode.OneWay });
            SixArrowMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["517b3"], Mode = BindingMode.OneWay });
            SixArrowMultiBind.NotifyOnSourceUpdated = true;
            this.SixArrow_EquiptStatus.SetBinding(Image.SourceProperty, SixArrowMultiBind);
            this.smSixLUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smSixLDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b1"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smSixRUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b2"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smSixRDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b3"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smSixMidUp.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b4"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smSixMidDown.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b5"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smSixBack.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["517b0"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            this.smSixEnable.SetBinding(SymbolMapping.LampTypeProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["518b6"], Mode = BindingMode.OneWay, Converter = new BoolTagConverter() });
            #endregion
        }

        List<AlarmInfo> alarmList = new List<AlarmInfo>();
        /// <summary>
        /// 绑定告警变量
        /// </summary>
        private void InitAlarmKey()
        {
            alarmList.Add(new AlarmInfo() { TagName = "701b0", Description = "液位过高", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "701b1", Description = "液位过低", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "701b2", Description = "液位极低", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "701b3", Description = "温度过高", NowType = 0, NeedCheck = true });
            alarmList.Add(new AlarmInfo() { TagName = "701b4", Description = "温度过低", NowType = 0, NeedCheck = true });
          
        }

        private int iTimeCnt = 0;//用来为时钟计数的变量

        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    iTimeCnt++;
                    if (iTimeCnt > 1000) iTimeCnt = 0;
                    this.Warnning();
                    MonitorAlarmStatus();
             
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 监控告警状态
        /// </summary>
        private void MonitorAlarmStatus()
        {
            foreach (AlarmInfo info in alarmList)
            {
                if (GlobalData.Instance.da[info.TagName] != null)
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
            if (iTimeCnt % 10 == 0)
            {
                this.tbTips.Visibility = Visibility.Visible;
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
                        this.tbTips.Text = tmp.Description;
                        tmp.NowType = 2;
                    }
                }
                else
                {
                    this.tbTips.Text = "暂无告警";
                }
            }
            else
            {
            }
            #endregion
        }
        /// <summary>
        ///// 绑定变量
        ///// </summary>
        //private void BindField()
        //{
        //    if (GlobalData.Instance.da["Cat_SL_Tag"] == null) return;
        //    if (GlobalData.Instance.da["Cat_SL_Tag"].Value.Byte == 1)
        //    {
        //        this.tbInseideUpDownCarValue.Text = (GlobalData.Instance.da["Cat_SL_CasingWinchLastValue"].Value.Byte/10.0).ToString();
        //        this.tbInseidePushCarValue.Text = (GlobalData.Instance.da["Cat_SL_CasingCartLastValue"].Value.Byte / 10.0).ToString();
        //        this.tbOutseideUpDownCarValue.Text = (GlobalData.Instance.da["Cat_SL_PipeWinchLastValue"].Value.Byte / 10.0).ToString();
        //        this.tbOutseidePushCarValue.Text = (GlobalData.Instance.da["Cat_SL_PipeCartLastValue"].Value.Byte / 10.0).ToString();
        //        this.tbUpDownCarUpMaxValue.Text = (GlobalData.Instance.da["Cat_SL_WinchUpMax"].Value.Byte/10.0).ToString();
        //        this.tbCarTurnFlowValue.Text = (GlobalData.Instance.da["Cat_SL_WinchSlowFlow"].Value.Int16 / 10.0).ToString();
        //    }
        //    else if (GlobalData.Instance.da["Cat_SL_Tag"].Value.Byte == 2)
        //    {
        //        this.tbUpDownCarBackValue.Text = (GlobalData.Instance.da["Cat_SL_WinchBackLimit"].Value.Byte / 10.0).ToString();
        //        this.tbPushCarFrontValue.Text = (GlobalData.Instance.da["Cat_SL_CartFrontLimit"].Value.Byte / 10.0).ToString();
        //        this.tbDrillCarBackValue.Text = (GlobalData.Instance.da["Cat_SL_DrillCarBackLimit"].Value.Byte / 10.0).ToString();
        //        this.tbPipeCarBackValue.Text = (GlobalData.Instance.da["Cat_SL_PipeCarBackLimit"].Value.Byte / 10.0).ToString();
        //    }
        //}
        /// <summary>
        /// 遥控/司钻
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_controlModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            byteToSend = new byte[10] { 16, 1, 2, 0, 0, 0, 0, 0, 0, 0 }; 
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 手动/自动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_oprModel(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.oprModel.IsChecked)  // 当前自动转手动
            {
                byteToSend = new byte[10] { 80, 48, 1, 2, 0, 0, 0, 0, 0, 0 };
            }
            else //当前手动转自动
            {
                byteToSend = new byte[10] { 80, 48, 1, 1, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 上/甩钻
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_drillOpr(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.drillOpr.IsChecked)  // 当前上钻切换甩钻
            {
                byteToSend = new byte[10] { 80, 48, 2, 2, 0, 0, 0, 0, 0, 0 };
            }
            else //当前甩钻切换上钻
            {
                byteToSend = new byte[10] { 80, 48, 2, 1, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左右旋转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LeftOrRight(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.LeftOrRight.IsChecked)  // 当前左切换右
            {
                byteToSend = new byte[10] { 80, 48, 6, 2, 0, 0, 0, 0, 0, 0 };
            }
            else //当前甩右换左
            {
                byteToSend = new byte[10] { 80, 48, 6, 1, 0, 0, 0, 0, 0, 0 };
            }

            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 泵启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PumpSet(object sender, EventArgs e)
        {
            //byte[] byteToSend;
            //if (this.PumpSet.IsChecked)  // 当前停止转启动
            //{
            //    byteToSend = new byte[10] { 80, 48, 5, 1, 0, 0, 0, 0, 0, 0 };
            //}
            //else //当前停止转启动
            //{
            //    byteToSend = new byte[10] { 80, 48, 5, 2, 0, 0, 0, 0, 0, 0 };
            //}

            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
      
        /// <summary>
        /// 液压站启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHSStart_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 5, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 液压组停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHSStop_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 5, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
    }
}
