using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCoreSqlDemo01.Models
{
   
    public partial class StudentClass_V
    {
        public string ClassName { get; set; }


        public string StuNo { get; set; }
        public string Name { get; set; }
        public string CardId { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public int? ClassId { get; set; }

        public Classes Class { get; set; }
        public ICollection<Scores> Scores { get; set; }
    }
}
