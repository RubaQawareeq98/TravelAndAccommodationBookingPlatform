using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Rooms.Mappers;

[Mapper]
public partial class RoomResponseMapper
{
    public partial RoomResponse MapRoomToRoomResponse(Room room);
    public partial List<RoomResponse>  MapRoomListToRoomResponseList(List<Room> rooms);
}
