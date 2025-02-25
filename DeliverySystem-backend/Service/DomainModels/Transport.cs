namespace DeliverySystemBackend.Service.DomainModels;

public class Transport
{
    public long Id { get; init; }
    public long CarrierId { get; set; }
    public string SerialNumber { get; set; }
    public string TransportType { get; set; } = string.Empty;
    public double Volume { get; set; }
    public double LoadCapacity { get; set; }
    public int AverageTransportationSpeed { get; set; }
    public TransportStatus Status { get; set; } = TransportStatus.Available;
    public string LocationCity { get; set; }
}

public enum TransportStatus
{
    Available,
    InUse,
    UnderMaintenance
}