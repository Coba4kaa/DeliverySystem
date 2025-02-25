using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Controller.DtoMappers;

public static class OrderDtoMapper
{
    public static OrderDto ToDto(Order domain)
    {
        return new OrderDto
        {
            Id = domain.Id,
            CargoOwnerId = domain.CargoOwnerId,
            CarrierId = domain.CarrierId,
            Price = domain.Price,
            SenderAddress = domain.SenderAddress,
            RecipientAddress = domain.RecipientAddress,
            Distance = domain.Distance,
            CargoId = domain.CargoId,
            TransportId = domain.TransportId,
            OrderStatus = domain.OrderStatus,
            SentDate = domain.SentDate,
            PlannedPickupDate = domain.PlannedPickupDate,
            ActualPickupDate = domain.ActualPickupDate,
            IsOrderConfirmedByCargoOwner = domain.IsOrderConfirmedByCargoOwner,
            IsOrderConfirmedByCarrier = domain.IsOrderConfirmedByCarrier,
            IsCargoDelivered = domain.IsCargoDelivered
        };
    }

    public static Order ToDomainModel(OrderDto dto)
    {
        return new Order
        {
            Id = dto.Id,
            CargoOwnerId = dto.CargoOwnerId,
            CarrierId = dto.CarrierId,
            Price = dto.Price,
            SenderAddress = dto.SenderAddress,
            RecipientAddress = dto.RecipientAddress,
            Distance = dto.Distance,
            CargoId = dto.CargoId,
            TransportId = dto.TransportId,
            OrderStatus = dto.OrderStatus,
            SentDate = dto.SentDate,
            PlannedPickupDate = dto.PlannedPickupDate,
            ActualPickupDate = dto.ActualPickupDate,
            IsOrderConfirmedByCargoOwner = dto.IsOrderConfirmedByCargoOwner,
            IsOrderConfirmedByCarrier = dto.IsOrderConfirmedByCarrier,
            IsCargoDelivered = dto.IsCargoDelivered
        };
    }
}