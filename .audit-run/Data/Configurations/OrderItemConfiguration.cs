using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", table =>
        {
            table.HasCheckConstraint("CHK_OrderItemQuantity", "Quantity > 0");
            table.HasCheckConstraint("CHK_OrderItemPrice", "Price >= 0");
        });

        builder.HasKey(orderItem => orderItem.Id);

        builder.Property(orderItem => orderItem.Quantity)
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(orderItem => orderItem.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasIndex(orderItem => new { orderItem.OrderId, orderItem.ProductId })
            .IsUnique()
            .HasDatabaseName("UQ_Order_Product");

        builder.HasOne(orderItem => orderItem.Order)
            .WithMany(order => order.OrderItems)
            .HasForeignKey(orderItem => orderItem.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_OrderItems_Orders");

        builder.HasOne(orderItem => orderItem.Product)
            .WithMany(product => product.OrderItems)
            .HasForeignKey(orderItem => orderItem.ProductId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_OrderItems_Products");
    }
}
