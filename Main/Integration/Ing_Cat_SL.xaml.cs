using COM.Common;
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

namespace Main.Integration
{
    /// <summary>
    /// Ing_Cat_SL.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_Cat_SL : UserControl
    {
        private static Ing_Cat_SL _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_Cat_SL Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_Cat_SL();
                        }
                    }
                }
                return _instance;
            }
        }
        public Ing_Cat_SL()
        {
            InitializeComponent();
            CatVariableBinding();
        }

        private void CatVariableBinding()
        {
            try
            {
                this.tbOnePumpFlow.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_PumpOneFlow"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbOnePumpPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_PumpOnePress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbTwoPumpFlow.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_PumpTwoFlow"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbTwoPumpPress.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["Cat_SL_PumpTwoPress"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
