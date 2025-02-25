using System.Collections.ObjectModel;
using DeliverySystemBackend.Repository.DbMappers;
using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace DeliverySystemBackend.Repository.Repositories;

public class CargoOwnerRepository(DeliveryDbContext context, ILogger<CargoOwnerRepository> logger) : ICargoOwnerRepository
{
    public async Task<CargoOwner> CreateCargoOwnerAsync(CargoOwner cargoOwner)
    {
        try
        {
            var cargoOwnerDbModel = CargoOwnerDbMapper.ToDbModel(cargoOwner);
            context.CargoOwners.Add(cargoOwnerDbModel);
            await context.SaveChangesAsync();
            return CargoOwnerDbMapper.ToDomainModel(cargoOwnerDbModel);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while creating a new cargo owner.");
            throw new Exception("An error occurred while creating the cargo owner. Please try again later.");
        }
    }
    
    public async Task<CargoOwner?> GetCargoOwnerByIdAsync(long id)
    {
        try
        {
            var cargoOwnerDbModel = await context.CargoOwners
                .Include(co => co.Orders)
                .Include(co => co.Cargos)
                .FirstOrDefaultAsync(co => co.Id == id);

            if (cargoOwnerDbModel != null) return CargoOwnerDbMapper.ToDomainModel(cargoOwnerDbModel);
            logger.LogWarning("Cargo owner with ID {CargoOwnerId} not found.", id);
            return null;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving the cargo owner with ID {CargoOwnerId}.", id);
            throw new Exception(
                $"{ex.Message} An error occurred while retrieving the cargo owner with ID {id}. Please try again later.");
        }
    }
    
    public async Task<CargoOwner?> GetCargoOwnerByUserIdAsync(long userId)
    {
        try
        {
            var cargoOwnerDbModel = await context.CargoOwners
                .Include(co => co.Orders)
                .Include(co => co.Cargos)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cargoOwnerDbModel != null) return CargoOwnerDbMapper.ToDomainModel(cargoOwnerDbModel);
            logger.LogWarning("CargoOwner with userId {userId} not found.", userId);
            return null;
        }
        
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving the cargoOwner with userId {userId}.", userId);
            throw new Exception(
                $"An error occurred while retrieving the cargoOwner with userId {userId}. Please try again later.");
        }
    }

    public async Task<Collection<CargoOwner>> GetAllCargoOwnersAsync()
    {
        try
        {
            var cargoOwners = new Collection<CargoOwner>(
                (await context.CargoOwners
                    .Include(co => co.Orders)
                    .Include(co => co.Cargos)
                    .ToListAsync())
                .ConvertAll(CargoOwnerDbMapper.ToDomainModel)
            );

            return cargoOwners;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving all cargo owners.");
            throw new Exception("An error occurred while retrieving the cargo owners. Please try again later.");
        }
    }

    public async Task<CargoOwner?> UpdateCargoOwnerAsync(CargoOwner cargoOwner)
    {
        try
        {
            var existingCargoOwner = await context.CargoOwners.FindAsync(cargoOwner.Id);

            if (existingCargoOwner == null)
            {
                logger.LogWarning("Cargo owner with ID {CargoOwnerId} not found for update.", cargoOwner.Id);
                return null;
            }

            var updatedCargoOwner = CargoOwnerDbMapper.ToDbModel(cargoOwner);
            context.Entry(existingCargoOwner).CurrentValues.SetValues(updatedCargoOwner);

            await context.SaveChangesAsync();
            return CargoOwnerDbMapper.ToDomainModel(existingCargoOwner);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while updating the cargo owner with ID {CargoOwnerId}.", cargoOwner.Id);
            throw new Exception(
                $"An error occurred while updating the cargo owner with ID {cargoOwner.Id}. Please try again later.");
        }
    }

    public async Task<bool> DeleteCargoOwnerAsync(long id)
    {
        try
        {
            var cargoOwner = await context.CargoOwners.FindAsync(id);

            if (cargoOwner == null)
            {
                logger.LogWarning("Cargo owner with ID {CargoOwnerId} not found for deletion.", id);
                return false;
            }
            
            var cargos = await context.Cargos
                .Where(c => c.CargoOwnerId == id)
                .ToListAsync();

            context.Cargos.RemoveRange(cargos);
            context.CargoOwners.Remove(cargoOwner);
            
            var user = await context.Users.FindAsync(cargoOwner.UserId);
            context.Users.Remove(user);
            
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting the cargo owner with ID {CargoOwnerId}.", id);
            throw new Exception(
                $"An error occurred while deleting the cargo owner with ID {id}. Please try again later.");
        }
    }
}