namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Review : AuditableBaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
}
