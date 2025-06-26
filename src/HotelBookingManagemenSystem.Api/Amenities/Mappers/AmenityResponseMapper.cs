using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Amenities.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Amenities.Mappers;

[Mapper]
public partial class AmenityResponseMapper
{
    public partial AmenityResponse MapAmenityToAmenityResponse(Amenity amenity);
    public partial List<AmenityResponse>  MapAmenityListToAmenityResponseList(List<Amenity> amenities);
}
