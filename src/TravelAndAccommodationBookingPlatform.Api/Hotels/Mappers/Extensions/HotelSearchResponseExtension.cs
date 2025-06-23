using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers.Extensions;

public static class HotelSearchResponseExtension
{
    public static HotelWithRoomCategoryResponse MapWithCity(this HotelSearchMapper mapper, RoomCategory room)
    {
        var dto = mapper.MapRoomCategoryToRoomCategoryResponse(room);

        dto.CityName = room.Hotel.City?.Name;
        dto.CountryName = room.Hotel.City?.Country;
        dto.PostalCode = room.Hotel.City?.PostalCode;

        return dto;
    }
}
