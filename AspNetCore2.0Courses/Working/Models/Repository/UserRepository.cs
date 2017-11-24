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
        /// 数据库对象
        /// </summary>
        IWorkingDB _workingDB;

        public UserRepository(IWorkingDB workingDB)
        {
            _workingDB = workingDB;

        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UserRole Login(string userName, string password)
        {

            var userRole = _workingDB.Query<UserRole>("select users.*,roles.rolename from users join roles on users.roleid=roles.id where username=@username and password=@password", new { username = userName, password = password }).SingleOrDefault();
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
            return _workingDB.Query<User>("select * from users where departmentid=@departmentid", new { departmentid = departmentID }).ToList();
        }
        /// <summary>
        /// 按ID获取用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public User GetUser(int userID)
        {
            return _workingDB.Query<User>("select * from users where id=@id", new { id = userID }).SingleOrDefault();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newPassword">新密码</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public bool ModifyPassword(string newPassword, string oldPassword, int userID)
        {
            var user = GetUser(userID);
            if (user!=null&&user.Password == oldPassword)
            {

                return _workingDB.Execute("update users set password=@password where id=@id", new { password = newPassword, id = userID }) > 0;
            }
            else
            {
               throw new Exception($"修改密码:修改密码失败:旧密码不正确");
            }
        }
        /// <summary>
        /// 查询全部门
        /// </summary>
        /// <returns></returns>
        public List<UserRole> GetDepartmentUsers(int departmentID)
        {
            return _workingDB.Query<UserRole>("select users.*,roles.rolename from users join roles on users.roleid=roles.id where users.departmentid=@departmentid", new { departmentid = departmentID }).ToList();

        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            if (user == null)
            {
                throw new Exception("添加的用户不能为Null");
            }
            else
            {
                user.Password = user.UserName;
                var result = _workingDB.Execute("insert into users(roleid,departmentid,name,username,password) values(@roleid,@departmentid,@name,@username,@password)", new { roleid = user.RoleID, departmentid = user.DepartmentID, name = user.Name, username = user.UserName, password = user.Password, });
                return result > 0;
            }
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public bool ModifyUser(User user)
        {
            if (user == null)
            {
                throw new Exception("修改的用户不能为Null");
            }
            else
            {
                return _workingDB.Execute("update users set roleid=@roleid,departmentid=@departmentid,name=@name,username=@username,password=@password where id=@id", new { roleid = user.RoleID, departmentid = user.DepartmentID, name = user.Name, username = user.UserName, password = user.Password, id = user.ID }) > 0;
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public bool RemoveUser(int userID)
        {
            return _workingDB.Execute("delete from users where id=@id", new { id = userID }) > 0;
        }
    }
}
