using Microsoft.EntityFrameworkCore;
using ORMFundamentals.Data.Entities;

namespace ORMFundamentals.Data
{
    public class Context : DbContext
    {
        public Context()
        {

        }

        public Context(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
               optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }
    }
}
