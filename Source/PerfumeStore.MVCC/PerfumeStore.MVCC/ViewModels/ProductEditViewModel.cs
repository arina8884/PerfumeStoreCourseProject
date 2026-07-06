using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PerfumeStore.MVCC.ViewModels;

public class ProductEditViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Выберите категорию.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Введите название.")]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите бренд.")]
    [StringLength(100)]
    public string Brand { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите цену.")]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Введите объём.")]
    [Range(1, int.MaxValue)]
    public int Volume { get; set; }

    public string? Description { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public IFormFile? ImageFile { get; set; }

    public IReadOnlyList<CategoryViewModel> Categories { get; set; } =
        Array.Empty<CategoryViewModel>();
}
