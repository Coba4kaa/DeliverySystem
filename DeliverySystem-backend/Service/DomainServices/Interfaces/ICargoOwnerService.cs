using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Service.DomainServices.Interfaces;

public interface ICargoOwnerService
{
    public Task<CargoOwner> RegisterCargoOwnerAsync(CargoOwner cargoOwner);
    public Task<CargoOwner?> GetCargoOwnerByIdAsync(long cargoOwnerId);
    public Task<CargoOwner?> GetCargoOwnerByUserIdAsync(long userId);
    public Task<Collection<CargoOwner>> GetAllCargoOwnersAsync();
    public Task<CargoOwner?> UpdateCargoOwnerAsync(CargoOwner cargoOwner);
    public Task<bool> DeleteCargoOwnerAsync(long cargoOwnerId);
    public Task<Cargo> AddCargoAsync(Cargo cargo);
    public Task<Cargo?> GetCargoByIdAsync(long cargoId);
    public Task<Collection<Cargo>> GetAllCargosAsync();
    public Task<Collection<Cargo>> GetCargoOwnerCargosAsync(long cargoOwnerId);
    public Task<Cargo?> UpdateCargoAsync(Cargo cargo);
    public Task<bool> DeleteCargoAsync(long cargoId);
}