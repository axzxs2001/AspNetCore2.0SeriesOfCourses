using System;
using System.Collections.Generic;

namespace TestProject.Models.DataModels
{
    public partial class Subjects
    {
        public Subjects()
        {
            Tests = new HashSet<Tests>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Tests> Tests { get; set; }
    }
}
