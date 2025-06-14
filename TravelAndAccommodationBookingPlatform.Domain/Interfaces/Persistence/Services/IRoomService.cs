using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IRoomService
{
    Task AddRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(Guid roomId);
    Task<Room> GetRoomByIdAsync(Guid roomId);
    Task<List<Room>> GetRoomsAsync(SieveModel sieveModel);
}
