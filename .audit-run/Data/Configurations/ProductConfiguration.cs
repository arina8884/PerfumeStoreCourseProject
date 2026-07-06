using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", table =>
        {
            table.HasCheckConstraint("CHK_ProductPrice", "Price >= 0");
            table.HasCheckConstraint("CHK_ProductStock", "StockQuantity >= 0");
        });

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(product => product.Brand)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(product => product.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(product => product.Volume)
            .IsRequired();

        builder.Property(product => product.Description);

        builder.Property(product => product.StockQuantity)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(product => product.ImageUrl)
            .HasMaxLength(500);

        builder.Property(product => product.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(product => product.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.HasIndex(product => new { product.Name, product.Brand, product.Volume })
            .IsUnique()
            .HasDatabaseName("UQ_Product");

        builder.HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Products_Categories");
    }
}
