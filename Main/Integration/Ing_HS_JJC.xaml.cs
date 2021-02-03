using COM.Common;
using ControlLibrary;
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
        public Ing_HS_JJC()
        {
            InitializeComponent();
            VariableBinding();
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
    }
}
