using System;
using System.Collections.Generic;

namespace TestProject.Models.DataModels
{
    public partial class Tests
    {
        public Tests()
        {
            ClassTests = new HashSet<ClassTests>();
            Questions = new HashSet<Questions>();
        }

        public int Id { get; set; }
        public string TestName { get; set; }
        public int? SubjectId { get; set; }
        public int? TeacherId { get; set; }

        public Subjects Subject { get; set; }
        public Teachers Teacher { get; set; }
        public ICollection<ClassTests> ClassTests { get; set; }
        public ICollection<Questions> Questions { get; set; }
    }
}
