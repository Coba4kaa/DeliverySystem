using System.Security.Claims;
using DeliverySystemBackend.Controller.DtoMappers;
using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Controller.Validators;
using DeliverySystemBackend.Service.DomainModels;
using DeliverySystemBackend.Service.DomainServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliverySystemBackend.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController(
    IOrderService orderService,
    ICargoOwnerService cargoOwnerService,
    ICarrierService carrierService,
    IUserService userService,
    OrderValidator orderValidator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto? order)
    {
        if (order == null)
            return BadRequest("Order cannot be null.");

        var validationResult = await orderValidator.ValidateAsync(order);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return BadRequest(errors);
        }

        if (!await IsUserAuthorizedForOrderAsync(order))
            return Forbid();

        var createdOrder = await orderService.CreateOrderAsync(OrderDtoMapper.ToDomainModel(order));
        return Ok(createdOrder);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(long id)
    {
        var order = await orderService.GetOrderByIdAsync(id);

        if (order == null)
            return NotFound($"Order with ID {id} not found.");

        if (!await IsUserAuthorizedForOrderAsync(OrderDtoMapper.ToDto(order)) &&
            order.OrderStatus != OrderStatus.Created)
            return Forbid();

        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAllOrders()
    {
        if (!User.IsInRole("Admin"))
            return Forbid();

        var orders = await orderService.GetAllOrdersAsync();
        return Ok(orders.Select(OrderDtoMapper.ToDto));
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetOrdersByStatus(OrderStatus status)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        if (status == OrderStatus.Created)
        {
            var createdOrders = await orderService.GetOrdersByStatusAsync(status);
            return Ok(createdOrders.Select(OrderDtoMapper.ToDto));
        }

        var orders = await orderService.GetOrdersByStatusAsync(status);
        var authorizedOrders = new List<Order>();

        foreach (var order in orders)
            if (await IsUserAuthorizedForOrderAsync(OrderDtoMapper.ToDto(order)))
                authorizedOrders.Add(order);

        if (!authorizedOrders.Any())
            return NotFound($"No orders found with status {status} for the current user.");

        return Ok(authorizedOrders.Select(OrderDtoMapper.ToDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder([FromBody] OrderDto? order)
    {
        if (order == null)
            return BadRequest("Order cannot be null.");
        
        if (await orderService.GetOrderByIdAsync(order.Id) == null)
            return NotFound($"Order with ID {order.Id} not found.");

        var validationErrors = await ValidateOrderUpdateAsync(order);
        if (validationErrors.Any())
            return BadRequest(validationErrors);

        var updatedOrder = await orderService.UpdateOrderAsync(OrderDtoMapper.ToDomainModel(order));
        return Ok(OrderDtoMapper.ToDto(updatedOrder));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteOrder(long id)
    {
        var order = await orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound($"Order with ID {id} not found.");

        if (!await IsUserAuthorizedForOrderAsync(OrderDtoMapper.ToDto(order)))
            return Forbid();

        var success = await orderService.DeleteOrderAsync(id);
        if (!success)
            return NotFound($"Order with ID {id} not found.");

        return Ok($"Order with ID {id} deleted successfully.");
    }

    [HttpGet("cargo-owner/{cargoOwnerId:long}")]
    public async Task<ActionResult<List<OrderDto>>> GetOrdersByCargoOwner(long cargoOwnerId)
    {
        var isOwner = await IsUserAuthorizedForEntityAsync(cargoOwnerId, cargoOwnerService.GetCargoOwnerByIdAsync);

        var orders = await orderService.GetOrdersByCargoOwnerAsync(cargoOwnerId);

        if (isOwner) return Ok(orders.Select(OrderDtoMapper.ToDto));
        
        const OrderStatus allowedStatus = OrderStatus.Created;
        orders = orders.Where(o => o.OrderStatus == allowedStatus).ToList();

        return Ok(orders.Select(OrderDtoMapper.ToDto));
    }

    [HttpGet("carrier/{carrierId:long}")]
    public async Task<ActionResult<List<OrderDto>>> GetOrdersByCarrier(long carrierId)
    {
        var isOwner = await IsUserAuthorizedForEntityAsync(carrierId, carrierService.GetCarrierByIdAsync);

        var orders = await orderService.GetOrdersByCarrierAsync(carrierId);

        if (isOwner) return Ok(orders.Select(OrderDtoMapper.ToDto));
        
        const OrderStatus allowedStatus = OrderStatus.Created;
        orders = orders.Where(o => o.OrderStatus == allowedStatus).ToList();

        return Ok(orders.Select(OrderDtoMapper.ToDto));
    }

    private async Task<List<string>> ValidateOrderUpdateAsync(OrderDto order)
    {
        var errors = new List<string>();

        var validationResult = await orderValidator.ValidateAsync(order);
        if (!validationResult.IsValid)
            errors.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));

        var existingOrder = await orderService.GetOrderByIdAsync(order.Id);
        if (existingOrder == null)
        {
            errors.Add($"Order with ID {order.Id} not found.");
            return errors;
        }

        if (!await IsUserAuthorizedForOrderAsync(OrderDtoMapper.ToDto(existingOrder)) &&
            existingOrder.OrderStatus != OrderStatus.Created)
            errors.Add("User is not authorized to modify this order.");

        if (existingOrder.OrderStatus == OrderStatus.Cancelled)
            errors.Add("Cancelled orders cannot be modified.");

        if (existingOrder.OrderStatus == OrderStatus.Completed)
            errors.Add("Completed orders cannot be modified.");

        if (order.OrderStatus < existingOrder.OrderStatus)
            errors.Add("Order status cannot be downgraded.");
        
        if (existingOrder.OrderStatus == OrderStatus.Pending && order.OrderStatus == OrderStatus.Confirmed && (!order.TransportId.HasValue || !order.CargoId.HasValue))
            errors.Add("Cargo and Transport IDs must be set.");

        var userId = GetUserId();
        var actualUser = await userService.GetUserByIdAsync(userId!.Value);

        if (existingOrder.OrderStatus == OrderStatus.Confirmed || order.OrderStatus == OrderStatus.InProgress ||
            order.OrderStatus == OrderStatus.Delivered)
        {
            if (existingOrder.Distance != order.Distance || existingOrder.Price != order.Price ||
                existingOrder.CargoId != order.CargoId || existingOrder.TransportId != order.TransportId ||
                existingOrder.PlannedPickupDate != order.PlannedPickupDate ||
                existingOrder.SentDate != order.SentDate)
                errors.Add("Confirmed order cannot be modified.");
        }

        if (existingOrder.OrderStatus == OrderStatus.Confirmed && order.OrderStatus == OrderStatus.InProgress)
        {
            if (actualUser.Role != UserRole.Carrier)
                errors.Add("Only the assigned Carrier can move the order to 'InProgress'.");
        }
        else if (existingOrder.OrderStatus == OrderStatus.InProgress && order.OrderStatus == OrderStatus.Delivered)
        {
            if (actualUser.Role != UserRole.Carrier)
                errors.Add("Only the assigned Carrier can move the order to 'Delivered'.");
        }
        else if (existingOrder.OrderStatus == OrderStatus.Delivered)
        {
            if (order.IsCargoDelivered != existingOrder.IsCargoDelivered)
            {
                if (actualUser.Role != UserRole.CargoOwner)
                    errors.Add("Only the Cargo Owner can confirm the delivery.");
            }
            else if (order.ActualPickupDate != existingOrder.ActualPickupDate)
            {
                if (actualUser.Role != UserRole.Carrier)
                    errors.Add("Only the assigned Carrier can update the delivery date.");
            }
            else
            {
                errors.Add(
                    "Only the Cargo Owner can modify the delivery confirmation, and only the Carrier can update the delivery date.");
            }
        }

        if (order.IsOrderConfirmedByCargoOwner != existingOrder.IsOrderConfirmedByCargoOwner)
        {
            if (actualUser.Role != UserRole.CargoOwner)
                errors.Add("Only the Cargo Owner can update their confirmation status.");
            
            if (existingOrder.OrderStatus == OrderStatus.Pending && (!order.TransportId.HasValue || !order.CargoId.HasValue))
                errors.Add("Cargo and Transport IDs must be set.");
        }

        if (order.IsOrderConfirmedByCarrier != existingOrder.IsOrderConfirmedByCarrier)
        {
            if (actualUser.Role != UserRole.Carrier)
                errors.Add("Only the Carrier can update their confirmation status.");
            
            if (existingOrder.OrderStatus == OrderStatus.Pending && (!order.TransportId.HasValue || !order.CargoId.HasValue))
                errors.Add("Cargo and Transport IDs must be set.");
        }
        
        if (order.IsCargoDelivered != existingOrder.IsCargoDelivered)
        {
            if (actualUser.Role != UserRole.CargoOwner || existingOrder.OrderStatus != OrderStatus.Delivered)
                errors.Add("Only the Cargo Owner can confirm deliverance.");
        }

        return errors;
    }

    private async Task<bool> IsUserAuthorizedForOrderAsync(OrderDto order)
    {
        var userId = GetUserId();
        if (userId == null)
            return false;

        return order.CargoOwnerId.HasValue &&
               await IsUserAuthorizedForEntityAsync(order.CargoOwnerId.Value, cargoOwnerService.GetCargoOwnerByIdAsync)
               || order.CarrierId.HasValue &&
               await IsUserAuthorizedForEntityAsync(order.CarrierId.Value, carrierService.GetCarrierByIdAsync)
               || User.IsInRole("Admin");
    }

    private async Task<bool> IsUserAuthorizedForEntityAsync<T>(long entityId, Func<long, Task<T?>> getEntityByIdAsync)
        where T : class
    {
        if (User.IsInRole("Admin"))
            return true;

        var entity = await getEntityByIdAsync(entityId);
        switch (entity)
        {
            case CargoOwner cargoOwner when cargoOwner.UserId == GetUserId():
            case Carrier carrier when carrier.UserId == GetUserId():
                return true;
            default:
                return false;
        }
    }

    private long? GetUserId()
    {
        return long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId)
            ? userId
            : null as long?;
    }
}