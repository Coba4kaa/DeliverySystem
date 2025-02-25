using System.Collections.ObjectModel;
using DeliverySystemBackend.Repository.DbMappers;
using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace DeliverySystemBackend.Repository.Repositories;

public class CargoRepository(DeliveryDbContext context, ILogger<CargoOwnerRepository> logger) : ICargoRepository
{
    public async Task<Cargo> CreateCargoAsync(Cargo cargo)
    {
        try
        {
            var cargoDbModel = CargoDbMapper.ToDbModel(cargo);
            context.Cargos.Add(cargoDbModel);
            await context.SaveChangesAsync();
            return CargoDbMapper.ToDomainModel(cargoDbModel);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while adding a new cargo.");
            throw new Exception($"{ex.Message} An error occurred while adding the cargo. Please try again later.");
        }
    }
    
    public async Task<Cargo?> GetCargoByIdAsync(long cargoId)
    {
        try
        {
            var cargoDbModel = await context.Cargos.FindAsync(cargoId);

            if (cargoDbModel != null) return CargoDbMapper.ToDomainModel(cargoDbModel);
            
            logger.LogWarning("Cargo with ID {CargoId} not found.", cargoId);
            return null;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving the cargo with ID {CargoId}.", cargoId);
            throw new Exception($"An error occurred while retrieving the cargo with ID {cargoId}. Please try again later.");
        }
    }

    public async Task<Collection<Cargo>> GetAllCargosAsync()
    {
        try
        {
            var cargos = new Collection<Cargo>(
                (await context.Cargos.ToListAsync())
                .ConvertAll(CargoDbMapper.ToDomainModel)
            );

            return cargos;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving all cargos.");
            throw new Exception("An error occurred while retrieving all cargos. Please try again later.");
        }
    }
    
    public async Task<Collection<Cargo>> GetCargosByCargoOwnerIdAsync(long cargoOwnerId)
    {
        try
        {
            var cargos = new Collection<Cargo>(
                (await context.Cargos
                    .Where(c => c.CargoOwnerId == cargoOwnerId)
                    .ToListAsync())
                .ConvertAll(CargoDbMapper.ToDomainModel)
            );

            return cargos;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving cargos for cargo owner with ID {CargoOwnerId}.",
                cargoOwnerId);
            throw new Exception(
                $"An error occurred while retrieving the cargos for cargo owner with ID {cargoOwnerId}. Please try again later.");
        }
    }
    
    public async Task<Cargo?> UpdateCargoAsync(Cargo cargo)
    {
        try
        {
            var existingCargo = await context.Cargos.FindAsync(cargo.Id);

            if (existingCargo == null)
            {
                logger.LogWarning("Cargo with ID {CargoId} not found for update.", cargo.Id);
                return null;
            }

            var updatedCargo = CargoDbMapper.ToDbModel(cargo);
            context.Entry(existingCargo).CurrentValues.SetValues(updatedCargo);

            await context.SaveChangesAsync();
            return CargoDbMapper.ToDomainModel(existingCargo);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while updating the cargo with ID {CargoId}.", cargo.Id);
            throw new Exception($"An error occurred while updating the cargo with ID {cargo.Id}. Please try again later.");
        }
    }

    public async Task<bool> DeleteCargoAsync(long cargoId)
    {
        try
        {
            var cargo = await context.Cargos.FindAsync(cargoId);

            if (cargo == null)
            {
                logger.LogWarning("Cargo with ID {CargoId} not found for deletion.", cargoId);
                return false;
            }

            context.Cargos.Remove(cargo);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting the cargo with ID {CargoId}.", cargoId);
            throw new Exception($"An error occurred while deleting the cargo with ID {cargoId}. Please try again later.");
        }
    }
}