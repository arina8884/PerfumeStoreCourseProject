using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(supplier => supplier.Id);

        builder.Property(supplier => supplier.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(supplier => supplier.Email)
            .HasMaxLength(150);

        builder.Property(supplier => supplier.Phone)
            .HasMaxLength(20);
    }
}
