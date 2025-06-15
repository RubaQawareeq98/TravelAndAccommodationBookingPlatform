using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Cities.Mappers;

[Mapper]
public partial class CityResponseMapper
{
    public partial CityResponse MapCityToCityResponse(City city);
    public partial List<CityResponse>  MapCityListToCityResponseList(List<City> cities);
}
