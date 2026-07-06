using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class SupplyRequestConfiguration : IEntityTypeConfiguration<SupplyRequest>
{
    public void Configure(EntityTypeBuilder<SupplyRequest> builder)
    {
        builder.ToTable("SupplyRequests", table =>
        {
            table.HasCheckConstraint(
                "CHK_SupplyRequestStatus",
                $"Status IN ('{SupplyRequestStatuses.Pending}', '{SupplyRequestStatuses.Approved}', '{SupplyRequestStatuses.Completed}', '{SupplyRequestStatuses.Rejected}')");
        });

        builder.HasKey(supplyRequest => supplyRequest.Id);

        builder.Property(supplyRequest => supplyRequest.RequestDate)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(supplyRequest => supplyRequest.Status)
            .HasMaxLength(50)
            .HasDefaultValue(SupplyRequestStatuses.Pending)
            .IsRequired();

        builder.Property(supplyRequest => supplyRequest.Comment)
            .HasMaxLength(500);

        builder.HasOne(supplyRequest => supplyRequest.Supplier)
            .WithMany(supplier => supplier.SupplyRequests)
            .HasForeignKey(supplyRequest => supplyRequest.SupplierId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_SupplyRequests_Suppliers");

        builder.HasOne(supplyRequest => supplyRequest.CreatedByUser)
            .WithMany(user => user.SupplyRequests)
            .HasForeignKey(supplyRequest => supplyRequest.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_SupplyRequests_Users");
    }
}
