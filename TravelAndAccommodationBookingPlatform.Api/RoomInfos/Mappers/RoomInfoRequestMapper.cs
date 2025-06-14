using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.RoomInfos.Mappers;

[Mapper]
public partial class RoomInfoRequestMapper
{
    public partial RoomInfo MapAddRoomInfoRequestToRoomInfo(AddRoomInfoRequest addRoomInfoRequest);
    public partial void MapAddRoomInfoRequestToRoomInfo(UpdateRoomInfoRequest updateRoomInfoRequest, RoomInfo roomInfo);
    public partial UpdateRoomInfoRequest MapRoomInfoToUpdateRoomInfoRequest(RoomInfo roomInfo);
}
