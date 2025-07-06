using TravelAndAccommodationBookingPlatform.Application.Features.RecentlyVisitedHotels.Dtos;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Mappers;

public static class RecentlyVisitedToBookingMapper
{
    public static List<Booking> MapToBookings(this List<RecentlyVisitedDto> recentlyVisited)
    {
        return recentlyVisited.Select(b => new Booking
        {
            HotelId = b.HotelId,
            Hotel = new Hotel
            {
                Name = b.HotelName,
                ThumbnailUrl = b.ThumbnailUrl,
                StarRating = b.StarRating,
                City = new City
                {
                    Name = b.CityName,
                    Country = b.CountryName,
                    PostalCode = b.PostalCode
                }
            },
            PaymentDetails = new PaymentDetails
            {
                Amount = b.Price,
                PaymentMethod = b.PaymentMethod
            },
            CheckInDate = b.CheckInDate,
            CheckOutDate = b.CheckOutDate,
        }).ToList();
    }
}
