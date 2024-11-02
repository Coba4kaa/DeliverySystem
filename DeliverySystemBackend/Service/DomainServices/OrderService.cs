using System.Collections.ObjectModel;
using DeliverySystemBackend.Repository;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Service.DomainServices
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        public Task<Order> CreateOrderAsync(Order order)
        {
            return orderRepository.CreateOrderAsync(order);
        }

        public Task<Collection<Order>> GetAllOrdersAsync()
        {
            return orderRepository.GetAllOrdersAsync();
        }

        public Task<Order> GetOrderByIdAsync(Guid id)
        {
            return orderRepository.GetOrderByIdAsync(id);
        }
    }
}