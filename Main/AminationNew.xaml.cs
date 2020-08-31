using COM.Common;
using ControlLibrary;
using DatabaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// AminationNew.xaml 的交互逻辑
    /// </summary>
    public partial class AminationNew : UserControl
    {
        private static AminationNew _instance = null;
        private static readonly object syncRoot = new object();

        public static AminationNew Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new AminationNew();
                        }
                    }
                }
                return _instance;
            }
        }

        private string configPath = System.Environment.CurrentDirectory + @"\Config.ini";
        //private bool bLoaded = false;
        const int STRINGMAX = 255;
        #region 二层台参数
        private int rows;//行数
        private int coloms;//列数 
        private int drillCnt;//钻铤的个数
        private int space = 45; // 间距
        //private int firstRowHeight = 25;// 第一行高度
        //private int rowHeight = 20; // 每行高度
        List<DrillModel> FingerBeamDrillPipeCountList = new List<DrillModel>();// 各个指梁所拥有的钻杆数目
        private int carMaxPosistion = 0;
        private int carMinPosistion = 0;
        private int armMaxPosistion = 0;

        public delegate void SendFingerBeamNumber(byte number);

        public event SendFingerBeamNumber SendFingerBeamNumberEvent;

        // 二层台指梁
        List<StackPanel> spSF = new List<StackPanel>();
        // 钻台面指梁
        List<StackPanel> spDR = new List<StackPanel>();
        /// <summary>
        /// 中间间距
        /// </summary>
        List<Border> bdSpace = new List<Border>();
        List<Border> bdSpace1 = new List<Border>();

        public static readonly DependencyProperty RowHeightProperty = DependencyProperty.Register("RowHeight", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0));
        public static readonly DependencyProperty FirstRowHeightProperty = DependencyProperty.Register("FirstRowHeight", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0));
        public static readonly DependencyProperty TBRowHeightProperty = DependencyProperty.Register("TBRowHeight", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0));
        public static readonly DependencyProperty TBFirstRowHeightProperty = DependencyProperty.Register("TBFirstRowHeight", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0));
        public static readonly DependencyProperty CurrentPointFingerBeamNumberProperty = DependencyProperty.Register("CurrentPointFingerBeamNumber", typeof(byte), typeof(AminationNew), new PropertyMetadata((byte)0));//当前所移动到的指梁号反馈
        public static readonly DependencyProperty OperationModelProperty = DependencyProperty.Register("OperationModel", typeof(byte), typeof(AminationNew), new PropertyMetadata((byte)0));//操作模式 
        public static readonly DependencyProperty PCFingerBeamNumberFeedBackProperty = DependencyProperty.Register("PCFingerBeamNumberFeedBack", typeof(byte), typeof(AminationNew), new PropertyMetadata((byte)0));//上位机选择的指梁序号反馈

        public static readonly DependencyProperty RealHeightProperty = DependencyProperty.Register("RealHeight", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0));// 小车运动实际高度
        public static readonly DependencyProperty MiddleHeightProperty = DependencyProperty.Register("MiddleHeight", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0));// 中间高度
        public static readonly DependencyProperty RobotCarPositionProperty = DependencyProperty.Register("RobotCarPosition", typeof(short), typeof(AminationNew), new PropertyMetadata((short)0)); // 小车位置
        public static readonly DependencyProperty RobotGripStatusProperty = DependencyProperty.Register("RobotGripStatus", typeof(byte), typeof(AminationNew), new PropertyMetadata((byte)0)); // 抓手状态
        public static readonly DependencyProperty RobotArmRotateAngleProperty = DependencyProperty.Register("RobotArmRotateAngle", typeof(short), typeof(AminationNew), new PropertyMetadata((short)0)); // 手臂旋转
        public static readonly DependencyProperty RobotArmPositionProperty = DependencyProperty.Register("RobotArmPosition", typeof(short), typeof(AminationNew), new PropertyMetadata((short)0)); // 手臂位置
        public static readonly DependencyProperty WorkAnimationWidthProperty = DependencyProperty.Register("WorkAnimationWidth", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0)); // 手臂最大位移

        public static readonly DependencyProperty SFProperty = DependencyProperty.Register("SFType", typeof(SystemType), typeof(AminationNew), new PropertyMetadata(SystemType.SecondFloor)); // 二层台显示
        public static readonly DependencyProperty DRProperty = DependencyProperty.Register("DRType", typeof(SystemType), typeof(AminationNew), new PropertyMetadata(SystemType.SecondFloor)); // 钻台面台显示

        /// <summary>
        /// 钻杠行高
        /// </summary>
        public double RowHeight
        {
            get { return (double)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }
        /// <summary>
        /// 钻铤行高
        /// </summary>
        public double FirstRowHeight
        {
            get { return (double)GetValue(FirstRowHeightProperty); }
            set { SetValue(FirstRowHeightProperty, value); }
        }

        /// <summary>
        /// 显示行行高
        /// </summary>
        public double TBRowHeight
        {
            get { return (double)GetValue(TBRowHeightProperty); }
            set { SetValue(TBRowHeightProperty, value); }
        }
        /// <summary>
        /// 显示行行高
        /// </summary>
        public double TBFirstRowHeight
        {
            get { return (double)GetValue(TBFirstRowHeightProperty); }
            set { SetValue(TBFirstRowHeightProperty, value); }
        }

        /// <summary>
        /// 当前指梁号
        /// </summary>
        public byte CurrentPointFingerBeamNumber
        {
            get { return (byte)GetValue(CurrentPointFingerBeamNumberProperty); }
            set { SetValue(CurrentPointFingerBeamNumberProperty, value); }
        }
        /// <summary>
        /// 操作模式
        /// </summary>
        public byte OperationModel
        {
            get { return (byte)GetValue(OperationModelProperty); }
            set { SetValue(OperationModelProperty, value); }
        }
        /// <summary>
        /// 上位机选择的指梁
        /// </summary>
        public byte PCFingerBeamNumberFeedBack
        {
            get { return (byte)GetValue(PCFingerBeamNumberFeedBackProperty); }
            set { SetValue(PCFingerBeamNumberFeedBackProperty, value); }
        }
        /// <summary>
        /// 小车高度
        /// </summary>
        public short RobotCarPosition
        {
            get { return (short)GetValue(RobotCarPositionProperty); }
            set { SetValue(RobotCarPositionProperty, value); }
        }
        ///<summary>
        /// 小车位移实际高度
        /// </summary>
        public double RealHeight
        {
            get { return (double)GetValue(RealHeightProperty); }
            set { SetValue(RealHeightProperty, value); }
        }

        ///<summary>
        /// 中间高度--减去用于置于零点
        /// </summary>
        public double MiddleHeight
        {
            get { return (double)GetValue(MiddleHeightProperty); }
            set { SetValue(MiddleHeightProperty, value); }
        }
        /// <summary>
        /// 抓手状态
        /// </summary>
        public byte RobotGripStatus
        {
            get { return (byte)GetValue(RobotGripStatusProperty); }
            set { SetValue(RobotGripStatusProperty, value); }
        }
        /// <summary>
        /// 手臂旋转角度
        /// </summary>
        public short RobotArmRotateAngle
        {
            get { return (short)GetValue(RobotArmRotateAngleProperty); }
            set { SetValue(RobotArmRotateAngleProperty, value); }
        }
        /// <summary>
        /// 手臂位置
        /// </summary>
        public short RobotArmPositon
        {
            get { return (short)GetValue(RobotArmPositionProperty); }
            set { SetValue(RobotArmPositionProperty, value); }
        }
        /// <summary>
        /// 手臂最大位移
        /// </summary>
        public double WorkAnimationWidth
        {
            get { return (double)GetValue(WorkAnimationWidthProperty); }
            set { SetValue(WorkAnimationWidthProperty, value); }
        }
        #endregion

        public SystemType SFType
        {
            get { return (SystemType)GetValue(SFProperty); }
            set { SetValue(SFProperty, value); }
        }

        public SystemType DRType
        {
            get { return (SystemType)GetValue(DRProperty); }
            set { SetValue(DRProperty, value); }
        }

        public int RowsCnt
        {
            get { return rows; }
        }

        #region 钻台面参数
        List<DrillModel> drDrillCountList = new List<DrillModel>();// 各个指梁所拥有的钻杆数目
        //private int drFirstHeight = 25;// 第一行高度
        //private int drHeight = 20; // 每行高度
        private int drSpace = 45; // 间距
        private int drcarMaxPosistion = 0;
        private int drcarMinPosistion = 0;
        private int drarmMaxPosistion = 0;

        public static readonly DependencyProperty DRRowHeightProperty = DependencyProperty.Register("DRRowHeight", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0));
        public static readonly DependencyProperty DRFirstRowHeightProperty = DependencyProperty.Register("DRFirstRowHeight", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0));
        public static readonly DependencyProperty DRCurrentPointFingerBeamNumberProperty = DependencyProperty.Register("DRCurrentPointFingerBeamNumber", typeof(byte), typeof(AminationNew), new PropertyMetadata((byte)0));//当前所移动到的指梁号反馈
        public static readonly DependencyProperty DROperationModelProperty = DependencyProperty.Register("DROperationModel", typeof(byte), typeof(AminationNew), new PropertyMetadata((byte)0));//操作模式 
        public static readonly DependencyProperty DRPCFingerBeamNumberFeedBackProperty = DependencyProperty.Register("DRPCFingerBeamNumberFeedBack", typeof(byte), typeof(AminationNew), new PropertyMetadata((byte)0));//上位机选择的指梁序号反馈

        public static readonly DependencyProperty DRRealHeightProperty = DependencyProperty.Register("DRRealHeight", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0));// 小车运动实际高度
        public static readonly DependencyProperty DRMiddleHeightProperty = DependencyProperty.Register("DRMiddleHeight", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0));// 中间高度
        public static readonly DependencyProperty DRRobotCarPositionProperty = DependencyProperty.Register("DRRobotCarPosition", typeof(short), typeof(AminationNew), new PropertyMetadata((short)0)); // 小车位置
        public static readonly DependencyProperty DRRobotGripStatusProperty = DependencyProperty.Register("DRRobotGripStatus", typeof(byte), typeof(AminationNew), new PropertyMetadata((byte)0)); // 抓手状态
        public static readonly DependencyProperty DRRobotArmRotateAngleProperty = DependencyProperty.Register("DRRobotArmRotateAngle", typeof(short), typeof(AminationNew), new PropertyMetadata((short)0)); // 手臂旋转
        public static readonly DependencyProperty DRRobotArmPositionProperty = DependencyProperty.Register("DRRobotArmPosition", typeof(short), typeof(AminationNew), new PropertyMetadata((short)0)); // 手臂位置
        public static readonly DependencyProperty DRWorkAnimationWidthProperty = DependencyProperty.Register("DRWorkAnimationWidth", typeof(double), typeof(AminationNew), new PropertyMetadata((double)0.0)); // 手臂最大位移

        /// <summary>
        /// 钻台面-钻杠行高
        /// </summary>
        public double DRRowHeight
        {
            get { return (double)GetValue(DRRowHeightProperty); }
            set { SetValue(DRRowHeightProperty, value); }
        }
        /// <summary>
        /// 钻台面-钻铤行高
        /// </summary>
        public double DRFirstRowHeight
        {
            get { return (double)GetValue(DRFirstRowHeightProperty); }
            set { SetValue(DRFirstRowHeightProperty, value); }
        }
        /// <summary>
        /// 钻台面-当前指梁号
        /// </summary>
        public byte DRCurrentPointFingerBeamNumber
        {
            get { return (byte)GetValue(DRCurrentPointFingerBeamNumberProperty); }
            set { SetValue(DRCurrentPointFingerBeamNumberProperty, value); }
        }
        /// <summary>
        /// 钻台面-操作模式
        /// </summary>
        public byte DROperationModel
        {
            get { return (byte)GetValue(DROperationModelProperty); }
            set { SetValue(DROperationModelProperty, value); }
        }
        /// <summary>
        /// 钻台面-上位机选择的指梁
        /// </summary>
        public byte DRPCFingerBeamNumberFeedBack
        {
            get { return (byte)GetValue(DRPCFingerBeamNumberFeedBackProperty); }
            set { SetValue(DRPCFingerBeamNumberFeedBackProperty, value); }
        }
        /// <summary>
        /// 钻台面-小车位置
        /// </summary>
        public short DRRobotCarPosition
        {
            get { return (short)GetValue(DRRobotCarPositionProperty); }
            set { SetValue(DRRobotCarPositionProperty, value); }
        }
        ///<summary>
        /// 钻台面-小车位移实际高度
        /// </summary>
        public double DRRealHeight
        {
            get { return (double)GetValue(DRRealHeightProperty); }
            set { SetValue(DRRealHeightProperty, value); }
        }

        ///<summary>
        /// 钻台面-中间高度--减去用于置于零点
        /// </summary>
        public double DRMiddleHeight
        {
            get { return (double)GetValue(DRMiddleHeightProperty); }
            set { SetValue(DRMiddleHeightProperty, value); }
        }
        /// <summary>
        /// 钻台面-抓手状态
        /// </summary>
        public byte DRRobotGripStatus
        {
            get { return (byte)GetValue(DRRobotGripStatusProperty); }
            set { SetValue(DRRobotGripStatusProperty, value); }
        }
        /// <summary>
        /// 钻台面-手臂旋转角度
        /// </summary>
        public short DRRobotArmRotateAngle
        {
            get { return (short)GetValue(DRRobotArmRotateAngleProperty); }
            set { SetValue(DRRobotArmRotateAngleProperty, value); }
        }
        /// <summary>
        /// 钻台面-手臂位置
        /// </summary>
        public short DRRobotArmPositon
        {
            get { return (short)GetValue(DRRobotArmPositionProperty); }
            set { SetValue(DRRobotArmPositionProperty, value); }
        }
        /// <summary>
        /// 钻台面-手臂最大位移
        /// </summary>
        public double DRWorkAnimationWidth
        {
            get { return (double)GetValue(DRWorkAnimationWidthProperty); }
            set { SetValue(DRWorkAnimationWidthProperty, value); }
        }
        #endregion

        public AminationNew()
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
                this.imageElevatorStatus.SetBinding(Image.SourceProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["164ElevatorStatus"], Mode = BindingMode.OneWay, Converter = new ElevatorStatusConverter() });
                this.SetBinding(OperationModelProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["operationModel"], Mode = BindingMode.OneWay });//操作模式   
                this.SetBinding(PCFingerBeamNumberFeedBackProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["pcFingerBeamNumberFeedback"], Mode = BindingMode.OneWay });//当前所移动到的指梁号反馈
                this.SetBinding(RobotCarPositionProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["carRealPosition"], Mode = BindingMode.OneWay });//小车实际位置
                this.SetBinding(RobotGripStatusProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["gripStatus"], Mode = BindingMode.OneWay });//抓手的18种状态
                this.SetBinding(RobotArmPositionProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["armRealPosition"], Mode = BindingMode.OneWay });//手臂的实际位置
                this.SetBinding(RobotArmRotateAngleProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["callAngle"], Mode = BindingMode.OneWay, Converter = new CallAngleConverter() });//回转电机的角度值
                this.SetBinding(CurrentPointFingerBeamNumberProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["116E1E2E4RobotPointFingerBeam"], Mode = BindingMode.OneWay });//当前所移动到的指梁号反馈
                #region 二层台参数
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
                leftDrillMultiBind.ConverterParameter = "RP L:";
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

                #region 初始化二层台钻杠/钻铤数量
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
                #region 初始化钻台面钻杠/钻铤数量
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
                this.SetBinding(DROperationModelProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["droperationModel"], Mode = BindingMode.OneWay });//操作模式  
                this.SetBinding(DRRobotCarPositionProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drCarPos"], Mode = BindingMode.OneWay });//小车实际位置
                this.SetBinding(DRRobotGripStatusProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drgripStatus"], Mode = BindingMode.OneWay });//抓手的18种状态
                this.SetBinding(DRRobotArmRotateAngleProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drRotePos"], Mode = BindingMode.OneWay, Converter = new DRCallAngleConverter() });//回转电机的角度值
                this.SetBinding(DRRobotArmPositionProperty, new Binding("ShortTag") { Source = GlobalData.Instance.da["drArmPos"], Mode = BindingMode.OneWay });//手臂的实际位置
                this.SetBinding(DRCurrentPointFingerBeamNumberProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drSelectDrill"], Mode = BindingMode.OneWay });//当前所移动到的指梁号反馈
                this.SetBinding(DRPCFingerBeamNumberFeedBackProperty, new Binding("ByteTag") { Source = GlobalData.Instance.da["drPCSelectDrill"], Mode = BindingMode.OneWay });//当前所移动到的指梁号反馈

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
                leftFPDrillMultiBind.ConverterParameter = "FP L:";
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
                RightFPDrillMultiBind.ConverterParameter = "R:";
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
                foreach (Border bd in FindVisualChildren<Border>(this.gdMidMain))
                {
                    if (bd.Tag != null && bd.Tag.ToString() == "space") bdSpace.Add(bd);
                    if (bd.Tag != null && bd.Tag.ToString() == "space1") bdSpace1.Add(bd);
                }
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
            }
        }

        /// <summary>
        /// 初始化钻台信息
        /// </summary>
        public void InitRowsColoms(SystemType systemType)
        {
            GetConfigPara(systemType);//读取配置文件
            InitHeightAndWidth(systemType);
            //ReCalLayoutSize(systemType); // 重画二层台/钻台面
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="systemType"></param>
        /// <returns></returns>
        private int GetConfigPara(SystemType systemType)
        {
            try
            {
                if (System.IO.File.Exists(configPath))
                {
                    StringBuilder sb = new StringBuilder(STRINGMAX);
                    string strColoms = "10";
                    string strSpace = "45";
                    string strrow = "0";
                    string strCarMaxPosistion = "0";
                    string strCarMinPosistion = "0";
                    string strArmMaxPosistion = "0";

                    rows = GlobalData.Instance.Rows;

                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "COLOMS", strColoms, sb, STRINGMAX, configPath);
                    strColoms = sb.ToString();
                    int.TryParse(strColoms, out coloms); 
                    drillCnt = GlobalData.Instance.DrillNum; // 最大钻铤数量

                    if (coloms == 0) coloms = 17;
                    if (rows == 0)
                    {
                        WinAPI.GetPrivateProfileString("SECONDFLOOR", "ROWS", strrow, sb, STRINGMAX, configPath);
                        strrow = sb.ToString();
                        int.TryParse(strrow, out rows);
                    }
                    if (rows == 0) rows = 10;
                    if (drillCnt == 0) drillCnt = 5;

                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "SPACE", strSpace, sb, STRINGMAX, configPath);
                    strSpace = sb.ToString();
                    int.TryParse(strSpace, out space);

                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "CARMAXPOSISTION", strCarMaxPosistion, sb, STRINGMAX, configPath);
                    strCarMaxPosistion = sb.ToString();
                    int.TryParse(strCarMaxPosistion, out carMaxPosistion);
                    GlobalData.Instance.CarMaxPosistion = carMaxPosistion;

                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "CARMINPOSISTION", strCarMinPosistion, sb, STRINGMAX, configPath);
                    strCarMinPosistion = sb.ToString();
                    int.TryParse(strCarMinPosistion, out carMinPosistion);
                    GlobalData.Instance.CarMinPosistion = carMinPosistion;

                    WinAPI.GetPrivateProfileString("SECONDFLOOR", "ARMMAXPOSISTION", strArmMaxPosistion, sb, STRINGMAX, configPath);
                    strArmMaxPosistion = sb.ToString();
                    int.TryParse(strArmMaxPosistion, out armMaxPosistion);
                    GlobalData.Instance.ArmMaxPosistion = armMaxPosistion;
                    //// 小车实际移动距离 = 钻铤行高度（this.FirstRowHeight + 8 ）+钻杠行高度*行数 - 小车大小
                    //RealHeight = this.FirstRowHeight + 8 + (rowHeight + 5) * rows - GlobalData.Instance.CarSize;
                    //// 中间高度 = （实际移动距离+最上方横梁）/2
                    //MiddleHeight = (RealHeight + 8) / 2.0;
                    //// X轴最大位移 指梁长度 + 间距+中间厚度
                    //WorkAnimationWidth = 170 + space + 5;

                    #region 读取钻台面
                    //string drstrHeight = "20";
                    //string drstrFirstHeight = "25";
                    string drstrSpace = "45";
                    string drstrCarMaxPosistion = "0";
                    string drstrCarMinPosistion = "0";
                    string drstrArmMaxPosistion = "0";

                    //// 钻杠高度
                    //WinAPI.GetPrivateProfileString("DRILLFLOOR", "HEIGHT", drstrHeight, sb, STRINGMAX, configPath);
                    //drstrHeight = sb.ToString();
                    //int.TryParse(drstrHeight, out drHeight);
                    //this.DRRowHeight = (double)drHeight;
                    //// 钻铤高度
                    //WinAPI.GetPrivateProfileString("DRILLFLOOR", "FIRSTHEIGHT", drstrFirstHeight, sb, STRINGMAX, configPath);
                    //drstrFirstHeight = sb.ToString();
                    //int.TryParse(drstrFirstHeight, out drFirstHeight);
                    //this.DRFirstRowHeight = (double)drFirstHeight;
                    // 间距
                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "SPACE", drstrSpace, sb, STRINGMAX, configPath);
                    drstrSpace = sb.ToString();
                    int.TryParse(drstrSpace, out drSpace);

                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "CARMAXPOSISTION", drstrCarMaxPosistion, sb, STRINGMAX, configPath);
                    drstrCarMaxPosistion = sb.ToString();
                    int.TryParse(drstrCarMaxPosistion, out drcarMaxPosistion);
                    GlobalData.Instance.DRCarMaxPosistion = drcarMaxPosistion;

                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "CARMINPOSISTION", drstrCarMinPosistion, sb, STRINGMAX, configPath);
                    drstrCarMinPosistion = sb.ToString();
                    int.TryParse(drstrCarMinPosistion, out drcarMinPosistion);
                    GlobalData.Instance.DRCarMinPosistion = drcarMinPosistion;

                    WinAPI.GetPrivateProfileString("DRILLFLOOR", "ARMMAXPOSISTION", drstrArmMaxPosistion, sb, STRINGMAX, configPath);
                    drstrArmMaxPosistion = sb.ToString();
                    int.TryParse(drstrArmMaxPosistion, out drarmMaxPosistion);
                    GlobalData.Instance.DRArmMaxPosistion = drarmMaxPosistion;
                    //// 小车实际移动距离 = 钻铤行高度（this.FirstRowHeight + 8 ）+钻杠行高度*行数 - 小车大小
                    //DRRealHeight = this.DRFirstRowHeight + 8 + (drHeight + 5) * rows - GlobalData.Instance.CarSize;
                    //// 中间高度 = （实际移动距离+最上方横梁）/2
                    //DRMiddleHeight = (DRRealHeight + 8) / 2.0;
                    //// X轴最大位移 指梁长度 + 间距+中间厚度
                    //DRWorkAnimationWidth = 170 + drSpace + 5;
                    #endregion

                    //if (systemType == SystemType.SecondFloor)
                    //{
                    //    this.TBRowHeight = this.RowHeight + 5;
                    //    this.TBFirstRowHeight = this.FirstRowHeight + 5;
                    //}
                    //else if (systemType == SystemType.DrillFloor)
                    //{
                    //    this.TBRowHeight = this.DRRowHeight + 5;
                    //    this.TBFirstRowHeight = this.DRFirstRowHeight + 5;
                    //}
                    return 1;
                }
                else
                {
                    return 0;//配置文件不存在
                }
            }
            catch (Exception e)
            {
                DataHelper.AddErrorLog(e);
                return -1;//出现异常情况
            }
        }

        /// <summary>
        /// 初始化行高/行宽
        /// </summary>
        private void InitHeightAndWidth(SystemType systemType)
        {
            try
            {
                CalHeight();
                CalWidth();
                CalCarInfo();
                LoadCols(systemType);
                LoadRows();
                LoadFingerBeamDrillPipe(systemType);
            }
            catch (Exception ex)
            {
                Log.Log4Net.AddLog(ex.StackTrace, Log.InfoLevel.ERROR);
            }
        }
        /// <summary>
        /// 加载列数
        /// </summary>
        /// <param name="systemType">系统类型</param>
        private void LoadCols(SystemType systemType)
        {
            try
            {
                // 二层台隐藏钻台面列
                if (systemType == SystemType.SecondFloor)
                {
                    this.spLeftFPCol.Visibility = Visibility.Collapsed;
                    this.spRightFPCol.Visibility = Visibility.Collapsed;
                    this.spLeftRPCol.Visibility = Visibility.Visible;
                    this.spRightRPCol.Visibility = Visibility.Visible;
                }//钻台面隐藏二层台列
                else if (systemType == SystemType.DrillFloor)
                {
                    this.spLeftRPCol.Visibility = Visibility.Collapsed;
                    this.spRightRPCol.Visibility = Visibility.Collapsed;
                    this.spLeftFPCol.Visibility = Visibility.Visible;
                    this.spRightFPCol.Visibility = Visibility.Visible;
                }
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
                #region 隐藏两侧多余行数
                HiddenTBInSP(this.spOneCol); // 隐藏第一列-左边二层台行数
                HiddenTBInSP(this.spTwoCol); // 隐藏第二列-左边钻台面行数
                HiddenTBInSP(this.spThreeCol); // 隐藏第三列-左边行数
                HiddenTBInSP(this.spFourCol); // 隐藏第四列-右边行数
                HiddenTBInSP(this.spFiveCol); // 隐藏第五列-右边钻台面行数
                HiddenTBInSP(this.spSixCol); // 隐藏第六列-右边二层台行数
                #endregion
                #region 隐藏中间行数
                HiddenDPInSP(this.spSFMid); // 隐藏二层台行数
                HiddenDPInSP(this.spDRMid); // 隐藏钻台面行数
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
                double TotalHight = 300.0 - 8.0; // 中间台面总高度-最上面横梁高度
                double avgHeight = (TotalHight-5) / (rows + 1); // 平均高度为(总高度-钻铤行多的高度)/(指梁行数+钻铤行)
                this.TBRowHeight = avgHeight;
                this.TBFirstRowHeight = avgHeight + 5;
                this.RowHeight = avgHeight - 3;
                this.FirstRowHeight = avgHeight;
                this.DRRowHeight = avgHeight - 3;
                this.DRFirstRowHeight = avgHeight;
                if (RowHeight < 20)
                {
                    this.RobotCar.Height = RowHeight;
                    this.RobotCar.Width = RowHeight;
                    this.RobotArm.Height = RowHeight;
                    this.RobotArm.Width = RowHeight;

                    this.drRobotCar.Height = RowHeight;
                    this.drRobotCar.Width = RowHeight;
                    this.DRRobotArm.Height = RowHeight;
                    this.DRRobotArm.Width = RowHeight;
                }
                else
                {
                    this.RobotCar.Height = 20;
                    this.RobotCar.Width = 20;
                    this.RobotArm.Height = 20;
                    this.RobotArm.Width = 20;
                    this.drRobotCar.Height = 20;
                    this.drRobotCar.Width = 20;
                    this.DRRobotArm.Height = 20;
                    this.DRRobotArm.Width = 20;
                }
            }
            catch(Exception ex)
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
                bdSpace.ForEach(f => f.Width = 45 + this.space);
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
                this.RealHeight = 300.0 - 8.0 - this.RobotCar.Height; // 小车上下行走距离=总高度-最上面横梁高度-小车高度
                this.MiddleHeight = (RealHeight + 8) / 2.0; // 中间高度 = （实际移动距离+最上方横梁）/2
                this.WorkAnimationWidth = 170 + 5 + 45 + space; // X轴最大位移 指梁长度 + 间距+中间厚度 + 补偿宽度

                this.DRRealHeight = 300.0 - 8.0 - this.RobotCar.Height; // 小车上下行走距离=总高度-最上面横梁高度-小车高度
                this.DRMiddleHeight = (RealHeight + 8) / 2.0; // 中间高度 = （实际移动距离+最上方横梁）/2
                this.DRWorkAnimationWidth = 170 + 5 + 45 + space; // X轴最大位移 指梁长度 + 间距+中间厚度 + 补偿宽度
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
        private void HiddenTBInSP(StackPanel sp)
        {
            foreach (Grid gd in FindVisualChildren<Grid>(sp))
            {
                if (gd.Name.Contains("row"))
                {
                    string result = Regex.Replace(gd.Name, @"[^0-9]+", "");
                    int iRow = -1;
                    int.TryParse(result, out iRow);
                    if (iRow > rows)
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
        /// 隐藏StackPanel中DockPanel
        /// </summary>
        /// <param name="sp">待隐藏的StackPanel</param>
        private void HiddenDPInSP(StackPanel sp)
        {
            foreach (DockPanel dp in FindVisualChildren<DockPanel>(sp))
            {
                if (dp.Name.Contains("row"))
                {
                    string result = Regex.Replace(dp.Name, @"[^0-9]+", "");
                    int iRow = -1;
                    int.TryParse(result, out iRow);
                    if (iRow > rows)
                    {
                        dp.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        dp.Visibility = Visibility.Visible;
                    }
                }
            }
        }
        /// <summary>
        /// 切换系统
        /// </summary>
        /// <param name="systemType"></param>
        public void SystemChange(SystemType systemType)
        {
            this.SFType = systemType;
            this.DRType = systemType;
        }

        /// <summary>
        /// 加载钻杠
        /// </summary>
        public void LoadFingerBeamDrillPipe(SystemType systemType)
        {
            try
            {
                int fingerBeamNumber = 0;
                int fingerBeamDrillPipeCount = 0;
                GlobalData.Instance.DrillLeftTotal = this.drDrillCountList.Where(w => w.LorR == "left").Sum(s => s.Num);
                if (systemType == SystemType.SecondFloor) // 二层台
                {
                    // 非参数配置界面并且非补偿模式-145-176字节读取指梁钻杆数目
                    if (GlobalData.Instance.da["Con_Set0"].Value.Byte != 23 && GlobalData.Instance.da["operationModel"].Value.Byte != 9)
                    {
                        foreach (var model in FingerBeamDrillPipeCountList)
                        {
                            if (model.Num != GlobalData.Instance.da[model.Name].Value.Byte) // 钻杠数量改变
                            {
                                model.Num = GlobalData.Instance.da[model.Name].Value.Byte;
                                Regex regexFingerBeam = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                                Match match = regexFingerBeam.Match(model.Name);//找出所在的行数
                                if (match.Success)
                                {
                                    fingerBeamNumber = int.Parse(match.Groups[1].Value);
                                    fingerBeamDrillPipeCount = (int)(GlobalData.Instance.da[model.Name].Value.Byte);
                                    SetDrillPipeCountVisible(fingerBeamNumber, fingerBeamDrillPipeCount, systemType);
                                }
                            }
                        }
                    }
                }
                else if (systemType == SystemType.DrillFloor) // 钻台面
                {
                    if (GlobalData.Instance.da["drPageNum"].Value.Byte == 30 || GlobalData.Instance.da["drPageNum"].Value.Byte == 33)
                    {
                        foreach (var model in this.drDrillCountList)
                        {
                            if (model.Num != GlobalData.Instance.da[model.Name].Value.Byte) // 钻杠数量改变
                            {
                                model.Num = GlobalData.Instance.da[model.Name].Value.Byte;
                                Regex regexFingerBeam = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
                                Match match = regexFingerBeam.Match(model.Name);//找出所在的行数
                                if (match.Success)
                                {
                                    fingerBeamNumber = int.Parse(match.Groups[1].Value);
                                    fingerBeamDrillPipeCount = (int)(GlobalData.Instance.da[model.Name].Value.Byte);
                                    SetDrillPipeCountVisible(fingerBeamNumber, fingerBeamDrillPipeCount, systemType);
                                }
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
        /// <param name="fingerBeamDrillPipeCount">钻杠数</param>
        public void SetDrillPipeCountVisible(int fingerBeamNumber, int fingerBeamDrillPipeCount, SystemType systemType)
        {
            try
            {
                if (fingerBeamNumber < 1 || fingerBeamNumber > 32 || fingerBeamDrillPipeCount < 0)
                {
                    return;
                }
                double width = 0.0; // 显示的钻杠/钻铤宽度
                double margin = 0.0; //边距
                if (systemType == SystemType.SecondFloor)
                {
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

                        #region 钻杠设置
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
                else if (systemType == SystemType.DrillFloor)
                {
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

                        #region 钻杠设置
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
