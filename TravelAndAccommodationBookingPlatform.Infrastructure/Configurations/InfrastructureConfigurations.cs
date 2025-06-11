using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Infrastructure.JwtAuth.Configurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.PersistenceConfigurations;
using TravelAndAccommodationBookingPlatform.Infrastructure.Security.Configurations;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Configurations;

public static class InfrastructureConfigurations
{
    public static IServiceCollection AddInfrastructureConfigurations(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        return builder.AddJwtParams()
            .AddPersistenceRepositories()
            .AddPersistenceServices()
            .AddHashingConfiguration()
            .AddPersistenceDbContexts(configuration);
    }
}
