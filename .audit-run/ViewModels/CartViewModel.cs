namespace PerfumeStore.MVCC.ViewModels;

public class CartViewModel
{
    public IReadOnlyList<CartItemViewModel> Items { get; set; } = [];

    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);

    public bool IsEmpty => Items.Count == 0;
}
