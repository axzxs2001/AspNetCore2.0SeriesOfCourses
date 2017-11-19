using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Working.Models.DataModel;
using Dapper;

namespace Working.Models.Repository
{
    /// <summary>
    /// 角色的仓储类
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        /// <summary>
        /// 连接对象
        /// </summary>
        IDbConnection _dbConnection;
        public RoleRepository(IDbConnection dbConnection, string connectionString)
        {
            dbConnection.ConnectionString = connectionString;
            _dbConnection = dbConnection;

        }
        /// <summary>
        /// 本询角色
        /// </summary>
        /// <returns></returns>
        public List<Role> GetRoles()
        {
            return _dbConnection.Query<Role>("select * from roles").ToList();
        }

    }
}
