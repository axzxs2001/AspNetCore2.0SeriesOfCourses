using Microsoft.EntityFrameworkCore;


namespace EFCoreSqliteDemo.Models.DataModel
{
    /// <summary>
    /// 工作记录DbContext
    /// </summary>
    public class WorkingDbContext : DbContext
    {
        public WorkingDbContext(DbContextOptions<WorkingDbContext> options) : base(options)
        { }
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// 工作记录
        /// </summary>
        public DbSet<WorkItem> WorkItems { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public DbSet<Department> Departments { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> Roles { get; set; }
   
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(m => m.ID);
            builder.Entity<WorkItem>().HasKey(m => m.ID);
            builder.Entity<Department>().HasKey(m => m.ID);
            builder.Entity<Role>().HasKey(m => m.ID);
           
            base.OnModelCreating(builder);
        }
    }
}
