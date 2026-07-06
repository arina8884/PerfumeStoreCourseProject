using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("Favorites");

        builder.HasKey(favorite => favorite.Id);

        builder.HasIndex(favorite => new { favorite.UserId, favorite.ProductId })
            .IsUnique()
            .HasDatabaseName("UQ_User_Product");

        builder.HasOne(favorite => favorite.User)
            .WithMany(user => user.Favorites)
            .HasForeignKey(favorite => favorite.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Favorites_Users");

        builder.HasOne(favorite => favorite.Product)
            .WithMany(product => product.Favorites)
            .HasForeignKey(favorite => favorite.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Favorites_Products");
    }
}
