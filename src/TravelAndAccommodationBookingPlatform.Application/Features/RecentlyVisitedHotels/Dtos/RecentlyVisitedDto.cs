using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Application.Features.RecentlyVisitedHotels.Dtos;

public class RecentlyVisitedDto
{
    public Guid HotelId { get; set; }
    public string HotelName { get; set; }
    public string ThumbnailUrl { get; set; }
    public int StarRating { get; set; }
    public string CityName { get; set; }
    public string CountryName { get; set; }
    public string PostalCode { get; set; }
    public decimal Price { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
