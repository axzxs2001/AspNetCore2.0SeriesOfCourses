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
        /// 按年月日用户ID查询
        /// </summary>
        [Fact]
        public void GetWorkItemByYearMonth_Default_Return()
        {
            var list = new List<WorkItem>() { new WorkItem { CreateUserID = 1 } };
            _dbMock.Setup(db => db.Query<WorkItem>(It.IsAny<string>(), It.IsAny<object>(), null, true, null, null)).Returns(list);
            var workitems = _workItemRepository.GetWorkItemByYearMonth(2017, 10, 1);
            Assert.Equal(31, workitems.Count);
        }
        /// <summary>
        /// 抛出未知异常测试
        /// </summary>
        [Fact]
        public void GetWorkItemByYearMonth_ThrowException_ReturnException()
        {
            var list = new List<WorkItem>() { new WorkItem { CreateUserID = 1 } };
            _dbMock.Setup(db => db.Query<WorkItem>(It.IsAny<string>(), It.IsAny<object>(), null, true, null, null)).Throws(new Exception("未知异常"));
            var exce = Assert.Throws<Exception>(() => _workItemRepository.GetWorkItemByYearMonth(2017, 10, 1));
            Assert.Equal("未知异常", exce.Message);
        }
        /// <summary>
        /// 添加工作项测试
        /// </summary>
        /// <param name="workItemID">工作项ID</param>
        /// <param name="returnResult">返回值</param>
        [Theory]
        [InlineData(1,1)]
        [InlineData(2,0)]
        public void AddWorkItem_Default_ReturnTrue(int workItemID ,int returnResult)
        {
            var list = new List<WorkItem>() { new WorkItem { CreateUserID = 1 } };
            _dbMock.Setup(db => db.Query<WorkItem>(It.IsAny<string>(), It.IsAny<object>(), null, true, null, null)).Returns(list);



            _dbMock.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>(),null,null,null)).Returns(value:returnResult);

            var result = _workItemRepository.AddWorkItem(new WorkItem { ID = workItemID }, 1);
            Assert.True(result==(workItemID==1));
        }
    }
}
