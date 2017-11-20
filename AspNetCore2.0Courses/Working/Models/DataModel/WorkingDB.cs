using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Working.Models.DataModel
{
    /// <summary>
    /// working数据库类型 
    /// </summary>
    public class WorkingDB : IWorkingDB
    {
        IDbConnection _dbConnection;
        public WorkingDB(IDbConnection dbConnection, string connectionString)
        {
            _dbConnection = dbConnection;
            _dbConnection.ConnectionString = connectionString;
        }
        /// <summary>
        /// 连接对象
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            return _dbConnection;
        }
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _dbConnection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return _dbConnection.Execute(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
