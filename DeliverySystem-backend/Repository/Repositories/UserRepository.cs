using DeliverySystemBackend.Repository.DbMappers;
using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace DeliverySystemBackend.Repository.Repositories;

public class UserRepository(DeliveryDbContext context, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            var userDbModel = UserDbMapper.ToDbModel(user);
            context.Users.Add(userDbModel);
            await context.SaveChangesAsync();
            return UserDbMapper.ToDomainModel(userDbModel);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while creating a new user.");
            throw new Exception("An error occurred while creating the user. Please try again later.");
        }
    }

    public async Task<User?> GetUserByIdAsync(long userId)
    {
        try
        {
            var userDbModel = await context.Users.FindAsync(userId);
            return userDbModel != null ? UserDbMapper.ToDomainModel(userDbModel) : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving the user with ID {UserId}.", userId);
            throw new Exception($"An error occurred while retrieving the user with ID {userId}. Please try again later.");
        }
    }

    public async Task<User?> GetUserByLoginAsync(string login)
    {
        try
        {
            var userDbModel = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
            return userDbModel != null ? UserDbMapper.ToDomainModel(userDbModel) : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving the user with login {Login}.", login);
            throw new Exception($"An error occurred while retrieving the user with login {login}. Please try again later.");
        }
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        try
        {
            var existingUser = await context.Users.FindAsync(user.Id);

            if (existingUser == null)
            {
                logger.LogWarning("User with ID {UserId} not found for update.", user.Id);
                throw new KeyNotFoundException($"User with ID {user.Id} not found.");
            }

            var updatedUserDbModel = UserDbMapper.ToDbModel(user);
            context.Entry(existingUser).CurrentValues.SetValues(updatedUserDbModel);
            await context.SaveChangesAsync();

            return UserDbMapper.ToDomainModel(updatedUserDbModel);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while updating the user with ID {UserId}.", user.Id);
            throw new Exception($"An error occurred while updating the user with ID {user.Id}. Please try again later.");
        }
    }

    public async Task<bool> DeleteUserAsync(long userId)
    {
        try
        {
            var user = await context.Users.FindAsync(userId);

            if (user == null)
            {
                logger.LogWarning("User with ID {UserId} not found for deletion.", userId);
                return false;
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting the user with ID {UserId}.", userId);
            throw new Exception($"An error occurred while deleting the user with ID {userId}. Please try again later.");
        }
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            var userDbModels = await context.Users.ToListAsync();
            return userDbModels.ConvertAll(UserDbMapper.ToDomainModel);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving all users.");
            throw new Exception("An error occurred while retrieving the users. Please try again later.");
        }
    }
}
