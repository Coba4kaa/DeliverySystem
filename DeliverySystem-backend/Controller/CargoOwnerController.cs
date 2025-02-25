using System.Security.Claims;
using DeliverySystemBackend.Controller.DtoMappers;
using DeliverySystemBackend.Controller.DtoModels;
using DeliverySystemBackend.Controller.Validators;
using DeliverySystemBackend.Service.DomainServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliverySystemBackend.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CargoOwnerController(
    ICargoOwnerService cargoOwnerService,
    CargoOwnerValidator cargoOwnerValidator,
    CargoValidator cargoValidator) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterCargoOwner([FromBody] CargoOwnerDto? cargoOwner)
    {
        if (cargoOwner is null) return BadRequest("CargoOwner cannot be null.");

        var validationResult = await cargoOwnerValidator.ValidateAsync(cargoOwner);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        
        var createdCargoOwner = await cargoOwnerService.RegisterCargoOwnerAsync(CargoOwnerDtoMapper.ToDomainModel(cargoOwner));
        return CreatedAtAction(nameof(GetCargoOwnerById), new { id = createdCargoOwner.Id }, CargoOwnerDtoMapper.ToDto(createdCargoOwner));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCargoOwnerById(long id)
    {
        var cargoOwner = await cargoOwnerService.GetCargoOwnerByIdAsync(id);
        if (cargoOwner is null) return NotFound($"CargoOwner with ID {id} not found.");

        return Ok(CargoOwnerDtoMapper.ToDto(cargoOwner));
    }
    
    [HttpGet("user/{userId:long}")]
    public async Task<IActionResult> GetCargoOwnerByUserId(long userId)
    {
        var cargoOwner = await cargoOwnerService.GetCargoOwnerByUserIdAsync(userId);
        if (cargoOwner is null)
            return NotFound($"CargoOwner with userId {userId} not found.");

        return Ok(CargoOwnerDtoMapper.ToDto(cargoOwner));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllCargoOwners()
    {
        var cargoOwners = await cargoOwnerService.GetAllCargoOwnersAsync();
        return Ok(cargoOwners.Select(CargoOwnerDtoMapper.ToDto).ToList());
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCargoOwner([FromBody] CargoOwnerDto? cargoOwner)
    {
        if (cargoOwner is null) return BadRequest("CargoOwner cannot be null.");

        if (!await IsUserAuthorizedForEntityAsync(cargoOwner.Id))
            return Forbid();

        var validationResult = await cargoOwnerValidator.ValidateAsync(cargoOwner);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var updatedCargoOwner = await cargoOwnerService.UpdateCargoOwnerAsync(CargoOwnerDtoMapper.ToDomainModel(cargoOwner));
        return Ok(CargoOwnerDtoMapper.ToDto(updatedCargoOwner));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteCargoOwner(long id)
    {
        if (!await IsUserAuthorizedForEntityAsync(id))
            return Forbid();

        var success = await cargoOwnerService.DeleteCargoOwnerAsync(id);
        if (!success) return NotFound($"CargoOwner with ID {id} not found.");

        return NoContent();
    }

    [HttpPost("cargo")]
    public async Task<IActionResult> AddCargo([FromBody] CargoDto? cargo)
    {
        if (cargo is null) return BadRequest("Cargo cannot be null.");
        
        if (!await IsUserAuthorizedForEntityAsync(cargo.CargoOwnerId))
            return Forbid();
        
        var validationResult = await cargoValidator.ValidateAsync(cargo);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var createdCargo = await cargoOwnerService.AddCargoAsync(CargoDtoMapper.ToDomainModel(cargo));
        return CreatedAtAction(nameof(GetCargoById), new { cargoId = createdCargo.Id }, CargoDtoMapper.ToDto(createdCargo));
    }

    [HttpGet("{cargoOwnerId:long}/cargo")]
    public async Task<IActionResult> GetCargoOwnerCargos(long cargoOwnerId)
    {
        var cargos = await cargoOwnerService.GetCargoOwnerCargosAsync(cargoOwnerId);
        return Ok(cargos.Select(CargoDtoMapper.ToDto).ToList());
    }

    [HttpGet("cargo/{cargoId:long}")]
    public async Task<IActionResult> GetCargoById(long cargoId)
    {
        var cargo = await cargoOwnerService.GetCargoByIdAsync(cargoId);
        if (cargo is null) return NotFound($"Cargo with ID {cargoId} not found.");

        return Ok(CargoDtoMapper.ToDto(cargo));
    }

    [HttpGet("cargos")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllCargos()
    {
        var cargos = await cargoOwnerService.GetAllCargosAsync();
        return Ok(cargos.Select(CargoDtoMapper.ToDto).ToList());
    }

    [HttpPut("cargo")]
    public async Task<IActionResult> UpdateCargo([FromBody] CargoDto? cargo)
    {
        if (cargo is null) return BadRequest("Cargo cannot be null.");

        if (!await IsUserAuthorizedForEntityAsync(cargo.CargoOwnerId))
            return Forbid();

        var validationResult = await cargoValidator.ValidateAsync(cargo);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var updatedCargo = await cargoOwnerService.UpdateCargoAsync(CargoDtoMapper.ToDomainModel(cargo));
        return Ok(CargoDtoMapper.ToDto(updatedCargo));
    }

    [HttpDelete("cargo/{cargoId:long}")]
    public async Task<IActionResult> DeleteCargo(long cargoId)
    {
        var cargo = await cargoOwnerService.GetCargoByIdAsync(cargoId);
        if (cargo is null) return NotFound($"Cargo with ID {cargoId} not found.");

        if (!await IsUserAuthorizedForEntityAsync(cargo.CargoOwnerId))
            return Forbid();

        var success = await cargoOwnerService.DeleteCargoAsync(cargoId);
        if (!success) return NotFound($"Cargo with ID {cargoId} not found.");

        return NoContent();
    }

    private async Task<bool> IsUserAuthorizedForEntityAsync(long entityId)
    {
        if (User.IsInRole("Admin")) return true;

        var cargoOwner = await cargoOwnerService.GetCargoOwnerByIdAsync(entityId);
        
        return cargoOwner is not null && cargoOwner.UserId == GetUserId();
    }

    private long? GetUserId()
    {
        return long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) ? userId : null;
    }
}
