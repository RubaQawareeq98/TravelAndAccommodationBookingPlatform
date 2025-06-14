using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Cities.Mappers;

[Mapper]
public partial class CityRequestMapper
{
    public partial City MapCityRequestToCity(AddCityRequest AddRequest);
    public partial UpdateCityRequest MapCityToUpdateCityRequest(City city);
    public partial void MapUpdateCityRequestToCity(UpdateCityRequest updateCityRequest, City city);
    
}
