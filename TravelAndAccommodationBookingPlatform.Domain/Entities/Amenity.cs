using Sieve.Attributes;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Amenity : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; } = string.Empty;
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Description { get; set; } = string.Empty;
    
    public virtual ICollection<RoomInfo> RoomInfos { get; set; } =[];
}
