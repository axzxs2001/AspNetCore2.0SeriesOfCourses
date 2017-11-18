using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Working.Models.DataModel;

namespace Working.Models.Repository
{
    /// <summary>
    /// 用户仓储类
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            var connectionString = string.Format(configuration.GetConnectionString("DefaultConnection"), System.IO.Directory.GetCurrentDirectory());
        
            _connectionString = connectionString;

        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UserRole Login(string userName, string password)
        {
            using (var con = new SqliteConnection())
            {
                var userRole = con.Query<UserRole>("select users.*,roles.rolename from users join roles on users.roleid=roles.id where username=@username and password=@password", new { username = userName, password = password }).SingleOrDefault();
                if (userRole == null)
                {
                    throw new Exception("用户名或密码错误！");
                }
                else
                {
                    return userRole;
                }
            }
        }
    }
}
