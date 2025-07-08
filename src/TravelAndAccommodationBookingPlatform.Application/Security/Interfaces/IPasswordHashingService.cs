namespace TravelAndAccommodationBookingPlatform.Application.Security.Interfaces;

public interface IPasswordHashingService
{
    string HashPassword(string password);
    bool IsPasswordVerified(string providedPassword, string hashedPassword);
}
