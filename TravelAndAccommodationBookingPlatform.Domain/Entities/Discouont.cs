using Microsoft.EntityFrameworkCore;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Discount : BaseEntity
{
    [Precision(10, 2)]
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomInfoId { get; set; }
    public RoomInfo RoomInfo { get; set; }
}
