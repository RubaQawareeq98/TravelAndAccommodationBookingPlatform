using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Security;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Security.Configurations;

public static class HashingConfiguration
{
    public static IServiceCollection AddHashingConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHashingService, PasswordHashingService>();
        return services;
    }
}
