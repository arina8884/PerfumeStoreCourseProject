namespace PerfumeStore.MVCC.ViewModels;

public class ManagerOrderDetailsViewModel
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public string ClientFullName { get; set; } = string.Empty;

    public string ClientEmail { get; set; } = string.Empty;

    public string? ClientPhone { get; set; }

    public string DeliveryAddress { get; set; } = string.Empty;

    public string DeliveryMethod { get; set; } = string.Empty;

    public string PaymentMethod { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public IReadOnlyList<ManagerOrderItemViewModel> Items { get; set; } =
        Array.Empty<ManagerOrderItemViewModel>();
}

public class ManagerOrderItemViewModel
{
    public string ProductName { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public int Volume { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal LineTotal => Price * Quantity;
}
