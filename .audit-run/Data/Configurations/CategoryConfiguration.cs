using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(category => category.Name)
            .IsUnique();

        builder.Property(category => category.Description);
    }
}
