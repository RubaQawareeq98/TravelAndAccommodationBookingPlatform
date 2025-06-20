using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Application.Interfaces.Persistence;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Abstraction;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.PersistenceConfigurations;

public static class AbstractionConfiguration
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
