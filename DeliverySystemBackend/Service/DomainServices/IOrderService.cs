using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Service.DomainServices
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Collection<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(Guid id);
    }
}