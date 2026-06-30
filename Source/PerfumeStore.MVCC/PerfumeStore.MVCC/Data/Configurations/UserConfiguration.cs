using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", table =>
        {
            table.HasCheckConstraint(
                "CHK_UserRole",
                $"Role IN ('{UserRoles.Client}', '{UserRoles.OrderManager}', '{UserRoles.ContentManager}', '{UserRoles.Admin}')");
        });

        builder.HasKey(user => user.Id);

        builder.Property(user => user.FullName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(user => user.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(user => user.Phone)
            .HasMaxLength(20);

        builder.Property(user => user.Role)
            .HasMaxLength(50)
            .HasDefaultValue(UserRoles.Client)
            .IsRequired();

        builder.Property(user => user.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();
    }
}
