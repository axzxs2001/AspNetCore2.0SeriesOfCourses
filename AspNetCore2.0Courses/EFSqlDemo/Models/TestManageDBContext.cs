using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFSqlDemo.Models
{
    public partial class TestManageDBContext : DbContext
    {
        public virtual DbSet<Answers> Answers { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<ClassTests> ClassTests { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Scores> Scores { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Subjects> Subjects { get; set; }
        public virtual DbSet<Teachers> Teachers { get; set; }
        public virtual DbSet<Tests> Tests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database= TestManageDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answers>(entity =>
            {
                entity.HasIndex(e => e.QuestionId)
                    .HasName("IX_QuestionID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Answer)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_dbo.Answers_dbo.Questions_QuestionID");
            });

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Memo).HasColumnType("text");
            });

            modelBuilder.Entity<ClassTests>(entity =>
            {
                entity.HasIndex(e => e.ClassId)
                    .HasName("IX_ClassID");

                entity.HasIndex(e => e.TestId)
                    .HasName("IX_TestID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassTests)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_dbo.ClassTests_dbo.Classes_ClassID");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.ClassTests)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK_dbo.ClassTests_dbo.Tests_TestID");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey });

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Questions>(entity =>
            {
                entity.HasIndex(e => e.TestId)
                    .HasName("IX_TestID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.No)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Question)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK_dbo.Questions_dbo.Tests_TestID");
            });

            modelBuilder.Entity<Scores>(entity =>
            {
                entity.HasIndex(e => e.AnswerId)
                    .HasName("IX_AnswerID");

                entity.HasIndex(e => e.StudentNo)
                    .HasName("IX_StudentNo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnswerId).HasColumnName("AnswerID");

                entity.Property(e => e.StudentNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK_dbo.Scores_dbo.Answers_AnswerID");

                entity.HasOne(d => d.StudentNoNavigation)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.StudentNo)
                    .HasConstraintName("FK_dbo.Scores_dbo.Students_StudentNo");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.StuNo);

                entity.HasIndex(e => e.ClassId)
                    .HasName("IX_ClassID");

                entity.Property(e => e.StuNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.CardId)
                    .HasColumnName("CardID")
                    .HasMaxLength(18)
                    .IsUnicode(false);

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sex)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_dbo.Students_dbo.Classes_ClassID");
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Teachers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TeaacherNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tests>(entity =>
            {
                entity.HasIndex(e => e.SubjectId)
                    .HasName("IX_SubjectID");

                entity.HasIndex(e => e.TeacherId)
                    .HasName("IX_TeacherID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

                entity.Property(e => e.TestName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Tests)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_dbo.Tests_dbo.Subjects_SubjectID");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Tests)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK_dbo.Tests_dbo.Teachers_TeacherID");
            });
        }
    }
}
