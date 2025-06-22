using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IRoomService
{
    Task UpdateRoom(Room room);
    Task<Result<Room>> DeleteRoom(Guid hotelId, Guid roomCategoryId, Guid roomId, CancellationToken cancellationToken);
    Task<List<Room>> GetRoomsByIds(List<Guid> roomIds);
    Task<Result<List<Room>>> GetRooms(Guid hotelId, Guid roomCategoryId, SieveModel sieveModel, CancellationToken cancellationToken);
    Task<Result<Room>> GetRoomById(Guid hotelId, Guid roomCategoryId, Guid roomId, CancellationToken cancellationToken);
    Task<Result<Room>> AddRoom(Room room, Guid hotelId, Guid roomCategoryId, CancellationToken cancellationToken);
}
