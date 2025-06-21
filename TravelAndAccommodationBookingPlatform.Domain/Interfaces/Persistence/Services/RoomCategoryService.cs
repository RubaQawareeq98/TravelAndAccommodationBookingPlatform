using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IRoomCategoryService
{
    Task AddRoomCategory(RoomCategory roomCategory, List<Guid> amenitiesIds);
    Task UpdateRoomCategory(RoomCategory roomCategory);
    Task DeleteRoomCategory(Guid roomCategoryId);
    Task<RoomCategory> GetRoomCategoryById(Guid roomCategoryId);
    Task<List<RoomCategory>> GetRoomCategories();
    Task<List<RoomCategory>> GetFilteredRooms(SieveModel sieveModel, List<Guid>? amenityIds);
}
