using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class RoomDetails : AuditableBaseEntity
{
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomType RoomType { get; set; }
}
