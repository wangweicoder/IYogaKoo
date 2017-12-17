using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Configuration;

namespace IYogaKoo.Dao
{
    public static class SQLHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string strconn = ConfigurationManager.ConnectionStrings["iyogakoo"].ToString();

        #region 原始
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="cmdTxt">命令文本</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="prms">数组集合</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteDataTable(string cmdTxt, CommandType cmdType, params SqlParameter[] prms)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(strconn);
            SqlDataAdapter da = new SqlDataAdapter(cmdTxt, conn);
            da.SelectCommand.CommandType = cmdType;

            if (prms != null)
            {
                foreach (SqlParameter prm in prms)
                {
                    da.SelectCommand.Parameters.Add(prm);
                }
            }

            da.Fill(dt);
            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
                conn.Dispose();
            }
            return dt;
        }
        /// <summary>
        /// 得到最大id
        /// </summary>
        /// <param name="FieldName">字段名</param>
        /// <param name="TableName">名表</param>
        /// <returns></returns>
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = ExecuteScalar(strsql, CommandType.Text, null);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        /// <summary>
        /// 返回ExecuteDataReader  调用后一定要关闭SqlDataReader
        /// </summary>
        /// <param name="cmdTxt">命令文本</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="prms">数组集合</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteDataReader(string cmdTxt, CommandType cmdType, params SqlParameter[] prms)
        {
            SqlDataReader dr = null;
            SqlConnection conn = new SqlConnection(strconn);

            conn.Open();
            SqlCommand cmd = new SqlCommand(cmdTxt, conn);
            cmd.CommandType = cmdType;

            if (prms != null)
            {
                foreach (SqlParameter prm in prms)
                {
                    cmd.Parameters.Add(prm);
                }
            }
            try
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                throw;
            }
            finally
            {
                //cmd.Dispose();
                //dr.Close();
            }
            return dr;
        }

        /// <summary>
        /// 返回IN型
        /// </summary>
        /// <param name="cmdTxt">命令文本</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="prms">数组集合</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string cmdTxt, CommandType cmdType, params SqlParameter[] prms)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand(cmdTxt, conn);
                cmd.CommandType = cmdType;

                if (prms != null)
                {
                    foreach (SqlParameter prm in prms)
                    {
                        cmd.Parameters.Add(prm);
                    }
                }
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            return result;
        }
        /// <summary>
        /// 2012-2-21新增重载，执行查询语句，返回DataSet
        /// </summary>
        /// <param name="connection">SqlConnection对象</param>
        /// <param name="trans">SqlTransaction事务</param>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(SqlConnection connection, SqlTransaction trans, string SQLString, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, trans, SQLString, cmdParms);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds, "ds");
                    cmd.Parameters.Clear();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    //trans.Rollback();
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        /// <summary>
        /// 2012-2-29新增重载，执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="connection">SqlConnection对象</param>
        /// <param name="trans">SqlTransaction对象</param>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(SqlConnection connection, SqlTransaction trans, string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, trans, SQLString, cmdParms);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    //trans.Rollback();
                    throw e;
                }
            }
        }
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        /// <summary>
        /// 2012-2-21新增重载，执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="connection">SqlConnection对象</param>
        /// <param name="trans">SqlTransaction事务</param>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(SqlConnection connection, SqlTransaction trans, string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, trans, SQLString, cmdParms);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    //trans.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// 返回Object型
        /// </summary>
        /// <param name="cmdTxt">命令文本</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="prms">数组集合</param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdTxt, CommandType cmdType, params SqlParameter[] prms)
        {
            object result;
            using (SqlConnection conn = new SqlConnection(strconn))
            {
                SqlCommand cmd = new SqlCommand(cmdTxt, conn);
                cmd.CommandType = cmdType;

                if (prms != null)
                {
                    foreach (SqlParameter prm in prms)
                    {
                        cmd.Parameters.Add(prm);
                    }
                }
                conn.Open();
                result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            return result;
        }
        #endregion

        #region 现代
        /// <summary>
        /// OleDb数据库增、删、改方法
        /// </summary>
        /// <param name="sql">执行数据库操作语句</param>
        /// <param name="param">参数数组</param>
        /// <returns>返回int类型，返回0则操作失败，返回数大于0则操作成功</returns>
        public static int ExecuteNonquery(string sql, params SqlParameter[] param)
        {
            int bFlag = 0;
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection con = new SqlConnection(strconn))
            {
                cmd.Connection = con;
                cmd.CommandText = sql;

                if (param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }

                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    bFlag = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return bFlag;
        }

        /// <summary>
        /// OleDb数据库增、删、改方法
        /// </summary>
        /// <param name="sql">执行数据库操作语句</param>
        /// <param name="param">参数数组</param>
        /// <returns>返回bool类型</returns>
        public static bool ExecuteNonqueryBool(string sql, params SqlParameter[] param)
        {
            bool bFlag = false;
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection con = new SqlConnection(strconn))
            {
                cmd.Connection = con;
                cmd.CommandText = sql;

                if (param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }

                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        bFlag = true;
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return bFlag;
        }

        /// <summary>
        /// OleDb数据库查询方法
        /// </summary>
        /// <param name="sql">执行数据库操作语句</param>
        /// <param name="param">参数数组</param>
        /// <returns>返回DataTable类型</returns>
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] param)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            da.SelectCommand = cmd;
            using (SqlConnection con = new SqlConnection(strconn))
            {
                cmd.Connection = con;
                cmd.CommandText = sql;

                if (param != null && param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                try
                {
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return dt;
        }
        /// <summary>
        /// OleDb数据库查询方法
        /// </summary>
        /// <param name="sql">执行数据库操作语句</param>
        /// <param name="param">参数数组</param>
        /// <returns>返回DataSet类型</returns>
        public static DataSet ExecuteDataSet(string sql, params SqlParameter[] param)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            da.SelectCommand = cmd;
            using (SqlConnection con = new SqlConnection(strconn))
            {
                cmd.Connection = con;
                cmd.CommandText = sql;

                if (param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                finally
                { }
            }
            return ds;
        }
        /// <summary>
        /// OleDb数据库查询方法
        /// </summary>
        /// <param name="sql">执行数据库操作语句</param>
        /// <param name="param">参数数组</param>
        /// <returns>返回DataSet类型</returns>
        public static DataSet QueryDataSet(string sql, params SqlParameter[] param)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            da.SelectCommand = cmd;
            using (SqlConnection con = new SqlConnection(strconn))
            {
                cmd.Connection = con;
                cmd.CommandText = sql;
                if (param != null)
                {
                    if (param.Length > 0)
                    {
                        foreach (SqlParameter p in param)
                        {
                            cmd.Parameters.Add(p);
                        }
                    }
                }
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }
        /// <summary>
        /// OleDb数据库查询方法
        /// </summary>
        /// <param name="sql">执行数据库操作语句</param>
        /// <param name="param">参数数组</param>
        /// <returns>返回ArrayList类型</returns>
        public static ArrayList ExecuteArrayList(string sql, params SqlParameter[] param)
        {
            SqlDataReader reader = null;
            ArrayList al = new ArrayList();
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection con = new SqlConnection(strconn))
            {
                cmd.Connection = con;
                cmd.CommandText = sql;

                if (param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        al.Add(reader[0]);
                    }
                    //while (reader.Read())
                    //{
                    //    //for (int i = 1; i < reader.FieldCount; i++)
                    //    //{ 
                    //    //    al.Add(reader[i]);
                    //    //}
                    //    foreach (Object obj in reader)
                    //    {
                    //        al.Add(obj);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                finally
                {
                    reader.Close();
                    con.Close();
                }
            }
            return al;
        }

        /// <summary>
        /// OleDb数据库查询方法
        /// </summary>
        /// <param name="sql">执行数据库操作语句</param>
        /// <param name="param">参数数组</param>
        /// <returns>返回Object类型</returns>
        public static Object ExecuteObject(string sql, params SqlParameter[] param)
        {
            SqlDataReader reader = null;
            Object obj = null;
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection con = new SqlConnection(strconn))
            {
                cmd.Connection = con;
                cmd.CommandText = sql;

                if (param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        obj = reader[0];
                    }
                    //while (reader.Read())
                    //{
                    //    //for (int i = 1; i < reader.FieldCount; i++)
                    //    //{ 
                    //    //    al.Add(reader[i]);
                    //    //}
                    //    foreach (Object obj in reader)
                    //    {
                    //        al.Add(obj);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
                finally
                {
                    reader.Close();
                    con.Close();
                }
            }
            return obj;
        }

        /// <summary>
        /// OleDb数据库验证方法
        /// </summary>
        /// <param name="sql">执行数据库操作语句</param>
        /// <param name="param">参数数组</param>
        /// <returns>返回bool类型</returns>
        public static bool Exists(string sql, params SqlParameter[] param)
        {
            SqlDataReader reader = null;
            bool flag = false;
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection con = new SqlConnection(strconn))
            {
                cmd.Connection = con;
                cmd.CommandText = sql;

                if (param.Length > 0)
                {
                    foreach (SqlParameter p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        flag = true;
                    }
                }
                catch (Exception ex)
                {
                    flag = false;
                    string msg = ex.Message;
                }
                finally
                {
                    reader.Close();
                    con.Close();
                }
            }
            return flag;
        }

        /// <summary>
        ///  获得参数对象 
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramType">数据类型</param>
        /// <param name="paramSize">长度</param>
        /// <param name="ColName">源列名称</param>
        /// <param name="paramValue">参数实值</param>
        /// <returns></returns>
        public static SqlParameter GetParameter(String paramName, SqlDbType paramType, Int32 paramSize, String ColName, Object paramValue)
        {
            SqlParameter param = new SqlParameter(paramName, paramType, paramSize, ColName);
            param.Value = paramValue;
            return param;
        }

        /// <summary>
        /// 获得参数对象
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramType">数据类型</param>
        /// <param name="paramSize">长度</param>
        /// <param name="ColName">源列名称</param>
        /// <returns></returns>
        public static SqlParameter GetParameter(String paramName, SqlDbType paramType, Int32 paramSize, String ColName)
        {
            SqlParameter param = new SqlParameter(paramName, paramType, paramSize, ColName);
            return param;
        }

        /// <summary>
        /// 获得参数对象
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramType">数据类型</param>
        /// <param name="paramSize">长度</param>
        /// <param name="ColName">源列名称</param>
        /// <returns></returns>
        public static SqlParameter GetParameter(String paramName, SqlDbType paramType, Object paramValue)
        {
            SqlParameter param = new SqlParameter(paramName, paramType);
            param.Value = paramValue;
            return param;
        }


        #endregion
        #region 存储过程操作

        /// <summary>
        /// 执行存储过程，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(strconn);
            SqlDataReader returnReader;
            connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;

        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(strconn))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            using (SqlConnection connection = new SqlConnection(strconn))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = Times;
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }


        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (SqlConnection connection = new SqlConnection(strconn))
            {
                int result;
                connection.Open();
                SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion
    }
}
