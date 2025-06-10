namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class GalleryImage : BaseEntity
{
    public Guid EntityId { get; set; }
    public string Path { get; set; } = string.Empty;
}
