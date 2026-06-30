namespace PerfumeStore.MVCC.Models;

public class Review
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime ReviewDate { get; set; }

    public User User { get; set; } = null!;

    public Product Product { get; set; } = null!;
}
