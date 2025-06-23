using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Requests;

public class AddRoomCategoryRequest
{
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomType RoomType { get; set; }
    public List<Guid>? AmenitiesIds { get; set; }
}
