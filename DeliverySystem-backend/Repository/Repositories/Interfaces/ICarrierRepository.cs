using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.Repositories.Interfaces;

public interface ICarrierRepository
{
    public Task<Carrier> CreateCarrierAsync(Carrier carrier);
    public Task<Carrier?> GetCarrierByIdAsync(long id);
    Task<Carrier?> GetCarrierByUserIdAsync(long userId);
    public Task<Collection<Carrier>> GetAllCarriersAsync();
    public Task<Carrier?> UpdateCarrierAsync(Carrier carrier);
    public Task<bool> DeleteCarrierAsync(long id);
}