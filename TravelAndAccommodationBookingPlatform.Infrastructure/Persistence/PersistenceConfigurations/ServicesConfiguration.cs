using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Amenities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Bookings;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Cities;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Discounts;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Hotels;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Owners;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Reviews;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.RoomInfos;
using TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Rooms;
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
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IAmenityService, AmenityService>();
        services.AddScoped<IBookingService, BookingService>();
        
        return services;
    }
}
