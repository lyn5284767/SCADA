using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Common
{
    /// <summary>
    /// 处理摄像头操作
    /// </summary>
    public class Camera
    {
        private static Camera _instance = null;
        private static readonly object syncRoot = new object();

        private Camera()
        {
        }

        public static Camera Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Camera();
                        }
                    }
                }
                return _instance;
            }
        }

        public void InitCameraInfo()
        { 
        }
    }
}
