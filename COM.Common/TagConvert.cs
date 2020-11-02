using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace COM.Common
{
    // SymbolMapping变量
    public class BoolTagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if ((bool)value)
            {
                return 1;
            }

            return 3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // SymbolMapping变量
    public class OppositeBoolTagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if ((bool)value)
            {
                return 3;
            }

            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 从下位机获取回转角度值 da["callAngle"]--15，16位
    /// </summary>
    public class CallAngleConverter : IValueConverter
    {
        //public int RotateSbyteToInt(byte byteIn)
        //{
        //    int intOut = unchecked((sbyte)byteIn);
        //    return intOut;
        //}
        /// <summary>
        ///  da["callAngle"]--15，16位换算成int
        /// </summary>
        /// <param name="shortIn"></param>
        /// <returns></returns>
        public double PositionSbyteToInt(short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            double inOut = unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128;
            return inOut;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }

            //int iTemp = RotateSbyteToInt((byte)value);
            // 传过来的角度要除以10
            double iTemp = PositionSbyteToInt((short)value) / 10.0;
            //因为 角度可能只会到 88 89 ，到不了90，所以作如此处理
            if (iTemp >= 88)
            {
                iTemp = 90;
            }
            else if (iTemp <= -88)
            {
                iTemp = -90;
            }

            return iTemp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻台面回转角度值 
    /// </summary>
    public class DRCallAngleConverter : IValueConverter
    {
        //public int RotateSbyteToInt(byte byteIn)
        //{
        //    int intOut = unchecked((sbyte)byteIn);
        //    return intOut;
        //}
        /// <summary>
        ///  da["callAngle"]--15，16位换算成int
        /// </summary>
        /// <param name="shortIn"></param>
        /// <returns></returns>
        public double PositionSbyteToInt(short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            double inOut = unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128;
            return inOut;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }

            //int iTemp = RotateSbyteToInt((byte)value);
            // 传过来的角度要除以10
            double iTemp = (double)((short)value) / 10.0;
            //因为 角度可能只会到 88 89 ，到不了90，所以作如此处理
            if (iTemp >= 88)
            {
                iTemp = 90;
            }
            else if (iTemp <= -88)
            {
                iTemp = -90;
            }

            return iTemp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TopWarningInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            switch ((byte)value)
            {
                case 0:
                    return "UDP通信失败，请检查IP和端口！";
                case 1:
                    return "";
                case 2:
                    return "工控机与操作台通信断开，请检查连接！";
                case 3:
                    return "二层台与操作台通信断开，请检查连接！";
            }

            return "程序初始化";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 操作模式
    /// </summary>
    public class OperationModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 1:
                    return "急停";
                case 2:
                    return "调试模式";
                case 3:
                    return "回零";
                case 4:
                    return "手动";
                case 5:
                    return "自动";
                case 6:
                    return "回收";
                case 7:
                    return "运输";
                case 8:
                    return "实验";
                case 9:
                    return "补偿模式";
            }

            return "操作模式";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 工作模式
    /// </summary>
    public class WorkModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 1:
                    return "送杆";
                case 2:
                    return "排杆";
            }

            return "工作模式";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻台面-工作模式
    /// </summary>
    public class DRWorkModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 1:
                    return "送杆";
                case 2:
                    return "排杆";
            }

            return "工作模式";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 挡绳
    /// </summary>
    public class RopeModelConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null)
                || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null) || (values[3] == DependencyProperty.UnsetValue) || (values[3] == null))
            {
                return "挡绳";
            }
            bool leftOut = (bool)values[0];
            bool leftIn = (bool)values[1];
            bool RightOut = (bool)values[2];
            bool RightIn = (bool)values[3];
            if (leftOut && RightOut) return "挡绳伸出";
            if (leftIn && RightIn) return "挡绳缩回";

            return "挡绳";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 挡绳
    /// </summary>
    public class RopeModelIsCheckConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null)
                || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null) || (values[3] == DependencyProperty.UnsetValue) || (values[3] == null))
            {
                return false;
            }
            bool leftOut = (bool)values[0];
            bool leftIn = (bool)values[1];
            bool RightOut = (bool)values[2];
            bool RightIn = (bool)values[3];
            if (leftOut && RightOut) return false;
            if (leftIn && RightIn) return true;

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InterLockingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }

            if ((bool)value)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InterLockingOppConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            if ((bool)value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IngInterLockingOppConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }

            if ((bool)value)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 钻台面-安全设置-大钩标定状态
    /// </summary>
    public class HookSetStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return "未标定";
            }

            int status = (byte)value;
            if (status == 1) return "二层台已标定";
            else if (status == 2) return "钻台面已标定";
            else if (status == 3) return "大钩已标定";

            return "未标定";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 工作时间
    /// </summary>
    public class UnitDivide10Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0.0;
            }

            int iTemp = (int)value;

            return (iTemp / 10.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 电机状态提示
    /// </summary>
    public class MotorStatusWarnInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bTemp = (byte)value;

            switch (bTemp)
            {
                case 5:
                    return "正常";
                case 2:
                    return "异常";
                case 3:
                    return "使能";
                case 4:
                    return "打开";
                case 1:
                    return "断电";
                case 6:
                    return "停止";
                case 7:
                    return "错误7";
                case 8:
                    return "错误8";
                case 127:
                    return "错误";

            }

            return "??";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ArrowDirectionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string path = string.Empty;
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) 
                || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null)
                || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null)
                || (values[3] == DependencyProperty.UnsetValue) || (values[3] == null))
            {
                path = "/Images/arrow.png";
            }

            bool up = (bool)values[0];
            bool down = (bool)values[1];
            bool left = (bool)values[2];
            bool right = (bool)values[3];

            if (up) path = "/Images/arrowUp.png";
            else if (down) path = "/Images/arrowDown.png";
            else if (left) path = "/Images/arrowLeft.png";
            else if(right) path = "/Images/arrowRight.png";
            else path = "/Images/arrow.png";
            var uriSource = new Uri(path, UriKind.Relative);
            return new BitmapImage(uriSource);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 反馈参数
    /// </summary>
    public class ReturnSelectParaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bTemp = (byte)value;
            switch (bTemp)
            {
                case 0:
                    return "反馈参数名称";
                case 1:
                    return "最靠近井口位置";
                case 2:
                    return "最远离井口位置";
                case 4:
                    return "左1#指梁位置";
                case 3:
                    return "右1#指梁位置";
                //case 3:
                //    return "右1#指梁位置";
                //case 4:
                //    return "左1#指梁位置";
                case 6:
                    return "小车排杆待机位置";
                case 7:
                    return "小车回收位置";
                case 10:
                    return "小车运输位置";
                case 21:
                    return "右16#指梁位置补偿";
                case 22:
                    return "左16#指梁位置补偿";
                case 23:
                    return "右1#特殊指梁位置";
                case 24:
                    return "左1#特殊指梁位置";
                case 33:
                    return "手臂最小缩回位置";
                case 34:
                    return "钻杆指梁手臂最大伸展位置";
                case 39:
                    return "手臂回收位置";
                case 40:
                    return "手臂井口最大伸展位置";
                case 42:
                    return "手臂运输位置";
                case 43:
                    return "钻铤指梁手臂最大位置";
                case 49:
                    return "左1#钻铤手臂伸展位置";
                case 50:
                    return "左2#钻铤手臂伸展位置";
                case 51:
                    return "左3#钻铤手臂伸展位置";
                case 52:
                    return "左4#钻铤手臂伸展位置";
                case 53:
                    return "左5#钻铤手臂伸展位置";
                case 54:
                    return "左6#钻铤手臂伸展位置";
                case 57:
                    return "右1#钻铤手臂伸展位置";
                case 58:
                    return "右2#钻铤手臂伸展位置";
                case 59:
                    return "右3#钻铤手臂伸展位置";
                case 60:
                    return "右4#钻铤手臂伸展位置";
                case 61:
                    return "右5#钻铤手臂伸展位置";
                case 62:
                    return "右6#钻铤手臂伸展位置";
                case 65:
                    return "回转-90°位置";
                case 66:
                    return "回转90°位置";
                case 71:
                    return "回转回收位置";
                case 72:
                    return "回转井口位置";
                case 74:
                    return "回转运输位置";
                case 97:
                    return "3.5寸档";
                case 110:
                    return "4寸补偿";
                case 112:
                    return "4.5寸补偿";
                case 98:
                    return "5寸档";
                case 114:
                    return "5.5寸补偿";
                case 108:
                    return "5寸7/8";
                case 99:
                    return "6寸";
                case 100:
                    return "6.5寸";
                case 109:
                    return "6寸5/8";
                case 101:
                    return "7寸";
                case 102:
                    return "7.5寸";
                case 103:
                    return "8寸";
                case 104:
                    return "9寸";
                case 105:
                    return "10寸";
                case 106:
                    return "11寸";
                case 107:
                    return "最大尺寸";
                case 129:
                    return "左手指最小值";
                case 130:
                    return "左手指最大值";
                case 131:
                    return "右手指最小值";
                case 132:
                    return "右手指最大值";
                case 133:
                    return "左挡绳最小值";
                case 134:
                    return "左挡绳最大值";
                case 135:
                    return "右挡绳最小值";
                case 136:
                    return "右挡绳最大值";
                case 137:
                    return "猴道伸出";
                case 138:
                    return "猴道缩回";
                case 141:
                    return "左1#钻铤锁最小值";
                case 142:
                    return "左2#钻铤锁最小值";
                case 143:
                    return "左3#钻铤锁最小值";
                case 144:
                    return "左4#钻铤锁最小值";
                case 145:
                    return "左5#钻铤锁最小值";
                case 146:
                    return "左6#钻铤锁最小值";
                case 149:
                    return "右1#钻铤锁最小值";
                case 150:
                    return "右2#钻铤锁最小值";
                case 151:
                    return "右3#钻铤锁最小值";
                case 152:
                    return "右4#钻铤锁最小值";
                case 165:
                    return "右5#钻铤锁最小值";
                case 166:
                    return "右6#钻铤锁最小值";
                case 153:
                    return "左1#钻铤锁最大值";
                case 154:
                    return "左2#钻铤锁最大值";
                case 155:
                    return "左3#钻铤锁最大值";
                case 156:
                    return "左4#钻铤锁最大值";
                case 157:
                    return "左5#钻铤锁最大值";
                case 158:
                    return "左6#钻铤锁最大值";
                case 161:
                    return "右1#钻铤锁最大值";
                case 162:
                    return "右2#钻铤锁最大值";
                case 163:
                    return "右3#钻铤锁最大值";
                case 164:
                    return "右4#钻铤锁最大值";
                case 173:
                    return "右5#钻铤锁最大值";
                case 174:
                    return "右6#钻铤锁最大值";
                default:
                    return "反馈参数名称";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 设备编码绑定-73，75-78
    /// </summary>
    public class DeviceEncodeMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null) || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return "";
            }
            string year = string.Empty;
            if (values[0].ToString().Length == 1) year = "0" + values[0].ToString();
            else year = values[0].ToString();
            int deviceModel = int.Parse(values[1].ToString());
            string dModel = string.Empty;
            if (deviceModel >= 2700) dModel = "0280";
            else dModel = "0230";
            string encode = string.Empty;
            if (values[2].ToString().Length < 4)
            {
                int tmpLen = 4 - values[2].ToString().Length;
                for (int i = 0; i < tmpLen; i++)
                {
                    encode += "0";
                }
                encode += values[2].ToString();
            }
            string txt = parameter.ToString();
            string deviceEncode = year + txt + dModel + encode;
            return deviceEncode;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 二层台版本信息绑定
    /// </summary>
    public class SecondVersionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null) || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return "";
            }
            string txt = parameter.ToString();
            return txt + values[0].ToString() + "." + values[1].ToString() + "." + values[2].ToString() + "-C";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 二层台版本年月日绑定-100-103
    /// </summary>
    public class SecondVersionDateMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null) || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return "";
            }
            string year = string.Empty;
            if (values[0].ToString().Length == 1) year = "200" + values[0].ToString();
            else year = "20" + values[0].ToString();
            string month = string.Empty;
            if (values[0].ToString().Length == 1) month = "0" + values[1].ToString();
            else month = values[1].ToString();
            string day = string.Empty;
            if (values[0].ToString().Length == 1) day = "0" + values[2].ToString();
            else day = values[2].ToString();
            return year + "." + month + "." + day;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 操作版本信息绑定
    /// </summary>
    public class OperVersionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null) || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return "";
            }
            return "SDCH2-V" + values[0].ToString() + "." + values[1].ToString() + "." + values[2].ToString() + "-C";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 操作台版本年月日绑定-542-544
    /// </summary>
    public class OperVersionDateMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null) || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return "";
            }
            string year = string.Empty;
            if (values[0].ToString().Length == 1) year = "200" + values[0].ToString();
            else year = "20" + values[0].ToString();
            string month = string.Empty;
            if (values[0].ToString().Length == 1) month = "0" + values[1].ToString();
            else month = values[1].ToString();
            string day = string.Empty;
            if (values[0].ToString().Length == 1) day = "0" + values[2].ToString();
            else day = values[2].ToString();
            return year + "." + month + "." + day;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 档绳
    /// </summary>
    public class RopeMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                return "";
            }
            string retTxt = "未知";
            if (values[0].ToString() == "True") retTxt = "伸出";
            if (values[1].ToString() == "True") retTxt = "缩回";
            return retTxt;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OperationModelIsCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }

            byte bType = (byte)value;

            if (bType == 5)//5 是自动，4 是手动
            {
                return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WorkModelIsCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            byte bType = (byte)value;

            if (bType == 2)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DRWorkModelIsCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            byte bType = (byte)value;

            if (bType == 2)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DrillPipeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 35:
                    return "3.5寸钻杆";
                case 40:
                    return "4寸钻杆";
                case 45:
                    return "4.5寸钻杆";
                case 50:
                    return "5寸钻杆";
                case 55:
                    return "5.5寸钻杆";
                case 60:
                    return "6寸钻铤";
                case 65:
                    return "6.5寸钻铤";
                case 70:
                    return "7寸钻铤";
                case 75:
                    return "7.5寸钻铤";
                case 80:
                    return "8寸钻铤";
                case 90:
                    return "9寸钻铤";
                case 100:
                    return "10寸钻铤";
                case 110:
                    return "11寸钻铤";
            }

            return "未选中管柱类型";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IngDrillPipeTypeConverter : IMultiValueConverter
    {
      

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[0]== DependencyProperty.UnsetValue
                || values[1] == null || values[1] == DependencyProperty.UnsetValue)
            {
                return string.Empty;
            }

            byte bType = (byte)values[0];
            byte bType2 = (byte)values[1];
            if(bType != bType2) return "管柱类型不一致";
            switch (bType)
            {
                case 35:
                    return "3.5寸钻杆";
                case 40:
                    return "4寸钻杆";
                case 45:
                    return "4.5寸钻杆";
                case 50:
                    return "5寸钻杆";
                case 55:
                    return "5.5寸钻杆";
                case 60:
                    return "6寸钻铤";
                case 65:
                    return "6.5寸钻铤";
                case 70:
                    return "7寸钻铤";
                case 75:
                    return "7.5寸钻铤";
                case 80:
                    return "8寸钻铤";
                case 90:
                    return "9寸钻铤";
                case 100:
                    return "10寸钻铤";
                case 110:
                    return "11寸钻铤";
            }

            return "未选中管柱类型";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 钻台面-目的地设置
    /// </summary>
    public class DesTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 1:
                    return "立根区";
                case 2:
                    return "猫道-井口";
                case 3:
                    return "猫道-鼠道";
            }

            return "未设置目的地";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WorkModeFlowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if ((bool)value)
            {
                return "排杆";
            }

            return "送杆";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 排杆/送杆步骤
    /// </summary>
    public class AutoModeNowStepCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string tb = (string)parameter;
            int workModel = 1;
            bool last = false;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                workModel = 1;
            }
            else
            {
                if (values[1].ToString() == "2")
                {
                    workModel = 2;
                }
            }
            int byteAutoModeNowStep = int.Parse(values[2].ToString());
        
            if (tb == "one")
            {
                if (workModel == 1) //送杆
                {
                    if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || values[0].ToString() != "5") // 非自动模式
                    {
                        return "送杆启动";
                    }

                    if (byteAutoModeNowStep == 0) return "送杆启动";
                    if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 10)) return "指梁定位";
                    if ((byteAutoModeNowStep >= 11) && (byteAutoModeNowStep <= 16)) return "指梁抓杆";
                    if ((byteAutoModeNowStep >= 17) && (byteAutoModeNowStep <= 18)) return "指梁锁确认";
                    if ((byteAutoModeNowStep >= 19) && (byteAutoModeNowStep <= 23)) return "井口等待";
                    if ((byteAutoModeNowStep >= 24) && (byteAutoModeNowStep <= 25)) return "井口旋转";
                    if ((byteAutoModeNowStep == 26)) return "吊卡送杆";
                    if ((byteAutoModeNowStep >= 27) && (byteAutoModeNowStep <= 28)) return "吊卡确认";
                    if ((byteAutoModeNowStep >= 24) && (byteAutoModeNowStep <= 34) || (byteAutoModeNowStep == 35)) return "井口位置";
                    if (byteAutoModeNowStep == 35) last = true;
                }
                else if (workModel == 2) // 排杆
                {
                    if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || values[0].ToString() != "5") // 非自动模式
                    {
                        return "排杆启动";
                    }
                    if ((byteAutoModeNowStep == 0)) return "排杆启动";
                    if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 9)) return "井口定位";
                    if ((byteAutoModeNowStep >= 10) && (byteAutoModeNowStep <= 11)) return "井口抓杆";
                    if ((byteAutoModeNowStep >= 10) && (byteAutoModeNowStep <= 16)) return "井口位置";
                    if ((byteAutoModeNowStep == 12)) return "吊卡确认";
                    if ((byteAutoModeNowStep >= 13) && (byteAutoModeNowStep <= 16)) return "井口运动";
                    if ((byteAutoModeNowStep >= 17) && (byteAutoModeNowStep <= 18)) return "指梁定位";
                    if ((byteAutoModeNowStep == 19)) return "指梁锁确认";
                    if ((byteAutoModeNowStep >= 20) && (byteAutoModeNowStep <= 24) || (byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) return "指梁排管";
                    if ((byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) last = true;
                }
            }
            else if (tb == "two")
            {
                if (workModel == 1) //送杆
                {
                    if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || values[0].ToString() != "5") // 非自动模式
                    {
                        return "指梁定位";
                    }
                    if (byteAutoModeNowStep == 0) return "指梁定位";
                    if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 10)) return "指梁抓杆";
                    if ((byteAutoModeNowStep >= 11) && (byteAutoModeNowStep <= 16)) return "指梁锁确认";
                    if ((byteAutoModeNowStep >= 17) && (byteAutoModeNowStep <= 18)) return "井口等待";
                    if ((byteAutoModeNowStep >= 19) && (byteAutoModeNowStep <= 23)) return "井口旋转";
                    if ((byteAutoModeNowStep >= 24) && (byteAutoModeNowStep <= 25)) return "吊卡送杆";
                    if ((byteAutoModeNowStep == 26)) return "吊卡确认";
                    if ((byteAutoModeNowStep >= 27) && (byteAutoModeNowStep <= 28)) return "井口位置";
                    if ((byteAutoModeNowStep >= 29) && (byteAutoModeNowStep <= 34) || (byteAutoModeNowStep == 35)) return "送杆完成";
                    if (byteAutoModeNowStep == 35) last = true;
                }
                else if (workModel == 2) // 排杆
                {
                    if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || values[0].ToString() != "5") // 非自动模式
                    {
                        return "井口定位";
                    }

                    if ((byteAutoModeNowStep == 0)) return "井口定位";
                    if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 9)) return "井口抓杆";
                    if ((byteAutoModeNowStep >= 10) && (byteAutoModeNowStep <= 11)) return "井口位置";
                    if ((byteAutoModeNowStep >= 10) && (byteAutoModeNowStep <= 16)) return "吊卡确认";
                    if ((byteAutoModeNowStep == 12)) return "井口运动";
                    if ((byteAutoModeNowStep >= 13) && (byteAutoModeNowStep <= 16)) return "指梁定位";
                    if ((byteAutoModeNowStep >= 17) && (byteAutoModeNowStep <= 18)) return "指梁锁确认";
                    if ((byteAutoModeNowStep == 19)) return "指梁排管";
                    if ((byteAutoModeNowStep >= 20) && (byteAutoModeNowStep <= 24) || (byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) return "排杆完成";
                    if ((byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) last = true;
                }
            }
            //// 最后一步 tbone改变字体颜色
            //if (tb == "foreColorOne" && last)
            //{
            //    return "Black";
            //}
            //else if (tb == "foreColorOne")
            //{
            //    return "White";
            //}
            //// 最后一步 bbone改变背景颜色
            //if (tb == "backColorOne" && last)
            //{
            //    return "#C1C1C1";
            //}
            //else if (tb == "foreColorOne")
            //{
            //    return "#1F7AFF";
            //}
            //// 最后一步 tbtwo改变字体颜色
            //if (tb == "foreColorTwo" && last)
            //{
            //    return "White";
            //}
            //else if (tb == "foreColorTwo")
            //{
            //    return "Black";
            //}
            //// 最后一步 bdTwo改变背景颜色
            //if (tb == "backColorTwo" && last)
            //{
            //    return "#1F7AFF";
            //}
            //else if (tb == "foreColorTwo")
            //{
            //    return "#C1C1C1";
            //}

            return "未知";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 排杆/送杆步骤
    /// </summary>
    public class AutoModeForeColorCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bc = new BrushConverter();
            string tb = (string)parameter;
            int workModel = 1;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                workModel = 1;
            }
            else
            {
                if (values[1].ToString() == "2")
                {
                    workModel = 2;
                }
            }
            int byteAutoModeNowStep = int.Parse(values[2].ToString());
            bool last = false;
            if (workModel == 1) //送杆
            {
                if (byteAutoModeNowStep == 35) last = true;
            }
            else if (workModel == 2) // 排杆
            {
                if ((byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) last = true;
            }

            // 最后一步 tbone改变字体颜色
            if (tb == "foreColorOne" && last)
            {
                return (Brush)bc.ConvertFrom("#000000");
            }
            else if (tb == "foreColorOne")
            {
                return (Brush)bc.ConvertFrom("#FFFFFF");
            }
            // 最后一步 tbtwo改变字体颜色
            if (tb == "foreColorTwo" && last)
            {
                return (Brush)bc.ConvertFrom("#FFFFFF");
            }
            else if (tb == "foreColorTwo")
            {
                return (Brush)bc.ConvertFrom("#000000");
            }

            return (Brush)bc.ConvertFrom("#000000");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 排杆/送杆步骤
    /// </summary>
    public class AutoModeBackColorCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bc = new BrushConverter();
            string tb = (string)parameter;
            int workModel = 1;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                workModel = 1;
            }
            else
            {
                if (values[1].ToString() == "2")
                {
                    workModel = 2;
                }
            }
            int byteAutoModeNowStep = int.Parse(values[2].ToString());
            bool last = false;
            if (workModel == 1) //送杆
            {
                if (byteAutoModeNowStep == 35) last = true;
            }
            else if (workModel == 2) // 排杆
            {
                if ((byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) last = true;
            }
            // 最后一步 bbone改变背景颜色
            if (tb == "backColorOne" && last)
            {
                return (Brush)bc.ConvertFrom("#C1C1C1");
            }
            else if (tb == "backColorOne")
            {
                return (Brush)bc.ConvertFrom("#1F7AFF");
            }
            // 最后一步 bdTwo改变背景颜色
            if (tb == "backColorTwo" && last)
            {
                return (Brush)bc.ConvertFrom("#1F7AFF");
            }
            else if (tb == "backColorTwo")
            {
                return (Brush)bc.ConvertFrom("#C1C1C1");
            }

            return (Brush)bc.ConvertFrom("#C1C1C1");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 自动模式提示
    /// </summary>
    public class AutoModeTipVisCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bc = new BrushConverter();
            int workModel = 1;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                workModel = 1;
            }
            else
            {
                if (values[1].ToString() == "2")
                {
                    workModel = 2;
                }
            }
            int byteAutoModeNowStep = int.Parse(values[2].ToString());
            if (workModel == 1) //送杆
            {
                if ((byteAutoModeNowStep >= 24) && (byteAutoModeNowStep <= 34))
                    return "井口位置";
            }
            else if (workModel == 2) // 排杆
            {
                if ((byteAutoModeNowStep >= 10) && (byteAutoModeNowStep <= 16))
                    return "井口位置";
            }

            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ElevatorStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = null;

            if ((value == DependencyProperty.UnsetValue) || (value == null))
            {
                path = "/Images/DK_1.png";
            }
            else
            {
                switch ((int)((byte)value))
                {
                    case 1:
                        path = "/Images/DK_1.png";
                        break;
                    case 2:
                        path = "/Images/DK_3.png";
                        break;
                    case 3:
                        path = "/Images/DK_2.png";
                        break;
                    case 4:
                        path = "/Images/DK_4.png";
                        break;
                    default:
                        path = "/Images/DK_1.png";
                        break;
                }
            }
            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 1.PCFingerBeamNumberFeedBack 点击指梁后 返回的指梁号
    /// 2.Name 箭头本身的名字
    /// 3.OperationModel 操作模式 
    /// </summary>
    public class AnimationFingerBeamNumberSelectCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string path = null;

            byte feedBackFingerBeamNumber;
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                feedBackFingerBeamNumber = 0;
                return "";
            }
            else
            {
                feedBackFingerBeamNumber = (byte)values[0];
            }

            string strImageName;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                strImageName = "";
            }
            else
            {
                strImageName = (string)(values[1]);
            }

            byte bOperationModel;
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                bOperationModel = 0;
            }
            else
            {
                bOperationModel = (byte)(values[2]);
            }

            Regex regex = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            Match match = regex.Match(strImageName);
            byte imageFingerBeamNumber;
            if (match.Success && (bOperationModel == 5 || bOperationModel == 3)) // 只有在回零 和 自动模式下才 生效
            {
                imageFingerBeamNumber = byte.Parse(match.Groups[1].Value);

                if (imageFingerBeamNumber != feedBackFingerBeamNumber)
                {
                    path = "./Images/A_5.png";
                }
                else
                {
                    if (feedBackFingerBeamNumber >= 17 && feedBackFingerBeamNumber <= 32)
                    {
                        path = "/Images/A_Y_3.png";
                    }
                    else if (feedBackFingerBeamNumber >= 1 && feedBackFingerBeamNumber <= 16)
                    {
                        path = "/Images/A_Y_1.png";
                    }
                    else
                    {
                        path = "/Images/A_5.png";
                    }
                    //switch (feedBackFingerBeamNumber)
                    //{
                    //    case 32:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 17:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 18:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 19:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 20:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 21:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 22:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 23:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 24:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 25:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 26:
                    //        path = "/Images/A_Y_3.png";
                    //        break;
                    //    case 16:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    case 1:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    case 2:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    case 3:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    case 4:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    case 5:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    case 6:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    case 7:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    case 8:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    case 9:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    case 10:
                    //        path = "/Images/A_Y_1.png";
                    //        break;
                    //    default:
                    //        path = "/Images/A_5.png";
                    //        break;
                    //}
                }
            }
            else
            {
                path = "/Images/A_5.png";
            }
            var uriSource = new Uri(path, UriKind.Relative);
            return new BitmapImage(uriSource);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 小车在轨道上面的 Y 轴的移动函数
    /// 1 row1的 高度
    /// 2 有多少行row
    /// 3 轨道中间地方的高度
    /// 4 小车的位置
    /// 5 设备型号
    /// </summary>
    public class AnimationCarTranslateTransformYCoverter : IMultiValueConverter
    {
        /// <summary>
        /// 小车位置转换函数，低字节 + 高字节 * 128
        /// </summary>
        /// <param name="shortIn"></param>
        /// <returns></returns>
        public int PositionSbyteToInt(short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            int inOut = unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128;
            return inOut;
        }

        public double GetCarMaxPostion(short shortIn)
        {
            if (shortIn >= 2500 && shortIn < 2700)
            {
                return (double)1985;
            }
            else if (shortIn >= 2700 && shortIn < 2750)
            {
                return (double)2170;
            }
            else if (shortIn >= 2750 && shortIn < 2800)
            {
                return (double)2400;
            }
            else if (shortIn >= 2800 && shortIn < 2900)
            {
                return (double)2400;
            }
            else if (shortIn >= 2900)
            {
                return (double)2400;
            }
            return (double)1985;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double tubeActualHeight;// 小车最大Y位移
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                return 0.0;
            }
            else
            {
                tubeActualHeight = (double)(values[0]);
            }

            double heightToMiddle;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                return 0.0;
            }
            else
            {
                heightToMiddle = (double)(values[1]);
            }

            double carPosition;
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return 0.0;
            }
            else
            {
                //carPosition = (double)((short)(values[3]));
                carPosition = (double)(PositionSbyteToInt((short)(values[2])));
            }
            double NowPosistion = -((carPosition - GlobalData.Instance.CarMinPosistion) / (GlobalData.Instance.CarMaxPosistion - GlobalData.Instance.CarMinPosistion) * tubeActualHeight) + heightToMiddle;

            //return (carPosition / GlobalData.Instance.CarMaxPosistion * ANIMATIONYMAXPOSITION - heightToMiddle + tubeActualHeight * 0.5 -2 + GlobalData.Instance.CompensateY);
            return NowPosistion;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻台面-小车在轨道上面的 Y 轴的移动函数
    /// </summary>
    public class DRAnimationCarTranslateTransformYCoverter : IMultiValueConverter
    {
        /// <summary>
        /// 小车位置转换函数，低字节 + 高字节 * 128
        /// </summary>
        /// <param name="shortIn"></param>
        /// <returns></returns>
        public int PositionSbyteToInt(short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            int inOut = unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128;
            return inOut;
        }


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double tubeActualHeight;// 小车最大Y位移
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                return 0.0;
            }
            else
            {
                tubeActualHeight = (double)(values[0]);
            }

            double heightToMiddle;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                return 0.0;
            }
            else
            {
                heightToMiddle = (double)(values[1]);
            }

            double carPosition;
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return 0.0;
            }
            else
            {
                //carPosition = (double)(PositionSbyteToInt((short)(values[2])));
                carPosition = (double)(((short)(values[2])));
            }
            double NowPosistion = -((carPosition - GlobalData.Instance.DRCarMinPosistion) / (GlobalData.Instance.DRCarMaxPosistion - GlobalData.Instance.DRCarMinPosistion) * tubeActualHeight) + heightToMiddle;

            return NowPosistion;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>   
    /// //抓手的18种状态
    /// </summary>
    public class AnimationArmImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = null;

            if ((value == DependencyProperty.UnsetValue) || (value == null))
            {
                path = "Images/Zhs_1.png";
            }
            else
            {
                switch ((int)((byte)value))
                {
                    case 1:
                        path = "Images/Zhs_1.png";
                        break;
                    case 2:
                        path = "Images/Zhs_2.png";
                        break;
                    case 3:
                        path = "Images/Zhs_3.png";
                        break;
                    case 4:
                        path = "Images/Zhs_4.png";
                        break;
                    case 5:
                        path = "Images/Zhs_5.png";
                        break;
                    case 6:
                        path = "Images/Zhs_6.png";
                        break;
                    case 7:
                        path = "Images/Zhs_7.png";
                        break;
                    case 8:
                        path = "Images/Zhs_8.png";
                        break;
                    case 9:
                        path = "Images/Zhs_9.png";
                        break;
                    case 10:
                        path = "Images/Zhs_10.png";
                        break;
                    case 11:
                        path = "Images/Zhs_11.png";
                        break;
                    case 12:
                        path = "Images/Zhs_12.png";
                        break;
                    case 13:
                        path = "Images/Zhs_13.png";
                        break;
                    case 14:
                        path = "Images/Zhs_14.png";
                        break;
                    case 15:
                        path = "Images/Zhs_15.png";
                        break;
                    case 16:
                        path = "Images/Zhs_16.png";
                        break;
                    case 17:
                        path = "Images/Zhs_17.png";
                        break;
                    case 18:
                        path = "Images/Zhs_18.png";
                        break;
                    default:
                        path = "Images/Zhs_1.png";
                        break;
                }
            }
            return path;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 就是把回转角度转换成单个字节的；
    /// </summary>
    public class AnimationArmAngleRotateConverter : IValueConverter
    {
        public int RotateSbyteToInt(short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            int inOut = unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128;
            return inOut;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            double rotateAngle;
            if ((value == DependencyProperty.UnsetValue) || (value == null))
            {
                return 0.0;
            }
            else
            {
                //armPosition = (double)((short)(values[2]));
                // UDP协议需要除10
                rotateAngle = double.Parse(value.ToString());
            }
            return rotateAngle;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 1.WorkAnimationWidth 其实就是移动的半边距离
    /// 2.RobotArmRotateAngle 手臂旋转的角度   
    /// 3.RobotArmPosition 手臂当前的位置
    /// 4.EquipmentModel 设备的型号 和 小车的轨道和 最长伸展的手臂有关
    /// </summary>
    public class AnimationArmTranslateTransformXCoverter : IMultiValueConverter
    {
        /// <summary>
        /// 手臂的实际位置
        /// </summary>
        /// <param name="shortIn"></param>
        /// <returns></returns>
        public int PositionSbyteToInt(short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            int intOut = unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128;
            return intOut;
        }

        /// <summary>
        /// 回转点击 的角度
        /// </summary>
        /// <param name="byteIn"></param>
        /// <returns></returns>
        public int RotateSbyteToInt(byte byteIn)
        {
            int intOut = unchecked((sbyte)byteIn);
            return intOut;
        }

        /// <summary>
        /// 手臂的最大长度
        /// </summary>
        /// <param name="shortIn"></param>
        /// <returns></returns>
        public double GetArmMaxPostion(short shortIn)
        {
            if (shortIn >= 2500 && shortIn < 2700)
            {
                return (double)2670;
            }
            else if (shortIn >= 2700 && shortIn < 2750)
            {
                return (double)3120;
            }
            else if (shortIn >= 2750 && shortIn < 2800)
            {
                return (double)2750;
            }
            else if (shortIn >= 2800 && shortIn < 2900)
            {
                return (double)3480;
            }
            else if (shortIn >= 2900)
            {
                return (double)3660;
            }
            return (double)2670;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double ANIMATIONXMAXPOSITION;//在界面上面能够伸展的最长距离
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                return 0.0;
            }
            else
            {
                ANIMATIONXMAXPOSITION = (double)(values[0]);
            }

            double rotateAngle;//回转电机的角度
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                return 0.0;
            }
            else
            {
                //armPosition = (double)((short)(values[2]));
                //rotateAngle = (double)(RotateSbyteToInt((byte)(values[1])));
                // UDP协议改后需要除10；
                rotateAngle = double.Parse(values[1].ToString());
            }



            double armPosition;//手臂的实际位置
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return 0.0;
            }
            else
            {
                //armPosition = (double)((short)(values[2]));
                armPosition = (double)(PositionSbyteToInt((short)(values[2])));
            }
            double armXPosition = -((armPosition / GlobalData.Instance.ArmMaxPosistion * ANIMATIONXMAXPOSITION) * Math.Sin(rotateAngle * Math.PI / 180));
            return armXPosition;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 1.WorkAnimationWidth 其实就是移动的半边距离
    /// 2.RobotArmRotateAngle 手臂旋转的角度   
    /// 3.RobotArmPosition 手臂当前的位置
    /// 4.EquipmentModel 设备的型号 和 小车的轨道和 最长伸展的手臂有关
    /// </summary>
    public class DRAnimationArmTranslateTransformXCoverter : IMultiValueConverter
    {
        /// <summary>
        /// 手臂的实际位置
        /// </summary>
        /// <param name="shortIn"></param>
        /// <returns></returns>
        public int PositionSbyteToInt(short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            int intOut = unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128;
            return intOut;
        }

        /// <summary>
        /// 回转点击 的角度
        /// </summary>
        /// <param name="byteIn"></param>
        /// <returns></returns>
        public int RotateSbyteToInt(byte byteIn)
        {
            int intOut = unchecked((sbyte)byteIn);
            return intOut;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double ANIMATIONXMAXPOSITION;//在界面上面能够伸展的最长距离
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                return 0.0;
            }
            else
            {
                ANIMATIONXMAXPOSITION = (double)(values[0]);
            }

            double rotateAngle;//回转电机的角度
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                return 0.0;
            }
            else
            {
                // UDP协议改后需要除10；
                rotateAngle = double.Parse(values[1].ToString());
            }

            double armPosition;//手臂的实际位置
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return 0.0;
            }
            else
            {
                //armPosition = (double)((short)(values[2]));
                //armPosition = (double)(PositionSbyteToInt((short)(values[2])));
                armPosition = (double)(((short)(values[2])));
            }
            double armXPosition = -((armPosition / GlobalData.Instance.DRArmMaxPosistion * ANIMATIONXMAXPOSITION) * Math.Sin(rotateAngle * Math.PI / 180));
            return armXPosition;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 1.ActualHeight  row1 的高度
    /// 2.TubeRows  真实的行数
    /// 3.MiddleHeight 中间位置的高度
    /// 4.RobotCarPosition 小车的实际位置
    /// 5.WorkAnimationWidth 其实就是移动的半边距离,在界面上的长度
    /// 6.RobotArmRotateAngle 手臂旋转的角度
    /// 7.RobotArmPosition 手臂当前的位置
    /// 8.EquipmentModel 设备的型号 和 小车的轨道和 最长伸展的手臂有关
    /// </summary>
    public class AnimationArmTranslateTransformYCoverter : IMultiValueConverter
    {
        public int PositionSbyteToInt(short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            int inOut = unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128;
            return inOut;
        }
        public int RotateSbyteToInt(byte byteIn)
        {
            int intOut = unchecked((sbyte)byteIn);
            return intOut;
        }

        public double GetArmMaxPostion(short shortIn)
        {
            if (shortIn >= 2500 && shortIn < 2700)
            {
                return (double)2670;
            }
            else if (shortIn >= 2700 && shortIn < 2750)
            {
                return (double)3120;
            }
            else if (shortIn >= 2750 && shortIn < 2800)
            {
                return (double)2750;
            }
            else if (shortIn >= 2800 && shortIn < 2900)
            {
                return (double)3480;
            }
            else if (shortIn >= 2900)
            {
                return (double)3660;
            }
            return (double)2670;
        }

        public double GetCarMaxPostion(short shortIn)
        {
            if (shortIn >= 2500 && shortIn < 2700)
            {
                return (double)1985;
            }
            else if (shortIn >= 2700 && shortIn < 2750)
            {
                return (double)2170;
            }
            else if (shortIn >= 2750 && shortIn < 2800)
            {
                return (double)2400;
            }
            else if (shortIn >= 2800 && shortIn < 2900)
            {
                return (double)2400;
            }
            else if (shortIn >= 2900)
            {
                return (double)2400;
            }
            return (double)1985;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 小车最大位移
            double tubeActualHeight;
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                return 0.0;
            }
            else
            {
                tubeActualHeight = (double)(values[0]);
            }

            //中间点的Y坐标
            double heightToMiddle;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                return 0.0;
            }
            else
            {
                heightToMiddle = (double)(values[1]);
            }

            //小车的实际位置
            double carPosition;
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return 0.0;
            }
            else
            {
                //carPosition = (double)((short)(values[3]));
                carPosition = (double)(PositionSbyteToInt((short)(values[2])));
            }


            //界面能伸长的最大X坐标
            double tubeActualWidth;
            if ((values[3] == DependencyProperty.UnsetValue) || (values[3] == null))
            {
                return 0.0;
            }
            else
            {
                tubeActualWidth = (double)(values[3]);
            }

            //旋转的角度值
            double rotateAngle;
            if ((values[4] == DependencyProperty.UnsetValue) || (values[4] == null))
            {
                return 0.0;
            }
            else
            {
                rotateAngle = double.Parse(values[4].ToString());
            }

            //手臂所在的位置
            double armPosition;
            if ((values[5] == DependencyProperty.UnsetValue) || (values[5] == null))
            {
                return 0.0;
            }
            else
            {
                armPosition = (double)(PositionSbyteToInt((short)(values[5])));
            }
            double armPos = ((armPosition / GlobalData.Instance.ArmMaxPosistion * tubeActualWidth / 2.5) * Math.Cos(rotateAngle * Math.PI / 180) + (-((carPosition - GlobalData.Instance.CarMinPosistion) / (GlobalData.Instance.CarMaxPosistion - GlobalData.Instance.CarMinPosistion) * tubeActualHeight) + heightToMiddle));

            return armPos;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻台面-手臂Y轴移动
    /// </summary>
    public class DRAnimationArmTranslateTransformYCoverter : IMultiValueConverter
    {
        public int PositionSbyteToInt(short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            int inOut = unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128;
            return inOut;
        }
        public int RotateSbyteToInt(byte byteIn)
        {
            int intOut = unchecked((sbyte)byteIn);
            return intOut;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 小车最大位移
            double tubeActualHeight;
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                return 0.0;
            }
            else
            {
                tubeActualHeight = (double)(values[0]);
            }

            //中间点的Y坐标
            double heightToMiddle;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                return 0.0;
            }
            else
            {
                heightToMiddle = (double)(values[1]);
            }

            //小车的实际位置
            double carPosition;
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return 0.0;
            }
            else
            {
                //carPosition = (double)((short)(values[3]));
                //carPosition = (double)(PositionSbyteToInt((short)(values[2])));
                carPosition = (double)(((short)(values[2])));
            }


            //界面能伸长的最大X坐标
            double tubeActualWidth;
            if ((values[3] == DependencyProperty.UnsetValue) || (values[3] == null))
            {
                return 0.0;
            }
            else
            {
                tubeActualWidth = (double)(values[3]);
            }

            //旋转的角度值
            double rotateAngle;
            if ((values[4] == DependencyProperty.UnsetValue) || (values[4] == null))
            {
                return 0.0;
            }
            else
            {
                rotateAngle = double.Parse(values[4].ToString());
            }

            //手臂所在的位置
            double armPosition;
            if ((values[5] == DependencyProperty.UnsetValue) || (values[5] == null))
            {
                return 0.0;
            }
            else
            {
                armPosition = (double)(((short)(values[5])));
            }
            double armPos = ((armPosition / GlobalData.Instance.DRArmMaxPosistion * tubeActualWidth/2.5) * Math.Cos(rotateAngle * Math.PI / 180) + (-((carPosition - GlobalData.Instance.DRCarMinPosistion) / (GlobalData.Instance.DRCarMaxPosistion - GlobalData.Instance.DRCarMinPosistion) * tubeActualHeight) + heightToMiddle));

            return armPos;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 整个就是算出旋转的手臂的中心点，仅此而已，其实还有更简单的办法
    /// </summary>
    public class AnimationArmRotateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 0;
            }

            return ((double)value) / 2;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 1 CurrentPointFingerBeamNumber，当前点击的指梁号
    /// 2 Name 绑定的是 border 自己的 name
    /// 3 OperationModel 操作模式
    /// </summary>
    public class AnimationCurrentFingerBeamThicknessCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            Thickness myThickness = new Thickness(0, 0, 0, 0);

            byte feedBackFingerBeamNumber;
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                feedBackFingerBeamNumber = 0;
                return myThickness;
            }
            else
            {
                feedBackFingerBeamNumber = (byte)values[0];
            }

            string strImageName;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                strImageName = "";
            }
            else
            {
                strImageName = (string)(values[1]);
            }

            byte bOperationModel;
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                bOperationModel = 0;
            }
            else
            {
                bOperationModel = (byte)(values[2]);
            }

            Regex regex = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);//后面必须是以数字结尾的
            Match match = regex.Match(strImageName);
            byte imageFingerBeamNumber;
            imageFingerBeamNumber = byte.Parse(match.Groups[1].Value);
            if (match.Success && (bOperationModel == 9 || bOperationModel == 4))//修改为==9    只有在补偿模式下面才有效
            {

                if (imageFingerBeamNumber != feedBackFingerBeamNumber)
                {
                    if (imageFingerBeamNumber < 17) myThickness = new Thickness(2, 0, 0, 0);
                    else myThickness = new Thickness(0, 0, 2, 0);
                }
                else
                {
                    myThickness = new Thickness(0, 0, 0, 0);
                }
            }
            else
            {
                if (imageFingerBeamNumber < 17) myThickness = new Thickness(2, 0, 0, 0);
                else myThickness = new Thickness(0, 0, 2, 0);
            }

            return myThickness;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 1 CurrentPointFingerBeamNumber，当前点击的指梁号
    /// 2 Name 绑定的是 border 自己的 name
    /// 3 OperationModel 操作模式
    /// </summary>
    public class AnimationCurrentFingerBeamVisableCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            //Thickness myThickness = new Thickness(0, 0, 0, 0);

            byte feedBackFingerBeamNumber;
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                feedBackFingerBeamNumber = 0;
                return Visibility.Visible;
            }
            else
            {
                feedBackFingerBeamNumber = (byte)values[0];
            }

            string strImageName;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                strImageName = "";
            }
            else
            {
                strImageName = (string)(values[1]);
            }

            byte bOperationModel;
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                bOperationModel = 0;
            }
            else
            {
                bOperationModel = (byte)(values[2]);
            }

            Regex regex = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);//后面必须是以数字结尾的
            Match match = regex.Match(strImageName);
            byte imageFingerBeamNumber;
            imageFingerBeamNumber = byte.Parse(match.Groups[1].Value);
            if (match.Success && (bOperationModel == 9 || bOperationModel == 4))//修改为==9    只有在补偿模式下面才有效
            {

                if (imageFingerBeamNumber != feedBackFingerBeamNumber)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 小车位置
    /// </summary>
    public class CarPosCoverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value == DependencyProperty.UnsetValue) || (value == null))
            {
                return 0;
            }
            return ((short)value).ShortToInt();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 手臂位置
    /// </summary>
    public class ArmPosCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value == DependencyProperty.UnsetValue) || (value == null))
            {
                return 0;
            }
            double val = ((short)value).ShortToInt();
            return val;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 左钻杆/钻铤统计
    /// </summary>
    public class LeftDrillCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int drOne = 0, drTwo = 0;
            for (int i = 1; i < values.Count(); i++)
            {
                if (values[i] == null || values[i] == DependencyProperty.UnsetValue)
                {
                    drOne += 0;
                }
                else
                {
                    drOne += (byte)values[i];
                }
            }
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue)
            {
                drTwo = 0;
            }
            else
            {
                drTwo += (byte)values[0];
            }
            string txt = parameter.ToString();
            return txt + drOne.ToString() + " + " + drTwo.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 右钻杆/钻铤统计
    /// </summary>
    public class RightDrillCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int drOne = 0, drTwo = 0;
            for (int i = 1; i < values.Count(); i++)
            {
                if (values[i] == null || values[i] == DependencyProperty.UnsetValue)
                {
                    drOne += 0;
                }
                else
                {
                    drOne += (byte)values[i];
                }
            }
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue)
            {
                drTwo = 0;
            }
            else
            {
                drTwo += (byte)values[0];
            }
            return "R:" + drOne.ToString() + " + " + drTwo.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 二层台是否显示
    /// </summary>
    public class SecondFloorVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((value == DependencyProperty.UnsetValue) || (value == null))
            {
                return Visibility.Collapsed;
            }
            if (value.ToString() == "SecondFloor")
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻台台是否显示
    /// </summary>
    public class DRVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((value == DependencyProperty.UnsetValue) || (value == null))
            {
                return Visibility.Collapsed;
            }
            if (value.ToString() == "DrillFloor")
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 排杆/送杆步骤
    /// </summary>
    public class IngNowStepCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string tb = (string)parameter;
            int workModel = 1;
            bool last = false;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                workModel = 1;
            }
            else
            {
                if (values[1].ToString() == "2")
                {
                    workModel = 2;
                }
            }
            int byteAutoModeNowStep = int.Parse(values[2].ToString());
     
            if (workModel == 1) //送杆
            {
                if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || values[0].ToString() != "5") // 非自动模式
                {
                    return "送杆启动";
                }

                if (byteAutoModeNowStep == 0) return "送杆启动";
                if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 10)) return "指梁定位";
                if ((byteAutoModeNowStep >= 11) && (byteAutoModeNowStep <= 16)) return "指梁抓杆";
                if ((byteAutoModeNowStep >= 17) && (byteAutoModeNowStep <= 18)) return "指梁锁确认";
                if ((byteAutoModeNowStep >= 19) && (byteAutoModeNowStep <= 23)) return "井口等待";
                if ((byteAutoModeNowStep >= 24) && (byteAutoModeNowStep <= 25)) return "井口旋转";
                if ((byteAutoModeNowStep == 26)) return "吊卡送杆";
                if ((byteAutoModeNowStep >= 27) && (byteAutoModeNowStep <= 28)) return "吊卡确认";
                if ((byteAutoModeNowStep >= 24) && (byteAutoModeNowStep <= 34) || (byteAutoModeNowStep == 35)) return "井口位置";
                if (byteAutoModeNowStep == 35) last = true;
            }
            else if (workModel == 2) // 排杆
            {
                if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || values[0].ToString() != "5") // 非自动模式
                {
                    return "排杆启动";
                }
                if ((byteAutoModeNowStep == 0)) return "排杆启动";
                if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 9)) return "井口定位";
                if ((byteAutoModeNowStep >= 10) && (byteAutoModeNowStep <= 11)) return "井口抓杆";
                if ((byteAutoModeNowStep >= 10) && (byteAutoModeNowStep <= 16)) return "井口位置";
                if ((byteAutoModeNowStep == 12)) return "吊卡确认";
                if ((byteAutoModeNowStep >= 13) && (byteAutoModeNowStep <= 16)) return "井口运动";
                if ((byteAutoModeNowStep >= 17) && (byteAutoModeNowStep <= 18)) return "指梁定位";
                if ((byteAutoModeNowStep == 19)) return "指梁锁确认";
                if ((byteAutoModeNowStep >= 20) && (byteAutoModeNowStep <= 24) || (byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) return "指梁排管";
                if ((byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) last = true;

            }

            return "未知";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 从下位机获取回转角度值 da["callAngle"]--15，16位
    /// </summary>
    public class GripConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "未知";
            }

            byte status = (byte)value;
            if (status == 1 || status == 4) return "打开";
            if (status == 2 || status == 5) return "半开";
            if (status == 3 || status == 6) return "关闭";

            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻台面-行车
    /// </summary>
    public class CarMoveModelCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null
                || values[0] == DependencyProperty.UnsetValue | values[1] == DependencyProperty.UnsetValue)
            {
                return "行车";
            }
            bool left = (bool)values[0];
            bool right = (bool)values[1];
            if (left) return "行车左移";
            if (right) return "行车右移";
            return "行车";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻台面-行车
    /// </summary>
    public class CarMoveModelIsCheckCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null
                || values[0] == DependencyProperty.UnsetValue | values[1] == DependencyProperty.UnsetValue)
            {
                return false;
            }
            bool left = (bool)values[0];
            bool right = (bool)values[1];
            if (left) return true;
            if (right) return false;
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻台面/二层台切换
    /// </summary>
    public class SelectTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return "机械手";
            }

            bool type = (bool)value;
            if (type) return "二层台";
            else return "钻台面";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 钻台面/二层台切换
    /// </summary>
    public class SelectTypeIsCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return false;
            }

            bool type = (bool)value;
            if (type) return false;
            else return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 近控
    /// </summary>
    public class TelecontrolModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return "操作";
            }

            bool type = (bool)value;
            if (type) return "遥控";
            else return "近控";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 钻台面/二层台切换
    /// </summary>
    public class TelecontrolModelIsCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return false;
            }

            bool type = (bool)value;
            if (type) return false;
            else return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 钻台面-排杆/送杆步骤
    /// </summary>
    public class DRStepCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null)
                || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null)
                || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return 0;
            }
            byte oprModel = (byte)values[0];
            if (oprModel != 5) // 非自动模式
            {
                return 0;
            }
            byte workModer = (byte)values[1];
            byte step = (byte)values[2];
            if (workModer == 2) // 排杆
            {
                if (step == 0) return 0;
                if (step >= 1 && step <= 6) return 1;
                if (step >= 7 && step <= 9) return 2;
                if (step >= 10 && step <= 17) return 3;
                if (step == 18) return 4;
                if (step >= 19 && step <= 20) return 5;
                if (step == 21) return 6;
            }
            else if (workModer == 1) // 送杆
            {
                if (step == 0) return 0;
                if (step >= 1 && step <= 5) return 1;
                if (step >= 6 && step <= 9) return 2;
                if (step >= 10 && step <= 15) return 3;
                if (step >= 16 && step <= 17) return 4;
                if (step >= 18 && step <= 19) return 5;
                if (step == 20) return 6;
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻台面-排杆/送杆步骤 文字显示
    /// </summary>
    public class DRStepTxtCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null)
                || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null)
                || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return "非自动模式";
            }
            byte oprModel = (byte)values[0];
            if (oprModel != 5) // 非自动模式
            {
                return "非自动模式";
            }
            byte workModer = (byte)values[1];
            byte step = (byte)values[2];
            if (workModer == 2) // 排管
            {
                if (step == 0) return "排管启动";
                if (step >= 1 && step <= 6) return "井口定位";
                if (step >= 7 && step <= 9) return "井口抓杆";
                if (step >= 10 && step <= 17) return "台面定位";
                if (step == 18) return "抓手松开";
                if (step >= 19 && step <= 20) return "手臂回位";
                if (step == 21) return "台面完成";
            }
            else if (workModer == 1) // 送杆
            {
                if (step == 0) return "送杆启动";
                if (step >= 1 && step <= 5) return "台面定位";
                if (step >= 6 && step <= 9) return "抓手夹紧";
                if (step >= 10 && step <= 15) return "井口定位";
                if (step >= 16 && step <= 17) return "井口送杆";
                if (step >= 18 && step <= 19) return "手臂回位";
                if (step == 20) return "送杆结束";
            }

            return "非自动模式";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻台面-抓手错误状态
    /// </summary>
    public class GridErrorMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[1] == null || values[2] == null || values[3] == null || values[4] == null || values[5] == null
                || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue
                || values[3] == DependencyProperty.UnsetValue || values[4] == DependencyProperty.UnsetValue || values[5] == DependencyProperty.UnsetValue)
            {
                return "未知";
            }
            if ((bool)values[0]) return "传感器故障";
            if ((bool)values[1]) return "打开卡滞";
            if ((bool)values[2]) return "关闭卡滞";
            return "未知";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 排杆/送杆步骤
    /// </summary>
    public class LinkErrorCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue
                || values[1] == null || values[1] == DependencyProperty.UnsetValue
                || values[2] == null || values[2] == DependencyProperty.UnsetValue
                || values[3] == null || values[3] == DependencyProperty.UnsetValue)
            {
                return "联动未开启";
            }
            int error = (byte)values[0];
            if(error == 23) return "模式不一致,无法开启联动";
            else if (error == 24) return "目标指梁不一致，无法开启联动";
            else if (error == 25) return "非立根模式，无法开启联动";
            else if (error == 26) return "非手自动模式，无法开启联动";
            else if (error == 40) return "请切换至二层台";
            else if (error == 41) return "钻台面叜初始位";
            else if (error == 42) return "二层台钳头禁止打开";
            else if (error == 45) return "请切换至钻台面";
            else if (error == 46) return "钻台面钳头禁止打开";
            else if (error == 47) return "清扣不在初始位";
            else if (error == 50) return "请切换至涂抹丝扣盒清扣";
            else if (error == 51) return "清扣盒涂抹禁止收回";
            else if (error == 52) return "铁钻工不在初始位";
            else if (error == 55) return "请切换至铁钻工";
            else if (error == 56) return "铁钻工禁止缩回";
            else if (error == 61) return "防喷盒不在初始位";
            else if (error == 62) return "请切换至防喷盒";
            else if (error == 63) return "防喷盒禁止缩回";
            else if (error == 64) return "二层台不在初始位";
            else if (error == 65) return "钻台面禁止缩回";
            else if (error == 66) return "二层台禁止缩回";
            else if (error == 120) return "二层台或钻台面不处于自动，无法开启";
            else if (error == 121) return "二层台或钻台面不处于初始步，无法开启";
            else if (error == 122) return "二层台或钻台面抓手有钻杆，无法开启";
            else if (error == 123) return "二层台或钻台面模式不一致，无法开启";
            else if (error == 124) return "二层台或钻台面目标指梁不一致，无法开启";
            else if (error == 125) return "钻台面目的非立根盒，无法开启";
            else if (error == 130) return "二层台或钻台面不处于自动，联动关闭";
            else if (error == 131) return "二层台或钻台面模式不一致，联动关闭";
            else if (error == 132) return "检测到联动流程调整异常，请确认并重新开启";
            else if (error == 140) return "手柄及旋转功能禁止使用";

            bool b0 = (bool)values[1];
            bool b1= (bool)values[2];
            bool b2 = (bool)values[3];
            if (b0)
            {
                if (b1 && b2) return "联动已开启，二层台/钻台面使能";
                else if (b1) return "联动已开启，二层台使能";
                else if (b2) return "联动已开启，钻台面使能";
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 集成系统-联动开启/关闭
    /// </summary>
    public class LinkOpenOrCloseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return "联动关闭";
            }

            bool type = (bool)value;
            if (type) return "联动开启";
            else return "联动关闭";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 集成系统-自动排杆/送杆步骤
    /// </summary>
    public class IngStepCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null)
                || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null)
                || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null)
                || (values[3] == DependencyProperty.UnsetValue) || (values[3] == null)
                || (values[4] == DependencyProperty.UnsetValue) || (values[4] == null)
                || (values[5] == DependencyProperty.UnsetValue) || (values[5] == null)
                || (values[6] == DependencyProperty.UnsetValue) || (values[6] == null))
            {
                return 0;
            }
            byte droprModel = (byte)values[0];
            byte sfOprModel = (byte)values[3];
            if (droprModel != 5 || sfOprModel != 5) // 非自动模式
            {
                return 0;
            }
            byte drworkModer = (byte)values[1];
            byte drstep = (byte)values[2];
            byte sfWorkModel = (byte)values[4];
            byte sfstep = (byte)values[5];
            bool b460b1 = (bool)values[6];
            if (drworkModer == 2 && sfWorkModel ==2) // 排杆
            {
                if (!b460b1)
                {
                    if (drstep == 0) return 0;
                    else if (drstep >= 1 && drstep <= 6) return 1;
                    else if (drstep >= 7 && drstep <= 9) return 2;
                    else if (drstep >= 10 && drstep <= 17) return 3;
                    else if (drstep == 18) return 4;
                    else if (drstep >= 19 && drstep <= 20) return 5;
                    else if (drstep == 21) return 6;
                }
                else
                {
                    if (sfstep >= 1 && sfstep <= 9) return 7;
                    else if (sfstep >= 10 && sfstep <= 11) return 8;
                    else if (sfstep == 12) return 9;
                    else if (sfstep >= 13 && sfstep <= 18) return 10;
                    else if (sfstep == 19) return 11;
                    else if (sfstep >= 20 && sfstep <= 24) return 12;
                    else if (sfstep >= 25) return 13;
                }
            }
            else if (drworkModer == 1 && sfWorkModel == 1) // 送杆
            {
                if (b460b1)
                {
                    if (sfstep == 0) return 0;
                    else if (sfstep >= 1 && sfstep <= 10) return 1;
                    else if (sfstep >= 11 && sfstep <= 16) return 2;
                    else if (sfstep >= 17 && sfstep <= 18) return 3;
                    else if (sfstep >= 19 && sfstep <= 25) return 4;
                    else if (sfstep == 26) return 5;
                    else if (sfstep >= 27 && sfstep <= 34) return 6;
                    else if (sfstep == 35) return 7;
                }
                else
                {
                    if (drstep >= 1 && drstep <= 5) return 8;
                    else if(drstep >= 6 && drstep <= 9) return 9;
                    else if(drstep >= 10 && drstep <= 15) return 10;
                    else if(drstep >= 16 && drstep <= 17) return 11;
                    else if(drstep >= 18 && drstep <= 19) return 12;
                    else if(drstep == 20) return 13;
                }
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 集成系统-自动排杆/送杆步骤
    /// </summary>
    public class IngAutoModeNowStepCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null)
                || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null)
                || (values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return "非自动模式";
            }
            byte droprModel = (byte)values[0];
            byte sfOprModel = (byte)values[3];

            
            if (droprModel != 5 || sfOprModel != 5) // 非自动模式
            {
                return "非自动模式";
            }
            bool b460b1 = (bool)values[6];
            byte drworkModer = (byte)values[1];
            byte drstep = (byte)values[2];
            byte sfWorkModel = (byte)values[4];
            byte sfstep = (byte)values[5];
            if (drworkModer == 2) // 排杆
            {
                if (!b460b1)
                {
                    if (drstep == 0) return "排杆启动";
                    else if (drstep >= 1 && drstep <= 6) return "井口定位";
                    else if (drstep >= 7 && drstep <= 9) return "井口抓杆";
                    else if (drstep >= 10 && drstep <= 17) return "台面定位";
                    else if (drstep == 18) return "抓手松开";
                    else if (drstep >= 19 && drstep <= 20) return "手臂回位";
                    else if (drstep == 21) return "台面完成";
                }
                else
                {
                 if (sfstep >= 1 && sfstep <= 9) return "井口定位";
                    else if (sfstep >= 10 && sfstep <= 11) return "井口抓杆";
                    else if (sfstep == 12) return "吊卡确认";
                    else if (sfstep > 12 && sfstep <= 18) return "指梁定位";
                    else if (sfstep == 19) return "指梁锁确认";
                    else if (sfstep >= 20 && sfstep <= 24) return "指梁排管";
                    else if (sfstep >= 25) return "排管结束";
                }
            }
            else if (drworkModer == 1 && sfWorkModel == 1) // 送杆
            {
                if (b460b1)
                {
                    if (sfstep == 0) return "送杆启动";
                    else if (sfstep >= 1 && sfstep <= 10) return "指梁定位";
                    else if (sfstep >= 11 && sfstep <= 16) return "指梁抓杆";
                    else if (sfstep >= 17 && sfstep <= 18) return "指梁锁确认";
                    else if (sfstep >= 19 && sfstep <= 25) return "井口等待";
                    else if (sfstep == 26) return "井口送杆";
                    else if (sfstep >= 27 && sfstep <= 34) return "吊卡确认";
                    else if (sfstep == 35) return "二层台完成";
                }
                else
                {
                    if (drstep >= 1 && drstep <= 5) return "台面定位";
                    else if (drstep >= 6 && drstep <= 9) return "抓手夹紧";
                    else if (drstep >= 10 && drstep <= 15) return "井口定位";
                    else if (drstep >= 16 && drstep <= 17) return "井口送杆";
                    else if (drstep >= 18 && drstep <= 19) return "手臂回位";
                    else if (drstep == 20) return "送杆结束";
                }
            }

            return "非自动模式";
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 集成系统-互锁提示
    /// </summary>
    public class IngLockTipsCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return "";
            }
            byte val = (byte)value;
            if (val == 30) return "吊卡与大钩互锁中";
            else if (val == 31) return "互锁解除中，请注意吊卡是否关闭";
            else if (val == 32) return "互锁解除中，请谨慎操作大钩";
            else if (val == 33) return "危险区域，大钩安全高度未标定";
            else if (val == 34) return "危险区域，机械手与大钩互锁中";
            else if (val == 35) return "危险区域，大钩下降中";
            else if (val == 36) return "危险区域，大钩上升中";
            else if (val == 37) return "危险区域，大钩在机械手下方";
            else if (val == 38) return "危险区域，大钩在机械手上方";
            else if (val == 43) return "大钩安全高度未标定";
            else if (val == 45) return "大钩下降中";
            else if (val == 46) return "大钩上升中";
            else if (val == 47) return "大钩在机械手下方";
            else if (val == 34) return "大钩在机械手上方";
            else if (val == 1) return "零位标定不合理";
            else if (val == 2) return "高位标定不合理";
            else if (val == 3) return "高度设置不合理";
            else if (val == 11) return "低位刹车设置成功";
            else if (val == 12) return "低位报警区设置成功";
            else if (val == 22) return "低位报警区设置不合理";
            else if (val == 13) return "低位设置成功";
            else if (val == 23) return "低位设置不合理";
            else if (val == 14) return "中位设置成功";
            else if (val == 24) return "中位设置不合理";
            else if (val == 15) return "高位设置成功";
            else if (val == 25) return "高位设置不合理";
            else if (val == 16) return "高位报警区设置成功";
            else if (val == 26) return "高位报警区设置不合理";
            else if (val == 17) return "高位刹车区设置成功";
            else if (val == 27) return "高位刹车区设置不合理";
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 钻杆/钻铤选择
    /// </summary>
    public class DrillTypeSelectCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[0] == null
                || values[1] == DependencyProperty.UnsetValue || values[1] == null)
            {
                return "数据错误";
            }
            byte bType = (byte)values[0];
            bool IsSpecial = (bool)values[1];
            if(bType == 35) return "3.5寸钻杆";
            else if (bType == 28) return "2寸7/8钻杆";
            else if(bType == 40) return "4寸钻杆";
            else if (bType == 45 && !IsSpecial) return "4.5寸钻杆";
            else if (bType == 35 && IsSpecial) return "3.5寸钻铤";
            else if (bType == 45 && IsSpecial) return "4.5寸钻铤";
            else if (bType == 50) return "5寸钻杆";
            else if (bType == 55) return "5.5寸钻杆";
            else if (bType == 57) return "5寸7/8钻杆";
            else if (bType == 60) return "6寸钻铤";
            else if (bType == 65) return "6.5寸钻铤";
            else if (bType == 68 && IsSpecial) return "6寸5/8钻杆";
            else if (bType == 70) return "7寸钻铤";
            else if (bType == 75) return "7.5寸钻铤";
            else if (bType == 80) return "8寸钻铤";
            else if (bType == 90) return "9寸钻铤";
            else if (bType == 100) return "10寸钻铤";
            else if (bType == 110) return "11寸钻铤";

            return "未选中管柱类型";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 左/右手指状态
    /// </summary>
    public class FingerStatusCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[0] == null
                || values[1] == DependencyProperty.UnsetValue || values[1] == null)
            {
                return "数据错误";
            }
            bool b3 = (bool)values[0];
            bool b5 = (bool)values[1];
            if (b3) return "打开";
            if (b5) return "关闭";
            if (!b3 && !b5) return "过渡";
            

            return "未知";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #region 铁钻工

    /// <summary>
    /// 铁钻工-右手柄选择
    /// </summary>
    public class SIRRightHandleSelectCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return "未知";
            }
            bool val = (bool)value;
            if (val) return "猫道";
            else return "铁钻工";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 铁钻工-工作状态
    /// </summary>
    public class SIRWorkModelCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return "未知";
            }
            byte val = (byte)value;
            if (val == 1) return "待机";
            else if (val == 2) return "伸出";
            else if (val == 3) return "缩回";
            else if (val == 4) return "上扣";
            else if (val == 5) return "卸扣";
            else if (val == 6) return "上升";
            else if (val == 7) return "下降";
            else if (val == 8) return "旋进";
            else if (val == 9) return "旋出";
            else if (val == 10) return "左转";
            else if (val == 11) return "右转";
            else if (val == 12) return "复位";
            else if (val == 13) return "急停";
            else return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 铁钻工-操作模式
    /// </summary>
    public class SIROprModelCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return "未知";
            }
            byte val = (byte)value;
            if (val == 1) return "遥控";
            else if (val == 2) return "司钻";
            else if (val == 3) return "本地";
            else return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 铁钻工-操作模式
    /// </summary>
    public class SIROprModelSelectCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return "false";
            }
            byte val = (byte)value;
            if (val == 1) return true;
            else if (val == 2) return false;

            else return "false";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 铁钻工-工作位置
    /// </summary>
    public class SIRWorkLocationCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return "未知";
            }
            byte val = (byte)value;
            if (val == 1) return "零位";
            else if (val == 2) return "待机";
            else if (val == 3) return "井口";
            else if (val == 4) return "鼠洞";
            else return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 铁钻工-计算实时压力
    /// </summary>
    public class SIRRealTimePressureCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[0] == null
                || values[1] == DependencyProperty.UnsetValue || values[1] == null)
            {
                return "0";
            }
            short val = (short)values[0];
            string unit = (string)values[1];
            double rate = 0.0;
            if (unit == "KN.M") rate = 1;
            else rate = 0.738;
            double ret = val * rate / 10;
            return ret.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 铁钻工-标定状态
    /// </summary>
    public class SIRCalibrationStatusCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return "异常";
            }
            bool val = (bool)value;
            if (val) return "正常";
            else return "异常";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 高档-Mpa转换为KN.m
    /// </summary>
    public class HighMpaToKNmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return "0";
            }
            int val = (byte)value;
            return (6 * val / 1000000 + 4757 * val / 10000 - 1).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 紧扣-Mpa转换为KN.m
    /// </summary>
    public class CloseMpaToKNmConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return "0";
            }
            int val = (byte)value;
            return (69 * val / 10).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region 液压站

    public class HyControlModelMuilCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bc = new BrushConverter();
            if (values[0] == DependencyProperty.UnsetValue || values[0] == null
                || values[1] == DependencyProperty.UnsetValue || values[1] == null)
            {
                return false;
            }
            bool valone = (bool)values[0];
            bool valtwo = (bool)values[1];
            if (valone && !valtwo) return true;
            else if (valtwo && !valone) return false;
            else if (!valtwo && !valone) return true;
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HyControlModelTxtMuilCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bc = new BrushConverter();
            if (values[0] == DependencyProperty.UnsetValue || values[0] == null
                || values[1] == DependencyProperty.UnsetValue || values[1] == null)
            {
                return "未知";
            }
            bool valone = (bool)values[0];
            bool valtwo = (bool)values[1];
            if (valone && !valtwo) return "本地";
            else if (valtwo && !valone) return "司钻";
            else if (!valtwo && !valone) return "分阀箱";
            return "未知";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SIRSelfRotateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return 0;
            }
            int val = (int)value;
            double ret = (val - 141666) * 8 / 4096.0;
            ret = Math.Round(ret, 0);
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DivideTenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return 0;
            }
            double val = 0.0;
            if (value.GetType().Name == "Int32")
            {
                val = (int)value / 10.0;
            }
            else if (value.GetType().Name == "Int16")
            {
                val = (short)value / 10.0;
            }
            else if (value.GetType().Name == "Byte")
            {
                val = (byte)value / 10.0;
            }

            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TakeTenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return 0;
            }
            int val = 0;
            if (value.GetType().Name == "Int32")
            {
                val = (int)value / 10;
            }
            else if (value.GetType().Name == "Int16")
            {
                val = (short)value / 10;
            }
            else if (value.GetType().Name == "Byte")
            {
                val = (byte)value / 10;
            }

            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DivideHundredConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return 0;
            }
            double val = 0.0;
            if (value.GetType().Name == "Int32")
            {
                val = (int)value / 100.0;
            }
            else if (value.GetType().Name == "Int16")
            {
                val = (short)value / 100.0;
            }
            else if (value.GetType().Name == "Byte")
            {
                val = (byte)value / 100.0;
            }

            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bc = new BrushConverter();
            if (values[0] == DependencyProperty.UnsetValue || values[0] == null
                || values[1] == DependencyProperty.UnsetValue || values[1] == null
                || values[2] == DependencyProperty.UnsetValue || values[2] == null)
            {
                return (Brush)bc.ConvertFrom("#ADD8E6");
            }
            double val = (short)values[0] / 10.0;
            double min = (double)values[1];
            double max = (double)values[2];
            if (max ==0) max = 100;
            double levelOne = (max - min) / 3;
            double levelTwo = ((max - min) / 3) * 2;
            if (val < levelOne) return (Brush)bc.ConvertFrom("#2968DC");
            else if (val < levelTwo) return (Brush)bc.ConvertFrom("#FFC464");
            else return (Brush)bc.ConvertFrom("#FF0000");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorDescCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bc = new BrushConverter();
            if (values[0] == DependencyProperty.UnsetValue || values[0] == null
                || values[1] == DependencyProperty.UnsetValue || values[1] == null
                || values[2] == DependencyProperty.UnsetValue || values[2] == null)
            {
                return (Brush)bc.ConvertFrom("#FF7C96");
            }
            double val = (short)values[0] / 10.0;
            double min = (double)values[1];
            double max = (double)values[2];
            if (max < 100) max = 100;
            double levelOne = (max - min) / 3;
            double levelTwo = ((max - min) / 3) * 2;
            if (val < levelOne) return (Brush)bc.ConvertFrom("#FF7C96");
            else if (val < levelTwo) return (Brush)bc.ConvertFrom("#FFC464");
            else return (Brush)bc.ConvertFrom("#ADD8E6");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PumpImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return "../../Images/pump1.png";
            }

            bool val = (bool)value;
            if(val) return "../../Images/pump2.png";
            else return "../../Images/pump1.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HotImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return "../../Images/hot1.png";
            }

            bool val = (bool)value;
            if (val) return "../../Images/hot2.png";
            else return "../../Images/hot1.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BtnColorCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bc = new BrushConverter();
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return (Brush)bc.ConvertFrom("#FFFFFF");
            }
            bool val = (bool)value;
            if(val) return (Brush)bc.ConvertFrom("#326CF3");
            else return (Brush)bc.ConvertFrom("#FFFFFF");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 上扣模式
    /// </summary>
    public class InButtonModelCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return "上扣模式选择:未知";
            }
            byte val = (byte)value;
            if (val ==0) return "上扣模式选择:全自动";
            else if(val == 1) return "上扣模式选择:高速";
            else if (val == 2) return "上扣模式选择:低速";
            else return "上扣模式选择:未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BtnColorMuilCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var bc = new BrushConverter();
            if (values[0] == DependencyProperty.UnsetValue || values[0] == null
                || values[1] == DependencyProperty.UnsetValue || values[1] == null)
            {
                return (Brush)bc.ConvertFrom("#FFFFFF");
            }
            bool valone = (bool)values[0];
            bool valtwo = (bool)values[1];
            if (valone && valtwo) return (Brush)bc.ConvertFrom("#326CF3");
            else return (Brush)bc.ConvertFrom("#FFFFFF");

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CbBoolTgaCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == DependencyProperty.UnsetValue || value == null)
            {
                return false;
            }
            bool val = (bool)value;
            if (val) return true;
            else return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion


    /// <summary>
    /// 排杆/送杆步骤
    /// </summary>
    public class AutoModeStepCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string tb = (string)parameter;
            int workModel = 1;
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                workModel = 1;
            }
            else
            {
                if (values[1].ToString() == "2")
                {
                    workModel = 2;
                }
            }
            int byteAutoModeNowStep = int.Parse(values[2].ToString());
            if (tb == "one")
            {
                if (workModel == 1) //送杆
                {
                    if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || values[0].ToString() != "5") // 非自动模式
                    {
                        return 0;
                    }

                    if (byteAutoModeNowStep == 0) return 0;
                    if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 10)) return 1;
                    if ((byteAutoModeNowStep >= 11) && (byteAutoModeNowStep <= 16)) return 2;
                    if ((byteAutoModeNowStep >= 17) && (byteAutoModeNowStep <= 18)) return 3;
                    if ((byteAutoModeNowStep >= 19) && (byteAutoModeNowStep <= 25)) return 4;
                    if ((byteAutoModeNowStep == 26)) return 5;
                    if ((byteAutoModeNowStep >= 27) && (byteAutoModeNowStep <= 28)) return 6;
                    if ((byteAutoModeNowStep >= 24) && (byteAutoModeNowStep <= 34)) return 7;
                    if (byteAutoModeNowStep == 35) return 8;
                }
                else if (workModel == 2) // 排杆
                {
                    if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || values[0].ToString() != "5") // 非自动模式
                    {
                        return 0;
                    }
                    if ((byteAutoModeNowStep == 0)) return 0;
                    if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 9)) return 1;
                    if ((byteAutoModeNowStep >= 10) && (byteAutoModeNowStep <= 11)) return 2;
                    if ((byteAutoModeNowStep == 12)) return 3;
                    if ((byteAutoModeNowStep >= 13) && (byteAutoModeNowStep <= 16)) return 4;
                    if ((byteAutoModeNowStep >= 17) && (byteAutoModeNowStep <= 18)) return 5;
                    if ((byteAutoModeNowStep == 19)) return 6;
                    if ((byteAutoModeNowStep >= 20) && (byteAutoModeNowStep <= 24)) return 7;
                    if ((byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) return 8;
                }
            }
            else if (tb == "two")
            {
                if (workModel == 1) //送杆
                {
                    if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || values[0].ToString() != "5") // 非自动模式
                    {
                        return "指梁定位";
                    }
                    if (byteAutoModeNowStep == 0) return "指梁定位";
                    if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 10)) return "指梁抓杆";
                    if ((byteAutoModeNowStep >= 11) && (byteAutoModeNowStep <= 16)) return "指梁锁确认";
                    if ((byteAutoModeNowStep >= 17) && (byteAutoModeNowStep <= 18)) return "井口等待";
                    if ((byteAutoModeNowStep >= 19) && (byteAutoModeNowStep <= 23)) return "井口旋转";
                    if ((byteAutoModeNowStep >= 24) && (byteAutoModeNowStep <= 25)) return "吊卡送杆";
                    if ((byteAutoModeNowStep == 26)) return "吊卡确认";
                    if ((byteAutoModeNowStep >= 27) && (byteAutoModeNowStep <= 28)) return "井口位置";
                    if ((byteAutoModeNowStep >= 29) && (byteAutoModeNowStep <= 34) || (byteAutoModeNowStep == 35)) return "送杆完成";
                    //if (byteAutoModeNowStep == 35) last = true;
                }
                else if (workModel == 2) // 排杆
                {
                    if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null) || values[0].ToString() != "5") // 非自动模式
                    {
                        return "井口定位";
                    }

                    if ((byteAutoModeNowStep == 0)) return "井口定位";
                    if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 9)) return "井口抓杆";
                    if ((byteAutoModeNowStep >= 10) && (byteAutoModeNowStep <= 11)) return "井口位置";
                    if ((byteAutoModeNowStep >= 10) && (byteAutoModeNowStep <= 16)) return "吊卡确认";
                    if ((byteAutoModeNowStep == 12)) return "井口运动";
                    if ((byteAutoModeNowStep >= 13) && (byteAutoModeNowStep <= 16)) return "指梁定位";
                    if ((byteAutoModeNowStep >= 17) && (byteAutoModeNowStep <= 18)) return "指梁锁确认";
                    if ((byteAutoModeNowStep == 19)) return "指梁排管";
                    if ((byteAutoModeNowStep >= 20) && (byteAutoModeNowStep <= 24) || (byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) return "排杆完成";
                    //if ((byteAutoModeNowStep >= 25) && (byteAutoModeNowStep <= 27)) last = true;
                }
            }

            return "未知";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool型通用，取反
    /// </summary>
    public class CheckedIsFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return false;
            }

            bool type = (bool)value;
            if (type) return false;
            else return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #region 自研铁钻工
    /// <summary>
    /// 自研铁钻工-操作模式
    /// </summary>
    public class SIRSelfOperationModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 1:
                    return "手动";
                case 2:
                    return "自动";
                case 5:
                    return "急停";
            }

            return "操作模式";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 自研铁钻工-操作模式选择
    /// </summary>
    public class SIRSelfIsCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            byte bType = (byte)value;

            if (bType == 1)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 自研铁钻工-液压钳缺口
    /// </summary>
    public class SIRSelfTongsGapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "未知";
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 10:
                    return "仅主钳完成复位";
                case 11:
                    return "仅背钳完成复位";
                case 12:
                    return "复位状态正常";
                case 20:
                    return "安全设置关闭";
            }

            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 自研铁钻工-工作模式
    /// </summary>
    public class SIRSelfWorkModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 1:
                    return "上扣";
                case 2:
                    return "卸扣";
                case 101:
                    return "切换中";
            }

            return "工作模式";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 自研铁钻工-钻具选择
    /// </summary>
    public class SIRSelfPipeTypeModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 109:
                    return "钻杆";
                case 99:
                    return "套管";
            }

            return "钻具选择";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 自研铁钻工-钻具选择
    /// </summary>
    public class SIRSelfPipeTypeIsCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 109:
                    return true;
                case 99:
                    return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 自研铁钻工-工位选择
    /// </summary>
    public class SIRSelfLocationModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 1:
                    return "井口";
                case 2:
                    return "鼠洞";
            }

            return "工位选择";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 1-false;2-true
    /// </summary>
    public class SIRSelfTwoToCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            byte bType = (byte)value;

            if (bType == 2)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 铁钻工自动步骤
    /// </summary>
    public class SIRSelfAutoModeStepCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string tb = (string)parameter;
            bool autoOpr = false;
            byte workmodel = 0; 
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                autoOpr = false;
            }
            else
            {
                if ((byte)values[0] == 2)
                    autoOpr = true;
            }
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                workmodel = 0;
            }
            else
            {
                    workmodel = (byte)values[1];
            }
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return 0;
            }
            int byteAutoModeNowStep = (byte)values[2];
            if (autoOpr && workmodel == 1)
            {
                if (byteAutoModeNowStep == 0) return 0;
                if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 6)) return 1;
                if ((byteAutoModeNowStep >= 7) && (byteAutoModeNowStep <= 8)) return 2;
                if ((byteAutoModeNowStep >= 49) && (byteAutoModeNowStep <= 51)) return 3;
                if ((byteAutoModeNowStep >= 52) && (byteAutoModeNowStep <= 53)) return 4;
                if ((byteAutoModeNowStep >= 9) && (byteAutoModeNowStep <= 10)) return 5;
                if ((byteAutoModeNowStep >= 11) && (byteAutoModeNowStep <= 12)) return 6;
                if ((byteAutoModeNowStep >= 13) && (byteAutoModeNowStep <= 17)) return 7;
                if (byteAutoModeNowStep == 18) return 8;
            }
            else if (autoOpr && workmodel == 2)// 卸扣
            {
                if (byteAutoModeNowStep == 0) return 0;
                if ((byteAutoModeNowStep >= 1) && (byteAutoModeNowStep <= 6)) return 1;
                if ((byteAutoModeNowStep >= 7) && (byteAutoModeNowStep <= 8)) return 2;
                if ((byteAutoModeNowStep >= 49) && (byteAutoModeNowStep <= 51)) return 3;
                if ((byteAutoModeNowStep >= 52) && (byteAutoModeNowStep <= 53)) return 4;
                if ((byteAutoModeNowStep >= 9) && (byteAutoModeNowStep <= 10)) return 5;
                if ((byteAutoModeNowStep >= 11) && (byteAutoModeNowStep <= 12)) return 6;
                if ((byteAutoModeNowStep >= 13) && (byteAutoModeNowStep <= 17)) return 7;
                if (byteAutoModeNowStep == 18) return 8;
            }
            

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region 通用
    /// <summary>
    /// Dictionary
    /// </summary>
    public class NumToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return string.Empty;
            }
            Dictionary<int, string> keyValues = (Dictionary<int, string>)parameter;
            int val = (byte)value;
            string str = string.Empty;
            bool hasVal = keyValues.TryGetValue(val, out str);
            if (hasVal) return str;
            else return val.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Dictionary
    /// </summary>
    public class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
            {
                return string.Empty;
            }
            Dictionary<int, string> keyValues = (Dictionary<int, string>)parameter;
            bool val = (bool)value;
            if (val) return "停用";
            else return "启用";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region 集成系统
    /// <summary>
    /// 操作模式-描述
    /// </summary>
    public class IngOprCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue
                || values[1] == null || values[1] == DependencyProperty.UnsetValue
                || values[2] == null || values[2] == DependencyProperty.UnsetValue)
            {
                return "操作模式";
            }
            byte sfType = (byte)values[0];
            byte drType = (byte)values[1];
            byte sirType = (byte)values[2];
            if (sfType == 4 && drType == 4 && sirType == 1) return "手动";
            if (sfType == 5 && drType == 5 && sirType == 2) return "自动";
            if (sfType ==1) return "二层台急停";
            if (drType == 1) return "钻台面急停";
            if (sirType == 5) return "铁钻工急停";
            if (sfType == 2) return "二层台调试";
            if (drType == 2) return "钻台面调试";
            if (sfType == 3) return "二层台回零";
            if (drType == 3) return "钻台面回零";
            if (sfType == 6) return "二层台回收";
            if (drType == 6) return "钻台面回收";
            if (sfType == 7) return "二层台运输";
            if (drType == 7) return "钻台面运输";
            if (sfType == 8) return "二层台实验";
            if (drType == 8) return "钻台面实验";
            if (sfType == 9) return "二层台补偿";
            if (drType == 9) return "钻台面补偿";

            return "操作模式";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 操作模式-选择
    /// </summary>
    public class IngOprCheckCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue
                || values[1] == null || values[1] == DependencyProperty.UnsetValue
                || values[2] == null || values[2] == DependencyProperty.UnsetValue)
            {
                return false;
            }
            byte sfType = (byte)values[0];
            byte drType = (byte)values[1];
            byte sirType = (byte)values[2];
            if (sfType == 4 && drType == 4 && sirType == 1) return true;
            if (sfType == 5 && drType == 5 && sirType == 2) return false;

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 工作模式-描述
    /// </summary>
    public class IngWorkCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue
                || values[1] == null || values[1] == DependencyProperty.UnsetValue
                || values[2] == null || values[2] == DependencyProperty.UnsetValue)
            {
                return "工作模式";
            }
            byte sfType = (byte)values[0];
            byte drType = (byte)values[1];
            byte sirType = (byte)values[2];
            if (sfType == 1 && drType == 1 && sirType == 1) return "送杆";
            if (sfType == 2 && drType == 2 && sirType == 2) return "排杆";

            return "工作模式";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 工作模式-选择
    /// </summary>
    public class IngWorkCheckCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null || values[0] == DependencyProperty.UnsetValue
                || values[1] == null || values[1] == DependencyProperty.UnsetValue
                || values[2] == null || values[2] == DependencyProperty.UnsetValue)
            {
                return false;
            }
            byte sfType = (byte)values[0];
            byte drType = (byte)values[1];
            byte sirType = (byte)values[2];
            if (sfType == 1 && drType == 1 && sirType == 1) return false;
            if (sfType == 2 && drType == 2 && sirType == 2) return true;

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region 轨道式铁钻工
    /// <summary>
    /// 轨道铁钻工-操作模式
    /// </summary>
    public class SIRRailWayOperationModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 2:
                    return "手动";
                case 1:
                    return "自动";
            }

            return "操作模式";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轨道铁钻工-操作模式选择
    /// </summary>
    public class SIRRailWayIsCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            byte bType = (byte)value;

            if (bType == 2)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轨道铁钻工-操作模式选择
    /// </summary>
    public class SIRRailWayIsCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            byte bType = (byte)value;

            if (bType == 2)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轨道铁钻工-操作模式
    /// </summary>
    public class SIRRailWayWorkModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 2:
                    return "卸扣";
                case 1:
                    return "上扣";
            }

            return "工作模式";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轨道铁钻工-控制模式
    /// </summary>
    public class SIRRailWayControlModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 2:
                    return "近控";
                case 1:
                    return "远控";
            }

            return "控制模式";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轨道铁钻工-自动步骤
    /// </summary>
    public class SIRRailWayAutoModeStepCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string tb = (string)parameter;
            bool autoOpr = false;
            byte workmodel = 0;
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null))
            {
                autoOpr = false;
            }
            else
            {
                if ((byte)values[0] == 1)
                    autoOpr = true;
            }
            if ((values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                workmodel = 0;
            }
            else
            {
                workmodel = (byte)values[1];
            }
            if ((values[2] == DependencyProperty.UnsetValue) || (values[2] == null))
            {
                return 0;
            }
            int byteAutoModeNowStep = (byte)values[2];
            if (autoOpr && workmodel == 1)
            {
                if (byteAutoModeNowStep == 0) return 0;
                else if (byteAutoModeNowStep == 1) return 1;
                else if (byteAutoModeNowStep == 2)  return 2;
                else if (byteAutoModeNowStep == 3) return 3;
                else if (byteAutoModeNowStep ==4) return 4;
                else if (byteAutoModeNowStep ==5) return 5;
                else if (byteAutoModeNowStep ==6) return 6;
                else if (byteAutoModeNowStep ==7) return 7;
                else if (byteAutoModeNowStep == 8) return 8;
                else if (byteAutoModeNowStep == 9) return 9;
            }
            else if (autoOpr && workmodel == 2)// 卸扣
            {
                if (byteAutoModeNowStep == 0) return 0;
                else if (byteAutoModeNowStep == 1) return 1;
                else if (byteAutoModeNowStep == 2) return 2;
                else if (byteAutoModeNowStep == 3) return 3;
                else if (byteAutoModeNowStep == 4) return 4;
                else if (byteAutoModeNowStep == 5) return 5;
                else if (byteAutoModeNowStep == 6) return 6;
                else if (byteAutoModeNowStep == 7) return 7;
                else if (byteAutoModeNowStep == 8) return 8;
                else if (byteAutoModeNowStep == 9) return 9;
            }


            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轨道铁钻工-管柱类型(1.钻杆;2.钻铤;3.套管)
    /// </summary>
    public class SIRRailWayDrillTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "未知";
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 1:
                    return "钻杆";
                case 2:
                    return "钻铤";
                case 3:
                    return "套管";
            }

            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轨道铁钻工-螺纹方向(0.右旋;1.左旋)
    /// </summary>
    public class SIRRailWayDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "未知";
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 2:
                    return "右旋";
                case 1:
                    return "左旋";
            }

            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轨道铁钻工-工位选择(1.待机位;2.井口位;3.鼠洞位)
    /// </summary>
    public class SIRRailWayLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "未知";
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 1:
                    return "待机位";
                case 2:
                    return "井口位";
                case 3:
                    return "鼠洞位";
            }

            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轨道铁钻工-喷涂标志(0.启用;1.停用)
    /// </summary>
    public class SIRRailWaySprayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "未知";
            }

            byte bType = (byte)value;
            switch (bType)
            {
                case 1:
                    return "启用";
                case 2:
                    return "停用";
            }

            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    

    /// <summary>
    /// 轨道铁钻工-高低档切换
    /// </summary>
    public class SIRRailWayHighOrLowCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null)
                || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                return "未知";
            }
            bool high = (bool)values[0];
            bool low = (bool)values[1];
            if (high) return "高档";
            if (low) return "低档";
            if (!high && !low) return "空挡";

            return "未知"; 
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 轨道铁钻工-高低档切换
    /// </summary>
    public class SIRRailWayHighOrLowCheckCoverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] == DependencyProperty.UnsetValue) || (values[0] == null)
                || (values[1] == DependencyProperty.UnsetValue) || (values[1] == null))
            {
                return false;
            }
            bool high = (bool)values[0];
            bool low = (bool)values[1];
            if (high) return true;
            if (low) return false;
            if (!high && !low) return false;

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
