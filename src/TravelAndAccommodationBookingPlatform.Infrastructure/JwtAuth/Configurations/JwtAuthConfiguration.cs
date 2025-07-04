using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TravelAndAccommodationBookingPlatform.Application.Auth.Interfaces;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Configurations;

public static class JwtAuthConfiguration
{

    public static IServiceCollection AddJwtParams(this WebApplicationBuilder builder)
    {
        var jwtSection = builder.Configuration.GetSection("JwtAuthentication");

        builder.Services.Configure<JwtAuthOptions>(jwtSection);

        var jwtOptions = jwtSection.Get<JwtAuthOptions>();
        ArgumentNullException.ThrowIfNull(jwtOptions);
        
       builder.Services.AddJwtAuthentication(jwtOptions);
        
        return builder.Services;
    }

    private static void AddJwtAuthentication(this IServiceCollection services, JwtAuthOptions jwtOptions)
        {
             services.AddAuthentication("Bearer")
                                .AddJwtBearer("Bearer", options =>
                               {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Convert.FromBase64String(jwtOptions.SecretKey)
                        )
                    };
                });

            services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();
        }
}
