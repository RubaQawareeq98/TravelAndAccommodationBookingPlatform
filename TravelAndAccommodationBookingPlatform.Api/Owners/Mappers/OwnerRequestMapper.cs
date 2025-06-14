using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Owners.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Owners.Mappers;

[Mapper]
public partial class OwnerRequestMapper
{
    public partial Owner MapAddOwnerRequestToOwner(AddOwnerRequest addOwnerRequest);
    public partial void MapAddOwnerRequestToOwner(UpdateOwnerRequest updateOwnerRequest, Owner owner);
    public partial UpdateOwnerRequest MapOwnerToUpdateOwnerRequest(Owner owner);
}
