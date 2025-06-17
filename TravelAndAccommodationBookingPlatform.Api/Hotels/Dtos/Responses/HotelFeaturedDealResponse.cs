
namespace TravelAndAccommodationBookingPlatform.Api.Hotels.Dtos.Responses;

public class HotelFeaturedDealResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string PhoneNumber { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int StarRating { get; set; }
    public int TotalRooms { get; set; }
    public string HotelType { get; set; }
    public string? CityName { get; set; }
    public string? CountryName { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public DateTime DiscountStartDate { get; set; }
    public DateTime DiscountEndDate { get; set; }
}
