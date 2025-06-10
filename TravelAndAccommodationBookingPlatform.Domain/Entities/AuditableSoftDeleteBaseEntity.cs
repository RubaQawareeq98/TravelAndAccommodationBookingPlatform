namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class AuditableSoftDeleteBaseEntity : SoftDeleteBaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
