using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Owners.Dtos.Responses;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Owners.Mappers;

[Mapper]
public partial class OwnerResponseMapper
{
    public partial OwnerResponse MapOwnerToOwnerResponse(Owner owner);
    public partial List<OwnerResponse>  MapOwnerListToOwnerResponseList(List<Owner> owners);
}
