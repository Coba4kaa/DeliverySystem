using DeliverySystemBackend.Repository.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliverySystemBackend.Repository.Configurations;

public class OrderDbModelConfiguration : IEntityTypeConfiguration<OrderDbModel>
{
    public void Configure(EntityTypeBuilder<OrderDbModel> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.CargoOwnerId)
            .IsRequired(false);

        builder.Property(o => o.CarrierId)
            .IsRequired(false);

        builder.Property(o => o.Price)
            .IsRequired();

        builder.Property(o => o.Distance)
            .IsRequired();

        builder.Property(o => o.OrderStatus)
            .IsRequired();

        builder.Property(o => o.SentDate)
            .IsRequired();

        builder.Property(o => o.PlannedPickupDate)
            .IsRequired();

        builder.Property(o => o.ActualPickupDate)
            .IsRequired(false);

        builder.Property(o => o.TransportId)
            .IsRequired(false);

        builder.Property(o => o.CargoId)
            .IsRequired(false);

        builder.Property(o => o.IsOrderConfirmedByCargoOwner)
            .IsRequired();

        builder.Property(o => o.IsOrderConfirmedByCarrier)
            .IsRequired();

        builder.Property(o => o.IsCargoDelivered)
            .IsRequired();

        builder.OwnsOne(
            o => o.SenderAddress,
            navigation =>
            {
                navigation.Property(l => l.HouseNumber).HasMaxLength(200);
                navigation.Property(a => a.Street).HasMaxLength(200);
                navigation.Property(a => a.City).HasMaxLength(100);
                navigation.Property(a => a.PostalCode).HasMaxLength(20);
                navigation.Property(a => a.Country).HasMaxLength(50);

                navigation.ToTable("SenderAddresses");
            });

        builder.OwnsOne(
            o => o.RecipientAddress,
            navigation =>
            {
                navigation.Property(l => l.HouseNumber).HasMaxLength(200);
                navigation.Property(a => a.Street).HasMaxLength(200);
                navigation.Property(a => a.City).HasMaxLength(100);
                navigation.Property(a => a.PostalCode).HasMaxLength(20);
                navigation.Property(a => a.Country).HasMaxLength(50);

                navigation.ToTable("RecipientAddresses");
            });

        builder.HasOne<CargoOwnerDbModel>()
            .WithMany()
            .HasForeignKey(o => o.CargoOwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CarrierDbModel>()
            .WithMany()
            .HasForeignKey(o => o.CarrierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CargoDbModel>()
            .WithOne()
            .HasForeignKey<OrderDbModel>(o => o.CargoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<TransportDbModel>()
            .WithMany()
            .HasForeignKey(o => o.TransportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}