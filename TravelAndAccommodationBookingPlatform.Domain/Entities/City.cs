namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class City : BaseEntity
{
    public required string Name { get; set; }
    public required string Country { get; set; }
    public string? ThumbnailUrl { get; set; }
    public required string PostalCode { get; set; }
}
