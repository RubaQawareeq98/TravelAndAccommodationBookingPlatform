using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Application.Security;
using TravelAndAccommodationBookingPlatform.Application.Security.Interfaces;
using TravelAndAccommodationBookingPlatform.Infrastructure.Security.Serrvices;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Security.Configurations;

public static class HashingConfiguration
{
    public static IServiceCollection AddHashingConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHashingService, PasswordHashingService>();
        return services;
    }
}
