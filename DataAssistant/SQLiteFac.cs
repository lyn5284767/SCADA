using DatabaseLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DatabaseLib
{
    public class SQLiteFac : IDataFactory
    {
        public bool BulkCopy(IDataReader reader, string tableName, string command = null, SqlBulkCopyOptions options = SqlBulkCopyOptions.Default)
        {
            while (reader.Read())
            {
            }
            return true;
        }

        public void CallException(string message)
        {
            throw new NotImplementedException();
        }

        public bool ConnectionTest()
        {
            throw new NotImplementedException();
        }

        public DbParameter CreateParam(string paramName, SqlDbType dbType, object objValue, int size = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            throw new NotImplementedException();
        }

        public DataRow ExecuteDataRowProcedure(string ProName, params DbParameter[] ParaName)
        {
            throw new NotImplementedException();
        }

        public DataRowView ExecuteDataRowViewProcedure(string ProName, params DbParameter[] ParaName)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataset(string SQL)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataset(string[] SQLs, string[] TableNames)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataset(string SQL, string TableName)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataSetProcedure(string ProName, params DbParameter[] ParaName)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataSetProcedure(string ProName, ref int returnValue, params DbParameter[] ParaName)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteDataTable(string SQL)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteDataTableProcedure(string ProName, params DbParameter[] ParaName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>        
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=Monitor.db3;Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }

        public DataTable ExecuteDataTableProcedure(string ProName, ref int returnValue, DbParameter[] ParaName)
        {
            throw new NotImplementedException();
        }

        public List<T> ExecuteList<T>(string SQL) where T : class, new()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=Monitor.db3;Version=3;"))
            {
                SQLiteCommand cmd = new SQLiteCommand(SQL, m_dbConnection);
                try
                {
                    if (cmd.Connection.State == ConnectionState.Closed)
                        cmd.Connection.Open();
                    DataSet ds = new DataSet();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    da.Dispose();
                    return ConvertToModel<T>(dt);
                }
                catch (Exception ex)
                {
                    Log.Log4Net.AddLog(ex.ToString(), Log.InfoLevel.ERROR);
                    return null;
                }
                finally
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
        }

        public int ExecuteNonQuery(string[] SQLs)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=Monitor.db3;Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLs.Count(); n++)
                    {
                        string strsql = SQLs[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return 1;
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }

        public int ExecuteNonQuery(string SQL)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=Monitor.db3;Version=3;"))
            {
                SQLiteCommand cmd = new SQLiteCommand(SQL, m_dbConnection);
                try
                {
                    if (cmd.Connection.State == ConnectionState.Closed)
                        cmd.Connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    cmd.Dispose();
                    return result;
                }
                catch (Exception ex)
                {
                    Log.Log4Net.AddLog(ex.ToString(), Log.InfoLevel.ERROR);
                    return 0;
                }
                finally
                {
                
                }
            }

        }

        public int ExecuteNonQuery(string[] SQLs, object[][] Pars)
        {
            throw new NotImplementedException();
        }

        public DbDataReader ExecuteProcedureReader(string sSQL, params DbParameter[] ParaName)
        {
            throw new NotImplementedException();
        }

        public DbDataReader ExecuteReader(string sSQL)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar(string sSQL)
        {
            throw new NotImplementedException();
        }

        public bool ExecuteStoredProcedure(string ProName)
        {
            throw new NotImplementedException();
        }

        public int ExecuteStoredProcedure(string ProName, params DbParameter[] ParaName)
        {
            throw new NotImplementedException();
        }

        public void FillDataSet(ref DataSet ds, string SQL, string TableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// add by lyn,2020.3.30
        /// DataTable映射到List
        /// </summary>
        public List<T> ConvertToModel<T>(DataTable dt) where T : class, new()
        {
            List<T> ts = new List<T>();// 定义集合
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite) continue;
                        var value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            if (pi.PropertyType.FullName.Contains("System.Nullable"))
                            {
                                pi.SetValue(t, Convert.ChangeType(value, (Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType)), null);
                            }
                            else
                            {
                                switch (pi.PropertyType.FullName)
                                {
                                    case "System.Decimal":
                                        pi.SetValue(t, decimal.Parse(value.ToString()), null);
                                        break;
                                    case "System.String":
                                        pi.SetValue(t, value.ToString(), null);
                                        break;
                                    case "System.Char":
                                        pi.SetValue(t, Convert.ToChar(value), null);
                                        break;
                                    case "System.Guid":
                                        pi.SetValue(t, value, null);
                                        break;
                                    case "System.Int16":
                                        pi.SetValue(t, Convert.ToInt16(value), null);
                                        break;
                                    case "System.Int32":
                                        pi.SetValue(t, int.Parse(value.ToString()), null);
                                        break;
                                    case "System.Int64":
                                        pi.SetValue(t, Convert.ToInt64(value), null);
                                        break;
                                    case "System.Byte[]":
                                        pi.SetValue(t, Convert.ToByte(value), null);
                                        break;
                                    case "System.Boolean":
                                        pi.SetValue(t, Convert.ToBoolean(value), null);
                                        break;
                                    case "System.Double":
                                        pi.SetValue(t, Convert.ToDouble(value.ToString()), null);
                                        break;
                                    case "System.DateTime":
                                        pi.SetValue(t, value ?? Convert.ToDateTime(value), null);
                                        break;
                                    default:
                                        throw new Exception("类型不匹配:" + pi.PropertyType.FullName);
                                }
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
    }
}
