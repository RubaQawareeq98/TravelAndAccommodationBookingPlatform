using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Auth;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Services.Auth;

public class JwtGeneratorService(IOptions<JwtAuthOptions> options) : IJwtGeneratorService
{
    private readonly JwtAuthOptions _jwtAuthOptions = options.Value;
    
    public Task<string> GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(
            Convert.FromBase64String(_jwtAuthOptions.SecretKey));
            
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claimsForToken = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("email", user.Email),
            new("role", user.Role.ToString())
        };

        var jwt = new JwtSecurityToken(
            _jwtAuthOptions.Issuer,
            _jwtAuthOptions.Audience,
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_jwtAuthOptions.TokenExpirationMinutes),
            signingCredentials
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(jwt);
            
        return Task.FromResult(token);
    }
}
