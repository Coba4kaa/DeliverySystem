using DeliverySystemBackend.Repository.DbModels;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.DbMappers;

public static class OrderDbMapper
{
    public static OrderDbModel ToDbModel(Order domain)
    {
        return new OrderDbModel
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

    public static Order ToDomainModel(OrderDbModel dbModel)
    {
        return new Order
        {
            Id = dbModel.Id,
            CargoOwnerId = dbModel.CargoOwnerId,
            CarrierId = dbModel.CarrierId,
            Price = dbModel.Price,
            SenderAddress = dbModel.SenderAddress,
            RecipientAddress = dbModel.RecipientAddress,
            Distance = dbModel.Distance,
            CargoId = dbModel.CargoId,
            TransportId = dbModel.TransportId,
            OrderStatus = dbModel.OrderStatus,
            SentDate = dbModel.SentDate,
            PlannedPickupDate = dbModel.PlannedPickupDate,
            ActualPickupDate = dbModel.ActualPickupDate,
            IsOrderConfirmedByCargoOwner = dbModel.IsOrderConfirmedByCargoOwner,
            IsOrderConfirmedByCarrier = dbModel.IsOrderConfirmedByCarrier,
            IsCargoDelivered = dbModel.IsCargoDelivered
        };
    }
}