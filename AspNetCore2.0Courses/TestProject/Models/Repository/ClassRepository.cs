
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Models.DataModels;

namespace TestProject
{
    /// <summary>
    /// 班级业务类
    /// </summary>
    public class ClassRepository : IClassRepository
    {
        /// <summary>
        /// 数据操作对象
        /// </summary>
        TestManageDBContext _db;

        public ClassRepository(TestManageDBContext db)
        {
            _db = db;
        }
        /// <summary>
        /// 添加班级
        /// </summary>
        /// <param name="cls">班级</param>
        /// <returns></returns>
        public bool AddClass(Classes cls)
        {
            if(cls==null)
            {
                throw new Exception("班级不能为Null");
            }
            _db.Classes.Add(cls);
            var result = _db.SaveChanges();
            return result > 0;
        }
        /// <summary>
        /// 查询全部班级
        /// </summary>
        /// <returns></returns>
        public IList GetClasses()
        {
            return _db.Classes.Select(s=>new {编号=s.Id,班级名称=s.ClassName,备注=s.Memo  }).ToList();
        }
        /// <summary>
        /// 修改班级
        /// </summary>
        /// <param name="cls">班级</param>
        /// <returns></returns>
        public bool ModifyClass(Classes cls)
        {
            var oldCls = _db.Classes.Find(cls.Id);
            if (oldCls == null)
            {
                throw new Exception($"查询不到ID为{cls.Id}的班级");
            }
            else
            {
                oldCls.ClassName = cls.ClassName;
                oldCls.Memo = cls.Memo;
                var result = _db.SaveChanges();
                return result > 0;
            }
        }
        /// <summary>
        /// 删除班级
        /// </summary>
        /// <param name="id">班级ID</param>
        /// <returns></returns>
        public bool RemoveClass(int id)
        {
            var oldCls = _db.Classes.Find(id);
            if (oldCls == null)
            {
                throw new Exception($"查询不到ID为{id}的班级");
            }
            else
            {
                _db.Classes.Remove(oldCls);
                var result = _db.SaveChanges();
                return result > 0;
            }

        }

    }
}
