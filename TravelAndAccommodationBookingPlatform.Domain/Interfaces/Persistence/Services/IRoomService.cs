using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IRoomService
{
    Task AddRoom(Room room);
    Task UpdateRoom(Room room);
    Task DeleteRoom(Guid roomId);
    Task<Room> GetRoomById(Guid roomId);
    Task<List<Room>> GetRooms(SieveModel sieveModel);
    Task<List<Room>> GetRoomsByIds(List<Guid> roomIds);
}
