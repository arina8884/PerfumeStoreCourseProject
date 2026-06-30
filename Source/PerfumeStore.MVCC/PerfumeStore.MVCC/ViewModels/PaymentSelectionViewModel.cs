using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class PaymentSelectionViewModel
{
    public string DeliveryAddress { get; set; } = string.Empty;

    public string DeliveryMethod { get; set; } = string.Empty;

    public IReadOnlyList<string> PaymentMethods { get; set; } = [];

    [Required(ErrorMessage = "Выберите способ оплаты.")]
    public string SelectedPaymentMethod { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }
}
