using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class PaymentDetail
{
    [Precision(10, 2)]
    public decimal Amount { get; set; }
    public string PaymentNumber { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public Guid BookingId { get; set; }
}
