using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.Bookings.Dtos;

public class BookingValidationResult
{
    public Hotel Hotel { get; set; }
    public User User { get; set; }
    public List<Room> Rooms { get; set; }
}
