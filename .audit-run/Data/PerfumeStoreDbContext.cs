using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Data;

public class PerfumeStoreDbContext : DbContext
{
    public PerfumeStoreDbContext(DbContextOptions<PerfumeStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Cart> Cart => Set<Cart>();

    public DbSet<CartItem> CartItems => Set<CartItem>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<Favorite> Favorites => Set<Favorite>();

    public DbSet<Review> Reviews => Set<Review>();

    public DbSet<Supplier> Suppliers => Set<Supplier>();

    public DbSet<SupplyRequest> SupplyRequests => Set<SupplyRequest>();

    public DbSet<SupplyRequestItem> SupplyRequestItems => Set<SupplyRequestItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PerfumeStoreDbContext).Assembly);
    }
}
