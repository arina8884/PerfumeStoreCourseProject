using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Cart");

        builder.HasKey(cart => cart.Id);

        builder.Property(cart => cart.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.HasIndex(cart => cart.UserId)
            .IsUnique();

        builder.HasOne(cart => cart.User)
            .WithOne(user => user.Cart)
            .HasForeignKey<Cart>(cart => cart.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Cart_Users");
    }
}
