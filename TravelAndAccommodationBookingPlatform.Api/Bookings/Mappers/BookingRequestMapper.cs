using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Rooms.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.Mappers;

[Mapper]
public partial class BookingRequestMapper
{
    public partial Room MapAddRoomRequestToRoom(AddRoomRequest addRoomRequest);
    public partial void MapUpdateRoomRequestToRoom(UpdateRoomRequest updateRoomRequest, Room room);
    public partial UpdateRoomRequest MapRoomToUpdateRoomRequest(Room room);
}
