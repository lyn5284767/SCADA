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

namespace Main.SecondFloor
{
    /// <summary>
    /// SFPosSetTwo.xaml 的交互逻辑
    /// </summary>
    public partial class SFPosSetTwo : UserControl
    {
        private static SFPosSetTwo _instance = null;
        private static readonly object syncRoot = new object();

        public static SFPosSetTwo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFPosSetTwo();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        public SFPosSetTwo()
        {
            InitializeComponent();
            this.txtOpModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay, Converter = new OperationModelConverter() });
            this.txtWorkModel_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["workModel"], Mode = BindingMode.OneWay, Converter = new WorkModelConverter() });
            this.txtRotateAngle_LocationCalibration.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });

            timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 100);
        }


        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    int paramNO = GlobalData.Instance.da["Con_Set1"].Value.Byte;
                    if (paramNO == 97) this.twt97.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//3.5寸
                    if (paramNO == 110) this.twt110.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//4寸补偿
                    if (paramNO == 112) this.twt112.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//4.5寸补偿
                    if (paramNO == 98) this.twt98.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//5寸
                    if (paramNO == 114) this.twt114.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//5.5寸补偿
                    if (paramNO == 108) this.twt108.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//5寸7/8
                    if (paramNO == 99) this.twt99.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//6寸
                    if (paramNO == 100) this.twt100.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//6.5寸
                    if (paramNO == 109) this.twt109.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//6寸5/8
                    if (paramNO == 101) this.twt101.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//7寸
                    if (paramNO == 102) this.twt102.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//7.5寸
                    if (paramNO == 103) this.twt103.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//8寸
                    if (paramNO == 104) this.twt104.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//9寸
                    if (paramNO == 105) this.twt105.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//10寸
                    if (paramNO == 106) this.twt106.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//11寸
                    if (paramNO == 107) this.twt107.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//最大寸
                    if (paramNO == 129) this.twt129.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左手指最小值
                    if (paramNO == 130) this.twt130.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左手指最大值
                    if (paramNO == 131) this.twt131.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右手指最小值
                    if (paramNO == 132) this.twt132.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右手指最大值
                    
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
