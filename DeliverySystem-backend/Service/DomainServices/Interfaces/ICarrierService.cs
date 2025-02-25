using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Service.DomainServices.Interfaces;

public interface ICarrierService
{
    public Task<Carrier> RegisterCarrierAsync(Carrier carrier);
    public Task<Carrier?> GetCarrierByIdAsync(long carrierId);
    public Task<Carrier?> GetCarrierByUserIdAsync(long userId);
    public Task<Collection<Carrier>> GetAllCarriersAsync();
    public Task<Carrier?> UpdateCarrierAsync(Carrier carrier);
    public Task<bool> DeleteCarrierAsync(long carrierId);
    public Task<Transport> AddTransportAsync(Transport transport);
    public Task<Transport?> GetTransportByIdAsync(long transportId);
    public Task<Collection<Transport>> GetAllTransportsAsync();
    public Task<Collection<Transport>> GetTransportsByCarrierIdAsync(long carrierId);
    public Task<List<Transport>> GetTransportsByStatusAsync(long carrierId, TransportStatus status);
    public Task<Transport?> UpdateTransportAsync(Transport transport);
    public Task<bool> DeleteTransportAsync(long transportId);
}