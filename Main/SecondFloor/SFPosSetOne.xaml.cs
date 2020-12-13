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
    /// SFPosSetNew.xaml 的交互逻辑
    /// </summary>
    public partial class SFPosSetOne : UserControl
    {
        private static SFPosSetOne _instance = null;
        private static readonly object syncRoot = new object();

        public static SFPosSetOne Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFPosSetOne();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        public SFPosSetOne()
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
                    if (paramNO == 1) this.twt1.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//最靠近井口位置
                    if (paramNO == 2) this.twt2.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//最远离井口位置
                    if (paramNO == 3) this.twt3.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右1#指梁位置
                    if (paramNO == 4) this.twt4.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左1#指梁位置
                    if (paramNO == 6) this.twt6.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//小车排杆待机位置
                    if (paramNO == 7) this.twt7.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//小车回收位置
                    if (paramNO == 10) this.twt10.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//小车运输位置
                    if (paramNO == 21) this.twt21.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右16#指梁位置补偿
                    if (paramNO == 22) this.twt22.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左16#指梁位置补偿
                    if (paramNO == 23) this.twt23.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//右1#特殊指梁位置
                    if (paramNO == 24) this.twt24.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//左1#特殊指梁位置
                    
                    if (paramNO == 33) this.twt33.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂最小缩回位置
                    if (paramNO == 34) this.twt34.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//钻杆指梁手臂最大伸展位置
                    if (paramNO == 39) this.twt39.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂回收位置
                    if (paramNO == 40) this.twt40.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂井口最大伸展位置
                    if (paramNO == 42) this.twt42.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂运输位置
                    if (paramNO == 43) this.twt43.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//钻铤指梁手臂最大位置
                    if (paramNO == 65) this.twt65.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//回转-90°位置
                    if (paramNO == 66) this.twt66.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//回转90°位置
                    if (paramNO == 71) this.twt71.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//回转回收位置
                    if (paramNO == 72) this.twt72.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//回转井口位置
                    if (paramNO == 74) this.twt74.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//回转运输位置

                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
