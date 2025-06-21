using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IRoomCategoryRepository
{
    Task AddRoomCategory(RoomCategory roomCategory);
    Task UpdateRoomCategory(RoomCategory roomCategory);
    Task<RoomCategory?> GetRoomCategory(Guid id);
    Task<List<RoomCategory>> GetAllRoomCategories();
    Task DeleteRoomCategory(RoomCategory roomCategory);
    Task<List<RoomCategory>> GetFilteredRoomCategories(SieveModel sieveModel, List<Guid>? amenityIds);
}
