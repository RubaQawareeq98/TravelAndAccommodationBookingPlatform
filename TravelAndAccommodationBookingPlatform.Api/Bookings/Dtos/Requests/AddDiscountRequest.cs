namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Dtos.Requests;

public class AddDiscountRequest
{
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomInfoId { get; set; }
}
