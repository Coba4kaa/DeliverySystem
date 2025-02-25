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
public class CarrierController(
    ICarrierService carrierService,
    CarrierValidator carrierValidator,
    TransportValidator transportValidator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RegisterCarrier([FromBody] CarrierDto? carrier)
    {
        if (carrier is null) return BadRequest("Carrier cannot be null.");

        var validationResult = await carrierValidator.ValidateAsync(carrier);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var createdCarrier = await carrierService.RegisterCarrierAsync(CarrierDtoMapper.ToDomainModel(carrier));

        return CreatedAtAction(nameof(GetCarrierById), new { id = createdCarrier.Id }, CarrierDtoMapper.ToDto(createdCarrier));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCarrierById(long id)
    {
        var carrier = await carrierService.GetCarrierByIdAsync(id);
        if (carrier is null)
            return NotFound($"Carrier with ID {id} not found.");

        return Ok(CarrierDtoMapper.ToDto(carrier));
    }
    
    [HttpGet("user/{userId:long}")]
    public async Task<IActionResult> GetCarrierByUserId(long userId)
    {
        var carrier = await carrierService.GetCarrierByUserIdAsync(userId);
        if (carrier is null)
            return NotFound($"Carrier with userId {userId} not found.");

        return Ok(CarrierDtoMapper.ToDto(carrier));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllCarriers()
    {
        var carriers = await carrierService.GetAllCarriersAsync();
        return Ok(carriers.Select(CarrierDtoMapper.ToDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCarrier([FromBody] CarrierDto? carrier)
    {
        if (carrier is null) return BadRequest("Carrier cannot be null.");

        if (!await IsUserAuthorizedForEntityAsync(carrier.Id))
            return Forbid();

        var validationResult = await carrierValidator.ValidateAsync(carrier);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var updatedCarrier = await carrierService.UpdateCarrierAsync(CarrierDtoMapper.ToDomainModel(carrier));
        return Ok(CarrierDtoMapper.ToDto(updatedCarrier));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteCarrier(long id)
    {
        if (!await IsUserAuthorizedForEntityAsync(id))
            return Forbid();

        var result = await carrierService.DeleteCarrierAsync(id);
        if (!result) return NotFound($"Carrier with ID {id} not found.");

        return NoContent();
    }

    [HttpPost("transport")]
    public async Task<IActionResult> AddTransport([FromBody] TransportDto? transport)
    {
        if (transport is null) return BadRequest("Transport cannot be null.");

        var carrier = await carrierService.GetCarrierByIdAsync(transport.CarrierId);
        if (carrier == null || !await IsUserAuthorizedForEntityAsync(carrier.Id))
            return Forbid();

        var validationResult = await transportValidator.ValidateAsync(transport);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var addedTransport = await carrierService.AddTransportAsync(TransportDtoMapper.ToDomainModel(transport));
        return CreatedAtAction(nameof(GetTransportById), new { id = addedTransport.Id }, TransportDtoMapper.ToDto(addedTransport));
    }

    [HttpGet("transport/{id:long}")]
    public async Task<IActionResult> GetTransportById(long id)
    {
        var transport = await carrierService.GetTransportByIdAsync(id);
        return transport is null 
            ? NotFound($"Transport with ID {id} not found.") 
            : Ok(TransportDtoMapper.ToDto(transport));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("transports")]
    public async Task<IActionResult> GetAllTransports()
    {
        var transports = await carrierService.GetAllTransportsAsync();
        return Ok(transports.Select(TransportDtoMapper.ToDto));
    }

    [HttpGet("transport/{carrierId:long}/status/{status}")]
    public async Task<IActionResult> GetTransportsByStatus(long carrierId, TransportStatus status)
    {
        var carrier = await carrierService.GetCarrierByIdAsync(carrierId);
        if (carrier == null || (!User.IsInRole("Admin") && carrier.UserId != GetUserId()))
            return Forbid();

        var transports = await carrierService.GetTransportsByStatusAsync(carrierId, status);
        return Ok(transports.Select(TransportDtoMapper.ToDto));
    }
    
    [HttpGet("{carrierId:long}/transport")]
    public async Task<IActionResult> GetTransportsByCarrierId(long carrierId)
    {
        var transports = await carrierService.GetTransportsByCarrierIdAsync(carrierId);
        return Ok(transports.Select(TransportDtoMapper.ToDto).ToList());
    }

    [HttpPut("transport")]
    public async Task<IActionResult> UpdateTransport([FromBody] TransportDto transport)
    {
        var existingTransport = await carrierService.GetTransportByIdAsync(transport.Id);
        if (existingTransport is null)
            return NotFound($"Transport with ID {transport.Id} not found.");

        var carrier = await carrierService.GetCarrierByIdAsync(existingTransport.CarrierId);
        if (carrier == null || !await IsUserAuthorizedForEntityAsync(carrier.Id))
            return Forbid();

        var validationResult = await transportValidator.ValidateAsync(transport);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var updatedTransport = await carrierService.UpdateTransportAsync(TransportDtoMapper.ToDomainModel(transport));
        return Ok(TransportDtoMapper.ToDto(updatedTransport));
    }

    [HttpDelete("transport/{id:long}")]
    public async Task<IActionResult> DeleteTransport(long id)
    {
        var transport = await carrierService.GetTransportByIdAsync(id);
        if (transport == null)
            return NotFound($"Transport with ID {id} not found.");

        var carrier = await carrierService.GetCarrierByIdAsync(transport.CarrierId);
        if (carrier == null || !await IsUserAuthorizedForEntityAsync(carrier.Id))
            return Forbid();

        await carrierService.DeleteTransportAsync(id);
        return NoContent();
    }

    private async Task<bool> IsUserAuthorizedForEntityAsync(long entityId)
    {
        if (User.IsInRole("Admin")) return true;

        var carrier = await carrierService.GetCarrierByIdAsync(entityId);
        
        return carrier is not null && carrier.UserId == GetUserId();
    }

    private long? GetUserId()
    {
        return long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) ? userId : null;
    }
}
