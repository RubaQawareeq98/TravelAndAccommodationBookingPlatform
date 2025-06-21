using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IRoomCategoryService
{
    Task<Result> AddRoomCategory(RoomCategory roomCategory, List<Guid> amenitiesIds,
        CancellationToken cancellationToken = default);
    Task UpdateRoomCategory(RoomCategory roomCategory, CancellationToken cancellationToken = default);
    Task<Result<RoomCategory>> DeleteRoomCategory(Guid roomCategoryId, CancellationToken cancellationToken = default);
    Task<Result<RoomCategory>> GetRoomCategoryById(Guid roomCategoryId, CancellationToken cancellationToken = default);
    Task<List<RoomCategory>> GetRoomCategories(CancellationToken cancellationToken = default);
    Task<List<RoomCategory>> GetFilteredRooms(SieveModel sieveModel, List<Guid>? amenityIds, CancellationToken cancellationToken);
}
