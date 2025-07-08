using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IRoomRepository
{
    Task AddRoom(Room room);
    Task UpdateRoom(Room room);
    Task<Room?> GetRoom(Guid id, CancellationToken cancellationToken);
    Task DeleteRoom(Room room, CancellationToken cancellationToken);
    Task<List<Room>> GetRoomsByRoomsIds(List<Guid> roomIds);
    Task<Room?> GetRoomByNumber(string roomNumber, Guid roomCategoryId, CancellationToken cancellationToken);
    Task<List<Room>> GetRoomsByRoomCategory(Guid roomCategoryId, SieveModel sieveModel, CancellationToken cancellationToken);
}
