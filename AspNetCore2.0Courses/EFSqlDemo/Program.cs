using System;
using EFSqlDemo.Models;
namespace EFSqlDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new Models.TestManageDBContext();
        
            var ans = new Answers() { Answer = "cccc", IsAnswer = true, QuestionId = 1};
            db.Answers.Add(ans);
            db.SaveChanges();


            foreach (var answer in db.Answers)
            {
                Console.WriteLine($"{answer.Id}:{answer.Answer}");
            }

            Console.WriteLine("Hello World!");
        }
    }
}
