using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.RoomInfos;

public class RoomInfoService(IRoomInfoRepository roomInfoRepository) : IRoomInfoService
{
    public async Task AddRoomInfoAsync(RoomInfo roomInfo)
    {
        await roomInfoRepository.AddRoomInfo(roomInfo);
    }

    public async Task UpdateRoomInfoAsync(RoomInfo roomInfo)
    {
        await roomInfoRepository.UpdateRoomInfo(roomInfo);
    }

    public async Task DeleteRoomInfoAsync(Guid roomInfoId)
    {
        var roomInfo = await GetRoomInfoByIdAsync(roomInfoId);
        await roomInfoRepository.DeleteRoomInfo(roomInfo);
    }

    public async Task<RoomInfo> GetRoomInfoByIdAsync(Guid roomInfoId)
    {
        var roomInfo = await roomInfoRepository.GetRoomInfo(roomInfoId);
        if (roomInfo is null)
        {
            throw new NotFoundException($"RoomInfo with if {roomInfoId} not found");
        }
        
        return roomInfo;
    }

    public async Task<List<RoomInfo>> GetRoomInfosAsync(SieveModel sieveModel)
    {
        return await roomInfoRepository.GetAllRoomInfos(sieveModel);
    }
}
