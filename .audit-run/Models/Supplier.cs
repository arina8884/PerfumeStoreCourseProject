namespace PerfumeStore.MVCC.Models;

public class Supplier
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public ICollection<SupplyRequest> SupplyRequests { get; set; } = new List<SupplyRequest>();
}
