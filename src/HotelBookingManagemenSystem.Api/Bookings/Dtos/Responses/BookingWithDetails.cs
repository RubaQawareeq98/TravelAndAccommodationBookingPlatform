using TravelAndAccommodationBookingPlatform.Api.Bookings.PaymentDetails.Dtos;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Responses;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Responses;

public class BookingWithDetails
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public string? GuestRemarks { get; set; }
    public PaymentDetailsDto PaymentDetail { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public DateOnly BookingDate { get; set; }
    public string UserName { get; set; }
    public string HotelName { get; set; }
    public List<RoomResponse> Rooms { get; set; }
}
