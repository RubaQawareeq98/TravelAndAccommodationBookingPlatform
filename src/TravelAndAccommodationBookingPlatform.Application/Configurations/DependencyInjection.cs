using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Application.Bookings.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Bookings.Validators;
using TravelAndAccommodationBookingPlatform.Application.Rooms.Interfaces;
using TravelAndAccommodationBookingPlatform.Application.Rooms.Validators;

namespace TravelAndAccommodationBookingPlatform.Application.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookingValidator, BookingValidator>();
        services.AddScoped<IRoomAvailabilityValidator, RoomAvailabilityValidator>();
        
        return services;
    }
}
