using COM.Common;
using ControlLibrary;
using DatabaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// WR_SFAmination.xaml 的交互逻辑
    /// </summary>
    public partial class WR_SFAmination : UserControl
    {
        private static WR_SFAmination _instance = null;
        private static readonly object syncRoot = new object();

        public static WR_SFAmination Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new WR_SFAmination();
                        }
                    }
                }
                return _instance;
            }
        }
        private List<BorderNum> sfSelectFingerList = new List<BorderNum>(); // 钻台面对准指梁
        List<DrillModel> FingerBeamDrillPipeCountList = new List<DrillModel>();// 各个指梁所拥有的钻杆数目
        private string configPath = System.Environment.CurrentDirectory + @"\Config.ini";
        //private bool bLoaded = false;
        const int STRINGMAX = 255;
        System.Threading.Timer timer;
        #region 二层台参数

        public delegate void SendFingerBeamNumber(byte number);

        public event SendFingerBeamNumber SendFingerBeamNumberEvent;

        public delegate void SetDrillNum(byte number);

        public event SetDrillNum SetDrillNumEvent;

        // 二层台指梁,用于显示钻杆/钻铤数目
        List<StackPanel> spSF = new List<StackPanel>();

        #endregion

        public int RowsCnt
        {
            get { return rows; }
        }
        private int rows;
        private int coloms;//列数 
        private int drillCnt;//钻铤的个数
        private int sfcarMaxPosistion = 0;
        private int sfcarMinPosistion = 0;
        private int sfarmMaxPosistion = 0;
        private bool showLeftOne = true;
        private bool showRightOne = true;
        private int leftRows = 0;
        private int rightRows = 0;
        #region 设置行高及列
        public static readonly DependencyProperty WR_SFLeftRowHeightProperty = DependencyProperty.Register("WR_SFLeftRowHeight", typeof(double), typeof(WR_SFAmination), new PropertyMetadata((double)0.0));
        /// <summary>
        /// 显示左侧行高
        /// </summary>
        public double WR_SFLeftRowHeight
        {
            get { return (double)GetValue(WR_SFLeftRowHeightProperty); }
            set { SetValue(WR_SFLeftRowHeightProperty, value); }
        }
        public static readonly DependencyProperty WR_SFLeftFirstRowHeightProperty = DependencyProperty.Register("WR_SFLeftFirstRowHeight", typeof(double), typeof(WR_SFAmination), new PropertyMetadata((double)0.0));
        /// <summary>
        /// 显示左侧钻铤行行高
        /// </summary>
        public double WR_SFLeftFirstRowHeight
        {
            get { return (double)GetValue(WR_SFLeftFirstRowHeightProperty); }
            set { SetValue(WR_SFLeftFirstRowHeightProperty, value); }
        }

        public static readonly DependencyProperty WR_SFRightRowHeightProperty = DependencyProperty.Register("WR_SFRightRowHeight", typeof(double), typeof(WR_SFAmination), new PropertyMetadata((double)0.0));
        /// <summary>
        /// 显示左侧行高
        /// </summary>
        public double WR_SFRightRowHeight
        {
            get { return (double)GetValue(WR_SFRightRowHeightProperty); }
            set { SetValue(WR_SFRightRowHeightProperty, value); }
        }
        public static readonly DependencyProperty WR_SFRightFirstRowHeightProperty = DependencyProperty.Register("WR_SFRightFirstRowHeight", typeof(double), typeof(WR_SFAmination), new PropertyMetadata((double)0.0));
        /// <summary>
        /// 显示左侧钻铤行行高
        /// </summary>
        public double WR_SFRightFirstRowHeight
        {
            get { return (double)GetValue(WR_SFRightFirstRowHeightProperty); }
            set { SetValue(WR_SFRightFirstRowHeightProperty, value); }
        }
        #endregion

        public static readonly DependencyProperty WR_RobotGripStatusProperty = DependencyProperty.Register("WR_RobotGripStatus", typeof(byte), typeof(WR_SFAmination), new PropertyMetadata((byte)0)); // 抓手状态
        /// <summary>
        /// 抓手状态
        /// </summary>
        public byte WR_RobotGripStatus
        {
            get { return (byte)GetValue(WR_RobotGripStatusProperty); }
            set { SetValue(WR_RobotGripStatusProperty, value); }
        }
        public static readonly DependencyProperty WR_RobotArmRotateAngleProperty = DependencyProperty.Register("WR_RobotArmRotateAngle", typeof(short), typeof(WR_SFAmination), new PropertyMetadata((short)0)); // 手臂旋转
        /// <summary>
        /// 手臂旋转角度
        /// </summary>
        public short WR_RobotArmRotateAngle
        {
            get { return (short)GetValue(WR_RobotArmRotateAngleProperty); }
            set { SetValue(WR_RobotArmRotateAngleProperty, value); }
        }

        public static readonly DependencyProperty WR_WorkAnimationWidthProperty = DependencyProperty.Register("WR_WorkAnimationWidth", typeof(double), typeof(WR_SFAmination), new PropertyMetadata((double)0.0)); // 手臂最大位移
        /// <summary>
        /// 手臂最大位移
        /// </summary>
        public double WR_WorkAnimationWidth
        {
            get { return (double)GetValue(WR_WorkAnimationWidthProperty); }
            set { SetValue(WR_WorkAnimationWidthProperty, value); }
        }
        public static readonly DependencyProperty WR_RobotArmPositionProperty = DependencyProperty.Register("WR_RobotArmPosition", typeof(short), typeof(WR_SFAmination), new PropertyMetadata((short)0)); // 手臂位置
        /// <summary>
        /// 手臂位置
        /// </summary>
        public short WR_RobotArmPosition
        {
            get { return (short)GetValue(WR_RobotArmPositionProperty); }
            set { SetValue(WR_RobotArmPositionProperty, value); }
        }
        public static readonly DependencyProperty WR_RealHeightProperty = DependencyProperty.Register("WR_RealHeight", typeof(double), typeof(WR_SFAmination), new PropertyMetadata((double)0.0));// 小车运动实际高度
        ///<summary>
        /// 小车位移实际高度
        /// </summary>
        public double WR_RealHeight
        {
            get { return (double)GetValue(WR_RealHeightProperty); }
            set { SetValue(WR_RealHeightProperty, value); }
        }
        public static readonly DependencyProperty WR_MiddleHeightProperty = DependencyProperty.Register("WR_MiddleHeight", typeof(double), typeof(WR_SFAmination), new PropertyMetadata((double)0.0));// 小车运动实际高度

        ///<summary>
        /// 中间高度--减去用于置于零点
        /// </summary>
        public double WR_MiddleHeight
        {
            get { return (double)GetValue(WR_MiddleHeightProperty); }
            set { SetValue(WR_MiddleHeightProperty, value); }
        }
        public static readonly DependencyProperty WR_RobotCarPositionProperty = DependencyProperty.Register("WR_RobotCarPosition", typeof(short), typeof(WR_SFAmination), new PropertyMetadata((short)0)); // 小车位置

        /// <summary>
        /// 小车位置
        /// </summary>
        public short WR_RobotCarPosition
        {
            get { return (short)GetValue(WR_RobotCarPositionProperty); }
            set { SetValue(WR_RobotCarPositionProperty, value); }
        }
        public static readonly DependencyProperty WR_PCFingerBeamNumberFeedBackProperty = DependencyProperty.Register("WR_PCFingerBeamNumberFeedBack", typeof(byte), typeof(WR_SFAmination), new PropertyMetadata((byte)0));//上位机选择的指梁序号反馈
        /// <summary>
        /// 上位机选择的指梁
        /// </summary>
        public byte WR_PCFingerBeamNumberFeedBack
        {
            get { return (byte)GetValue(WR_PCFingerBeamNumberFeedBackProperty); }
            set { SetValue(WR_PCFingerBeamNumberFeedBackProperty, value); }
        }
        public static readonly DependencyProperty WR_CurrentPointFingerBeamNumberProperty = DependencyProperty.Register("WR_CurrentPointFingerBeamNumber", typeof(byte), typeof(WR_SFAmination), new PropertyMetadata((byte)0));//当前所移动到的指梁号反馈
        /// <summary>
        /// 当前指梁号
        /// </summary>
        public byte WR_CurrentPointFingerBeamNumber
        {
            get { return (byte)GetValue(WR_CurrentPointFingerBeamNumberProperty); }
            set { SetValue(WR_CurrentPointFingerBeamNumberProperty, value); }
        }
        public static readonly DependencyProperty WR_OperationModelProperty = DependencyProperty.Register("WR_OperationModel", typeof(byte), typeof(WR_SFAmination), new PropertyMetadata((byte)0));//操作模式 
        /// <summary>
        /// 操作模式
        /// </summary>
        public byte WR_OperationModel
        {
            get { return (byte)GetValue(WR_OperationModelProperty); }
            set { SetValue(WR_OperationModelProperty, value); }
        }
        public WR_SFAmination()
        {
            InitializeComponent();
            VariableBinding();
        }



        /// <summary>
        /// 绑定遍历
        /// </summary>
        private void VariableBinding()
        {
            try
            {
                #region 二层台所有指梁
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow1, Num = 1 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow2, Num = 2 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow3, Num = 3 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow4, Num = 4 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow5, Num = 5 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow6, Num = 6 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow7, Num = 7 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow8, Num = 8 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow9, Num = 9 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow10, Num = 10 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow11, Num = 11 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow12, Num = 12 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow13, Num = 13 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow14, Num = 14 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow15, Num = 15 });

                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow17, Num = 17 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow18, Num = 18 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow19, Num = 19 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow20, Num = 20 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow21, Num = 21 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow22, Num = 22 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow23, Num = 23 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow24, Num = 24 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow25, Num = 25 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow26, Num = 26 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow27, Num = 27 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow28, Num = 28 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow29, Num = 29 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow30, Num = 30 });
                sfSelectFingerList.Add(new BorderNum { SelectBorder = this.sfFingerBeamArrow31, Num = 31 });

                #endregion
                #region 初始化二层台钻杆/钻铤数量
                FingerBeamDrillPipeCountList.Clear();
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "111N2N22N9FingerBeamDrillPipeCount1", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "111N2N22N9FingerBeamDrillPipeCount2", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "111N2N22N9FingerBeamDrillPipeCount3", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "111N2N22N9FingerBeamDrillPipeCount4", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "111N2N22N9FingerBeamDrillPipeCount5", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "111N2N22N9FingerBeamDrillPipeCount6", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "111N2N22N9FingerBeamDrillPipeCount7", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "111N2N22N9FingerBeamDrillPipeCount8", Num = 0 });

                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "112N2N22N9FingerBeamDrillPipeCount9", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "112N2N22N9FingerBeamDrillPipeCount10", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "112N2N22N9FingerBeamDrillPipeCount11", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "112N2N22N9FingerBeamDrillPipeCount12", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "112N2N22N9FingerBeamDrillPipeCount13", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "112N2N22N9FingerBeamDrillPipeCount14", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "112N2N22N9FingerBeamDrillPipeCount15", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "112N2N22N9FingerBeamDrillPipeCount16", Num = 0 });

                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "113N2N22N9FingerBeamDrillPipeCount17", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "113N2N22N9FingerBeamDrillPipeCount18", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "113N2N22N9FingerBeamDrillPipeCount19", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "113N2N22N9FingerBeamDrillPipeCount20", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "113N2N22N9FingerBeamDrillPipeCount21", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "113N2N22N9FingerBeamDrillPipeCount22", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "113N2N22N9FingerBeamDrillPipeCount23", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "113N2N22N9FingerBeamDrillPipeCount24", Num = 0 });

                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "114N2N22N9FingerBeamDrillPipeCount25", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "114N2N22N9FingerBeamDrillPipeCount26", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "114N2N22N9FingerBeamDrillPipeCount27", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "114N2N22N9FingerBeamDrillPipeCount28", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "114N2N22N9FingerBeamDrillPipeCount29", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "114N2N22N9FingerBeamDrillPipeCount30", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "114N2N22N9FingerBeamDrillPipeCount31", Num = 0 });
                FingerBeamDrillPipeCountList.Add(new DrillModel() { Name = "114N2N22N9FingerBeamDrillPipeCount32", Num = 0 });
                #endregion
                #region 二层台钻杆数读取
                this.tbrow0LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount16"], Mode = BindingMode.OneWay });
                this.tbrow1LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount1"], Mode = BindingMode.OneWay });
                this.tbrow2LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount2"], Mode = BindingMode.OneWay });
                this.tbrow3LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount3"], Mode = BindingMode.OneWay });
                this.tbrow4LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount4"], Mode = BindingMode.OneWay });
                this.tbrow5LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount5"], Mode = BindingMode.OneWay });
                this.tbrow6LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount6"], Mode = BindingMode.OneWay });
                this.tbrow7LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount7"], Mode = BindingMode.OneWay });
                this.tbrow8LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount8"], Mode = BindingMode.OneWay });
                this.tbrow9LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount9"], Mode = BindingMode.OneWay });
                this.tbrow10LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount10"], Mode = BindingMode.OneWay });
                this.tbrow11LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount11"], Mode = BindingMode.OneWay });
                this.tbrow12LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount12"], Mode = BindingMode.OneWay });
                this.tbrow13LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount13"], Mode = BindingMode.OneWay });
                this.tbrow14LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount14"], Mode = BindingMode.OneWay });
                this.tbrow15LeftRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount15"], Mode = BindingMode.OneWay });
                LeftDrillCoverter leftDrillCoverter = new LeftDrillCoverter();
                MultiBinding leftDrillMultiBind = new MultiBinding();
                leftDrillMultiBind.Converter = leftDrillCoverter;
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount16"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount1"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount2"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount3"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount4"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount5"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount6"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount7"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["111N2N22N9FingerBeamDrillPipeCount8"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount9"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount10"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount11"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount12"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount13"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount14"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["112N2N22N9FingerBeamDrillPipeCount15"], Mode = BindingMode.OneWay });
                leftDrillMultiBind.NotifyOnSourceUpdated = true;
                leftDrillMultiBind.ConverterParameter = "二层台 左:";
                this.tbRPLeft.SetBinding(TextBlock.TextProperty, leftDrillMultiBind);

                RightDrillCoverter rightDrillCoverter = new RightDrillCoverter();
                MultiBinding rightDrillMultiBind = new MultiBinding();
                rightDrillMultiBind.Converter = rightDrillCoverter;
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount32"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount17"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount18"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount19"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount20"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount21"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount22"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount23"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount24"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount25"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount26"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount27"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount28"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount29"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount30"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount31"], Mode = BindingMode.OneWay });
                rightDrillMultiBind.NotifyOnSourceUpdated = true;
                this.tbRPRight.SetBinding(TextBlock.TextProperty, rightDrillMultiBind);

                this.tbrow1RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount17"], Mode = BindingMode.OneWay });
                this.tbrow2RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount18"], Mode = BindingMode.OneWay });
                this.tbrow3RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount19"], Mode = BindingMode.OneWay });
                this.tbrow4RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount20"], Mode = BindingMode.OneWay });
                this.tbrow5RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount21"], Mode = BindingMode.OneWay });
                this.tbrow6RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount22"], Mode = BindingMode.OneWay });
                this.tbrow7RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount23"], Mode = BindingMode.OneWay });
                this.tbrow8RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["113N2N22N9FingerBeamDrillPipeCount24"], Mode = BindingMode.OneWay });
                this.tbrow9RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount25"], Mode = BindingMode.OneWay });
                this.tbrow10RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount26"], Mode = BindingMode.OneWay });
                this.tbrow11RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount27"], Mode = BindingMode.OneWay });
                this.tbrow12RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount28"], Mode = BindingMode.OneWay });
                this.tbrow13RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount29"], Mode = BindingMode.OneWay });
                this.tbrow14RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount30"], Mode = BindingMode.OneWay });
                this.tbrow15RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount31"], Mode = BindingMode.OneWay });
                this.tbrow0RightRP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["114N2N22N9FingerBeamDrillPipeCount32"], Mode = BindingMode.OneWay });
                #endregion
                this.imageElevatorStatus.SetBinding(Image.SourceProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["164ElevatorStatus"], Mode = BindingMode.OneWay, Converter = new ElevatorStatusConverter() });
                this.SetBinding(WR_RobotGripStatusProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["gripStatus"], Mode = BindingMode.OneWay });//抓手的18种状态
                this.SetBinding(WR_RobotArmRotateAngleProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });//回转电机的角度值
                this.SetBinding(WR_RobotArmPositionProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["armRealPosition"], Mode = BindingMode.OneWay });//手臂的实际位置
                this.SetBinding(WR_RobotCarPositionProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["carRealPosition"], Mode = BindingMode.OneWay });//小车实际位置
                this.SetBinding(WR_OperationModelProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });//操作模式   
                this.SetBinding(WR_PCFingerBeamNumberFeedBackProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["pcFingerBeamNumberFeedback"], Mode = BindingMode.OneWay });//当前所移动到的指梁号反馈
                this.SetBinding(WR_CurrentPointFingerBeamNumberProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });//当前所移动到的指梁号反馈

                spSF.Clear();
                foreach (StackPanel sp in FindVisualChildren<StackPanel>(this.gdMidMain))
                {
                    if (sp.Name.Contains("sfDrill")) spSF.Add(sp);
                }
                timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟
                timer.Change(0, 500);
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 定时器绑定手动对准指梁
        /// </summary>
        private void FingerBeamArrowBindByTime()
        {
            try
            {
                // 二层台指梁绑定
                byte sfSelectDrill = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"].Value.Byte;
                byte sfoperationModel = GlobalData.Instance.da["operationModel"].Value.Byte;
                if (sfoperationModel == 4 || sfoperationModel == 9)
                {
                    Border selectbd = sfSelectFingerList.Where(w => w.Num == sfSelectDrill).Select(s => s.SelectBorder).FirstOrDefault();
                    if (selectbd != null && selectbd.Visibility == Visibility.Visible)
                    {
                        selectbd.Visibility = Visibility.Hidden;
                        sfSelectFingerList.Where(w => w.Num != sfSelectDrill).ToList().ForEach(o => o.SelectBorder.Visibility = Visibility.Visible);
                    }
                    else
                    {
                        sfSelectFingerList.ForEach(o => o.SelectBorder.Visibility = Visibility.Visible);
                    }
                }
                else
                {
                    sfSelectFingerList.ForEach(o => o.SelectBorder.Visibility = Visibility.Visible);
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 切换当前运行设备顶上去
        /// </summary>
        /// <param name="obj"></param>
        private void Timer_Elapsed(object obj)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    FingerBeamArrowBindByTime();
                }));
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private void btn_SendFingerBeamNumber(object sender, MouseButtonEventArgs e)
        {
            string strImageName = ((Image)sender).Name;
            Regex regex = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);//查找以数字结尾的字符串
            Match match = regex.Match(strImageName);
            byte fingerBeamNumber;
            if (match.Success)
            {
                fingerBeamNumber = byte.Parse(match.Groups[1].Value);

                if (SendFingerBeamNumberEvent != null)
                {
                    SendFingerBeamNumberEvent(fingerBeamNumber);
                }

                if (SetDrillNumEvent != null)
                {
                    SetDrillNumEvent(fingerBeamNumber);
                }
            }
        }

        /// <summary>
        /// 初始化钻台信息
        /// </summary>
        public void InitRowsColoms()
        {
            GetConfigPara();
            InitHeightAndWidth();
        }

        /// <summary>
        /// 加载指梁行，钻杆列，钻铤列，小车最小/最大位移，手臂最大位移
        /// </summary>
        private bool GetConfigPara()
        {
            try
            {
                if (System.IO.File.Exists(configPath))
                {
                    if (GlobalData.Instance.da.GloConfig.SysType == 1)
                    {
                        rows = GlobalData.Instance.Rows + 1;
                    }
                    else
                    {
                        rows = GlobalData.Instance.Rows;
                    }
                    drillCnt = GlobalData.Instance.DrillNum; // 最大钻铤数量
                    // 未从操作台读取到，则加载默认值
                    if (rows == 0) rows = 10;
                    if (drillCnt == 0) drillCnt = 5;

                    StringBuilder sb = new StringBuilder(STRINGMAX);
                    // 加载钻杆列，未读取到则加载默认值
                    string strColoms = "0";
                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "COLOMS", strColoms, sb, STRINGMAX, configPath);
                    strColoms = sb.ToString();
                    int.TryParse(strColoms, out coloms);
                    if (coloms == 0) coloms = 17;

                    string sfstrCarMaxPosistion = "0";
                    string sfstrCarMinPosistion = "0";
                    string sfstrArmMaxPosistion = "0";


                    // 小车最大位移
                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "CARMAXPOSISTION", sfstrCarMaxPosistion, sb, STRINGMAX, configPath);
                    sfstrCarMaxPosistion = sb.ToString();
                    int.TryParse(sfstrCarMaxPosistion, out sfcarMaxPosistion);
                    GlobalData.Instance.CarMaxPosistion = sfcarMaxPosistion;
                    // 小车最小位移
                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "CARMINPOSISTION", sfstrCarMinPosistion, sb, STRINGMAX, configPath);
                    sfstrCarMinPosistion = sb.ToString();
                    int.TryParse(sfstrCarMinPosistion, out sfcarMinPosistion);
                    GlobalData.Instance.CarMinPosistion = sfcarMinPosistion;
                    // 手臂最大位移
                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "ARMMAXPOSISTION", sfstrArmMaxPosistion, sb, STRINGMAX, configPath);
                    sfstrArmMaxPosistion = sb.ToString();
                    int.TryParse(sfstrArmMaxPosistion, out sfarmMaxPosistion);
                    GlobalData.Instance.ArmMaxPosistion = sfarmMaxPosistion;

                    // 左钻铤是否存在
                    string showleftone = string.Empty;
                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "ShowLeftOne", showleftone, sb, STRINGMAX, configPath);
                    showleftone = sb.ToString();
                    if (showleftone == "1") this.showLeftOne = true;
                    else this.showLeftOne = false;
                    // 右钻铤是否存在
                    string showrightone = string.Empty;
                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "ShowRightOne", showrightone, sb, STRINGMAX, configPath);
                    showrightone = sb.ToString();
                    if (showrightone == "1") this.showRightOne = true;
                    else this.showRightOne = false;
                    // 左边行数
                    string leftrows = string.Empty;
                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "LeftRows", leftrows, sb, STRINGMAX, configPath);
                    leftrows = sb.ToString();
                    this.leftRows = int.Parse(leftrows);
                    // 右边行数
                    string rightrows = string.Empty;
                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "RightRows", rightrows, sb, STRINGMAX, configPath);
                    rightrows = sb.ToString();
                    this.rightRows = int.Parse(rightrows);

                    return true;
                }
                else
                {
                    return false;//配置文件不存在
                }
            }
            catch (Exception e)
            {
                DataHelper.AddErrorLog(e);
                return false;
            }
        }

        /// <summary>
        /// 初始化行高/行宽
        /// </summary>
        private void InitHeightAndWidth()
        {
            try
            {
                CalHeight();
                CalCarInfo();
                LoadRows();
                LoadFingerBeamDrillPipe();
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 加载行数
        /// </summary>
        private void LoadRows()
        {
            try
            {
                int leftrow = this.leftRows;
                if (this.showLeftOne) leftrow -= 1; // 左边有钻铤，减去一行
                int rightrow = this.rightRows;
                if (this.showRightOne) rightrow -= 1; // 左边有钻铤，减去一行
                #region 隐藏两侧多余行数
                HiddenTBInSP(this.spOneCol, leftrow); // 隐藏第一列-左边二层台行数
                HiddenTBInSP(this.spThreeCol, leftrow); // 隐藏第三列-左边行数
                HiddenTBInSP(this.spFourCol, rightrow); // 隐藏第四列-右边行数
                HiddenTBInSP(this.spSixCol, rightrow); // 隐藏第六列-右边二层台行数
                #endregion
                #region 隐藏指梁数
                HiddenLeftDrillInSP(this.spLeftDrill, leftrow);
                HiddenLeftDrillInSP(this.spLeftImg, leftrow);
                HiddenLeftDrillInSP(this.spRightImg, rightrow);
                HiddenLeftDrillInSP(this.spRightDrill, rightrow);
                #endregion
                if (this.showLeftOne)
                {
                    this.leftRow16.Visibility = Visibility.Visible;
                    this.gdLeftImgRow16.Visibility = Visibility.Visible;
                }
                if (this.showRightOne)
                {
                    this.rightRow16.Visibility = Visibility.Visible;
                    this.gdRightImgRow16.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 计算高度
        /// </summary>
        private void CalHeight()
        {
            try
            {
                int leftrow = this.leftRows;
                if (this.showLeftOne) leftrow -= 1; // 左边有钻铤，减去一行
                int rightrow = this.rightRows;
                if (this.showRightOne) rightrow -= 1; // 左边有钻铤，减去一行

                double TotalHight = 300.0-8; // 中间台面总高度-最上面横梁高度
                #region 计算左侧高度
                double leftAvgHeight = 0.0;
                if (this.showLeftOne) // 左钻铤行存在
                {
                    leftAvgHeight = (TotalHight - 5) / (leftrow + 1); // 平均高度为(总高度-钻铤行多的高度)/(指梁行数+钻铤行)
                    this.WR_SFLeftRowHeight = leftAvgHeight;
                    this.WR_SFLeftFirstRowHeight = leftAvgHeight + 5;

                }
                else
                {
                    leftAvgHeight = TotalHight / leftrow; // 平均高度为(总高度-钻铤行多的高度)/(指梁行数+钻铤行)
                    this.WR_SFLeftRowHeight = leftAvgHeight;
                }
                #endregion
                #region 计算右侧高度
                double rightAvgHeight = 0.0;
                if (this.showRightOne) // 左钻铤行存在
                {
                    rightAvgHeight = (TotalHight - 5) / (rightrow + 1); // 平均高度为(总高度-钻铤行多的高度)/(指梁行数+钻铤行)
                    this.WR_SFRightRowHeight = rightAvgHeight;
                    this.WR_SFRightFirstRowHeight = rightAvgHeight + 5;

                }
                else
                {
                    rightAvgHeight = TotalHight / rightrow; // 平均高度为(总高度-钻铤行多的高度)/(指梁行数+钻铤行)
                    this.WR_SFRightRowHeight = rightAvgHeight;
                }
                #endregion
                double carHeight = this.WR_SFRightRowHeight > this.WR_SFLeftRowHeight ? this.WR_SFLeftRowHeight : this.WR_SFRightRowHeight;
                if (carHeight < 20)
                {
                    this.RobotCar.Height = carHeight;
                    this.RobotCar.Width = carHeight;
                    this.RobotArm.Height = carHeight;
                    this.RobotArm.Width = carHeight;
                }
                else
                {
                    this.RobotCar.Height = 20;
                    this.RobotCar.Width = 20;
                    this.RobotArm.Height = 20;
                    this.RobotArm.Width = 20;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 计算小车信息
        /// </summary>
        private void CalCarInfo()
        {
            try
            {
                this.WR_RealHeight = 300.0 - 8.0 - this.RobotCar.Height; // 小车上下行走距离=总高度-最上面横梁高度-小车高度
                this.WR_MiddleHeight = (WR_RealHeight + 8) / 2.0; // 中间高度 = （实际移动距离+最上方横梁）/2
                this.WR_WorkAnimationWidth = (500 - this.RobotCar.Width) / 2; // X轴最大位移 (台面宽度-小车宽度)/2
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 隐藏StackPanel中Textblock
        /// </summary>
        /// <param name="sp">待隐藏的StackPanel</param>
        private void HiddenTBInSP(StackPanel sp,int realRow)
        {
            foreach (Grid gd in FindVisualChildren<Grid>(sp))
            {
                if (gd.Name.Contains("row"))
                {
                    string result = Regex.Replace(gd.Name, @"[^0-9]+", "");
                    int iRow = -1;
                    int.TryParse(result, out iRow);
                    if (iRow > realRow)
                    {
                        gd.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        gd.Visibility = Visibility.Visible;
                    }
                }
            }
        }


        private void HiddenLeftDrillInSP(StackPanel sp,int realRows)
        {
            foreach (Grid gd in FindVisualChildren<Grid>(sp))
            {
                if (gd.Name.Contains("Row"))
                {
                    string result = Regex.Replace(gd.Name, @"[^0-9]+", "");
                    int iRow = -1;
                    int.TryParse(result, out iRow);
                    if (iRow > realRows)
                    {
                        gd.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        gd.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        /// <summary>
        /// 加载钻杆
        /// </summary>
        public void LoadFingerBeamDrillPipe()
        {
            try
            {
                int fingerBeamNumber = 0;
                int fingerBeamDrillPipeCount = 0;
                // 非参数配置界面并且非补偿模式-145-176字节读取指梁钻杆数目
                if (GlobalData.Instance.da["Con_Set0"].Value.Byte != 23 && GlobalData.Instance.da["operationModel"].Value.Byte != 9)
                {
                    foreach (var model in FingerBeamDrillPipeCountList)
                    {
                        if (model.Num != GlobalData.Instance.da[model.Name].Value.Byte) // 钻杆数量改变
                        {
                            model.Num = GlobalData.Instance.da[model.Name].Value.Byte;
                            Regex regexFingerBeam = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                            Match match = regexFingerBeam.Match(model.Name);//找出所在的行数
                            if (match.Success)
                            {
                                fingerBeamNumber = int.Parse(match.Groups[1].Value);
                                fingerBeamDrillPipeCount = (int)(GlobalData.Instance.da[model.Name].Value.Byte);
                                SetDrillPipeCountVisible(fingerBeamNumber, fingerBeamDrillPipeCount);
                            }
                        }
                    }
                }
                

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 更新具体的钻杆数量
        /// </summary>
        /// <param name="fingerBeamNumber">行数</param>
        /// <param name="fingerBeamDrillPipeCount">钻杆数</param>
        public void SetDrillPipeCountVisible(int fingerBeamNumber, int fingerBeamDrillPipeCount)
        {
            try
            {
                if (fingerBeamNumber < 1 || fingerBeamNumber > 32 || fingerBeamDrillPipeCount < 0)
                {
                    return;
                }
                double width = 0.0; // 显示的钻杆/钻铤宽度
                double margin = 0.0; //边距
                if (fingerBeamNumber == 16 || fingerBeamNumber == 32)
                {
                    #region 钻铤设置
                    if (fingerBeamDrillPipeCount > drillCnt)
                    {
                        fingerBeamDrillPipeCount = drillCnt;
                    }

                    width = 100.0 / drillCnt;
                    margin = 0.0;
                    if (width > 25)
                    {
                        margin = (width - 25) / 2.0;
                        width = 25;
                    }
                    StackPanel sp = spSF.Where(w => int.Parse(w.Tag.ToString()) == fingerBeamNumber).FirstOrDefault();
                    if (sp != null)
                    {
                        sp.Children.Clear();
                        for (int i = 0; i < fingerBeamDrillPipeCount; i++)
                        {
                            AnimationButton btn = InitBtn(width, width, "#6DB4EF", margin);
                            sp.Children.Add(btn);
                        }
                    }
                    #endregion
                }
                else
                {

                    #region 钻杆设置
                    if (fingerBeamDrillPipeCount > coloms)
                    {
                        fingerBeamDrillPipeCount = coloms;
                    }

                    width = 170.0 / coloms;
                    margin = 0.0;
                    if (width > 10)
                    {
                        margin = (width - 10) / 2.0;
                        width = 10;
                    }
                    StackPanel spp = spSF.Where(w => int.Parse(w.Tag.ToString()) == fingerBeamNumber).FirstOrDefault();
                    if (spp != null)
                    {
                        spp.Children.Clear();
                        for (int i = 0; i < fingerBeamDrillPipeCount; i++)
                        {
                            AnimationButton btn = InitBtn(width, width, "#6DB4EF", margin);
                            spp.Children.Add(btn);
                        }
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        private AnimationButton InitBtn(double width, double height, string Background, double margin)
        {
            var bc = new BrushConverter();
            AnimationButton btn = new AnimationButton();
            btn.Width = width;
            btn.Height = width;
            btn.Style = FindResource("CircleButton") as Style;
            btn.Background = (Brush)bc.ConvertFrom(Background);
            btn.Margin = new Thickness(margin, 0, margin, 0);
            return btn;
        }

        /// <summary>
        ///  查找子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
