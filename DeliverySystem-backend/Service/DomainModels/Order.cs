namespace DeliverySystemBackend.Service.DomainModels;

public class Order
{
    public long Id { get; init; }
    public long? CargoOwnerId { get; set; }
    public long? CarrierId { get; set; }
    public double Price { get; set; }
    public Address? SenderAddress { get; set; } = new();
    public Address? RecipientAddress { get; set; } = new();
    public int Distance { get; set; }
    public long? CargoId { get; set; }
    public long? TransportId { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Created;
    public DateTime? SentDate { get; init; }
    public DateTime? PlannedPickupDate { get; set; }
    public DateTime? ActualPickupDate { get; set; }
    public bool IsOrderConfirmedByCargoOwner { get; set; } = false;
    public bool IsOrderConfirmedByCarrier { get; set; } = false;
    public bool IsCargoDelivered { get; set; } = false;
}

public enum OrderStatus
{
    Created,
    Pending,
    Confirmed,
    InProgress,
    Delivered,
    Completed,
    Cancelled
}