using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Hotels;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Users;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.PersistenceConfigurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHotelService, HotelService>();
        
        return services;
    }
}
