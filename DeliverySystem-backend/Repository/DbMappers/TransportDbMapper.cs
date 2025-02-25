using DeliverySystemBackend.Repository.DbModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.DbMappers;

public static class TransportDbMapper
{
    public static TransportDbModel ToDbModel(Transport domain)
    {
        return new TransportDbModel
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

    public static Transport ToDomainModel(TransportDbModel dbModel)
    {
        return new Transport
        {
            Id = dbModel.Id,
            CarrierId = dbModel.CarrierId,
            SerialNumber = dbModel.SerialNumber,
            TransportType = dbModel.TransportType,
            Volume = dbModel.Volume,
            LoadCapacity = dbModel.LoadCapacity,
            AverageTransportationSpeed = dbModel.AverageTransportationSpeed,
            Status = dbModel.Status,
            LocationCity = dbModel.LocationCity
        };
    }
}