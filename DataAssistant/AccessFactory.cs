using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using DAO = Microsoft.Office.Interop.Access.Dao;

namespace DatabaseLib
{
    public class AccessFactory : IDataFactory
    {
        public bool BulkCopy(IDataReader reader, string tableName, string command = null, SqlBulkCopyOptions options = SqlBulkCopyOptions.Default)
        {
            HDAAccessReader accessReader = reader as HDAAccessReader;

            if (accessReader == null)
            {
                CallException("批量拷贝数据到Access数据库失败：BulkCopy 函数中，IDataReader 转 HDAAccessReader 失败！");
                return false;
            }



            DAO.DBEngine dbEngine = new DAO.DBEngine();
            DAO.Database db =  dbEngine.OpenDatabase(DataHelper.DataPath);

            if (!string.IsNullOrEmpty(command))
            {
                db.Execute(command);
            }
             
            DAO.Recordset rs = db.OpenRecordset(tableName);

            try
            {
                DAO.Field[] myFields = new DAO.Field[accessReader.FieldCount];
                myFields[0] = rs.Fields[accessReader.GetName(0)];
                myFields[1] = rs.Fields[accessReader.GetName(1)];
                myFields[2] = rs.Fields[accessReader.GetName(2)];

                while (accessReader.Read())
                {
                    rs.AddNew();
                    myFields[0].Value = accessReader.GetValue(0);
                    myFields[1].Value = accessReader.GetValue(1);
                    myFields[2].Value = accessReader.GetValue(2);

                    rs.Update();
                }
            }
            catch (Exception e)
            {
                CallException(e.Message);

                return false;
            }
            finally
            {
                rs.Close();
                db.Close();
            }

            return true;

        }

        public void CallException(string message)
        {
            DataHelper.AddErrorLog(new Exception(message));
        }

        public bool ConnectionTest()
        {
            //创建连接对象
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                try
                {
                    //Open DataBase
                    //打开数据库
                    m_Conn.Open();
                    if (m_Conn.State == ConnectionState.Open)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    CallException(e.Message);
                }
            }

            return false;
        }

        public DbParameter CreateParam(string paramName, SqlDbType dbType, object objValue, int size = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            OleDbParameter parameter = new OleDbParameter(paramName, dbType);
            if (size > 0) parameter.Size = size;
            if (objValue == null)
            {
                if (direction == ParameterDirection.Output)
                {
                    parameter.Direction = direction;
                    return parameter;
                }
                parameter.IsNullable = true;
                parameter.Value = DBNull.Value;
                return parameter;
            }
            parameter.Value = objValue;
            return parameter;
        }

        public DataRow ExecuteDataRowProcedure(string ProName, params DbParameter[] ParaName)
        {
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(ProName, m_Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (ParaName != null)
                    {
                        cmd.Parameters.AddRange(ParaName);
                    }
                    if (m_Conn.State == ConnectionState.Closed)
                        m_Conn.Open();
                    DataTable table = new DataTable();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(table);
                    if (table.Rows.Count > 0)
                        return table.Rows[0];
                    else
                        return table.NewRow();
                }
                catch (Exception e)
                {
                    CallException(ProName + "        " + e.Message);
                    return null;
                }
            }
        }

        public DataRowView ExecuteDataRowViewProcedure(string ProName, params DbParameter[] ParaName)
        {
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(ProName, m_Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (ParaName != null)
                    {
                        cmd.Parameters.AddRange(ParaName);
                    }
                    if (m_Conn.State == ConnectionState.Closed)
                        m_Conn.Open();
                    DataTable table = new DataTable();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(table);
                    if (table.Rows.Count > 0)
                        return table.DefaultView[0];
                    else
                        return table.DefaultView.AddNew();
                }
                catch (Exception e)
                {
                    CallException(ProName + "        " + e.Message);
                    return null;
                }
            }
        }

        public DataSet ExecuteDataset(string SQL)
        {
            DataSet ds = new DataSet();
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                try
                {
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(SQL, m_Conn);
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
                catch (Exception e)
                {
                    CallException(SQL + "        " + e.Message);
                }
            }
            return ds;
        }

        public DataSet ExecuteDataset(string[] SQLs, string[] TableNames)
        {
            DataSet ds = new DataSet();
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                try
                {
                    for (int i = 0; i < SQLs.Length; i++)
                    {

                        OleDbDataAdapter da = new OleDbDataAdapter();
                        OleDbCommand cmd = new OleDbCommand(SQLs[i], m_Conn);
                        da.SelectCommand = cmd;
                        da.Fill(ds, TableNames[i]);
                    }
                }
                catch (Exception e)
                {
                    CallException(SQLs + "        " + e.Message);
                }
            }
            return ds;
        }

        public DataSet ExecuteDataset(string SQL, string TableName)
        {
            DataSet ds = new DataSet();
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                try
                {
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(SQL, m_Conn);
                    da.SelectCommand = cmd;
                    da.Fill(ds, TableName);
                }
                catch (Exception e)
                {
                    CallException(SQL + "        " + e.Message);
                }
            }
            return ds;
        }

        public DataSet ExecuteDataSetProcedure(string ProName, params DbParameter[] ParaName)
        {
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                DataSet ds = new DataSet();
                try
                {
                    OleDbCommand cmd = new OleDbCommand(ProName, m_Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (ParaName != null)
                    {
                        cmd.Parameters.AddRange(ParaName);
                    }
                    if (m_Conn.State == ConnectionState.Closed)
                        m_Conn.Open();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    return ds;
                }

                catch (Exception e)
                {
                    CallException(ProName + "        " + e.Message);
                    return null;
                }
            }
        }

        public DataSet ExecuteDataSetProcedure(string ProName, ref int returnValue, params DbParameter[] ParaName)
        {
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                DataSet ds = new DataSet();
                try
                {
                    OleDbCommand cmd = new OleDbCommand(ProName, m_Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (ParaName != null)
                    {
                        cmd.Parameters.AddRange(ParaName);
                    }
                    OleDbParameter param = new OleDbParameter { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(param);
                    if (m_Conn.State == ConnectionState.Closed)
                        m_Conn.Open();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    returnValue = (int)param.Value;
                    return ds;
                }
                catch (Exception e)
                {
                    CallException(ProName + "        " + e.Message);
                    return null;
                }
            }
        }

        public DataTable ExecuteDataTable(string SQL)
        {
            DataTable dt = new DataTable();
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                try
                {
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    OleDbCommand cmd = new OleDbCommand(SQL, m_Conn);
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception e)
                {
                    CallException(SQL + "        " + e.Message);
                }
            }
            return dt;
        }

        public DataTable ExecuteDataTableProcedure(string ProName, params DbParameter[] ParaName)
        {
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                DataTable ds = new DataTable();
                try
                {
                    OleDbCommand cmd = new OleDbCommand(ProName, m_Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (ParaName != null)
                    {
                        cmd.Parameters.AddRange(ParaName);
                    }
                    if (m_Conn.State == ConnectionState.Closed)
                        m_Conn.Open();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    return ds;
                }
                catch (Exception e)
                {
                    CallException(ProName + "        " + e.Message);
                    return null;
                }
            }
        }

        public DataTable ExecuteDataTableProcedure(string ProName, ref int returnValue, DbParameter[] ParaName)
        {
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                DataTable ds = new DataTable();
                try
                {
                    OleDbCommand cmd = new OleDbCommand(ProName, m_Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (ParaName != null)
                    {
                        cmd.Parameters.AddRange(ParaName);
                    }
                    OleDbParameter param = new OleDbParameter { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(param);
                    if (m_Conn.State == ConnectionState.Closed)
                        m_Conn.Open();
                    OleDbDataAdapter da = new OleDbDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    returnValue = (int)param.Value;
                    return ds;
                }
                catch (Exception e)
                {
                    CallException(ProName + "        " + e.Message);
                    return null;
                }
            }
        }

        public int ExecuteNonQuery(string[] SQLs)
        {
            int res = -1;
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                OleDbTransaction sqlT = null;
                OleDbCommand cmd = new OleDbCommand();
                try
                {
                    if (m_Conn.State == ConnectionState.Closed)
                        m_Conn.Open();
                    cmd.Connection = m_Conn;
                    sqlT = m_Conn.BeginTransaction();
                    cmd.Transaction = sqlT;
                    for (int i = 0; i < SQLs.Length; i++)
                    {
                        cmd.CommandText = SQLs[i];
                        res = cmd.ExecuteNonQuery();
                    }
                    sqlT.Commit();
                }
                catch (Exception e)
                {
                    if (sqlT != null)
                        sqlT.Rollback();
                    CallException(SQLs + "        " + e.Message);
                    res = -1;
                }
                return res;
            }
        }

        public int ExecuteNonQuery(string SQL)
        {
            int res = -1;
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                OleDbTransaction sqlT = null;
                try
                {
                    using (OleDbCommand cmd = new OleDbCommand(SQL, m_Conn))
                    {
                        if (m_Conn.State == ConnectionState.Closed)
                            m_Conn.Open();
                        cmd.Connection = m_Conn;
                        sqlT = m_Conn.BeginTransaction();
                        cmd.Transaction = sqlT;
                        res = cmd.ExecuteNonQuery();
                        sqlT.Commit();
                    }
                }
                catch (Exception e)
                {
                    if (sqlT != null)
                        sqlT.Rollback();
                    CallException(SQL + "   " + e.Message);
                    return -1;
                }
                return res;
            }
        }

        public int ExecuteNonQuery(string[] SQLs, object[][] Pars)
        {
            int res = -1;
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                OleDbTransaction sqlT = null;
                OleDbCommand cmd = new OleDbCommand();
                try
                {
                    if (m_Conn.State == ConnectionState.Closed)
                        m_Conn.Open();
                    cmd.Connection = m_Conn;
                    sqlT = m_Conn.BeginTransaction();
                    cmd.Transaction = sqlT;
                    for (int i = 0; i < SQLs.Length; i++)
                    {
                        cmd.CommandText = SQLs[i];
                        cmd.Parameters.Clear();
                        for (int j = 0; j < Pars[i].Length; j++)
                        {
                            cmd.Parameters.AddWithValue("@p" + j.ToString(), Pars[i][j]);
                        }
                        res = cmd.ExecuteNonQuery();
                    }
                    sqlT.Commit();
                }
                catch (Exception e)
                {
                    if (sqlT != null)
                        sqlT.Rollback();
                    CallException(SQLs + "        " + e.Message);
                    res = -1;
                }
                return res;
            }
        }

        public DbDataReader ExecuteProcedureReader(string sSQL, params DbParameter[] ParaName)
        {
            OleDbConnection connection = new OleDbConnection(DataHelper.ConnectString);
            OleDbCommand command = new OleDbCommand(sSQL, connection);
            command.CommandType = CommandType.StoredProcedure;
            if (ParaName != null)
            {
                command.Parameters.AddRange(ParaName);
            }
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                CallException(sSQL + "        " + e.Message);
                return null;
            }
        }

        public DbDataReader ExecuteReader(string sSQL)
        {
            OleDbConnection connection = new OleDbConnection(DataHelper.ConnectString);
            OleDbCommand command = new OleDbCommand(sSQL, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public object ExecuteScalar(string sSQL)
        {

            OleDbTransaction sqlT = null;
            
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                OleDbCommand cmd = new OleDbCommand(sSQL, m_Conn);
                try
                {
                    if (m_Conn.State == ConnectionState.Closed)
                        m_Conn.Open();
                    sqlT = m_Conn.BeginTransaction();
                    cmd.Transaction = sqlT;
                    var res = cmd.ExecuteScalar();
                    sqlT.Commit();
                    if (res == DBNull.Value) res = null;
                    return res;
                }
                catch (Exception e)
                {
                    if (sqlT != null)
                        sqlT.Rollback();
                    CallException(sSQL + "        " + e.Message);
                    return null;
                }
            }
        }

        public bool ExecuteStoredProcedure(string ProName)
        {
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                try
                {
                    if (m_Conn.State == ConnectionState.Closed)
                        m_Conn.Open();
                    OleDbCommand cmd = new OleDbCommand(ProName, m_Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    CallException(ProName + "        " + e.Message);
                    return false;
                }
            }
        }

        public int ExecuteStoredProcedure(string ProName, params DbParameter[] ParaName)
        {
            using (OleDbConnection m_Conn = new OleDbConnection(DataHelper.ConnectString))
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(ProName, m_Conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    if (ParaName != null)
                    {
                        cmd.Parameters.AddRange(ParaName);
                    }
                    OleDbParameter param = new OleDbParameter();
                    cmd.Parameters.Add(param);
                    param.Direction = ParameterDirection.ReturnValue;
                    if (m_Conn.State == ConnectionState.Closed)
                    {
                        m_Conn.Open();
                    }
                    cmd.ExecuteNonQuery();
                    return (int)param.Value;
                }
                catch (Exception e)
                {
                    CallException(ProName + "        " + e.Message);
                    return -1;
                }
            }
        }

        public void FillDataSet(ref DataSet ds, string SQL, string TableName)
        {
            try
            {
                OleDbConnection m_Conn;
                m_Conn = new OleDbConnection(DataHelper.ConnectString);
                OleDbDataAdapter da = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand(SQL, m_Conn);
                da.SelectCommand = cmd;
                da.Fill(ds, TableName);
            }
            catch (Exception e)
            {
                CallException(SQL + "        " + e.Message);
            }
        }

        public List<T> ExecuteList<T>(string SQL)
            where T : class, new()
        {
            return null;
        }
    }
}
