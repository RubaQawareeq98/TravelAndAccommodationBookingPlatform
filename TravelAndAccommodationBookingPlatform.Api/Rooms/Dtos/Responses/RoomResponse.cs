using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Responses;

public class RoomResponse
{
    public Guid Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public Guid RoomCategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
