using COM.Common;
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

namespace Main.Integration
{
    /// <summary>
    /// ModelDetailData.xaml 的交互逻辑
    /// </summary>
    public partial class ModelDetailData : UserControl
    {
        public ModelDetailData()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据库读取数据显示具体模式
        /// </summary>
        /// <param name="globalModel"></param>
        public ModelDetailData(GlobalModel globalModel)
            : this()
        {
            string title = string.Empty;
            if (globalModel.HS_PumpType == 1) title = "1#泵";
            else if (globalModel.HS_PumpType == 2) title = "2#泵";
            else if(globalModel.HS_PumpType == 3) title = "双泵";

            if (globalModel.WorkType == 1) title += "排杆模式";
            else if(globalModel.WorkType ==2) title += "送杆模式";
            this.tbTitle.Text = title;

            string pipeType = string.Empty;
            if (globalModel.PipeType == 1)
            {
                if (globalModel.PipeSize == 28) pipeType = "2 7/8寸钻杆";
                else if (globalModel.PipeSize == 35) pipeType = "3.5寸钻杆";
                else if (globalModel.PipeSize == 40) pipeType = "4寸钻杆";
                else if (globalModel.PipeSize == 45) pipeType = "4.5寸钻杆";
                else if (globalModel.PipeSize == 50) pipeType = "5寸钻杆";
                else if (globalModel.PipeSize == 55) pipeType = "5.5寸钻杆";
                else if (globalModel.PipeSize == 57) pipeType = "5 7/8寸钻杆";
                else if (globalModel.PipeSize == 68) pipeType = "6 5/8寸钻杆";
            }
            else if (globalModel.PipeType == 2)
            {
                if (globalModel.PipeSize == 35) pipeType = "3.5寸钻铤";
                else if (globalModel.PipeSize == 45) pipeType = "4.5寸钻铤";
                else if (globalModel.PipeSize == 60) pipeType = "6寸钻铤";
                else if (globalModel.PipeSize == 65) pipeType = "6.5寸钻铤";
                else if (globalModel.PipeSize == 70) pipeType = "7寸钻铤";
                else if (globalModel.PipeSize == 75) pipeType = "7.5寸钻铤";
                else if (globalModel.PipeSize == 80) pipeType = "8寸钻铤";
                else if (globalModel.PipeSize == 90) pipeType = "9寸钻铤";
                else if (globalModel.PipeSize == 100) pipeType = "10寸钻铤";
                else if (globalModel.PipeSize == 110) pipeType = "11寸钻铤";
            }
            this.tbPipeType.Text = pipeType;

            if (globalModel.DesType == 1) this.tbDesType.Text = "立根区";
            else if (globalModel.DesType == 2) this.tbDesType.Text = "猫道-井口";
            else if (globalModel.DesType == 3) this.tbDesType.Text = "猫道-鼠洞";

            if (globalModel.SelectDrill == -1)
            {
                this.tbHandOrAuto.Text = "自动";
                this.tbSelectedDrill.Text = "自动";
            }
            else if (globalModel.SelectDrill > 0)
            {
                this.tbHandOrAuto.Text = "手动";
                if (globalModel.SelectDrill < 16)
                {
                    this.tbSelectedDrill.Text = "左" + globalModel.SelectDrill.ToString();
                }
                else if (globalModel.SelectDrill == 16)
                {
                    this.tbSelectedDrill.Text = "左钻铤";
                }
                else if (globalModel.SelectDrill > 16 && globalModel.SelectDrill < 32)
                {
                    this.tbSelectedDrill.Text = "右" + (globalModel.SelectDrill - 16).ToString();
                }
                else if (globalModel.SelectDrill == 32)
                {
                    this.tbSelectedDrill.Text = "右钻铤";
                }
            }
        }
    }
}
