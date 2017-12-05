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
    /// 角色仓储测试
    /// </summary>
    [Trait("角色仓储层", "RoleRepository")]
    public class RoleRepositoryTest
    {
        /// <summary>
        /// 数据库Mock对象
        /// </summary>
        Mock<IWorkingDB> _dbMock;
        /// <summary>
        /// 角色仓储对象
        /// </summary>
        IRoleRepository _roleRepository;
        public RoleRepositoryTest()
        {
            _dbMock = new Mock<IWorkingDB>();
            _roleRepository = new RoleRepository(_dbMock.Object);
        }
        /// <summary>
        /// 按部门ID查询用户测试
        /// </summary>
        [Fact]
        public void GetRoles_Default_Return()
        {
            var list = new List<Role>() { new Role(),new Role()};
            _dbMock.Setup(db => db.Query<Role>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);
            var roles = _roleRepository.GetRoles();
            Assert.Equal(2,roles.Count);
        }

    }
}
