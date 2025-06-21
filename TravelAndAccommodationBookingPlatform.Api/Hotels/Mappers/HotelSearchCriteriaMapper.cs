using Riok.Mapperly.Abstractions;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;

[Mapper]
public partial class HotelSearchCriteriaMapper
{
    [MapProperty(nameof(HotelSearchRequest.Filters), nameof(SieveModel.Filters))]
    [MapProperty(nameof(HotelSearchRequest.Sorts), nameof(SieveModel.Sorts))]
    [MapProperty(nameof(HotelSearchRequest.Page), nameof(SieveModel.Page))]
    [MapProperty(nameof(HotelSearchRequest.PageSize), nameof(SieveModel.PageSize))]
    public partial SieveModel MapSearchCritereaToSieveModel(HotelSearchRequest searchRequest);
    
    public partial List<RoomCategoryResponse>  MapRoomCategoryListToRoomCategoryResponseList(List<RoomCategory> roomCategories);
}
