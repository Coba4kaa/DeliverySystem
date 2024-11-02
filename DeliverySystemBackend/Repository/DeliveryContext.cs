using DeliverySystemBackend.Repository.Models;

namespace DeliverySystemBackend.Repository;

using Microsoft.EntityFrameworkCore;

public class DeliveryContext(DbContextOptions<DeliveryContext> options) : DbContext(options)
{
    public DbSet<OrderDbModel>? Orders { get; set; }
}