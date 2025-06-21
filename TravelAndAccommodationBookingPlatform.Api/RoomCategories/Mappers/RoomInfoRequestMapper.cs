using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.RoomCategories.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.RoomCategories.Mappers;

[Mapper]
public partial class RoomCategoryRequestMapper
{
    public partial RoomCategory MapAddRoomCategoryRequestToRoomCategory(AddRoomCategoryRequest addRoomCategoryRequest);
    public partial void MapUpdateRoomCategoryRequestToRoomCategory(UpdateRoomCategoryRequest updateRoomCategoryRequest, RoomCategory roomCategory);
    public partial UpdateRoomCategoryRequest MapRoomCategoryToUpdateRoomCategoryRequest(RoomCategory roomCategory);
}
