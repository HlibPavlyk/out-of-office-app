using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutOfOfficeApp.CoreDomain.Entities;

namespace OutOfOfficeApp.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Employee", NormalizedName = "EMPLOYEE" },
                new IdentityRole { Id = "2", Name = "HRManager", NormalizedName = "HRMANAGER" },
                new IdentityRole { Id = "3", Name = "ProjectManager", NormalizedName = "PROJECTMANAGER" },
                new IdentityRole { Id = "4", Name = "Administrator", NormalizedName = "ADMINISTRATOR" }
            );
        }

    }
}
