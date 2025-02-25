using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Controller.DtoMappers;

public static class UserDtoMapper
{
    public static UserDto ToDto(User domain)
    {
        return new UserDto
        {
            Id = domain.Id,
            Login = domain.Login,
            PasswordHash = domain.PasswordHash,
            Role = domain.Role,
        };
    }

    public static User ToDomainModel(UserDto dto)
    {
        return new User
        {
            Id = dto.Id,
            Login = dto.Login,
            PasswordHash = dto.PasswordHash,
            Role = dto.Role,
        };
    }
}