using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

public interface IRoomInfoService
{
    Task AddRoomInfoAsync(RoomInfo roomInfo);
    Task UpdateRoomInfoAsync(RoomInfo roomInfo);
    Task DeleteRoomInfoAsync(Guid roomInfoId);
    Task<RoomInfo> GetRoomInfoByIdAsync(Guid roomInfoId);
    Task<List<RoomInfo>> GetRoomInfosAsync(SieveModel sieveModel);
}
