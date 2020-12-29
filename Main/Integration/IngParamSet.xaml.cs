using COM.Common;
using ControlLibrary;
using ControlLibrary.InputControl;
using DatabaseLib;
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
    /// IngParamSet.xaml 的交互逻辑
    /// </summary>
    public partial class IngParamSet : UserControl
    {
        private static IngParamSet _instance = null;
        private static readonly object syncRoot = new object();

        public static IngParamSet Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IngParamSet();
                        }
                    }
                }
                return _instance;
            }
        }
        public IngParamSet()
        {
            InitializeComponent();
            VariableBinding();
        }

        /// <summary>
        /// 绑定变量
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                #region 配置参数 弃用
                //this.twtR17.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["171LeftHandleConfigurationInformation"], Mode = BindingMode.OneWay });
                //this.twtR18.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["172RightHandleConfigurationInformation"], Mode = BindingMode.OneWay });
                //this.twtR19.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["KeyPanelSetting"], Mode = BindingMode.OneWay });
                //this.twtR20.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["OperPanelSetting"], Mode = BindingMode.OneWay });
                //this.twtR21.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["DRTelectrSetting"], Mode = BindingMode.OneWay });
                //this.twtR22.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["Kava"], Mode = BindingMode.OneWay });
                //this.twtR23.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["IDFactoryType"], Mode = BindingMode.OneWay });
                //this.twtR24.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["CatType"], Mode = BindingMode.OneWay });
                //this.twtR25.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["HSType"], Mode = BindingMode.OneWay });
                //this.twtR26.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["DRType"], Mode = BindingMode.OneWay });
                //this.twtR27.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SFType"], Mode = BindingMode.OneWay });
                //this.twtR28.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["ElevatorType"], Mode = BindingMode.OneWay });
                //this.twtR29.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["ClearBtnType"], Mode = BindingMode.OneWay });
                //this.twtR30.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["PreventBoxType"], Mode = BindingMode.OneWay });
                //this.twtR31.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["HookType"], Mode = BindingMode.OneWay });
                //this.twtR32.SetBinding(TextBlockWithTextBox.ShowTextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["TopType"], Mode = BindingMode.OneWay });
                #endregion
                #region 配置参数
                // 左手柄
                this.twt1.dicNumToValue = new Dictionary<int, string>();
                this.twt1.dicNumToValue.Add(0, "无");
                this.twt1.dicNumToValue.Add(1, "4按键CAN0总线手柄");
                this.twt1.dicNumToValue.Add(2, "4按键CAN1总线手柄");
                this.twt1.dicNumToValue.Add(3, "2按键CAN0总线手柄");
                this.twt1.dicNumToValue.Add(4, "2按键CAN1总线手柄");
                this.twt1.dicNumToValue.Add(7, "模拟手柄");
                this.twt1.dicNumToValue.Add(8, "6按键CAN0总线手柄");
                this.twt1.dicNumToValue.Add(9, "6按键CAN1总线手柄");
                this.twt1.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["171LeftHandleConfigurationInformation"], Mode = BindingMode.OneWay,ConverterParameter= this.twt1.dicNumToValue,Converter=new NumToTextConverter() });
                // 右手柄
                this.twt2.dicNumToValue = new Dictionary<int, string>();
                this.twt2.dicNumToValue.Add(0, "无");
                this.twt2.dicNumToValue.Add(3, "2按键CAN0总线手柄");
                this.twt2.dicNumToValue.Add(4, "2按键CAN1总线手柄");
                this.twt2.dicNumToValue.Add(5, "4按键CAN0总线手柄");
                this.twt2.dicNumToValue.Add(6, "4按键CAN1总线手柄");
                this.twt2.dicNumToValue.Add(7, "模拟手柄");
                this.twt2.dicNumToValue.Add(8, "6按键CAN0总线手柄");
                this.twt2.dicNumToValue.Add(9, "6按键CAN1总线手柄");
                this.twt2.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["172RightHandleConfigurationInformation"], Mode = BindingMode.OneWay, ConverterParameter = this.twt2.dicNumToValue, Converter = new NumToTextConverter() });
                // 按键面板
                this.twt3.dicNumToValue = new Dictionary<int, string>();
                this.twt3.dicNumToValue.Add(0, "无");
                this.twt3.dicNumToValue.Add(1, "8按键");
                this.twt3.dicNumToValue.Add(2, "20按键");
                this.twt3.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["KeyPanelSetting"], Mode = BindingMode.OneWay, ConverterParameter = this.twt3.dicNumToValue, Converter = new NumToTextConverter() });
                // 操作台
                this.twt4.dicNumToValue = new Dictionary<int, string>();
                this.twt4.dicNumToValue.Add(0, "单机");
                this.twt4.dicNumToValue.Add(1, "SDCH 230F/230H");
                this.twt4.dicNumToValue.Add(2, "SDCH230E");
                this.twt4.dicNumToValue.Add(3, "SDCH280C");
                this.twt4.dicNumToValue.Add(4, "PX2230C.11");
                this.twt4.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["OperPanelSetting"], Mode = BindingMode.OneWay, ConverterParameter = this.twt4.dicNumToValue, Converter = new NumToTextConverter() });
                // 钻台面遥控器
                this.twt5.dicNumToValue = new Dictionary<int, string>();
                this.twt5.dicNumToValue.Add(0, "无");
                this.twt5.dicNumToValue.Add(1, "遥感");
                this.twt5.dicNumToValue.Add(2, "手持");
                this.twt5.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["DRTelectrSetting"], Mode = BindingMode.OneWay, ConverterParameter = this.twt5.dicNumToValue, Converter = new NumToTextConverter() });
                // 卡瓦
                this.twt6.dicNumToValue = new Dictionary<int, string>();
                this.twt6.dicNumToValue.Add(0, "无");
                this.twt6.dicNumToValue.Add(1, "有");
                this.twt6.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["Kava"], Mode = BindingMode.OneWay, ConverterParameter = this.twt6.dicNumToValue, Converter = new NumToTextConverter() });
                // 铁钻工厂家
                this.twt7.dicNumToValue = new Dictionary<int, string>();
                this.twt7.dicNumToValue.Add(0, "无");
                this.twt7.dicNumToValue.Add(1, "三一");
                this.twt7.dicNumToValue.Add(2, "JJC");
                this.twt7.dicNumToValue.Add(3, "宝石");
                this.twt7.dicNumToValue.Add(4, "江汉");
                this.twt7.dicNumToValue.Add(5, "三一轨道式");
                this.twt7.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SIRType"], Mode = BindingMode.OneWay, ConverterParameter = this.twt7.dicNumToValue, Converter = new NumToTextConverter() });
                // 猫道厂家
                this.twt8.dicNumToValue = new Dictionary<int, string>();
                this.twt8.dicNumToValue.Add(0, "无");
                this.twt8.dicNumToValue.Add(1, "三一");
                this.twt8.dicNumToValue.Add(2, "宝石");
                this.twt8.dicNumToValue.Add(3, "宏达");
                this.twt8.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["CatType"], Mode = BindingMode.OneWay, ConverterParameter = this.twt8.dicNumToValue, Converter = new NumToTextConverter() });
                // 液压站厂家
                this.twt9.dicNumToValue = new Dictionary<int, string>();
                this.twt9.dicNumToValue.Add(0, "无");
                this.twt9.dicNumToValue.Add(1, "三一");
                this.twt9.dicNumToValue.Add(2, "宝石");
                this.twt9.dicNumToValue.Add(3, "JJC");
                this.twt9.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["HSType"], Mode = BindingMode.OneWay, ConverterParameter = this.twt9.dicNumToValue, Converter = new NumToTextConverter() });
                // 钻台面厂家
                this.twt10.dicNumToValue = new Dictionary<int, string>();
                this.twt10.dicNumToValue.Add(0, "无");
                this.twt10.dicNumToValue.Add(1, "三一");
                this.twt10.dicNumToValue.Add(2, "杰瑞");
                this.twt10.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["DRType"], Mode = BindingMode.OneWay, ConverterParameter = this.twt10.dicNumToValue, Converter = new NumToTextConverter() });
                // 二层台厂家
                this.twt11.dicNumToValue = new Dictionary<int, string>();
                this.twt11.dicNumToValue.Add(0, "无");
                this.twt11.dicNumToValue.Add(1, "三一");
                this.twt11.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["SFType"], Mode = BindingMode.OneWay, ConverterParameter = this.twt11.dicNumToValue, Converter = new NumToTextConverter() });
                // 吊卡厂家
                this.twt12.dicNumToValue = new Dictionary<int, string>();
                this.twt12.dicNumToValue.Add(0, "无");
                this.twt12.dicNumToValue.Add(1, "如通带关门");
                this.twt12.dicNumToValue.Add(2, "如通带关门和管柱");
                this.twt12.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["ElevatorType"], Mode = BindingMode.OneWay, ConverterParameter = this.twt12.dicNumToValue, Converter = new NumToTextConverter() });
                // 清扣厂家
                this.twt13.dicNumToValue = new Dictionary<int, string>();
                this.twt13.dicNumToValue.Add(0, "无");
                this.twt13.dicNumToValue.Add(1, "成创带关门");
                this.twt13.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["ClearBtnType"], Mode = BindingMode.OneWay, ConverterParameter = this.twt13.dicNumToValue, Converter = new NumToTextConverter() });
                // 防喷盒厂家
                this.twt14.dicNumToValue = new Dictionary<int, string>();
                this.twt14.dicNumToValue.Add(0, "无");
                this.twt14.dicNumToValue.Add(1, "有");
                this.twt14.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["PreventBoxType"], Mode = BindingMode.OneWay, ConverterParameter = this.twt14.dicNumToValue, Converter = new NumToTextConverter() });
                // 大钩厂家
                this.twt15.dicNumToValue = new Dictionary<int, string>();
                this.twt15.dicNumToValue.Add(0, "无");
                this.twt15.dicNumToValue.Add(1, "单控制编码器");
                this.twt15.dicNumToValue.Add(2, "双控制编码器");
                this.twt15.dicNumToValue.Add(3, "外部信号");
                this.twt15.dicNumToValue.Add(4, "盛特模拟器");
                this.twt15.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["HookType"], Mode = BindingMode.OneWay, ConverterParameter = this.twt15.dicNumToValue, Converter = new NumToTextConverter() });
                // 顶驱厂家
                this.twt16.dicNumToValue = new Dictionary<int, string>();
                this.twt16.dicNumToValue.Add(0, "无");
                this.twt16.dicNumToValue.Add(1, "有");
                this.twt16.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["TopType"], Mode = BindingMode.OneWay, ConverterParameter = this.twt16.dicNumToValue, Converter = new NumToTextConverter() });
                // 吊卡自动打开关闭
                this.twt17.dicNumToValue = new Dictionary<int, string>();
                this.twt17.dicNumToValue.Add(1, "关闭");
                this.twt17.dicNumToValue.Add(2, "开启");
                this.twt17.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["589b0"], Mode = BindingMode.OneWay, ConverterParameter = this.twt16.dicNumToValue, Converter = new BoolToTextConverter() });
                // 吊卡自动关闭
                this.twt18.dicNumToValue = new Dictionary<int, string>();
                this.twt18.dicNumToValue.Add(1, "关闭");
                this.twt18.dicNumToValue.Add(2, "开启");
                this.twt18.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["589b1"], Mode = BindingMode.OneWay, ConverterParameter = this.twt16.dicNumToValue, Converter = new BoolToTextConverter() });
                // 防喷盒自动打开
                this.twt19.dicNumToValue = new Dictionary<int, string>();
                this.twt19.dicNumToValue.Add(1, "关闭");
                this.twt19.dicNumToValue.Add(2, "开启");
                this.twt19.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["589b2"], Mode = BindingMode.OneWay, ConverterParameter = this.twt16.dicNumToValue, Converter = new BoolToTextConverter() });
                // 对接厂家配置
                this.twt20.dicNumToValue = new Dictionary<int, string>();
                this.twt20.dicNumToValue.Add(0, "宝石");
                this.twt20.dicNumToValue.Add(1, "宏华/胜利");
                this.twt20.SetBinding(TextWithCombox.ShowTxtWithCBProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["IngSetting"], Mode = BindingMode.OneWay, ConverterParameter = this.twt16.dicNumToValue, Converter = new BoolToTextConverter() });
                #endregion
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        private byte[] bConfigParameter = new byte[3];
        private void ParamThreeSet(object sender, RoutedEventArgs e)
        {
            //bConfigParameter = GlobalData.Instance.ConfigParameter;
            //if (bConfigParameter[0] != 0)
            //{
            //    byte[] byteToSend = GlobalData.Instance.SFSendToOpr(new List<byte> { 23, bConfigParameter[0], bConfigParameter[1], bConfigParameter[2] });
            //    GlobalData.Instance.da.SendBytes(byteToSend);
            //}
            if (GlobalData.Instance.SetParam[0] != 0)
            {
                byte[] byteToSend = GlobalData.Instance.SetParam;
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            if (GlobalData.Instance.SetParam[3] == 81)// 铁钻工配置
            {
                int sirType = GlobalData.Instance.SetParam[4];
                string sql = string.Format("update GloConfig Set SIRType='{0}'", sirType);
                DataHelper.Instance.ExecuteNonQuery(sql);
                GlobalData.Instance.da.GloConfig.SIRType = sirType;
            }
            else if (GlobalData.Instance.SetParam[3] == 82)// 猫道配置
            {
                int catType = GlobalData.Instance.SetParam[4];
                string sql = string.Format("update GloConfig Set CatType='{0}'", catType);
                DataHelper.Instance.ExecuteNonQuery(sql);
                GlobalData.Instance.da.GloConfig.CatType = catType;
            }
            else if (GlobalData.Instance.SetParam[3] == 83)// 液压站配置
            {
                int hydType = GlobalData.Instance.SetParam[4];
                string sql = string.Format("update GloConfig Set HydType='{0}'", hydType);
                DataHelper.Instance.ExecuteNonQuery(sql);
                GlobalData.Instance.da.GloConfig.HydType = hydType;
            }
            else if (GlobalData.Instance.SetParam[3] == 84)// 钻台面配置
            {
                int drType = GlobalData.Instance.SetParam[4];
                string sql = string.Format("update GloConfig Set DRType='{0}'", drType);
                DataHelper.Instance.ExecuteNonQuery(sql);
                GlobalData.Instance.da.GloConfig.DRType = drType;
            }
            else if (GlobalData.Instance.SetParam[3] == 85)// 二层台配置
            {
                int sfType = GlobalData.Instance.SetParam[4];
                string sql = string.Format("update GloConfig Set DRType='{0}'", sfType);
                DataHelper.Instance.ExecuteNonQuery(sql);
                GlobalData.Instance.da.GloConfig.SFType = sfType;
            }
        }
    }
}
