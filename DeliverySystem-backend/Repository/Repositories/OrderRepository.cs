using DeliverySystemBackend.Repository.DbMappers;
using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace DeliverySystemBackend.Repository.Repositories;

public class OrderRepository(DeliveryDbContext context, ILogger<OrderRepository> logger) : IOrderRepository
{
    public async Task<Order> CreateOrderAsync(Order order)
    {
        try
        {
            var orderDbModel = OrderDbMapper.ToDbModel(order);

            context.Orders.Add(orderDbModel);
            await context.SaveChangesAsync();

            return OrderDbMapper.ToDomainModel(orderDbModel);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while creating a new order.");
            throw new Exception($"{ex.InnerException.Message}An error occurred while creating the order. Please try again later.");
        }
    }
    
    public async Task<Order?> GetOrderByIdAsync(long id)
    {
        try
        {
            var orderDbModel = await context.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orderDbModel != null)
            {
                return OrderDbMapper.ToDomainModel(orderDbModel);
            }
        
            logger.LogWarning("Order with ID {OrderId} not found.", id);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving the order with ID {OrderId}.", id);
            throw new Exception($"An error occurred while retrieving the order with ID {id}. Please try again later.");
        }
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        try
        {
            var orders = new List<Order>((
                    await context.Orders
                        .ToListAsync())
                .ConvertAll(OrderDbMapper.ToDomainModel)
            );

            return orders;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving all orders.");
            throw new Exception("An error occurred while retrieving the orders. Please try again later.");
        }
    }

    public async Task<Order?> UpdateOrderAsync(Order order)
    {
        try
        {
            var existingOrder = await context.Orders.FindAsync(order.Id);

            if (existingOrder == null)
            {
                logger.LogWarning("Order with ID {OrderId} not found for update.", order.Id);
                return null;
            }

            var updatedOrder = OrderDbMapper.ToDbModel(order);
            context.Entry(existingOrder).CurrentValues.SetValues(updatedOrder);

            await context.SaveChangesAsync();
            return OrderDbMapper.ToDomainModel(updatedOrder);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error occurred while updating the order with ID {OrderId}.", order.Id);
            throw new Exception(
                $"An error occurred while updating the order with ID {order.Id}. Please try again later.");
        }
    }

    public async Task<bool> DeleteOrderAsync(long id)
    {
        try
        {
            var order = await context.Orders.FindAsync(id);

            if (order == null)
            {
                logger.LogWarning("Order with ID {OrderId} not found for deletion.", id);
                return false;
            }

            context.Orders.Remove(order);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting the order with ID {OrderId}.", id);
            throw new Exception($"An error occurred while deleting the order with ID {id}. Please try again later.");
        }
    }
    
    public async Task<List<Order>> GetCargoOwnerOrdersAsync(long cargoOwnerId)
    {
        try
        {
            var orders = new List<Order>((
                    await context.Orders
                        .Where(o => o.CargoOwnerId == cargoOwnerId)
                        .ToListAsync())
                .ConvertAll(OrderDbMapper.ToDomainModel)
            );

            return orders;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving orders for cargo owner with ID {CargoOwnerId}.",
                cargoOwnerId);
            throw new Exception(
                $"{ex.InnerException?.Message ?? ex.Message} An error occurred while retrieving the orders for cargo owner with ID {cargoOwnerId}. Please try again later.");
        }
    }
    
    public async Task<List<Order>> GetCarrierOrdersAsync(long carrierId)
    {
        try
        {
            var orders = new List<Order>((
                    await context.Orders
                        .Where(o => o.CarrierId == carrierId)
                        .ToListAsync())
                .ConvertAll(OrderDbMapper.ToDomainModel)
            );

            return orders;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving orders for carrier with ID {CarrierId}.", carrierId);
            throw new Exception(
                $"{ex.InnerException?.Message ?? ex.Message} An error occurred while retrieving the orders for carrier with ID {carrierId}. Please try again later.");
        }
    }
}