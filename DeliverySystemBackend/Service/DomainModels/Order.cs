namespace DeliverySystemBackend.Service.DomainModels

{
    public class Order
    {
        public Guid Id { get; init; }
        public string SenderCity { get; init; } = string.Empty;
        public string SenderAddress { get; init; } = string.Empty;
        public string RecipientCity { get; init; } = string.Empty;
        public string RecipientAddress { get; init; } = string.Empty;    
        public double Weight { get; init; }
        public DateTime PickupDate { get; set; }
    }
}
