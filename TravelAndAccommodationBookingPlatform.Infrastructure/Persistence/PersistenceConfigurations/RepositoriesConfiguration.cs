using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Cities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Hotels;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Owners;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.RoomInfos;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Rooms;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Users;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.PersistenceConfigurations;

public static class RepositoriesConfiguration
{
    public static IServiceCollection AddPersistenceRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IOwnerRepository, OwnerRepository>();
        services.AddScoped<IRoomInfoRepository, RoomInfoRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        
        return services;
    }
}
