namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class City : AuditableSoftDeleteBaseEntity
{
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string PostalCode { get; set; }
    public string? ThumbnailUrl { get; set; }
    public virtual ICollection<Hotel> Hotels { get; set; } = [];
}
