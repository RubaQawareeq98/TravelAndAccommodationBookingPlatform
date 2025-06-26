namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;

public class UpdateBookingRequest
{
    public Guid? UserId { get; set; }
    public Guid? HotelId { get; set; }
    public string? GuestRemarks { get; set; }
    public DateOnly? CheckInDate { get; set; }
    public DateOnly? CheckOutDate { get; set; }
}
