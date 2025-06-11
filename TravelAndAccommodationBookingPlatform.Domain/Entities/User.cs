using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class User : SoftDeleteBaseEntity
{
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Address { get; set; }
    public required string PhoneNumber { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public virtual ICollection<Booking> Bookings { get; set; } = [];
}
