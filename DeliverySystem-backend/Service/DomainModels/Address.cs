namespace DeliverySystemBackend.Service.DomainModels;

public class Address
{
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Country}, {City}, {Street}, {HouseNumber}, {PostalCode}";
    }
}