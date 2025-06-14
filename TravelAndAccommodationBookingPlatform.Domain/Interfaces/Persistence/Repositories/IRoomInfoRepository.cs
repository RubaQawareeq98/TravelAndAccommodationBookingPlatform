using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;

namespace TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;

public interface IRoomInfoRepository
{
    Task AddRoomInfo(RoomInfo roomInfo);
    Task UpdateRoomInfo(RoomInfo roomInfo);
    Task<RoomInfo?> GetRoomInfo(Guid id);
    Task<List<RoomInfo>> GetAllRoomInfos(SieveModel sieveModel);
    Task DeleteRoomInfo(RoomInfo roomInfo);
}
