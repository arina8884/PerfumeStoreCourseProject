using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;
using System.Diagnostics;

namespace PerfumeStore.MVCC.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;

        public HomeController(
            ILogger<HomeController> logger,
            ICategoryService categoryService,
            IProductService productService,
            IReviewService reviewService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _productService = productService;
            _reviewService = reviewService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                PopularProducts = await _productService.GetPopularProductsAsync(4),
                NewProducts = await _productService.GetNewProductsAsync(4),
                Categories = await _categoryService.GetAllAsync(),
                Brands = await _productService.GetDistinctBrandsAsync(12),
                RecentReviews = await _reviewService.GetRecentReviewsAsync(4)
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("Home/Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
