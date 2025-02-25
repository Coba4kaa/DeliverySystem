using DeliverySystemBackend.Repository.DbModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.DbMappers;

public static class CarrierDbMapper
{
    public static CarrierDbModel ToDbModel(Carrier domain)
    {
      return new CarrierDbModel
        {
            Id = domain.Id,
            UserId = domain.UserId,
            Name = domain.Name,
            CompanyName = domain.CompanyName,
            ContactEmail = domain.ContactEmail,
            ContactPhone = domain.ContactPhone,
            Rating = domain.Rating,
            Transports = domain.Transports.Select(TransportDbMapper.ToDbModel).ToList(),
            Orders = domain.Orders.Select(OrderDbMapper.ToDbModel).ToList(),
        };
    }

    public static Carrier ToDomainModel(CarrierDbModel dbModel)
    {
        return new Carrier
        {
            Id = dbModel.Id,
            UserId = dbModel.UserId,
            Name = dbModel.Name,
            CompanyName = dbModel.CompanyName,
            ContactEmail = dbModel.ContactEmail,
            ContactPhone = dbModel.ContactPhone,
            Rating = dbModel.Rating,
            Transports = dbModel.Transports.Select(TransportDbMapper.ToDomainModel).ToList(),
            Orders = dbModel.Orders.Select(OrderDbMapper.ToDomainModel).ToList()
        };
    }
}