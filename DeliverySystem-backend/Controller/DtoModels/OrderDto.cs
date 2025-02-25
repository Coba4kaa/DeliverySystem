using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Controller.DtoModels;

public class OrderDto
{
    public long Id { get; init; }
    public long? CargoOwnerId { get; set; } = null;
    public long? CarrierId { get; set; } = null;
    public double Price { get; init; }
    public Address? SenderAddress { get; init; }
    public Address? RecipientAddress { get; init; }
    public int Distance { get; init; }
    public long? CargoId { get; set; } = null;
    public long? TransportId { get; set; } = null;
    public OrderStatus OrderStatus { get; init; }
    public DateTime? SentDate { get; init; }
    public DateTime? PlannedPickupDate { get; init; }
    public DateTime? ActualPickupDate { get; init; }
    public bool IsOrderConfirmedByCargoOwner { get; init; }
    public bool IsOrderConfirmedByCarrier { get; init; }
    public bool IsCargoDelivered { get; init; }
}