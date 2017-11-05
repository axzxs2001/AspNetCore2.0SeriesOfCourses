using System;
using System.Collections.Generic;

namespace EFCoreSqlDemo.Models.DataModels
{
    public partial class Scores
    {
        public int Id { get; set; }
        public string StudentNo { get; set; }
        public int? AnswerId { get; set; }
        public double? Score { get; set; }

        public Answers Answer { get; set; }
        public Students StudentNoNavigation { get; set; }
    }
}
