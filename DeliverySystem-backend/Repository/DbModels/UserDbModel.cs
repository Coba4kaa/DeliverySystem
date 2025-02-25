using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.DbModels;

public class UserDbModel
{
    public long Id { get; init; }
    public string Login { get; init; }
    public string PasswordHash { get; init; }
    public UserRole Role { get; init; }
}