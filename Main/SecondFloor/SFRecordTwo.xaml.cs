using COM.Common;
using ControlLibrary;
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

namespace Main.SecondFloor
{
    /// <summary>
    /// SFIOOne.xaml 的交互逻辑
    /// </summary>
    public partial class SFRecordTwo : UserControl
    {
        private static SFRecordTwo _instance = null;
        private static readonly object syncRoot = new object();

        public static SFRecordTwo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFRecordTwo();
                        }
                    }
                }
                return _instance;
            }
        }

        public struct ShowData
        {
            public short Index { get; set; }
            public string Value { get; set; }
            public string TimeStamp { get; set; }
        }

        Dictionary<string, string> startCnfrmLightDic;
        Dictionary<string, string> manipulatorWellHeadSignDic;
        Dictionary<string, string> operationModelDic;
        Dictionary<string, string> workModelDic;
        Dictionary<string, string> drillPipeTypeDic;
        Dictionary<string, string> motorAlarmPromptDic;

        public SFRecordTwo()
        {
            InitializeComponent();
            this.Loaded += SFRecordTwo_Loaded;
        }

        private void SFRecordTwo_Loaded(object sender, RoutedEventArgs e)
        {
            startCnfrmLightDic = SplitStringToDic(GlobalData.Instance.da["101B7b0"].Description);
            manipulatorWellHeadSignDic = SplitStringToDic(GlobalData.Instance.da["101B7b1"].Description);
            operationModelDic = SplitStringToDic(GlobalData.Instance.da["operationModel"].Description);
            workModelDic = SplitStringToDic(GlobalData.Instance.da["workModel"].Description);
            drillPipeTypeDic = SplitStringToDic(GlobalData.Instance.da["drillPipeType"].Description);
            motorAlarmPromptDic = SplitStringToDic(GlobalData.Instance.da["motorAlarmPrompt"].Description);

            QueryMotorDrill();
        }

        public Dictionary<string, string> SplitStringToDic(string str)
        {
            try
            {
                Dictionary<string, string> tempDic = new Dictionary<string, string>();
                string[] sArray = str.Split(new char[2] { ',', '，' });

                if (sArray != null)
                {
                    foreach (var item in sArray)
                    {
                        string[] arrTemp = item.Split(new char[2] { ':', '：' });
                        if (arrTemp != null)
                        {
                            tempDic.Add(arrTemp[0], arrTemp[1]);
                        }
                    }
                }
                else
                {
                    return null;
                }

                return tempDic;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
                return null;
            }
        }
        public void QueryMotorDrill()
        {
            try
            {
                List<ShowData> startCnfrmLightData = new List<ShowData>();
                List<ShowData> manipulatorWellHeadSignData = new List<ShowData>();
                List<ShowData> operationModelData = new List<ShowData>();
                List<ShowData> workModelData = new List<ShowData>();
                List<ShowData> drillPipeTypeData = new List<ShowData>();
                List<ShowData> motorAlarmPromptData = new List<ShowData>();

                short index = 0;
                string sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["101B7b0"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (startCnfrmLightDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = startCnfrmLightDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            startCnfrmLightData.Add(data);
                        }
                    }
                }
                this.StartCnfrmLightData.ItemsSource = startCnfrmLightData;

                index = 0;
                sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["101B7b1"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (manipulatorWellHeadSignDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = manipulatorWellHeadSignDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            manipulatorWellHeadSignData.Add(data);
                        }
                    }
                }
                this.ManipulatorWellHeadSignData.ItemsSource = manipulatorWellHeadSignData;

                index = 0;
                sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["operationModel"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (operationModelDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = operationModelDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            operationModelData.Add(data);
                        }
                    }
                }
                this.OperationModelData.ItemsSource = operationModelData;

                index = 0;
                sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["workModel"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (workModelDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = workModelDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            workModelData.Add(data);
                        }
                    }
                }
                this.WorkModelData.ItemsSource = workModelData;

                index = 0;
                sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["drillPipeType"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (drillPipeTypeDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = drillPipeTypeDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            drillPipeTypeData.Add(data);
                        }
                    }
                }
                this.DrillPipeTypeData.ItemsSource = drillPipeTypeData;

                index = 0;
                sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["motorAlarmPrompt"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (motorAlarmPromptDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = motorAlarmPromptDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            motorAlarmPromptData.Add(data);
                        }
                    }
                }
                this.MotorAlarmPromptData.ItemsSource = motorAlarmPromptData;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
