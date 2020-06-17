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
    public partial class SFRecordThree : UserControl
    {
        private static SFRecordThree _instance = null;
        private static readonly object syncRoot = new object();

        public static SFRecordThree Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFRecordThree();
                        }
                    }
                }
                return _instance;
            }
        }

        Dictionary<string, string> promptInfoDic;
        Dictionary<string, string> gripStatusDic;

        public struct ShowData
        {
            public short Index { get; set; }
            public string Value { get; set; }
            public string TimeStamp { get; set; }
        }

        public SFRecordThree()
        {
            InitializeComponent();
            this.Loaded += SFRecordThree_Loaded;
        }

        private void SFRecordThree_Loaded(object sender, RoutedEventArgs e)
        {
            promptInfoDic = SplitStringToDic(GlobalData.Instance.da["promptInfo"].Description);
            gripStatusDic = SplitStringToDic(GlobalData.Instance.da["gripStatus"].Description);

            QueryGripStatusPromptInfo();
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

        private void QueryGripStatusPromptInfo()
        {
            try
            {
                List<ShowData> promptInfoData = new List<ShowData>();
                List<ShowData> gripStatusData = new List<ShowData>();

                short index = 0;
                string sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["promptInfo"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (promptInfoDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = promptInfoDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            promptInfoData.Add(data);
                        }
                    }
                }
                this.PromptInfoData.ItemsSource = promptInfoData;

                index = 0;
                sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["gripStatus"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (gripStatusDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = gripStatusDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            gripStatusData.Add(data);
                        }
                    }
                }
                this.GripStatusData.ItemsSource = gripStatusData;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
