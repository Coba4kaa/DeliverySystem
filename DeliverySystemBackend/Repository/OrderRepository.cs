using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeliverySystemBackend.Repository
{
    public class OrderRepository(DeliveryContext context, ILogger<OrderRepository> logger) : IOrderRepository
    {
        public async Task<Order> CreateOrderAsync(Order order)
        {
            try
            {
                order.PickupDate = order.PickupDate.ToUniversalTime();
                var orderDbModel = OrderMapper.ToDbModel(order);

                context.Orders.Add(orderDbModel);
                await context.SaveChangesAsync();

                return OrderMapper.ToDomainModel(orderDbModel);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Error occurred while creating a new order.");
                throw new Exception("An error occurred while creating the order. Please try again later.");
            }
        }

        public async Task<Collection<Order>> GetAllOrdersAsync()
        {
            try
            {
                var orders = new Collection<Order>(
                    (await context.Orders.ToListAsync())
                    .ConvertAll(OrderMapper.ToDomainModel)
                );

                return orders;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all orders.");
                throw new Exception("An error occurred while retrieving the orders. Please try again later.");
            }
        }


        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            try
            {
                var orderDbModel = await context.Orders.FindAsync(id);

                if (orderDbModel == null)
                {
                    logger.LogWarning("Order with ID {OrderId} not found.", id);
                    return null;
                }

                return OrderMapper.ToDomainModel(orderDbModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving the order with ID {OrderId}.", id);
                throw new Exception($"An error occurred while retrieving the order with ID {id}. Please try again later.");
            }
        }
    }
}
