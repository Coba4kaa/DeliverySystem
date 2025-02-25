using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Controller.DtoModels;

public class CargoDto
{
    public long Id { get; init; }
    public long CargoOwnerId { get; init; }
    public double Weight { get; init; }
    public double Volume { get; init; }
    public string CargoType { get; init; }
    public CargoStatus Status { get; init; }
}