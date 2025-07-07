using Sieve.Attributes;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Review : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid HotelId { get; set; }
    
    public Hotel Hotel { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Content { get; set; } = string.Empty;
    
    [Sieve(CanFilter = true, CanSort = true)]
    public int Rating { get; set; }
}
