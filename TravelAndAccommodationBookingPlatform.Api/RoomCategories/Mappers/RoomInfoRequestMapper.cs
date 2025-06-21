using Riok.Mapperly.Abstractions;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.RoomCategories.Mappers;

[Mapper]
public partial class RoomCategoryRequestMapper
{
    public partial RoomCategory MapAddRoomCategoryRequestToRoomCategory(AddRoomCategoryRequest addRoomCategoryRequest);
    public partial void MapUpdateRoomCategoryRequestToRoomCategory(UpdateRoomCategoryRequest updateRoomCategoryRequest, RoomCategory roomCategory);
    public partial UpdateRoomCategoryRequest MapRoomCategoryToUpdateRoomCategoryRequest(RoomCategory roomCategory);
    
    [MapProperty(nameof(RoomSearchRequest.Filters), nameof(SieveModel.Filters))]
    [MapProperty(nameof(RoomSearchRequest.Sorts), nameof(SieveModel.Sorts))]
    [MapProperty(nameof(RoomSearchRequest.Page), nameof(SieveModel.Page))]
    [MapProperty(nameof(RoomSearchRequest.PageSize), nameof(SieveModel.PageSize))]
    public partial SieveModel MapSearchCritereaToSieveModel(RoomSearchRequest searchRequest);
}
