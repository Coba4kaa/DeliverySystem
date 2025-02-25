using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.Repositories.Interfaces;

public interface ICargoRepository
{
    public Task<Cargo> CreateCargoAsync(Cargo cargo);
    public Task<Cargo?> GetCargoByIdAsync(long cargoId);
    public Task<Collection<Cargo>> GetCargosByCargoOwnerIdAsync(long cargoOwnerId);
    public Task<Collection<Cargo>> GetAllCargosAsync();
    public Task<Cargo?> UpdateCargoAsync(Cargo cargo);
    public Task<bool> DeleteCargoAsync(long cargoId);
}