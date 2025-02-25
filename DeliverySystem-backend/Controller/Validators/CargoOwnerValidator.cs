using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainServices.Interfaces;
using FluentValidation;

namespace DeliverySystemBackend.Controller.Validators;

public class CargoOwnerValidator : AbstractValidator<CargoOwnerDto>
{
    private readonly ICargoOwnerService _cargoOwnerService;
    private readonly ICarrierService _carrierService;
    private readonly IUserService _userService;
    
    public CargoOwnerValidator(ICargoOwnerService cargoOwnerService, ICarrierService carrierService, IUserService userService)
    {
        _cargoOwnerService = cargoOwnerService;
        _carrierService = carrierService;
        _userService = userService;
        
        RuleFor(c => c.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty.")
            .MustAsync(async (userId, _) => await UserExistsAsync(userId))
            .WithMessage("The specified UserId does not exist.")
            .MustAsync(async (carrier, userId, _) => await IsUserIdUniqueAsync(userId, carrier.Id))
            .WithMessage("A carrier or cargoOwner with this UserId already exists.");
        
        RuleFor(c => c.CompanyName)
            .NotEmpty().WithMessage("CompanyName is required.")
            .MaximumLength(100).WithMessage("CompanyName cannot exceed 100 characters.");

        RuleFor(c => c.ContactEmail)
            .NotEmpty().WithMessage("ContactEmail is required.")
            .EmailAddress().WithMessage("ContactEmail must be a valid email address.");

        RuleFor(c => c.ContactPhone)
            .NotEmpty().WithMessage("ContactPhone is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("ContactPhone must be a valid phone number.");
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