using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeManagement.Domain.Entities;

namespace TimeManagement.Infrastructure.Database.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Code).IsRequired().HasMaxLength(10);
        builder.HasIndex(d => d.Code).IsUnique();
        builder.HasOne(d => d.ParentDepartment)
            .WithMany(d => d.SubDepartments)
            .HasForeignKey(d => d.ParentDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
