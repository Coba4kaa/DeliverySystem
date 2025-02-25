using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;
using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainServices.Interfaces;

namespace DeliverySystemBackend.Service.DomainServices;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<User> RegisterUserAsync(User user)
    {
        var existingUser = await userRepository.GetUserByLoginAsync(user.Login);
        if (existingUser != null)
            throw new InvalidOperationException("A user with this login already exists.");
        
        user.PasswordHash = HashPassword(user.PasswordHash);
        
        return await userRepository.CreateUserAsync(user);
    }

    public async Task<User?> GetUserByIdAsync(long userId)
    {
        return await userRepository.GetUserByIdAsync(userId);
    }

    public async Task<User?> GetUserByLoginAsync(string login)
    {
        return await userRepository.GetUserByLoginAsync(login);
    }

    public async Task<Collection<User>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllUsersAsync();
        return new Collection<User>(users);
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        return await userRepository.UpdateUserAsync(user);
    }

    public async Task<bool> DeleteUserAsync(long userId)
    {
        return await userRepository.DeleteUserAsync(userId);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Password verification failed.", ex);
        }
    }
    
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}