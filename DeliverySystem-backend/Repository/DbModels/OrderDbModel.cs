using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.DbModels;

public class OrderDbModel
{
    public long Id { get; init; }
    public long? CargoOwnerId { get; init; }
    public long? CarrierId { get; init; }
    public double Price { get; init; }
    public Address? SenderAddress { get; init; }
    public Address? RecipientAddress { get; init; }
    public int Distance { get; init; }
    public long? CargoId { get; init; }
    public long? TransportId { get; init; }
    public OrderStatus OrderStatus { get; init; }
    public DateTime? SentDate { get; init; }
    public DateTime? PlannedPickupDate { get; init; }
    public DateTime? ActualPickupDate { get; init; }
    public bool IsOrderConfirmedByCargoOwner { get; init; }
    public bool IsOrderConfirmedByCarrier { get; init; }
    public bool IsCargoDelivered { get; init; }
}