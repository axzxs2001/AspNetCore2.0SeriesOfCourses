using System;


namespace Working.Models.DataModel
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    public class User
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name
        { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID
        { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartmentID
        { get; set; }        
    }
}
