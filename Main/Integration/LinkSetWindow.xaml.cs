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
    /// LinkSetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LinkSetWindow : Window
    {
        public GlobalModel globalModel = new GlobalModel();
        /// <summary>
        /// 当前步骤 0-液压站；1-工作模式；2-管柱类型；3-管柱尺寸选择；4-目的地；5-指梁选择;6-确认配置
        /// </summary>
        private int Step { get; set; } = 0;
        public LinkSetWindow()
        {
            InitializeComponent();
            this.Loaded += LinkSetWindow_Loaded; 
        }

        private void LinkSetWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //ShowStep(0);
        }
        BrushConverter bc = new BrushConverter();
        /// <summary>
        /// 显示对应步骤
        /// </summary>
        /// <param name="step">当前步骤</param>
        public void ShowStep(int step)
        {
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
            if (step == 0)
            {
                this.tbNowStep.Text = "液压站设置";
                this.gdHSSet.Visibility = Visibility.Visible;
                if (globalModel.HS_PumpType == 1)
                {
                    this.btnPumpOne.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                    this.btnPumpTwo.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnPumpBoth.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.tbConfirmHS.Text = "液压站:1#主泵";
                }
                else if (globalModel.HS_PumpType == 2)
                {
                    this.btnPumpOne.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnPumpTwo.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                    this.btnPumpBoth.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.tbConfirmHS.Text = "液压站:2#主泵";
                }
                else if (globalModel.HS_PumpType == 3)
                {
                    this.btnPumpOne.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnPumpTwo.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnPumpBoth.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                    this.tbConfirmHS.Text = "液压站:双泵";
                }
            }
            else if (step == 1)
            {
                this.tbNowStep.Text = "工作模式设置";
                this.gdWorkModel.Visibility = Visibility.Visible;
                if (globalModel.WorkType == 1)
                {
                    this.btnDrillUp.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnDrillDown.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                    this.tbConfirmWorkModel.Text = "工况:送杆";
                }
                else if (globalModel.WorkType == 2)
                {
                    this.btnDrillUp.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                    this.btnDrillDown.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.tbConfirmWorkModel.Text = "工况:排杆";
                }
            }
            else if (step == 2)
            {
                this.tbNowStep.Text = "钻具类型设置";
                this.gdPipeOrCollar.Visibility = Visibility.Visible;
                if (globalModel.PipeType == 1)
                {
                    this.btnPipe.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                    this.btnCollar.Foreground = (Brush)bc.ConvertFrom("#000000");
                }
                else if (globalModel.PipeType == 2)
                {
                    this.btnPipe.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnCollar.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                }
            }
            else if (step == 3)
            {
                if (globalModel.PipeType == 1)
                {
                    this.tbNowStep.Text = "尺寸设置";
                    this.gdPipeType.Visibility = Visibility.Visible;
                    foreach (Button btn in GlobalMethed.FindVisualChildren<Button>(this.gdPipeType))
                    {
                        if (btn.Tag.ToString() == globalModel.PipeSize.ToString())
                        {
                            btn.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                        }
                        else
                        {
                            btn.Foreground = (Brush)bc.ConvertFrom("#000000");
                        }
                    }
                }
                else if (globalModel.PipeType == 2)
                {
                    this.tbNowStep.Text = "尺寸设置";
                    this.gdCollarType.Visibility = Visibility.Visible;
                    foreach (Button btn in GlobalMethed.FindVisualChildren<Button>(this.gdCollarType))
                    {
                        if (btn.Tag.ToString() == globalModel.PipeSize.ToString())
                        {
                            btn.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                        }
                        else
                        {
                            btn.Foreground = (Brush)bc.ConvertFrom("#000000");
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
                this.gdDesType.Visibility = Visibility.Visible;
                if (globalModel.DesType == 1)
                {
                    this.btnRooting.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                    this.btnCatToWell.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnCatToMouse.Foreground = (Brush)bc.ConvertFrom("#000000");
                }
                else if (globalModel.DesType == 2)
                {
                    this.btnRooting.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnCatToWell.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                    this.btnCatToMouse.Foreground = (Brush)bc.ConvertFrom("#000000");
                }
                else if (globalModel.DesType == 3)
                {
                    this.btnRooting.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnCatToWell.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnCatToMouse.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                }
            }
            else if (step == 5)
            {
                this.tbNowStep.Text = "工作指梁设置";
                this.gdSelectedAutoOrHand.Visibility = Visibility.Visible;
                if (globalModel.SelectDrill == -1)
                {
                    this.btnAutoSelected.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                    this.btnHandSelected.Foreground = (Brush)bc.ConvertFrom("#000000");
                }
                else if (globalModel.SelectDrill > 0)
                {
                    this.btnAutoSelected.Foreground = (Brush)bc.ConvertFrom("#000000");
                    this.btnHandSelected.Foreground = (Brush)bc.ConvertFrom("#2B71CA");
                }
            }
            else if (step == 6)
            {
                this.tbNowStep.Text = "工作指梁设置";
                this.gdSelectedDrill.Visibility = Visibility.Visible;
                this.aminationNew.InitRowsColoms(COM.Common.SystemType.SecondFloor);
                this.aminationNew.SetDrillNumEvent += AminationNew_SetDrillNumEvent;
                this.aminationNew.ShwoSelectDrill((byte)globalModel.SelectDrill);
                AminationNew_SetDrillNumEvent((byte)globalModel.SelectDrill);
                //this.tbNowStep.Text = "确认设置";
                //this.gdConfirm.Visibility = Visibility.Visible;
                //this.btnNext.Content = "完成";

                //if (globalModel.HS_PumpType == 1) this.tbConfirmHS.Text = "液压站:1#主泵";
                //else if (globalModel.HS_PumpType == 2) this.tbConfirmHS.Text = "液压站:2#主泵";
                //else if (globalModel.HS_PumpType == 3) this.tbConfirmHS.Text = "液压站:双泵";
                //else this.tbConfirmHS.Text = "液压站:未选择";

                //if (globalModel.WorkType == 1) this.tbConfirmWorkModel.Text = "工况:送杆";
                //else if (globalModel.WorkType == 2) this.tbConfirmWorkModel.Text = "工况:排杆";
                //else this.tbConfirmWorkModel.Text = "工况:未选择";

                //if (globalModel.PipeType == 1)
                //{
                //    foreach (Button btn in GlobalMethed.FindVisualChildren<Button>(this.gdPipeType))
                //    {
                //        if (btn.Tag.ToString() == globalModel.PipeSize.ToString())
                //        {
                //            this.tbConfirmPipe.Text = "管柱:" + btn.Content + "钻杆";
                //            break;
                //        }
                //    }
                //}
                //else if (globalModel.PipeType == 2)
                //{
                //    foreach (Button btn in GlobalMethed.FindVisualChildren<Button>(this.gdCollarType))
                //    {
                //        if (btn.Tag.ToString() == globalModel.PipeSize.ToString())
                //        {
                //            this.tbConfirmPipe.Text = "管柱:" + btn.Content + "钻铤";
                //            break;
                //        }
                //    }
                //}
                //else
                //{
                //    this.tbConfirmPipe.Text = "管柱:未选择";
                //}

                //if (globalModel.DesType == 1) this.tbConfirmDes.Text = "目的地:立根盒";
                //else if (globalModel.DesType == 2) this.tbConfirmDes.Text = "目的地:井口-猫道";
                //else if (globalModel.DesType == 3) this.tbConfirmDes.Text = "目的地:猫道-鼠道";
                //else this.tbConfirmDes.Text = "目的地:未选择";

                //if (globalModel.SelectDrill == -1)
                //{
                //    this.tbConfirmHandOrAuto.Text = "指梁选择:自动";
                //    this.tbConfirmSelectedDrill.Text = "已选指梁:自动";
                //}
                //else if (globalModel.SelectDrill > 0)
                //{
                //    this.tbConfirmHandOrAuto.Text = "指梁选择:手动";
                //    this.tbConfirmSelectedDrill.Text = "已选指梁:" + this.tbSelectedDrill.Text;
                //}
                //else
                //{
                //    this.tbConfirmHandOrAuto.Text = "指梁选择:未选择";
                //    this.tbConfirmSelectedDrill.Text = "已选指梁:未选择";
                //}

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
            this.btnPumpOne.Background = (Brush)bc.ConvertFrom("#2B71CA");
            this.btnPumpTwo.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnPumpBoth.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            //ShowStep(1);
        }
        /// <summary>
        /// 设置主泵2
        /// </summary>
        private void SelectedPumpTwo_Click(object sender, RoutedEventArgs e)
        {
            globalModel.HS_PumpType = 2;
            this.btnPumpOne.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnPumpTwo.Background = (Brush)bc.ConvertFrom("#2B71CA");
            this.btnPumpBoth.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            //ShowStep(1);
        }
        /// <summary>
        /// 设置主泵2
        /// </summary>
        private void SelectedPumpBoth_Click(object sender, RoutedEventArgs e)
        {
            globalModel.HS_PumpType = 3;
            this.btnPumpOne.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnPumpTwo.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnPumpBoth.Background = (Brush)bc.ConvertFrom("#2B71CA");
            //ShowStep(1);
        }
        /// <summary>
        /// 设置排杆
        /// </summary>
        private void DrillUp_Click(object sender, RoutedEventArgs e)
        {
            this.btnDrillUp.Background = (Brush)bc.ConvertFrom("#2B71CA");
            this.btnDrillDown.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            globalModel.WorkType = 2;
            //ShowStep(2);
        }
        /// <summary>
        /// 设置送杆
        /// </summary>
        private void DrillDown_Click(object sender, RoutedEventArgs e)
        {
            this.btnDrillUp.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnDrillDown.Background = (Brush)bc.ConvertFrom("#2B71CA");
            globalModel.WorkType = 1;
            //ShowStep(2);
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
            Button button = sender as Button;
            int size = int.Parse(button.Tag.ToString());
            globalModel.PipeSize = size;
            foreach (Button btn in GlobalMethed.FindVisualChildren<Button>(this.gdCollarType))
            {
                if (btn.Tag.ToString() == globalModel.PipeSize.ToString())
                {
                    btn.Background = (Brush)bc.ConvertFrom("#2B71CA");
                }
                else
                {
                    btn.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                }
            }
            //ShowStep(4);
        }
        /// <summary>
        /// 设置钻杆尺寸
        /// </summary>
        private void PipeSize_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int size = int.Parse(button.Tag.ToString());
            globalModel.PipeSize = size;
            foreach (Button btn in GlobalMethed.FindVisualChildren<Button>(this.gdPipeType))
            {
                if (btn.Tag.ToString() == globalModel.PipeSize.ToString())
                {
                    btn.Background = (Brush)bc.ConvertFrom("#2B71CA");
                }
                else
                {
                    btn.Background = (Brush)bc.ConvertFrom("#FFFFFF");
                }
            }
            //ShowStep(4);
        }
        /// <summary>
        /// 目的地设置-立根盒
        /// </summary>
        private void Rooting_Click(object sender, RoutedEventArgs e)
        {
            this.btnRooting.Background = (Brush)bc.ConvertFrom("#2B71CA");
            this.btnCatToWell.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnCatToMouse.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            globalModel.DesType = 1;
            //ShowStep(5);
        }
        /// <summary>
        /// 目的地设置-猫道-井口
        /// </summary>
        private void CatToWell_Click(object sender, RoutedEventArgs e)
        {
            this.btnRooting.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnCatToWell.Background = (Brush)bc.ConvertFrom("#2B71CA");
            this.btnCatToMouse.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            globalModel.DesType = 2;
            //ShowStep(5);
        }
        /// <summary>
        /// 目的地设置-猫道-鼠洞
        /// </summary>
        private void CatToMouse_Click(object sender, RoutedEventArgs e)
        {
            this.btnRooting.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnCatToWell.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            this.btnCatToMouse.Background = (Brush)bc.ConvertFrom("#2B71CA");
            globalModel.DesType = 3;
            //ShowStep(5);
        }
        /// <summary>
        /// 手动选择
        /// </summary>
        private void Hand_Click(object sender, RoutedEventArgs e)
        {
            this.gdSelectedAutoOrHand.Visibility = Visibility.Collapsed;
            this.tbNowStep.Text = "工作指梁设置";
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
            //ShowStep(6);
        }
        /// <summary>
        /// 确认
        /// </summary>
        private void btnPre_Click(object sender, RoutedEventArgs e)
        {
            if (globalModel.ID > 0)
            {
                string sql = string.Format("Update GlobalModel Set HS_PumpType='{0}',WorkType='{1}',PipeType='{2}',PipeSize='{3}',DesType='{4}',SelectDrill='{5}',Selected='{6}'" +
                    "Where ID='{7}'", globalModel.HS_PumpType, globalModel.WorkType, globalModel.PipeType, globalModel.PipeSize, globalModel.DesType, globalModel.SelectDrill, false, globalModel.ID);
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
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnVoltagePump_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnVoltagePump.Background.ToString() == "FF2B71CA")
            {
                globalModel.HS_VoltagePump = 0;
                this.btnVoltagePump.Background = (Brush)bc.ConvertFrom("#FFFFFF");
            }
            else
            {
                globalModel.HS_VoltagePump = 1;
                this.btnVoltagePump.Background = (Brush)bc.ConvertFrom("#2B71CA");
            }
        }
    }
}
