using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Amenities.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Amenities.Mappers;

[Mapper]
public partial class AmenityRequestMapper
{
    public partial Amenity MapAddAmenityRequestToAmenity(AddAmenityRequest addAmenityRequest);
    public partial void MapUpdateAmenityRequestToAmenity(UpdateAmenityRequest updateAmenityRequest, Amenity amenity);
    public partial UpdateAmenityRequest MapAmenityToUpdateAmenityRequest(Amenity amenity);
}
