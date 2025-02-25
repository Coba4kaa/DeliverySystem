using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainModels;
using DeliverySystemBackend.Service.DomainServices.Interfaces;
using FluentValidation;

namespace DeliverySystemBackend.Controller.Validators;

public class TransportValidator : AbstractValidator<TransportDto>
{
    private readonly ICarrierService _carrierService;
    private readonly IOrderService _orderService;

    public TransportValidator(ICarrierService carrierService, IOrderService orderService)
    {
        _carrierService = carrierService;
        _orderService = orderService;

        RuleFor(t => t.CarrierId)
            .NotEmpty().WithMessage("CarrierId is required.")
            .MustAsync(async (id, _) => await ValidateCarrierExistsAsync(id))
            .WithMessage("Carrier with the specified ID does not exist.");
        
        RuleFor(t => t.SerialNumber)
            .NotEmpty().WithMessage("Serial number is required.")
            .MaximumLength(100).WithMessage("Serial number cannot exceed 100 characters.");

        RuleFor(t => t.TransportType)
            .NotEmpty().WithMessage("TransportType is required.")
            .MaximumLength(50).WithMessage("TransportType cannot exceed 50 characters.");

        RuleFor(t => t.Volume)
            .GreaterThan(0).WithMessage("Volume must be greater than 0.")
            .LessThanOrEqualTo(10000).WithMessage("Volume cannot exceed 10,000 units.");

        RuleFor(t => t.LoadCapacity)
            .GreaterThan(0).WithMessage("LoadCapacity must be greater than 0.")
            .LessThanOrEqualTo(50000).WithMessage("LoadCapacity cannot exceed 50,000 units.");

        RuleFor(t => t.AverageTransportationSpeed)
            .GreaterThan(0).WithMessage("AverageTransportationSpeed must be greater than 0.")
            .LessThanOrEqualTo(300).WithMessage("AverageTransportationSpeed cannot exceed 300 km/h.");

        RuleFor(t => t.Status)
            .IsInEnum().WithMessage("Invalid status.");

        RuleFor(t => t.LocationCity)
            .NotEmpty().WithMessage("LocationCity is required.")
            .MaximumLength(100).WithMessage("LocationCity cannot exceed 100 characters.");
    }
    
    public async Task<bool> TransportExistsAsync(OrderDto order)
    {
        var existingOrder = await _orderService.GetOrderByIdAsync(order.Id);
        if (existingOrder != null && existingOrder.TransportId != null && existingOrder.TransportId == order.TransportId)
            return true;
        
        var transport = await _carrierService.GetTransportByIdAsync(order.TransportId.Value);
        return transport is not null && transport.Status == TransportStatus.Available;
    }

    public async Task<bool> ValidateCarrierExistsAsync(long carrierId)
    {
        var carrier = await _carrierService.GetCarrierByIdAsync(carrierId);
        return carrier is not null;
    }
    
    public async Task<bool> ValidateTransportOwnershipAsync(long transportId, long carrierId)
    {
        var transport = await _carrierService.GetTransportByIdAsync(transportId);
        return transport != null && transport.CarrierId == carrierId;
    }
}