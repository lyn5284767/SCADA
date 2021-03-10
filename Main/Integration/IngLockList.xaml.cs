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
using System.Windows.Shapes;

namespace Main.Integration
{
    /// <summary>
    /// IngLockList.xaml 的交互逻辑
    /// </summary>
    public partial class IngLockList : Window
    {
        public IngLockList()
        {
            InitializeComponent();
            GetLockDevice();
        }

        private void GetLockDevice()
        {
            List<LockInfo> list = new List<LockInfo>();
            list.Add(new LockInfo() { ID = 1, Content = "扶杆臂与大钩互锁", Opr = "请解除扶杆臂锁大钩" });
            list.Add(new LockInfo() { ID = 2, Content = "扶杆臂与大钩互锁", Opr = "请解除扶杆臂锁大钩" });
            list.Add(new LockInfo() { ID = 3, Content = "扶杆臂与大钩互锁", Opr = "请解除扶杆臂锁大钩" });
            this.lvRecord.ItemsSource = list;
        }
    }
    /// <summary>
    /// 互锁绑定列表
    /// </summary>
    public class LockInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作
        /// </summary>
        public string Opr { get; set; }
    }
}
