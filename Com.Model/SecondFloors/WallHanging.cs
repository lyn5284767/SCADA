using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Model
{
    /// <summary>
    /// 二层台挂壁
    /// </summary>
    public class WallHanging
    {
        /// <summary>
        /// 行数
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// 列数
        /// </summary>
        public int Coloms { get; set; }
        /// <summary>
        /// 是否有钻铤,1代表左边有，2代表右边有， 3代表两边都有
        /// </summary>
        public int HaveDrill{ get; set; }
        /// <summary>
        /// 钻铤的个数
        /// </summary>
        public int DrillCnt { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
    }
}
