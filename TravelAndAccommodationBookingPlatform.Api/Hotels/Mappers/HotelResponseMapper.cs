using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;

[Mapper]
public partial class HotelResponseMapper
{
    public partial HotelResponse MapHotelToHotelResponse(Hotel hotel);
    public partial List<HotelResponse>  MapHotelListToHotelResponseList(List<Hotel> hotels);
}
