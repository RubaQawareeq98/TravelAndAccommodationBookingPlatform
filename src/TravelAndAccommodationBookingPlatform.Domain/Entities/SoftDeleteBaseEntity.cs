namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class SoftDeleteBaseEntity : BaseEntity
{
    public bool IsDeleted { get; set; }
}
