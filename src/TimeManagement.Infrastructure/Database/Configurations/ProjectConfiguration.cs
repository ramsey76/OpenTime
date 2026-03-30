using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeManagement.Domain.Entities;

namespace TimeManagement.Infrastructure.Database.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Code).IsRequired().HasMaxLength(10);
        builder.Property(p => p.Status).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.HasIndex(p => p.Code).IsUnique();
    }
}
