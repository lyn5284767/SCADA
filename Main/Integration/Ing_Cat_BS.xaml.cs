﻿using COM.Common;
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
    /// Ing_Cat_BS.xaml 的交互逻辑
    /// </summary>
    public partial class Ing_Cat_BS : UserControl
    {
        private static Ing_Cat_BS _instance = null;
        private static readonly object syncRoot = new object();

        public static Ing_Cat_BS Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Ing_Cat_BS();
                        }
                    }
                }
                return _instance;
            }
        }
        public Ing_Cat_BS()
        {
            InitializeComponent();
            CatVariableBinding();
        }

        #region 猫道
        private void CatVariableBinding()
        {
            try
            {
                this.CatControlModel.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["701b0"], Mode = BindingMode.OneWay });
                this.CatMainPumpOne.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b1"], Mode = BindingMode.OneWay, Converter = new CheckedIsFalseConverter() });
                this.CatMainPumpTwo.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b3"], Mode = BindingMode.OneWay, Converter = new CheckedIsFalseConverter() });
                this.CatLeftOrRight.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["706b1"], Mode = BindingMode.OneWay });
                this.CatInOrOut.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["706b3"], Mode = BindingMode.OneWay });

                this.cbDRSafeLimit.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["505b7"], Mode = BindingMode.OneWay });
                this.cbIgnoreLimit.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b6"], Mode = BindingMode.OneWay });
                this.cbSelectPipe.SetBinding(CheckBox.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["705b7"], Mode = BindingMode.OneWay });

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 控制模式本地/司钻
        /// </summary>
        private void btn_CatcontrolModel(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 8, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 1#泵启动/停止
        /// </summary>
        private void btn_CatMainPumpOne(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 5, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 2#泵启动/停止
        /// </summary>
        private void btn_CatMainPumpTwo(object sender, EventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 5, 2, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左右选择
        /// </summary>
        private void btn_CatLeftOrRight(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.CatLeftOrRight.IsChecked) //当前左
            {
                byteToSend = new byte[10] { 80, 48, 6, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前右
            {
                byteToSend = new byte[10] { 80, 48, 6, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 内外选择
        /// </summary>
        private void btn_CatInOrLeft(object sender, EventArgs e)
        {
            byte[] byteToSend;
            if (this.CatLeftOrRight.IsChecked) //当前内
            {
                byteToSend = new byte[10] { 80, 48, 7, 2, 0, 0, 0, 0, 0, 0 };
            }
            else//当前外
            {
                byteToSend = new byte[10] { 80, 48, 7, 1, 0, 0, 0, 0, 0, 0 };
            }
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 钻台面安全限制解除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDRSafeLimit_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked.Value)
            {
                byte[] byteToSend = new byte[10] { 80, 48, 9, 0, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            else
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("确认解除钻台面对猫道得安全设置?", "提示", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    byte[] byteToSend = new byte[10] { 80, 48, 9, 0, 1, 0, 0, 0, 0, 0 };
                    GlobalData.Instance.da.SendBytes(byteToSend);
                }
            }
        }

        /// <summary>
        /// 忽略限制
        /// </summary>
        private void cbIgnoreLimit_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 2, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 选择套管
        /// </summary>
        private void cbSelectPipe_Checked(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 80, 48, 3, 1, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        #endregion
    }
}