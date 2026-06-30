using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class SupplyRequestItemConfiguration : IEntityTypeConfiguration<SupplyRequestItem>
{
    public void Configure(EntityTypeBuilder<SupplyRequestItem> builder)
    {
        builder.ToTable("SupplyRequestItems", table =>
        {
            table.HasCheckConstraint("CHK_SupplyRequestItemQuantity", "Quantity > 0");
        });

        builder.HasKey(supplyRequestItem => supplyRequestItem.Id);

        builder.Property(supplyRequestItem => supplyRequestItem.Quantity)
            .HasDefaultValue(1)
            .IsRequired();

        builder.HasIndex(supplyRequestItem => new { supplyRequestItem.SupplyRequestId, supplyRequestItem.ProductId })
            .IsUnique()
            .HasDatabaseName("UQ_SupplyRequest_Product");

        builder.HasOne(supplyRequestItem => supplyRequestItem.SupplyRequest)
            .WithMany(supplyRequest => supplyRequest.SupplyRequestItems)
            .HasForeignKey(supplyRequestItem => supplyRequestItem.SupplyRequestId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_SupplyRequestItems_Requests");

        builder.HasOne(supplyRequestItem => supplyRequestItem.Product)
            .WithMany(product => product.SupplyRequestItems)
            .HasForeignKey(supplyRequestItem => supplyRequestItem.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_SupplyRequestItems_Products");
    }
}
