using Riok.Mapperly.Abstractions;
using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;

[Mapper]
public partial class HotelSearchMapper
{
    [MapProperty(nameof(HotelSearchRequest.Filters), nameof(SieveModel.Filters))]
    [MapProperty(nameof(HotelSearchRequest.Sorts), nameof(SieveModel.Sorts))]
    [MapProperty(nameof(HotelSearchRequest.Page), nameof(SieveModel.Page))]
    [MapProperty(nameof(HotelSearchRequest.PageSize), nameof(SieveModel.PageSize))]
    public partial SieveModel MapSearchCriteriaToSieveModel(HotelSearchRequest searchRequest);
    
    [MapProperty(nameof(RoomCategory.Hotel.StarRating), nameof(HotelWithRoomCategoryResponse.StarRating))]
    [MapProperty(nameof(RoomCategory.Hotel.ThumbnailUrl), nameof(HotelWithRoomCategoryResponse.ThumbnailUrl))]
    public partial HotelWithRoomCategoryResponse MapRoomCategoryToRoomCategoryResponse(RoomCategory roomCategories);
}
