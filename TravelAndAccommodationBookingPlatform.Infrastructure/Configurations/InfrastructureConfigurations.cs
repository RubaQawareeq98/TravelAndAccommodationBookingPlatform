using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.PersistenceConfigurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Services.Hashing;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;

public static class InfrastructureConfigurations
{
    public static IServiceCollection AddInfrastructureConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddPersistenceRepositories()
            .AddPersistenceServices()
            .AddHashingConfiguration()
            .AddPersistenceDbContexts(configuration);
    }
}
