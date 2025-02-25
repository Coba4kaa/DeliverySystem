using DeliverySystemBackend.Repository.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliverySystemBackend.Repository.Configurations;

public class CargoOwnerDbModelConfiguration : IEntityTypeConfiguration<CargoOwnerDbModel>
{
    public void Configure(EntityTypeBuilder<CargoOwnerDbModel> builder)
    {
        builder.HasKey(co => co.Id);

        builder.Property(co => co.CompanyName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(co => co.ContactEmail)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(co => co.ContactPhone)
            .IsRequired()
            .HasMaxLength(15);

        builder.HasOne<UserDbModel>()
            .WithOne()
            .HasForeignKey<CargoOwnerDbModel>(co => co.UserId)
            .IsRequired();
    }
}