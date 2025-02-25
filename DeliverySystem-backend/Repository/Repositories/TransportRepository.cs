using System.Collections.ObjectModel;
using DeliverySystemBackend.Repository.DbMappers;
using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace DeliverySystemBackend.Repository.Repositories;

public class TransportRepository(DeliveryDbContext context, ILogger<CarrierRepository> logger) : ITransportRepository
{
    public async Task<Transport> CreateTransportAsync(Transport transport)
    {
        try
        {
            var carrier = await context.Carriers.FindAsync(transport.CarrierId);
            if (carrier == null)
            {
                throw new KeyNotFoundException($"Carrier with ID {transport.CarrierId} not found.");
            }

            var transportDbModel = TransportDbMapper.ToDbModel(transport);

            context.Transports.Add(transportDbModel);
            await context.SaveChangesAsync();

            return TransportDbMapper.ToDomainModel(transportDbModel);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while adding a new transport.");
            throw new Exception("An error occurred while adding the transport. Please try again later.");
        }
    }
    
    public async Task<Transport?> GetTransportByIdAsync(long transportId)
    {
        try
        {
            var transportDbModel = await context.Transports
                .FirstOrDefaultAsync(t => t.Id == transportId);

            if (transportDbModel != null) return TransportDbMapper.ToDomainModel(transportDbModel);
            logger.LogWarning("Transport with ID {TransportId} not found.", transportId);
            return null;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving transport with ID {TransportId}.", transportId);
            throw new Exception("An error occurred while retrieving the transport. Please try again later.");
        }
    }
    
    public async Task<Collection<Transport>> GetTransportsByCarrierIdAsync(long carrierId)
    {
        try
        {
            var transports = new Collection<Transport>(
                (await context.Transports
                    .Where(t => t.CarrierId == carrierId)
                    .ToListAsync())
                .ConvertAll(TransportDbMapper.ToDomainModel)
            );

            return transports;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving transports for carrier with ID {CarrierId}.",
                carrierId);
            throw new Exception(
                $"An error occurred while retrieving the transports for carrier with ID {carrierId}. Please try again later.");
        }
    }
    
    public async Task<Collection<Transport>> GetAllTransportsAsync()
    {
        try
        {
            var transports = new Collection<Transport>(
                (await context.Transports.ToListAsync())
                .ConvertAll(TransportDbMapper.ToDomainModel)
            );

            return transports;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving all transports.");
            throw new Exception("An error occurred while retrieving all transports. Please try again later.");
        }
    }

    public async Task<Transport?> UpdateTransportAsync(Transport transport)
    {
        try
        {
            var existingTransport = await context.Transports.FindAsync(transport.Id);
            
            if (existingTransport == null)
            {
                logger.LogWarning("Transport with ID {TransportId} not found for deletion.", transport.Id);
                return null;
            }

            var updatedTransport = TransportDbMapper.ToDbModel(transport);
            context.Entry(existingTransport).CurrentValues.SetValues(updatedTransport);

            await context.SaveChangesAsync();
            return TransportDbMapper.ToDomainModel(updatedTransport);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while updating the transport with ID {TransportId}.", transport.Id);
            throw new Exception(
                $"An error occurred while updating the transport with ID {transport.Id}. Please try again later.");
        }
    }
    
    public async Task<bool> DeleteTransportAsync(long transportId)
    {
        try
        {
            var transport = await context.Transports.FindAsync(transportId);

            if (transport == null)
            {
                logger.LogWarning("Transport with ID {TransportId} not found for deletion.", transportId);
                return false;
            }

            context.Transports.Remove(transport);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting the transport with ID {TransportId}.", transportId);
            throw new Exception(
                $"An error occurred while deleting the transport with ID {transportId}. Please try again later.");
        }
    }
}