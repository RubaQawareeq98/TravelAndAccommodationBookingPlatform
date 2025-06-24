using TravelAndAccommodationBookingPlatform.Api.Amenities.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Responses;

public class RoomCategoryResponse
{
    public Guid Id { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid HotelId { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomType RoomType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<AmenityResponse> Amenities { get; set; }
}
