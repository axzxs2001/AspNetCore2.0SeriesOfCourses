using System;
using System.Collections.Generic;

namespace TestProject.Models.DataModels
{
    public partial class Questions
    {
        public Questions()
        {
            Answers = new HashSet<Answers>();
        }

        public int Id { get; set; }
        public int? TestId { get; set; }
        public string No { get; set; }
        public string Question { get; set; }
        public double? FullScore { get; set; }

        public Tests Test { get; set; }
        public ICollection<Answers> Answers { get; set; }
    }
}
