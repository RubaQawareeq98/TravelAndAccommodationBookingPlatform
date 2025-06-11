using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.Auth;

public interface IJwtGeneratorService
{
    Task<string> GenerateJwtToken(User user);
}
