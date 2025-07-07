using Sieve.Attributes;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Owner : SoftDeleteBaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public required string Email { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public required string FirstName { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public required string LastName { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public required string PhoneNumber { get; set; }
    public virtual ICollection<Hotel> Hotels { get; set; } = [];
}
