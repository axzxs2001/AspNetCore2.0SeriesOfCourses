using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Working.Models.DataModel;
using Dapper;

namespace Working.Models.Repository
{
    /// <summary>
    /// 工作的仓储类
    /// </summary>
    public class WorkItemRepository : IWorkItemRepository
    {
        /// <summary>
        /// 数据库对象
        /// </summary>
        IWorkingDB _workingDB;
        public WorkItemRepository(IWorkingDB workingDB)
        {
            _workingDB = workingDB;

        }
        /// <summary>
        /// 查询人员某月工作记录
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public List<WorkItem> GetWorkItemByYearMonth(int year, int month, int userID)
        {
            var beginDT =DateTime.Parse( $"{year}-{month}-01 00:00:00.000");
            var endDT =DateTime.Parse( $"{year}-{month}-{DateTime.DaysInMonth(year, month)} 23:59:59.999");
            var workItems = _workingDB.Query<WorkItem>("select * from workitems where recorddate>=@begindt and recorddate<=@enddt and createuserid=@userid", new { begindt = beginDT, enddt = endDT, userid = userID }).ToList();
            var newWrokItems = new List<WorkItem>();
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {

                var beginDay = DateTime.Parse($"{year}-{month}-{i} 00:00:00.000");
                var endDay = DateTime.Parse($"{year}-{month}-{i} 23:59:59.999");
                var oneDayWorkItem = workItems.SingleOrDefault(s => s.RecordDate >= beginDay && s.RecordDate <= endDay);
                if (oneDayWorkItem == null)
                {
                    newWrokItems.Add(new WorkItem() { RecordDate = beginDay });
                }
                else
                {
                    newWrokItems.Add(oneDayWorkItem);
                }
            }
            return newWrokItems;
        }

        /// <summary>
        /// 添加工作记录
        /// </summary>
        /// <param name="workItem">工作记录</param>
        /// <returns></returns>
        public bool AddWorkItem(WorkItem workItem, int userID)
        {
            var beginDay = DateTime.Parse($"{workItem.RecordDate.ToShortDateString()} 00:00:00.000");
            var endDay = DateTime.Parse($"{workItem.RecordDate.ToShortDateString()} 23:59:59.999");
            var count = _workingDB.Query<WorkItem>("select * from workitems where recorddate>=@begindt and recorddate<=@enddt and createuserid=@userid", new { begindt = beginDay, enddt = endDay, userid = userID }).Count();
            if (count == 0)
            {
                return _workingDB.Execute("insert into workitems(createtime,createuserid,recorddate,workcontent,memos) values(@createtime,@createuserid,@recorddate,@workcontent,@memos)", new { createtime = DateTime.Now, createuserid = userID, recorddate = workItem.RecordDate, workcontent = workItem.WorkContent, memos = workItem.Memos }) > 0;
            }
            else
            {
                return _workingDB.Execute("update  workitems set createtime=@createtime,createuserid=@createuserid,recorddate=@recorddate,workcontent=@workcontent,memos=@memos where id=@id", new { createtime = DateTime.Now, createuserid = userID, recorddate = workItem.RecordDate, workcontent = workItem.WorkContent, memos = workItem.Memos,id=workItem.ID }) > 0;
            }

        }

    }
}
