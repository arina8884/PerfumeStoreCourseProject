namespace PerfumeStore.MVCC.Models;

public class SupplyRequest
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime RequestDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Comment { get; set; }

    public Supplier Supplier { get; set; } = null!;

    public User CreatedByUser { get; set; } = null!;

    public ICollection<SupplyRequestItem> SupplyRequestItems { get; set; } = new List<SupplyRequestItem>();
}
