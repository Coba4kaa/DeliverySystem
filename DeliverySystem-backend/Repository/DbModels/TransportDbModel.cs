using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.DbModels;

public class TransportDbModel
{
    public long Id { get; init; }
    public long CarrierId { get; init; }
    public string SerialNumber { get; init; }
    public string TransportType { get; init; }
    public double Volume { get; init; }
    public double LoadCapacity { get; init; }
    public int AverageTransportationSpeed { get; init; }
    public TransportStatus Status { get; init; }
    public string LocationCity { get; init; }
}