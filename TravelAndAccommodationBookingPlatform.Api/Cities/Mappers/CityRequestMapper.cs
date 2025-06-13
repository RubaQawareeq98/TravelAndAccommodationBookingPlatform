using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Cities.Mappers;

[Mapper]
public partial class CityRequestMapper
{
    public partial City MapCityRequestToCity(AddCityRequest request);
}
