using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeManagement.Domain.Entities;

namespace TimeManagement.Infrastructure.Database.Configurations;

public class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
{
    public void Configure(EntityTypeBuilder<EmployeeRole> builder)
    {
        builder.HasKey(er => new { er.EmployeeId, er.RoleId });

        builder.HasOne(er => er.Employee)
               .WithMany(e => e.EmployeeRoles)
               .HasForeignKey(er => er.EmployeeId);

        builder.HasOne(er => er.Role)
               .WithMany(r => r.EmployeeRoles)
               .HasForeignKey(er => er.RoleId);
    }
}
