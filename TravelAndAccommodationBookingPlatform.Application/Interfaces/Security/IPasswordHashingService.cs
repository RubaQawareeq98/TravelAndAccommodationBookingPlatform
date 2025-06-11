namespace TravelAndAccommodationBookingPlatform.Application.Interfaces.Security;

public interface IPasswordHashingService
{
    string HashPassword(string password);
    bool IsPasswordVerified(string providedPassword, string hashedPassword);
}
