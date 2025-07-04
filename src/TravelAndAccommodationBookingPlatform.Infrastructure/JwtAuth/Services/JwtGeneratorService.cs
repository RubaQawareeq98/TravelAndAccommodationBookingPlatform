using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
            Encoding.UTF8.GetBytes(_jwtAuthOptions.SecretKey));
            
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
            
        return token;
    }
}
