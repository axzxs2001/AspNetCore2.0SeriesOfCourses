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
        /// 数据库对象
        /// </summary>
        IWorkingDB _workingDB;
        public RoleRepository(IWorkingDB workingDB)
        {
            _workingDB = workingDB;

        }
        /// <summary>
        /// 本询角色
        /// </summary>
        /// <returns></returns>
        public List<Role> GetRoles()
        {
            return _workingDB.Query<Role>("select * from roles").ToList();
        }
    }
}
