using System.Collections.ObjectModel;
using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainModels;
using DeliverySystemBackend.Service.DomainServices.Interfaces;

namespace DeliverySystemBackend.Service.DomainServices;

public class CarrierService(ICarrierRepository carrierRepository, ITransportRepository transportRepository) : ICarrierService
{
    public async Task<Carrier> RegisterCarrierAsync(Carrier carrier)
    {
        return await carrierRepository.CreateCarrierAsync(carrier);
    }

    public async Task<Carrier?> GetCarrierByIdAsync(long carrierId)
    {
        return await carrierRepository.GetCarrierByIdAsync(carrierId);
    }
    
    public async Task<Carrier?> GetCarrierByUserIdAsync(long userId)
    {
        return await carrierRepository.GetCarrierByUserIdAsync(userId);
    }
    
    public async Task<Collection<Carrier>> GetAllCarriersAsync()
    {
        return await carrierRepository.GetAllCarriersAsync();
    }
    
    public async Task<Carrier?> UpdateCarrierAsync(Carrier carrier)
    {
        return await carrierRepository.UpdateCarrierAsync(carrier);
    }

    public async Task<bool> DeleteCarrierAsync(long carrierId)
    {
        return await carrierRepository.DeleteCarrierAsync(carrierId);
    }
    
    public async Task<Transport> AddTransportAsync(Transport transport)
    {
        return await transportRepository.CreateTransportAsync(transport);;
    }

    public async Task<Transport?> GetTransportByIdAsync(long transportId)
    {
        return await transportRepository.GetTransportByIdAsync(transportId);
    }

    public async Task<Collection<Transport>> GetAllTransportsAsync()
    {
        return await transportRepository.GetAllTransportsAsync();
    }
    
    public async Task<Collection<Transport>> GetTransportsByCarrierIdAsync(long carrierId)
    {
        return await transportRepository.GetTransportsByCarrierIdAsync(carrierId);
    }

    public async Task<List<Transport>> GetTransportsByStatusAsync(long carrierId, TransportStatus status)
    {
        var carrier = await GetCarrierByIdAsync(carrierId);

        return carrier.Transports
            .Where(t => t.Status == status)
            .ToList();
    }

    public async Task<Transport?> UpdateTransportAsync(Transport transport)
    {
        return await transportRepository.UpdateTransportAsync(transport);
    }
    
    public async Task<bool> DeleteTransportAsync(long transportId)
    {
        return await transportRepository.DeleteTransportAsync(transportId);
    }
}
