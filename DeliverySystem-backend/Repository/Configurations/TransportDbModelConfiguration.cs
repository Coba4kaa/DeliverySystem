using DeliverySystemBackend.Repository.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliverySystemBackend.Repository.Configurations;

public class TransportDbModelConfiguration : IEntityTypeConfiguration<TransportDbModel>
{
    public void Configure(EntityTypeBuilder<TransportDbModel> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.SerialNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.TransportType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Volume)
            .IsRequired();

        builder.Property(t => t.LoadCapacity)
            .IsRequired();

        builder.Property(t => t.AverageTransportationSpeed)
            .IsRequired();

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.LocationCity)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne<CarrierDbModel>()
            .WithMany(c => c.Transports)
            .HasForeignKey(t => t.CarrierId);
    }
}