namespace TravelAndAccommodationBookingPlatform.Api.Reviews.Dtos.Requests;

public class UpdateReviewRequest
{
    public Guid? UserId { get; set; }
    public Guid? HotelId { get; set; }
    public string? Content { get; set; }
    public int? Rating { get; set; }
}
