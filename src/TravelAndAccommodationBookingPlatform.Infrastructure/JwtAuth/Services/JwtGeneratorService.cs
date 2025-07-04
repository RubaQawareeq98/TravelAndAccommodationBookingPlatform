using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TravelAndAccommodationBookingPlatform.Application.Auth.Interfaces;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Configurations;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Services;

public class JwtGeneratorService(IOptions<JwtAuthOptions> options) : IJwtGeneratorService
{
    private readonly JwtAuthOptions _jwtAuthOptions = options.Value;

    public string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(
            Convert.FromBase64String(_jwtAuthOptions.SecretKey));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var jwt = new JwtSecurityToken(
            _jwtAuthOptions.Issuer,
            _jwtAuthOptions.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(_jwtAuthOptions.TokenExpirationMinutes),
            signingCredentials
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(jwt);
            
        return token;
    }
}
