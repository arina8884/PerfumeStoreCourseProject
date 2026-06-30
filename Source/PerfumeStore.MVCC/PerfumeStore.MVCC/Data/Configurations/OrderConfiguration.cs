using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", table =>
        {
            table.HasCheckConstraint("CHK_OrderTotal", "TotalAmount >= 0");
            table.HasCheckConstraint(
                "CHK_OrderStatus",
                $"Status IN ('{OrderStatuses.New}', '{OrderStatuses.Confirmed}', '{OrderStatuses.Assembling}', '{OrderStatuses.InDelivery}', '{OrderStatuses.OnTheWay}', '{OrderStatuses.Delivered}', '{OrderStatuses.Canceled}')");
        });

        builder.HasKey(order => order.Id);

        builder.Property(order => order.OrderDate)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(order => order.UpdatedAt);

        builder.Property(order => order.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(order => order.DeliveryAddress)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(order => order.PaymentMethod)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(order => order.DeliveryMethod)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(order => order.Status)
            .HasMaxLength(50)
            .HasDefaultValue(OrderStatuses.New)
            .IsRequired();

        builder.HasOne(order => order.User)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Orders_Users");
    }
}
