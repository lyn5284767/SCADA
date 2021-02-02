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
using COM.Common;
using ControlLibrary;
using DatabaseLib;

namespace Main
{
    /// <summary>
    /// WR_Amination.xaml 的交互逻辑
    /// </summary>
    public partial class WR_Amination : UserControl
    {
        private static WR_Amination _instance = null;
        private static readonly object syncRoot = new object();

        public static WR_Amination Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new WR_Amination();
                        }
                    }
                }
                return _instance;
            }
        }
        private List<BorderNum> drSelectFingerList = new List<BorderNum>(); // 钻台面对准指梁
        private string configPath = System.Environment.CurrentDirectory + @"\Config.ini";
        //private bool bLoaded = false;
        const int STRINGMAX = 255;
        System.Threading.Timer timer;
        #region 二层台参数
        private int rows;
        private int coloms;//列数 
        private int drillCnt;//钻铤的个数
        private int space = 45; // 间距

        public delegate void SendFingerBeamNumber(byte number);

        public event SendFingerBeamNumber SendFingerBeamNumberEvent;

        public delegate void SetDrillNum(byte number);

        public event SetDrillNum SetDrillNumEvent;

        // 二层台指梁
        List<StackPanel> spSF = new List<StackPanel>();
        // 钻台面指梁
        List<StackPanel> spDR = new List<StackPanel>();
        /// <summary>
        /// 中间间距
        /// </summary>
        List<Border> bdSpace = new List<Border>();
        List<Border> bdSpace1 = new List<Border>();

        #endregion

        public int RowsCnt
        {
            get { return rows; }
        }

        #region 钻台面参数
        public static readonly DependencyProperty WR_DRCurrentPointFingerBeamNumberProperty = DependencyProperty.Register("WR_DRCurrentPointFingerBeamNumber", typeof(byte), typeof(WR_Amination), new PropertyMetadata((byte)0));//当前所移动到的指梁号反馈
        public static readonly DependencyProperty WR_DROperationModelProperty = DependencyProperty.Register("WR_DROperationModel", typeof(byte), typeof(WR_Amination), new PropertyMetadata((byte)0));//操作模式 
        public static readonly DependencyProperty WR_DRPCFingerBeamNumberFeedBackProperty = DependencyProperty.Register("WR_DRPCFingerBeamNumberFeedBack", typeof(byte), typeof(WR_Amination), new PropertyMetadata((byte)0));//上位机选择的指梁序号反馈

        public static readonly DependencyProperty WR_DRRobotCarPositionProperty = DependencyProperty.Register("WR_DRRobotCarPosition", typeof(short), typeof(WR_Amination), new PropertyMetadata((short)0)); // 小车位置
        public static readonly DependencyProperty WR_DRRobotGripStatusProperty = DependencyProperty.Register("WR_DRRobotGripStatus", typeof(byte), typeof(WR_Amination), new PropertyMetadata((byte)0)); // 抓手状态
        public static readonly DependencyProperty WR_DRRobotArmRotateAngleProperty = DependencyProperty.Register("WR_DRRobotArmRotateAngle", typeof(short), typeof(WR_Amination), new PropertyMetadata((short)0)); // 手臂旋转
        public static readonly DependencyProperty WR_DRRobotArmPositionProperty = DependencyProperty.Register("WR_DRRobotArmPosition", typeof(short), typeof(WR_Amination), new PropertyMetadata((short)0)); // 手臂位置

        /// <summary>
        /// 钻台面-当前指梁号
        /// </summary>
        public byte WR_DRCurrentPointFingerBeamNumber
        {
            get { return (byte)GetValue(WR_DRCurrentPointFingerBeamNumberProperty); }
            set { SetValue(WR_DRCurrentPointFingerBeamNumberProperty, value); }
        }
        /// <summary>
        /// 钻台面-操作模式
        /// </summary>
        public byte WR_DROperationModel
        {
            get { return (byte)GetValue(WR_DROperationModelProperty); }
            set { SetValue(WR_DROperationModelProperty, value); }
        }
        /// <summary>
        /// 钻台面-上位机选择的指梁
        /// </summary>
        public byte WR_DRPCFingerBeamNumberFeedBack
        {
            get { return (byte)GetValue(WR_DRPCFingerBeamNumberFeedBackProperty); }
            set { SetValue(WR_DRPCFingerBeamNumberFeedBackProperty, value); }
        }
        /// <summary>
        /// 钻台面-小车位置
        /// </summary>
        public short WR_DRRobotCarPosition
        {
            get { return (short)GetValue(WR_DRRobotCarPositionProperty); }
            set { SetValue(WR_DRRobotCarPositionProperty, value); }
        }
        /// <summary>
        /// 钻台面-抓手状态
        /// </summary>
        public byte WR_DRRobotGripStatus
        {
            get { return (byte)GetValue(WR_DRRobotGripStatusProperty); }
            set { SetValue(WR_DRRobotGripStatusProperty, value); }
        }
        /// <summary>
        /// 钻台面-手臂旋转角度
        /// </summary>
        public short WR_DRRobotArmRotateAngle
        {
            get { return (short)GetValue(WR_DRRobotArmRotateAngleProperty); }
            set { SetValue(WR_DRRobotArmRotateAngleProperty, value); }
        }
        /// <summary>
        /// 钻台面-手臂位置
        /// </summary>
        public short WR_DRRobotArmPosition
        {
            get { return (short)GetValue(WR_DRRobotArmPositionProperty); }
            set { SetValue(WR_DRRobotArmPositionProperty, value); }
        }
        #endregion

        public static readonly DependencyProperty WR_DRRealMoveXProperty = DependencyProperty.Register("WR_DRRealMoveX", typeof(double), typeof(WR_Amination), new PropertyMetadata((double)0.0));
        ///<summary>
        /// 钻台面-小车在X轴上最大位移
        /// </summary>
        public double WR_DRRealMoveX
        {
            get { return (double)GetValue(WR_DRRealMoveXProperty); }
            set { SetValue(WR_DRRealMoveXProperty, value); }
        }

        public static readonly DependencyProperty WR_DRMiddleXProperty = DependencyProperty.Register("WR_DRMiddleX", typeof(double), typeof(WR_Amination), new PropertyMetadata((double)0.0));// 中间高度

        ///<summary>
        /// 钻台面-中间高度--减去用于置于零点
        /// </summary>
        public double WR_DRMiddleX
        {
            get { return (double)GetValue(WR_DRMiddleXProperty); }
            set { SetValue(WR_DRMiddleXProperty, value); }
        }

        List<DrillModel> drDrillCountList = new List<DrillModel>();// 各个指梁所拥有的钻杆数目
        //private int drFirstHeight = 25;// 第一行高度
        //private int drHeight = 20; // 每行高度
        private int drSpace = 45; // 间距
        private int drcarMaxPosistion = 0;
        private int drcarMinPosistion = 0;
        private int drarmMaxPosistion = 0;
        private bool showLeftOne = true;
        private bool showRightOne = true;
        private int leftRows = 0;
        private int rightRows = 0;
        #region 设置行高及列
        public static readonly DependencyProperty WR_LeftRowHeightProperty = DependencyProperty.Register("WR_LeftRowHeight", typeof(double), typeof(WR_Amination), new PropertyMetadata((double)0.0));
        /// <summary>
        /// 显示左侧行高
        /// </summary>
        public double WR_LeftRowHeight
        {
            get { return (double)GetValue(WR_LeftRowHeightProperty); }
            set { SetValue(WR_LeftRowHeightProperty, value); }
        }
        public static readonly DependencyProperty WR_LeftFirstRowHeightProperty = DependencyProperty.Register("WR_LeftFirstRowHeight", typeof(double), typeof(WR_Amination), new PropertyMetadata((double)0.0));
        /// <summary>
        /// 显示左侧钻铤行行高
        /// </summary>
        public double WR_LeftFirstRowHeight
        {
            get { return (double)GetValue(WR_LeftFirstRowHeightProperty); }
            set { SetValue(WR_LeftFirstRowHeightProperty, value); }
        }

        public static readonly DependencyProperty WR_RightRowHeightProperty = DependencyProperty.Register("WR_RightRowHeight", typeof(double), typeof(WR_Amination), new PropertyMetadata((double)0.0));
        /// <summary>
        /// 显示左侧行高
        /// </summary>
        public double WR_RightRowHeight
        {
            get { return (double)GetValue(WR_RightRowHeightProperty); }
            set { SetValue(WR_RightRowHeightProperty, value); }
        }
        public static readonly DependencyProperty WR_RightFirstRowHeightProperty = DependencyProperty.Register("WR_RightFirstRowHeight", typeof(double), typeof(WR_Amination), new PropertyMetadata((double)0.0));
        /// <summary>
        /// 显示左侧钻铤行行高
        /// </summary>
        public double WR_RightFirstRowHeight
        {
            get { return (double)GetValue(WR_RightFirstRowHeightProperty); }
            set { SetValue(WR_RightFirstRowHeightProperty, value); }
        }
        #endregion

        #region 设置运动位移
        public static readonly DependencyProperty WR_DRWorkAnimationWidthProperty = DependencyProperty.Register("WR_DRWorkAnimationWidth", typeof(double), typeof(WR_Amination), new PropertyMetadata((double)0.0)); // 手臂最大位移
        /// <summary>
        /// 钻台面-手臂最大位移
        /// </summary>
        public double WR_DRWorkAnimationWidth
        {
            get { return (double)GetValue(WR_DRWorkAnimationWidthProperty); }
            set { SetValue(WR_DRWorkAnimationWidthProperty, value); }
        }
        //public static readonly DependencyProperty WR_DRMiddleXProperty = DependencyProperty.Register("WR_DRMiddleXProperty", typeof(double), typeof(WR_Amination), new PropertyMetadata((double)0.0));// 中间高度
        ///// <summary>
        ///// 手臂运行的最大距离
        ///// </summary>
        //public double WR_DRRealHeight
        //{
        //    get { return (double)GetValue(WR_DRMiddleXProperty); }
        //    set { SetValue(WR_DRMiddleXProperty, value); }
        //}
        #endregion

        public WR_Amination()
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
                #region 钻台面所有指梁
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow1, Num = 1 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow2, Num = 2 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow3, Num = 3 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow4, Num = 4 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow5, Num = 5 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow6, Num = 6 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow7, Num = 7 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow8, Num = 8 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow9, Num = 9 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow10, Num = 10 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow11, Num = 11 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow12, Num = 12 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow13, Num = 13 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow14, Num = 14 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow15, Num = 15 });

                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow17, Num = 17 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow18, Num = 18 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow19, Num = 19 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow20, Num = 20 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow21, Num = 21 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow22, Num = 22 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow23, Num = 23 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow24, Num = 24 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow25, Num = 25 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow26, Num = 26 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow27, Num = 27 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow28, Num = 28 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow29, Num = 29 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow30, Num = 30 });
                drSelectFingerList.Add(new BorderNum { SelectBorder = this.drFingerBeamArrow31, Num = 31 });

                #endregion
                #region 初始化钻台面钻杆/钻铤数量
                this.drDrillCountList.Clear();
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum1", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum2", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum3", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum4", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum5", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum6", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum7", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum8", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum9", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum10", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum11", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum12", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum13", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum14", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum15", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum16", Num = 0, LorR = "left" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum17", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum18", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum19", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum20", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum21", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum22", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum23", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum24", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum25", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum26", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum27", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum28", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum29", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum30", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum31", Num = 0, LorR = "right" });
                this.drDrillCountList.Add(new DrillModel() { Name = "drDrillNum32", Num = 0, LorR = "right" });
                #endregion

                #region 钻台面参数绑定
                this.SetBinding(WR_DROperationModelProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });//操作模式  
                this.SetBinding(WR_DRRobotCarPositionProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drCarPos"], Mode = BindingMode.OneWay });//小车实际位置
                this.SetBinding(WR_DRRobotGripStatusProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drgripStatus"], Mode = BindingMode.OneWay });//抓手的18种状态
                this.SetBinding(WR_DRRobotArmRotateAngleProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drRotePos"], Mode = BindingMode.OneWay, Converter = new WR_DRCallAngleConverter() });//回转电机的角度值
                this.SetBinding(WR_DRRobotArmPositionProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drArmPos"], Mode = BindingMode.OneWay });//手臂的实际位置
                this.SetBinding(WR_DRCurrentPointFingerBeamNumberProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });//当前所移动到的指梁号反馈
                this.SetBinding(WR_DRPCFingerBeamNumberFeedBackProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drPCSelectDrill"], Mode = BindingMode.OneWay });//当前所移动到的指梁号反馈

                this.tbrow0LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum16"], Mode = BindingMode.OneWay });
                this.tbrow1LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum1"], Mode = BindingMode.OneWay });
                this.tbrow2LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum2"], Mode = BindingMode.OneWay });
                this.tbrow3LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum3"], Mode = BindingMode.OneWay });
                this.tbrow4LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum4"], Mode = BindingMode.OneWay });
                this.tbrow5LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum5"], Mode = BindingMode.OneWay });
                this.tbrow6LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum6"], Mode = BindingMode.OneWay });
                this.tbrow7LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum7"], Mode = BindingMode.OneWay });
                this.tbrow8LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum8"], Mode = BindingMode.OneWay });
                this.tbrow9LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum9"], Mode = BindingMode.OneWay });
                this.tbrow10LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum10"], Mode = BindingMode.OneWay });
                this.tbrow11LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum11"], Mode = BindingMode.OneWay });
                this.tbrow12LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum12"], Mode = BindingMode.OneWay });
                this.tbrow13LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum13"], Mode = BindingMode.OneWay });
                this.tbrow14LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum14"], Mode = BindingMode.OneWay });
                this.tbrow15LeftFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum15"], Mode = BindingMode.OneWay });

                LeftDrillCoverter leftFPDrillCoverter = new LeftDrillCoverter();
                MultiBinding leftFPDrillMultiBind = new MultiBinding();
                leftFPDrillMultiBind.Converter = leftFPDrillCoverter;
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum16"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum1"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum2"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum3"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum4"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum5"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum6"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum7"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum8"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum9"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum10"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum11"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum12"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum13"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum14"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum15"], Mode = BindingMode.OneWay });
                leftFPDrillMultiBind.NotifyOnSourceUpdated = true;
                leftFPDrillMultiBind.ConverterParameter = "钻台面 左:";
                this.tbFPLeft.SetBinding(TextBlock.TextProperty, leftFPDrillMultiBind);

                this.tbrow1RightFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum17"], Mode = BindingMode.OneWay });
                this.tbrow2RightFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum18"], Mode = BindingMode.OneWay });
                this.tbrow3RightFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum19"], Mode = BindingMode.OneWay });
                this.tbrow4RightFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum20"], Mode = BindingMode.OneWay });
                this.tbrow5RightFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum21"], Mode = BindingMode.OneWay });
                this.tbrow6RightFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum22"], Mode = BindingMode.OneWay });
                this.tbrow7RightFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum23"], Mode = BindingMode.OneWay });
                this.tbrow8RightFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum24"], Mode = BindingMode.OneWay });
                this.tbrow9RightFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum25"], Mode = BindingMode.OneWay });
                this.tbrow10RightFP.SetBinding(TextBlock.TextProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum26"], Mode = BindingMode.OneWay });


                LeftDrillCoverter RightFPDrillCoverter = new LeftDrillCoverter();
                MultiBinding RightFPDrillMultiBind = new MultiBinding();
                RightFPDrillMultiBind.Converter = RightFPDrillCoverter;
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum32"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum17"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum18"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum19"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum20"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum21"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum22"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum23"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum24"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum25"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum26"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum27"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum28"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum29"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum30"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drDrillNum31"], Mode = BindingMode.OneWay });
                RightFPDrillMultiBind.NotifyOnSourceUpdated = true;
                RightFPDrillMultiBind.ConverterParameter = "右:";
                this.tbFPRight.SetBinding(TextBlock.TextProperty, RightFPDrillMultiBind);
                #endregion
                spSF.Clear();
                spDR.Clear();
                foreach (StackPanel sp in FindVisualChildren<StackPanel>(this.gdMidMain))
                {
                    if (sp.Name.Contains("sfDrill")) spSF.Add(sp);
                    if (sp.Name.Contains("drDrill")) spDR.Add(sp);
                }
                // 加载中间间距所有控件后续好设置宽度
                //foreach (Border bd in FindVisualChildren<Border>(this.gdMidMain))
                //{
                //    if (bd.Tag != null && bd.Tag.ToString() == "space") bdSpace.Add(bd);
                //    if (bd.Tag != null && bd.Tag.ToString() == "space1") bdSpace1.Add(bd);
                //}
                //this.FingerBeamArrowBind();

                timer = new System.Threading.Timer(new TimerCallback(Timer_Elapsed), this, 2000, 50);//改成50ms 的时钟
                timer.Change(0, 500);
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 绑定手动对准指梁
        /// </summary>
        private void FingerBeamArrowBind()
        {
            //#region 钻台面
            //AnimationCurrentFingerBeamVisableCoverter drCurrentFingerBeamCoverter = new AnimationCurrentFingerBeamVisableCoverter();
            //MultiBinding dr15DrillMultiBind = new MultiBinding();
            //dr15DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr15DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr15DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow15, Mode = BindingMode.OneWay });
            //dr15DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr15DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow15.SetBinding(Border.VisibilityProperty, dr15DrillMultiBind);

            //MultiBinding dr31DrillMultiBind = new MultiBinding();
            //dr31DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr31DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr31DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow31, Mode = BindingMode.OneWay });
            //dr31DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr31DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow31.SetBinding(Border.VisibilityProperty, dr31DrillMultiBind);

            //MultiBinding dr14DrillMultiBind = new MultiBinding();
            //dr14DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr14DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr14DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow14, Mode = BindingMode.OneWay });
            //dr14DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr14DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow14.SetBinding(Border.VisibilityProperty, dr14DrillMultiBind);

            //MultiBinding dr30DrillMultiBind = new MultiBinding();
            //dr30DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr30DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr30DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow30, Mode = BindingMode.OneWay });
            //dr30DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr30DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow30.SetBinding(Border.VisibilityProperty, dr30DrillMultiBind);

            //MultiBinding dr13DrillMultiBind = new MultiBinding();
            //dr13DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr13DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr13DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow13, Mode = BindingMode.OneWay });
            //dr13DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr13DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow13.SetBinding(Border.VisibilityProperty, dr13DrillMultiBind);

            //MultiBinding dr29DrillMultiBind = new MultiBinding();
            //dr29DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr29DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr29DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow29, Mode = BindingMode.OneWay });
            //dr29DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr29DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow29.SetBinding(Border.VisibilityProperty, dr29DrillMultiBind);

            //MultiBinding dr12DrillMultiBind = new MultiBinding();
            //dr12DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr12DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr12DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow12, Mode = BindingMode.OneWay });
            //dr12DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr12DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow12.SetBinding(Border.VisibilityProperty, dr12DrillMultiBind);

            //MultiBinding dr28DrillMultiBind = new MultiBinding();
            //dr28DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr28DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr28DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow28, Mode = BindingMode.OneWay });
            //dr28DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr28DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow28.SetBinding(Border.VisibilityProperty, dr28DrillMultiBind);

            //MultiBinding dr11DrillMultiBind = new MultiBinding();
            //dr11DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr11DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr11DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow11, Mode = BindingMode.OneWay });
            //dr11DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr11DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow11.SetBinding(Border.VisibilityProperty, dr11DrillMultiBind);

            //MultiBinding dr27DrillMultiBind = new MultiBinding();
            //dr27DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr27DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr27DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow27, Mode = BindingMode.OneWay });
            //dr27DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr27DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow27.SetBinding(Border.VisibilityProperty, dr27DrillMultiBind);

            //MultiBinding dr10DrillMultiBind = new MultiBinding();
            //dr10DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr10DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr10DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow10, Mode = BindingMode.OneWay });
            //dr10DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr10DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow10.SetBinding(Border.VisibilityProperty, dr10DrillMultiBind);

            //MultiBinding dr26DrillMultiBind = new MultiBinding();
            //dr26DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr26DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr26DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow26, Mode = BindingMode.OneWay });
            //dr26DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr26DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow26.SetBinding(Border.VisibilityProperty, dr26DrillMultiBind);

            //MultiBinding dr9DrillMultiBind = new MultiBinding();
            //dr9DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr9DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr9DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow9, Mode = BindingMode.OneWay });
            //dr9DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr9DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow9.SetBinding(Border.VisibilityProperty, dr9DrillMultiBind);

            //MultiBinding dr25DrillMultiBind = new MultiBinding();
            //dr25DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr25DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr25DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow25, Mode = BindingMode.OneWay });
            //dr25DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr25DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow25.SetBinding(Border.VisibilityProperty, dr25DrillMultiBind);

            //MultiBinding dr8DrillMultiBind = new MultiBinding();
            //dr8DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr8DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr8DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow8, Mode = BindingMode.OneWay });
            //dr8DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr8DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow8.SetBinding(Border.VisibilityProperty, dr8DrillMultiBind);

            //MultiBinding dr24DrillMultiBind = new MultiBinding();
            //dr24DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr24DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr24DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow24, Mode = BindingMode.OneWay });
            //dr24DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr24DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow24.SetBinding(Border.VisibilityProperty, dr24DrillMultiBind);

            //MultiBinding dr7DrillMultiBind = new MultiBinding();
            //dr7DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr7DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr7DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow7, Mode = BindingMode.OneWay });
            //dr7DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr7DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow7.SetBinding(Border.VisibilityProperty, dr7DrillMultiBind);

            //MultiBinding dr23DrillMultiBind = new MultiBinding();
            //dr23DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr23DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr23DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow23, Mode = BindingMode.OneWay });
            //dr23DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr23DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow23.SetBinding(Border.VisibilityProperty, dr23DrillMultiBind);

            //MultiBinding dr6DrillMultiBind = new MultiBinding();
            //dr6DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr6DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr6DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow6, Mode = BindingMode.OneWay });
            //dr6DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr6DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow6.SetBinding(Border.VisibilityProperty, dr6DrillMultiBind);

            //MultiBinding dr22DrillMultiBind = new MultiBinding();
            //dr22DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr22DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr22DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow22, Mode = BindingMode.OneWay });
            //dr22DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr22DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow22.SetBinding(Border.VisibilityProperty, dr22DrillMultiBind);

            //MultiBinding dr5DrillMultiBind = new MultiBinding();
            //dr5DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr5DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr5DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow5, Mode = BindingMode.OneWay });
            //dr5DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr5DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow5.SetBinding(Border.VisibilityProperty, dr5DrillMultiBind);

            //MultiBinding dr21DrillMultiBind = new MultiBinding();
            //dr21DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr21DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr21DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow21, Mode = BindingMode.OneWay });
            //dr21DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr21DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow21.SetBinding(Border.VisibilityProperty, dr21DrillMultiBind);

            //MultiBinding dr4DrillMultiBind = new MultiBinding();
            //dr4DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr4DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr4DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow4, Mode = BindingMode.OneWay });
            //dr4DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr4DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow4.SetBinding(Border.VisibilityProperty, dr4DrillMultiBind);

            //MultiBinding dr20DrillMultiBind = new MultiBinding();
            //dr20DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr20DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr20DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow20, Mode = BindingMode.OneWay });
            //dr20DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr20DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow20.SetBinding(Border.VisibilityProperty, dr20DrillMultiBind);

            //MultiBinding dr3DrillMultiBind = new MultiBinding();
            //dr3DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr3DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr3DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow3, Mode = BindingMode.OneWay });
            //dr3DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr3DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow3.SetBinding(Border.VisibilityProperty, dr3DrillMultiBind);

            //MultiBinding dr19DrillMultiBind = new MultiBinding();
            //dr19DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr19DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr19DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow19, Mode = BindingMode.OneWay });
            //dr19DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr19DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow19.SetBinding(Border.VisibilityProperty, dr19DrillMultiBind);

            //MultiBinding dr2DrillMultiBind = new MultiBinding();
            //dr2DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr2DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr2DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow2, Mode = BindingMode.OneWay });
            //dr2DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr2DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow2.SetBinding(Border.VisibilityProperty, dr2DrillMultiBind);

            //MultiBinding dr18DrillMultiBind = new MultiBinding();
            //dr18DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr18DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr18DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow18, Mode = BindingMode.OneWay });
            //dr18DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr18DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow18.SetBinding(Border.VisibilityProperty, dr18DrillMultiBind);

            //MultiBinding dr1DrillMultiBind = new MultiBinding();
            //dr1DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr1DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr1DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow1, Mode = BindingMode.OneWay });
            //dr1DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr1DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow1.SetBinding(Border.VisibilityProperty, dr1DrillMultiBind);

            //MultiBinding dr17DrillMultiBind = new MultiBinding();
            //dr17DrillMultiBind.Converter = drCurrentFingerBeamCoverter;
            //dr17DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });
            //dr17DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.drFingerBeamArrow17, Mode = BindingMode.OneWay });
            //dr17DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });
            //dr17DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.drFingerBeamArrow17.SetBinding(Border.VisibilityProperty, dr17DrillMultiBind);
            //#endregion
        }

        /// <summary>
        /// 定时器绑定手动对准指梁
        /// </summary>
        private void FingerBeamArrowBindByTime()
        {
            try
            {
                // 钻台面指梁绑定
                byte drSelectDrill = GlobalData.Instance.da["drSelectDrill"].Value.Byte;
                byte droperationModel = GlobalData.Instance.da["droperationModel"].Value.Byte;
                if (droperationModel == 4 || droperationModel == 9)
                {
                    Border selectbd = drSelectFingerList.Where(w => w.Num == drSelectDrill).Select(s => s.SelectBorder).FirstOrDefault();
                    if (selectbd != null && selectbd.Visibility == Visibility.Visible)
                    {
                        selectbd.Visibility = Visibility.Hidden;
                        drSelectFingerList.Where(w => w.Num != drSelectDrill).ToList().ForEach(o => o.SelectBorder.Visibility = Visibility.Visible);
                    }
                    else
                    {
                        drSelectFingerList.ForEach(o => o.SelectBorder.Visibility = Visibility.Visible);
                    }
                }
                else
                {
                    drSelectFingerList.ForEach(o => o.SelectBorder.Visibility = Visibility.Visible);
                }
               
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }

            //#region 二层台
            //AnimationCurrentFingerBeamVisableCoverter CurrentFingerBeamCoverter = new AnimationCurrentFingerBeamVisableCoverter();
            //MultiBinding sf15DrillMultiBind = new MultiBinding();
            //sf15DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf15DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf15DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow15, Mode = BindingMode.OneWay });
            //sf15DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf15DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow15.SetBinding(Border.VisibilityProperty, sf15DrillMultiBind);

            //MultiBinding sf31DrillMultiBind = new MultiBinding();
            //sf31DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf31DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf31DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow31, Mode = BindingMode.OneWay });
            //sf31DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf31DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow31.SetBinding(Border.VisibilityProperty, sf31DrillMultiBind);

            //MultiBinding sf14DrillMultiBind = new MultiBinding();
            //sf14DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf14DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf14DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow14, Mode = BindingMode.OneWay });
            //sf14DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf14DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow14.SetBinding(Border.VisibilityProperty, sf14DrillMultiBind);

            //MultiBinding sf30DrillMultiBind = new MultiBinding();
            //sf30DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf30DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf30DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow30, Mode = BindingMode.OneWay });
            //sf30DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf30DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow30.SetBinding(Border.VisibilityProperty, sf30DrillMultiBind);

            //MultiBinding sf13DrillMultiBind = new MultiBinding();
            //sf13DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf13DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf13DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow13, Mode = BindingMode.OneWay });
            //sf13DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf13DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow13.SetBinding(Border.VisibilityProperty, sf13DrillMultiBind);

            //MultiBinding sf29DrillMultiBind = new MultiBinding();
            //sf29DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf29DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf29DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow29, Mode = BindingMode.OneWay });
            //sf29DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf29DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow29.SetBinding(Border.VisibilityProperty, sf29DrillMultiBind);

            //MultiBinding sf12DrillMultiBind = new MultiBinding();
            //sf12DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf12DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf12DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow12, Mode = BindingMode.OneWay });
            //sf12DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf12DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow12.SetBinding(Border.VisibilityProperty, sf12DrillMultiBind);

            //MultiBinding sf28DrillMultiBind = new MultiBinding();
            //sf28DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf28DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf28DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow28, Mode = BindingMode.OneWay });
            //sf28DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf28DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow28.SetBinding(Border.VisibilityProperty, sf28DrillMultiBind);

            //MultiBinding sf11DrillMultiBind = new MultiBinding();
            //sf11DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf11DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf11DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow11, Mode = BindingMode.OneWay });
            //sf11DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf11DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow11.SetBinding(Border.VisibilityProperty, sf11DrillMultiBind);

            //MultiBinding sf27DrillMultiBind = new MultiBinding();
            //sf27DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf27DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf27DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow27, Mode = BindingMode.OneWay });
            //sf27DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf27DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow27.SetBinding(Border.VisibilityProperty, sf27DrillMultiBind);

            //MultiBinding sf10DrillMultiBind = new MultiBinding();
            //sf10DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf10DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf10DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow10, Mode = BindingMode.OneWay });
            //sf10DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf10DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow10.SetBinding(Border.VisibilityProperty, sf10DrillMultiBind);

            //MultiBinding sf26DrillMultiBind = new MultiBinding();
            //sf26DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf26DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf26DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow26, Mode = BindingMode.OneWay });
            //sf26DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf26DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow26.SetBinding(Border.VisibilityProperty, sf26DrillMultiBind);

            //MultiBinding sf9DrillMultiBind = new MultiBinding();
            //sf9DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf9DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf9DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow9, Mode = BindingMode.OneWay });
            //sf9DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf9DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow9.SetBinding(Border.VisibilityProperty, sf9DrillMultiBind);

            //MultiBinding sf25DrillMultiBind = new MultiBinding();
            //sf25DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf25DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf25DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow25, Mode = BindingMode.OneWay });
            //sf25DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf25DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow25.SetBinding(Border.VisibilityProperty, sf25DrillMultiBind);

            //MultiBinding sf8DrillMultiBind = new MultiBinding();
            //sf8DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf8DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf8DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow8, Mode = BindingMode.OneWay });
            //sf8DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf8DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow8.SetBinding(Border.VisibilityProperty, sf8DrillMultiBind);

            //MultiBinding sf24DrillMultiBind = new MultiBinding();
            //sf24DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf24DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf24DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow24, Mode = BindingMode.OneWay });
            //sf24DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf24DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow24.SetBinding(Border.VisibilityProperty, sf24DrillMultiBind);

            //MultiBinding sf7DrillMultiBind = new MultiBinding();
            //sf7DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf7DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf7DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow7, Mode = BindingMode.OneWay });
            //sf7DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf7DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow7.SetBinding(Border.VisibilityProperty, sf7DrillMultiBind);

            //MultiBinding sf23DrillMultiBind = new MultiBinding();
            //sf23DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf23DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf23DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow23, Mode = BindingMode.OneWay });
            //sf23DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf23DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow23.SetBinding(Border.VisibilityProperty, sf23DrillMultiBind);

            //MultiBinding sf6DrillMultiBind = new MultiBinding();
            //sf6DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf6DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf6DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow6, Mode = BindingMode.OneWay });
            //sf6DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf6DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow6.SetBinding(Border.VisibilityProperty, sf6DrillMultiBind);

            //MultiBinding sf22DrillMultiBind = new MultiBinding();
            //sf22DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf22DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf22DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow22, Mode = BindingMode.OneWay });
            //sf22DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf22DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow22.SetBinding(Border.VisibilityProperty, sf22DrillMultiBind);

            //MultiBinding sf5DrillMultiBind = new MultiBinding();
            //sf5DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf5DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf5DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow5, Mode = BindingMode.OneWay });
            //sf5DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf5DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow5.SetBinding(Border.VisibilityProperty, sf5DrillMultiBind);

            //MultiBinding sf21DrillMultiBind = new MultiBinding();
            //sf21DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf21DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf21DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow21, Mode = BindingMode.OneWay });
            //sf21DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf21DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow21.SetBinding(Border.VisibilityProperty, sf21DrillMultiBind);

            //MultiBinding sf4DrillMultiBind = new MultiBinding();
            //sf4DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf4DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf4DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow4, Mode = BindingMode.OneWay });
            //sf4DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf4DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow4.SetBinding(Border.VisibilityProperty, sf4DrillMultiBind);

            //MultiBinding sf20DrillMultiBind = new MultiBinding();
            //sf20DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf20DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf20DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow20, Mode = BindingMode.OneWay });
            //sf20DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf20DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow20.SetBinding(Border.VisibilityProperty, sf20DrillMultiBind);

            //MultiBinding sf3DrillMultiBind = new MultiBinding();
            //sf3DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf3DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf3DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow3, Mode = BindingMode.OneWay });
            //sf3DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf3DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow3.SetBinding(Border.VisibilityProperty, sf3DrillMultiBind);

            //MultiBinding sf19DrillMultiBind = new MultiBinding();
            //sf19DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf19DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf19DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow19, Mode = BindingMode.OneWay });
            //sf19DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf19DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow19.SetBinding(Border.VisibilityProperty, sf19DrillMultiBind);

            //MultiBinding sf2DrillMultiBind = new MultiBinding();
            //sf2DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf2DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf2DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow2, Mode = BindingMode.OneWay });
            //sf2DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf2DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow2.SetBinding(Border.VisibilityProperty, sf2DrillMultiBind);

            //MultiBinding sf18DrillMultiBind = new MultiBinding();
            //sf18DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf18DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf18DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow18, Mode = BindingMode.OneWay });
            //sf18DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf18DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow18.SetBinding(Border.VisibilityProperty, sf18DrillMultiBind);

            //MultiBinding sf1DrillMultiBind = new MultiBinding();
            //sf1DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf1DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf1DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow1, Mode = BindingMode.OneWay });
            //sf1DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf1DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow1.SetBinding(Border.VisibilityProperty, sf1DrillMultiBind);

            //MultiBinding sf17DrillMultiBind = new MultiBinding();
            //sf17DrillMultiBind.Converter = CurrentFingerBeamCoverter;
            //sf17DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });
            //sf17DrillMultiBind.Bindings.Add(new Binding("Name") { Source = this.FingerBeamArrow17, Mode = BindingMode.OneWay });
            //sf17DrillMultiBind.Bindings.Add(new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });
            //sf17DrillMultiBind.NotifyOnSourceUpdated = true;
            //this.FingerBeamArrow17.SetBinding(Border.VisibilityProperty, sf17DrillMultiBind);
            //#endregion
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

                    string drstrSpace = "45";
                    string drstrCarMaxPosistion = "0";
                    string drstrCarMinPosistion = "0";
                    string drstrArmMaxPosistion = "0";

                    // 间距
                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "SPACE", drstrSpace, sb, STRINGMAX, configPath);
                    drstrSpace = sb.ToString();
                    int.TryParse(drstrSpace, out drSpace);
                    // 小车最大位移
                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "CARMAXPOSISTION", drstrCarMaxPosistion, sb, STRINGMAX, configPath);
                    drstrCarMaxPosistion = sb.ToString();
                    int.TryParse(drstrCarMaxPosistion, out drcarMaxPosistion);
                    GlobalData.Instance.DRCarMaxPosistion = drcarMaxPosistion;
                    // 小车最小位移
                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "CARMINPOSISTION", drstrCarMinPosistion, sb, STRINGMAX, configPath);
                    drstrCarMinPosistion = sb.ToString();
                    int.TryParse(drstrCarMinPosistion, out drcarMinPosistion);
                    GlobalData.Instance.DRCarMinPosistion = drcarMinPosistion;
                    // 手臂最大位移
                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "ARMMAXPOSISTION", drstrArmMaxPosistion, sb, STRINGMAX, configPath);
                    drstrArmMaxPosistion = sb.ToString();
                    int.TryParse(drstrArmMaxPosistion, out drarmMaxPosistion);
                    GlobalData.Instance.DRArmMaxPosistion = drarmMaxPosistion;

                    // 左钻铤是否存在
                    string showleftone = string.Empty;
                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "ShowLeftOne", showleftone, sb, STRINGMAX, configPath);
                    showleftone = sb.ToString();
                    if (showleftone == "1") this.showLeftOne = true;
                    else this.showLeftOne = false;
                    // 右钻铤是否存在
                    string showrightone = string.Empty;
                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "ShowRightOne", showrightone, sb, STRINGMAX, configPath);
                    showrightone = sb.ToString();
                    if (showrightone == "1") this.showRightOne = true;
                    else this.showRightOne = false;
                    // 左边行数
                    string leftrows = string.Empty;
                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "LeftRows", leftrows, sb, STRINGMAX, configPath);
                    leftrows = sb.ToString();
                    this.leftRows = int.Parse(leftrows);
                    // 右边行数
                    string rightrows = string.Empty;
                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "RightRows", rightrows, sb, STRINGMAX, configPath);
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
                CalWidth();
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
                HiddenDrillInSP(this.spLeftDrill, leftrow);
                HiddenDrillInSP(this.spLeftImg, leftrow);
                HiddenDrillInSP(this.spRightDrill, rightrow);
                HiddenDrillInSP(this.spRightImg, rightrow);
                // 左右钻铤行
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
                #endregion
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
                double TotalHight = 300.0 - 8.0; // 中间台面总高度-最上面横梁高度
                #region 计算左侧高度
                double leftAvgHeight = 0.0;
                if (this.showLeftOne) // 左钻铤行存在
                {
                    leftAvgHeight = (TotalHight - 5) / (leftrow + 1); // 平均高度为(总高度-钻铤行多的高度)/(指梁行数+钻铤行)
                    this.WR_LeftRowHeight = leftAvgHeight;
                    this.WR_LeftFirstRowHeight = leftAvgHeight + 5;

                }
                else
                {
                    leftAvgHeight = TotalHight / leftrow; // 平均高度为(总高度-钻铤行多的高度)/(指梁行数+钻铤行)
                    this.WR_LeftRowHeight = leftAvgHeight;
                }
                #endregion
                #region 计算右侧高度
                double rightAvgHeight = 0.0;
                if (this.showRightOne) // 左钻铤行存在
                {
                    rightAvgHeight = (TotalHight - 5) / (rightrow + 1); // 平均高度为(总高度-钻铤行多的高度)/(指梁行数+钻铤行)
                    this.WR_RightRowHeight = rightAvgHeight;
                    this.WR_RightFirstRowHeight = rightAvgHeight + 5;

                }
                else
                {
                    rightAvgHeight = TotalHight / rightrow; // 平均高度为(总高度-钻铤行多的高度)/(指梁行数+钻铤行)
                    this.WR_RightRowHeight = rightAvgHeight;
                }
                #endregion
                double carHeight = this.WR_RightRowHeight > this.WR_LeftRowHeight ? this.WR_LeftRowHeight : this.WR_RightRowHeight;
                if (carHeight < 20)
                {
                    this.drRobotCar.Height = carHeight;
                    this.drRobotCar.Width = carHeight;
                    this.DRRobotArm.Height = carHeight;
                    this.DRRobotArm.Width = carHeight;
                }
                else
                {
                    this.drRobotCar.Height = 20;
                    this.drRobotCar.Width = 20;
                    this.DRRobotArm.Height = 20;
                    this.DRRobotArm.Width = 20;
                }
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }

        /// <summary>
        /// 计算中间宽度
        /// </summary>
        private void CalWidth()
        {
            try
            {
                this.spMidMain.Width = 460 + this.space;
                bdSpace.ForEach(f => f.Width = 40 + this.space);
                bdSpace1.ForEach(f => f.Width = (40 + this.space));
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
                this.WR_DRRealMoveX = (500-20); // 小车左右位移距离=(总长度-小车宽度)/2
                this.WR_DRMiddleX = (500 - 10) / 2.0; // X轴中间 = （实际移动距离+左右壁框）/2
                this.WR_DRWorkAnimationWidth = (340 - this.drRobotCar.Width); // X轴最大位移 (台面宽度-小车宽度)/2
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
        private void HiddenTBInSP(StackPanel sp,int realrows)
        {
            foreach (Grid gd in FindVisualChildren<Grid>(sp))
            {
                if (gd.Name.Contains("row"))
                {
                    string result = Regex.Replace(gd.Name, @"[^0-9]+", "");
                    int iRow = -1;
                    int.TryParse(result, out iRow);
                    if (iRow > realrows)
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
        

        private void HiddenDrillInSP(StackPanel sp,int realrows)
        {
            foreach (Grid gd in FindVisualChildren<Grid>(sp))
            {
                if (gd.Name.Contains("Row"))
                {
                    string result = Regex.Replace(gd.Name, @"[^0-9]+", "");
                    int iRow = -1;
                    int.TryParse(result, out iRow);
                    if (iRow > realrows)
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
                GlobalData.Instance.DrillLeftTotal = this.drDrillCountList.Where(w => w.LorR == "left").Sum(s => s.Num);
                    if (GlobalData.Instance.da["drPageNum"].Value.Byte == 30 || GlobalData.Instance.da["drPageNum"].Value.Byte == 33)
                    {
                        foreach (var model in this.drDrillCountList)
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
                        StackPanel sp = spDR.Where(w => int.Parse(w.Tag.ToString()) == fingerBeamNumber).FirstOrDefault();
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
                        StackPanel spp = spDR.Where(w => int.Parse(w.Tag.ToString()) == fingerBeamNumber).FirstOrDefault();
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
