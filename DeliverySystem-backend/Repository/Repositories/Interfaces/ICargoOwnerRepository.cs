using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.Repositories.Interfaces;

public interface ICargoOwnerRepository
{
    public Task<CargoOwner> CreateCargoOwnerAsync(CargoOwner cargoOwner);
    public Task<CargoOwner?> GetCargoOwnerByIdAsync(long id);
    public Task<CargoOwner?> GetCargoOwnerByUserIdAsync(long userId);
    public Task<Collection<CargoOwner>> GetAllCargoOwnersAsync();
    public Task<CargoOwner?> UpdateCargoOwnerAsync(CargoOwner cargoOwner);
    public Task<bool> DeleteCargoOwnerAsync(long id);
}