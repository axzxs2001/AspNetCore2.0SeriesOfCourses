using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Working.Models.DataModel;

namespace Working.Models.Repository
{
    /// <summary>
    /// 部门的仓储类
    /// </summary>
    public class DepartmentRepository : IDepartmentRepository
    {
        /// <summary>
        /// 数据库对象
        /// </summary>
        IWorkingDB _workingDB;
        public DepartmentRepository(IWorkingDB workingDB)
        {
            _workingDB = workingDB;
        }

        /// <summary>
        /// 查询全部部门带父部门
        /// </summary>
        /// <returns></returns>
        public List<FullDepartment> GetAllPDepartment()
        {
            return _workingDB.Query<FullDepartment>("select d.*,pd.departmentname as pdepartmentname from departments as d join departments as pd on d.pdepartmentid=pd.id ").ToList();
        }
        /// <summary>
        /// 查询部门
        /// </summary>
        /// <returns></returns>
        public List<Department> GetAllDepartment()
        {
            return _workingDB.Query<Department>("select * from departments").ToList();
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="department">部门</param>
        /// <returns></returns>
        public bool AddDepartment(Department department)
        {
            return _workingDB.Execute("insert into departments(departmentname,pdepartmentid) values(@departmentname,@pdepartmentid)", new { departmentname = department.DepartmentName, pdepartmentid = department.PDepartmentID }) > 0;
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="department">部门</param>
        /// <returns></returns>
        public bool ModifyDepartment(Department department)
        {
            return _workingDB.Execute("update departments set departmentname=@departmentname,pdepartmentid=@pdepartmentid where id=@id", new { departmentname = department.DepartmentName, pdepartmentid = department.PDepartmentID, id = department.ID }) > 0;
        }
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="departmentID">部门ID</param>
        /// <returns></returns>
        public bool RemoveDepartment(int departmentID)
        {
            return _workingDB.Execute("delete from departments where id=@id", new { id = departmentID }) > 0;
        }

        /// <summary>
        /// 按部门ID查询所有子部门
        /// </summary>
        /// <param name="departmentID">部门ID</param>
        /// <returns></returns>
        public List<Department> GetDeparmentByPID(int departmentID)
        {
            var departments = new List<Department>();
            departments.AddRange(_workingDB.Query<Department>("select * from departments where id=@id", new { id = departmentID }));
            departments.AddRange(GetChildDeaprtment(departmentID));
            return departments;
        }
        /// <summary>
        /// 查询询子部门
        /// </summary>
        /// <param name="departmentID">部门ID</param>
        /// <returns></returns>
        List<Department> GetChildDeaprtment(int departmentID)
        {
            var departments = new List<Department>();
            var childDepartments = _workingDB.Query<Department>("select * from departments where PDepartmentID=@id", new { id = departmentID }).ToList();

            departments.AddRange(childDepartments);
            foreach (var department in childDepartments)
            {
                departments.AddRange(GetChildDeaprtment(department.ID));
            }

            return departments;
        }
    }
}
