using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.Exceptions;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Rooms;

public class RoomService(IRoomRepository roomRepository) : IRoomService
{
    public async Task AddRoom(Room room)
    {
        await roomRepository.AddRoom(room);
    }

    public async Task UpdateRoom(Room room)
    {
        await roomRepository.UpdateRoom(room);
    }

    public async Task DeleteRoom(Guid roomId)
    {
        var room = await GetRoomById(roomId);
        await roomRepository.DeleteRoom(room);
    }

    public async Task<Room> GetRoomById(Guid roomId)
    {
        var room = await roomRepository.GetRoom(roomId);
        if (room is null)
        {
            throw new NotFoundException($"Room with id {roomId} not found");
        }
        
        return room;
    }

    public async Task<List<Room>> GetRooms(SieveModel sieveModel)
    {
        return await roomRepository.GetAllRooms(sieveModel);
    }

    public async Task<List<Room>> GetRoomsByIds(List<Guid> roomIds)
    {
        return await roomRepository.GetRoomsByRoomsIds(roomIds);
    }
}
