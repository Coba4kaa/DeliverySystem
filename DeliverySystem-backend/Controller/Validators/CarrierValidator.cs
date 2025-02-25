using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainServices.Interfaces;
using FluentValidation;

namespace DeliverySystemBackend.Controller.Validators;

public class CarrierValidator : AbstractValidator<CarrierDto>
{
    private readonly ICargoOwnerService _cargoOwnerService;
    private readonly ICarrierService _carrierService;
    private readonly IUserService _userService;

    public CarrierValidator(ICarrierService carrierService, ICargoOwnerService cargoOwnerService, IUserService userService)
    {
        _carrierService = carrierService;
        _cargoOwnerService = cargoOwnerService;
        _userService = userService;

        RuleFor(c => c.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty.")
            .MustAsync(async (userId, _) => await UserExistsAsync(userId))
            .WithMessage("The specified UserId does not exist.")
            .MustAsync(async (carrier, userId, _) => await IsUserIdUniqueAsync(userId, carrier.Id))
            .WithMessage("A carrier or cargoOwner with this UserId already exists.");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Carrier name is required.")
            .MaximumLength(100).WithMessage("Carrier name cannot exceed 100 characters.");

        RuleFor(c => c.CompanyName)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(150).WithMessage("Company name cannot exceed 150 characters.");

        RuleFor(c => c.ContactEmail)
            .NotEmpty().WithMessage("Contact email is required.")
            .EmailAddress().WithMessage("Contact email must be a valid email address.");

        RuleFor(c => c.ContactPhone)
            .NotEmpty().WithMessage("Contact phone is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Contact phone must be a valid phone number.");
    }

    private async Task<bool> UserExistsAsync(long userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        return user is not null;
    }

    private async Task<bool> IsUserIdUniqueAsync(long userId, long carrierId)
    {
        var existingCargoOwner = await _cargoOwnerService.GetCargoOwnerByUserIdAsync(userId);
        if (existingCargoOwner != null && existingCargoOwner.Id != carrierId)
            return false;

        var existingCarrier = await _carrierService.GetCarrierByUserIdAsync(userId);
        if (existingCarrier != null && existingCarrier.Id != carrierId)
            return false;

        return true;
    }
}
