using TravelAndAccommodationBookingPlatform.Application.Common.Interfaces;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Services.Hashing;

public class PasswordHashingService : IPasswordHashingService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string providedPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}
