using System;


namespace Working.Models.DataModel
{
    /// <summary>
    /// 用户色角实体类
    /// </summary>
    public class UserRole:User
    {        
        /// <summary>
        /// 用户角色
        /// </summary>        
        public string RoleName
        { get; set; }
    }
}
