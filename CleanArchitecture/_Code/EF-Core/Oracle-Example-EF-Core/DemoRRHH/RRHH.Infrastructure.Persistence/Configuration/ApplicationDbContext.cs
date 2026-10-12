using Microsoft.EntityFrameworkCore;
using RRHH.Infrastructure.Persistence.Configuration.Tables;
using RRHH.Infrastructure.Persistence.Entities;


namespace RRHH.Infrastructure.Persistence.Configuration
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employee => Set<Employee>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EmployeeConfiguration());
        }
       
    }
}


