using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class RoomInfo : AuditableSoftDeleteBaseEntity
{
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomType RoomType { get; set; }
    public virtual ICollection<Room> Rooms { get; set; } = [];
    public virtual ICollection<Amenity> Amenities { get; set; } = [];
    public virtual ICollection<Discount> Discounts { get; set; } = [];
    public virtual ICollection<GalleryImage> Gallery { get; set; } = [];
}
