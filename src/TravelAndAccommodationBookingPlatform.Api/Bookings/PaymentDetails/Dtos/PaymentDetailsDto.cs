using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Api.Bookings.PaymentDetails.Dtos;

public class PaymentDetailsDto
{
    public decimal Amount { get; set; }
    public int PaymentNumber { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
