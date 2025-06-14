using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Requests;

public class AddRoomRequest
{
    public string RoomNumber { get; set; } = string.Empty;
    public RoomInfo RoomInfo { get; set; }
    public Guid RoomInfoId { get; set; }
}
