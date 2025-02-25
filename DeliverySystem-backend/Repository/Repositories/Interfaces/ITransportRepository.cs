using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.Repositories.Interfaces;

public interface ITransportRepository
{
    public Task<Transport> CreateTransportAsync(Transport transport);
    public Task<Transport?> GetTransportByIdAsync(long transportId);
    public Task<Collection<Transport>> GetTransportsByCarrierIdAsync(long carrierId);
    public Task<Collection<Transport>> GetAllTransportsAsync();
    public Task<Transport?> UpdateTransportAsync(Transport transport);
    public Task<bool> DeleteTransportAsync(long transportId);
}