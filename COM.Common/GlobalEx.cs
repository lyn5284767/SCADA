using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Common
{
    public static class GlobalEx
    {

        /// <summary>
        /// Short转为Double
        /// </summary>
        /// <param name="shortIn">输入</param>
        public static double ShortToDouble(this short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            double doubleOut = unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128;
            return doubleOut;
        }

        /// <summary>
        /// Byte转为Double
        /// </summary>
        /// <param name="shortIn">输入</param>
        public static double ByteToDouble(this byte byteIn)
        {
            double doubleOut = unchecked((sbyte)byteIn);
            return doubleOut;
        }

        /// <summary>
        /// Byte转为Int
        /// </summary>
        /// <param name="shortIn">输入</param>
        public static int ByteToInt(this byte byteIn)
        {
            int doubleOut = unchecked((sbyte)byteIn);
            return doubleOut;
        }

        /// <summary>
        /// Short转为string
        /// </summary>
        /// <param name="shortIn">输入</param>
        public static string ShortToString(this short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            string strOut = (unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128).ToString();
            return strOut;
        }

        /// Short转为int
        /// </summary>
        /// <param name="shortIn">输入</param>
        public static int ShortToInt(this short shortIn)
        {
            byte[] bytes = BitConverter.GetBytes(shortIn);
            int intOut = (unchecked((sbyte)bytes[0]) + unchecked((sbyte)bytes[1]) * 128);
            return intOut;
        }
    }
}
