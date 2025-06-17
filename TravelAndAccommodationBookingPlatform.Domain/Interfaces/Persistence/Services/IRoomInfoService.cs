using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IRoomInfoService
{
    Task AddRoomInfo(RoomInfo roomInfo, List<Guid> amenitiesIds);
    Task UpdateRoomInfo(RoomInfo roomInfo);
    Task DeleteRoomInfo(Guid roomInfoId);
    Task<RoomInfo> GetRoomInfoById(Guid roomInfoId);
    Task<List<RoomInfo>> GetRoomInfos();
    Task<List<RoomInfo>> GetFilteredRooms(SieveModel sieveModel, List<Guid>? amenityIds);
}
