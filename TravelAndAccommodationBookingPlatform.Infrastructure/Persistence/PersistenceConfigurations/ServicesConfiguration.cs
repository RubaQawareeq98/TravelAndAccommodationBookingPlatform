using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Cities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Hotels;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Owners;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.RoomInfos;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Users;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.PersistenceConfigurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IOwnerService, OwnerService>();
        services.AddScoped<IRoomInfoService, RoomInfoService>();
        
        return services;
    }
}
