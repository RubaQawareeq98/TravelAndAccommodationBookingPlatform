using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Owners.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Owners.Mappers;

[Mapper]
public partial class AddOwnerRequestMapper
{
    public partial Owner MapAddOwnerRequestToOwner(AddOwnerRequest addOwnerRequest);
}
