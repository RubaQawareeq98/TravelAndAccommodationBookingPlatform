using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Application.Services.Amenities;
using TravelAndAccommodationBookingPlatform.Application.Services.Bookings;
using TravelAndAccommodationBookingPlatform.Application.Services.Cities;
using TravelAndAccommodationBookingPlatform.Application.Services.Discounts;
using TravelAndAccommodationBookingPlatform.Application.Services.Hotels;
using TravelAndAccommodationBookingPlatform.Application.Services.Images;
using TravelAndAccommodationBookingPlatform.Application.Services.Owners;
using TravelAndAccommodationBookingPlatform.Application.Services.Reviews;
using TravelAndAccommodationBookingPlatform.Application.Services.RoomCategories;
using TravelAndAccommodationBookingPlatform.Application.Services.Rooms;
using TravelAndAccommodationBookingPlatform.Application.Services.Users;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.PersistenceConfigurations;

public static class ServicesConfiguration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IOwnerService, OwnerService>();
        services.AddScoped<IRoomCategoryService, RoomCategoryService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IAmenityService, AmenityService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IGalleryImageService, GalleryImageService>();
        
        return services;
    }
}
