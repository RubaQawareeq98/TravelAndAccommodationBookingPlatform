using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Amenities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Bookings;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Cities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Discounts;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Hotels;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Images;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Owners;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Repositories.Reviews;
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
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<IAmenityRepository, AmenityRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IGalleryImageRepository, GalleryImageRepository>();
        
        return services;
    }
}
