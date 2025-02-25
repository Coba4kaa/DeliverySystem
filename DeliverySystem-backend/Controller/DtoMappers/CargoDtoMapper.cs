using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Controller.DtoMappers;

public static class CargoDtoMapper
{
    public static CargoDto ToDto(Cargo domain)
    {
        return new CargoDto
        {
            Id = domain.Id,
            CargoOwnerId = domain.CargoOwnerId,
            Weight = domain.Weight,
            Volume = domain.Volume,
            CargoType = domain.CargoType,
            Status = domain.Status,
        };
    }

    public static Cargo ToDomainModel(CargoDto dto)
    {
        return new Cargo
        {
            Id = dto.Id,
            CargoOwnerId = dto.CargoOwnerId,
            Weight = dto.Weight,
            Volume = dto.Volume,
            CargoType = dto.CargoType,
            Status = dto.Status,
        };
    }
}