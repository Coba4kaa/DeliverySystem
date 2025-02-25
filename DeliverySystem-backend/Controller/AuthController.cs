using DeliverySystemBackend.Service.DomainServices.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DeliverySystemBackend.Controller.DtoMappers;
using DeliverySystemBackend.Controller.DtoModels;

namespace DeliverySystemBackend.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await userService.GetUserByLoginAsync(loginDto.Login);

        try
        {
            if (user == null || !userService.VerifyPassword(loginDto.Password, user.PasswordHash))
                return Unauthorized("Invalid login or password.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);

        return Ok(UserDtoMapper.ToDto(user));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return Ok("Logged out successfully.");
    }
}