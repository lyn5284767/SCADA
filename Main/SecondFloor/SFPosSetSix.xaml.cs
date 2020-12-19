﻿using COM.Common;
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
    /// SFPosSetSix.xaml 的交互逻辑
    /// </summary>
    public partial class SFPosSetSix : UserControl
    {
        private static SFPosSetSix _instance = null;
        private static readonly object syncRoot = new object();

        public static SFPosSetSix Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFPosSetSix();
                        }
                    }
                }
                return _instance;
            }
        }
        System.Threading.Timer timer;
        public SFPosSetSix()
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
                    if (paramNO == 49) this.twt49.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩左1#钻铤
                    if (paramNO == 50) this.twt50.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩左2#钻铤
                    if (paramNO == 51) this.twt51.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩左3#钻铤
                    if (paramNO == 52) this.twt52.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩左4#钻铤
                    if (paramNO == 53) this.twt53.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩左5#钻铤
                    if (paramNO == 54) this.twt54.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩左6#钻铤
                    if (paramNO == 55) this.twt55.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩左7#钻铤
                    if (paramNO == 56) this.twt56.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩左8#钻铤

                    if (paramNO == 57) this.twt57.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩右1#钻铤
                    if (paramNO == 58) this.twt58.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩右2#钻铤
                    if (paramNO == 59) this.twt59.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩右3#钻铤
                    if (paramNO == 60) this.twt60.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩右4#钻铤
                    if (paramNO == 61) this.twt61.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩右5#钻铤
                    if (paramNO == 62) this.twt62.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩右6#钻铤
                    if (paramNO == 63) this.twt63.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩右7#钻铤
                    if (paramNO == 64) this.twt64.SetControlShow = GlobalData.Instance.da["108N23PositionCalibrationValue"].Value.Int32;//手臂伸缩右8#钻铤

                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}