using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IRoomRepository
{
    Task AddRoom(Room room);
    Task UpdateRoom(Room room);
    Task<Room?> GetRoom(Guid id);
    Task<List<Room>> GetAllRooms(SieveModel sieveModel);
    Task DeleteRoom(Room room);
    Task<List<Room>> GetRoomsByRoomsIds(List<Guid> roomIds);
}
