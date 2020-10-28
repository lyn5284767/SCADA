using COM.Common;
using ControlLibrary.InputControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main.SIR.SanyRail
{
    /// <summary>
    /// SIRPosSet.xaml 的交互逻辑
    /// </summary>
    public partial class SIRRailWayPosSet : UserControl
    {
        private static SIRRailWayPosSet _instance = null;
        private static readonly object syncRoot = new object();

        public static SIRRailWayPosSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SIRRailWayPosSet();
                        }
                    }
                }
                return _instance;
            }
        }
        public SIRRailWayPosSet()
        {
            InitializeComponent();
            VariableBinding();
        }

        private void VariableBinding()
        {
            try
            {
                this.twt1.SetBinding(TextWithBtnNew.ShowTxtWithBtnTxtProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_StaySet"], Mode = BindingMode.OneWay });
                this.twt2.SetBinding(TextWithBtnNew.ShowTxtWithBtnTxtProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_WellSet"], Mode = BindingMode.OneWay });
                this.twt3.SetBinding(TextWithBtnNew.ShowTxtWithBtnTxtProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_MouseSet"], Mode = BindingMode.OneWay });
                this.twt4.SetBinding(TextWithBtnNew.ShowTxtWithBtnTxtProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIE_RailWay_RailWayFrontLocation"], Mode = BindingMode.OneWay });
                this.twt5.SetBinding(TextWithBtnNew.ShowTxtWithBtnTxtProperty, new Binding("IntTag") { Source = GlobalData.Instance.da["SIE_RailWay_RailWayBackLocation"], Mode = BindingMode.OneWay });
                this.twt6.SetBinding(TextWithBtnNew.ShowTxtWithBtnTxtProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SIE_RailWay_TongsHeightSet"], Mode = BindingMode.OneWay });

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.StackTrace);
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
