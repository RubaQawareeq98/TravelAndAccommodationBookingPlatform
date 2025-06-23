using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IRoomCategoryRepository
{
    Task AddRoomCategory(RoomCategory roomCategory, CancellationToken cancellationToken);
    Task UpdateRoomCategory(RoomCategory roomCategory, CancellationToken cancellationToken);
    Task<RoomCategory?> GetRoomCategoryById(Guid id, CancellationToken cancellationToken);
    Task<List<RoomCategory>> GetAllRoomCategoriesByHotelId(Guid hotelId, CancellationToken cancellationToken);
    Task DeleteRoomCategory(RoomCategory roomCategory, CancellationToken cancellationToken);
    Task<List<RoomCategory>> GetFilteredRoomCategories(SieveModel sieveModel, List<Guid>? amenityIds, CancellationToken cancellationToken);
}
