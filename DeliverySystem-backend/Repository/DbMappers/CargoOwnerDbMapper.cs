using DeliverySystemBackend.Repository.DbModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.DbMappers;

public static class CargoOwnerDbMapper
{
    public static CargoOwnerDbModel ToDbModel(CargoOwner domain)
    {
        return new CargoOwnerDbModel
        {
            Id = domain.Id,
            UserId = domain.UserId,
            CompanyName = domain.CompanyName,
            ContactEmail = domain.ContactEmail,
            ContactPhone = domain.ContactPhone,
            Cargos = domain.Cargos.Select(CargoDbMapper.ToDbModel).ToList(),
            Orders = domain.Orders.Select(OrderDbMapper.ToDbModel).ToList(),
        };
    }

    public static CargoOwner ToDomainModel(CargoOwnerDbModel dbModel)
    {
        return new CargoOwner
        {
            Id = dbModel.Id,
            UserId = dbModel.UserId,
            CompanyName = dbModel.CompanyName,
            ContactEmail = dbModel.ContactEmail,
            ContactPhone = dbModel.ContactPhone,
            Cargos = dbModel.Cargos.Select(CargoDbMapper.ToDomainModel).ToList(),
            Orders = dbModel.Orders.Select(OrderDbMapper.ToDomainModel).ToList()
        };
    }
}