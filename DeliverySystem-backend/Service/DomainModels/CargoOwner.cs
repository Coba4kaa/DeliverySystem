namespace DeliverySystemBackend.Service.DomainModels;

public class CargoOwner
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public string CompanyName { get; init; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public List<Order> Orders { get; set; } = new();
    public List<Cargo> Cargos { get; set; } = new();
}