using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Working.Models.DataModel;

namespace Working.Models.Repository
{
    /// <summary>
    /// 工作的仓储接口
    /// </summary>
    public interface IWorkItemRepository
    {
        /// <summary>
        /// 查询人员某月工作记录
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        List<WorkItem> GetWorkItemByYearMonth(int year, int month, int userID);


        /// <summary>
        /// 添加工作记录
        /// </summary>
        /// <param name="workItem">工作记录</param>
        /// <returns></returns>
        bool AddWorkItem(WorkItem workItem, int userID);
    }
}
