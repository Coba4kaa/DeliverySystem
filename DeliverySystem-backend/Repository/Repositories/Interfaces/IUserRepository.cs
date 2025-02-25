using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<User> CreateUserAsync(User user);
    public Task<User?> GetUserByIdAsync(long userId);
    public Task<User?> GetUserByLoginAsync(string login);
    public Task<List<User>> GetAllUsersAsync();
    public Task<User?> UpdateUserAsync(User user);
    public Task<bool> DeleteUserAsync(long userId);
}