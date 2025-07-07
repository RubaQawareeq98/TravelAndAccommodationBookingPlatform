using TravelAndAccommodationBookingPlatform.Api.Bookings.PaymentDetails.Dtos;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Responses;

public class BookingResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public string? GuestRemarks { get; set; }
    public PaymentDetailsDto PaymentDetails { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public DateOnly BookingDate { get; set; }
}
