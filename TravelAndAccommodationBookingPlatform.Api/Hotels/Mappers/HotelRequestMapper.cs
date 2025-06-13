using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Mappers;

[Mapper]
public partial class HotelRequestMapper
{
    public partial Hotel MapHotelRequestToHotel(AddHotelRequest request);
}
