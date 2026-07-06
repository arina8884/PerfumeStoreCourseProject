namespace PerfumeStore.MVCC.Models;

public class SupplyRequestItem
{
    public int Id { get; set; }

    public int SupplyRequestId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public SupplyRequest SupplyRequest { get; set; } = null!;

    public Product Product { get; set; } = null!;
}
