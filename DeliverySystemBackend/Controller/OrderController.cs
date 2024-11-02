using System.Collections.ObjectModel;
using DeliverySystemBackend.Service.DomainModels;
using DeliverySystemBackend.Service.DomainServices;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace DeliverySystemBackend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IOrderService orderService, IValidator<Order> validator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order is null)
                return BadRequest("Order cannot be null.");

            ValidationResult validationResult = await validator.ValidateAsync(order);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            await orderService.CreateOrderAsync(order);
            return Ok("Order created successfully.");
        }

        [HttpGet]
        public async Task<ActionResult<Collection<Order>>> GetAllOrders()
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(string id)
        {
            if (!Guid.TryParse(id, out var orderId))
                return BadRequest($"The value '{id}' is not a valid GUID.");
    
            var order = await orderService.GetOrderByIdAsync(orderId);

            if (order is null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(order);
        }
    }
}