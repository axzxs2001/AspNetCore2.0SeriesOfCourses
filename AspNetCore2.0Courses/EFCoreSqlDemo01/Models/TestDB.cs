using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSqlDemo01.Models
{
    class TestDB : TestManageDBContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentClass_V>(entity =>
            {
                entity.HasKey("StuNo");
            });

            modelBuilder.Entity<ABC>(entity =>
            {
                entity.HasKey("a");
            });
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<StudentClass_V> StudentClass_V { get; set; }

        public virtual DbSet<ABC> ABC { get; set; }
    }
}
