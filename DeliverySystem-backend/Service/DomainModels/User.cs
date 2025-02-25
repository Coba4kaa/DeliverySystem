namespace DeliverySystemBackend.Service.DomainModels;

public class User
{
    public long Id { get; init; }
    public string Login { get; init; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; init; }
}

public enum UserRole
{
    Admin,
    Carrier,
    CargoOwner
}
