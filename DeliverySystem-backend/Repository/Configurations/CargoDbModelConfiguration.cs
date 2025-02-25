using DeliverySystemBackend.Repository.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliverySystemBackend.Repository.Configurations
{
    public class CargoDbModelConfiguration : IEntityTypeConfiguration<CargoDbModel>
    {
        public void Configure(EntityTypeBuilder<CargoDbModel> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Weight)
                .IsRequired();

            builder.Property(c => c.Volume)
                .IsRequired();

            builder.Property(c => c.CargoType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Status)
                .IsRequired();

            builder.HasOne<CargoOwnerDbModel>()
                .WithMany(c => c.Cargos)
                .HasForeignKey(c => c.CargoOwnerId);
        }
    }
}