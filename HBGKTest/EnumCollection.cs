using System;

namespace HBGKTest
{
    #region ��ʾģʽö��
    /// <summary>
    /// ��ʾģʽö��
    /// </summary>
    public enum EnumCCTVVideoMode
    {
        /// <summary>
        /// ��������ʾ
        /// </summary>
        ModeOne = 1,
        /// <summary>
        /// ��������ʾ
        /// </summary>
        ModeThree = 3,
        /// <summary>
        /// �Ļ�����ʾ
        /// </summary>
        ModeFour = 4,
        /// <summary>
        /// �Ż�����ʾ
        /// </summary>
        ModeNine = 9,
        /// <summary>
        /// ʮ��������ʾ
        /// </summary>
        ModeSixteen = 16,
        /// <summary>
        /// ʮ�˻�����ʾ
        /// </summary>
        ModeEighteen = 18

    }
    #endregion

    #region ��Ƶ�������͵�ö��
    /// <summary>
    /// ��Ƶ�������͵�ö��
    /// </summary>
    public enum EnumCCTVControlType
    {
        /// <summary>
        /// ��Ч����
        /// </summary>
        INVALID_COMMAND=0,

        /// <summary>
        /// ����
        /// </summary>
        TILT_UP,
        /// <summary>
        /// ����
        /// </summary>
        TILT_DOWN,
        /// <summary>
        /// ����
        /// </summary>
        PAN_LEFT,
        /// <summary>
        /// ����
        /// </summary>
        PAN_RIGHT,
        /// <summary>
        /// ��С
        /// </summary>
        ZOOM_OUT,
        /// <summary>
        /// �Ŵ�
        /// </summary>
        ZOOM_IN,
        /// <summary>
        /// ��Ȧ�Ŵ�
        /// </summary>
        IRIS_OPEN,
        /// <summary>
        /// ��Ȧ��С
        /// </summary>
        IRIS_CLOSE,
        /// <summary>
        /// ����Ŵ�
        /// </summary>
        FOCUS_FAR,
        /// <summary>
        /// ������С
        /// </summary>
        FOCUS_NEAR,
        /// <summary>
        /// ֹͣ����
        /// </summary>
        STOP,
        /// <summary>
        /// ����Ԥ��λ
        /// </summary>
        SET_PRESET,
        /// <summary>
        /// ����Ԥ��λ
        /// </summary>
        GO_PRESET,
        /// <summary>
        /// ѡ��ĳ�������
        /// </summary>
        CAM_SELECT,
        /// <summary>
        /// ѡ����һ�������
        /// </summary>
        CAM_NEXT,
        /// <summary>
        /// ѡ����һ�������
        /// </summary>
        CAM_PREV,
        /// <summary>
        /// ����
        /// </summary>
        Pan_NW,
        /// <summary>
        /// ����
        /// </summary>
        Pan_NE,
        /// <summary>
        /// ����
        /// </summary>
        Pan_SE,
        /// <summary>
        /// ����
        /// </summary>
        Pan_SW
    }

    #endregion

    #region ����������

    public enum EncoderType
    {
        BOKANG = 1,
        ADV = 2,  //�Ƚ���Ѷ3.0 ��վ��ʽ
        OMT = 3,
        DAHUA = 4,  //��
        DALI = 5,       //����
        HAIKANG = 6,      //����
        H3C = 7,  //����Ivs
        SLT = 8,    //����־�Ϲھ�
        TianDySS = 9,    //���ΰҵ��ý�������
        ALK = 10,
        BlueStar = 11,     //��ɫ�Ǽʿؼ���������Ƶ���
        BOKANGDG = 12,
        Ppvsguard = 13,//�Ĵ��˱���gps�����ϵ���Ƶ������������
        PuTian = 14,//�Ĵ��˱�
        NanWang = 15,//����
        NetPosa = 16, //��������
        H3cIMos = 17,//����IMos
        VoxCvsa = 18, //��ʽCVSA
        DaHuaCom = 19,  //��Com
        JingSL = 20,  //������DVR/DVS
        JingSLserver = 21,  //������ת��������
        IDVR50 = 22,  //�Ƚ���ѶIDVR5.0
        NrCap = 23,   //���մ���
        ZxNvms = 24,    //����
        GA1 = 25,  //������һ��
        ZCPlat = 26,    //־�ɹھ�����sdk
        DaHuaOcx = 27,  //��Ocx
        Sunpanel = 28,  //����
        ZXing = 29,//����
        WinVroad = 30,//��·��
        ZxViss = 31,     //������Ƶ���ؼ�
        Pelco = 32, //�ɶ���
        DaHeng = 33, //�ɶ���
        API = 34,//������
        ZhongDian = 35,//�е��·�
        LPT = 36,  //������
        LPT_OCX = 37,  //������
        HBGK = 38,  //����߿�
    }

    #endregion

    #region ���ƿؼ�����

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
