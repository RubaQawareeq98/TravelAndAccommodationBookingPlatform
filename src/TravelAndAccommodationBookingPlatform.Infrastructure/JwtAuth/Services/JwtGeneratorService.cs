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

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
                issuer: _jwtAuthOptions.Issuer,   
                audience: _jwtAuthOptions.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_jwtAuthOptions.TokenExpirationMinutes),
                signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
