using Sieve.Attributes;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class RoomCategory : AuditableSoftDeleteBaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public int AdultsCapacity { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public int ChildrenCapacity { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; } = string.Empty;
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string? Description { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid HotelId { get; set; }
    
    public Hotel Hotel { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public decimal PricePerNight { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public RoomType RoomType { get; set; }
    
    public virtual ICollection<Room> Rooms { get; set; } = [];
    
    public ICollection<Amenity> Amenities { get; set; } = [];
    
    public virtual ICollection<Discount> Discounts { get; set; } = [];
    
    public virtual ICollection<GalleryImage> Gallery { get; set; } = [];
}
