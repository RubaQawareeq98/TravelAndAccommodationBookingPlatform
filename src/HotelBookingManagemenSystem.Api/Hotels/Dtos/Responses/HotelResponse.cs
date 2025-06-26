using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;

public class HotelResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int StarRating { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public int TotalRooms { get; set; }
    public HotelType HotelType { get; set; }
    public Guid CityId { get; set; }
    public Guid OwnerId { get; set; }
    public string ThumbnailUrl { get; set; }
}
