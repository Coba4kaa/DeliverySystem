using System.Text.Json.Serialization;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Controller.DtoModels;

public class UserDto
{
    public long Id { get; init; }
    public string Login { get; init; }
    public string PasswordHash { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; init; }
}