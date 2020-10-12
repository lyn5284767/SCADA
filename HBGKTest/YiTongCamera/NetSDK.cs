using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SDK_HANDLE = System.Int32;

namespace HBGKTest
{
    enum SearchModeType
    {
        DDNS_SERIAL = 0,//按序列号
        DDNS_USERNAME,  //按用户名
    }
    public enum SDK_RET_CODE
    {
        H264_DVR_NOERROR = 0,					//没有错误
        H264_DVR_SUCCESS = 1,					//返回成功
        H264_DVR_SDK_NOTVALID = -10000,				//非法请求
        H264_DVR_NO_INIT = -10001,				//SDK未经初始化
        H264_DVR_ILLEGAL_PARAM = -10002,			//用户参数不合法
        H264_DVR_INVALID_HANDLE = -10003,			//句柄无效
        H264_DVR_SDK_UNINIT_ERROR = -10004,			//SDK清理出错
        H264_DVR_SDK_TIMEOUT = -10005,			//等待超时
        H264_DVR_SDK_MEMORY_ERROR = -10006,			//内存错误，创建内存失败
        H264_DVR_SDK_NET_ERROR = -10007,			//网络错误
        H264_DVR_SDK_OPEN_FILE_ERROR = -10008,			//打开文件失败
        H264_DVR_SDK_UNKNOWNERROR = -10009,			//未知错误
        H264_DVR_DEV_VER_NOMATCH = -11000,			//收到数据不正确，可能版本不匹配
        H264_DVR_SDK_NOTSUPPORT = -11001,			//版本不支持

        H264_DVR_OPEN_CHANNEL_ERROR = -11200,			//打开通道失败
        H264_DVR_CLOSE_CHANNEL_ERROR = -11201,			//关闭通道失败
        H264_DVR_SUB_CONNECT_ERROR = -11202,			//建立媒体子连接失败
        H264_DVR_SUB_CONNECT_SEND_ERROR = -11203,			//媒体子连接通讯失败
        /// 用户管理部分错误码
        H264_DVR_NOPOWER = -11300,			//无权限
        H264_DVR_PASSWORD_NOT_VALID = -11301,			// 账号密码不对
        H264_DVR_LOGIN_USER_NOEXIST = -11302,			//用户不存在
        H264_DVR_USER_LOCKED = -11303,			// 该用户被锁定
        H264_DVR_USER_IN_BLACKLIST = -11304,			// 该用户不允许访问(在黑名单中)
        H264_DVR_USER_HAS_USED = -11305,			// 该用户已登陆
        H264_DVR_USER_NOT_LOGIN = -11306,			// 该用户没有登陆
        H264_DVR_CONNECT_DEVICE_ERROR = -11307,			//可能设备不存在
        H264_DVR_ACCOUNT_INPUT_NOT_VALID = -11308,			//用户管理输入不合法
        H264_DVR_ACCOUNT_OVERLAP = -11309,			//索引重复
        H264_DVR_ACCOUNT_OBJECT_NONE = -11310,			//不存在对象, 用于查询时
        H264_DVR_ACCOUNT_OBJECT_NOT_VALID = -11311,			//不存在对象
        H264_DVR_ACCOUNT_OBJECT_IN_USE = -11312,			//对象正在使用
        H264_DVR_ACCOUNT_SUBSET_OVERLAP = -11313,			//子集超范围 (如组的权限超过权限表，用户权限超出组的权限范围等等)
        H264_DVR_ACCOUNT_PWD_NOT_VALID = -11314,			//密码不正确
        H264_DVR_ACCOUNT_PWD_NOT_MATCH = -11315,			//密码不匹配
        H264_DVR_ACCOUNT_RESERVED = -11316,			//保留帐号
        /// 配置管理相关错误码

        H264_DVR_OPT_RESTART = -11400,			// 保存配置后需要重启应用程序
        H264_DVR_OPT_REBOOT = -11401,			// 需要重启系统
        H264_DVR_OPT_FILE_ERROR = -11402,			// 写文件出错
        H264_DVR_OPT_CAPS_ERROR = -11403,			// 配置特性不支持
        H264_DVR_OPT_VALIDATE_ERROR = -11404,			// 配置校验失败
        H264_DVR_OPT_CONFIG_NOT_EXIST = -11405,			// 请求或者设置的配置不存在
        /// 
        H264_DVR_CTRL_PAUSE_ERROR = -11500,			//暂停失败
        H264_DVR_SDK_NOTFOUND = -11501,			//查找失败，没有找到对应文件
        H264_DVR_CFG_NOT_ENABLE = -11502,           //配置未启用
        H264_DVR_DECORD_FAIL = -11503,           //配置未启用
        //DNS协议解析返回错误码
        H264_DVR_SOCKET_ERROR = -11600,         //创建套节字失败
        H264_DVR_SOCKET_CONNECT = -11601,         //连接套节字失败
        H264_DVR_SOCKET_DOMAIN = -11602,         //域名解析失败
        H264_DVR_SOCKET_SEND = -11603,         //发送数据失败
    }
    public enum SDK_CONFIG_TYPE
    {
        E_SDK_CONFIG_NOTHING = 0,
        E_SDK_CONFIG_USER,					//用户信息，包含了权限列表，用户列表和组列表   对应结构体USER_MANAGE_INFO
        E_SDK_CONFIG_ADD_USER,				//增加用户---对应结构体USER_INFO
        E_SDK_CONFIG_MODIFY_USER,			//修改用户---对应结构体CONF_MODIFYUSER
        E_SDK_CONFIG_DELETE_USER,			//对应结构体USER_INFO
        E_SDK_CONFIG_ADD_GROUP,				//增加组---对应结构体USER_GROUP_INFO
        E_SDK_CONFIG_MODIFY_GROUP,			//修改组---对应结构体CONF_MODIFYGROUP
        E_SDK_COFIG_DELETE_GROUP,			//对应结构体---USER_GROUP_INFO
        E_SDK_CONFIG_MODIFY_PSW,			//修改密码---对应结构体_CONF_MODIFY_PSW
        E_SDK_CONFIG_ABILITY_SYSFUNC = 9,	//支持的网络功能---对应结构体SDK_SystemFunction
        E_SDK_CONFIG_ABILTY_ENCODE = 10,	//首先获得编码能力---对应结构体CONFIG_EncodeAbility
        E_SDK_CONFIG_ABILITY_PTZPRO,		//云台协议---对应结构体SDK_PTZPROTOCOLFUNC
        E_SDK_COMFIG_ABILITY_COMMPRO,		//串口协议---对应结构体SDK_COMMFUNC
        E_SDK_CONFIG_ABILITY_MOTION_FUNC,	//动态检测块---对应结构体SDK_MotionDetectFunction
        E_SDK_CONFIG_ABILITY_BLIND_FUNC,	//视频遮挡块---对应结构体SDK_BlindDetectFunction
        E_SDK_CONFIG_ABILITY_DDNS_SERVER,	//DDNS服务支持类型---对应结构体SDK_DDNSServiceFunction
        E_SDK_CONFIG_ABILITY_TALK,			//对讲编码类型---对应结构体SDK_DDNSServiceFunction
        E_SDK_CONFIG_SYSINFO = 17,			//系统信息---对应结构体H264_DVR_DEVICEINFO
        E_SDK_CONFIG_SYSNORMAL,				//普通配置---对应结构体SDK_CONFIG_NORMAL
        E_SDK_CONFIG_SYSENCODE,				//编码配置---对应结构体SDK_EncodeConfigAll
        E_SDK_CONFIG_SYSNET = 20,			//网络设置---对应结构体SDK_CONFIG_NET_COMMON
        E_SDK_CONFIG_PTZ,					//云台页面---对应结构体SDK_STR_PTZCONFIG_ALL
        E_SDK_CONFIG_COMM,					//串口页面---对应结构体SDK_CommConfigAll
        E_SDK_CONFIG_RECORD,				//录像设置界面---对应结构体SDK_RECORDCONFIG
        E_SDK_CONFIG_MOTION,				//动态检测页面---对应结构体SDK_MOTIONCONFIG
        E_SDK_CONFIG_SHELTER,				//视频遮挡---对应结构体SDK_BLINDDETECTCONFIG
        E_SDK_CONFIG_VIDEO_LOSS,  			//视频丢失---对应结构体SDK_VIDEOLOSSCONFIG
        E_SDK_CONFIG_ALARM_IN,				//报警输入---对应结构体SDK_ALARM_INPUTCONFIG
        E_SDK_CONFIG_ALARM_OUT,				//报警输出---对应结构体SDK_AlarmOutConfigAll
        E_SDK_CONFIG_DISK_MANAGER,			//硬盘管理界面---对应结构体SDK_StorageDeviceControl
        E_SDK_CONFIG_OUT_MODE = 30,			//输出模式界面---对应结构体SDK_VideoWidgetConfigAll
        E_SDK_CONFIG_CHANNEL_NAME,			//通道名称---对应结构体SDK_ChannelNameConfigAll
        E_SDK_CONFIG_AUTO,					//自动维护界面配置---对应结构体SDK_AutoMaintainConfig
        E_SDK_CONFIG_DEFAULT,     			//恢复默认界面配置---对应结构体SDK_SetDefaultConfigTypes
        E_SDK_CONFIG_DISK_INFO,				//硬盘信息---对应结构体SDK_StorageDeviceInformationAll
        E_SDK_CONFIG_LOG_INFO,				//查询日志---对应结构体SDK_LogList
        E_SDK_CONFIG_NET_IPFILTER,			//黑名单配置---对应结构体SDK_NetIPFilterConfig
        E_SDK_CONFIG_NET_DHCP,				//DHCP配置---对应结构体SDK_NetDHCPConfigAll
        E_SDK_CONFIG_NET_DDNS,				//DDNS信息---对应结构体SDK_NetDDNSConfigALL
        E_SDK_CONFIG_NET_EMAIL,				//EMAIL---对应结构体SDK_NetEmailConfig
        E_SDK_CONFIG_NET_MULTICAST = 40,	//组播---对应结构体SDK_NetMultiCastConfig
        E_SDK_CONFIG_NET_NTP,				//NTP---对应结构体SDK_NetNTPConfig
        E_SDK_CONFIG_NET_PPPOE,				//PPPOE---对应结构体SDK_NetPPPoEConfig
        E_SDK_CONFIG_NET_DNS,				//DNS---对应结构体SDK_NetDNSConfig
        E_SDK_CONFIG_NET_FTPSERVER,			//FTP---对应结构体SDK_FtpServerConfig
        E_SDK_CONFIG_SYS_TIME,				//系统时间---对应结构体SDK_SYSTEM_TIME(接口H264_DVR_SetSystemDateTime也可以实现)
        E_SDK_CONFIG_CLEAR_LOG,				//清除日志(接口H264_DVR_ControlDVR)											
        E_SDK_REBOOT_DEV,					//重启启动设备(接口H264_DVR_ControlDVR)												
        E_SDK_CONFIG_ABILITY_LANG,			//支持语言---对应结构体SDK_MultiLangFunction
        E_SDK_CONFIG_VIDEO_FORMAT,
        E_SDK_CONFIG_COMBINEENCODE = 50,	//组合编码---对应结构体SDK_CombineEncodeConfigAll
        E_SDK_CONFIG_EXPORT,				//配置导出														
        E_SDK_CONFIG_IMPORT,				//配置导入
        E_SDK_LOG_EXPORT,					//日志导出														
        E_SDK_CONFIG_COMBINEENCODEMODE, 	//组合编码模式---对应结构体SDK_CombEncodeModeAll
        E_SDK_WORK_STATE,					//运行状态---SDK_DVR_WORKSTATE(接口H264_DVR_GetDVRWorkState也可以获取)															
        E_SDK_ABILITY_LANGLIST, 			//实际支持的语言集---对应结构体SDK_MultiLangFunction									
        E_SDK_CONFIG_NET_ARSP,				//ARSP---对应结构体SDK_NetARSPConfigAll
        E_SDK_CONFIG_SNAP_STORAGE,			//抓图设置---对应结构体SDK_SnapshotConfig
        E_SDK_CONFIG_NET_3G, 				//3G拨号---对应结构体SDK_Net3GConfig
        E_SDK_CONFIG_NET_MOBILE = 60, 		//手机监控---对应结构体SDK_NetMoblieConfig
        E_SDK_CONFIG_UPGRADEINFO, 			//获取升级信息/参数/文件名---对应结构体SDK_UpgradeInfo
        E_SDK_CONFIG_NET_DECODER,			//解码器地址设置V1(弃用)---对应结构体SDK_NetDecoderConfigAll
        E_SDK_ABILITY_VSTD, 				//实际支持的视频制式---对应结构体SDK_MultiVstd
        E_SDK_CONFIG_ABILITY_VSTD,			//支持视频制式---对应结构体SDK_MultiVstd
        E_SDK_CONFIG_NET_UPNP, 				//UPUN设置---对应结构体SDK_NetUPNPConfig
        E_SDK_CONFIG_NET_WIFI,				//WIFI---对应结构体SDK_NetWifiConfig
        E_SDK_CONFIG_NET_WIFI_AP_LIST,		//搜索到的wifi列表---对应结构体SDK_NetWifiDeviceAll
        E_SDK_CONFIG_SYSENCODE_SIMPLIIFY, 	//简化的编码配置---对应结构SDK_EncodeConfigAll_SIMPLIIFY
        E_SDK_CONFIG_ALARM_CENTER,  		//告警中心---对应结构体SDK_NetAlarmServerConfigAll
        E_SDK_CONFIG_NET_ALARM = 70,		//网络告警---对应结构体SDK_NETALARMCONFIG_ALL																		
        E_SDK_CONFIG_NET_MEGA,     			//互信互通---对应结构体SDK_CONFIG_NET_MEGA
        E_SDK_CONFIG_NET_XINGWANG, 			//星望---对应结构体SDK_CONFIG_NET_XINGWANG
        E_SDK_CONFIG_NET_SHISOU,   			//视搜---对应结构体SDK_CONFIG_NET_SHISOU
        E_SDK_CONFIG_NET_VVEYE,    			//VVEYE---对应结构体SDK_CONFIG_NET_VVEYE
        E_SDK_CONFIG_NET_PHONEMSG,  		//短信---对应结构体SDK_NetShortMsgCfg
        E_SDK_CONFIG_NET_PHONEMEDIAMSG,  	//彩信---对应结构体SDK_NetMultimediaMsgCfg
        E_SDK_VIDEO_PREVIEW,				//
        E_SDK_CONFIG_NET_DECODER_V2,		//解码器地址设置V2(弃用)---对应结构体SDK_NetDecorderConfigAll_V2
        E_SDK_CONFIG_NET_DECODER_V3,		//解码器地址设置V3---对应结构体SDK_NetDecorderConfigAll_V3
        E_SDK_CONFIG_ABILITY_SERIALNO = 80,	//序列号---对应结构体SDK_AbilitySerialNo(经测试不是设备序列号(暂弃用),序列号可以从登陆接口获取到)
        E_SDK_CONFIG_NET_RTSP,    			//RTSP---对应结构体SDK_NetRTSPConfig
        E_SDK_GUISET,              			//本地GUI输出设置---对应结构体SDK_GUISetConfig
        E_SDK_CATCHPIC,               		//抓图												
        E_SDK_VIDEOCOLOR,             		//视频颜色设置---对应结构体SDK_VideoColorConfigAll
        E_SDK_CONFIG_COMM485,				//串口485协议配置---对应结构体SDK_STR_RS485CONFIG_ALL
        E_SDK_COMFIG_ABILITY_COMMPRO485, 	//串口485协议---对应结构体SDK_COMMFUNC
        E_SDK_CONFIG_SYS_TIME_NORTC,		//设置系统时间noRtc---对应结构体SDK_SYSTEM_TIME
        E_SDK_CONFIG_REMOTECHANNEL,   		//远程通道---弃用
        E_SDK_CONFIG_OPENTRANSCOMCHANNEL, 	//打开透明串口---对应结构体TransComChannel
        E_SDK_CONFIG_CLOSETRANSCOMCHANNEL = 90,//关闭透明串口
        E_SDK_CONFIG_SERIALWIRTE,  			//写入透明串口信息
        E_SDK_CONFIG_SERIALREAD,   			//读取透明串口信息
        E_SDK_CONFIG_CHANNELTILE_DOT,		//点阵信息-修改IPC通道名需要点阵信息---对应结构体SDK_TitleDot
        E_SDK_CONFIG_CAMERA,           		//摄象机参数---对应结构体SDK_CameraParam
        E_SDK_CONFIG_ABILITY_CAMERA,    	//曝光能力级---对应结构体SDK_CameraAbility
        E_SDK_CONFIG_BUGINFO,    			//命令调试													
        E_SDK_CONFIG_STORAGENOTEXIST,		//硬盘不存在---对应结构体SDK_VIDEOLOSSCONFIG
        E_SDK_CONFIG_STORAGELOWSPACE, 		//硬盘容量不足---对应结构体SDK_StorageLowSpaceConfig								
        E_SDK_CONFIG_STORAGEFAILURE, 		//硬盘出错---对应结构体SDK_StorageFailConfig
        E_SDK_CFG_NETIPCONFLICT = 100,   	//IP冲突---对应结构体SDK_VIDEOLOSSCONFIG
        E_SDK_CFG_NETABORT,  				//网络异常---对应结构体SDK_VIDEOLOSSCONFIG
        E_SDK_CONFIG_CHNSTATUS, 			//通道状态---对应结构体SDK_NetDecorderChnStatusAll
        E_SDK_CONFIG_CHNMODE,  				//通道模式---对应结构体SDK_NetDecorderChnModeConfig
        E_SDK_CONFIG_NET_DAS,    			//主动注册---对应结构体SDK_DASSerInfo
        E_SDK_CONFIG_CAR_INPUT_EXCHANGE,    //外部信息输入与车辆状态的对应关系---对应结构体SDK_CarStatusExchangeAll			
        E_SDK_CONFIG_DELAY_TIME,       		//车载系统延时配置---对应结构体SDK_CarDelayTimeConfig
        E_SDK_CONFIG_NET_ORDER,             //网络优先级---对应结构体SDK_NetOrderConfig
        E_SDK_CONFIG_ABILITY_NETORDER, 	//网络优先级设置能力---对应结构体SDK_NetOrderFunction
        E_SDK_CONFIG_CARPLATE,				//车牌号配置---对应结构体SDK_CarPlates
        E_SDK_CONFIG_LOCALSDK_NET_PLATFORM = 110, //网络平台信息设置---对应结构体SDK_LocalSdkNetPlatformConfig
        E_SDK_CONFIG_GPS_TIMING,            //GPS校时相关配置---对应结构体SDK_GPSTimingConfig
        E_SDK_CONFIG_VIDEO_ANALYZE, 		//视频分析(智能DVR)---对应结构体SDK_ANALYSECONFIG
        E_SDK_CONFIG_GODEYE_ALARM,			//神眼接警中心系统---对应结构体SDK_GodEyeConfig
        E_SDK_CONFIG_NAT_STATUS_INFO,   	//nat状态信息---对应结构体SDK_NatStatusInfo	
        E_SDK_CONFIG_BUGINFOSAVE,     		//命令调试(保存)									 
        E_SDK_CONFIG_MEDIA_WATERMARK,		//水印设置---对应结构体SDK_WaterMarkConfigAll
        E_SDK_CONFIG_ENCODE_STATICPARAM,	//编码器静态参数---对应结构体SDK_EncodeStaticParamAll
        E_SDK_CONFIG_LOSS_SHOW_STR,		    //视频丢失显示字符串
        E_SDK_CONFIG_DIGMANAGER_SHOW,	    //通道管理显示配置---对应结构体SDK_DigManagerShowStatus
        E_SDK_CONFIG_ABILITY_ANALYZEABILITY = 120,//智能分析能力---对应结构体SDK_ANALYZEABILITY
        E_SDK_CONFIG_VIDEOOUT_PRIORITY,   //显示HDMI VGA优先级别配置
        E_SDK_CONFIG_NAT,		  		  //NAT功能，MTU值配置---对应结构体SDK_NatConfig
        E_SDK_CONFIG_CPCINFO,		      //智能CPC计数数据信息---对应结构体SDK_CPCDataAll
        E_SDK_CONFIG_STORAGE_POSITION,    // 录像存储设备类型---对应结构体SDK_RecordStorageType
        E_SDK_CONFIG_ABILITY_CARSTATUSNUM,//车辆状态数---对应结构体SDK_CarStatusNum
        E_SDK_CFG_VPN,					//VPN---对应结构体SDK_VPNConfig
        E_SDK_CFG_VIDEOOUT,				//VGA视频分辨率---对应结构体SDK_VGAresolution
        E_SDK_CFG_ABILITY_VGARESOLUTION,//支持的VGA分辨率列表---对应结构体SDK_VGAResolutionAbility
        E_SDK_CFG_NET_LOCALSEARCH,      //搜索设备，设备端的局域网设备---对应结构体SDK_NetDevList
        E_SDK_CFG_NETPLAT_KAINENG = 130,//客户配置---对应结构体SDK_CONFIG_KAINENG_INFO
        E_SDK_CFG_ENCODE_STATICPARAM_V2,//DVR编码器静态参数---对应结构体SDK_EncodeStaticParamV2
        E_SDK_ABILITY_ENC_STATICPARAM,	//静态编码能力集---对应结构体SDK_EncStaticParamAbility (掩码)
        E_SDK_CFG_C7_PLATFORM,          //C7平台配置---对应结构体SDK_C7PlatformConfig
        E_SDK_CFG_MAIL_TEST,            //邮件测试---对应结构体SDK_NetEmailConfig
        E_SDK_CFG_NET_KEYBOARD,         //网络键盘服务---对应结构体SDK_NetKeyboardConfig
        E_SDK_ABILITY_NET_KEYBOARD,		//网络键盘协议---对应结构体SDK_NetKeyboardAbility  
        E_SDK_CFG_SPVMN_PLATFORM,       //28181协议配置---对应结构体SDK_ASB_NET_VSP_CONFIG	
        E_SDK_CFG_PMS,				    //手机服务---对应结构体SDK_PMSConfig
        E_SDK_CFG_OSD_INFO,             //屏幕提示信息---对应结构体SDK_OSDInfoConfigAll
        E_SDK_CFG_KAICONG = 140,        //客户配置---对应结构体SDK_KaiCongAlarmConfig
        E_SDK_CFG_DIGITAL_REAL,			//真正支持的通道模式---对应结构体SDK_VideoChannelManage
        E_SDK_ABILITY_PTZCONTROL,		//PTZ控制能力集---对应结构体SDK_PTZControlAbility
        E_SDK_CFG_XMHEARTBEAT,			//对应结构体SDK_XMHeartbeatConfig
        E_SDK_CFG_MONITOR_PLATFORM,		//平台配置---对应结构体SDK_MonitorPlatformConfig
        E_SDK_CFG_PARAM_EX,				//摄像头扩展参数---对应结构体SDK_CameraParamEx
        E_SDK_CFG_NETPLAT_ANJU_P2P,		//安巨P2P---对应结构体SDK_NetPlatformCommonCfg  
        E_SDK_GPS_STATUS,				//GPS连接信息---对应结构体SDK_GPSStatusInfo 
        E_SDK_WIFI_STATUS,				//Wifi连接信息---对应结构体SDK_WifiStatusInfo
        E_SDK_3G_STATUS,  				//3G连接信息---对应结构体SDK_WirelessStatusInfo
        E_SDK_DAS_STATUS = 150, 		//主动注册状态---对应结构体SDK_DASStatusInfo 
        E_SDK_ABILITY_DECODE_DELEY,		//解码策略能力---对应结构体SDK_DecodeDeleyTimePrame
        E_SDK_CFG_DECODE_PARAM,     	//解码最大延时---对应结构体SDK_DecodeParam
        E_SDK_CFG_VIDEOCOLOR_CUSTOM,    //SDK_VIDEOCOLOR_PARAM_CUSTOM
        E_SDK_ABILITY_ONVIF_SUB_PROTOCOL,//onvif子协议---对应结构体SDK_AbilityMask   
        E_SDK_CONFIG_EXPORT_V2,      	//导出设备默认配置，即出厂的配置
        E_SDK_CFG_CAR_BOOT_TYPE,      	//车载开关机模式---对应结构体SDK_CarBootTypeConfig
        E_SDK_CFG_IPC_ALARM,			//IPC网络报警---对应结构体SDK_IPCAlarmConfigAll
        E_SDK_CFG_NETPLAT_TUTK_IOTC,	//TUTK IOTC平台配置---对应结构体SDK_NetPlatformCommonCfg
        E_SDK_CFG_BAIDU_CLOUD,     		//百度云---对应结构体SDK_BaiduCloudCfg
        E_SDK_CFG_PMS_MSGNUM = 160,		//手机订阅数---对应结构体SDK_PhoneInfoNum
        E_SDK_CFG_IPC_IP,           	//控制DVR去修改设备IP---对应结构体SDK_IPSetCfg
        E_SDK_ABILITY_DIMEN_CODE,       //二维码点阵---对应结构体SDK_DimenCodeAll
        E_SDK_CFG_MOBILE_WATCH, 	 	//中国电信手机看店平台配置---对应结构体SDK_MobileWatchCfg	
        E_SDK_CFG_BROWSER_LANGUAGE,   	//使用浏览器时使用的语言---对应结构体SDK_BrowserLanguageType
        E_SDK_CFG_TIME_ZONE,			//时区配置---对应结构体SDK_TimeZone
        E_SDK_CFG_NETBJTHY,       		//客户配置---对应结构体SDK_MonitorPlatformConfig
        E_SDK_ABILITY_MAX_PRE_RECORD,   //最大可设置预录时间1~30---对应结构体SDK_AbilityMask
        E_SDK_CFG_DIG_TIME_SYN,			//数字通道时间同步配置(决定前端同步方式)---对应结构体SDK_TimeSynParam
        E_SDK_CONFIG_OSDINFO_DOT,		//3行OSD
        E_SDK_CFG_NET_POS = 170,		//POS机配置---对应结构体SDK_NetPosConfigAll
        E_SDK_CFG_CUSTOMIZE_OEMINFO,	//定制OEM客户版本信息---对应结构体SDK_CustomizeOEMInfo
        E_SDK_CFG_DIGITAL_ENCODE, 		//数字通道精简版编码配置---对应结构体SDK_EncodeConfigAll_SIMPLIIFY
        E_SDK_CFG_DIGITAL_ABILITY, 		//数字通道的编码能力---对应结构体SDK_DigitDevInfo
        E_SDK_CFG_ENCODECH_DISPLAY,		//IE端编码配置显示的前端通道号---对应结构体SDK_EncodeChDisplay
        E_SDK_CFG_RESUME_PTZ_STATE,		//开机云台状态---对应结构体SDK_ResumePtzState
        E_SDK_CFG_LAST_SPLIT_STATE,   	//最近一次的画面分割模式，用于重启后恢复之前的分割模式
        E_SDK_CFG_SYSTEM_TIMING_WORK,   //设备定时开关机时间配置。隐藏在自动维护页面里，要用超级密码登陆才能看到界面
        E_SDK_CFG_GBEYESENV,			//宝威环境监测平台配置---对应结构体SDK_NetPlatformCommonCfg
        E_SDK_ABILITY_AHD_ENCODE_L, 	//AHDL能力集---对应结构体SDK_AHDEncodeLMask
        E_SDK_CFG_SPEEDALARM = 180,		//速度报警---对应结构体SDK_SpeedAlarmConfigAll
        E_SDK_CFG_CORRESPONDENT_INFO,	//用户自定义配置---对应结构体SDK_CorrespondentOwnInfo
        E_SDK_SET_OSDINFO,				//OSD信息设置---对应结构体SDK_OSDInfo,(此项功能只支持模拟通道)
        E_SDK_SET_OSDINFO_V2,			//OSD信息叠加，不保存配置---对应结构体SDK_OSDInfoConfigAll，(此项功能只支持模拟通道)
        E_SDK_ABILITY_SUPPORT_EXTSTREAM,//支持辅码流录像---对应结构体SDK_AbilityMask
        E_SDK_CFG_EXT_RECORD,			//辅码流配置---对应结构体SDK_RECORDCONFIG_ALL/SDK_RECORDCONFIG
        E_SDK_CFG_APP_DOWN_LINK,		//用于用户定制下载链接---对应结构体SDK_AppDownloadLink
        E_SDK_CFG_EX_USER_MAP,			//用于保存明文数据---对应结构体SDK_UserMap
        E_SDK_CFG_TRANS_COMM_DATA, 		//串口数据主动上传到UDP或TCP服务器，其中TCP服务器可以支持双向通信---对应结构体SDK_NetTransCommData
        E_SDK_EXPORT_LANGUAGE,			//语言导出
        E_SDK_IMPORT_LANGUAGE = 190,	//语言导入
        E_SDK_DELETE_LANGUAGE,			//语言删除
        E_SDK_CFG_UPGRADE_VERSION_LIST,	//云升级文件列表---对应结构体SDK_CloudUpgradeList
        E_SDK_CFG_GSENSORALARM,			//GSENSOR报警
        E_SDK_CFG_USE_PROGRAM,			//启动客户小程序---对应结构体SDK_NetUseProgram
        E_SDK_CFG_FTP_TEST,             //FTP测试---对应结构体SDK_FtpServerConfig
        E_SDK_CFG_FbExtraStateCtrl,  	//消费类产品的录像灯的状态---对应结构体SDK_FbExtraStateCtrl
        E_SDK_CFG_PHONE,				//手机用
        E_SDK_PicInBuffer,				//手机抓图,弃用 
        E_SDK_GUARD,					//布警 弃用
        E_SDK_UNGUARD = 200,			//撤警，弃用
        E_SDK_CFG_START_UPGRADE,		//开始升级，弃用
        E_SDK_CFG_AUTO_SWITCH,			//插座定时开关---获取配置都用H264_DVR_GetDevConfig_Json,配置使用H264_DVR_SetDevConfig_Json(配置时的格式见智能插座用到的命令.doc)(两个接口简称Json接口,下面用简称) "Name":"PowerSocket.AutoSwitch"
        E_SDK_CFG_POWER_SOCKET_SET,		//控制插座开关---Json接口 "Name":"OPPowerSocketGet"
        E_SDK_CFG_AUTO_ARM,				//插座的定时布撤防---Json接口 "Name":"PowerSocket.AutoArm"
        E_SDK_CFG_WIFI_MODE,			//Wifi模式配置，用于行车记录仪切换AP模式---对应结构体SDK_NetWifiMode
        E_SDK_CFG_CIENT_INFO,			//传递手机客户端信息---Json接口 "Name":"PowerSocket.ClientInfo"
        E_SDK_CFG_ATHORITY,				//SDK_Authority---Json接口 "Name":"PowerSocket.Authority"
        E_SDK_CFG_ARM,					//SDK_Arm---Json接口 "Name":"PowerSocket.Arm"
        E_SDK_CFG_AUTOLIGHT,			//设置夜灯的定时开关功能 --Json接口 "Name" : "PowerSocket.AutoLight",
        E_SDK_CFG_LIGHT = 210,			//使能和禁止夜灯的动检响应功能---Json接口 "Name" : "PowerSocket.Light",
        E_SDK_CFG_WORKRECORD,			//进行电量统计---Json接口 "Name" : "PowerSocket.WorkRecord",
        E_SDK_CFG_SYSTEMTIME,			//设置时间的命令 ,当局域网连接的时候,连接的时候,发送对时命令 --Json接口 "Name":"System.Time"
        E_SDK_CFG_USB,					//usb接口控制功能---Json接口 "Name":"PowerSocket.Usb"
        E_SDK_CFG_NETPLAT_BJHONGTAIHENG,//北京鸿泰恒平台---对应结构体SDK_CONFIG_NET_BJHONGTAIHENG
        E_SDK_CFG_CLOUD_STORAGE,		//云存储相关配置---对应结构体SDK_CloudRecordConfigAll
        E_SDK_CFG_IDLE_PTZ_STATE,       //云台空闲动作相关配置---对应结构体SDK_PtzIdleStateAll
        E_SDK_CFG_CAMERA_CLEAR_FOG,	    //去雾功能配置---对应结构体SDK_CameraClearFogAll
        E_SDK_CFG_WECHATACCOUNT,		//---对应json "Name":"PowerSocket.WechatAccount"
        E_SDK_CFG_WECHATRENEW,			//---对应json "Name":"PowerSocket.WechatRenew" 
        E_SDK_CFG_POWERSOCKET_WIFI = 220,//---对应json "Name":"PowerSocket.WiFi"
        E_SDK_CFG_CAMERA_MOTOR_CONTROL, //机器人马达控制---对应结构体SDK_CameraMotorCtrl
        E_SDK_CFG_ENCODE_ADD_BEEP,		//设置编码加入每隔30秒beep声---对应结构体SDK_EncodeAddBeep
        E_SDK_CFG_DATALINK,			    //datalink客户在网络服务中的执行程序使能配置---对应结构体 SDK_DataLinkConfig
        E_SDK_CFG_FISH_EYE_PARAM,	    //鱼眼功能参数配置---对应结构体SDK_FishEyeParam
        E_SDK_OPERATION_SET_LOGO,	    //视频上叠加厂家的LOGO---对应结构体SDK_SetLogo
        E_SDK_CFG_SPARSH_HEARTBEAT,		//Sparsh客户的心跳功能配置---对应结构体 SDK_SparshHeartbeat
        E_SDK_CFG_LOGIN_FAILED,			//登录失败时发送邮件，使用结构体:基本事件结构---对应结构体 SDK_VIDEOLOSSCONFIG
        E_SDK_CFG_NETPLAT_SPVMN_NAS,	//安徽超清客户的nas服务器配置---对应结构体SDK_SPVMN_NAS_SERVER
        E_SDK_CFG_DDNS_APPLY,			//ddns 按键功能测试---对应结构体SDK_NetDDNSConfigALL
        E_SDK_OPERATION_NEW_UPGRADE_VERSION_REQ = 230,	//新版云升级版本查询请求---对应结构体SDK_CloudUpgradeVersionRep
        E_SDK_CFG_IPV6_ADDRESS,			//ipv6------对应的结构体SDK_IPAddressV6
        E_SDK_CFG_DDNS_IPMSG,         	//DDNS外网IP地址
        E_SDK_CFG_ONLINE_UPGRADE,		//在线升级相关配置-----对应的结构体SDK_OnlineUpgradeCfg
        E_SDK_CFG_CONS_SENSOR_ALARM,    //家用产品433报警联动项配置-----对应的SDK_ConsSensorAlarmCfg
        E_SDK_OPEARTION_SPLIT_CONTROL,  //画面分割模式-----对应的结构体SDK_SplitControl
        E_SDK_CFG_Netinfo_TRANS_COMM,	//Netinfo_cctv客户增加串口数据到指定服务器配置-----对应的结构体SDK_NetinfoNetTransComm
        E_SDK_CFG_RECORD_ENABLE,       	//是否准备好开始录像和抓图，现在用于日本客户通过串口控制开启和关闭录像功能
        E_SDK_CFG_NAS,					//nas配置		//SDK_NAS_LIST
        E_SDK_CFG_NKB_DEVICE_LIST,		//网络键盘设备链表
        E_SDK_CFG_PARKING_PORT = 240,	//泊车系统端口号配置  SDK_PortService
        E_SDK_CFG_NET_GBEYES,			//信产全球眼平台 SDK_GbEyesCfg
        E_SDK_CFG_GLOBALEYE,			//全球眼配置 SDK_DefaultResponse
        E_SDK_OPERATION_FISHEYE_MODIFY_CENTER,	//鱼眼圆心校正 SDK_Point
        E_SDK_OPERATION_UTC_TIME_SETTING = 244,	//设置UTC时间---对应结构体SDK_SYSTEM_TIME
        E_SDK_CFG_INTELBRAS_SPECIAL_INFO,		//interbras 特殊tcp端口号-----SDK_IntelBrasSpecialInfo
        E_SDK_CFG_SPVMN_PLATFORM_SIP,			//28181协议配置sip板卡ip-------SDK_SIP_NET_IP_CONFIG
        E_SDK_CFG_FISH_LENS_PARAM,				//魚眼鏡頭參數------SDK_FishLensParam
        E_SDK_CFG_PTZCTRLMODE,					//模拟通道云台控制的控制方式选择-----SDK_PTZControlModeAll
        E_SDK_CFG_ENCODE_SmartH264,				//SmartH264+配置------SDK_SmartH264ParamAll
        E_SDK_CFG_WIFI_INFO,					//无线WIFI信息--SDK_WifiInfo
        E_SDK_CFG_NET_RTMP,						//RTMP协议--SDK_NetRTMPConfig
        E_SDK_CFG_SNAP_SCHEDULE,				//定时抓图配置--SDK_SnapConfigAll
        E_SDK_OPERATION_SET_LANGUAGE,			//设置一种语言
        E_SDK_CFG_PTZPRESET,					//预置点配置--SDK_PtzPreset
        E_SDK_CFG_PTZTOUR,						//巡航配置--SDK_PtzTour
        E_SDK_CFG_PWD_SAFETY,					//安全问题相关配置(用于重置密码)--SDK_PasswordSafety
        E_SDK_ABILITY_QUESTION_DELIVERY,		//获取密码找回问题--SDK_QuestionDelivery
        E_SDK_CFG_TUTK_VERSION,					//TUTK客户定制版本信息--SDK_TutkVersion
        E_SDK_CFG_BREVIARY,						//缩略图配置
        E_SDK_CFG_SERIALPORT_ALARM,				//串口报警配置--SDK_SerialPortAlarm
        E_SDK_OPEARTION_SET_LEARN_CODE,			//支持串口报警设置才能设置学码--SDK_AMIRLearnCode
        E_SDK_CFG_PIR_ALARM,					//客户定制PIR报警配置--SDK_PIRConfigAll
        E_SDK_OPEARTION_CAMERA_VISCA,			//亿嘉和Visca协议修改，设置和获取倍率焦距等--SDK_CameraViscaControl
        E_SDK_OPERATION_TIME_SETTING_NEW_WAY,	//设置系统时间（想关掉时间同步的程序可以用这个命令，并禁用之前的时间设置命令）
        E_SDK_OPERATION_UTC_TIME_SETTING_NEW_WAY,//设置UTC时间（用于其他协议设置UTC时间）
        E_SDK_CFG_DDNSADDRINFO,					//DDNS状态信息
        E_SDK_CFG_THXY_VERION_INFO,				//天宏旭鹰定制版本信息--SDK_THXY_VersionInfo
        E_SDK_CFG_ALARM_BLUR_CHECK,				//图像模糊检测--SDK_BlurCheckAlarmAll
        E_SDK_CFG_INTEL_ENCODE,					//智能编码--SDK_IntelEnCodeCfgAll
        E_SDK_CFG_PLATE_DETECT_WHITE_LIST,		//车牌识别白名单--SDK_PlateDetectWhiteList
        E_SDK_OPERATION_PLATE_DETECT_LIFT,		//车牌侦测抬杆--SDK_PlateDetectLiftBar
        E_SDK_CFG_ALARM_PLATE_DETECT,			//车牌侦测报警--SDK_PlateDetectAll
        E_SDK_CFG_ALARM_FACE_DETECT,			//人脸识别报警--SDK_FaceDetectAll
        E_SDK_CFG_NET_IPADAPTIVE,				//ip自适应网关功能使能配置--SDK_IPAdaptiveCfg
        E_SDK_CFG_OEM_GETINFO,					//客户定制获取系统信息--SDK_OemGetInfo
        E_SDK_CFG_433_ALARM_DEV,				//客户定制433报警配置--SDK_ConsumerAlarm433DevList
        E_SDK_CFG_NET_ONVIF_PWD_CHECKOUT,		//onvif 密码校验--SDK_IpcOnvifPwdCheckout
        E_SDK_CFG_BALL_CAMERA_TRACK_DETECT,		//球机跟踪识别配置--SDK_BallCameraTrackDetectParamAll
        E_SDK_CFG_CAMERA_SPECIAL_NIGHT,			//夜晚情景特殊模式--SDK_CameraSpecialNightCtrl
        E_SDK_CFG_LPR_LIGHT_CONTROL,			//车牌识别白光灯控制--SDK_LPRLigthControl
        E_SDK_CFG_LPR_RECOGNIZE_TRIGGERMODE,	//车牌识别触发方式--SDK_LPRRecognizeTriggerMode
        E_SDK_CFG_LPR_TEMP_CAR_CHARGE_RULE,		//临时车收费规则--SDK_LPRTempCarChargeRule
        E_SDK_OPERATION_LPR_FORCE_RECOGNIZE,	//强制开始识别车牌--SDK_LprForceTrigRecognize
        E_SDK_CFG_LPR_DISPLAY,					//车牌识别 LED显示屏配置--SDK_LprLedSet
        E_SDK_CFG_LPR_BLACK_WHITE_LIST_WORK_MODE,//车牌收费系统黑白名单工作模式--SDK_LprBlackWhiteListModeAll
        E_SDK_OPERATION_LPR_LED_SHOW,			//显示屏显示收费金额,空余车位--SDK_LprLedShow
        E_SDK_CFG_LPR_AUTO_CONTRL_GATE,			//车牌收费系统针对临时车收费自动抬杆配置--SDK_LprAutoContrlGate
        E_SDK_LPR_ENTRY_EXIT_EXPORT,			//停车收费系统出入记录导出--
        E_SDK_CFG_LPR_ENTRY_EXIT,				//车牌收费系统出入场配置--SDK_LprEntryExitSet
        E_SDK_CFG_LPR_PARKING_LOT_INFO,			//车牌收费系统停车场信息--SDK_LprParkingLotInfo
        E_SDK_CFG_HARDWARE_ABILITY,				//发送数据
        E_SDK_CFG_URL_LOAD,						//上海熠知二次开发获取上传路径--SDK_CustomURLCfg
        E_SDK_CFG_LPR_HTTP_COMMUNICATION,		//车牌识别结果通过http协议推送配置--SDK_LprHttpCommunication
        E_SDK_CFG_PIRDETECT,					//PIR 检测--SDK_PIRDetectV2Config
        E_SDK_CFG_SERIAL_TRANS,					//杭州庄贤串口透传获取web 端的配置--SDK_SerialTransConfig
        E_SDK_CFG_CAMERA_SET_AWB,   			//西安知象定制白平衡--SDK_AWB_ATTR
        E_SDK_CFG_ControlPTZ,					//控制云台
        E_SDK_OPERATION_SET_SENSOR_ABILITY,		//设置合封模组sensor等级信息
        E_SDK_OPERATION_GET_SENSOR_ABILITY,		//获取合封模组sensor等级信息
        E_SDK_OPERATION_SWITCH_WIFI_MODE,		//切换AP模式--SDK_SWITCH_WIFI_MODE
        E_SDK_DEVICE_INFO_LPR_VERSION,			//车牌识别版本信息--SDK_LPR_VERSION

    }
    public enum MEDIA_FILE_TYPE
    {
        MEDIA_FILE_NONE = 0,
        MEDIA_FILE_H264 = 1,
        MEDIA_FILE_AVI = 2,
        MEDIA_FILE_RMVB = 3,
        MEDIA_FILE_MPG4 = 4,
        MEDIA_FILE_NUM
    }

    public class SocketStyle
    {
        public const Int32 TCPSOCKET = 0;
        public const Int32 UDPSOCKET = 1;
    };

    public enum SERIAL_TYPE
    {
        RS232 = 0,
        RS485 = 1,
    };
    public enum SDK_RSSI_SINGNAL
    {
	    SDK_RSSI_NO_SIGNAL,   //<= -90db
	    SDK_RSSI_VERY_LOW,     //<= -81db
	    SDK_RSSI_LOW,          //<= -71db
	    SDK_RSSI_GOOD,         //<= -67db
	    SDK_RSSI_VERY_GOOD,    //<= -57db
	    SDK_RSSI_EXCELLENT     //>-57db
    };
    public enum SDK_DeviceType
    {
        SDK_DEVICE_TYPE_DVR,	///< 普通DVR设备
        SDK_DEVICE_TYPE_NVS,	///< NVS设备
        SDK_DEVICE_TYPE_IPC,	///< IPC设备
        SDK_DEVICE_TYPE_HVR,	///<混合dvr
        SDK_DEVICE_TYPE_IVR,	///<智能dvr
        SDK_DEVICE_TYPE_MVR,	///<车载dvr
        SDK_DEVICE_TYPE_NR
    };

    //本地播放控制
    public enum SDK_LoalPlayAction
    {
        SDK_Local_PLAY_PAUSE,		/*<! 暂停播放 */
        SDK_Local_PLAY_CONTINUE,		/*<! 继续正常播放 */
        SDK_Local_PLAY_FAST,	        /*<! 加速播放 */
        SDK_Local_PLAY_SLOW,	        /*<! 减速播放 */
    };
    //云台操作类型
    public enum PTZ_ControlType
    {
        TILT_UP = 0,			//上
        TILT_DOWN,				//下
        PAN_LEFT,				//左
        PAN_RIGHT,				//右
        PAN_LEFTTOP,			//左上
        PAN_LEFTDOWN,			//左下
        PAN_RIGTHTOP,			//右上
        PAN_RIGTHDOWN,			//右下
        ZOOM_OUT,				//变倍小
        ZOOM_IN,				//变倍大
        FOCUS_FAR,				//焦点后调
        FOCUS_NEAR,				//焦点前调
        IRIS_OPEN,				//光圈扩大
        IRIS_CLOSE,				//光圈缩小13

        EXTPTZ_OPERATION_ALARM,			///< 报警功能 
        EXTPTZ_LAMP_ON,					///< 灯光开
        EXTPTZ_LAMP_OFF,				//灯光关
        EXTPTZ_POINT_SET_CONTROL,			//设置预置点 
        EXTPTZ_POINT_DEL_CONTROL,			//清除预置点 
        EXTPTZ_POINT_MOVE_CONTROL,			//转预置点
        EXTPTZ_STARTPANCRUISE,			//开始水平旋转		20	
        EXTPTZ_STOPPANCRUISE,			//停止水平旋转	
        EXTPTZ_SETLEFTBORDER,			//设置左边界		
        EXTPTZ_SETRIGHTBORDER,			//设置右边界	
        EXTPTZ_STARTLINESCAN,			//自动扫描开始 
        EXTPTZ_CLOSELINESCAN,			//自动扫描开停止 
        EXTPTZ_ADDTOLOOP,				//加入预置点到巡航	p1巡航线路	p2预置点值	
        EXTPTZ_DELFROMLOOP,				//删除巡航中预置点	p1巡航线路	p2预置点值	
        EXTPTZ_POINT_LOOP_CONTROL,			//开始巡航 28
        EXTPTZ_POINT_STOP_LOOP_CONTROL,	//停止巡航
        EXTPTZ_CLOSELOOP,				//清除巡航	p1巡航线路		
        EXTPTZ_FASTGOTO,				//快速定位	
        EXTPTZ_AUXIOPEN,				//辅助开关，关闭在子命令中//param1 参见SDK_PtzAuxStatus，param2传入具体数值
        EXTPTZ_OPERATION_MENU,				//球机菜单操作，其中包括开，关，确定等等
        EXTPTZ_REVERSECOMM,				//镜头翻转
        EXTPTZ_OPERATION_RESET,			///< 云台复位

        EXTPTZ_TOTAL,
    };
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct CONFIG_IPAddress
    {	//IP addr
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] c;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDK_CONFIG_NET_COMMON
    {
        //!主机名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string HostName;
        //!主机IP
        public CONFIG_IPAddress HostIP;
        //!子网掩码
        public CONFIG_IPAddress Submask;
        //!网关IP
        public CONFIG_IPAddress Gateway;
        //!HTTP服务端口
        public Int32 HttpPort;
        //!TCP侦听端口
        public Int32 TCPPort;
        //!SSL侦听端口
        public Int32 SSLPort;
        //!UDP侦听端口
        public Int32 UDPPort;
        //!最大连接数
        public Int32 MaxConn;
        //!监视协议 {"TCP","UDP","MCAST",…}
        public Int32 MonMode;
        //!限定码流值
        public Int32 MaxBps;
        //!传输策略
        //char TransferPlan[NET_NAME_PASSWORD_LEN];
        public Int32 TransferPlan;

        //!是否启用高速录像下载测率
        public byte bUseHSDownLoad;

        //!MAC地址
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string sMac;
    }

     [StructLayout(LayoutKind.Sequential)]
    public struct SDK_CONFIG_NET_COMMON_V2
    {
	    //!主机名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string HostName;
        //!主机IP
        public CONFIG_IPAddress HostIP;
        //!子网掩码
        public CONFIG_IPAddress Submask;
        //!网关IP
        public CONFIG_IPAddress Gateway;
        //!HTTP服务端口
        public Int32 HttpPort;
        //!TCP侦听端口
        public Int32 TCPPort;
        //!SSL侦听端口
        public Int32 SSLPort;
        //!UDP侦听端口
        public Int32 UDPPort;
        //!最大连接数
        public Int32 MaxConn;
        //!监视协议 {"TCP","UDP","MCAST",…}
        public Int32 MonMode;
        //!限定码流值
        public Int32 MaxBps;
        //!传输策略
        //char TransferPlan[NET_NAME_PASSWORD_LEN];
        public Int32 TransferPlan;

        //!是否启用高速录像下载测率
        public byte bUseHSDownLoad;

	    //!MAC地址
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string sMac;

	    //序列号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string sSn;
	    //保留
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string Resume;
    };

    public struct DDNS_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string ID;    //设备标识
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string IP;   //内网IP
        public Int32 WebPort; //Web端口,默认为80
        public Int32 MediaPort; //媒体端口,默认为34567
        public Int32 MobilePort;  //手机监控端口，默认为34599
        public Int32 UPNPWebPort;  //UPNP启动下Web端口,UPNP不开启为0
        public Int32 UPNPMediaPort; //UPNP启动下媒体端口,UPNP不开启为0
        public Int32 UPNPMobilePort; //UPNP启动下手机监控端口,UPNP不开启为0
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        string Username; //用户名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        string Password; //密码
    }
    public struct SDK_NetKeyBoardData
    {
        public Int32 iValue;
        public Int32 iState;
    }
    public struct SDK_NetAlarmInfo
    {
        public Int32 iEvent;  //目前未使用
        public Int32 iState;   //每bit表示一个通道,bit0:第一通道,0-无报警 1-有报警, 依次类推
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SDK_SYSTEM_TIME
    {
        public Int32 year;///< 年。   
        public Int32 month;///< 月，January = 1, February = 2, and so on.   
        public Int32 day;///< 日。   
        public Int32 wday;///< 星期，Sunday = 0, Monday = 1, and so on   
        public Int32 hour;///< 时。   
        public Int32 minute;///< 分。   
        public Int32 second;///< 秒。   
        public Int32 isdst;///< 夏令时标识。   
    }
    public struct TransComChannel//透明窗口
    {
        public SERIAL_TYPE TransComType;//SERIAL_TYPE
        public ushort baudrate;
        public ushort databits;
        public ushort stopbits;
        public ushort parity;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct H264_DVR_DEVICEINFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string sSoftWareVersion;	///< 软件版本信息
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string sHardWareVersion;	///< 硬件版本信息
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string sEncryptVersion;	///< 加密版本信息
        public SDK_SYSTEM_TIME tmBuildTime;///< 软件创建时间
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string sSerialNumber;			///< 设备序列号
        public Int32 byChanNum;				///< 视频输入通道数
        public Int32 iVideoOutChannel;		///< 视频输出通道数
        public Int32 byAlarmInPortNum;		///< 报警输入通道数
        public Int32 byAlarmOutPortNum;		///< 报警输出通道数
        public Int32 iTalkInChannel;		///< 对讲输入通道数
        public Int32 iTalkOutChannel;		///< 对讲输出通道数
        public Int32 iExtraChannel;			///< 扩展通道数	
        public Int32 iAudioInChannel;		///< 音频输入通道数
        public Int32 iCombineSwitch;		///< 组合编码通道分割模式是否支持切换
        public Int32 iDigChannel;		    ///<数字通道数
        public Int32 uiDeviceRunTime;	    ///<系统运行时间
        public Int32 deviceTye;	///设备类型
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]                                    ///
        public string sHardWare;		    ///<设备型号                                   ///
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string uUpdataTime;	        ///<更新日期 例如 2013-09-03 14:15:13
	    public Int32 uUpdataType;	        ///<更新内容
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string sDeviceModel;         ///设备型号(底层库从加密里获得，sHardWare针对多个设备用同一个程序这种情况区分不了) 
        public Int32 nLanguage;             ///国家的语言ID,0英语 1中文 2中文繁体 3韩语 4德语 5葡萄牙语 6俄语
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string sCloudErrCode;        ///云登陆具体错误内容
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public Int32[] status;
        public void Init()
        {
            status = new Int32[32];
            status[31] = NetSDK.g_config.nSDKType;
        }
    }
    public struct H264_DVR_CLIENTINFO
    {
        public Int32 nChannel;	//通道号
        public Int32 nStream;	//0表示主码流，为1表示子码流
        public Int32 nMode;		//0：TCP方式,1：UDP方式,2：多播方式,3 - RTP方式，4-音视频分开(TCP)
        public Int32 nComType;	//只对组合编码通道有效, 组合编码通道的拼图模式
        public IntPtr hWnd;
    }
    public struct SDK_TIMESECTION
    {
        //!使能
        public Int32 enable;
        //!开始时间:小时
        public Int32 startHour;
        //!开始时间:分钟
        public Int32 startMinute;
        //!开始时间:秒钟
        public Int32 startSecond;
        //!结束时间:小时
        public Int32 endHour;
        //!结束时间:分钟
        public Int32 endMinute;
        //!结束时间:秒钟
        public Int32 endSecond;
    }
    //查询录像条件
    public struct H264_DVR_FINDINFO
    {
	    public Int32 nChannelN0;			//通道号
	    public Int32 nFileType;			//文件类型, 见SDK_File_Type
	    public H264_DVR_TIME startTime;	//开始时间
	    public H264_DVR_TIME endTime;	//结束时间
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]     	
        public string szCard;		//卡号
	    public uint hWnd;
    };

    //录像文件返回结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct H264_DVR_FILE_DATA 
     {
        public Int32 ch;						//通道号
        public Int32 size;                    //文件大小
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 108)]     
        public string sFileName;		///< 文件名
        public SDK_SYSTEM_TIME stBeginTime;	///< 文件开始时间
        public SDK_SYSTEM_TIME stEndTime;		///< 文件结束时间
        public IntPtr hWnd;
        public Int32 StreamType;	//码流类型是回放主码流还是辅助码流
//         H264_DVR_FILE_DATA()
//         {
//             hWnd = NULL;
//             StreamType = 0;
//         }
     };

    public struct H264_DVR_TIME {
	    public Int32 dwYear;		//年
        public Int32 dwMonth;	//月
	    public Int32 dwDay;		//日
	    public Int32 dwHour;		//时
	    public Int32 dwMinute;	//分
	    public Int32 dwSecond;	//秒
    };

    public struct SDK_VIDEOCOLOR_PARAM
    {
        public Int32 nBrightness;		///< 亮度	0-100
        public Int32 nContrast;			///< 对比度	0-100
        public Int32 nSaturation;		///< 饱和度	0-100
        public Int32 nHue;				///< 色度	0-100
        public Int32 mGain;				///< 增益	0-100 第７位置1表示自动增益		
        public Int32 mWhitebalance;		///< 自动白电平控制，bit7置位表示开启自动控制.0x0,0x1,0x2分别代表低,中,高等级
        public Int32 nAcutance;          ///< 锐度   0-15
    };
    public struct SDK_VIDEOCOLOR
    {
        public SDK_TIMESECTION tsTimeSection;		/// 时间段
        public SDK_VIDEOCOLOR_PARAM dstColor;			/// 颜色定义
        public Int32 iEnable;
    }
    public struct SDK_CONFIG_VIDEOCOLOR
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public SDK_VIDEOCOLOR[] dstVideoColor;
    }
    public struct SearchMode
    {
        public Int32 nType;        //查询类型，见SearchModeType
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szSerIP;//服务器地址
        public Int32 nSerPort;           //服务器端口号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string szSerialInfo;  //如果是按序列号，则为序列号，如果是用户名，则为用户名
    }

    
    //摄象机参数.....

    //曝光配置
    public struct SDK_ExposureCfg
    {
	    public Int32  level;    //曝光等级
	    public uint leastTime;//自动曝光时间下限或手动曝光时间，单位微秒
	    public uint mostTime; //自动曝光时间上限，单位微秒
    };

    //增益配置
    public struct SDK_GainCfg
    {
	    public Int32 gain;    //自动增益上限(自动增益启用)或固定增益值
	    public Int32 autoGain;//自动增益是否启用，0:不开启  1:开启
    };

    //网络摄像机配置
    public struct SDK_CameraParam
    {
	    public uint whiteBalance;         //白平衡
	    public uint dayNightColor;        //日夜模式，取值有彩色、自动切换和黑白
	    public Int32 elecLevel;             //参考电平值
	    public uint apertureMode;          //自动光圈模式
	    public uint BLCMode;               //背光补偿模式
	    public SDK_ExposureCfg exposureConfig;//曝光配置
	    public SDK_GainCfg     gainConfig;    //增益配置

	    public uint PictureFlip;		//图片上下翻转
	    public uint PictureMirror;	//图片左右翻转(镜像)
	    public uint RejectFlicker;	//日光灯防闪功能
	    public uint EsShutter;		//电子慢快门功能

	    public Int32 ircut_mode;		//IR-CUT切换 0 = 红外灯同步切换 1 = 自动切换

	    public Int32 dnc_thr;			//日夜转换阈值
	    public Int32 ae_sensitivity;	//ae灵敏度配置
	    public Int32 Day_nfLevel;		//noise filter 等级，0-5,0不滤波，1-5 值越大滤波效果越明显
	    public Int32 Night_nfLevel;
	    public Int32 Ircut_swap;		//ircut 正常序= 0        反序= 1
    };

    //所有摄象机配置
    public struct SDK_AllCameraParam
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	    public SDK_CameraParam[] vCameraParamAll;   //所有的通道
    };
       
    public struct PACKET_INFO_EX
    {
	    public int		nPacketType;				// 包类型,见MEDIA_PACK_TYPE
	    public string	pPacketBuffer;				// 缓存区地址
	    public uint	dwPacketSize;				// 包的大小

	    // 绝对时标
	    public int		nYear;						// 时标:年		
	    public int		nMonth;						// 时标:月
	    public int		nDay;						// 时标:日
	    public int		nHour;						// 时标:时
	    public int		nMinute;					// 时标:分
	    public int		nSecond;					// 时标:秒
	    public uint 	dwTimeStamp;					// 相对时标低位，单位为毫秒
	    public uint	dwTimeStampHigh;        //相对时标高位，单位为毫秒
	    public uint   dwFrameNum;             //帧序号
	    public uint   dwFrameRate;            //帧率
	    public ushort uWidth;              //图像宽度
	    public ushort uHeight;             //图像高度
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	    public ushort[]       Reserved;            //保留
    } ;
    ///< 夏令时结构
    public struct DSTPoint
    {
        public Int32 iYear;
        public Int32 iMonth;
        public Int32 iWeek;		///<周1:first  to2 3 4 -1:last one   0:表示使用按日计算的方法[-1,4]
        public Int32 iWeekDay;	///<weekday from sunday=0	[0, 6]
        public Int32 Hour;
        public Int32 Minute;
    };
    //普通配置页结构体
    public struct SDK_CONFIG_NORMAL
    {
        public SDK_SYSTEM_TIME sysTime;		//系统时间

	    public Int32 iLocalNo;			/*!< 本机编号:[0, 998] */
        public Int32 iOverWrite;			/*!< 硬盘满时处理 "OverWrite", "StopRecord" */
        public Int32 iSnapInterval;			///< 定时抓图的时间间隔，以秒为单位 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
	    public string sMachineName;	///< 机器名
        public Int32 iVideoStartOutPut;	/*!< 输出模式 */
        public Int32 iAutoLogout;			///< 本地菜单自动注销(分钟)	[0, 120]

        public Int32 iVideoFormat;		/*!< 视频制式:“PAL”, “NTSC”, “SECAM” */
        public Int32 iLanguage;			/*!< 语言选择:“English”, “SimpChinese”, “TradChinese”, “Italian”, “Spanish”, “Japanese”, “Russian”, “French”, “German” */
        public Int32 iDateFormat;		/*!< 日期格式:“YYMMDD”, “MMDDYY”, “DDMMYY” */
        public Int32 iDateSeparator;		/*!< 日期分割符:“.”, “-”, “/” */
        public Int32 iTimeFormat;		/*!< 时间格式:“12”, “24” */
        public Int32 iDSTRule;			///< 夏令时规则 
        public Int32 iWorkDay;			///< 工作日
        public DSTPoint dDSTStart;
        public DSTPoint dDSTEnd;
    };
        /// 编码功能
    public enum SDK_EncodeFunctionTypes
    {
	    SDK_ENCODE_FUNCTION_TYPE_DOUBLE_STREAM,		///< 双码流功能
	    SDK_ENCODE_FUNCTION_TYPE_COMBINE_STREAM,	///< 组合编码功能
	    SDK_ENCODE_FUNCTION_TYPE_SNAP_STREAM,		///< 抓图功能
	    SDK_ENCODE_FUNCTION_TYPE_WATER_MARK,		///< 水印功能
	    SDK_ENCODE_FUNCTION_TYPE_NR,
    };
    public enum SDK_AlarmFucntionTypes
    {
	    SDK_ALARM_FUNCTION_TYPE_MOTION_DETECT,	///< 动态检测
	    SDK_ALARM_FUNCTION_TYPE_BLIND_DETECT,	///< 视屏遮挡
	    SDK_ALARM_FUNCTION_TYPE_LOSS_DETECT,	///< 视屏丢失
	    SDK_ALARM_FUNCTION_TYPE_LOCAL_ALARM,	///< 本地报警
	    SDK_ALARM_FUNCTION_TYPE_NET_ALARM,		///< 网络报警
	    SDK_ALARM_FUNCTION_TYPE_IP_CONFLICT,	///< IP地址冲突
	    SDK_ALARM_FUNCTION_TYPE_NET_ABORT,		///< 网络异常
	    SDK_ALARM_FUNCTION_TYPE_STORAGE_NOTEXIST,	///< 存储设备不存在
	    SDK_ALARM_FUNCTION_TYPE_STORAGE_LOWSPACE,	///< 存储设备容量不足
	    SDK_ALARM_FUNCTION_TYPE_STORAGE_FAILURE,	///< 存储设备访问失败
	    SDK_ALARM_FUNCTION_TYPE_VIDEOANALYSE,///<视频分析
	    SDK_ALARM_FUNCTION_TYPE_NET_ABORT_EXTEND,	//网络异常扩展
	    SDK_ALARM_FUNCTION_TYPE_NR
    };
        /// 网络服务功能
    public enum SDK_NetServerTypes
    {
	    SDK_NET_SERVER_TYPES_IPFILTER,		///< 白黑名单
	    SDK_NET_SERVER_TYPES_DHCP,			///< DHCP功能
	    SDK_NET_SERVER_TYPES_DDNS,			///< DDNS功能
	    SDK_NET_SERVER_TYPES_EMAIL,			///< Email功能
	    SDK_NET_SERVER_TYPES_MULTICAST,		///< 多播功能
	    SDK_NET_SERVER_TYPES_NTP,			///< NTP功能
	    SDK_NET_SERVER_TYPES_PPPOE,
	    SDK_NET_SERVER_TYPES_DNS,
	    SDK_NET_SERVER_TYPES_ARSP,			///< 主动注册服务
	    SDK_NET_SERVER_TYPES_3G,            ///< 3G拨号
	    SDK_NET_SERVER_TYPES_MOBILE=10,        ///< 手机监控
	    SDK_NET_SERVER_TYPES_UPNP,			    ///< UPNP
	    SDK_NET_SERVER_TYPES_FTP,			    ///< FTP
	    SDK_NET_SERVER_TYPES_WIFI,          ///<WIFI
	    SDK_NET_SERVER_TYPES_ALARM_CENTER,  ///< 告警中心
	    SDK_NET_SERVER_TYPES_NETPLAT_MEGA,  ///< 互信互通
	    SDK_NET_SERVER_TYPES_NETPLAT_XINWANG,  ///< 星望
	    SDK_NET_SERVER_TYPES_NETPLAT_SHISOU,  ///< 视搜
	    SDK_NET_SERVER_TYPES_NETPLAT_VVEYE,  ///< 威威眼
	    SDK_NET_SERVER_TYPES_RTSP,     //RTSP
	    SDK_NET_SERVER_TYPES_PHONEMSG=20,     //手机信息发送配置
	    SDK_NET_SERVER_TYPES_PHONEMULTIMEDIAMSG,     //手机信息发送配置
	    SDK_NET_SERVER_TYPES_DAS,          //主动注册
	    SDK_NET_SERVER_TYPES_LOCALSDK_PLATFORM,          //网络平台信息设置
	    SDK_NET_SERVER_TYPES_GOD_EYE,///<神眼接警中心系统
	    SDK_NET_SERVER_TYPES_NAT,		///NAT穿透，MTU配置
	    SDK_NET_SERVER_TYPES_VPN,     ///VPN
	    SDK_NET_SERVER_TYPES_NET_KEYBOARD,	///网络键盘配置
	    SDK_NET_SERVER_TYPES_SPVMN,		///28181协议配置
	    SDK_NET_SERVER_TYPES_PMS,      //手机服务
	    SDK_NET_SERVER_TYPE_KAICONG,		///凯聪配置
	    SDK_NET_SERVER_TYPE_PROTOCOL_MAC,///支持MAC协议
	    SDK_NET_SERVER_TYPE_XMHEARTBEAT, //雄迈心跳
	    SDK_NET_SERVER_TYPES_MONITOR_PLATFORM, //神州数码监控平台
	    SDK_NET_SERVER_TYPES_ANJUP2P,			//
	    SDK_NET_SERVER_TYPES_NR,   
    };
    ///串口类型
    public enum SDK_CommTypes
    {
	    SDK_COMM_TYPES_RS485,			///<485串口
	    SDK_COMM_TYPES_RS232,			///<232串口
	    SDK_COMM_TYPES_NR
    };
    //输入法限制
    public enum SDK_InPutMethod
    {
	    SDK_NO_SUPPORT_CHINESE,		//不支持中文输入
	    SDK_NO_SUPPORT_NR
    };
    //报警中标签显示
    public enum SDK_TipShow
    {
	    SDK_NO_BEEP_TIP_SHOW,  //蜂鸣提示
	    SDK_NO_FTP_TIP_SHOW,  //FTP提示
	    SDK_NO_EMAIL_TIP_SHOW,  //EMAIL提示
	    SDK_NO_TIP_SHOW_NR
    };
    /// 预览功能
    public enum SDK_PreviewTypes
    {
	    SDK_PREVIEW_TYPES_TOUR,		///< 轮巡
	    SDK_PREVIEW_TYPES_TALK,		///< GUI界面配置
	    SDK_PREVIEW_TYPES_NR
    };

    ///车载功能
    public enum SDK_MobileCar
    {
	    SDK_MOBILEDVR_STATUS_EXCHANGE,
	    SDK_MOBILEDVR_DELAY_SET,
	    SDK_MOBILEDVR_CARPLATE_SET,
	    SDK_MOBILEDVR_GPS_TIMING,
	    SDK_MOBILEDVR_NR
    };
    ///其他功能
    public enum SDK_OtherFunction
    {
	    SDK_OTHER_DOWNLOADPAUSE,		//录像下载暂停功能
	    SDK_OTHER_USB_SUPPORT_RECORD,	//USB支持录像功能
	    SDK_OTHER_SD_SUPPORT_RECORD,		//SD支持录像功能
	    SDK_OTHER_ONVIF_CLIENT_SUPPORT,	//是否支持ONVIF客户端
	    SDK_OTHER_NET_LOCALSEARCH_SUPPORT,	//是否支持远程搜索
	    SDK_OTHER_MAXPLAYBACK_SUPPORT, //是否支持最大回放通道数显示
	    SDK_OTHER_NVR_SUPPORT, //是否是专业NVR
	    SDK_OTHER_C7_PLATFORM_SUPPORT,//支持C7平台
	    SDK_OTHER_MAIL_TEST_SUPPORT,		//支持邮件测试
	    SDK_OTHER_SHOW_OSD_INFO,            //支持显示3行OSD信息
	    SDK_OTHER_HIDE_DIGITAL, //通道模式屏蔽功能	
	    SDK_OTHER_ACUTANCE_HORIZONTAL,	//锐度
	    SDK_OTHER_ACUTANCE_VERTIAL,
	    SDK_OTHER_BROAD_TRENDS,		//宽动态功能
	    SDK_OTHER_NO_TALK,		//对讲能力
	    SDK_OTHER_ALTER_DIGITAL_NAME,	//修改数字通道名称
	    SDK_OTHER_SHOW_CONNECT_STATUS,      //支持显示wifi 3G 主动注册等的连接状态
	    SDK_OTHER_SUPPORT_ECACT_SEEK,	//支持回放精准定位
	    SDK_OTHER_UPLOAD_TITLEANDSTATE,		//通道标题和数字通道状态上传能力集
	    SDK_OTHER_NO_HDD_RECORD,		//无硬盘录像
	    SDK_OTHER_NR
    };
    ///支持的系统功能
    [StructLayout(LayoutKind.Sequential)]
    public struct SDK_SystemFunction
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)SDK_EncodeFunctionTypes.SDK_ENCODE_FUNCTION_TYPE_NR)]
        public byte[] vEncodeFunction;	///< 编码功能SDK_EncodeFunctionTypes
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)SDK_AlarmFucntionTypes.SDK_ALARM_FUNCTION_TYPE_NR)]
        public byte[] vAlarmFunction;		///< 报警功能AlarmFucntionTypes
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)SDK_NetServerTypes.SDK_NET_SERVER_TYPES_NR)]
        public byte[] vNetServerFunction;	///< 网络服务功能NetServerTypes
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)SDK_PreviewTypes.SDK_PREVIEW_TYPES_NR)]
        public byte[] vPreviewFunction;		///< 预览功能PreviewTypes
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)SDK_CommTypes.SDK_COMM_TYPES_NR)]
        public byte[] vCommFunction;			///<串口类型SDK_CommTypes
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)SDK_InPutMethod.SDK_NO_SUPPORT_NR)]
        public byte[] vInputMethodFunction;  //<输入法限制SDK_InPutMethod>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)SDK_TipShow.SDK_NO_TIP_SHOW_NR)]
        public byte[] vTipShowFunction;               //报警标签显示SDK_TipShow>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)SDK_MobileCar.SDK_MOBILEDVR_NR)]
        public byte[] vMobileCarFunction;//车载功能
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)SDK_OtherFunction.SDK_OTHER_NR)]
        public byte[] vOtherFunction;				//其他功能OtherFunction
    };

    public struct SDK_NetWifiDevice
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
	    public string sSSID;            //SSID Number
	    public Int32 nRSSI;                 //SEE SDK_RSSI_SINGNAL
	    public Int32 nChannel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	    public string sNetType;         //Infra, Adhoc
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	    public string sEncrypType;      //NONE, WEP, TKIP, AES
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	    public string sAuth;            //OPEN, SHARED, WEPAUTO, WPAPSK, WPA2PSK, WPANONE, WPA, WPA2
    };
   
    public struct SDK_NetWifiDeviceAll
    {
	    public Int32 nDevNumber;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]  ///NET_MAX_AP_NUMBER=32
	    public SDK_NetWifiDevice[] vNetWifiDeviceAll;
    };
    
    ///< WIFI设置
    public struct SDK_NetWifiConfig
    {
	    public byte bEnable;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
	    public string sSSID;            //SSID Number
	    public Int32 nChannel;                   //channel
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	    public string sNetType;         //Infra, Adhoc
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	    public string sEncrypType;      //NONE, WEP, TKIP, AES
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	    public string sAuth;            //OPEN, SHARED, WEPAUTO, WPAPSK, WPA2PSK, WPANONE, WPA, WPA2
	    public Int32  nKeyType;                  //0:Hex 1:ASCII
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] ///NET_IW_ENCODING_TOKEN_MAX = 128
	    public string sKeys;
	    public CONFIG_IPAddress HostIP;		///< host ip
	    public CONFIG_IPAddress Submask;		///< netmask
	    public CONFIG_IPAddress Gateway;		///< gateway
    };

    public struct SDK_NetDHCPConfig
    {
	    public byte bEnable;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	    public string ifName;
    };
    /// 所有网卡的DHCP配置
    public struct SDK_NetDHCPConfigAll
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	    public SDK_NetDHCPConfig[] vNetDHCPConfig;
    };

    /// 报警事件码
    public enum SDK_EventCodeTypes
    {
        SDK_EVENT_CODE_INIT = 0,
        SDK_EVENT_CODE_LOCAL_ALARM = 1,		//本地报警(外部报警)
        SDK_EVENT_CODE_NET_ALARM,			//网络报警
        SDK_EVENT_CODE_MANUAL_ALARM,		//手动报警
        SDK_EVENT_CODE_VIDEO_MOTION,		//动态检测
        SDK_EVENT_CODE_VIDEO_LOSS,			//视频丢失
        SDK_EVENT_CODE_VIDEO_BLIND,			//视频遮挡
        SDK_EVENT_CODE_VIDEO_TITLE,
        SDK_EVENT_CODE_VIDEO_SPLIT,
        SDK_EVENT_CODE_VIDEO_TOUR,
        SDK_EVENT_CODE_STORAGE_NOT_EXIST,	//存储设备不存在
        SDK_EVENT_CODE_STORAGE_FAILURE,		//存储设备访问失败
        SDK_EVENT_CODE_LOW_SPACE,			//存储设备容量过低
        SDK_EVENT_CODE_NET_ABORT,
        SDK_EVENT_CODE_COMM,
        SDK_EVENT_CODE_STORAGE_READ_ERROR,	//存储设备读错误
        SDK_EVENT_CODE_STORAGE_WRITE_ERROR,	//存储设备写错误
        SDK_EVENT_CODE_NET_IPCONFLICT,		//ip冲突
        SDK_EVENT_CODE_ALARM_EMERGENCY,
        SDK_EVENT_CODE_DEC_CONNECT,
        SDK_EVENT_CODE_UPGRADE,
        SDK_EVENT_CODE_BACK_UP,
        SDK_EVENT_CODE_SHUT_DOWN,
        SDK_EVENT_CODE_REBOOT,
        SDK_EVENT_CODE_NEWFILE,
        SDK_EVENT_CODE_VideoAnalyze,
        SDK_EVENT_CODE_IPC_ALARM,
        SDK_EVENT_CODE_SPEED_ALARM,
        SDK_EVENT_CODE_GSENSOR_AlARM,
        SDK_EVENT_CODE_LOGIN_FAILED,		//登录失败
        SDK_EVENT_CODE_SERIAL_ALARM,
        SDK_EVENT_CODE_PIR_ALARM,			//客户定制PIR报警
        SDK_EVENT_CODE_CONSSENSOR_ALARM, 	//消费类产品绑定的外部设备报警
        SDK_EVENT_CODE_WORDDETECT,			//家用产品，语音识别敏感词报警
        SDK_EVENT_CODE_BLURCHECK_ALARM,		//模糊检测报警
        SDK_EVENT_CODE_PLATEDETECT,			//车牌检测报警
        SDK_EVENT_CODE_FACEDETECT,			//人脸检测报警
        SDK_EVENT_CODE_433ALARM,			//客户定制433报警
        SDK_EVENT_CODE_PIRDetect = 38,		//PIR检测
        SDK_EVENT_CODE_NR,
    };

    //报警信息
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SDK_ALARM_INFO
    {
        public Int32 nChannel;
        public Int32 iEvent; //报警事件码:见枚举SDK_EventCodeTypes
        public Int32 iStatus;//0:报警开始，1:报警结束
        public SDK_SYSTEM_TIME SysTime;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] pExtInfo;//附加信息，发送者和接收者对各种报警类型进行格式约定
    };

    public static class SDK_TYPE_LEN  //define some constant
    {
        public const int NET_NAME_PASSWORD_LEN = 64;    //maximum length of passwrod
        public const int NET_MAX_USER_NUM = 60;		    //最多用户数
        public const int NET_MAX_RIGTH_NUM = 200;		//最多权限数
        public const int NET_MAX_GROUP_NUM = 50;		//最多组数
        public const int NET_MAX_USER_LENGTH = 32;		//用户名密码最大长度
        public const int SDK_MAX_DRIVER_PER_DISK = 4;	///< 每个磁盘最多的分区数
        public const int SDK_MAX_DISK_PER_MACHINE = 8;	///< 最多支持8块硬盘
        public const int NET_MAX_CHANNUM = 64;			//最大通道个数
    }


    ///< 服务器结构定义
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SDK_RemoteServerConfig
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_NAME_PASSWORD_LEN)]
        public string ServerName;		///< 服务名
        public CONFIG_IPAddress ip;		///< IP地址
        public Int32 Port;				///< 端口号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_NAME_PASSWORD_LEN)]
        public string UserName;		    ///< 用户名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_NAME_PASSWORD_LEN)]
        public string Password;		   ///< 密码
        public byte Anonymity;		///< 是否匿名登录
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] res;
    };

    ///< NTP设置
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SDK_NetNTPConfig
    {
        ///< 是否开启
        public byte Enable;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] res;
        ///< PPPOE服务器
        public SDK_RemoteServerConfig Server;
        ///< 更新周期
        public Int32 UpdatePeriod;
        ///< 时区
        public Int32 TimeZone;
    };

    ///////////////////用户帐号管理相关结构体/////////////////////////////////
    [StructLayout(LayoutKind.Sequential)]
    public struct  OPR_RIGHT
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_MAX_USER_LENGTH)]
	    public string name;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct USER_INFO
    {
	    public Int32			rigthNum;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = SDK_TYPE_LEN.NET_MAX_RIGTH_NUM)]
	    public OPR_RIGHT []rights;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_MAX_USER_LENGTH)]
	    public string Groupname;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_MAX_USER_LENGTH)]
	    public string memo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_MAX_USER_LENGTH)]
	    public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_MAX_USER_LENGTH)]
	    public string passWord;
	    public byte	  reserved;		//是否保留
        public byte   shareable;		//本用户是否允许复用 1-复用，0-不复用
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte  []res;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct USER_GROUP_INFO
    {
	    public Int32			rigthNum;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_MAX_USER_LENGTH)]
	    public string memo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_MAX_USER_LENGTH)]
	    public string name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SDK_TYPE_LEN.NET_MAX_RIGTH_NUM)]
        public OPR_RIGHT[] rights;	//权限列表
    };


    //用户信息配置结构
    [StructLayout(LayoutKind.Sequential)]
    public struct USER_MANAGE_INFO
    {
	    public Int32 	rigthNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SDK_TYPE_LEN.NET_MAX_RIGTH_NUM)]
        public OPR_RIGHT[] rightList;
	    public Int32 	groupNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SDK_TYPE_LEN.NET_MAX_GROUP_NUM)]
        public USER_GROUP_INFO[] groupList;
	    public Int32 	userNum;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SDK_TYPE_LEN.NET_MAX_USER_NUM)]
        public USER_INFO[] userList;
    };

    //修改用户
    [StructLayout(LayoutKind.Sequential)]
    public struct CONF_MODIFYUSER
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK_TYPE_LEN.NET_MAX_USER_LENGTH)]
	    public string UserName;
        public USER_INFO User;
    };

    // 修改设备时区
    [StructLayout(LayoutKind.Sequential)]
    public struct SDK_TimeZone
    {
        public Int32 minuteswest; 	//跟UTC时间的差值，单位分钟，可以为负
        public Int32  FistFlag;		//用于保证第一次使用的时候时间不变
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SDK_HardDiskInfo
    {
        public bool bIsCurrent;			///< 是否为当前工作盘
        public UInt32 uiTotalSpace;		///< 总容量，MB为单位
        public UInt32 uiRemainSpace;	///< 剩余容量，MB为单位
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SDK_OemGetInfo
    {
        public Int32 iConnectNum;					//连接数
        public Int32 iDisk;							//硬盘个数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SDK_TYPE_LEN.SDK_MAX_DISK_PER_MACHINE)]
        public SDK_HardDiskInfo[] cHardDisk;        //硬盘剩余容量
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SDK_TYPE_LEN.NET_MAX_CHANNUM)]
        public byte[] cRecordState;	                //录像状态
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string version;					    //版本信息
    }

    class NetSDK
    {
        public delegate void fDisConnect(SDK_HANDLE lLoginID, string pchDVRIP, int nDVRPort, IntPtr dwUser);
        public delegate bool fMessCallBack(SDK_HANDLE lLoginID, IntPtr pAlarm, uint dwBufLen, IntPtr dwUser);
        public delegate void fTransComCallBack(SDK_HANDLE lLoginID, int lTransComType, StringBuilder pBuffer, uint dwBufSize, uint dwUser);
        public delegate void fDownLoadPosCallBack(SDK_HANDLE lPlayHandle, int lTotalSize, int lDownLoadSize, int dwUser);
        public delegate void fPlayDrawCallBack(SDK_HANDLE lPlayHand, IntPtr hDc, uint nUser);
        public delegate void fLocalPlayFileCallBack(SDK_HANDLE lPlayHand, uint nUser);
        public delegate void InfoFramCallBack(SDK_HANDLE lPlayHand, uint nType, string pBuf, uint nSize, uint nUser);
        public delegate int fRealDataCallBack_V2(SDK_HANDLE lRealHandle, ref PACKET_INFO_EX pFrame, int dwUser);
        public delegate int fRealDataCallBack(SDK_HANDLE lRealHandle, int dwDataType, string strBuf, int lbufsize, int dwUser);

        public static SDK_Config g_config = new SDK_Config();
        public struct SDK_Config 
        {
            public Int32 nSDKType;
        }
        public static class SDK_TYPE  
        {
            public const int SDK_TYPE_GENERAL = 0;    //通用类型
            public const int SDK_TYPE_1004 = 1004;
        }

        public static int SDK_Init(SDK_Config config, fDisConnect cbDisConnect, IntPtr dwUser)
        {
            g_config = config;
            return H264_DVR_Init(cbDisConnect, dwUser);;
        }

        [DllImport("NetSdk.dll")]
        public static extern void H264_DVR_SetDVRMessCallBack(fMessCallBack cbAlarmcallback, IntPtr lUser);

        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_Init(fDisConnect cbDisConnect, IntPtr dwUser);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_SetConnectTime(int nWaitTime, int nTryTimes);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_Cleanup();

        [DllImport("NetSdk.dll")]
        public static extern SDK_HANDLE H264_DVR_Login(string sDVRIP, ushort wDVRPort, string sUserName, string sPassword,
                              ref H264_DVR_DEVICEINFO lpDeviceInfo, out int error, Int32 socketstyle);

        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_GetLastError();

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_ClickKey(SDK_HANDLE lLoginID, ref SDK_NetKeyBoardData pKeyBoardData);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_SendNetAlarmMsg(SDK_HANDLE lLoginID, ref SDK_NetAlarmInfo pAlarmInfo);

        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_GetDevConfig(SDK_HANDLE lLoginID, uint dwCommand, int nChannelNO, IntPtr bufptr, uint dwOutBufferSize, out uint lpBytesReturned, int waittime);
        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_SetDevConfig(SDK_HANDLE lLoginID, uint dwCommand, int nChannelNO, IntPtr bufptr, uint dwInBufferSize, int waittime);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_StopLocalRecord(SDK_HANDLE lRealHandle);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_StartLocalRecord(SDK_HANDLE lRealHandle, string szSaveFileName, int type);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_OpenSound(SDK_HANDLE lHandle);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_LocalSetColor(SDK_HANDLE lHandle, UInt32 nRegionNum, int nBrightness, int nContrast, int nSaturation, int nHue);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_LocalGetColor(SDK_HANDLE lHandle, UInt32 nRegionNum, out int pBrightness, out int pContrast, out int pSaturation, out int pHue);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_MakeKeyFrame(SDK_HANDLE lLoginID, int nChannel, int nStream);

        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_StopRealPlay(SDK_HANDLE lLoginID, uint dwUser);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_CloseSound(SDK_HANDLE lHandle);

        [DllImport("NetSdk.dll")]
        public static extern SDK_HANDLE H264_DVR_RealPlay(SDK_HANDLE lLoginID, ref H264_DVR_CLIENTINFO lpClientInfo);
  
        [DllImport("NetSdk.dll")]
        public static extern SDK_HANDLE H264_DVR_Login(StringBuilder sDVRIP, ushort wDVRPort, StringBuilder sUserName, StringBuilder sPassword,
                              out H264_DVR_DEVICEINFO lpDeviceInfo, out short error, Int32 socketstyle);
        [DllImport("NetSdk.dll")]
        public static extern Int32 H264_DVR_Logout(SDK_HANDLE lLoginID);//登出设备
        [DllImport("NetSdk.dll")]
        public static extern void DisConnectBackCallFunc(SDK_HANDLE lLoginID, IntPtr pchDVRIP, ushort nDVRPort, uint dwUser);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_OpenTransComChannel(SDK_HANDLE lLoginID, ref TransComChannel TransInfo, fTransComCallBack cbTransCom, IntPtr dwUser);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_CloseTransComChannel(SDK_HANDLE lLoginID, SERIAL_TYPE nType);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_SerialWrite(SDK_HANDLE lLoginID, SERIAL_TYPE nType, string Buffer, int nBufLen);

        [DllImport("NetSdk.dll")]
        public static extern SDK_HANDLE H264_DVR_StartLocalVoiceCom(SDK_HANDLE lLoginID);

        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_StopVoiceCom(long lVoiceHandle);

        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_GetDDNSInfo(ref SearchMode searchmode, out DDNS_INFO[] pDevicInfo, int maxDeviceNum, out int nretNum);

        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_SetupAlarmChan(SDK_HANDLE lLoginID);

        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_CloseAlarmChan(SDK_HANDLE lLoginID);
        
        // 播放本地文件
        [DllImport("NetSdk.dll")]
        public static extern SDK_HANDLE H264_DVR_StartLocalPlay(string pFileName, IntPtr hWnd, fPlayDrawCallBack drawCallBack, uint user);

        //关闭本地文件
        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_StopLocalPlay(SDK_HANDLE lPlayHandle);

        //获取播放位置用于回放，和本地播放
        [DllImport("NetSdk.dll")]
        public static extern float H264_DVR_GetPlayPos(SDK_HANDLE lPlayHandle);
        //设置播放位置（百分比）用于回放，和本地播放
        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_SetPlayPos(SDK_HANDLE lPlayHandle, float fRelativPos);
        //播放控制（播放，停止，恢复，快发，慢放）
        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_LocalPlayCtrl(SDK_HANDLE lPlayHandle, SDK_LoalPlayAction action, uint lCtrlValue);
        //设置播放结束回调
        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_SetFileEndCallBack(SDK_HANDLE lPlayHandle, fLocalPlayFileCallBack callBack, IntPtr user);
        //设置信息帧回调
        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_SetInfoFrameCallBack(SDK_HANDLE lPlayHandle, InfoFramCallBack callback, uint user);

        //录像查询
        //lLoginID		登陆句柄
        //lpFindInfo	查询条件
        //lpFileData	查找到的录像数据，外部开内存
        //lMaxCount		最大录像数目
        //findcount		查找到的录像数目
        //waittime		查询超时时间
        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_FindFile(SDK_HANDLE lLoginID, ref H264_DVR_FINDINFO lpFindInfo, IntPtr ptr, int lMaxCount, out int findcount, int waittime);
        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_GetFileByName(SDK_HANDLE lLoginID, ref H264_DVR_FILE_DATA sPlayBackFile, string sSavedFileName, 
											fDownLoadPosCallBack cbDownLoadPos , int dwDataUser , fRealDataCallBack fDownLoadDataCallBack);
        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_GetFileByTime(SDK_HANDLE lLoginID, ref H264_DVR_FINDINFO lpFindInfo, string sSavedFileDIR, bool bMerge,
											fDownLoadPosCallBack cbDownLoadPos , int dwDataUser, fRealDataCallBack fDownLoadDataCallBack );
        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_PlayBackByName_V2(SDK_HANDLE lLoginID, ref H264_DVR_FILE_DATA sPlayBackFile, fDownLoadPosCallBack cbDownLoadPos, fRealDataCallBack_V2 fDownLoadDataCallBack, int dwDataUser);
        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_PlayBackByTimeEx(SDK_HANDLE lLoginID, ref H264_DVR_FINDINFO lpFindInfo, fRealDataCallBack fDownLoadDataCallBack, int dwDataUser,
											fDownLoadPosCallBack cbDownLoadPos, int dwPosUser);
        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_PlayBackControl(SDK_HANDLE lPlayHandle, int lControlCode, int lCtrlValue);
        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_StopGetFile(SDK_HANDLE lFileHandle);
           
        [DllImport("NetSdk.dll")]
        public static extern bool H264_DVR_StopPlayBack(SDK_HANDLE lPlayHandle);
        [DllImport("NetSdk.dll")]
        public static extern int H264_DVR_GetDownloadPos(SDK_HANDLE lFileHandle);

         [DllImport("NetSdk.dll")]
        //抓本地图片 //预览，回放，本地播放 一个函数
        public static extern bool H264_DVR_LocalCatchPic(SDK_HANDLE lHandle, string strPath);

        [DllImport("NetSdk.dll")]
         public static extern bool H264_DVR_PTZControl(SDK_HANDLE lLoginID, int nChannelNo, int lPTZCommand, bool bStop, long lSpeed);

        public static int ToInt(string text, int nDef)
        {
            try
            {
                return int.Parse(text);
            }
            catch
            {
                
            }
            return nDef;
        }
        public static int ToInt(string text)
        {
            return ToInt(text, 0);
        }
    }

}
