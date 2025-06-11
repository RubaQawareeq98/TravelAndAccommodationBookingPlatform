namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.Security;

public interface IPasswordHashingService
{
    string HashPassword(string password);
    bool VerifyPassword(string providedPassword, string hashedPassword);
}
