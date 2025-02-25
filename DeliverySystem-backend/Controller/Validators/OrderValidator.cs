using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainModels;
using FluentValidation;

namespace DeliverySystemBackend.Controller.Validators;

public class OrderValidator : AbstractValidator<OrderDto>
{
    public OrderValidator(
        TransportValidator transportValidator,
        CargoValidator cargoValidator)
    {
        RuleFor(order => order.CargoOwnerId)
            .GreaterThan(0).WithMessage("Cargo owner ID must be greater than zero.")
            .MustAsync(async (id, _) => !id.HasValue || await cargoValidator.ValidateCargoOwnerExistsAsync(id.Value))
            .WithMessage("Specified Cargo Owner does not exist.");

        RuleFor(order => order.CarrierId)
            .GreaterThan(0).WithMessage("Carrier ID must be greater than zero.")
            .MustAsync(async (id, _) => !id.HasValue || await transportValidator.ValidateCarrierExistsAsync(id.Value))
            .WithMessage("Specified Carrier does not exist when provided.");

        RuleFor(order => order.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative.");

        RuleFor(order => order.Distance)
            .GreaterThan(0).WithMessage("Distance must be greater than zero.");

        RuleFor(order => order.SenderAddress)
            .NotNull().WithMessage("Sender address is required.")
            .SetValidator(new AddressValidator());

        RuleFor(order => order.RecipientAddress)
            .NotNull().WithMessage("Recipient address is required.")
            .SetValidator(new AddressValidator());

        RuleFor(order => order.SentDate)
            .NotEmpty();
        
        RuleFor(order => order.PlannedPickupDate)
            .NotEmpty();

        RuleFor(order => order.CargoId)
            .GreaterThan(0).WithMessage("Cargo ID must be greater than zero.")
            .MustAsync(async (order, id, _) => !id.HasValue || await cargoValidator.CargoExistsAsync(order))
            .WithMessage("Specified Cargo does not exist or does not available.")
            .MustAsync(async (order, id, _) =>
                !id.HasValue ||
                !order.CargoOwnerId.HasValue ||
                await cargoValidator.ValidateCargoOwnershipAsync(id.Value, order.CargoOwnerId.Value))
            .WithMessage("The specified Cargo does not belong to the specified Cargo Owner.");

        RuleFor(order => order.TransportId)
            .GreaterThan(0).WithMessage("Transport ID must be greater than zero.")
            .MustAsync(async (order, id, _) => !id.HasValue || await transportValidator.TransportExistsAsync(order))
            .WithMessage("Specified Transport does not exist or does not available.")
            .MustAsync(async (order, id, _) =>
                !id.HasValue ||
                !order.CarrierId.HasValue ||
                await transportValidator.ValidateTransportOwnershipAsync(id.Value, order.CarrierId.Value))
            .WithMessage("The specified Transport does not belong to the specified Carrier.");
    }
}

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(address => address.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(200).WithMessage("Street cannot exceed 200 characters.");

        RuleFor(address => address.HouseNumber)
            .NotEmpty().WithMessage("House number is required.")
            .MaximumLength(200).WithMessage("House number cannot exceed 200 characters.");

        RuleFor(address => address.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

        RuleFor(address => address.PostalCode)
            .NotEmpty().WithMessage("Postal code is required.")
            .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters.");

        RuleFor(address => address.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(50).WithMessage("Country cannot exceed 50 characters.");
    }
}