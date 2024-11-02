using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace DeliverySystemBackend.Repository
{
    public class OrderRepository(DeliveryContext context) : IOrderRepository
    {
        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.PickupDate = order.PickupDate.ToUniversalTime();
            var orderDbModel = OrderMapper.ToDbModel(order);

            context.Orders.Add(orderDbModel);
            await context.SaveChangesAsync();

            return OrderMapper.ToDomainModel(orderDbModel);
        }

        public async Task<Collection<Order>> GetAllOrdersAsync()
        {
            var orderDbModels = await context.Orders.ToListAsync();
            var orders = new Collection<Order>();

            foreach (var orderDbModel in orderDbModels)
                orders.Add(OrderMapper.ToDomainModel(orderDbModel));

            return orders;
        }


        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            var orderDbModel = await context.Orders.FindAsync(id);

            if (orderDbModel == null)
                return null;

            return OrderMapper.ToDomainModel(orderDbModel);
        }

    }
}