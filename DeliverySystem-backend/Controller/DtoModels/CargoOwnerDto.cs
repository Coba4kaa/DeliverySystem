namespace DeliverySystemBackend.Controller.DtoModels;

public class CargoOwnerDto
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public string CompanyName { get; init; }
    public string ContactEmail { get; init; }
    public string ContactPhone { get; init; }
    public List<OrderDto> Orders { get; init; }
    public List<CargoDto> Cargos { get; init; }
}