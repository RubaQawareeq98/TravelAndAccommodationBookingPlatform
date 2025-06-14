using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;

public class AddBookingRequest
{
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public string? GuestRemarks { get; set; }
    public PaymentDetail PaymentDetail { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public DateOnly BookingDate { get; set; }
}
