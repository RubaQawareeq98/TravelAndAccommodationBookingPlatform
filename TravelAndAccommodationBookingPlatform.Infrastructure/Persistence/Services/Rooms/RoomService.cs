using Sieve.Models;
using TravelAndAccommodationBookingPlatform.Domain.Entities;
using TravelAndAccommodationBookingPlatform.Domain.EntitiesErrors;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Repositories;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Persistence.Services;
using TravelAndAccommodationBookingPlatform.Domain.Shared.Results;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Persistence.Services.Rooms;

public class RoomService(IRoomRepository roomRepository, IRoomCategoryService roomCategoryService) : IRoomService
{
    public async Task<Result<Room>> AddRoom(Room room, Guid hotelId, Guid roomCategoryId, CancellationToken cancellationToken)
    {
        var roomCategoryResult = await roomCategoryService.GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken);
        if (roomCategoryResult.IsFailure)
        {
            return Result<Room>.Failure(RoomCategoryError.RoomCategoryNotFound(roomCategoryId));
        }
        
        var existRoom = await roomRepository.GetRoomByNumber(room.RoomNumber, roomCategoryId, cancellationToken);
        if (existRoom is not null)
        {
            return Result<Room>.Failure(RoomError.RoomNumberAlreadyExist(room.RoomNumber));
        }
        
        room.RoomCategoryId = roomCategoryId;
        await roomRepository.AddRoom(room);
        return Result<Room>.Success(room);
    }

    public async Task UpdateRoom(Room room)
    {
        await roomRepository.UpdateRoom(room);
    }
    
    public async Task<Result<Room>> DeleteRoom(Guid hotelId, Guid roomCategoryId, Guid roomId, CancellationToken cancellationToken)
    {
        var result = await GetRoomById(hotelId, roomCategoryId, roomId, cancellationToken);
        if (result.IsFailure)
        {
             return Result<Room>.Failure(RoomError.RoomNotFound(roomId));
        }
        
        var room = result.Value;
        await roomRepository.DeleteRoom(room, cancellationToken);
        return Result<Room>.Success(room);
    }
    
    public async Task<List<Room>> GetRoomsByIds(List<Guid> roomIds)
    {
        return await roomRepository.GetRoomsByRoomsIds(roomIds);
    }

    public async Task<Result<List<Room>>> GetRooms(Guid hotelId, Guid roomCategoryId, SieveModel sieveModel, CancellationToken cancellationToken)
    {
        var roomCategoryResult = await roomCategoryService.GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken);
        if (roomCategoryResult.IsFailure)
        {
            return Result<List<Room>>.Failure(RoomCategoryError.RoomCategoryNotFound(roomCategoryId));
        }

        var rooms = await roomRepository.GetRoomsByRoomCategory(roomCategoryId, sieveModel, cancellationToken);
        
        return Result<List<Room>>.Success(rooms);
    }

    public async Task<Result<Room>> GetRoomById(Guid hotelId, Guid roomCategoryId, Guid roomId,
        CancellationToken cancellationToken)
    {
        var roomCategoryResult = await roomCategoryService.GetRoomCategoryById(hotelId, roomCategoryId, cancellationToken);
        if (roomCategoryResult.IsFailure)
        {
            return Result<Room>.Failure(RoomCategoryError.RoomCategoryNotFound(roomCategoryId));
        }
        
        var room = await roomRepository.GetRoom(roomId, cancellationToken);
        if (room is null)
        {
            return Result<Room>.Failure(RoomError.RoomNotFound(roomId));
        }

        return room.RoomCategoryId != roomCategoryResult.Value.Id ? Result<Room>.Failure(RoomError.RoomNotFound(roomId)) : Result<Room>.Success(room);
    }
}
