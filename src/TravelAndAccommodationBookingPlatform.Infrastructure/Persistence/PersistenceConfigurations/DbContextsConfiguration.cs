using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.DbContexts;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.PersistenceConfigurations;

public static class DbContextsConfiguration
{
    public static IServiceCollection AddPersistenceDbContexts(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<HotelBookingManagementDbContext>(options => 
            options.UseSqlServer(
                    configuration.GetConnectionString("SqlConnectionString"),
                    sqlServerOptions => 
                    {
                        sqlServerOptions.EnableRetryOnFailure(
                            maxRetryCount: 5, 
                            maxRetryDelay: TimeSpan.FromSeconds(30), 
                            errorNumbersToAdd: null);
                    })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging());
        
        return services;
    }
}
