using DeliverySystemBackend.Repository.Repositories.Interfaces;
using DeliverySystemBackend.Service.DomainModels;
using DeliverySystemBackend.Service.DomainServices.Interfaces;

namespace DeliverySystemBackend.Service.DomainServices;

public class OrderService(IOrderRepository orderRepository, ICarrierService carrierService, ICargoOwnerService cargoOwnerService) : IOrderService
{
    public async Task<Order> CreateOrderAsync(Order order)
    {
        await UpdateEntitiesStatusesAsync(order);
        
        return await orderRepository.CreateOrderAsync(order);
    }

    public async Task<Order?> UpdateOrderAsync(Order order)
    {
        if (order.OrderStatus is OrderStatus.Created && order.CargoOwnerId is not null && order.CarrierId is not null)
            order.OrderStatus = OrderStatus.Pending;
        if (order.OrderStatus is OrderStatus.Pending && order.IsOrderConfirmedByCarrier && order.IsOrderConfirmedByCargoOwner)
            order.OrderStatus = OrderStatus.Confirmed;
        if (order.OrderStatus is OrderStatus.Delivered && order.IsCargoDelivered)
            order.OrderStatus = OrderStatus.Completed;
        
        await UpdateEntitiesStatusesAsync(order);
        
        var updatedOrder = await orderRepository.UpdateOrderAsync(order);
        return updatedOrder;
    }

    public async Task<bool> DeleteOrderAsync(long orderId)
    {
        var order  = await orderRepository.GetOrderByIdAsync(orderId);
        var cargo = new Cargo();
        var transport = new Transport();
        
        if (order.CargoId != null)
        {
            cargo = await cargoOwnerService.GetCargoByIdAsync(order.CargoId.Value);
        }
        if (order.TransportId != null)
        {
            transport = await carrierService.GetTransportByIdAsync(order.TransportId.Value);
        }
        
        var success = await orderRepository.DeleteOrderAsync(orderId);
        if (!success) return false;
        if (cargo != null)
        {
            cargo.Status = CargoStatus.Registered;
            await cargoOwnerService.UpdateCargoAsync(cargo);
        }
        if (transport != null)
        {
            transport.Status = TransportStatus.Available;
            await carrierService.UpdateTransportAsync(transport);
        }
        
        return true;
    }

    public async Task<Order?> GetOrderByIdAsync(long orderId)
    {
        return await orderRepository.GetOrderByIdAsync(orderId);
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await orderRepository.GetAllOrdersAsync();
    }
    
    public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        var orders = await GetAllOrdersAsync();

        return orders
            .Where(o => o.OrderStatus == status)
            .ToList();
    }

    public async Task<List<Order>> GetOrdersByCargoOwnerAsync(long cargoOwnerId)
    {
        return await orderRepository.GetCargoOwnerOrdersAsync(cargoOwnerId);
    }

    public async Task<List<Order>> GetOrdersByCarrierAsync(long carrierId)
    {
        return await orderRepository.GetCarrierOrdersAsync(carrierId);
    }

    private async Task UpdateEntitiesStatusesAsync(Order order)
    {
        if (order.CargoId is not null)
        {
            var cargo = await cargoOwnerService.GetCargoByIdAsync(order.CargoId.Value);
            cargo.Status = CargoStatus.InProcess;

            if (order.OrderStatus is OrderStatus.Cancelled)
                cargo.Status = CargoStatus.Registered;
            if (order.OrderStatus is OrderStatus.Completed)
                cargo.Status = CargoStatus.Delivered;

            await cargoOwnerService.UpdateCargoAsync(cargo);
        }
        
        if (order.TransportId is not null)
        {
            var transport = await carrierService.GetTransportByIdAsync(order.TransportId.Value);
            transport.Status = TransportStatus.InUse;
            
            if (order.OrderStatus is OrderStatus.Cancelled || order.OrderStatus is OrderStatus.Completed)
                transport.Status = TransportStatus.Available;
            
            await carrierService.UpdateTransportAsync(transport);
        }
    }
}
