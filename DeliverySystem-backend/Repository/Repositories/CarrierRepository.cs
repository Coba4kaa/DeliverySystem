using System.Collections.ObjectModel;
using DeliverySystemBackend.Repository.DbMappers;
using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace DeliverySystemBackend.Repository.Repositories;

public class CarrierRepository(DeliveryDbContext context, ILogger<CarrierRepository> logger) : ICarrierRepository
{
    public async Task<Carrier> CreateCarrierAsync(Carrier carrier)
    {
        try
        {
            var carrierDbModel = CarrierDbMapper.ToDbModel(carrier);
            context.Carriers.Add(carrierDbModel);
            await context.SaveChangesAsync();
            return CarrierDbMapper.ToDomainModel(carrierDbModel);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while creating a new carrier.");
            throw new Exception("An error occurred while creating the carrier. Please try again later.");
        }
    }

    public async Task<Carrier?> GetCarrierByIdAsync(long id)
    {
        try
        {
            var carrierDbModel = await context.Carriers
                .Include(c => c.Orders)
                .Include(c => c.Transports)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carrierDbModel != null) return CarrierDbMapper.ToDomainModel(carrierDbModel);
            logger.LogWarning("Carrier with ID {CarrierId} not found.", id);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving the carrier with ID {CarrierId}.", id);
            throw new Exception(
                $"An error occurred while retrieving the carrier with ID {id}. Please try again later.");
        }
    }

    public async Task<Carrier?> GetCarrierByUserIdAsync(long userId)
    {
        try
        {
            var carrierDbModel = await context.Carriers
                .Include(c => c.Orders)
                .Include(c => c.Transports)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (carrierDbModel != null) return CarrierDbMapper.ToDomainModel(carrierDbModel);
            logger.LogWarning("Carrier with userId {userId} not found.", userId);
            return null;
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving the carrier with userId {userId}.", userId);
            throw new Exception(
                $"An error occurred while retrieving the carrier with userId {userId}. Please try again later.");
        }
    }

    public async Task<Collection<Carrier>> GetAllCarriersAsync()
    {
        try
        {
            var carriers = new Collection<Carrier>(
                (await context.Carriers
                    .Include(c => c.Orders)
                    .Include(c => c.Transports)
                    .ToListAsync())
                .ConvertAll(CarrierDbMapper.ToDomainModel)
            );

            return carriers;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving all carriers.");
            throw new Exception("An error occurred while retrieving the carriers. Please try again later.");
        }
    }

    public async Task<Carrier?> UpdateCarrierAsync(Carrier carrier)
    {
        try
        {
            var existingCarrier = await context.Carriers.FindAsync(carrier.Id);

            var updatedCarrier = CarrierDbMapper.ToDbModel(carrier);
            context.Entry(existingCarrier).CurrentValues.SetValues(updatedCarrier);

            await context.SaveChangesAsync();
            return CarrierDbMapper.ToDomainModel(updatedCarrier);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while updating the carrier with ID {CarrierId}.", carrier.Id);
            throw new Exception(
                $"An error occurred while updating the carrier with ID {carrier.Id}. Please try again later.");
        }
    }

    public async Task<bool> DeleteCarrierAsync(long id)
    {
        try
        {
            var carrier = await context.Carriers.FindAsync(id);

            if (carrier == null)
            {
                logger.LogWarning("Carrier with ID {CarrierId} not found for deletion.", id);
                return false;
            }

            var transports = await context.Transports
                .Where(t => t.CarrierId == id)
                .ToListAsync();

            context.Transports.RemoveRange(transports);
            context.Carriers.Remove(carrier);

            var user = await context.Users.FindAsync(carrier.UserId);
            context.Users.Remove(user);

            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting the carrier with ID {CarrierId}.", id);
            throw new Exception(
                $"An error occurred while deleting the carrier with ID {id}. Please try again later.");
        }
    }
}