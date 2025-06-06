namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class AuditableBaseEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
