using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.RoomCategories.Mappers;

[Mapper]
public partial class RoomCategoryResponseMapper
{
    public partial RoomCategoryResponse MapRoomCategoryToRoomCategoryResponse(RoomCategory roomCategory);
    public partial List<RoomCategoryResponse>  MapRoomCategoryListToRoomCategoryResponseList(List<RoomCategory> roomCategories);
}
