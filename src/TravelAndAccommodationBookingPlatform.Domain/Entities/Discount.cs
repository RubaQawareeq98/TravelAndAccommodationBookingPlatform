using Microsoft.EntityFrameworkCore;

namespace TravelAndAccommodationBookingPlatform.Domain.Entities;

public class Discount : BaseEntity
{
    [Precision(10, 2)]
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomCategoryId { get; set; }
    public RoomCategory RoomCategory { get; set; }
}
