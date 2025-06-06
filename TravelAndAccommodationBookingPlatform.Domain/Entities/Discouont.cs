using Microsoft.EntityFrameworkCore;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Discount : BaseEntity
{
    [Precision(10, 2)]
    public decimal DiscountPercentage { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Guid RoomDetailsId { get; set; }
    public RoomInfo RoomInfo { get; set; }
}
