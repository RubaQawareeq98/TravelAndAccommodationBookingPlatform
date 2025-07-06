using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Mappers;

[Mapper]
public partial class BookingResponseMapper
{
    [MapProperty(nameof(Booking.PaymentDetails), nameof(BookingResponse.PaymentDetails))]
    public partial BookingResponse MapBookingToBookingResponse(Booking booking);
    
    [MapProperty(nameof(Booking.User.FirstName), nameof(BookingWithDetails.UserName))]
    public partial BookingWithDetails MapBookingWithDetailsToBookingResponse(Booking booking);

    public partial List<BookingResponse> MapBookingListToBookingResponseList(List<Booking> bookings);
}
