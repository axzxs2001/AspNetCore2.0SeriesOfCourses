using System;
using System.Collections.Generic;

namespace TestProject.Models.DataModels
{
    public partial class Classes
    {
        public Classes()
        {
            ClassTests = new HashSet<ClassTests>();
            Students = new HashSet<Students>();
        }

        public int Id { get; set; }
        public string ClassName { get; set; }
        public string Memo { get; set; }

        public ICollection<ClassTests> ClassTests { get; set; }
        public ICollection<Students> Students { get; set; }
    }
}
