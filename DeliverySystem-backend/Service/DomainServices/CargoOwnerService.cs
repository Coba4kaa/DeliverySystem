using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;
using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainServices.Interfaces;

namespace DeliverySystemBackend.Service.DomainServices;

public class CargoOwnerService(ICargoOwnerRepository cargoOwnerRepository, ICargoRepository cargoRepository)
    : ICargoOwnerService
{
    public async Task<CargoOwner> RegisterCargoOwnerAsync(CargoOwner cargoOwner)
    {
        return await cargoOwnerRepository.CreateCargoOwnerAsync(cargoOwner);
    }

    public async Task<CargoOwner?> GetCargoOwnerByIdAsync(long cargoOwnerId)
    {
        return await cargoOwnerRepository.GetCargoOwnerByIdAsync(cargoOwnerId);
    }

    public async Task<CargoOwner?> GetCargoOwnerByUserIdAsync(long userId)
    {
        return await cargoOwnerRepository.GetCargoOwnerByUserIdAsync(userId);
    }

    public async Task<Collection<CargoOwner>> GetAllCargoOwnersAsync()
    {
        return await cargoOwnerRepository.GetAllCargoOwnersAsync();
    }

    public async Task<CargoOwner?> UpdateCargoOwnerAsync(CargoOwner cargoOwner)
    {
        return await cargoOwnerRepository.UpdateCargoOwnerAsync(cargoOwner);
    }

    public async Task<bool> DeleteCargoOwnerAsync(long cargoOwnerId)
    {
        return await cargoOwnerRepository.DeleteCargoOwnerAsync(cargoOwnerId);
    }
    
    public async Task<Cargo> AddCargoAsync(Cargo cargo)
    {
        return await cargoRepository.CreateCargoAsync(cargo);
    }

    public async Task<Cargo?> GetCargoByIdAsync(long cargoId)
    {
        return await cargoRepository.GetCargoByIdAsync(cargoId);
    }

    public async Task<Collection<Cargo>> GetAllCargosAsync()
    {
        return await cargoRepository.GetAllCargosAsync();
    }

    public async Task<Cargo?> UpdateCargoAsync(Cargo cargo)
    {
        return await cargoRepository.UpdateCargoAsync(cargo);
    }

    public async Task<bool> DeleteCargoAsync(long cargoId)
    {
        return await cargoRepository.DeleteCargoAsync(cargoId);
    }
    
    public async Task<Collection<Cargo>> GetCargoOwnerCargosAsync(long cargoOwnerId)
    {
        return await cargoRepository.GetCargosByCargoOwnerIdAsync(cargoOwnerId);
    }
}