using System.ComponentModel.DataAnnotations;

namespace PerfumeStore.MVCC.ViewModels;

public class CheckoutViewModel
{
    public IReadOnlyList<CartItemViewModel> Items { get; set; } = [];

    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);

    [Required(ErrorMessage = "Введите адрес доставки.")]
    [StringLength(500, ErrorMessage = "Адрес доставки не должен превышать 500 символов.")]
    public string DeliveryAddress { get; set; } = string.Empty;

    public string? DeliveryMethod { get; set; }

    public string? PaymentMethod { get; set; }
}
