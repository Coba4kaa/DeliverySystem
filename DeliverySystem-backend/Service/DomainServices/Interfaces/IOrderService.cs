using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Service.DomainServices.Interfaces
{
    public interface IOrderService
    {
        public Task<Order> CreateOrderAsync(Order order);
        public Task<Order?> GetOrderByIdAsync(long orderId);
        public Task<List<Order>> GetAllOrdersAsync();
        public Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
        public Task<List<Order>> GetOrdersByCargoOwnerAsync(long cargoOwnerId);
        public Task<List<Order>> GetOrdersByCarrierAsync(long carrierId);
        public Task<Order?> UpdateOrderAsync(Order order);
        public Task<bool> DeleteOrderAsync(long orderId);
    }
}