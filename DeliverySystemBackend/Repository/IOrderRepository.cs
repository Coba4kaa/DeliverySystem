using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository;

using System.Threading.Tasks;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Collection<Order>> GetAllOrdersAsync();
    Task<Order> GetOrderByIdAsync(Guid id);
}