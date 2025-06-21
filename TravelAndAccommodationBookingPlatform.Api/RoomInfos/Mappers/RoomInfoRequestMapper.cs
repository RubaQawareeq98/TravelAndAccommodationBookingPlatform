using Riok.Mapperly.Abstractions;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.RoomInfos.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.RoomInfos.Mappers;

[Mapper]
public partial class RoomInfoRequestMapper
{
    public partial RoomInfo MapAddRoomInfoRequestToRoomInfo(AddRoomInfoRequest addRoomInfoRequest);
    public partial void MapUpdateRoomInfoRequestToRoomInfo(UpdateRoomInfoRequest updateRoomInfoRequest, RoomInfo roomInfo);
    public partial UpdateRoomInfoRequest MapRoomInfoToUpdateRoomInfoRequest(RoomInfo roomInfo);
    
    [MapProperty(nameof(RoomSearchRequest.Filters), nameof(SieveModel.Filters))]
    [MapProperty(nameof(RoomSearchRequest.Sorts), nameof(SieveModel.Sorts))]
    [MapProperty(nameof(RoomSearchRequest.Page), nameof(SieveModel.Page))]
    [MapProperty(nameof(RoomSearchRequest.PageSize), nameof(SieveModel.PageSize))]
    public partial SieveModel MapSearchCritereaToSieveModel(RoomSearchRequest searchRequest);
}
