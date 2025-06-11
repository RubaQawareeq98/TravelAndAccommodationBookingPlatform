using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Auth;
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
            var key = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();
        }
}
