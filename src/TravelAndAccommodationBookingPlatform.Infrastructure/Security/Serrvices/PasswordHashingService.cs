using TravelAndAccommodationBookingPlatform.Application.Security;
using TravelAndAccommodationBookingPlatform.Application.Security.Interfaces;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Security.Serrvices;

public class PasswordHashingService : IPasswordHashingService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool IsPasswordVerified(string providedPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}
