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
    /// 工作项仓储测试
    /// </summary>
    [Trait("工作项仓储层", "WorkItemRepository")]
    public class WorkItemRepositoryTest
    {
        /// <summary>
        /// 数据库Mock对象
        /// </summary>
        Mock<IWorkingDB> _dbMock;
        /// <summary>
        /// 工作项仓储对象
        /// </summary>
        IWorkItemRepository _workItemRepository;
        public WorkItemRepositoryTest()
        {
            _dbMock = new Mock<IWorkingDB>();
            _workItemRepository = new WorkItemRepository(_dbMock.Object);
        }
        /// <summary>
        /// 按部门ID查询用户测试
        /// </summary>
        [Fact]
        public void GetWorkItemByYearMonth_Default_Return()
        {
            var list = new List<WorkItem>() { new WorkItem { CreateUserID=1 } };
            _dbMock.Setup(db => db.Query<WorkItem>(It.IsAny<string>(),It.IsAny<object>(),null,true, null, null)).Returns(list);
            var workitems = _workItemRepository.GetWorkItemByYearMonth(2017,10,1);
            Assert.Equal(31,workitems.Count);
        }

    }
}
