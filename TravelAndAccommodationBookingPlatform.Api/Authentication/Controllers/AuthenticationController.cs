using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Mappers;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Auth;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Api.Authentication.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(IUserService userService,
    IJwtGeneratorService jwtGeneratorService,
    RegisterRequestMapper requestMapper) : ControllerBase
{
    /// <summary>
    /// Authenticate user by email & password Credentials
    /// </summary>
    /// <param name="loginRequest">Login request credentials.</param>
    /// <response code="200">When valid credentials provided, return generated token with user Id.</response>
    /// <response code="400">List of errors when invalid credentials provided.</response>
    /// <response code="401">When user with provided credentials not exist.</response>
    /// <returns>Generated Jwt token when valid user credentials & the user exist.</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var user = await userService.GetUserByCredentialsAsync(loginRequest.Email, loginRequest.Password);
        if (user is null)
        {
            return Unauthorized();
        }
        var token = jwtGeneratorService.GenerateJwtToken(user);
        return Ok(new LoginResponse
        {
            Token = token,
            UserId = user.Id
        });
    }

    /// <summary>
    /// Register new user with valid data
    /// </summary>
    /// <param name="registerRequest">Register request data.</param>
    /// <response code="200">If the user registration was successful.</response>
    /// <response code="400">If the register data are invalid.</response>
    /// <response code="409">If a user with the same email is already registered.</response>
    /// <returns>Successful created message</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var user = requestMapper.MapRegisterRequestToUser(registerRequest);
        await userService.AddUserAsync(user);
        return Ok("User created successfully");
    }
}
