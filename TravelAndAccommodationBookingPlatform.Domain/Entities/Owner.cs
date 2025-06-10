namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Owner : SoftDeleteBaseEntity
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public virtual ICollection<Hotel> Hotels { get; set; } = [];
}
