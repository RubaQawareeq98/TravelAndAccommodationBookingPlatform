using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Room : AuditableSoftDeleteBaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string RoomNumber { get; set; } = string.Empty;
    
    public RoomCategory RoomCategory { get; set; }
    
    public Guid RoomCategoryId { get; set; }
    
    public ICollection<Booking> Bookings { get; set; } = [];
    
    public virtual ICollection<GalleryImage> Gallery { get; set; } = [];

    [Timestamp]
    public byte[] RowVersion { get; set; }
}
