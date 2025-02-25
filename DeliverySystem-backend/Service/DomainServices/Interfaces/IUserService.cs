using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Service.DomainServices.Interfaces;

public interface IUserService
{
    Task<User> RegisterUserAsync(User user);
    Task<User?> GetUserByIdAsync(long userId);
    Task<User?> GetUserByLoginAsync(string login);
    Task<Collection<User>> GetAllUsersAsync();
    Task<User?> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(long userId);
    public bool VerifyPassword(string password, string hashedPassword);
}