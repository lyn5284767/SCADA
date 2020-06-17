using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Log
{
    public enum InfoLevel : byte
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3,
        FATAL = 4
    }

    public class Log4Net
    {
        static ILog _log;

        static Log4Net()
        {
            _log = LogManager.GetLogger("log");
        }

        public static ILog Instance
        {
            get
            {
                return _log;
            }
        }

        public static void AddLog(string message, InfoLevel infoLevel = InfoLevel.DEBUG)
        {
            switch (infoLevel)
            {
                case InfoLevel.DEBUG:
                    Instance.Debug(message);
                    break;
                case InfoLevel.INFO:
                    Instance.Info(message);
                    break;
                case InfoLevel.WARN:
                    Instance.Warn(message);
                    break;
                case InfoLevel.ERROR:
                    Instance.Error(message);
                    break;
                case InfoLevel.FATAL:
                    Instance.Fatal(message);
                    break;
                default:
                    break;
            }
        }
    }
}