using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class DeliverySelectionViewModel
{
    public string DeliveryAddress { get; set; } = string.Empty;

    public IReadOnlyList<string> DeliveryMethods { get; set; } = [];

    [Required(ErrorMessage = "Выберите способ доставки.")]
    public string SelectedDeliveryMethod { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }
}
