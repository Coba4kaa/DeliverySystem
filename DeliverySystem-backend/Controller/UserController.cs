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
public class UserController(IUserService userService, UserValidator validator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserDto user)
    {
        var validationResult = await validator.ValidateAsync(user);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var newUser = await userService.RegisterUserAsync(UserDtoMapper.ToDomainModel(user));
        return CreatedAtAction(nameof(GetUserById), new { userId = newUser.Id }, UserDtoMapper.ToDto(newUser));
    }

    [HttpGet("{userId:long}")]
    public async Task<IActionResult> GetUserById(long userId)
    {
        if (!User.IsInRole("Admin") && GetUserId() != userId)
            return Forbid();

        var user = await userService.GetUserByIdAsync(userId);
        if (user is null)
            return NotFound(new { message = $"User with ID {userId} not found." });

        return Ok(UserDtoMapper.ToDto(user));
    }

    [HttpGet("by-login/{login}")]
    public async Task<IActionResult> GetUserByLogin(string login)
    {
        var user = await userService.GetUserByLoginAsync(login);
        if (user is null)
            return NotFound(new { message = $"User with login '{login}' not found." });

        if (!User.IsInRole("Admin") && GetUserId() != user.Id)
            return Forbid();

        return Ok(UserDtoMapper.ToDto(user));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        if (!User.IsInRole("Admin"))
            return Forbid();

        var users = await userService.GetAllUsersAsync();
        return Ok(users.Select(UserDtoMapper.ToDto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserDto user)
    {
        if (!User.IsInRole("Admin") && GetUserId() != user.Id)
            return Forbid();

        var validationResult = await validator.ValidateAsync(user);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var userToUpdate = await userService.GetUserByIdAsync(user.Id);
        if (userToUpdate is null)
            return NotFound(new { message = $"User with ID '{user.Id}' not found." });

        var updatedUser = await userService.UpdateUserAsync(UserDtoMapper.ToDomainModel(user));
        return Ok(UserDtoMapper.ToDto(updatedUser));
    }

    [HttpDelete("{userId:long}")]
    public async Task<IActionResult> DeleteUser(long userId)
    {
        if (!User.IsInRole("Admin") && GetUserId() != userId)
            return Forbid();

        var success = await userService.DeleteUserAsync(userId);
        if (!success)
            return NotFound(new { message = $"User with ID {userId} not found." });

        return NoContent();
    }

    private long? GetUserId()
    {
        return long.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var userId)
            ? userId
            : null as long?;
    }
}