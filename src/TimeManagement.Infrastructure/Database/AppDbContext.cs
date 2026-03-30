using Microsoft.EntityFrameworkCore;
using TimeManagement.Domain.Entities;

namespace TimeManagement.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeeRole> EmployeeRoles => Set<EmployeeRole>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectAssignment> ProjectAssignments => Set<ProjectAssignment>();
    public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("timemanagement");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
