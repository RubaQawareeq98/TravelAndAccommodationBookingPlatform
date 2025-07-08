using TravelAndAccommodationBookingPlatform.Api.Amenities.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;

public class HotelWithRoomCategoryResponse
{
    public Guid Id { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid HotelId { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomType RoomType { get; set; }
    public List<AmenityResponse> Amenities { get; set; }
    public string HotelName { get; set; }
    public string HotelDescription{ get; set; }
    public string ThumbnailUrl{ get; set; }
    public int StarRating{ get; set; }
    public string CityName { get; set; }
    public string CountryName { get; set; }
    public string PostalCode { get; set; }
    
    
}