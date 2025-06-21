using TravelAndAccommodationBookingPlatform.Api.Amenities.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Cities.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Discounts.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Images.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Owners.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Reviews.Mappers;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Mappers;
using TravelAndAccommodationBookingPlatform.Api.Users.Mappers;
using RoomCategoryRequestMapper = TravelAndAccommodationBookingPlatform.Api.RoomCategories.Mappers.RoomCategoryRequestMapper;
using RoomCategoryResponseMapper = TravelAndAccommodationBookingPlatform.Api.RoomCategories.Mappers.RoomCategoryResponseMapper;

namespace TravelAndAccommodationBookingPlatform.Api.Configurations;

public static class MappersConfiguration
{
    public static IServiceCollection AddMapperConfigurations(this IServiceCollection services)
    {
        services.AddSingleton<RegisterRequestMapper>();
        
        services.AddSingleton<CityRequestMapper>();
        services.AddSingleton<CityResponseMapper>();
        
        services.AddSingleton<HotelRequestMapper>();
        services.AddSingleton<HotelResponseMapper>();
        
        services.AddSingleton<OwnerRequestMapper>();
        services.AddSingleton<OwnerResponseMapper>();
        
        services.AddSingleton<RoomCategoryRequestMapper>();
        services.AddSingleton<RoomCategoryResponseMapper>();
        
        services.AddSingleton<RoomRequestMapper>();
        services.AddSingleton<RoomResponseMapper>();
        
        services.AddSingleton<ReviewRequestMapper>();
        services.AddSingleton<ReviewResponseMapper>();
        
        services.AddSingleton<AmenityRequestMapper>();
        services.AddSingleton<AmenityResponseMapper>();
        
        services.AddSingleton<BookingRequestMapper>();
        services.AddSingleton<BookingResponseMapper>();
        
        services.AddSingleton<DiscountRequestMapper>();
        services.AddSingleton<DiscountResponseMapper>();
        
        services.AddSingleton<GalleryImageMapper>();
        
        services.AddSingleton<RecentBookingsToHotelsMapper>();
        
        return services;
    }
}
