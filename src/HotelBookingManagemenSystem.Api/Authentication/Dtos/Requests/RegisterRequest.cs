using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos.Requests;

public class RegisterRequest
{
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Address { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole? Role { get; set; } = UserRole.User;
}
