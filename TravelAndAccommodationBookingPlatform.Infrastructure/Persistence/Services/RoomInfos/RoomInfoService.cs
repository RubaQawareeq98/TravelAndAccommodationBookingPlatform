using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.RoomInfos;

public class RoomInfoService(IRoomInfoRepository roomInfoRepository,
    IAmenityService amenityService,
    IHotelService hotelService) : IRoomInfoService
{
    public async Task AddRoomInfo(RoomInfo roomInfo, List<Guid> amenitiesIds)
    {
        foreach (var id in amenitiesIds)
        {
            var amenity = await amenityService.GetAmenityById(id);
            if (amenity is null)
            {
                throw new NotFoundException($"No amenity with id {id} could be found.");
            }
         //   roomInfo.Amenities.Add(amenity);
        }
        
        var isHotelExist = await hotelService.IsHotelExist(roomInfo.HotelId);
        if (!isHotelExist)
        {
            throw new NotFoundException($"No hotel with id {roomInfo.HotelId} could be found.");
        }
        
        await roomInfoRepository.AddRoomInfo(roomInfo);
    }

    public async Task UpdateRoomInfo(RoomInfo roomInfo)
    {
        await roomInfoRepository.UpdateRoomInfo(roomInfo);
    }

    public async Task DeleteRoomInfo(Guid roomInfoId)
    {
        var roomInfo = await GetRoomInfoById(roomInfoId);
        await roomInfoRepository.DeleteRoomInfo(roomInfo);
    }

    public async Task<RoomInfo> GetRoomInfoById(Guid roomInfoId)
    {
        var roomInfo = await roomInfoRepository.GetRoomInfo(roomInfoId);
        if (roomInfo is null)
        {
            throw new NotFoundException($"RoomInfo with if {roomInfoId} not found");
        }
        
        return roomInfo;
    }

    public async Task<List<RoomInfo>> GetRoomInfos()
    {
        return await roomInfoRepository.GetAllRoomInfos();
    }

    public async Task<List<RoomInfo>> GetFilteredRooms(SieveModel sieveModel, List<Guid>? amenityIds)
    {
        return await roomInfoRepository.GetFilteredRoomInfos(sieveModel, amenityIds);
    }
}
