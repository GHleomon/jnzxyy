using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BeginScreen
{
    public static class DBHelper
    {
        private static readonly string _ConnectionString = ConfigurationManager.ConnectionStrings["MindraySqlString"].ToString();

        //App.config中如果加密，应先解密再使用
        //此处添加解密代码

        private static SqlConnection mConnection;
        /// <summary>
        /// 得到一个已经Open的Connection连接对象
        /// </summary>
        public static SqlConnection Connection
        {
            get
            {
                if (mConnection == null)
                {
                    mConnection = new SqlConnection(_ConnectionString);
                }

                if (mConnection.State == ConnectionState.Closed)
                {
                    mConnection.Open();
                }
                else if (mConnection.State == ConnectionState.Broken)
                {
                    mConnection.Close();
                    mConnection.Open();
                }
                return mConnection;
            }
        }

        /// <summary>
        /// 执行sql操作（增、删、改）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>执行的记录数</returns>
        public static int ExecNonQuery(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行sql操作（增、删、改），以参数方式传值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="values">参数数组</param>
        /// <returns>执行的记录数</returns>
        public static int ExecNonQuery(string sql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.Parameters.AddRange(values);
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行sql查询，得到单个值
        /// </summary>
        /// <param name="safeSql">得到单个值的sql查询语句</param>
        /// <returns>单个值对象</returns>
        public static object ExecuteScalar(string safeSql)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            return cmd.ExecuteScalar();
        }
        /// <summary>
        /// 执行sql查询，得到单个值，以参数方式传值
        /// </summary>
        /// <param name="safeSql">得到单个值的sql查询语句</param>
        /// <param name="values">参数数组</param>
        /// <returns>单个值对象</returns>
        public static object ExecuteScalar(string safeSql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            cmd.Parameters.AddRange(values);
            return cmd.ExecuteScalar();
        }
        /// <summary>
        /// 执行sql查询，得到一个DataReader对象
        /// </summary>
        /// <param name="sql">sql查询语句</param>
        /// <returns>DataReader对象</returns>
        public static SqlDataReader GetDataReader(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
        /// <summary>
        ///  执行sql查询，得到一个DataReader对象，以参数方式传值
        /// </summary>
        /// <param name="sql">sql查询语句</param>
        /// <param name="values">参数数组</param>
        /// <returns>DataReader对象</returns>
        public static SqlDataReader GetDataReader(string sql, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.Parameters.AddRange(values);
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
        /// <summary>
        /// 执行sql查询，得到一个DataTable对象
        /// </summary>
        /// <param name="sql">sql查询语句</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        /// <summary>
        /// 执行sql查询，得到一个DataTable对象，以参数方式传值
        /// </summary>
        /// <param name="sql">sql查询语句</param>
        /// <param name="values">参数数组</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string sql, params SqlParameter[] values)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, Connection);
            da.SelectCommand.Parameters.AddRange(values);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        /// <summary>
        /// 执行sql查询，得到一个DataSet对象
        /// </summary>
        /// <param name="sql">sql查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, Connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        /// <summary>
        /// 执行sql查询，得到一个DataSet对象，以参数方式传值
        /// </summary>
        /// <param name="sql">sql查询语句</param>
        /// <param name="values">参数数组</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string sql, params SqlParameter[] values)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, Connection);
            da.SelectCommand.Parameters.AddRange(values);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static bool ExecuteTrasaction(string sqlStr)
        {
            bool result = true;
            SqlTransaction tran = null;
            try
            {
                tran = Connection.BeginTransaction("Tran");
                SqlCommand cmd = new SqlCommand(sqlStr, Connection, tran);
                cmd.ExecuteNonQuery();
                tran.Commit();
                result = true;
            }
            catch
            {
                tran.Rollback();
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static bool ExecuteTrasaction(List<string> sqlStr)
        {
            bool result = true;
            SqlTransaction tran = null;
            try
            {
                tran = Connection.BeginTransaction("Tran");
                for (int i = 0; i < sqlStr.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand(sqlStr[i], Connection, tran);
                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
                result = true;
            }
            catch
            {
                tran.Rollback();
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 拆箱获得int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetInt(object obj)
        {
            return (int)obj;
        }
        /// <summary>
        /// 拆箱获得float
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float GetFloat(object obj)
        {
            return (float)obj;
        }
        /// <summary>
        /// 拆箱获得double
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetDouble(object obj)
        {
            return (double)obj;
        }
        /// <summary>
        /// 拆箱获得long
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long GetLong(object obj)
        {
            return (long)obj;
        }
        /// <summary>
        /// 拆箱获得decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal GetDecimal(object obj)
        {
            return (decimal)obj;
        }
        /// <summary>
        /// 拆箱获得bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetBoolean(object obj)
        {
            return (bool)obj;
        }
        /// <summary>
        /// 拆箱获得DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object obj)
        {
            return (DateTime)obj;
        }
        /// <summary>
        /// 拆箱获得string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetString(object obj)
        {
            return obj + "";
        }

        public static DateTime SystemDate()
        {
            string sqlStr = "select getdate() as SysDate";
            DataTable dt = GetDataTable(sqlStr);
            return DateTime.Parse(DateTime.Parse(dt.Rows[0]["SysDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
        }

    }
}
