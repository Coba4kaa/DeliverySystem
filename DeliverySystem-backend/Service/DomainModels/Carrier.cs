namespace DeliverySystemBackend.Service.DomainModels;

public class Carrier
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string ContactEmail { get; init; } = string.Empty;
    public string ContactPhone { get; init; } = string.Empty;
    public double Rating { get; set; }
    public List<Order> Orders { get; set; } = new();
    public List<Transport> Transports { get; set; } = new();
}