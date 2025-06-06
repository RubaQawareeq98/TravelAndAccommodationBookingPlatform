namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Amenity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid RoomId { get; set; }
    public Room Room { get; set; }
}
