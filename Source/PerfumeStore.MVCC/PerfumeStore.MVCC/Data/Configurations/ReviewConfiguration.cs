using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews", table =>
        {
            table.HasCheckConstraint("CHK_ReviewRating", "Rating BETWEEN 1 AND 5");
        });

        builder.HasKey(review => review.Id);

        builder.Property(review => review.Rating)
            .IsRequired();

        builder.Property(review => review.Comment)
            .HasMaxLength(1000);

        builder.Property(review => review.ReviewDate)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.HasIndex(review => new { review.UserId, review.ProductId })
            .IsUnique()
            .HasDatabaseName("UQ_UserReview");

        builder.HasOne(review => review.User)
            .WithMany(user => user.Reviews)
            .HasForeignKey(review => review.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Reviews_Users");

        builder.HasOne(review => review.Product)
            .WithMany(product => product.Reviews)
            .HasForeignKey(review => review.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Reviews_Products");
    }
}
