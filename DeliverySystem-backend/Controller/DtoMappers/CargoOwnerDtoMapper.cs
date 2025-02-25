using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Controller.DtoMappers;

public static class CargoOwnerDtoMapper
{
    public static CargoOwnerDto ToDto(CargoOwner domain)
    {
        return new CargoOwnerDto
        {
            Id = domain.Id,
            UserId = domain.UserId,
            CompanyName = domain.CompanyName,
            ContactEmail = domain.ContactEmail,
            ContactPhone = domain.ContactPhone,
            Cargos = domain.Cargos.Select(CargoDtoMapper.ToDto).ToList(),
            Orders = domain.Orders.Select(OrderDtoMapper.ToDto).ToList()
        };
    }

    public static CargoOwner ToDomainModel(CargoOwnerDto dto)
    {
        return new CargoOwner
        {
            Id = dto.Id,
            UserId = dto.UserId,
            CompanyName = dto.CompanyName,
            ContactEmail = dto.ContactEmail,
            ContactPhone = dto.ContactPhone,
            Cargos = dto.Cargos.Select(CargoDtoMapper.ToDomainModel).ToList(),
            Orders = dto.Orders.Select(OrderDtoMapper.ToDomainModel).ToList()
        };
    }
}