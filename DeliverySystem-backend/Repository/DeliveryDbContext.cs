using DeliverySystemBackend.Repository.Configurations;
using DeliverySystemBackend.Repository.DbModels;

namespace DeliverySystemBackend.Repository;

using Microsoft.EntityFrameworkCore;

public class DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : DbContext(options)
{
    public DbSet<OrderDbModel> Orders { get; set; }
    public DbSet<CargoDbModel> Cargos { get; set; }
    public DbSet<CargoOwnerDbModel> CargoOwners { get; set; }
    public DbSet<CarrierDbModel> Carriers { get; set; }
    public DbSet<TransportDbModel> Transports { get; set; }
    public DbSet<UserDbModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CargoDbModelConfiguration());
        modelBuilder.ApplyConfiguration(new CargoOwnerDbModelConfiguration());
        modelBuilder.ApplyConfiguration(new CarrierDbModelConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDbModelConfiguration());
        modelBuilder.ApplyConfiguration(new TransportDbModelConfiguration());
        modelBuilder.ApplyConfiguration(new UserDbModelConfiguration());
    }
}