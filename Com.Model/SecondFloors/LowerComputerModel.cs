using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Com.Model.SecondFloors
{
    /// <summary>
    /// 下位机相关信息
    /// </summary>
    public class LowerComputerModel
    {
        /// <summary>
        /// 设备年份
        /// </summary>
        public int DeviceYear { get; set; }
        /// <summary>
        /// 设备类型（预留）
        /// </summary>
        public int DeviceType { get; set; }
        /// <summary>
        /// 设备机型
        /// </summary>
        public int DeviceModel { get; set; }
        /// <summary>
        /// 设备车号
        /// </summary>
        public int DeviceCarNum { get; set; }

      
    }
}
