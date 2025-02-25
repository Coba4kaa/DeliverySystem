namespace DeliverySystemBackend.Repository.DbModels;

public class CargoOwnerDbModel
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public string CompanyName { get; init; }
    public string ContactEmail { get; init; }
    public string ContactPhone { get; init; }
    public List<OrderDbModel> Orders { get; init; }
    public List<CargoDbModel> Cargos { get; init; }
}