using COM.Common;
using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main.HydraulicStation.Sany
{
    /// <summary>
    /// OilOneWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OilOneWindow : Window
    {
        System.Threading.Timer timerWarning;
        public OilOneWindow()
        {
            InitializeComponent();
            VariableBinding();
            timerWarning = new System.Threading.Timer(new TimerCallback(TimerWarning_Elapsed), this, 2000, 500);//改成50ms 的时钟
        }

        private void VariableBinding()
        {
            try
            {
                #region 处理与mouse与click事件冲突
                btnLeftCatHeadReach.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnLeftCatReach_Click), true);
                btnLeftCatHeadReach.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnLeftCatHeadReach_MouseUp), true);
                btnLeftCatHeadRetract.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnLeftCatRetract_Click), true);
                btnLeftCatHeadRetract.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnLeftCatHeadRetract_MouseUp), true);
                btnRightCatHeadReach.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnRightCatReach_Click), true);
                btnRightCatHeadReach.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnRightCatHeadReach_MouseUp), true);
                btnRightCatHeadRetract.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(BtnRightCatRetract_Click), true);
                btnRightCatHeadRetract.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnRightCatHeadRetract_MouseUp), true);
                btnRotateCatHeadLeft.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(btnRotateCatHeadLeft_Click), true);
                btnRotateCatHeadLeft.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnRotateCatHeadLeft_MouseUp), true);
                btnRotateCatHeadRight.AddHandler(Button.MouseDownEvent, new RoutedEventHandler(btnRotateCatHeadRight_Click), true);
                btnRotateCatHeadRight.AddHandler(Button.MouseUpEvent, new RoutedEventHandler(btnRotateCatHeadRight_MouseUp), true);
                #endregion
                //this.Iron.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b1"], Mode = BindingMode.OneWay });
                //this.Tongs.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b3"], Mode = BindingMode.OneWay });
                //this.DF.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b3"], Mode = BindingMode.OneWay });
                //this.BufferArm.SetBinding(BasedSwitchButton.IsCheckedProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b7"], Mode = BindingMode.OneWay });

                this.btnLeftCatHeadRetract.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnLeftCatHeadClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnLeftCatHeadReach.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                this.btnRightCatHeadRetract.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRightCatHeadClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRightCatHeadReach.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                //this.btnIron.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnIronCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnIronCloseMultiBind = new MultiBinding();
                btnIronCloseMultiBind.Converter = btnIronCloseMultiConverter;
                btnIronCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["768b1"], Mode = BindingMode.OneWay });
                btnIronCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["768b3"], Mode = BindingMode.OneWay });
                btnIronCloseMultiBind.NotifyOnSourceUpdated = true;
                //this.btnIronClose.SetBinding(Button.BackgroundProperty, btnIronCloseMultiBind);
                //this.btnTong.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["768b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                //this.btnDF.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnDFCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnDFCloseMultiBind = new MultiBinding();
                btnDFCloseMultiBind.Converter = btnDFCloseMultiConverter;
                btnDFCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b3"], Mode = BindingMode.OneWay });
                btnDFCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b5"], Mode = BindingMode.OneWay });
                btnDFCloseMultiBind.NotifyOnSourceUpdated = true;
                //this.btnDFClose.SetBinding(Button.BackgroundProperty, btnDFCloseMultiBind);
                //this.btnSpaceThree.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                //this.btnWellBuffer.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                BtnColorMuilCoverter btnWellBufferCloseMultiConverter = new BtnColorMuilCoverter();
                MultiBinding btnWellBufferCloseMultiBind = new MultiBinding();
                btnWellBufferCloseMultiBind.Converter = btnWellBufferCloseMultiConverter;
                btnWellBufferCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["769b7"], Mode = BindingMode.OneWay });
                btnWellBufferCloseMultiBind.Bindings.Add(new Binding("BoolTag") { Source = GlobalData.Instance.da["770b1"], Mode = BindingMode.OneWay });
                btnWellBufferCloseMultiBind.NotifyOnSourceUpdated = true;
                //this.btnWellBufferClose.SetBinding(Button.BackgroundProperty, btnWellBufferCloseMultiBind);
                //this.btnSpaceFour.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });

                this.tbLeftCatHead.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["CatPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.tbRightCatHead.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["CatPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.tbIronTongs.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["IRPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                //this.tbDFSpThree.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["FPPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });

                this.btnElevatorOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnElevatorClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.tbMainPumpUnload.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["MPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.btnCraneOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b2"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnCraneClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b3"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                //this.tbKavaUnload.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["SlipPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.btnWellMoveOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnWellMoveClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.tbWellMove.SetBinding(TextBlock.TextProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["TPressAI"], Mode = BindingMode.OneWay, Converter = new DivideTenConverter() });
                this.btnSpareOneOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnSpareOneClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["767b7"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnLeftCarOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnLeftCarClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["769b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRightCarOpen.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b0"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRightCarClose.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["770b1"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRotateCatHeadLeft.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["794b4"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRotateCatHeadStop.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["794b6"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
                this.btnRotateCatHeadRight.SetBinding(Button.BackgroundProperty, new Binding("BoolTag") { Source = GlobalData.Instance.da["794b5"], Mode = BindingMode.OneWay, Converter = new BtnColorCoverter() });
            }
            catch (Exception ex)
            {
                Log4Net.AddLog(ex.StackTrace, InfoLevel.ERROR);
            }
        }

        bool leftCatReach = false;
        bool leftCatRetract = false;
        bool rightCatReach = false;
        bool rightCatRetract = false;
        bool rotateCatLeft = false;
        bool rotateCatRight = false;
        /// <summary>
        /// 循环发送命令
        /// </summary>
        private void SendByCycle()
        {
            if (leftCatReach) // 左猫头伸
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 4, 4, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 21, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            if (leftCatRetract) // 左猫头缩
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 4, 5, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 21, 0, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            if (rightCatReach) // 右猫头伸
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 4, 7, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 22, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            if (rightCatRetract) // 右猫头缩
            {
                //byte[] byteToSend = new byte[10] { 0, 19, 4, 8, 0, 0, 0, 0, 0, 0 };
                byte[] byteToSend = new byte[10] { 0, 19, 22, 0, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }

            if (rotateCatLeft) // 旋转猫头左转
            {
                byte[] byteToSend = new byte[10] { 0, 19, 23, 0, 1, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
            if (rotateCatRight) // 旋转猫头右转
            {
                byte[] byteToSend = new byte[10] { 0, 19, 23, 0, 2, 0, 0, 0, 0, 0 };
                GlobalData.Instance.da.SendBytes(byteToSend);
            }
        }

        private void TimerWarning_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    SendByCycle();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 旋转猫头停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRotateCatHeadStop_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 23, 0, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 旋转猫头左转-按下
        /// </summary>
        private void btnRotateCatHeadLeft_Click(object sender, RoutedEventArgs e)
        {
            rotateCatLeft = true;
        }
        /// <summary>
        /// 旋转猫头左转-抬起
        /// </summary>
        private void btnRotateCatHeadLeft_MouseUp(object sender, RoutedEventArgs e)
        {
            rotateCatLeft = false;
        }

        /// <summary>
        /// 旋转猫头左转-按下
        /// </summary>
        private void btnRotateCatHeadRight_Click(object sender, RoutedEventArgs e)
        {
            rotateCatRight = true;
        }
        /// <summary>
        /// 旋转猫头左转-抬起
        /// </summary>
        private void btnRotateCatHeadRight_MouseUp(object sender, RoutedEventArgs e)
        {
            rotateCatRight = false;
        }

        /// <summary>
        /// 吊卡打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnElevatorOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 9, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 吊卡关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnElevatorClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 10, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 吊机打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCraneOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 11, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 吊机关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCraneClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 12, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井架平移打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWellMoveOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 13, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 井架平移关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWellMoveClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 14, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 备用油源1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSpareOneOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 15, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btnSpareOneClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 16, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 备用油源2打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeftCarOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 29, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 备用油源2关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeftCarClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 30, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 备用油源3打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRightCarOpen_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 33, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 备用油源3关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRightCarClose_Click(object sender, RoutedEventArgs e)
        {
            byte[] byteToSend = new byte[10] { 0, 19, 34, 0, 1, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        /// <summary>
        /// 左猫头伸
        /// </summary>
        private void BtnLeftCatReach_Click(object sender, RoutedEventArgs e)
        {
            leftCatReach = true;
        }

        //private void BtnLeftCatReach_Click(object sender, MouseButtonEventArgs e)
        //{
        //    byte[] byteToSend = new byte[10] { 0, 19, 4, 4, 0, 0, 0, 0, 0, 0 };
        //    GlobalData.Instance.da.SendBytes(byteToSend);
        //}
        /// <summary>
        /// 左猫头伸-按键抬起关闭
        /// </summary>
        private void btnLeftCatHeadReach_MouseUp(object sender, RoutedEventArgs e)
        {
            leftCatReach = false;
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 6, 0, 0, 0, 0, 0, 0 };
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左猫头缩
        /// </summary>
        private void BtnLeftCatRetract_Click(object sender, RoutedEventArgs e)
        {
            leftCatRetract = true;
        }
        /// <summary>
        /// 左猫头缩-按键抬起关闭
        /// </summary>
        private void btnLeftCatHeadRetract_MouseUp(object sender, RoutedEventArgs e)
        {
            leftCatRetract = false;
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 6, 0, 0, 0, 0, 0, 0 };
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 左猫头关
        /// </summary>
        private void BtnLeftCatClose_Click(object sender, RoutedEventArgs e)
        {
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 5, 0, 0, 0, 0, 0, 0 };
            byte[] byteToSend = new byte[10] { 0, 19, 21, 0, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头伸
        /// </summary>
        private void BtnRightCatReach_Click(object sender, RoutedEventArgs e)
        {
            rightCatReach = true;
        }
        /// <summary>
        /// 右猫头伸-抬起关闭
        /// </summary>
        private void btnRightCatHeadReach_MouseUp(object sender, RoutedEventArgs e)
        {
            rightCatReach = false;
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 9, 0, 0, 0, 0, 0, 0 };
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头缩
        /// </summary>
        private void BtnRightCatRetract_Click(object sender, RoutedEventArgs e)
        {
            rightCatRetract = true;
        }
        /// <summary>
        /// 右猫头缩-按键抬起关闭
        /// </summary>
        private void btnRightCatHeadRetract_MouseUp(object sender, RoutedEventArgs e)
        {
            rightCatRetract = false;
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 9, 0, 0, 0, 0, 0, 0 };
            //GlobalData.Instance.da.SendBytes(byteToSend);
        }
        /// <summary>
        /// 右猫头关
        /// </summary>
        private void BtnRightCatClose_Click(object sender, RoutedEventArgs e)
        {
            //byte[] byteToSend = new byte[10] { 0, 19, 4, 9, 0, 0, 0, 0, 0, 0 };
            byte[] byteToSend = new byte[10] { 0, 19, 22, 0, 0, 0, 0, 0, 0, 0 };
            GlobalData.Instance.da.SendBytes(byteToSend);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
