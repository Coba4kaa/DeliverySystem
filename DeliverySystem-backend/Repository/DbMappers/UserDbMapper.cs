using DeliverySystemBackend.Repository.DbModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.DbMappers;

public static class UserDbMapper
{
    public static UserDbModel ToDbModel(User domain)
    {
        return new UserDbModel
        {
            Id = domain.Id,
            Login = domain.Login,
            PasswordHash = domain.PasswordHash,
            Role = domain.Role,
        };
    }

    public static User ToDomainModel(UserDbModel dbModel)
    {
        return new User
        {
            Id = dbModel.Id,
            Login = dbModel.Login,
            PasswordHash = dbModel.PasswordHash,
            Role = dbModel.Role,
        };
    }
}