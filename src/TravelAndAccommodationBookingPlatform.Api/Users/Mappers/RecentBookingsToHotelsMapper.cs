using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Application.Features.RecentlyVisitedHotels.Dtos;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Users.Mappers;

[Mapper]
public partial class RecentBookingsToHotelsMapper
{
    [MapProperty(nameof(Booking.HotelId), nameof(RecentlyVisitedDto.HotelId))]
    [MapProperty(nameof(Booking.Hotel.Name), nameof(RecentlyVisitedDto.HotelName))]
    [MapProperty(nameof(Booking.Hotel.ThumbnailUrl), nameof(RecentlyVisitedDto.ThumbnailUrl))]
    [MapProperty(nameof(Booking.Hotel.StarRating), nameof(RecentlyVisitedDto.StarRating))]
    [MapProperty(nameof(Booking.PaymentDetails.Amount), nameof(RecentlyVisitedDto.Price))]
    [MapProperty(nameof(Booking.PaymentDetails.PaymentMethod), nameof(RecentlyVisitedDto.PaymentMethod))]
    public partial RecentlyVisitedDto MapBookedHotelsToRecentlyVisitedHotels(Booking booking);
}
