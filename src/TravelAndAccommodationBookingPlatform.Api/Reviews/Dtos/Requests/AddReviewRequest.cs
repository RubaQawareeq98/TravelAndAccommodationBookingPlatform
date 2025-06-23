
namespace TravelAndAccommodationBookingPlatform.Api.Reviews.Dtos.Requests;

public class AddReviewRequest
{
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
}
