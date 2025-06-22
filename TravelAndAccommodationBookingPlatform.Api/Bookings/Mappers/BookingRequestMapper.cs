using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Mappers;

[Mapper]
public partial class BookingRequestMapper
{
    [MapProperty(nameof(addBookingRequest.PaymentMethod), nameof(Booking.PaymentDetail.PaymentMethod))]
    public partial Booking MapAddBookingRequestToBooking(AddBookingRequest addBookingRequest);
    public partial void MapUpdateBookingRequestToBooking(UpdateBookingRequest updateBookingRequest, Booking booking);
    public partial UpdateBookingRequest MapBookingToUpdateBookingRequest(Booking booking);
}
