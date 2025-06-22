namespace TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Requests;

public class UpdateRoomRequest
{
    public string? RoomNumber { get; set; }
    public Guid? RoomCategoryId { get; set; }
}
