using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.Auth.Interfaces;

public interface IJwtGeneratorService
{
    string GenerateJwtToken(User user);
}
