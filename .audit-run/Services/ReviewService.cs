using Microsoft.EntityFrameworkCore;

using PerfumeStore.MVCC.Data;

using PerfumeStore.MVCC.Models;

using PerfumeStore.MVCC.Services.Interfaces;

using PerfumeStore.MVCC.ViewModels;



namespace PerfumeStore.MVCC.Services;



public class ReviewService : IReviewService

{

    private readonly PerfumeStoreDbContext _context;



    public ReviewService(PerfumeStoreDbContext context)

    {

        _context = context;

    }



    public async Task<ReviewListViewModel> GetUserReviewsAsync(int userId)

    {

        var reviews = await _context.Reviews

            .AsNoTracking()

            .Where(review => review.UserId == userId)

            .OrderByDescending(review => review.ReviewDate)

            .Select(review => new ReviewItemViewModel

            {

                ReviewId = review.Id,

                ProductId = review.ProductId,

                ProductName = review.Product.Name,

                UserFullName = review.User.FullName,

                Rating = review.Rating,

                Comment = review.Comment,

                ReviewDate = review.ReviewDate,

                IsOwnReview = true

            })

            .ToListAsync();



        return new ReviewListViewModel

        {

            Reviews = reviews

        };

    }



    public async Task<IReadOnlyList<ReviewItemViewModel>> GetProductReviewsAsync(

        int productId,

        int? currentUserId = null)

    {

        return await _context.Reviews

            .AsNoTracking()

            .Where(review => review.ProductId == productId)

            .OrderByDescending(review => review.ReviewDate)

            .Select(review => new ReviewItemViewModel

            {

                ReviewId = review.Id,

                ProductId = review.ProductId,

                ProductName = review.Product.Name,

                UserFullName = review.User.FullName,

                Rating = review.Rating,

                Comment = review.Comment,

                ReviewDate = review.ReviewDate,

                IsOwnReview = currentUserId.HasValue && review.UserId == currentUserId.Value

            })

            .ToListAsync();

    }



    public async Task<IReadOnlyList<ReviewItemViewModel>> GetRecentReviewsAsync(int count)

    {

        return await _context.Reviews

            .AsNoTracking()

            .OrderByDescending(review => review.ReviewDate)

            .Take(count)

            .Select(review => new ReviewItemViewModel

            {

                ReviewId = review.Id,

                ProductId = review.ProductId,

                ProductName = review.Product.Name,

                UserFullName = review.User.FullName,

                Rating = review.Rating,

                Comment = review.Comment,

                ReviewDate = review.ReviewDate,

                IsOwnReview = false

            })

            .ToListAsync();

    }



    public async Task<bool> HasUserReviewAsync(int userId, int productId)

    {

        return await _context.Reviews

            .AsNoTracking()

            .AnyAsync(review => review.UserId == userId && review.ProductId == productId);

    }



    public async Task<bool> CreateReviewAsync(int userId, int productId, int rating, string? comment)

    {

        if (rating is < 1 or > 5)

        {

            return false;

        }



        var productExists = await _context.Products

            .AsNoTracking()

            .AnyAsync(product => product.Id == productId && product.IsActive);



        if (!productExists)

        {

            return false;

        }



        if (await HasUserReviewAsync(userId, productId))

        {

            return false;

        }



        var userExists = await _context.Users

            .AsNoTracking()

            .AnyAsync(user => user.Id == userId);



        if (!userExists)

        {

            return false;

        }



        _context.Reviews.Add(new Review

        {

            UserId = userId,

            ProductId = productId,

            Rating = rating,

            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim()

        });



        await _context.SaveChangesAsync();



        return true;

    }



    public async Task<ReviewEditViewModel?> GetReviewForEditAsync(int userId, int reviewId)

    {

        return await _context.Reviews

            .AsNoTracking()

            .Where(review => review.Id == reviewId && review.UserId == userId)

            .Select(review => new ReviewEditViewModel

            {

                ReviewId = review.Id,

                ProductId = review.ProductId,

                ProductName = review.Product.Name,

                Rating = review.Rating,

                Comment = review.Comment

            })

            .FirstOrDefaultAsync();

    }



    public async Task<bool> UpdateReviewAsync(int userId, int reviewId, int rating, string? comment)

    {

        if (rating is < 1 or > 5)

        {

            return false;

        }



        var review = await _context.Reviews

            .FirstOrDefaultAsync(item => item.Id == reviewId && item.UserId == userId);



        if (review is null)

        {

            return false;

        }



        review.Rating = rating;

        review.Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();



        await _context.SaveChangesAsync();



        return true;

    }



    public async Task<bool> DeleteReviewAsync(int userId, int reviewId)

    {

        var review = await _context.Reviews

            .FirstOrDefaultAsync(item => item.Id == reviewId && item.UserId == userId);



        if (review is null)

        {

            return false;

        }



        _context.Reviews.Remove(review);

        await _context.SaveChangesAsync();



        return true;

    }

}


