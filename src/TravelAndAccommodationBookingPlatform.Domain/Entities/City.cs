using Sieve.Attributes;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class City : AuditableSoftDeleteBaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public required string Name { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public required string Country { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public required string PostalCode { get; set; }
    
    public string? ThumbnailUrl { get; set; }
    
    public virtual ICollection<Hotel> Hotels { get; set; } = [];
}
