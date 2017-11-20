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
        /// 连接对象
        /// </summary>
        IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection, string connectionString)
        {
            dbConnection.ConnectionString = connectionString;
            _dbConnection = dbConnection;

        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UserRole Login(string userName, string password)
        {

            var userRole = _dbConnection.Query<UserRole>("select users.*,roles.rolename from users join roles on users.roleid=roles.id where username=@username and password=@password", new { username = userName, password = password }).SingleOrDefault();
            if (userRole == null)
            {
                throw new Exception("用户名或密码错误！");
            }
            else
            {
                return userRole;
            }

        }
        /// <summary>
        /// 按部门查询用户
        /// </summary>
        /// <param name="departmentID">部门ID</param>
        /// <returns></returns>

        public List<User> GetUsersByDepartmentID(int departmentID)
        {
            return _dbConnection.Query<User>("select * from users where departmentid=@departmentid", new { departmentid = departmentID }).ToList();
        }
        /// <summary>
        /// 按ID获取用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public User GetUser(int userID)
        {
            return _dbConnection.Query<User>("select * from users where id=@id", new { id = userID }).SingleOrDefault();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newPassword">新密码</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public bool ModifyPassword(string newPassword, int userID)
        {
            return _dbConnection.Execute("update users set password=@password where id=@id", new { password = newPassword, id = userID }) > 0;
        }
        /// <summary>
        /// 查询全部门
        /// </summary>
        /// <returns></returns>
        public List<UserRole> GetDepartmentUsers(int departmentID)
        {
            return _dbConnection.Query<UserRole>("select users.*,roles.rolename from users join roles on users.roleid=roles.id where users.departmentid=@departmentid", new { departmentid = departmentID }).ToList();

        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            user.Password = user.UserName;
            return _dbConnection.Execute("insert into users(roleid,departmentid,name,username,password) values(@roleid,@departmentid,@name,@username,@password)", new { roleid = user.RoleID, departmentid = user.DepartmentID, name = user.Name, username = user.UserName, password = user.Password, }) > 0;
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public bool ModifyUser(User user)
        {
            return _dbConnection.Execute("update users set roleid=@roleid,departmentid=@departmentid,name=@name,username=@username,password=@password where id=@id", new { roleid = user.RoleID, departmentid = user.DepartmentID, name = user.Name, username = user.UserName, password = user.Password, id = user.ID }) > 0;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public bool RemoveUser(int userID)
        {
            return _dbConnection.Execute("delete from users where id=@id", new { id = userID }) > 0;
        }
    }
}
