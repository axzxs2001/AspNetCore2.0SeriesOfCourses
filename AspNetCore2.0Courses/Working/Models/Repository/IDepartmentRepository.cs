using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Working.Models.DataModel;

namespace Working.Models.Repository
{
    /// <summary>
    /// 部门的仓储接口
    /// </summary>
    public interface IDepartmentRepository
    {
     
        /// <summary>
        /// 查询全部部门带父部门
        /// </summary>
        /// <returns></returns>
        List<FullDepartment> GetAllPDepartment();

        /// <summary>
        /// 查询全部部门
        /// </summary>
        /// <returns></returns>
        List<Department> GetAllDepartment();
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="department">部门</param>
        /// <returns></returns>
        bool AddDepartment(Department department);
    }
}
