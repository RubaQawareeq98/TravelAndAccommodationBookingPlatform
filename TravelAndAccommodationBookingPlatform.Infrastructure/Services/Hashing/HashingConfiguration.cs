using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Application.Common.Interfaces;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Services.Hashing;

public static class HashingConfiguration
{
    public static IServiceCollection AddHashingConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHashingService, PasswordHashingService>();
        return services;
    }
}
