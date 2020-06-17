using System;

namespace HBGKTest
{
    #region 显示模式枚举
    /// <summary>
    /// 显示模式枚举
    /// </summary>
    public enum EnumCCTVVideoMode
    {
        /// <summary>
        /// 单画面显示
        /// </summary>
        ModeOne = 1,
        /// <summary>
        /// 三画面显示
        /// </summary>
        ModeThree = 3,
        /// <summary>
        /// 四画面显示
        /// </summary>
        ModeFour = 4,
        /// <summary>
        /// 九画面显示
        /// </summary>
        ModeNine = 9,
        /// <summary>
        /// 十六画面显示
        /// </summary>
        ModeSixteen = 16,
        /// <summary>
        /// 十八画面显示
        /// </summary>
        ModeEighteen = 18

    }
    #endregion

    #region 视频控制类型的枚举
    /// <summary>
    /// 视频控制类型的枚举
    /// </summary>
    public enum EnumCCTVControlType
    {
        /// <summary>
        /// 无效命令
        /// </summary>
        INVALID_COMMAND=0,

        /// <summary>
        /// 向上
        /// </summary>
        TILT_UP,
        /// <summary>
        /// 向下
        /// </summary>
        TILT_DOWN,
        /// <summary>
        /// 向左
        /// </summary>
        PAN_LEFT,
        /// <summary>
        /// 向右
        /// </summary>
        PAN_RIGHT,
        /// <summary>
        /// 缩小
        /// </summary>
        ZOOM_OUT,
        /// <summary>
        /// 放大
        /// </summary>
        ZOOM_IN,
        /// <summary>
        /// 光圈放大
        /// </summary>
        IRIS_OPEN,
        /// <summary>
        /// 光圈缩小
        /// </summary>
        IRIS_CLOSE,
        /// <summary>
        /// 焦距放大
        /// </summary>
        FOCUS_FAR,
        /// <summary>
        /// 焦距缩小
        /// </summary>
        FOCUS_NEAR,
        /// <summary>
        /// 停止操作
        /// </summary>
        STOP,
        /// <summary>
        /// 设置预置位
        /// </summary>
        SET_PRESET,
        /// <summary>
        /// 调用预置位
        /// </summary>
        GO_PRESET,
        /// <summary>
        /// 选择某个摄像机
        /// </summary>
        CAM_SELECT,
        /// <summary>
        /// 选择下一个摄像机
        /// </summary>
        CAM_NEXT,
        /// <summary>
        /// 选择上一个摄像机
        /// </summary>
        CAM_PREV,
        /// <summary>
        /// 左上
        /// </summary>
        Pan_NW,
        /// <summary>
        /// 右上
        /// </summary>
        Pan_NE,
        /// <summary>
        /// 右下
        /// </summary>
        Pan_SE,
        /// <summary>
        /// 左下
        /// </summary>
        Pan_SW
    }

    #endregion

    #region 编码器类型

    public enum EncoderType
    {
        BOKANG = 1,
        ADV = 2,  //先进视讯3.0 网站方式
        OMT = 3,
        DAHUA = 4,  //大华
        DALI = 5,       //大立
        HAIKANG = 6,      //海康
        H3C = 7,  //华三Ivs
        SLT = 8,    //湖南志诚冠军
        TianDySS = 9,    //天地伟业流媒体服务器
        ALK = 10,
        BlueStar = 11,     //蓝色星际控件，仅能视频浏览
        BOKANGDG = 12,
        Ppvsguard = 13,//四川宜宾的gps车辆上的视频，仅看不控制
        PuTian = 14,//四川宜宾
        NanWang = 15,//南望
        NetPosa = 16, //东方网力
        H3cIMos = 17,//华三IMos
        VoxCvsa = 18, //蛙式CVSA
        DaHuaCom = 19,  //大华Com
        JingSL = 20,  //金三立DVR/DVS
        JingSLserver = 21,  //金三立转发服务器
        IDVR50 = 22,  //先进视讯IDVR5.0
        NrCap = 23,   //安徽创世
        ZxNvms = 24,    //中兴
        GA1 = 25,  //公安部一所
        ZCPlat = 26,    //志成冠军网络sdk
        DaHuaOcx = 27,  //大华Ocx
        Sunpanel = 28,  //磐晟
        ZXing = 29,//中星
        WinVroad = 30,//威路特
        ZxViss = 31,     //中星视频流控件
        Pelco = 32, //派尔高
        DaHeng = 33, //派尔高
        API = 34,//德锐视
        ZhongDian = 35,//中电新发
        LPT = 36,  //罗普特
        LPT_OCX = 37,  //罗普特
        HBGK = 38,  //汉邦高科
    }

    #endregion

    #region 控制控件类型

    public enum ControlType
    {
        Master = 1,
        BOKANG,
        ADV,
        OMT,
        HAIKANG
    }

    #endregion
}
