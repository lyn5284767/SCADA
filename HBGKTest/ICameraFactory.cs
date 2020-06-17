using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBGKTest
{
    public delegate void ChangeVideo();
    public delegate void FullScreen();
    public interface ICameraFactory
    {
        ChannelInfo Info { get; set; }
        /// <summary>
        /// 初始化摄像头
        /// </summary>
        /// <returns></returns>
        bool InitCamera(ChannelInfo chInfo);
        /// <summary>
        /// 播放视频
        /// </summary>
        void PlayCamera();
        /// <summary>
        /// 停止视频
        /// </summary>
        void StopCamera();
        /// <summary>
        /// 保存录像
        /// </summary>
        void SaveFile(string filePath, string fileName);
        /// <summary>
        /// 停止录像
        /// </summary>
        void StopFile();
        /// <summary>
        /// 设置大小
        /// </summary>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        void SetSize(double height, double width);

        event ChangeVideo ChangeVideoEvent;
        event FullScreen FullScreenEvent;
    }
}
