using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Domain.Enums;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class PaymentDetail
{
    [Precision(10, 2)]
    public decimal Amount { get; set; }
    public int PaymentNumber { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public Guid BookingId { get; set; }
}
