using System;


namespace Working.Models.DataModel
{
    public class WorkItem
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        { get; set; }
        /// <summary>
        /// 工作记录内容
        /// </summary>
        public string WorkContent
        {
            get; set;
        }
        /// <summary>
        /// 工作日期
        /// </summary>
        public DateTime RecordDate
        {
            get; set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memos
        { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateUserID
        { get; set; }       
    }
}
