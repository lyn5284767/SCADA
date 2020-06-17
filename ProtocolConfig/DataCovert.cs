using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;

namespace ProtocolConfig
{
    public class DataTypeSource
    {
        byte _type;
        public byte DataType { get { return _type; } set { _type = value; } }

        string _name;
        public string Name { get { return _name; } set { _name = value; } }

        public DataTypeSource(byte type, string name)
        {
            _type = type;
            _name = name;
        }
    }

    public static class DataConvert //:MarshalByRefObject
    {
        public static DataTable ConvertTextToTable(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            DataTable mydt = new DataTable("");
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                using (var mysr = new StreamReader(stream))
                {
                    string strline = mysr.ReadLine();
                    string[] aryline = strline.Split('\t');
                    for (int i = 0; i < aryline.Length; i++)
                    {
                        aryline[i] = aryline[i].Replace("\"", "");
                        mydt.Columns.Add(new DataColumn(aryline[i] + i));
                    }
                    int intColCount = aryline.Length;
                    while ((strline = mysr.ReadLine()) != null)
                    {
                        aryline = strline.Split('\t');
                        DataRow mydr = mydt.NewRow();
                        for (int i = 0; i < intColCount; i++)
                        {
                            mydr[i] = aryline[i].Replace("\"", "");
                        }
                        mydt.Rows.Add(mydr);
                    }
                    return mydt;
                }
            }
        }

        public static DataTable ConvertCSVToTable(string str)
        {
            DataTable mydt = new DataTable("");
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                using (var mysr = new StreamReader(stream))
                {
                    string strline = mysr.ReadLine();

                    Regex reg = new Regex(@",(?=(?:[^\""]*\""[^\""]*\"")*(?![^\""]*\""))");
                    string[] aryline = reg.Split(strline);
                    for (int i = 0; i < aryline.Length; i++)
                    {
                        aryline[i] = aryline[i].Replace("\"", "");
                        mydt.Columns.Add(new DataColumn(aryline[i]));
                    }
                    int intColCount = aryline.Length;
                    while ((strline = mysr.ReadLine()) != null)
                    {
                        aryline = reg.Split(strline);

                        DataRow mydr = mydt.NewRow();
                        for (int i = 0; i < intColCount; i++)
                        {
                            mydr[i] = aryline[i].Replace("\"", "");
                        }
                        mydt.Rows.Add(mydr);
                    }
                    return mydt;
                }
            }
        }
    }
}
