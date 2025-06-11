using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.Auth;

public interface IJwtGeneratorService
{
    string GenerateJwtToken(User user);
}
