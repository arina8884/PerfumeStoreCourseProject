using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class SupplierEditViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Введите название поставщика.")]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(150)]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }
}

public class SupplierDetailsViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public IReadOnlyList<SupplyRequestSummaryViewModel> SupplyRequests { get; set; } =
        Array.Empty<SupplyRequestSummaryViewModel>();
}
