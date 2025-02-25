using DeliverySystemBackend.Repository.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliverySystemBackend.Repository.Configurations;

public class CarrierDbModelConfiguration : IEntityTypeConfiguration<CarrierDbModel>
{
    public void Configure(EntityTypeBuilder<CarrierDbModel> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.CompanyName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.ContactEmail)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ContactPhone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(c => c.Rating)
            .HasDefaultValue(0);

        builder.HasOne<UserDbModel>()
            .WithOne()
            .HasForeignKey<CarrierDbModel>(c => c.UserId)
            .IsRequired();
    }
}