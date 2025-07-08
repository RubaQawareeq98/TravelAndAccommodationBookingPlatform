using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            var key = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

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
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };
                    
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("Authentication failed: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("Token validated");
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddScoped<IJwtGeneratorService, JwtGeneratorService>();
        }
}
