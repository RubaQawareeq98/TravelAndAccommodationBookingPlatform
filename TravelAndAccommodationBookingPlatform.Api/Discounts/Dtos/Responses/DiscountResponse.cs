namespace TravelAndAccommodationBookingPlatform.Api.Discounts.Dtos.Responses;

public class DiscountResponse
{
    public Guid Id { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomInfoId { get; set; }
}
