using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ControlLibrary
{
    public struct DataForm
    {
        public bool IsRS { get; set; }
        public string Buffer { get; set; }
        public int Length { get; set; } //字节长度
        public string IPPort { get; set; }// 从哪里接收，或发送至何方
        public DateTime DTime { get; set; }

        public void SetValue(bool rs, string content,string ip,int length)
        {
            IsRS = rs; //false 接收 true 发送
            IPPort = ip;
            Buffer = content;
            Length = length;
            DTime = DateTime.Now;
        }
    }

    public class ToggleButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (parameter == null)
            {
                return value;
            }

            string  content = (string)value;
            int index;

            if (!int.TryParse((string)parameter, out index))
            {
                return value;
            }

            string[] sArray = content.Split(new char[2] { ',', '，' });

            switch (index)
            {
                case 0:
                    return sArray[0];
                case 1:
                    return sArray[1];
                case 2:
                    return sArray[2];
                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
