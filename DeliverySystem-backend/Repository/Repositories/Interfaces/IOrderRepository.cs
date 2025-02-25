using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository.Repositories.Interfaces;

public interface IOrderRepository
{
    public Task<Order> CreateOrderAsync(Order order);
    public Task<Order?> GetOrderByIdAsync(long id);
    public Task<List<Order>> GetAllOrdersAsync();
    public Task<List<Order>> GetCargoOwnerOrdersAsync(long cargoOwnerId);
    public Task<List<Order>> GetCarrierOrdersAsync(long carrierId);
    public Task<Order?> UpdateOrderAsync(Order order);
    public Task<bool> DeleteOrderAsync(long id);
}