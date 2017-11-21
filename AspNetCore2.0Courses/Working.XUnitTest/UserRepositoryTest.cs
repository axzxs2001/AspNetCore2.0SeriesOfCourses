using Moq;
using System;
using System.Data;
using Working.Models.Repository;
using Xunit;
using Dapper;
using Working.Models.DataModel;
using System.Collections.Generic;

namespace Working.XUnitTest
{
    /// <summary>
    /// 用户仓储测试
    /// </summary>
    [Trait("仓储层", "UserRepository")]
    public class UserRepositoryTest
    {
        Mock<IWorkingDB> _dbMock;
        UserRepository _userRepository;
        public UserRepositoryTest()
        {
            _dbMock = new Mock<IWorkingDB>();
            _userRepository = new UserRepository(_dbMock.Object);
        }

        #region 登录测试
        /// <summary>
        /// 测试登录正常值
        /// </summary>
        [Fact]
        public void Login_Default_Return()
        {
            var list = new List<UserRole>() {
                new UserRole{ ID=1, Name="桂素伟", DepartmentID=1, Password="gsw", RoleID=1, RoleName="Leader", UserName="gsw" }
            };
            _dbMock.Setup(db => db.Query<UserRole>(It.IsAny<string>(), It.IsAny<object>(), null, true, null, null)).Returns(list);
            var userRole = _userRepository.Login("gsw", "gsw");

            Assert.NotNull(userRole);


        }
        /// <summary>
        /// 测试登录用户名或密码错误
        /// </summary>
        [Fact]
        public void Login_Null_ThrowException()
        {
            var list = new List<UserRole>();
            _dbMock.Setup(db => db.Query<UserRole>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);
            var exc = Assert.Throws<Exception>(() => { _userRepository.Login("gsw", "gsw"); });

            Assert.Contains("用户名或密码错误！", exc.Message);
        }
        /// <summary>
        /// 测试登录用户名或密码错误
        /// </summary>
        [Fact]
        public void Login_Unkonow_ThrowException()
        {
            var list = new List<UserRole>();
            _dbMock.Setup(db => db.Query<UserRole>(It.IsAny<string>(), It.IsAny<object>(), null, true, null, null)).Throws(new Exception("未知"));
            var exc = Assert.Throws<Exception>(() => { _userRepository.Login("gsw", "gsw"); });
            Assert.Contains("未知", exc.Message);
        }
        #endregion


        #region 添加用户测试
        /// <summary>
        /// 测试异常添加
        /// </summary>
        [Fact]
        public void AddUser_NullUser_ThrowException()
        {
            var exception = Assert.Throws<Exception>(() => { _userRepository.AddUser(null); });
            Assert.Contains("添加的用户不能为Null", exception.Message);
        }

        /// <summary>
        /// 测试异常添加
        /// </summary>
        [Fact]
        public void AddUser_Default_ReturnTrue()
        {
            _dbMock.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).Returns(1);
            var result = _userRepository.AddUser(new User { UserName = "test" });
            Assert.True(result);
        }
        #endregion

        #region 修改用户测试
        /// <summary>
        /// 测试异常修改
        /// </summary>
        [Fact]
        public void ModifyUser_NullUser_ThrowException()
        {
            var exception = Assert.Throws<Exception>(() => { _userRepository.ModifyUser(null); });
            Assert.Contains("修改的用户不能为Null", exception.Message);
        }

        /// <summary>
        /// 测试异常修改
        /// </summary>
        [Fact]
        public void ModifyUser_Default_ReturnTrue()
        {
            _dbMock.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).Returns(1);
            var result = _userRepository.ModifyUser(new User { UserName = "test" });
            Assert.True(result);
        }
        #endregion

        #region 修改用户测试     

        /// <summary>
        /// 测试用户修改
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="backResult">返回值</param>
        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 0)]
        public void RemoveUser_Default_ReturnTrue(int userID, int backResult)
        {
            _dbMock.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).Returns(backResult);
            var result = _userRepository.RemoveUser(userID);
            Assert.Equal(userID == 1, result);
        }
        #endregion
    }
}
