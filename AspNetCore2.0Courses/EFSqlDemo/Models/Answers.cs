using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFSqlDemo.Models
{
    public partial class Answers
    {
        public Answers()
        {
            Scores = new HashSet<Scores>();
        }
     
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string Answer { get; set; }
        public bool? IsAnswer { get; set; }

        public Questions Question { get; set; }
        public ICollection<Scores> Scores { get; set; }
    }
}
