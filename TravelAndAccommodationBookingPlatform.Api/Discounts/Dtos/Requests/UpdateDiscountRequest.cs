namespace TravelAndAccommodationBookingPlatform.Api.Discounts.Dtos.Requests;

public class UpdateDiscountRequest
{
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomCategoryId { get; set; }
}
