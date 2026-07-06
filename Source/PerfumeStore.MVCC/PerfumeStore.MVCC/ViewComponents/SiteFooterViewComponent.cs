using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.ViewComponents;

public class SiteFooterViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public SiteFooterViewComponent(ICategoryService categoryService, IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new SiteFooterViewModel
        {
            Categories = await _categoryService.GetAllAsync(),
            Brands = await _productService.GetDistinctBrandsAsync(8)
        };

        return View(model);
    }
}
