using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainModels;
using DeliverySystemBackend.Service.DomainServices.Interfaces;
using FluentValidation;

namespace DeliverySystemBackend.Controller.Validators;

public class CargoValidator : AbstractValidator<CargoDto>
{
    private readonly ICargoOwnerService _cargoOwnerService;
    private readonly IOrderService _orderService;
    
    public CargoValidator(ICargoOwnerService cargoOwnerService, IOrderService orderService)
    {
        _cargoOwnerService = cargoOwnerService;
        _orderService = orderService;
        
        RuleFor(c => c.CargoOwnerId)
            .NotEmpty().WithMessage("CargoOwnerId is required.")
            .MustAsync(async (id, _) => await ValidateCargoOwnerExistsAsync(id))
            .WithMessage("CargoOwner with the specified ID does not exist.");

        RuleFor(c => c.Volume)
            .GreaterThan(0).WithMessage("Volume must be greater than 0.")
            .LessThanOrEqualTo(10000).WithMessage("Weight cannot exceed 10,000 units.");

        RuleFor(c => c.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0.")
            .LessThanOrEqualTo(10000).WithMessage("Weight cannot exceed 10,000 units.");

        RuleFor(c => c.CargoType)
            .NotEmpty().WithMessage("CargoType is required.");
    }
    
    public async Task<bool> CargoExistsAsync(OrderDto order)
    {
        var existingOrder = await _orderService.GetOrderByIdAsync(order.Id);
        if (existingOrder != null && existingOrder.CargoId != null && existingOrder.CargoId == order.CargoId)
            return true;
        
        var cargo = await _cargoOwnerService.GetCargoByIdAsync(order.CargoId.Value);
        return cargo is not null && cargo.Status == CargoStatus.Registered;
    }

    public async Task<bool> ValidateCargoOwnerExistsAsync(long cargoOwnerId)
    {
        var cargoOwner = await _cargoOwnerService.GetCargoOwnerByIdAsync(cargoOwnerId);
        return cargoOwner is not null;
    }
    
    public async Task<bool> ValidateCargoOwnershipAsync(long cargoId, long cargoOwnerId)
    {
        var cargo = await _cargoOwnerService.GetCargoByIdAsync(cargoId);
        return cargo != null && cargo.CargoOwnerId == cargoOwnerId;
    }

}