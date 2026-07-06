using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Controllers;

[Route("product")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IFavoriteService _favoriteService;
    private readonly IReviewService _reviewService;

    public ProductController(
        IProductService productService,
        IFavoriteService favoriteService,
        IReviewService reviewService)
    {
        _productService = productService;
        _favoriteService = favoriteService;
        _reviewService = reviewService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetDetailsAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        if (User.IsInRole(UserRoles.Client))
        {
            var userId = User.GetRequiredUserId();
            product.Reviews = await _reviewService.GetProductReviewsAsync(id, userId);
            product.ReviewsCount = product.Reviews.Count;
            product.AverageRating = product.Reviews.Count > 0
                ? product.Reviews.Average(review => review.Rating)
                : null;
            product.IsInFavorites = await _favoriteService.IsFavoriteAsync(userId, id);
            product.CanLeaveReview = !await _reviewService.HasUserReviewAsync(userId, id);
        }
        else
        {
            product.Reviews = await _reviewService.GetProductReviewsAsync(id);
            product.ReviewsCount = product.Reviews.Count;
            product.AverageRating = product.Reviews.Count > 0
                ? product.Reviews.Average(review => review.Rating)
                : null;
        }

        product.SimilarProducts = await _productService.GetSimilarProductsAsync(
            product.Id,
            product.CategoryId,
            product.Brand,
            4);

        return View(product);
    }
}
