using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Hotel : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Email { get; set; }
    public required string PhoneNumber { get; set; }
    public int StarRating { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int TotalRooms { get; set; }
    public HotelType HotelType { get; set; }
    public City? City { get; set; }
    public Guid CityId { get; set; }
    public Owner? Owner { get; set; }
    public Guid OwnerId { get; set; }
}
