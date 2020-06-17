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

namespace ControlLibrary
{
    /// <summary>
    /// SliderRotation.xaml 的交互逻辑
    /// </summary>
    public partial class SliderRotation : UserControl
    {
        #region 私有属性

        private Point cen;   //中心点
        private Point first;
        private Point second;

        private bool flag = false;

        private double angle;   //旋钮实际旋转角度

        private double turns = 0;  //旋钮旋转圈数

        private double valueChangeToChange;   //旋钮当前旋转角度，正数为顺时针 负数为逆时针

        #endregion

        #region 公有属性

        /// <summary>
        /// 获取或设置Value数值
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty;

        /// <summary>
        /// 获取或设置最小旋转角度
        /// </summary>
        public double SmallAngle
        {
            get { return (double)GetValue(SmallAngleProperty); }
            set { SetValue(SmallAngleProperty, value); }
        }
        public static readonly DependencyProperty SmallAngleProperty;

        /// <summary>
        /// 获取或设置最小旋转角度代表的Value数值
        /// </summary>
        public double SmallChange
        {
            get { return (double)GetValue(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }
        public static readonly DependencyProperty SmallChangeProperty;

        /// <summary>
        /// 获取或设置最大Value数值
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty;

        /// <summary>
        /// 获取或设置最小Value数值
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty;

        #endregion

        public SliderRotation()
        {
            InitializeComponent();
        }

        static SliderRotation()
        {
            ValueProperty = DependencyProperty.RegisterAttached(
                "Value",
                typeof(double),
                typeof(SliderRotation),
                new FrameworkPropertyMetadata((double)0, new PropertyChangedCallback(ValuePropertyChangedCallback)));
            SmallAngleProperty = DependencyProperty.RegisterAttached(
                "SmallAngle",
                typeof(double),
                typeof(SliderRotation),
                new FrameworkPropertyMetadata((double)10, new PropertyChangedCallback(SmallAnglePropertyChangedCallback)));
            SmallChangeProperty = DependencyProperty.RegisterAttached(
                "SmallChange",
                typeof(double),
                typeof(SliderRotation),
                new FrameworkPropertyMetadata((double)1));
            MaximumProperty = DependencyProperty.RegisterAttached(
                "Maximum",
                typeof(double),
                typeof(SliderRotation),
                new FrameworkPropertyMetadata((double)36));
            MinimumProperty = DependencyProperty.RegisterAttached(
                "Minimum",
                typeof(double),
                typeof(SliderRotation),
                new FrameworkPropertyMetadata((double)0));
        }

        private static void ValuePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if (sender != null && sender is SliderRotation)
            {
                SliderRotation sliderRotation = sender as SliderRotation;
                RoutedPropertyChangedEventArgs<object> valueArg =
                    new RoutedPropertyChangedEventArgs<object>(arg.OldValue, arg.NewValue, ValueChangedEvent);
                sliderRotation.RaiseEvent(valueArg);

                if (sliderRotation.valueChangeToChange > 0)
                {
                    RoutedPropertyChangedEventArgs<object> valueAddArg =
                        new RoutedPropertyChangedEventArgs<object>(arg.OldValue, arg.NewValue, ValueAddEvent);
                    sliderRotation.RaiseEvent(valueAddArg);

                }
                else if (sliderRotation.valueChangeToChange < 0)
                {
                    RoutedPropertyChangedEventArgs<object> valueReducedArg =
                       new RoutedPropertyChangedEventArgs<object>(arg.OldValue, arg.NewValue, ValueReducedEvent);
                    sliderRotation.RaiseEvent(valueReducedArg);
                }
            }
        }

        private static void SmallAnglePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            //if (sender != null && sender is SliderRotation)
            //{
            //    SliderRotation sliderRotation = sender as SliderRotation;
            //    double SmallellipesStaff = (sliderRotation.ellipesStaff.Height + sliderRotation.ellipesStaff.Width) * sliderRotation.SmallAngle / 720;
            //    DoubleCollection doubleCollection = new DoubleCollection();
            //    doubleCollection.Add(0.5);
            //    doubleCollection.Add(SmallellipesStaff - 0.45);
            //    sliderRotation.ellipesStaff.StrokeDashArray = doubleCollection;
            //}
        }


        #region 事件路由

        public event RoutedPropertyChangedEventHandler<object> ValueChanged
        {
            add
            {
                this.AddHandler(ValueChangedEvent, value);
            }

            remove
            {
                this.RemoveHandler(ValueChangedEvent, value);
            }
        }

        public event RoutedPropertyChangedEventHandler<object> ValueAdd
        {
            add
            {
                this.AddHandler(ValueAddEvent, value);
            }

            remove
            {
                this.RemoveHandler(ValueAddEvent, value);
            }
        }

        public event RoutedPropertyChangedEventHandler<object> ValueReduced
        {
            add
            {
                this.AddHandler(ValueReducedEvent, value);
            }

            remove
            {
                this.RemoveHandler(ValueReducedEvent, value);
            }
        }


        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<object>),
            typeof(SliderRotation));

        public static readonly RoutedEvent ValueAddEvent = EventManager.RegisterRoutedEvent(
            "ValueAdd",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<object>),
            typeof(SliderRotation));

        public static readonly RoutedEvent ValueReducedEvent = EventManager.RegisterRoutedEvent(
            "ValueReduced",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<object>),
            typeof(SliderRotation));

        #endregion


        private double GetAngle(Point point)     //获取点到中心的角度      构造平面直角坐标系 计算点在该坐标系与y轴（正方向）的夹角
        {
            const double M_PI = 3.1415926535897;
            if (point.X >= 0)
            {
                double hypotenuse = Math.Sqrt(point.X * point.X + point.Y * point.Y);
                return Math.Acos(point.Y / hypotenuse) * 180 / M_PI;
            }
            else
            {
                double hypotenuse = Math.Sqrt(point.X * point.X + point.Y * point.Y);
                return 360 - Math.Acos(point.Y / hypotenuse) * 180 / M_PI;
            }
        }

        private void ellipseBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            flag = true;
            cen = new Point(ellipseBack.Width / 2, ellipseBack.Height / 2);
            first = new Point(e.GetPosition(canvas).X - cen.X, cen.Y - e.GetPosition(canvas).Y);
        }


        private void ellipseBack_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            flag = false;
            angle = rotate.Angle;
        }

        private void ellipseBack_MouseLeave(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            flag = false;
            angle = rotate.Angle;
        }

        private void ellipseBack_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;        //停止路由事件向上传递

            if (flag)
            {
                second = new Point(e.GetPosition(canvas).X - cen.X, cen.Y - e.GetPosition(canvas).Y);    //确定鼠标移动的点坐标（相对中心点的位置）

                if (second == new Point(0, 0))
                {
                    return;
                }

                double anglePointToPoint = GetAngle(second) - GetAngle(first);        //得到鼠标移动之前与鼠标移动之后之间的夹角

                first = second;

                if (Math.Abs(anglePointToPoint) > 90)                               //夹角如果大于90度忽略(大于90度的夹角有可能是计算错误得出来的)
                {
                    anglePointToPoint = 0;
                }

                angle += anglePointToPoint;                                       //

                valueChangeToChange = (int)((angle - rotate.Angle) / SmallAngle) * SmallChange;    //根据角度计算出夹角的数值

                if (valueChangeToChange != 0)
                {
                    rotate.Angle = angle - angle % SmallAngle;                                  //计算出旋转角度

                    double value = (Value + valueChangeToChange) - (Value + valueChangeToChange) % SmallChange;   //计算出属性值

                    if (value < Minimum)                                      //属性值小于最小值 则 赋予最小值
                    {
                        Value = Minimum;
                    }
                    else if (value > Maximum)                                 //属性值大于最大值 则 赋予最大值
                    {
                        Value = Maximum;
                    }
                    else
                    {
                        Value = value;
                    }

                    if (angle > 360 && rotate.Angle > 360)                     //旋钮旋转超过 360度 或小于零度 将该角度计算到圈数中 
                    {
                        angle -= 360;
                        rotate.Angle -= 360;
                        turns++;
                    }
                    else if (angle < 0 && rotate.Angle < 360)
                    {
                        angle += 360;
                        rotate.Angle += 360;
                        turns--;
                    }
                }
            }
        }
    }
}
