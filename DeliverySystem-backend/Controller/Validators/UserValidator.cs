using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainServices.Interfaces;
using FluentValidation;

namespace DeliverySystemBackend.Controller.Validators;

public class UserValidator : AbstractValidator<UserDto>
{
    public UserValidator(IUserService userService)
    {
        RuleFor(u => u.Login)
            .NotEmpty().WithMessage("Login is required.")
            .MaximumLength(50).WithMessage("Login must be at most 50 characters long.")
            .MustAsync(async (u, login, _) => await userService.GetUserByLoginAsync(login) is null)
            .WithMessage("User with this login already exists.");

        RuleFor(u => u.PasswordHash)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(u => u.Role)
            .IsInEnum().WithMessage("Invalid role specified.");
    }
}