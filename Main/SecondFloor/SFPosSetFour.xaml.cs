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
    /// SFPosSetFour.xaml 的交互逻辑
    /// </summary>
    public partial class SFPosSetFour : UserControl
    {
        private static SFPosSetFour _instance = null;
        private static readonly object syncRoot = new object();

        public static SFPosSetFour Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFPosSetFour();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        public SFPosSetFour()
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
                    if (paramNO == 141) this.twt141.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左1#最小值
                    if (paramNO == 142) this.twt142.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左2#最小值
                    if (paramNO == 143) this.twt143.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左3#最小值
                    if (paramNO == 144) this.twt144.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左4#最小值
                    if (paramNO == 145) this.twt145.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左5#最小值
                    if (paramNO == 146) this.twt146.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左6#最小值
                    if (paramNO == 147) this.twt147.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左7#最小值
                    if (paramNO == 148) this.twt148.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左8#最小值
                    
                    if (paramNO == 153) this.twt153.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左1#最大值
                    if (paramNO == 154) this.twt154.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左2#最大值
                    if (paramNO == 155) this.twt155.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左3#最大值
                    if (paramNO == 156) this.twt156.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左4#最大值
                    if (paramNO == 157) this.twt157.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左5#最大值
                    if (paramNO == 158) this.twt158.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左6#最大值
                    if (paramNO == 159) this.twt159.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左7#最大值
                    if (paramNO == 160) this.twt160.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左8#最大值

                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
