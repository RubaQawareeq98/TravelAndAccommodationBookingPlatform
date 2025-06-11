using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Authentication.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(IUserService userService) : ControllerBase
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
}