using DeliverySystemBackend.Service.DomainModels;
using FluentValidation;

namespace DeliverySystemBackend.Controller
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.SenderCity)
                .NotEmpty().WithMessage("Город отправителя обязателен.");

            RuleFor(order => order.SenderAddress)
                .NotEmpty().WithMessage("Адрес отправителя обязателен.");

            RuleFor(order => order.RecipientCity)
                .NotEmpty().WithMessage("Город получателя обязателен.");

            RuleFor(order => order.RecipientAddress)
                .NotEmpty().WithMessage("Адрес получателя обязателен.");

            RuleFor(order => order.Weight)
                .GreaterThan(0).WithMessage("Вес груза должен быть положительным.");

            RuleFor(order => order.PickupDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Дата забора груза должна быть в будущем.");
        }
    }
}