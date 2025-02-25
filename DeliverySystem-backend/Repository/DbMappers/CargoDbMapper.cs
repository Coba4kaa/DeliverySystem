using DeliverySystemBackend.Repository.DbModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.DbMappers;

public static class CargoDbMapper
{
    public static CargoDbModel ToDbModel(Cargo domain)
    {
        return new CargoDbModel
        {
            Id = domain.Id,
            CargoOwnerId = domain.CargoOwnerId,
            Weight = domain.Weight,
            Volume = domain.Volume,
            CargoType = domain.CargoType,
            Status = domain.Status,
        };
    }

    public static Cargo ToDomainModel(CargoDbModel dbModel)
    {
        return new Cargo
        {
            Id = dbModel.Id,
            CargoOwnerId = dbModel.CargoOwnerId,
            Weight = dbModel.Weight,
            Volume = dbModel.Volume,
            CargoType = dbModel.CargoType,
            Status = dbModel.Status,
        };
    }
}