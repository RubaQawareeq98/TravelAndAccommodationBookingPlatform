namespace TravelAndAccommodationBookingPlatform.Api.Authentication.Dtos.Responses;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
