using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Model;

namespace YZXDMS.Helpers
{
    /// <summary>
	/// MySql 数据库操作类
	/// </summary>
	public class MySqlHelper
    {
        /// <summary>
        /// MysqlConnection
        /// </summary>
        private static MySql.Data.MySqlClient.MySqlConnection MysqlConnection;

        /// <summary>
        /// 获MySql 连接置信息
        /// </summary>
        /// <returns></returns>
        public static MySql.Data.MySqlClient.MySqlConnection GetCon()
        {
            string mysqlConnectionString = "server=192.168.1.133;user id=root;password=root;database=jyjgxt";//System.Configuration.ConfigurationManager.ConnectionStrings["Libor_MySql_QuoteCenter_ConnectionString"].ToString();

            if (MysqlConnection == null)
                using (MysqlConnection = new MySql.Data.MySqlClient.MySqlConnection(mysqlConnectionString)) { };

            if (MysqlConnection.State == System.Data.ConnectionState.Closed)
                MysqlConnection.Open();

            if (MysqlConnection.State == System.Data.ConnectionState.Broken)
            {
                MysqlConnection.Close();
                MysqlConnection.Open();
            }

            return MysqlConnection;
        }


        #region 执行MySQL语句或存储过程,返回受影响的行数
        /// <summary>
        /// 执行MySQL语句或存储过程
        /// </summary>
        /// <param name="type">命令类型</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="pstmt">参数</param>
        /// <returns>执行结果</returns>
        public static int ExecuteNonQuery(CommandType type, String sqlString, MySql.Data.MySqlClient.MySqlParameter[] para)
        {
            try
            {
                using (MySql.Data.MySqlClient.MySqlCommand com = new MySql.Data.MySqlClient.MySqlCommand())
                {
                    com.Connection = GetCon();
                    com.CommandText = @sqlString;
                    com.CommandType = type;
                    if (para != null)
                        com.Parameters.AddRange(para);

                    int val = com.ExecuteNonQuery();
                    com.Parameters.Clear();

                    return val;
                }
            }
            catch (Exception ex)
            {
               // Logger.Error("执行MySQL语句或存储过程,异常！", ex);

                return 0;
            }
            finally
            {
                if (MysqlConnection.State != ConnectionState.Closed)
                    MysqlConnection.Close();
            }
        }


        /// <summary>
        /// 执行带事务的SQL语句或存储过程
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="type">命令类型</param>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="pstmt">参数</param>
        /// <returns>执行结果</returns>
        public static int ExecuteNonQuery(MySql.Data.MySqlClient.MySqlTransaction trans, CommandType type, String sqlString, MySql.Data.MySqlClient.MySqlParameter[] para)
        {
            try
            {
                using (MySql.Data.MySqlClient.MySqlCommand com = new MySql.Data.MySqlClient.MySqlCommand())
                {
                    com.Connection = MysqlConnection;
                    com.CommandText = @sqlString;
                    com.CommandType = type;
                    if (para != null)
                        com.Parameters.AddRange(para);
                    if (trans != null)
                        com.Transaction = trans;

                    int val = com.ExecuteNonQuery();
                    com.Parameters.Clear();

                    return val;
                }
            }
            catch (Exception ex)
            {
                //Logger.Error("执行MySQL语句或存储过程2,异常！", ex);

                return 0;
            }
            finally
            {
                if (MysqlConnection.State != ConnectionState.Closed)
                    MysqlConnection.Close();
            }
        }
        #endregion


        #region 执行SQL语句或存储过程,返回 DataTable
        /// <summary>
        /// 执行SQL语句或存储过程,返回 DataTable
        /// </summary>
        /// <param name="type">命令类型</param>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="pstmt">参数</param>
        /// <returns>执行结果</returns>
        public static DataTable ExecuteReaderToDataTable(CommandType type, String sqlString, MySql.Data.MySqlClient.MySqlParameter[] para = null)
        {
            DataTable dt = new DataTable();
            MySql.Data.MySqlClient.MySqlDataReader dr = null;

            try
            {
                using (MySql.Data.MySqlClient.MySqlCommand com = new MySql.Data.MySqlClient.MySqlCommand())
                {
                    com.Connection = GetCon();
                    com.CommandText = @sqlString;
                    com.CommandType = type;
                    if (para != null)
                        com.Parameters.AddRange(para);

                    using (dr = com.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (dr != null)
                            dt.Load(dr);

                        com.Parameters.Clear();
                    }

                    return dt;
                }
            }
            catch (Exception ex)
            {
                //Logger.Error("执行SQL语句或存储过程,返回 DataTable,异常！", ex);

                return null;
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                    dr.Close();

                if (MysqlConnection.State != ConnectionState.Closed)
                    MysqlConnection.Close();
            }
        }
        #endregion


        #region EFHelper


        //public static void GetList<T>() where T : class
        //{
        //    jyjgxtEntities1 je = new jyjgxtEntities1();
        //    var t = default(T);
        //    je.Entry<T>(t);

        //}

        #endregion

    }
}
