using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IRoomCategoryService
{
    Task<Result<RoomCategory>> AddRoomCategory(Guid hotelId, RoomCategory roomCategory, List<Guid> amenitiesIds,
        CancellationToken cancellationToken = default);
    Task<Result> UpdateRoomCategory(Guid hotelId, RoomCategory roomCategory, CancellationToken cancellationToken = default);
    Task<Result> DeleteRoomCategory(Guid hotelId, Guid roomCategoryId, CancellationToken cancellationToken = default);
    Task<Result<RoomCategory>> GetRoomCategoryById(Guid hotelId, Guid roomCategoryId,
        CancellationToken cancellationToken = default);
    Task<Result<List<RoomCategory>>> GetRoomCategories(Guid hotelId, SieveModel sieveModel,
        CancellationToken cancellationToken = default);
}
