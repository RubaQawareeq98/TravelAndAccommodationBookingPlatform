using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Application.Rooms.Interfaces;

public interface IRoomAvailabilityValidator
{
    Result ValidateRoomAvailability(Booking booking, List<Room> rooms);
}
