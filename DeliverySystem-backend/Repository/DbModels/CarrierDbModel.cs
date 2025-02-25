namespace DeliverySystemBackend.Repository.DbModels;

public class CarrierDbModel
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public string Name { get; init; }
    public string CompanyName { get; init; } 
    public string ContactEmail { get; init; }
    public string ContactPhone { get; init; }
    public double Rating { get; init; }
    public List<OrderDbModel> Orders { get; init; }
    public List<TransportDbModel> Transports { get; init; }
}