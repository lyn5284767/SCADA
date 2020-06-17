using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Log
{
    public class LogBytes
    {
        static readonly string m_Path = Environment.CurrentDirectory + @"\Logs";
        static string m_FullPath;
        static bool CreateFile()
        {
            DateTime date = DateTime.Now;
            int year = date.Year; int month = date.Month; int day = date.Day; int minute = date.Minute; int hour = date.Hour; int second = date.Second;
            m_FullPath = string.Concat(m_Path, "\\", year.ToString(), "-", month.ToString(), "-", day.ToString(), "-", hour.ToString(), "-", minute.ToString(), "-", second.ToString(), "-raw.bin");
            try
            {
                if (Directory.Exists(m_Path))
                {
                    if (File.Exists(m_FullPath))
                    {
                        return true;
                    }
                }
                else
                {
                    Directory.CreateDirectory(m_Path);
                }
                using (var stream = File.Create(m_FullPath))
                {
                    return true;
                }
            }
            catch (Exception err)
            {
                Log4Net.AddLog(err.Message, InfoLevel.ERROR);
                return false;
            }
        }

        static LogBytes()
        {
            CreateFile();
        }

        public static void WriteBytesToFile(byte[] bytes, int cnt)
        {
            try
            {
                using (var sw = new FileStream(m_FullPath, FileMode.Append, FileAccess.Write))
                {
                    using (var bw = new BinaryWriter(sw))
                    {
                        byte[] bTemp = new byte[cnt + 4];

                        Array.Copy(bytes, bTemp, cnt);

                        bTemp[cnt] = 0xff;
                        bTemp[cnt + 1] = 0xff;
                        bTemp[cnt + 2] = 0xff;
                        bTemp[cnt + 3] = 0xff;

                        bw.Write(bTemp);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.AddLog(ex.StackTrace);
            }
        }
    }
}