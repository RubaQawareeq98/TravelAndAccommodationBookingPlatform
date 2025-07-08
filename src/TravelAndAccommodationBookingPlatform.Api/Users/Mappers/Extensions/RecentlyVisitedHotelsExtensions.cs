using TravelAndAccommodationBookingPlatform.Application.Features.RecentlyVisitedHotels.Dtos;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Users.Mappers.Extensions;

public static class RecentlyVisitedHotelsExtensions
{
    public static RecentlyVisitedDto MapWithCity(this RecentBookingsToHotelsMapper mapper, Booking booking)
    {
        var dto = mapper.MapBookedHotelsToRecentlyVisitedHotels(booking);

        dto.CityName = booking.Hotel.City?.Name;
        dto.CountryName = booking.Hotel.City?.Country;
        dto.PostalCode = booking.Hotel.City?.PostalCode;

        return dto;
    }
}
