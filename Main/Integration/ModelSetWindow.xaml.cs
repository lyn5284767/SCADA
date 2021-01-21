using COM.Common;
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
using System.Windows.Shapes;

namespace Main.Integration
{
    /// <summary>
    /// ModelSetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ModelSetWindow : Window
    {
        public GlobalModel globalModel = new GlobalModel();
        public bool IsModify = false;
        /// <summary>
        /// 当前步骤 0-液压站；1-工作模式；2-管柱类型；3-管柱尺寸选择；4-目的地；5-指梁选择;6-确认配置
        /// </summary>
        private int Step { get; set; } = 0;
        public ModelSetWindow()
        {
            InitializeComponent();
            this.Loaded += ModelSetWindow_Loaded;
        }

        private void ModelSetWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowStep(0);
        }

        /// <summary>
        /// 显示对应步骤
        /// </summary>
        /// <param name="step">当前步骤</param>
        private void ShowStep(int step)
        {
            var bc = new BrushConverter();
            this.Step = step;
            this.gdHSSet.Visibility = Visibility.Collapsed;
            this.gdWorkModel.Visibility = Visibility.Collapsed;
            this.gdPipeOrCollar.Visibility = Visibility.Collapsed;
            this.gdPipeType.Visibility = Visibility.Collapsed;
            this.gdCollarType.Visibility = Visibility.Collapsed;
            this.gdDesType.Visibility = Visibility.Collapsed;
            this.gdSelectedAutoOrHand.Visibility = Visibility.Collapsed;
            this.gdSelectedDrill.Visibility = Visibility.Collapsed;
            this.gdConfirm.Visibility = Visibility.Collapsed;
            this.tbTips.Visibility = Visibility.Collapsed;
            this.btnPre.Visibility = Visibility.Visible;
            this.btnNext.Visibility = Visibility.Visible;
            this.btnNext.Content = "下一步";
            if (step == 0)
            {
                this.tbNowStep.Text = "液压站设置";
                this.tbFinishStep.Text = "第1项，共6项";
                this.gdHSSet.Visibility = Visibility.Visible;
                this.btnPre.Visibility = Visibility.Collapsed;
                if (globalModel.HS_PumpType == 1)
                {
                    this.btnPumpOne.Background = (Brush)bc.ConvertFrom("#72C9F6");
                    this.btnPumpTwo.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnPumpBoth.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.tbConfirmHS.Text = "液压站:1#主泵";
                }
                else if (globalModel.HS_PumpType == 2)
                {
                    this.btnPumpOne.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnPumpTwo.Background = (Brush)bc.ConvertFrom("#72C9F6");
                    this.btnPumpBoth.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.tbConfirmHS.Text = "液压站:2#主泵";
                }
                else if (globalModel.HS_PumpType == 3)
                {
                    this.btnPumpOne.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnPumpTwo.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnPumpBoth.Background = (Brush)bc.ConvertFrom("#72C9F6");
                    this.tbConfirmHS.Text = "液压站:双泵";
                }
            }
            else if (step == 1)
            {
                this.tbNowStep.Text = "工作模式设置";
                this.tbFinishStep.Text = "第2项，共6项";
                this.gdWorkModel.Visibility = Visibility.Visible;
                if (globalModel.WorkType == 1)
                {
                    this.btnDrillUp.Background = (Brush)bc.ConvertFrom("#72C9F6");
                    this.btnDrillDown.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.tbConfirmWorkModel.Text = "工况:送杆";
                }
                else if (globalModel.WorkType == 2)
                {
                    this.btnDrillUp.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnDrillDown.Background = (Brush)bc.ConvertFrom("#72C9F6");
                    this.tbConfirmWorkModel.Text = "工况:排杆";
                }
            }
            else if (step == 2)
            {
                this.tbNowStep.Text = "钻具类型设置";
                this.tbFinishStep.Text = "第3项，共6项";
                this.gdPipeOrCollar.Visibility = Visibility.Visible;
                if (globalModel.PipeType == 1)
                {
                    this.btnPipe.Background = (Brush)bc.ConvertFrom("#72C9F6");
                    this.btnCollar.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                }
                else if (globalModel.PipeType == 2)
                {
                    this.btnPipe.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnCollar.Background = (Brush)bc.ConvertFrom("#72C9F6");
                }
            }
            else if (step == 3)
            {
                if (globalModel.PipeType == 1)
                {
                    this.tbNowStep.Text = "尺寸设置";
                    this.tbFinishStep.Text = "第4项，共6项";
                    this.gdPipeType.Visibility = Visibility.Visible;
                    foreach (Button btn in GlobalMethed.FindVisualChildren<Button>(this.gdPipeType))
                    {
                        if (btn.Tag.ToString() == globalModel.PipeSize.ToString())
                        {
                            btn.Background = (Brush)bc.ConvertFrom("#72C9F6");
                        }
                        else
                        {
                            btn.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                        }
                    }
                }
                else if (globalModel.PipeType == 2)
                {
                    this.tbNowStep.Text = "尺寸设置";
                    this.tbFinishStep.Text = "第4项，共6项";
                    this.gdCollarType.Visibility = Visibility.Visible;
                    foreach (Button btn in GlobalMethed.FindVisualChildren<Button>(this.gdCollarType))
                    {
                        if (btn.Tag.ToString() == globalModel.PipeSize.ToString())
                        {
                            btn.Background = (Brush)bc.ConvertFrom("#72C9F6");
                        }
                        else
                        {
                            btn.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                        }
                    }
                }
                else
                {
                    this.tbTips.Visibility = Visibility.Visible;
                    this.tbTips.Text = "请返回上一步设置钻具类型";
                }
            }
            else if (step == 4)
            {
                this.tbNowStep.Text = "目的地设置";
                this.tbFinishStep.Text = "第5项，共6项";
                this.gdDesType.Visibility = Visibility.Visible;
                if (globalModel.DesType == 1)
                {
                    this.btnRooting.Background = (Brush)bc.ConvertFrom("#72C9F6");
                    this.btnCatToWell.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnCatToMouse.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                }
                else if (globalModel.DesType == 2)
                {
                    this.btnRooting.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnCatToWell.Background = (Brush)bc.ConvertFrom("#72C9F6");
                    this.btnCatToMouse.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                }
                else if (globalModel.DesType == 3)
                {
                    this.btnRooting.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnCatToWell.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnCatToMouse.Background = (Brush)bc.ConvertFrom("#72C9F6");
                }
            }
            else if (step == 5)
            {
                this.tbNowStep.Text = "工作指梁设置";
                this.tbFinishStep.Text = "第6项，共6项";
                this.gdSelectedAutoOrHand.Visibility = Visibility.Visible;
                if (globalModel.SelectDrill == -1)
                {
                    this.btnAutoSelected.Background = (Brush)bc.ConvertFrom("#72C9F6");
                    this.btnHandSelected.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                }
                else if(globalModel.SelectDrill>0)
                {
                    this.btnAutoSelected.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                    this.btnHandSelected.Background = (Brush)bc.ConvertFrom("#72C9F6");
                }
            }
            else if (step == 6)
            {
                this.tbNowStep.Text = "确认设置";
                this.tbFinishStep.Text = "第6项，共6项";
                this.gdConfirm.Visibility = Visibility.Visible;
                this.btnNext.Content = "完成";

                if (globalModel.HS_PumpType == 1) this.tbConfirmHS.Text = "液压站:1#主泵";
                else if (globalModel.HS_PumpType == 2) this.tbConfirmHS.Text = "液压站:2#主泵";
                else if (globalModel.HS_PumpType == 3) this.tbConfirmHS.Text = "液压站:双泵";
                else this.tbConfirmHS.Text = "液压站:未选择";

                if (globalModel.WorkType == 1) this.tbConfirmWorkModel.Text = "工况:排杆";
                else if (globalModel.WorkType == 2) this.tbConfirmWorkModel.Text = "工况:送杆";
                else this.tbConfirmWorkModel.Text = "工况:未选择";

                if (globalModel.PipeType == 1)
                {
                    foreach (Button btn in GlobalMethed.FindVisualChildren<Button>(this.gdPipeType))
                    {
                        if (btn.Tag.ToString() == globalModel.PipeSize.ToString())
                        {
                            this.tbConfirmPipe.Text = "管柱:" + btn.Content + "钻杆";
                            break;
                        }
                    }
                }
                else if (globalModel.PipeType == 2)
                {
                    foreach (Button btn in GlobalMethed.FindVisualChildren<Button>(this.gdCollarType))
                    {
                        if (btn.Tag.ToString() == globalModel.PipeSize.ToString())
                        {
                            this.tbConfirmPipe.Text = "管柱:" + btn.Content + "钻铤";
                            break;
                        }
                    }
                }
                else
                {
                    this.tbConfirmPipe.Text = "管柱:未选择";
                }

                if (globalModel.DesType == 1) this.tbConfirmDes.Text = "目的地:立根盒";
                else if (globalModel.DesType == 2) this.tbConfirmDes.Text = "目的地:井口-猫道";
                else if (globalModel.DesType == 3) this.tbConfirmDes.Text = "目的地:猫道-鼠道";
                else this.tbConfirmDes.Text = "目的地:未选择";

                if (globalModel.SelectDrill == -1)
                {
                    this.tbConfirmHandOrAuto.Text = "指梁选择:自动";
                    this.tbConfirmSelectedDrill.Text = "已选指梁:自动";
                }
                else if (globalModel.SelectDrill > 0)
                {
                    this.tbConfirmHandOrAuto.Text = "指梁选择:手动";
                    this.tbConfirmSelectedDrill.Text = "已选指梁:" + this.tbSelectedDrill.Text;
                }
                else
                {
                    this.tbConfirmHandOrAuto.Text = "指梁选择:未选择";
                    this.tbConfirmSelectedDrill.Text = "已选指梁:未选择";
                }
                
            }
        }

        private void AminationNew_SetDrillNumEvent(byte number)
        {
            if (number < 16)
            {
                if (globalModel.PipeType == 2)
                {
                    MessageBox.Show("管柱类型为钻铤，请选择钻铤行!");
                    return;
                }
                this.tbSelectedDrill.Text = "左" + number.ToString();
            }
            else if (number == 16)
            {
                if (globalModel.PipeType == 1)
                {
                    MessageBox.Show("管柱类型为钻杆，请选择钻杆行!");
                    return;
                }
                this.tbSelectedDrill.Text = "左钻铤";
            }
            else if (number > 16 && number < 32)
            {
                if (globalModel.PipeType == 2)
                {
                    MessageBox.Show("管柱类型为钻铤，请选择钻铤行!");
                    return;
                }
                this.tbSelectedDrill.Text = "右" + (number - 16).ToString();
            }
            else if (number == 32)
            {
                if (globalModel.PipeType == 1)
                {
                    MessageBox.Show("管柱类型为钻杆，请选择钻杆行!");
                    return;
                }
                this.tbSelectedDrill.Text = "右钻铤";
            }
            else this.tbSelectedDrill.Text = "未选择";

            globalModel.SelectDrill = number;
        }
        /// <summary>
        /// 设置主泵1
        /// </summary>
        private void SelectedPumpOne_Click(object sender, RoutedEventArgs e)
        {
            globalModel.HS_PumpType = 1;
            ShowStep(1);
        }
        /// <summary>
        /// 设置主泵2
        /// </summary>
        private void SelectedPumpTwo_Click(object sender, RoutedEventArgs e)
        {
            globalModel.HS_PumpType = 2;
            ShowStep(1);
        }
        /// <summary>
        /// 设置主泵2
        /// </summary>
        private void SelectedPumpBoth_Click(object sender, RoutedEventArgs e)
        {
            globalModel.HS_PumpType = 3;
            ShowStep(1);
        }
        /// <summary>
        /// 设置排杆
        /// </summary>
        private void DrillUp_Click(object sender, RoutedEventArgs e)
        {
            globalModel.WorkType = 2;
            ShowStep(2);
        }
        /// <summary>
        /// 设置送杆
        /// </summary>
        private void DrillDown_Click(object sender, RoutedEventArgs e)
        {
            globalModel.WorkType = 1;
            ShowStep(2);
        }
        /// <summary>
        /// 设置钻杆类型
        /// </summary>
        private void Pipe_Click(object sender, RoutedEventArgs e)
        {
            globalModel.PipeType = 1;
            ShowStep(3);
        }
        /// <summary>
        /// 设置钻铤类型
        /// </summary>
        private void Collar_Click(object sender, RoutedEventArgs e)
        {
            globalModel.PipeType = 2;
            ShowStep(3);
        }
        /// <summary>
        /// 设置钻铤尺寸
        /// </summary>
        private void CollarSize_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int size = int.Parse(btn.Tag.ToString());
            globalModel.PipeSize = size;
            ShowStep(4);
        }
        /// <summary>
        /// 设置钻杆尺寸
        /// </summary>
        private void PipeSize_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int size = int.Parse(btn.Tag.ToString());
            globalModel.PipeSize = size;
            ShowStep(4);
        }
        /// <summary>
        /// 目的地设置-立根盒
        /// </summary>
        private void Rooting_Click(object sender, RoutedEventArgs e)
        {
            globalModel.DesType = 1;
            ShowStep(5);
        }
        /// <summary>
        /// 目的地设置-猫道-井口
        /// </summary>
        private void CatToWell_Click(object sender, RoutedEventArgs e)
        {
            globalModel.DesType = 2;
            ShowStep(5);
        }
        /// <summary>
        /// 目的地设置-猫道-鼠洞
        /// </summary>
        private void CatToMouse_Click(object sender, RoutedEventArgs e)
        {
            globalModel.DesType = 3;
            ShowStep(5);
        }
        /// <summary>
        /// 手动选择
        /// </summary>
        private void Hand_Click(object sender, RoutedEventArgs e)
        {
            this.gdSelectedAutoOrHand.Visibility = Visibility.Collapsed;
            this.tbNowStep.Text = "工作指梁设置";
            this.tbFinishStep.Text = "第6项，共6项";
            this.gdSelectedDrill.Visibility = Visibility.Visible;
            this.aminationNew.InitRowsColoms(COM.Common.SystemType.SecondFloor);
            this.aminationNew.SetDrillNumEvent += AminationNew_SetDrillNumEvent;
        }
        /// <summary>
        /// 自动选择指梁
        /// </summary>
        private void Auto_Click(object sender, RoutedEventArgs e)
        {
            globalModel.SelectDrill = -1;
            ShowStep(6);
        }
        /// <summary>
        /// 上一步
        /// </summary>
        private void btnPre_Click(object sender, RoutedEventArgs e)
        {
            this.Step -= 1;
            if (this.Step < 0) this.Step = 0;
            ShowStep(this.Step);
        }
        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Content.ToString() == "完成")
            {
                if (IsModify)
                {
                    string sql = string.Format("Update GlobalModel Set HS_PumpType='{0}',WorkType='{1}',PipeType='{2}',PipeSize='{3}',DesType='{4}',SelectDrill='{5}',Selected='{6}'" +
                        "Where ID='{7}'", globalModel.HS_PumpType, globalModel.WorkType, globalModel.PipeType, globalModel.PipeSize, globalModel.DesType, globalModel.SelectDrill, false,globalModel.ID);
                    DataHelper.Instance.ExecuteNonQuery(sql);
                }
                else
                {
                    string sql = string.Format("Insert Into GlobalModel(HS_PumpType,WorkType,PipeType,PipeSize,DesType,SelectDrill,Selected) " +
                        "Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", globalModel.HS_PumpType, globalModel.WorkType, globalModel.PipeType, globalModel.PipeSize, globalModel.DesType, globalModel.SelectDrill, false);
                    DataHelper.Instance.ExecuteNonQuery(sql);
                }
                this.Close();
            }
            this.Step += 1;
            if (this.Step > 6) this.Step = 6;
            ShowStep(this.Step);
        }
    }
}
