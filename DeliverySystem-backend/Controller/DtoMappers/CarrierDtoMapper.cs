using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Controller.DtoMappers;

public static class CarrierDtoMapper
{
    public static CarrierDto ToDto(Carrier domain)
    {
        return new CarrierDto
        {
            Id = domain.Id,
            UserId = domain.UserId,
            Name = domain.Name,
            CompanyName = domain.CompanyName,
            ContactEmail = domain.ContactEmail,
            ContactPhone = domain.ContactPhone,
            Rating = domain.Rating,
            Transports = domain.Transports.Select(TransportDtoMapper.ToDto).ToList(),
            Orders = domain.Orders.Select(OrderDtoMapper.ToDto).ToList(),
        };
    }

    public static Carrier ToDomainModel(CarrierDto dto)
    {
        return new Carrier
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Name = dto.Name,
            CompanyName = dto.CompanyName,
            ContactEmail = dto.ContactEmail,
            ContactPhone = dto.ContactPhone,
            Rating = dto.Rating,
            Transports = dto.Transports.Select(TransportDtoMapper.ToDomainModel).ToList(),
            Orders = dto.Orders.Select(OrderDtoMapper.ToDomainModel).ToList()
        };
    }
}