using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Users;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.PersistenceConfigurations;

public static class RepositoriesConfiguration
{
    public static IServiceCollection AddPersistenceRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHotelRepository, IHotelRepository>();
        
        return services;
    }
}
