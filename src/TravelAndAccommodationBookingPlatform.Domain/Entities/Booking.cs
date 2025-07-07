using Sieve.Attributes;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid HotelId { get; set; }
    public Hotel Hotel { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string? GuestRemarks { get; set; }
    
    public PaymentDetails PaymentDetails { get; set; } = new();
    
    [Sieve(CanFilter = true, CanSort = true)]
    public DateOnly CheckInDate { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public DateOnly CheckOutDate { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime BookingDate { get; set; }
    public ICollection<Room> Rooms { get; set; } = [];
}
