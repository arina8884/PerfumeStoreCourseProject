using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class SupplyRequestEditViewModel
{
    [Required(ErrorMessage = "Выберите поставщика.")]
    public int SupplierId { get; set; }

    [StringLength(500)]
    public string? Comment { get; set; }

    public IReadOnlyList<SupplyRequestLineEditViewModel> Items { get; set; } =
        [new SupplyRequestLineEditViewModel()];

    public IReadOnlyList<SupplierItemViewModel> Suppliers { get; set; } =
        Array.Empty<SupplierItemViewModel>();

    public IReadOnlyList<ProductSelectViewModel> Products { get; set; } =
        Array.Empty<ProductSelectViewModel>();
}

public class SupplyRequestLineEditViewModel
{
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0.")]
    public int Quantity { get; set; } = 1;
}

public class ProductSelectViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;
}
