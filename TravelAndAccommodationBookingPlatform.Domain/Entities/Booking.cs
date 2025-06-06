namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Booking : AuditableBaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; }
    public PaymentDetail PaymentDetail { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public DateOnly BookingDate { get; set; }
}
