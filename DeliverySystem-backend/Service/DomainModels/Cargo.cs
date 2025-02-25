namespace DeliverySystemBackend.Service.DomainModels;

public class Cargo
{
    public long Id { get; init; }
    public long CargoOwnerId { get; set; }
    public double Weight { get; set; }
    public double Volume { get; set; }
    public string CargoType { get; set; } = string.Empty;
    public CargoStatus Status { get; set; } = CargoStatus.Registered;
}

public enum CargoStatus
{
    Registered,
    InProcess,
    Delivered
}