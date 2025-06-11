using Riok.Mapperly.Abstractions;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos;
using TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos.Requests;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Api.Authentication.Mappers;

[Mapper]
public partial class RegisterRequestMapper
{
    public partial User MapRegisterRequestToUser(RegisterRequest registerRequest);
}
