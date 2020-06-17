using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DatabaseLib
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

}
