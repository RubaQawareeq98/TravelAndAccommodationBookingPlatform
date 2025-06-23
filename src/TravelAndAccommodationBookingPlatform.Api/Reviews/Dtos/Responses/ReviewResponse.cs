namespace TravelAndAccommodationBookingPlatform.Api.Reviews.Dtos.Responses;

public class ReviewResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
}
