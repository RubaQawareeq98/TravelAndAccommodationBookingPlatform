namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Room : AuditableBaseEntity
{
    public string RoomNumber { get; set; } = string.Empty;
    public RoomDetails RoomDetails { get; set; }
    public Guid RoomDetailsId { get; set; }
}
