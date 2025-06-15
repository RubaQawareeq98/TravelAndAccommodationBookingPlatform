using Sieve.Attributes;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Hotel : AuditableSoftDeleteBaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public required string Name { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public required string Description { get; set; }
    
    public string? ThumbnailUrl { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string? Email { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public required string PhoneNumber { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public int StarRating { get; set; }
    
    [Sieve(CanFilter = true, CanSort = false)]
    public double Longitude { get; set; }
    
    [Sieve(CanFilter = true, CanSort = false)]
    public double Latitude { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public int TotalRooms { get; set; }
    
    [Sieve(CanFilter = true, CanSort = false)]
    public HotelType HotelType { get; set; }
    
    public City? City { get; set; }
    public Guid CityId { get; set; }
    public Owner? Owner { get; set; }
    public Guid OwnerId { get; set; }
    public virtual ICollection<RoomInfo> RoomInfos { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
    public virtual ICollection<GalleryImage> Gallery { get; set; } = [];
    public virtual ICollection<Booking> Bookings { get; set; } = [];
}
