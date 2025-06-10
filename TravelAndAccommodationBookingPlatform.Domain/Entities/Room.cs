namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Room : AuditableSoftDeleteBaseEntity
{
    public string RoomNumber { get; set; } = string.Empty;
    public RoomInfo RoomInfo { get; set; }
    public Guid RoomInfoId { get; set; }
    public ICollection<Booking> Bookings { get; set; } = [];
}
