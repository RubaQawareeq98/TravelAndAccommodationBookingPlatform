using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Mappers;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Authentication.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(IUserService userService, RegisterRequestMapper requestMapper) : ControllerBase
{
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await userService.GetUserByCredentialsAsync(request.Email, request.Password);
        if (user is null)
        {
            return Unauthorized();
        }
        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            var user = requestMapper.MapRegisterRequestToUser(request);
            await userService.AddUserAsync(user);
            return Ok("User created successfully");
        }
        catch (EmailAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
