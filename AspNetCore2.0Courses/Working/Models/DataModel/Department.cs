using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Working.Models.DataModel
{
    /// <summary>
    /// 部门实体类
    /// </summary>
    public class Department
    {
        /// <summary>
        /// ID
        /// </summary> 
        public int ID
        { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string DepartmentName
        { get; set; } 
        /// <summary>
        /// 父部门ID
        /// </summary>
        public int PDepartmentID
        { get; set; }
    }
}
