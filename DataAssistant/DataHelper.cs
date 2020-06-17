using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Log;

namespace DatabaseLib
{
    public static class DataHelper
    {
        static string m_DataPath = System.Environment.CurrentDirectory + @"\Data\Data.accdb";
        static string m_ConnStr = @"Provider= Microsoft.Ace.OLEDB.12.0;Data Source = " + m_DataPath;
        static string m_Path = System.Environment.CurrentDirectory + @"\HisData\";

        static string m_RemoteIP = "127.0.0.1";
        static string m_LocalIP = "127.0.0.1";
        static string m_RemotePort = "8081";
        static string m_LocalPort = "8080";
        static string m_type = "ACCESS";
        static string m_SaveBytes = "1";
        static string m_HaveVedio = "0";
        //数据库工厂接口  

        static  string INIPATH = System.Environment.CurrentDirectory + @"\Config.ini";
        const int STRINGMAX = 255;

        #region GetInstance
        private static IDataFactory _ins;

        public static IDataFactory Instance
        {
            get
            {
                return _ins;
            }
        }

        public static bool IsSaveBytes
        {
            get
            {
                int iTemp;
                if (int.TryParse(m_SaveBytes,out iTemp))
                {
                    if (iTemp == 1)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static bool HaveVedio
        {
            get
            {
                int iTemp;
                if (int.TryParse(m_HaveVedio, out iTemp))
                {
                    if (iTemp == 1)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static string RemoteIP
        {
            get { return m_RemoteIP; }
        }

        public static int RemotePort
        {
            get
            {
                int iPort;
                if (int.TryParse(m_RemotePort, out iPort))
                {
                    return iPort;
                }
                return 8081;
             }
        }

        public static string LocalIP
        {
            get { return m_LocalIP; }
        }

        public static int LocalPort
        {
            get
            {
                int iPort;
                if (int.TryParse(m_LocalPort, out iPort))
                {
                    return iPort;
                }
                return 8080;
            }
        }

        public static string ConnectString
        {
            get { return m_ConnStr; }
        }

        public static string DataPath
        {
            get { return m_DataPath; }
        }

        public static string HdaPath
        {
            get { return m_Path; }
        }
        #endregion
        /// <summary>  
        /// 数据库工厂构造函数  
        /// </summary>  
        /// <param name="dbtype">数据库枚举</param>  
        static DataHelper()
        {
            try
            {
                if (File.Exists(INIPATH))
                {
                    StringBuilder sb = new StringBuilder(STRINGMAX);

                    WinAPI.GetPrivateProfileString("HOST", "REMOTEIP", m_RemoteIP, sb, STRINGMAX, INIPATH);
                    m_RemoteIP = sb.ToString();

                    WinAPI.GetPrivateProfileString("HOST", "LOCALIP", m_LocalIP, sb, STRINGMAX, INIPATH);
                    m_LocalIP = sb.ToString();

                    WinAPI.GetPrivateProfileString("HOST", "REMOTEPORT", m_RemotePort, sb, STRINGMAX, INIPATH);
                    m_RemotePort = sb.ToString();

                    WinAPI.GetPrivateProfileString("HOST", "LOCALPORT", m_LocalPort, sb, STRINGMAX, INIPATH);
                    m_LocalPort = sb.ToString();

                    WinAPI.GetPrivateProfileString("DATABASE", "ARCHIVE", m_Path, sb, STRINGMAX, INIPATH);
                    m_Path = sb.ToString();
                    WinAPI.GetPrivateProfileString("DATABASE", "TYPE", m_type, sb, STRINGMAX, INIPATH);
                    m_type = sb.ToString();

                    WinAPI.GetPrivateProfileString("OTHERS", "SAVEBYTES", m_SaveBytes, sb, STRINGMAX, INIPATH);
                    m_SaveBytes = sb.ToString();

                    WinAPI.GetPrivateProfileString("OTHERS", "HAVEVEDIO", m_HaveVedio, sb, STRINGMAX, INIPATH);
                    m_HaveVedio = sb.ToString();

                    if (m_type.ToUpper().Equals("MSSQL"))
                    {
                        WinAPI.GetPrivateProfileString("DATABASE", "CONNSTRING", m_ConnStr, sb, STRINGMAX, INIPATH);
                        m_ConnStr = sb.ToString();
                    }
                }

                switch (m_type.ToUpper())
                {
                    case "MSSQL":
                        _ins = new MssqlFactory();
                        break;
                    case "ACCESS":
                        _ins = new AccessFactory();
                        break;
                    default:
                        _ins = new AccessFactory();
                        break;
                }
            } 
            catch (Exception e)
            {
                AddErrorLog(e);
            }
        }

        public static DbParameter CreateParam(string paramName, SqlDbType dbType, object objValue, int size = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            return _ins.CreateParam(paramName, dbType, objValue, size, direction);
        }

        public static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。  
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。  
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。  
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    var txt = row[colum] == null ? "" : row[colum].ToString();
                    if (colum.DataType == typeof(string) && txt.Contains(","))
                    {
                        sb.Append("\"" + txt.Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(txt);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static string ReaderToCsv(IDataReader reader)
        {  
            StringBuilder sb = new StringBuilder();
            var colcount = reader.FieldCount;
            while (reader.Read())
            {
                for (int i = 0; i < colcount; i++)
                {
                    if (i != 0) sb.Append(",");
                    var txt = reader[i] == null ? "" : reader[i].ToString();
                    if (txt.Contains(","))
                    {
                        sb.Append("\"" + txt.Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(txt);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static void AddErrorLog(Exception e)
        {
            string err = "";
            Exception exp = e;
            while (exp != null)
            {
                err += string.Format("{0}\n ", exp.Message);
                exp = exp.InnerException;
            }
            err += string.Format("{0}\n ", e.StackTrace);

            Log4Net.AddLog(err, InfoLevel.ERROR);
        }

        public static string GetNullableString(this DbDataReader reader, int index)
        {
            SqlDataReader dataReader = reader as SqlDataReader;
            if (dataReader != null)
            {
                var svr = dataReader.GetSqlString(index);
                return svr.IsNull ? null : svr.Value;
            }
            else return reader.GetString(index);
        }

        public static DateTime? GetNullableTime(this DbDataReader reader, int index)
        {
            SqlDataReader dataReader = reader as SqlDataReader;
            if (dataReader == null) return reader.GetDateTime(index);
            var svr = dataReader.GetSqlDateTime(index);
            return svr.IsNull ? default(Nullable<DateTime>) : svr.Value;
        }

        public static int GetTimeTick(this DbDataReader reader, int index)
        {
            SqlDataReader dataReader = reader as SqlDataReader;
            if (dataReader != null)
            {
                return dataReader.GetSqlDateTime(index).TimeTicks;
            }
            var datetime = reader.GetDateTime(index);
            var value = datetime.Subtract(new DateTime(1900, 1, 1));
            long num2 = value.Ticks - value.Days * 864000000000;
            if (num2 < 0)
                num2 += 864000000000;
            int num3 = (int)(num2 / 10000.0 * 0.3 + 0.5);
            if (num3 > 300 * 60 * 60 * 24 - 1)
                num3 = 0;
            return num3;
        }
    }


    public static class WinAPI
    {

        //参数说明：section：INI文件中的段落；key：INI文件中的关键字；val：INI文件中关键字的数值；filePath：INI文件的完整的路径和名称。
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        //参数说明：section：INI文件中的段落名称；key：INI文件中的关键字；def：无法读取时候时候的缺省数值；retVal：读取数值；size：数值的大小；filePath：INI文件的完整路径和名称。
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        public const Int32 WM_SYSCOMMAND = 274;
        public const UInt32 SC_CLOSE = 61536;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        public static void TextBox_Name_LostFocus(object sender, EventArgs e)
        {
            IntPtr TouchhWnd = new IntPtr(0);
            TouchhWnd = FindWindow("IPTip_Main_Window", null);
            if (TouchhWnd == IntPtr.Zero)
                return;
            PostMessage(TouchhWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
        }

        public static void TextBox_Name_GotFocus(object sender, EventArgs e)
        {
            try
            {
                dynamic file = "C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe";
                if (!System.IO.File.Exists(file))
                    return;
                Process.Start(file);
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }
    }
}