using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems", table =>
        {
            table.HasCheckConstraint("CHK_CartItemQuantity", "Quantity > 0");
        });

        builder.HasKey(cartItem => cartItem.Id);

        builder.Property(cartItem => cartItem.Quantity)
            .HasDefaultValue(1)
            .IsRequired();

        builder.HasIndex(cartItem => new { cartItem.CartId, cartItem.ProductId })
            .IsUnique()
            .HasDatabaseName("UQ_Cart_Product");

        builder.HasOne(cartItem => cartItem.Cart)
            .WithMany(cart => cart.CartItems)
            .HasForeignKey(cartItem => cartItem.CartId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CartItems_Cart");

        builder.HasOne(cartItem => cartItem.Product)
            .WithMany(product => product.CartItems)
            .HasForeignKey(cartItem => cartItem.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CartItems_Products");
    }
}
