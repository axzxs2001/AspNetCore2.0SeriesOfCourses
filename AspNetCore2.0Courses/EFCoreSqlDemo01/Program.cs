using System;
using EFCoreSqlDemo01.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace EFCoreSqlDemo01
{
    public class Program
    {
        public static void Main()
        {

            var db = new TestDB();

            #region 事务
            db.Database.OpenConnection();
            var tran = db.Database.BeginTransaction();
            try
            {
                var cls = new Classes { ClassName = "新班级", Memo = "" };
                db.Classes.Add(cls);
                db.SaveChanges();

                var stu = new Students { Name = "新学生", StuNo = DateTime.Now.ToString("Smmfff"), ClassId = cls.Id, CardId = "111111111111111111111111111111111111111111111111111111111111111111111111111111111111111" };
                db.Students.Add(stu);
                db.SaveChanges();

                tran.Commit();
            }
            catch (Exception exc)
            {
                tran.Rollback();
                Console.WriteLine(exc.Message);
            }

            #endregion

            #region 存储过程
            //var par1 = new SqlParameter("@studentno", "S00001");
            //var par2 = new SqlParameter();
            //par2.Direction = System.Data.ParameterDirection.Output;
            //par2.ParameterName = "@sumscore";
            //par2.SqlDbType = System.Data.SqlDbType.Float;
            //存储过程，出参查询
            //db.Database.ExecuteSqlCommand("exec getscore @studentno,@sumscore output", par1, par2);
            //Console.WriteLine(par2.Value);

            //存储过程，视图查询
            //var list = db.ABC.FromSql("exec getscore @studentno,@sumscore output", par1, par2);

            //foreach (var item in list)
            //{
            //    Console.WriteLine($"A:{item.a} B:{item.b}  C:{item.c}");
            //}
            //Console.WriteLine($"Par2.Value={par2.Value}");
            #endregion

            #region 视图查询
            //foreach(var scv in  db.StudentClass_V)
            //{
            //    Console.WriteLine($"ClsName:{scv.ClassName} StudentName:{scv.Name}");
            //}
            #endregion

            #region 增删改查
            //AddCls(db);

            //RemoveCls(db);

            //ModifyCls(db);

            //QueryAll(db);
            #endregion
        }

        private static void RemoveCls(TestManageDBContext db)
        {
            var cls = db.Classes.SingleOrDefault(s => s.Id == 2);
            if (cls != null)
            {
                db.Classes.Remove(cls);
                db.SaveChanges();
            }
        }

        private static void ModifyCls(TestManageDBContext db)
        {
            var cls = db.Classes.SingleOrDefault(s => s.Id == 2);
            if (cls != null)
            {
                cls.Memo = "二班人很多1111111";
                db.SaveChanges();
            }
        }

        private static void AddCls(TestManageDBContext db)
        {
            var cls = new Classes { ClassName = "班级二", Memo = "第二" };
            db.Classes.Add(cls);
            db.SaveChanges();
        }

        static void QueryAll(TestManageDBContext db)
        {
            foreach (var cls in db.Classes.Where(w => w.Id == 1))
            {
                Console.WriteLine($"ID:{cls.Id} Name:{cls.ClassName} Memo:{cls.Memo}");
            }
        }
    }
}
