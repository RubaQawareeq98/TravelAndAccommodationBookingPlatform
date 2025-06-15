namespace TravelAndAccommodationBookingPlatform.Api.Cities.Dtos.Responses;

public class CityResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }
    public string PostalCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } 
}
