using Moq;
using System;
using System.Data;
using Working.Models.Repository;
using Xunit;
using Dapper;
using Working.Models.DataModel;
using System.Collections.Generic;
using DotNetCore.Moq.Dapper;
namespace Working.XUnitTest
{
    /// <summary>
    /// 用户仓储测试
    /// </summary>
    [Trait("仓储层", "UserRepository")]
    public class UserRepositoryTest
    {
        #region 登录测试
        /// <summary>
        /// 测试登录正常值
        /// </summary>
        [Fact]
        public void Login_Default_Return()
        {
            var dbMock = new Mock<IDbConnection>();
            var userRepository = new UserRepository(dbMock.Object, "");

            var list = new List<UserRole>() {
                new UserRole{ ID=1, Name="桂素伟", DepartmentID=1, Password="gsw", RoleID=1, RoleName="Leader", UserName="gsw" }
            };
           dbMock.SetupDapper(db => db.Query<UserRole>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);

            var userRole = userRepository.Login("gsw", "gsw");

            Assert.NotNull(userRole);


        }
        /// <summary>
        /// 测试登录用户名或密码错误
        /// </summary>
        [Fact]
        public void Login_Null_ThrowException()
        {
            var dbMock = new Mock<IDbConnection>();
            var userRepository = new UserRepository(dbMock.Object, "");

            var list = new List<UserRole>();
            dbMock.SetupDapper(db => db.Query<UserRole>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);

            var exc = Assert.Throws<Exception>(() => { userRepository.Login("gsw", "gsw"); });

            Assert.Contains("用户名或密码错误！", exc.Message);
        }
        /// <summary>
        /// 测试登录用户名或密码错误
        /// </summary>
        [Fact]
        public void Login_Unkonow_ThrowException()
        {
            var dbMock = new Mock<IDbConnection>();
            var userRepository = new UserRepository(dbMock.Object, "");

            var list = new List<UserRole>();
            dbMock.SetupGet(db=>db.ConnectionString).Throws(new Exception("未知"));
            var exc = Assert.Throws<Exception>(() => { userRepository.Login("gsw", "gsw"); });
            Assert.Contains("未知", exc.Message);
        }
        #endregion

       
    }
}
