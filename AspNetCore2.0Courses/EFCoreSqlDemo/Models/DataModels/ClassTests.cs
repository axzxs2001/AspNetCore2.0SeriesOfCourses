using System;
using System.Collections.Generic;

namespace EFCoreSqlDemo.Models.DataModels
{
    public partial class ClassTests
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? TestId { get; set; }
        public bool? IsValidate { get; set; }

        public Classes Class { get; set; }
        public Tests Test { get; set; }
    }
}
