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
    /// 部门仓储测试
    /// </summary>
    [Trait("部门仓储层", "RoleRepository")]
    public class DepartmentRepositoryTest
    {
        /// <summary>
        /// 数据库Mock对象
        /// </summary>
        Mock<IWorkingDB> _dbMock;
        /// <summary>
        /// 部门仓储对象
        /// </summary>
        IDepartmentRepository _departmentRepository;
        public DepartmentRepositoryTest()
        {
            _dbMock = new Mock<IWorkingDB>();
            _departmentRepository = new DepartmentRepository(_dbMock.Object);
        }
        /// <summary>
        /// 按部门ID查询有父部门的部门
        /// </summary>
        [Fact]
        public void GetAllPDepartment_Default_Return()
        {
            var list = new List<FullDepartment>() { new FullDepartment()};
            _dbMock.Setup(db => db.Query<FullDepartment>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);
            var departments = _departmentRepository.GetAllPDepartment();
            Assert.Single(departments);
        }

    }
}
