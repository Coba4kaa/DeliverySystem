using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Controller.DtoMappers;

public static class TransportDtoMapper
{
    public static TransportDto ToDto(Transport domain)
    {
        return new TransportDto
        {
            Id = domain.Id,
            CarrierId = domain.CarrierId,
            SerialNumber = domain.SerialNumber,
            TransportType = domain.TransportType,
            Volume = domain.Volume,
            LoadCapacity = domain.LoadCapacity,
            AverageTransportationSpeed = domain.AverageTransportationSpeed,
            Status = domain.Status,
            LocationCity = domain.LocationCity
        };
    }

    public static Transport ToDomainModel(TransportDto dto)
    {
        return new Transport
        {
            Id = dto.Id,
            CarrierId = dto.CarrierId,
            SerialNumber = dto.SerialNumber,
            TransportType = dto.TransportType,
            Volume = dto.Volume,
            LoadCapacity = dto.LoadCapacity,
            AverageTransportationSpeed = dto.AverageTransportationSpeed,
            Status = dto.Status,
            LocationCity = dto.LocationCity
        };
    }
}