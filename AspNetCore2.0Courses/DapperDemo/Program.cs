using System;
using System.Data.SqlClient;
using Dapper;

namespace DapperDemo
{
    public class Program
    {
        public static void Main()
        {
            using (var con = new SqlConnection("server=.;database=testmanagedb;uid=sa;pwd=1;"))
            {
                var list = con.Query<Cls>("select * from classes");
                foreach(var cls in list)
                {
                    Console.WriteLine(cls);
                }
            }
        }
    }

    public class Cls
    {
        public int ID
        { get; set; }
        public string ClassName
        {
            get; set;
        }

        public string Memo
        {
            get; set;
        }

        public override string ToString()
        {
            return $"ID={ID}  ClassName={ClassName}  Memo={Memo}";
        }
    }
}
