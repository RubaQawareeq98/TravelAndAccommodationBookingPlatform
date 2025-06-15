
namespace TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Requests;

public class AddRoomRequest
{
    public string RoomNumber { get; set; } = string.Empty;
    public Guid RoomInfoId { get; set; }
}
