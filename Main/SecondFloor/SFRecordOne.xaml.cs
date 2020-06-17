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
    public partial class SFRecordOne : UserControl
    {
        private static SFRecordOne _instance = null;
        private static readonly object syncRoot = new object();

        public static SFRecordOne Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SFRecordOne();
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

        Dictionary<string, string> armMotorRetZeroStatusDic;
        Dictionary<string, string> carMotorRetZeroStatusDic;
        Dictionary<string, string> rotateMotorRetZeroStatusDic;
        Dictionary<string, string> armMotorWorkStatusDic;
        Dictionary<string, string> carMotorWorkStatusDic;
        Dictionary<string, string> rotateMotorWorkStatusDic;

        public SFRecordOne()
        {
            InitializeComponent();
            this.Loaded += SFRecordOne_Loaded;
        }

        private void SFRecordOne_Loaded(object sender, RoutedEventArgs e)
        {
            armMotorRetZeroStatusDic = SplitStringToDic(GlobalData.Instance.da["armMotorRetZeroStatus"].Description);
            carMotorRetZeroStatusDic = SplitStringToDic(GlobalData.Instance.da["carMotorRetZeroStatus"].Description);
            rotateMotorRetZeroStatusDic = SplitStringToDic(GlobalData.Instance.da["rotateMotorRetZeroStatus"].Description);

            armMotorWorkStatusDic = SplitStringToDic(GlobalData.Instance.da["armMotorWorkStatus"].Description);
            carMotorWorkStatusDic = SplitStringToDic(GlobalData.Instance.da["carMotorWorkStatus"].Description);
            rotateMotorWorkStatusDic = SplitStringToDic(GlobalData.Instance.da["rotateMotorWorkStatus"].Description);

            QueryMotorRetZeroData();
            QueryMotorWorkData();
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

        private void QueryMotorRetZeroData()
        {
            try
            {
                List<ShowData> armMotorRetData = new List<ShowData>();
                List<ShowData> carMotorRetData = new List<ShowData>();
                List<ShowData> rotateMotorRetData = new List<ShowData>();

                short index = 0;
                string sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["armMotorRetZeroStatus"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (armMotorRetZeroStatusDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = armMotorRetZeroStatusDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            armMotorRetData.Add(data);
                        }
                    }
                }
                this.ArmMotorRetData.ItemsSource = armMotorRetData;

                index = 0;
                sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["carMotorRetZeroStatus"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (carMotorRetZeroStatusDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = carMotorRetZeroStatusDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            carMotorRetData.Add(data);
                        }
                    }
                }
                this.CarMotorRetData.ItemsSource = carMotorRetData;

                index = 0;
                sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["rotateMotorRetZeroStatus"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (rotateMotorRetZeroStatusDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = rotateMotorRetZeroStatusDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            rotateMotorRetData.Add(data);
                        }
                    }
                }
                this.RotateMotorRetData.ItemsSource = rotateMotorRetData;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void QueryMotorWorkData()
        {
            try
            {
                List<ShowData> armMotorWorkData = new List<ShowData>();
                List<ShowData> carMotorWorkData = new List<ShowData>();
                List<ShowData> rotateMotorWorkData = new List<ShowData>();

                short index = 0;
                string sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["armMotorWorkStatus"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (armMotorWorkStatusDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = armMotorWorkStatusDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            armMotorWorkData.Add(data);
                        }
                    }
                }
                this.ArmMotorWorkData.ItemsSource = armMotorWorkData;

                index = 0;
                sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["carMotorWorkStatus"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (carMotorWorkStatusDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = carMotorWorkStatusDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            carMotorWorkData.Add(data);
                        }
                    }
                }
                this.CarMotorWorkData.ItemsSource = carMotorWorkData;

                index = 0;
                sql = string.Format("SELECT TOP 100 Value,TimeStamp FROM Log WHERE Id={0} ORDER BY  TimeStamp DESC;", GlobalData.Instance.da["rotateMotorWorkStatus"].ID);
                using (var reader = DataHelper.Instance.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (rotateMotorWorkStatusDic.ContainsKey(reader.GetFloat(0).ToString()))
                        {
                            index++;
                            ShowData data = new ShowData { Index = index, Value = rotateMotorWorkStatusDic[reader.GetFloat(0).ToString()], TimeStamp = reader.GetDateTime(1).ToString("yyyy/MM/dd HH:mm") };
                            rotateMotorWorkData.Add(data);
                        }
                    }
                }
                this.RotateMotorWorkData.ItemsSource = rotateMotorWorkData;
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
    }
}
