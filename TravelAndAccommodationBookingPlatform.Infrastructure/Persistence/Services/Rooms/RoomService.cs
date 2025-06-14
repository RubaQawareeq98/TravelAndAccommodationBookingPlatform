using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Rooms;

public class RoomService(IRoomRepository roomRepository) : IRoomService
{
    public async Task AddRoomAsync(Room room)
    {
        await roomRepository.AddRoom(room);
    }

    public async Task UpdateRoomAsync(Room room)
    {
        await roomRepository.UpdateRoom(room);
    }

    public async Task DeleteRoomAsync(Guid roomId)
    {
        var room = await GetRoomByIdAsync(roomId);
        await roomRepository.DeleteRoom(room);
    }

    public async Task<Room> GetRoomByIdAsync(Guid roomId)
    {
        var room = await roomRepository.GetRoom(roomId);
        if (room is null)
        {
            throw new NotFoundException($"Room with if {roomId} not found");
        }
        
        return room;
    }

    public async Task<List<Room>> GetRoomsAsync(SieveModel sieveModel)
    {
        return await roomRepository.GetAllRooms(sieveModel);
    }
}
