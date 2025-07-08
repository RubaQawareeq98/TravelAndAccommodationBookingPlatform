using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Discount : BaseEntity
{
    [Precision(10, 2)]
    [Sieve(CanFilter = true, CanSort = true)]
    public decimal DiscountPercentage { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime StartDate { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime EndDate { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid RoomCategoryId { get; set; }
    
    public RoomCategory RoomCategory { get; set; }
}
