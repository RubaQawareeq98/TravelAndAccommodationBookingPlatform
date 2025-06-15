using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.RoomInfos.Mappers;

[Mapper]
public partial class RoomInfoResponseMapper
{
    public partial RoomInfoResponse MapRoomInfoToRoomInfoResponse(RoomInfo roomInfo);
    public partial List<RoomInfoResponse>  MapRoomInfoListToRoomInfoResponseList(List<RoomInfo> roomInfos);
}
